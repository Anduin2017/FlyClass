using Anduin.FlyClass.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Anduin.FlyClass.Data;

public class FlyClassDbContext : IdentityDbContext<Teacher>
{
    public FlyClassDbContext(DbContextOptions<FlyClassDbContext> options)
        : base(options)
    {
    }

    public DbSet<Teacher> Teachers { get; set; }
    public DbSet<TeachEvent> TeachEvents { get; set; }
    public DbSet<Level> Levels { get; set; }
    public DbSet<Site> Sites { get; set; }
    public DbSet<ClassType> ClassTypes { get; set; }
    public DbSet<MoneyMap> MoneyMaps { get; set; }
}
