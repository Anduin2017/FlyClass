using Aiursoft.DbTools;
using Anduin.FlyClass.Entities;
using static Aiursoft.WebTools.Extends;

namespace Anduin.FlyClass;

public abstract class Program
{
    public static async Task Main(string[] args)
    {
        var app = await AppAsync<Startup>(args);
        await app.UpdateDbAsync<FlyClassDbContext>();
        await app.SeedAsync();
        await app.RunAsync();
    }
}
