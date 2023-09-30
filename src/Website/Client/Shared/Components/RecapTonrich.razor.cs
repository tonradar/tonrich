using Microsoft.AspNetCore.Components.Web;
using System.Text.Json;
using Tonrich.Shared.Util;

namespace Tonrich.Client.Shared.Components;

public partial class RecapTonrich
{
    private void HandelPluginButtonClick()
    {   
        NavigationManager.NavigateTo(AppUrlLocalizer.GetString(nameof(AppUrlStrings.TonRichPluginUrl)));
    }
}
