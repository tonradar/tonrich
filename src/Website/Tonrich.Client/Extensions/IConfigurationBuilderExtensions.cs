using System.Reflection;

namespace Microsoft.Extensions.Configuration;

public static class IConfigurationBuilderExtensions
{
    public static void AddClientConfigurations(this IConfigurationBuilder builder)
    {
        var assembly = Assembly.Load("Tonrich.Client");
        builder.AddJsonStream(assembly.GetManifestResourceStream("Tonrich.Client.appsettings.json")!);
    }
}
