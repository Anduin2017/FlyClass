using Aiursoft.DbTools;
using FlyClass.Data;
using FlyClass.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using static Aiursoft.WebTools.Extends;

namespace FlyClass;

public class Program
{
    public static async Task Main(string[] args)
    {
        var app = App<Startup>(args);
        await app.UpdateDbAsync<ApplicationDbContext>(UpdateMode.MigrateThenUse);
        await app.SeedAsync();
        await app.RunAsync();
    }
}

public static class ProgramExtends
{
    private static async Task AddPaymentOption(ApplicationDbContext db, string levelType, string classType, int payment)
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

    public static async Task<IHost> SeedAsync(this IHost host)
    {
        using var scope = host.Services.CreateScope();
        var services = scope.ServiceProvider;
        var db = services.GetRequiredService<ApplicationDbContext>();
        var userManager = services.GetRequiredService<UserManager<Teacher>>();
        var roleManager = services.GetRequiredService<RoleManager<IdentityRole>>();
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

        var role = await roleManager.FindByNameAsync("Admin");
        if (role == null)
        {
            role = new IdentityRole("Admin");
            await roleManager.CreateAsync(role);
        }

        role = await roleManager.FindByNameAsync("Reviewer");
        if (role == null)
        {
            role = new IdentityRole("Reviewer");
            await roleManager.CreateAsync(role);
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
            _ = await userManager.CreateAsync(user, "admin123");
            await userManager.AddToRoleAsync(user, "Admin");
        }
        return host;
    }
}
