using Microsoft.Extensions.Configuration;

namespace FlyClass.Tests;

public class TestStartup : Startup
{
    public TestStartup(IConfiguration configuration) : base(configuration)
    {
    }
}