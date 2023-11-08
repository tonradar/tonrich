using Microsoft.AspNetCore.Components.Web;
using Tonrich.Client.Shared.Components;

namespace Tonrich.Client.Shared.Pages.Website;

public partial class HomePage
{
    [AutoInject] private IConfigService ConfigService { get; set; } = default!;
    public string WalletAddress { get; set; }
    private string? SearchWalletText { get; set; }

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

    private void OnSearchWalletClick()
    {
        if (string.IsNullOrWhiteSpace(SearchWalletText))
            return;

        NavigationManager.NavigateTo($"/wallet/{SearchWalletText}");
    }

    private void HandleOnKeyDownSearch(KeyboardEventArgs e)
    {
        if (e.Key == "Enter")
        {
            OnSearchWalletClick();
        }
    }
}
