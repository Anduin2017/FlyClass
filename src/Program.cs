using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using FlyClass.Data;
using FlyClass.Models;
using Microsoft.AspNetCore.HttpOverrides;
using NuGet.DependencyResolver;
using Microsoft.CodeAnalysis.CSharp.Syntax;

var builder = WebApplication.CreateBuilder(args);
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection") ?? throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");

builder.Services.AddDbContextPool<ApplicationDbContext>(options => options.UseSqlite(connectionString));

builder.Services.AddIdentity<Teacher, IdentityRole>(options => options.Password = new PasswordOptions
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

builder.Services.AddDatabaseDeveloperPageExceptionFilter();
builder.Services.AddControllersWithViews();
builder.Services.Configure<ForwardedHeadersOptions>(options =>
{
    options.ForwardedHeaders =
        ForwardedHeaders.XForwardedFor | ForwardedHeaders.XForwardedProto;
});

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseMigrationsEndPoint();
}
else
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}
app.UseForwardedHeaders();
app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseRouting();
app.UseAuthentication();
app.UseAuthorization();
app.MapDefaultControllerRoute();

await Seed(app.Services.CreateScope().ServiceProvider);
app.Run();

async Task Seed(IServiceProvider services)
{
    var db = services.GetRequiredService<ApplicationDbContext>();
    await db.Database.EnsureCreatedAsync();
    await db.Database.MigrateAsync();
    var userManager = services.GetRequiredService<UserManager<Teacher>>();
    if (!await db.Levels.AnyAsync())
    {
        db.Levels.Add(new Level
        {
            Name = "默认教师等级"
        });
        await db.SaveChangesAsync();
    }

    if (!await db.Teachers.AnyAsync())
    {
        var defaultLevel = await db.Levels.FirstOrDefaultAsync();
        var user = new Teacher
        {
            ChineseName = "管理员",
            UserName = "admin@default.com",
            Email = "admin@default.com",
            LevelId = defaultLevel.Id,
        };
        var result = await userManager.CreateAsync(user, "admin123");
    }
}