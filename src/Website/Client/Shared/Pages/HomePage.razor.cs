namespace Tonrich.Client.Shared.Pages;

public partial class HomePage
{
    [AutoInject] private IConfigService ConfigService { get; set; } = default!;
    public string WalletAddress { get; set; }
    void ChangeWalletAddress(string address)
    {
        WalletAddress = address;
    }

    private async Task HandelPluginButtonClickAsync()
    {
        NavigationManager.NavigateTo(await ConfigService.GetTonRichPluginUrl());
    }

    private async Task HandelTelegramButtonClickAsync()
    {
        NavigationManager.NavigateTo(await ConfigService.GetTonRichTelegramBotUrl());
    }
}
