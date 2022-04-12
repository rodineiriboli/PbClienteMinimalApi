using ClienteApi.Models;
using NUnit.Framework;
using System.Collections.Generic;
using System.Net;
using System.Net.Http.Json;
using System.Threading.Tasks;

namespace ClienteApi.Test
{
    public class ClienteApiUnitTest
    {
        [Test]
        public async Task RetornaTodosClientes_Ok()
        {
            await using var application = new ClienteApiAplication();

            await ClienteMock.CreateCliente(application, true);
            var url = "/todosClientes";

            var client = application.CreateClient();

            var result = await client.GetAsync(url);
            var clientes = await client.GetFromJsonAsync<List<Cliente>>(url);

            Assert.AreEqual(HttpStatusCode.OK, result.StatusCode);
            Assert.IsNotNull(clientes);
            Assert.IsTrue(clientes.Count == 3);
        }

        [Test]
        public async Task AdicionaNovoCliente_Ok()
        {
            await using var application = new ClienteApiAplication();

            await ClienteMock.CreateCliente(application, true);
            var url = "/adicionaCliente";

            var cliente = new Cliente
            {
                Id = new System.Guid(),
                Nome = "Thiago Mendes",
                Email = "mendes@gmail.com"
            };

            var client = application.CreateClient();

            var response = await client.PostAsJsonAsync(url, cliente);

            Assert.AreEqual(HttpStatusCode.Created, response.StatusCode);
        }

        [Test]
        public async Task ConsultarClientePorEmail_Ok()
        {
            await using var application = new ClienteApiAplication();

            await ClienteMock.CreateCliente(application, true);
            var url = "/clientePorEmailvitor%40gmail.com";

            var client = application.CreateClient();

            var response = await client.GetAsync(url);
            var cliente = await client.GetFromJsonAsync<Cliente>(url);

            Assert.AreEqual(HttpStatusCode.OK, response.StatusCode);
            Assert.AreEqual(cliente.Nome, "Vitor Belmont");
        }

        [Test]
        public async Task AtualizaCliente_Ok()
        {
            await using var application = new ClienteApiAplication();
            await ClienteMock.CreateCliente(application, true);

            var client = application.CreateClient();

            var url1 = "/clientePorEmailvitor%40gmail.com";
            var clienteGuidId = await client.GetFromJsonAsync<Cliente>(url1);

            var guidId = clienteGuidId.Id;

            var url = $"/atualizaCliente{guidId}";

            var cliente = new Cliente
            {
                Id = guidId,
                Nome = "Vitor Belmont da Silva",
                Email = "vitor@gmail.com.br"
            };

            var response = await client.PutAsJsonAsync(url, cliente);

            Assert.AreEqual(HttpStatusCode.NoContent, response.StatusCode);
        }

        [Test]
        public async Task DeletaClienteInexistente()
        {
            await using var application = new ClienteApiAplication();

            await ClienteMock.CreateCliente(application, true);

            var client = application.CreateClient();
            var url = "/excluiClientegustavo%40gmail.com.br";
            var response = await client.DeleteAsync(url);

            Assert.AreEqual(HttpStatusCode.NotFound, response.StatusCode);
        }

        [Test]
        public async Task DeletaClienteExistente()
        {
            await using var application = new ClienteApiAplication();

            await ClienteMock.CreateCliente(application, true);

            var client = application.CreateClient();
            var url = "/excluiClientegustavo%40gmail.com";
            var response = await client.DeleteAsync(url);

            var clientes = await client.GetFromJsonAsync<List<Cliente>>("/todosClientes");

            Assert.AreEqual(HttpStatusCode.NoContent, response.StatusCode);
            Assert.IsNotNull(clientes);
            Assert.IsTrue(clientes.Count == 2);
        }
    }
}
