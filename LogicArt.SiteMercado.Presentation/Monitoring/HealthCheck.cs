using System;
using System.Threading;
using System.Threading.Tasks;
using LogicArt.SiteMercado.Infrastructure.Persistence;
using Microsoft.Extensions.Diagnostics.HealthChecks;

namespace LogicArt.SiteMercado.Presentation.Monitoring
{
    public class HealthCheck : IHealthCheck
    {

        private readonly ApplicationDbContext _dbContext;

        public HealthCheck(ApplicationDbContext dbContext) => _dbContext = dbContext;

        public async Task<HealthCheckResult> CheckHealthAsync(HealthCheckContext context, CancellationToken cancellationToken = new())
        {

            try
            {

                if (await _dbContext.Database.CanConnectAsync(cancellationToken))
                {
                    return HealthCheckResult.Healthy();
                }

                return HealthCheckResult.Healthy();

            }
            catch (Exception exception)
            {
                return HealthCheckResult.Unhealthy(exception.ToString(), exception);
            }

        }
    }
}
