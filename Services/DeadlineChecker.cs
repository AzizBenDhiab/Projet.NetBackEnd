using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Threading;
using System.Threading.Tasks;
using ProjetNET.Controllers;

public class DeadlineChecker : BackgroundService
{
    private readonly IServiceProvider _services;

    public DeadlineChecker(IServiceProvider services)
    {
        _services = services;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        while (!stoppingToken.IsCancellationRequested)
        {
            using (var scope = _services.CreateScope())
            {
                var dbContext = scope.ServiceProvider.GetRequiredService<AppDbContext>(); // Replace YourDbContext with your actual DbContext

                // Fetch and process entities with deadlines
                var entitiesWithDeadlines = dbContext.Tasks.Where(e => e.DeadLine <= DateTime.Now && e.Status != "Terminé");
                foreach (var entity in entitiesWithDeadlines)
                {
                    entity.Status = "Terminé";
                }
            }

            await Task.Delay(TimeSpan.FromMinutes(1), stoppingToken); // 1 minute interval
        }
    }
}
