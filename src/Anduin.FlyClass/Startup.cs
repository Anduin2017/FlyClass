using Aiursoft.CSTools.Tools;
using Aiursoft.DbTools.Sqlite;
using Aiursoft.DbTools.Switchable;
using Aiursoft.WebTools.Abstractions.Models;
using Anduin.FlyClass.Entities;
using Anduin.FlyClass.InMemory;
using Anduin.FlyClass.MySql;
using Anduin.FlyClass.Sqlite;
using Microsoft.AspNetCore.Identity;

namespace Anduin.FlyClass;

public class Startup : IWebStartup
{
    public void ConfigureServices(IConfiguration configuration, IWebHostEnvironment environment, IServiceCollection services)
    {
        var (connectionString, dbType, allowCache) = configuration.GetDbSettings();
        services.AddSwitchableRelationalDatabase(
            dbType: EntryExtends.IsInUnitTests() ? "InMemory": dbType,
            connectionString: connectionString,
            supportedDbs:
            [
                new MySqlSupportedDb(allowCache: allowCache, splitQuery: false),
                new SqliteSupportedDb(allowCache: allowCache, splitQuery: true),
                new InMemorySupportedDb()
            ]);

        services.AddMemoryCache();
        services.AddAiurSqliteWithCache<FlyClassDbContext>(connectionString);


        services.AddIdentity<Teacher, IdentityRole>(options => options.Password = new PasswordOptions
        {
            RequireNonAlphanumeric = false,
            RequireDigit = false,
            RequiredLength = 6,
            RequiredUniqueChars = 0,
            RequireLowercase = false,
            RequireUppercase = false
        })
        .AddEntityFrameworkStores<FlyClassDbContext>()
        .AddDefaultTokenProviders();

        services.AddControllersWithViews().AddApplicationPart(typeof(Startup).Assembly);
    }

    public void Configure(WebApplication app)
    {
        app.UseExceptionHandler("/Home/Error");
        app.UseStaticFiles();
        app.UseRouting();
        app.UseAuthentication();
        app.UseAuthorization();
        app.MapDefaultControllerRoute();
    }
}
