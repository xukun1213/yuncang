namespace Huayu.Oms.Infrastructure;

public class Dependencies
{
    public static void ConfigureServices(IConfiguration configuration, IServiceCollection services)
    {
        var connString = configuration.GetConnectionString("OmsConnectionString");
        services.AddDbContext<OmsContext>(
        options => options
            .UseMySql(connString, ServerVersion.AutoDetect(connString),
            mySqlOptionsAction: sqlOptions =>
            {
                sqlOptions.SchemaBehavior(Pomelo.EntityFrameworkCore.MySql.Infrastructure.MySqlSchemaBehavior.Ignore);
                sqlOptions.MigrationsAssembly(typeof(Dependencies).Assembly.GetName().Name);
                sqlOptions.EnableRetryOnFailure(maxRetryCount: 15, maxRetryDelay: TimeSpan.FromSeconds(30), errorNumbersToAdd: null);
            })
            // The following three options help with debugging, but should
            // be changed or removed for production.
            .LogTo(Console.WriteLine, LogLevel.Information)
            .EnableSensitiveDataLogging()
            .EnableDetailedErrors(),
        ServiceLifetime.Scoped
            );
    }
}
