using Microsoft.AspNetCore.Components.Web;
using System.Text.Json;
using Tonrich.Shared.Util;

namespace Tonrich.Client.Shared.Components;

public partial class RecapTonrich
{
    [AutoInject] private IConfigService ConfigService { get; set; } = default!;
    private async Task HandelPluginButtonClickAsync()
    {   
        NavigationManager.NavigateTo(await ConfigService.GetTonRichPluginUrl());
    }
}
