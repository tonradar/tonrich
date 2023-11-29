using Tonrich.Client.Services.HttpMessageHandlers;
using Microsoft.AspNetCore.Components.WebAssembly.Services;
using Microsoft.Extensions.Configuration;
using System.Net.Http.Headers;

namespace Microsoft.Extensions.DependencyInjection;

public static class IServiceCollectionExtensions
{
    public static IServiceCollection AddClientSharedServices(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddSharedServices();

        services.AddTransient<IPrerenderStateService, PrerenderStateService>();
        services.AddTransient<IExceptionHandler, ExceptionHandler>();
        services.AddScoped<IPubSubService, PubSubService>();
        services.AddBitBlazorUIServices();

        services.AddTransient<RequestHeadersDelegationHandler>();
        services.AddTransient<AuthDelegatingHandler>();
        services.AddTransient<RetryDelegatingHandler>();
        services.AddTransient<ExceptionDelegatingHandler>();
        services.AddTransient<HttpClientHandler>();

        services.AddScoped<AuthenticationStateProvider, AppAuthenticationManager>();
        services.AddScoped(sp => (AppAuthenticationManager)sp.GetRequiredService<AuthenticationStateProvider>());

        services.AddTransient<MessageBoxService>();

        services.AddTransient<LazyAssemblyLoader>();
        services.AddScoped<IConfigService, ConfigService>();
        services.AddHttpClient("TonApi", c =>
        {
            c.BaseAddress = new Uri("https://tonapi.io/v1/");
            c.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", configuration.GetValue<string>("TonApiClientKey"));
        });

        return services;
    }
}
