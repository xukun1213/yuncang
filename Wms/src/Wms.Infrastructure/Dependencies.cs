
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

using Wms.Infrastructure.Data;

namespace Wms.Infrastructure;

public static class Dependencies
{
    public static void ConfigureServices(IConfiguration configuration, IServiceCollection services)
    {

        services.AddDbContext<WmsDbContext>(
        dbContextOptions => dbContextOptions
            .UseMySql(configuration.GetConnectionString("CatalogConnection"), ServerVersion.AutoDetect(configuration.GetConnectionString("CatalogConnection")))
            // The following three options help with debugging, but should
            // be changed or removed for production.
            .LogTo(Console.WriteLine, LogLevel.Information)
            .EnableSensitiveDataLogging()
            .EnableDetailedErrors()
            );

    }
}
