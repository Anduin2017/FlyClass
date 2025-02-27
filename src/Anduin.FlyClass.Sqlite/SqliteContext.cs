using Anduin.FlyClass.Entities;
using Microsoft.EntityFrameworkCore;

namespace Anduin.FlyClass.Sqlite;

public class SqliteContext(DbContextOptions<SqliteContext> options) : FlyClassDbContext(options)
{
    public override Task<bool> CanConnectAsync()
    {
        return Task.FromResult(true);
    }
}
