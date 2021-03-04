using Lamar.Microsoft.DependencyInjection;
using LogicArt.SiteMercado.Presentation;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;

namespace LogicArt.SiteMercado.Hosting
{
    internal class Program
    {
        private static void Main(string[] args) => CreateHostBuilder(args).Build().Run();

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .UseLamar()
                .ConfigureWebHostDefaults(webBuilder => webBuilder.UseStartup<Startup>());
    }
}
