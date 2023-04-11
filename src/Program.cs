using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using FlyClass.Data;
using FlyClass.Models;
using Microsoft.AspNetCore.HttpOverrides;
using NuGet.DependencyResolver;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using FlyClass.Migrations;

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

async Task AddPaymentOption(ApplicationDbContext db, string levelType, string classType, int payment)
{
    var classTypeId = await db.ClassTypes.Where(t => t.Name == classType).Select(t => t.Id).FirstAsync();
    var levelTypeId = await db.Levels.Where(t => t.Name == levelType).Select(t => t.Id).FirstAsync();

    await db.AddAsync(new MoneyMap 
    {
        ClassTypeId = classTypeId,
        LevelId = levelTypeId,
        Bonus = payment
    });
}

async Task Seed(IServiceProvider services)
{
    var db = services.GetRequiredService<ApplicationDbContext>();
    await db.Database.MigrateAsync();
    var userManager = services.GetRequiredService<UserManager<Teacher>>();
    if (!await db.Levels.AnyAsync())
    {
        await db.Levels.AddAsync(new Level
        {
            Name = "实习老师"
        });
        await db.Levels.AddAsync(new Level
        {
            Name = "初级老师"
        });
        await db.Levels.AddAsync(new Level
        {
            Name = "中级老师"
        });
        await db.Levels.AddAsync(new Level
        {
            Name = "高级老师"
        });
        await db.SaveChangesAsync();
    }
    if (!await db.Sites.AnyAsync())
    {
        await db.Sites.AddAsync(new Site
        {
            SiteName = "昆山校区"
        });
        await db.Sites.AddAsync(new Site
        {
            SiteName = "相城校区"
        });
        await db.SaveChangesAsync();
    }
    if (!await db.ClassTypes.AnyAsync())
    {
        await db.ClassTypes.AddAsync(new ClassType
        {
            Name = "45分钟正式课"
        });
        await db.ClassTypes.AddAsync(new ClassType
        {
            Name = "60分钟正式课"
        });
        await db.ClassTypes.AddAsync(new ClassType
        {
            Name = "90分钟正式课"
        });
        await db.SaveChangesAsync();
    }

    if (!await db.MoneyMaps.AnyAsync())
    {
        await AddPaymentOption(db, "实习老师", "45分钟正式课", 120);
        await AddPaymentOption(db, "实习老师", "60分钟正式课", 140);
        await AddPaymentOption(db, "实习老师", "90分钟正式课", 160);
        await AddPaymentOption(db, "初级老师", "45分钟正式课", 160);
        await AddPaymentOption(db, "初级老师", "60分钟正式课", 200);
        await AddPaymentOption(db, "初级老师", "90分钟正式课", 300);
        await AddPaymentOption(db, "中级老师", "45分钟正式课", 200);
        await AddPaymentOption(db, "中级老师", "60分钟正式课", 260);
        await AddPaymentOption(db, "中级老师", "90分钟正式课", 400);
        await AddPaymentOption(db, "高级老师", "45分钟正式课", 240);
        await AddPaymentOption(db, "高级老师", "60分钟正式课", 300);
        await AddPaymentOption(db, "高级老师", "90分钟正式课", 440);
        await db.SaveChangesAsync();
    }

    if (!await db.Teachers.AnyAsync())
    {
        var defaultLevel = await db.Levels.OrderByDescending(t => t.Id).FirstOrDefaultAsync();
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