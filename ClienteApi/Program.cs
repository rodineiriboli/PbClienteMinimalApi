using ClienteApi.Data;
using ClienteApi.Models;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddDbContext<ClienteContextDb>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

#region GetClienteList
app.MapGet("/todosClientes", async (
    ClienteContextDb context) =>
    await context.Clientes.ToListAsync())
    .Produces<Cliente>(StatusCodes.Status200OK)
    .Produces(StatusCodes.Status404NotFound)
    .WithName("GetCliente")
    .WithTags("Cliente");
#endregion

#region GetClienteByEmail
app.MapGet("/clientePorEmail{email}", async (
    string email,
    ClienteContextDb context) =>
    await context.Clientes.AsNoTracking<Cliente>().FirstOrDefaultAsync(c => c.Email == email)
    is Cliente cliente
        ? Results.Ok(cliente)
        : Results.NotFound())
    .Produces<Cliente>(StatusCodes.Status200OK)
    .Produces(StatusCodes.Status404NotFound)
    .WithName("GetClienteByEmail")
    .WithTags("Cliente");
#endregion

#region PostCliente
app.MapPost("/adicionaCliente", async (
    ClienteContextDb context,
    Cliente cliente) =>
    {
        if (string.IsNullOrEmpty(cliente.Nome) || string.IsNullOrEmpty(cliente.Email))
            return Results.BadRequest("Todos os campos devem ser preenchidos");

        cliente.Id = new Guid();

        context.Clientes.Add(cliente);
        var result = await context.SaveChangesAsync();

        return result > 0
        ? Results.CreatedAtRoute("GetCliente", new { email = cliente.Email }, cliente)
        : Results.BadRequest("Erro ao salvar registro");
    })
    .Produces<Cliente>(StatusCodes.Status201Created)
    .Produces(StatusCodes.Status400BadRequest)
    .WithName("PostCliente")
    .WithTags("Cliente");
#endregion

#region PutCliente
app.MapPut("/atualizaCliente{id}", async (
    Guid id,
    ClienteContextDb context,
    Cliente cliente) =>
    {
        var clienteAtual = await context.Clientes.AsNoTracking<Cliente>().FirstOrDefaultAsync(c => c.Id == id);
        if (clienteAtual == null)
            return Results.NotFound();
        cliente.Id = clienteAtual.Id;
        clienteAtual = cliente;

        if (string.IsNullOrEmpty(cliente.Nome) || string.IsNullOrEmpty(cliente.Email))
            return Results.BadRequest("Todos os campos devem ser preenchidos ao atualizar um cliente");

        context.Clientes.Update(clienteAtual);
        var result = await context.SaveChangesAsync();

        return result > 0
            ? Results.NoContent()
            : Results.BadRequest("Ocorreu erro ao atualizar cliente");
    })
    .Produces(StatusCodes.Status204NoContent)
    .Produces(StatusCodes.Status400BadRequest)
    .Produces(StatusCodes.Status404NotFound)
    .WithName("PutCliente")
    .WithTags("Cliente");
#endregion

#region DeleteCliente
app.MapDelete("/excluiCliente{email}", async (
    ClienteContextDb context,
    string email) =>
    {
        var clienteAtual = await context.Clientes.AsNoTracking<Cliente>().FirstOrDefaultAsync(c => c.Email == email);
        if (clienteAtual == null)
            return Results.NotFound();

        context.Clientes.Remove(clienteAtual);
        var result = await context.SaveChangesAsync();

        return result > 0
            ? Results.NoContent()
            : Results.BadRequest("Ocorreu erro ao excluir o cliente");
    })
    .Produces(StatusCodes.Status204NoContent)
    .Produces(StatusCodes.Status400BadRequest)
    .Produces(StatusCodes.Status404NotFound)
    .WithName("DeleteCliente")
    .WithTags("Cliente");
#endregion

app.Run();

public partial class Program { }