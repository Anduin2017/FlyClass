using Aiursoft.DbTools.Sqlite;
using Aiursoft.WebTools.Abstractions.Models;
using Anduin.FlyClass.Data;
using Anduin.FlyClass.Models;
using Microsoft.AspNetCore.Identity;

namespace Anduin.FlyClass;

public class Startup : IWebStartup
{
    public void ConfigureServices(IConfiguration configuration, IWebHostEnvironment environment, IServiceCollection services)
    {
        var connectionString = configuration.GetConnectionString("DefaultConnection") ?? throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");

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