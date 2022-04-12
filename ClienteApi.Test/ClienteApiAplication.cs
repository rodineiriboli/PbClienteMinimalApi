using ClienteApi.Data;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Hosting;

namespace ClienteApi.Test
{
    public class ClienteApiAplication : WebApplicationFactory<Program>
    {
        protected override IHost CreateHost(IHostBuilder builder)
        {
            var root = new InMemoryDatabaseRoot();

            builder.ConfigureServices(services =>
            {
                services.RemoveAll(typeof(DbContextOptions<ClienteContextDb>));

                services.AddDbContext<ClienteContextDb>(options =>
                    options.UseInMemoryDatabase("ClienteDatabase", root));
            });

            return base.CreateHost(builder);
        }
    }
}
