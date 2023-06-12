using Tonrich.Shared.Services.Implementations;

namespace Microsoft.Extensions.DependencyInjection;

public static class IServiceCollectionExtensions
{
    public static void AddSharedServices(this IServiceCollection services)
    {
        // Services being registered here can get injected everywhere (Api, Web, Android, iOS, Windows, and Mac)

        services.AddScoped<ITonService, TonService>();
        services.AddScoped<ITonApiProvider, TonApiProvider>();
        services.AddScoped<IFragmentProvider, FragmentProvider>();

        services.AddLocalization();

        services.AddAuthorizationCore();

        services.AddSingleton<IDateTimeProvider, DateTimeProvider>();

        var token = "eyJhbGciOiJFZERTQSIsInR5cCI6IkpXVCJ9.eyJhdWQiOlsiQWZzaGluX0FsaXphZGVoIl0sImV4cCI6MTgzNTQ0ODk2NiwiaXNzIjoiQHRvbmFwaV9ib3QiLCJqdGkiOiJIMktNTDM1Qk9CUkdPVDZFNVZVV0FJNlgiLCJzY29wZSI6ImNsaWVudCIsInN1YiI6InRvbmFwaSJ9.wiUKHyB61XQjyJrPdIclSsAhBIz5U42PHMHpb8OlJjqAUBThB-czcqmevciNcY7cZWx8xmAyfYsexdEiihSACQ";

        if (BlazorModeDetector.Current.IsBlazorWebAssembly())
        {
            token = "eyJhbGciOiJFZERTQSIsInR5cCI6IkpXVCJ9.eyJhdWQiOlsiQWZzaGluX0FsaXphZGVoIl0sImV4cCI6MTgzNTQ0ODk2NSwiaXNzIjoiQHRvbmFwaV9ib3QiLCJqdGkiOiJYWkoyQ0JSMkNXREVNR1RVNUZST1NPNVIiLCJzY29wZSI6InNlcnZlciIsInN1YiI6InRvbmFwaSJ9.zvC_fLCaK8XxeWrtYqYF81LmF085PQZcsYHNkX-1ITJIXSZtPIIxwSJ5xt_dL11akzVgT0BKyuMz6BtNhPrGAQ";
        }

        services.AddHttpClient("TonApi", c =>
        {
            c.BaseAddress = new Uri("https://tonapi.io/v1/");
            c.DefaultRequestHeaders.Add("Authorization", $"Bearer {token}");
        });
    }
}
