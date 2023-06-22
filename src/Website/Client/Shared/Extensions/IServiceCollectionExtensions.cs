using System.Net.Http.Headers;

namespace Microsoft.Extensions.DependencyInjection;

public static class IServiceCollectionExtensions
{
    public static IServiceCollection AddClientSharedServices(this IServiceCollection services)
    {
        services.AddScoped<IStateService, StateService>();
        services.AddScoped<IExceptionHandler, ExceptionHandler>();
        services.AddScoped<IPubSubService, PubSubService>();

        services.AddTransient<AppHttpClientHandler>();

        services.AddScoped<AuthenticationStateProvider, AppAuthenticationStateProvider>();
        services.AddScoped<IAuthenticationService, AuthenticationService>();
        services.AddScoped(sp => (AppAuthenticationStateProvider)sp.GetRequiredService<AuthenticationStateProvider>());
        services.AddHttpClient("TonApi", c =>
        {
            c.BaseAddress = new Uri("https://tonapi.io/v1/");
            c.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", "eyJhbGciOiJFZERTQSIsInR5cCI6IkpXVCJ9.eyJhdWQiOlsiQWZzaGluX0FsaXphZGVoIl0sImV4cCI6MTgzNTQ0ODk2NSwiaXNzIjoiQHRvbmFwaV9ib3QiLCJqdGkiOiJYWkoyQ0JSMkNXREVNR1RVNUZST1NPNVIiLCJzY29wZSI6InNlcnZlciIsInN1YiI6InRvbmFwaSJ9.zvC_fLCaK8XxeWrtYqYF81LmF085PQZcsYHNkX-1ITJIXSZtPIIxwSJ5xt_dL11akzVgT0BKyuMz6BtNhPrGAQ");
        });
        return services;
    }
}
