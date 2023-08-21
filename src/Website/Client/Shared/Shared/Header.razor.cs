namespace Tonrich.Client.Shared;

public partial class Header : IDisposable
{
    private bool _disposed;
    private bool _isUserAuthenticated;

    [Parameter] public EventCallback OnToggleMenu { get; set; }
    [Parameter] public bool IsDarkTheme {get; set;} = true;
    [Parameter] public EventCallback OnToggleTheme { get; set; }

    protected override async Task OnInitAsync()
    {
        AuthenticationStateProvider.AuthenticationStateChanged += VerifyUserIsAuthenticatedOrNot;

        _isUserAuthenticated = await StateService.GetValue($"{nameof(Header)}-isUserAuthenticated", AuthenticationStateProvider.IsUserAuthenticatedAsync);

        await base.OnInitAsync();
    }

    async void VerifyUserIsAuthenticatedOrNot(Task<AuthenticationState> task)
    {
        try
        {
            _isUserAuthenticated = await AuthenticationStateProvider.IsUserAuthenticatedAsync();
        }
        catch (Exception ex)
        {
            ExceptionHandler.Handle(ex);
        }
        finally
        {
            StateHasChanged();
        }
    }

    private async Task ToggleMenu()
    {
        await OnToggleMenu.InvokeAsync();
    }

    public async Task ToggleTheme() {
        await OnToggleTheme.InvokeAsync();
    }



    public void Dispose()
    {
        Dispose(true);
        GC.SuppressFinalize(this);
    }

    protected virtual void Dispose(bool disposing)
    {
        if (_disposed) return;

        AuthenticationStateProvider.AuthenticationStateChanged -= VerifyUserIsAuthenticatedOrNot;

        _disposed = true;
    }
}
