using Huayu.Wms.Identity.Infrastructure.Data.Contexts;
using Huayu.Wms.Identity.Infrastructure.Models;

using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace Huayu.Wms.Identity.Infrastructure
{
    public class Dependencies
    {
        public static void ConfigureServices(IConfiguration configuration, IServiceCollection services)
        {
            var connString = configuration.GetConnectionString("OmsConnectionString");
            services.AddDbContext<ApplicationContext>(
            options => options
                .UseMySql(connString, ServerVersion.AutoDetect(connString),
                mySqlOptionsAction: sqlOptions =>
                {
                    var translator = new Pomelo.EntityFrameworkCore.MySql.Infrastructure.MySqlSchemaNameTranslator((schemaName, objecName) => { return schemaName + "_" + objecName; });
                    sqlOptions.SchemaBehavior(Pomelo.EntityFrameworkCore.MySql.Infrastructure.MySqlSchemaBehavior.Translate, translator);
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



            services
                .AddIdentity<ApplicationUser, ApplicationRole>(options =>
                {
                    options.Password.RequiredLength = 6;
                    options.Password.RequireDigit = false;
                    options.Password.RequireLowercase = false;
                    options.Password.RequireNonAlphanumeric = false;
                    options.Password.RequireUppercase = false;
                    options.User.RequireUniqueEmail = true;
                })
                .AddEntityFrameworkStores<ApplicationContext>()
                .AddDefaultTokenProviders();


            services.AddHttpContextAccessor();
            services.AddScoped<ICurrentUserService, CurrentUserService>();
        }
    }
}
