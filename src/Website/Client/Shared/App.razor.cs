using System.Net.Http;
using System.Reflection;
using Microsoft.AspNetCore.Components.Routing;

namespace Tonrich.Client.Shared;

public partial class App
{
#if BlazorWebAssembly && !BlazorHybrid
    private List<Assembly> _lazyLoadedAssemblies = new();
    [AutoInject] private Microsoft.AspNetCore.Components.WebAssembly.Services.LazyAssemblyLoader _assemblyLoader = default!;
#endif

    [AutoInject] private IJSRuntime _jsRuntime = default!;

    [AutoInject] private HttpClient _httpClient = default!;

    private bool _cultureHasNotBeenSet = true;

    private async Task OnNavigateAsync(NavigationContext args)
    {
        // Blazor Server & Pre Rendering use created cultures in UseRequestLocalization middleware
        // Android, windows and iOS have to set culture programmatically.
        // Browser is gets handled in Web project's Program.cs
#if BlazorHybrid && MultilingualEnabled
        if (_cultureHasNotBeenSet)
        {
            _cultureHasNotBeenSet = false;
            var preferredCultureCookie = Preferences.Get(".AspNetCore.Culture", null);
            CultureInfoManager.SetCurrentCulture(preferredCultureCookie);
        }
#endif

#if BlazorWebAssembly && !BlazorHybrid
        if (args.Path.Contains("some-lazy-loaded-page") && _lazyLoadedAssemblies.Any(asm => asm.GetName().Name == "SomeAssembly") is false)
        {
            var assemblies = await _assemblyLoader.LoadAssembliesAsync(new[] { "SomeAssembly.dll" });
            _lazyLoadedAssemblies.AddRange(assemblies);
        }
#endif
    }

    protected override async Task OnAfterRenderAsync(bool firstRender)
    {
        if (firstRender)
        {
            var tonRichPluginUrl = await _jsRuntime.InvokeAsync<string>("App.getLocalStorageItem", "TonRichPluginUrl");
            var tonRichTelegramBotUrl = await _jsRuntime.InvokeAsync<string>("App.getLocalStorageItem", "TonRichTelegramBotUrl");
            if (!string.IsNullOrEmpty(tonRichPluginUrl) && !string.IsNullOrEmpty(tonRichTelegramBotUrl))
                return;

            var config = await _httpClient.GetFromJsonAsync<ConfigDto>("Config/GetConfig");
            await _jsRuntime.InvokeVoidAsync("App.setLocalStorageItem", nameof(config.TonRichPluginUrl), config?.TonRichPluginUrl);
            await _jsRuntime.InvokeVoidAsync("App.setLocalStorageItem", nameof(config.TonRichTelegramBotUrl), config?.TonRichTelegramBotUrl);
        }
        await base.OnAfterRenderAsync(firstRender);
    }
}
