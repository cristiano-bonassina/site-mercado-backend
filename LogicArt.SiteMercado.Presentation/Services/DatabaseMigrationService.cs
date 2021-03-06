using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace LogicArt.SiteMercado.Presentation.Services
{
    public class DatabaseMigrationService : IHostedService
    {
        private readonly IServiceProvider _serviceProvider;

        public DatabaseMigrationService(IServiceProvider serviceProvider) => _serviceProvider = serviceProvider;

        public Task StartAsync(CancellationToken cancellationToken) => _serviceProvider.GetRequiredService<DbContext>().Database.MigrateAsync(cancellationToken);

        public Task StopAsync(CancellationToken cancellationToken) => Task.CompletedTask;
    }
}
