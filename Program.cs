using CouponUnblockQualifier.Database;
using CouponUnblockQualifier.Process;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace CouponUnblockQualifier
{
    public class Program
    {
        public IConfiguration? Configuration { get; }

        public static async Task Main(string[] args)
        {
            Console.WriteLine("Program start");
            var host = CreateHostBuilder(args).Build();
            using (var serviceScope = host.Services.CreateScope())
            {
                var services = serviceScope.ServiceProvider;
                var processor = services.GetRequiredService<Processor>();
                await processor.Process().ConfigureAwait(false);
            }
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
                 Host.CreateDefaultBuilder(args).UseEnvironment("Development") 
            .ConfigureAppConfiguration(c => c.AddJsonFile("appsettings.json", optional: false, reloadOnChange: true))
            .ConfigureServices((hostContext, services) =>
            {
                services.AddSingleton<IDBHelper, DBHelper>();
                services.AddSingleton<Processor>();                
            });
    }
}