namespace Tonrich.Client.Shared.Services.Contracts;

public interface IConfigService
{
    Task<string> GetTonRichPluginUrl();
    Task<string> GetTonRichTelegramBotUrl();
}