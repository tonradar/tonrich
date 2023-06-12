using Microsoft.Extensions.Hosting;

namespace Tonrich.Shared.Test.Common;

[TestClass]
public class TestBase
{
    protected static IServiceProvider ServiceProvider = default!;
    [AssemblyInitialize]
    public static void Bootstrap(TestContext testContext)
    {
        var testHost = Host
            .CreateDefaultBuilder()
            .ConfigureServices((_, services) =>
            {
                services.AddSharedServices();
            }).Build();

        var serviceScope = testHost.Services.CreateScope();
        ServiceProvider = serviceScope.ServiceProvider;

    }

    [AssemblyCleanup]
    public static void Clean()
    {
    }
}
