using Aiursoft.DbTools;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Anduin.FlyClass.Entities;

public abstract class FlyClassDbContext(DbContextOptions options) : IdentityDbContext<Teacher>(options), ICanMigrate
{
    public DbSet<Teacher> Teachers => Set<Teacher>();
    public DbSet<TeachEvent> TeachEvents => Set<TeachEvent>();
    public DbSet<Level> Levels => Set<Level>();
    public DbSet<Site> Sites => Set<Site>();
    public DbSet<ClassType> ClassTypes => Set<ClassType>();
    public DbSet<MoneyMap> MoneyMaps => Set<MoneyMap>();

    public virtual  Task MigrateAsync(CancellationToken cancellationToken) =>
        Database.MigrateAsync(cancellationToken);

    public virtual  Task<bool> CanConnectAsync() =>
        Database.CanConnectAsync();
}
