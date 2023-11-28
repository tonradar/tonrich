using System.Net.Http;
using Microsoft.AspNetCore.Components.Routing;

namespace Tonrich.Client;

public partial class Routes
{
    private List<System.Reflection.Assembly> lazyLoadedAssemblies = [];
    [AutoInject] private Microsoft.AspNetCore.Components.WebAssembly.Services.LazyAssemblyLoader assemblyLoader = default!;

    [AutoInject] private IJSRuntime _jsRuntime = default!;

    [AutoInject] private HttpClient _httpClient = default!;
    private async Task OnNavigateAsync(NavigationContext args)
    {
        if (OperatingSystem.IsBrowser())
        {
            if ((args.Path is "dashboard") && lazyLoadedAssemblies.Any(asm => asm.GetName().Name == "Newtonsoft.Json") is false)
            {
                var assemblies = await assemblyLoader.LoadAssembliesAsync(["Newtonsoft.Json.wasm", "System.Private.Xml.wasm", "System.Data.Common.wasm"]);
                lazyLoadedAssemblies.AddRange(assemblies);
            }
        }
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
