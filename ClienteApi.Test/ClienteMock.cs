using ClienteApi.Data;
using ClienteApi.Models;
using Microsoft.Extensions.DependencyInjection;
using System.Threading.Tasks;

namespace ClienteApi.Test
{
    public class ClienteMock
    {
        public static async Task CreateCliente(ClienteApiAplication application, bool create)
        {
            using (var scope = application.Services.CreateScope())
            {
                var provider = scope.ServiceProvider;
                using (var clienteContextDb = provider.GetRequiredService<ClienteContextDb>())
                {
                    await clienteContextDb.Database.EnsureCreatedAsync();

                    if (create)
                    {
                        await clienteContextDb.Clientes.AddAsync(new Cliente
                        { Nome = "Francisco Nogueira", Email = "francisco@gmail.com" });

                        await clienteContextDb.Clientes.AddAsync(new Cliente
                        { Nome = "Gustavo Betor", Email = "gustavo@gmail.com" });

                        await clienteContextDb.Clientes.AddAsync(new Cliente
                        { Nome = "Vitor Belmont", Email = "vitor@gmail.com" });

                        await clienteContextDb.SaveChangesAsync();
                    }
                }
            }
        }
    }
}
