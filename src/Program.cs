using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using FlyClass.Data;
using FlyClass.Models;

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

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseRouting();
app.UseAuthentication();
app.UseAuthorization();
app.MapDefaultControllerRoute();

var db = app.Services.CreateScope().ServiceProvider.GetRequiredService<ApplicationDbContext>();
if (!await db.Levels.AnyAsync())
{
    db.Levels.Add(new Level
    {
        Name = "Default"
    });
    await db.SaveChangesAsync();
}

app.Run();
