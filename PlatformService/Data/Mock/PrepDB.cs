using PlatformService.Data.Repositories;
using PlatformService.Models;

namespace PlatformService.Data.Mock;

/// <summary>
/// clase utilizada solamente con fines de testing, ya que se esta utilizando una base de datos en memoria con entity
/// framework
/// </summary>
public static class PrepDB
{
    public static void PrepareSeeding(IApplicationBuilder app, bool isDevelopmentEnviroment)
    {
        if (isDevelopmentEnviroment)
        {
            using (var serviceScope = app.ApplicationServices.CreateScope())
            {
                var dbContext = serviceScope.ServiceProvider.GetService<IPlatformRepository>();
                var logger = serviceScope.ServiceProvider.GetService<ILogger<Program>>();

                SeedData(dbContext!, logger!);
            }
        }
        else
        {
            Console.WriteLine("-->Seeding Cancelado, utilizando base de datos SQL Server en Kubernetes.");
        }
        
    }

    private static void SeedData(IPlatformRepository repository, ILogger logger)
    {
        if (!repository.Any())
        {
            logger.LogInformation("--> Realizando Seeding <--");

            var platforms = new Platform[] {
                new Platform() { Name = ".NET", Publisher = "Microsoft", Cost = "Free" },
                new Platform() { Name = "SQL Server", Publisher = "Microsoft", Cost = "Free" },
                new Platform { Name = "Kubernetes", Publisher = "C.N.C.F", Cost = "Free" }
            };
            
            foreach (var platform in platforms)
            {
                repository.Create(platform);
            }

            if (repository.Commit())
            {
                logger.LogInformation("--> Seeding realizado correctamente <--");
            }
        }
        else
        {
            logger.LogInformation("--> Ya existen datos en la base de datos");
        }
    }
}