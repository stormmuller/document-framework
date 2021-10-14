using System;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.DependencyInjection;
using System.Threading.Tasks;
using MongoDB.Driver;

namespace Ornaments
{
    class Program
    {
        const string connectionString = "mongodb://user:OrnamentsG4lore!@localhost:27017/?authSource=admin";

        static Task Main(string[] args)
        {
            using IHost host = CreateHostBuilder(args).Build();

            var ornamentsContext = host.Services.GetRequiredService<OrnamentsContext>();
            var snowGlobeService = host.Services.GetRequiredService<SnowGlobeService>();

            ornamentsContext.SyncMigrations();    

            foreach (var snowGlobe in snowGlobeService.GetSnowGlobes())
            {
                Console.WriteLine($"{snowGlobe.Name} is a snowglobe that contains a {snowGlobe.Description}");
            }

            return host.RunAsync();
        }

        static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureServices((hostContext, services) =>
                {
                    services
                        .AddTransient<SnowGlobeService>()
                        .AddTransient<IMongoDatabase>(services => 
                        {
                            var client = new MongoClient(connectionString);
                            
                            return client.GetDatabase("ornaments-db");
                        })
                        .AddMongoContext<OrnamentsContext>();
                });
    }
}
