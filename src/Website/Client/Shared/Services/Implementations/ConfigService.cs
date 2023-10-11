namespace Tonrich.Client.Shared.Services.Implementations;

public partial class ConfigService : IConfigService
{
    [AutoInject] private IJSRuntime _jsRuntime = default!;
    
    public async Task<string> GetTonRichPluginUrl()
    {
        var tonRichPluginUrl = await _jsRuntime.InvokeAsync<string>("App.getLocalStorageItem", "TonRichPluginUrl");
        
        return tonRichPluginUrl;
    }
    
    public async Task<string> GetTonRichTelegramBotUrl()
    {
        var tonRichTelegramBotUrl = await _jsRuntime.InvokeAsync<string>("App.getLocalStorageItem", "TonRichTelegramBotUrl");
        
        return tonRichTelegramBotUrl;
    }
}