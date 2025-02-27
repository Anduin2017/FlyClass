using Anduin.FlyClass.Entities;
using Microsoft.EntityFrameworkCore;

namespace Anduin.FlyClass.InMemory;

public class InMemoryContext(DbContextOptions<InMemoryContext> options) : FlyClassDbContext(options)
{
    public override Task MigrateAsync(CancellationToken cancellationToken)
    {
        return Database.EnsureCreatedAsync(cancellationToken);
    }

    public override Task<bool> CanConnectAsync()
    {
        return Task.FromResult(true);
    }
}
