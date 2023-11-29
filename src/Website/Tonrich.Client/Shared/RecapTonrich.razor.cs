namespace Tonrich.Client.Shared;

public partial class RecapTonrich
{
    [AutoInject] private IConfigService ConfigService { get; set; } = default!;
    private async Task HandelPluginButtonClickAsync()
    {
        NavigationManager.NavigateTo(await ConfigService.GetTonRichPluginUrl());
    }
}
