using Aiursoft.DbTools;
using Aiursoft.DbTools.InMemory;
using Anduin.FlyClass.Entities;
using Microsoft.Extensions.DependencyInjection;

namespace Anduin.FlyClass.InMemory;

public class InMemorySupportedDb : SupportedDatabaseType<FlyClassDbContext>
{
    public override string DbType => "InMemory";

    public override IServiceCollection RegisterFunction(IServiceCollection services, string connectionString)
    {
        return services.AddAiurInMemoryDb<InMemoryContext>();
    }

    public override FlyClassDbContext ContextResolver(IServiceProvider serviceProvider)
    {
        return serviceProvider.GetRequiredService<InMemoryContext>();
    }
}