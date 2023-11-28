namespace Tonrich.Client.Services.Contracts;
public interface IConfigService
{
    Task<string> GetTonRichPluginUrl();
    Task<string> GetTonRichTelegramBotUrl();
}
