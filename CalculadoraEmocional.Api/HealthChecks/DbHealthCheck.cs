using CalculadoraEmocional.Api.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Diagnostics.HealthChecks;

namespace CalculadoraEmocional.Api.HealthChecks
{
    public class DbHealthCheck : IHealthCheck
    {
        private readonly CalculadoraEmocionalContext _context;

        public DbHealthCheck(CalculadoraEmocionalContext context)
        {
            _context = context;
        }

        public async Task<HealthCheckResult> CheckHealthAsync(
            HealthCheckContext context,
            CancellationToken cancellationToken = default)
        {
            try
            {
                var canConnect = await _context.Database.CanConnectAsync(cancellationToken);

                if (!canConnect)
                {
                    return HealthCheckResult.Unhealthy("Não foi possível conectar ao Azure SQL.");
                }
                await _context.Checkins
                              .AsNoTracking()
                              .Take(1)
                              .ToListAsync(cancellationToken);

                return HealthCheckResult.Healthy("Azure SQL respondendo normalmente.");
            }
            catch (Exception ex)
            {
                return HealthCheckResult.Unhealthy("Erro ao consultar o Azure SQL.", ex);
            }
        }
    }
}
