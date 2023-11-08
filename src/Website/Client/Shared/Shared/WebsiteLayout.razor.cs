﻿using Microsoft.AspNetCore.Components.Web;

namespace Tonrich.Client.Shared;

public partial class WebsiteLayout : IDisposable
{
    private bool _disposed;
    private bool _isMenuOpen;
    private bool _isUserAuthenticated;
    public bool IsDarkTheme = true;

    static string ThemeClass(bool isDarkTheme) 
        => isDarkTheme ? "theme-dark" : "theme-light";
#pragma warning disable CS0414 // The field 'MainLayout.ErrorBoundaryRef' is assigned but its value is never used
    private ErrorBoundary ErrorBoundaryRef = default!;
#pragma warning restore CS0414 // The field 'MainLayout.ErrorBoundaryRef' is assigned but its value is never used

    [AutoInject] private IStateService _stateService = default!;

    [AutoInject] private IExceptionHandler _exceptionHandler = default!;

    [AutoInject] private AppAuthenticationStateProvider _authStateProvider = default!;

    [AutoInject] private AppStateDto AppStateDto { get; set; } = default!;

    protected override void OnParametersSet()
    {
        // TODO: we can try to recover from exception after rendering the ErrorBoundary with this line.
        // but for now it's better to persist the error ui until a force refresh.
        // ErrorBoundaryRef.Recover();

        base.OnParametersSet();
    }

    protected override async Task OnInitializedAsync()
    {
        try
        {
            _authStateProvider.AuthenticationStateChanged += VerifyUserIsAuthenticatedOrNot;

            _isUserAuthenticated = await _stateService.GetValue($"{nameof(MainLayout)}-isUserAuthenticated", _authStateProvider.IsUserAuthenticatedAsync);

            await base.OnInitializedAsync();
        }
        catch (Exception exp)
        {
            _exceptionHandler.Handle(exp);
        }
    }

    async void VerifyUserIsAuthenticatedOrNot(Task<AuthenticationState> task)
    {
        try
        {
            _isUserAuthenticated = await _authStateProvider.IsUserAuthenticatedAsync();
        }
        catch (Exception ex)
        {
            _exceptionHandler.Handle(ex);
        }
        finally
        {
            StateHasChanged();
        }
    }

    private void ToggleMenuHandler()
    {
        _isMenuOpen = !_isMenuOpen;
    }

    private void HandleToggleTheme() {
        IsDarkTheme = !IsDarkTheme;
    }

    public void Dispose()
    {
        Dispose(true);
        GC.SuppressFinalize(this);
    }

    protected virtual void Dispose(bool disposing)
    {
        if (_disposed) return;

        _authStateProvider.AuthenticationStateChanged -= VerifyUserIsAuthenticatedOrNot;

        _disposed = true;
    }
}
