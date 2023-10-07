namespace Tonrich.Client.Shared.Pages;

public partial class WalletPage:IDisposable
{
    [Parameter] public required string WalletId { get; set; }
    [Parameter] public string Theme { get; set; } = "light";

    [CascadingParameter(Name = "AppStateDto")]
    private AppStateDto? AppStateDto { get; set; }
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

        if (AppStateDto != null) 
            AppStateDto.IsInWalletPage = true;

        base.OnInitialized();
    }

    private bool isAccountBoxBusy = true;

    private void AccountLoad(bool value)
    {
        isAccountBoxBusy = !value;
    }

    public void Dispose()
    {
        if (AppStateDto != null) 
            AppStateDto.IsInWalletPage = false;
    }
}