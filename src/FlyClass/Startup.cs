using Aiursoft.DbTools;
using Microsoft.AspNetCore.Identity;
using FlyClass.Data;
using FlyClass.Models;
using Microsoft.AspNetCore.HttpOverrides;

namespace FlyClass;

public class Startup
{
    public Startup(IConfiguration configuration)
    {
        Configuration = configuration;
    }

    private IConfiguration Configuration { get; }

    public void ConfigureServices(IServiceCollection services)
    {
        var connectionString = Configuration.GetConnectionString("DefaultConnection") ?? throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");

        services.AddMemoryCache();
        services.AddAiurSqliteWithCache<ApplicationDbContext>(connectionString);

        services.AddIdentity<Teacher, IdentityRole>(options => options.Password = new PasswordOptions
        {
            RequireNonAlphanumeric = false,
            RequireDigit = false,
            RequiredLength = 6,
            RequiredUniqueChars = 0,
            RequireLowercase = false,
            RequireUppercase = false
        })
        .AddEntityFrameworkStores<ApplicationDbContext>()
        .AddDefaultTokenProviders();

        services.AddDatabaseDeveloperPageExceptionFilter();
        services.AddControllersWithViews();
        services.Configure<ForwardedHeadersOptions>(options =>
        {
            options.ForwardedHeaders =
                ForwardedHeaders.XForwardedFor | ForwardedHeaders.XForwardedProto;
        });
    }

    public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
    {
        app.UseExceptionHandler("/Home/Error");
        app.UseForwardedHeaders();
        app.UseStaticFiles();
        app.UseRouting();
        app.UseAuthentication();
        app.UseAuthorization();
        app.UseEndpoints(endpoints => endpoints.MapDefaultControllerRoute());
    }
}