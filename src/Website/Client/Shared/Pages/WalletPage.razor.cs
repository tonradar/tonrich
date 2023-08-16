using Microsoft.AspNetCore.Components.Web;
using System.Globalization;
using System.Text.Json;
using Tonrich.Client.Shared.Shared;
using Tonrich.Shared.Util;
using static System.Net.Mime.MediaTypeNames;

namespace Tonrich.Client.Shared.Pages;

public partial class WalletPage
{
    [Parameter] public required string WalletId { get; set; }
    [Parameter] public string Theme { get; set; } = "light";
    protected override void OnInitialized()
    {
        var query = new Uri(NavigationManager.Uri).Query;
        if (!string.IsNullOrWhiteSpace(query))
        {
            var parameters = query.Replace("?", "").Split('&');
            foreach (var param in parameters)
            {
                if (param.StartsWith("theme"))
                {
                    Theme = param.Split('=')[1];
                }
            }
        }

        base.OnInitialized();
    }
    private bool isAccountBoxBusy = true;

    private void AccountLoad(bool value)
    {
        isAccountBoxBusy = !value;
    }
}