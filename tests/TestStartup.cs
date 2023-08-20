using Microsoft.Extensions.Configuration;

namespace Anduin.FlyClass.Tests;

public class TestStartup : Startup
{
    public TestStartup(IConfiguration configuration) : base(configuration)
    {
    }
}