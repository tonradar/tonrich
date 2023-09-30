namespace Tonrich.Client.Shared.Pages;

public partial class HomePage
{
    public string WalletAddress { get; set; }
    void ChangeWalletAddress(string address)
    {
        WalletAddress = address;
    }

    private void HandelPluginButtonClick()
    {
        NavigationManager.NavigateTo(AppUrlLocalizer.GetString(nameof(AppUrlStrings.TonRichPluginUrl)));
    }

    private void HandelTelegramButtonClick()
    {
        NavigationManager.NavigateTo(AppUrlLocalizer.GetString(nameof(AppUrlStrings.TonRichTelegramBotUrl)));
    }
}
