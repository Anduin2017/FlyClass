using Aiursoft.DbTools;
using Aiursoft.DbTools.Sqlite;
using Anduin.FlyClass.Entities;
using Microsoft.Extensions.DependencyInjection;

namespace Anduin.FlyClass.Sqlite;

public class SqliteSupportedDb(bool allowCache, bool splitQuery) : SupportedDatabaseType<FlyClassDbContext>
{
    public override string DbType => "Sqlite";

    public override IServiceCollection RegisterFunction(IServiceCollection services, string connectionString)
    {
        return services.AddAiurSqliteWithCache<SqliteContext>(
            connectionString,
            splitQuery: splitQuery,
            allowCache: allowCache);
    }

    public override FlyClassDbContext ContextResolver(IServiceProvider serviceProvider)
    {
        return serviceProvider.GetRequiredService<SqliteContext>();
    }
}