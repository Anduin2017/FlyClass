using Anduin.FlyClass.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Anduin.FlyClass.Data;

public class FlyClassDbContext(DbContextOptions<FlyClassDbContext> options) : IdentityDbContext<Teacher>(options)
{
    public DbSet<Teacher> Teachers => Set<Teacher>();
    public DbSet<TeachEvent> TeachEvents => Set<TeachEvent>();
    public DbSet<Level> Levels => Set<Level>();
    public DbSet<Site> Sites => Set<Site>();
    public DbSet<ClassType> ClassTypes => Set<ClassType>();
    public DbSet<MoneyMap> MoneyMaps => Set<MoneyMap>();
}
