using Tonrich.Server.Api;

namespace Microsoft.Extensions.Configuration;

public static class IConfigurationExtensions
{
    public static void FillCustomConfigs(this IConfiguration configuration, AppSettings? config)
    {
        IConfigurationSection baseSection = configuration.GetSection("AppSettings");
        config!.TonRichPluginUrl = baseSection.GetSection("TonRichPluginUrl").Value!;
        config!.TonRichTelegramBotUrl = baseSection.GetSection("TonRichTelegramBotUrl").Value!;
    }
}
