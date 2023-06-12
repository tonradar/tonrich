using System.Net;
using System.Net.Http.Headers;

namespace Tonrich.Client.Shared.Services.Implementations;

public partial class AppHttpClientHandler : HttpClientHandler
{
    [AutoInject] private IAuthTokenProvider _tokenProvider = default!;

    protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
    {
        if (request.Headers.Authorization is null)
        {
            var access_token = await _tokenProvider.GetAcccessTokenAsync();
            if (access_token is not null)
            {
                request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", access_token);
            }
        }

#if MultilingualEnabled && (BlazorServer || BlazorHybrid)
        string cultureCookie = $"c={CultureInfo.CurrentCulture.Name}|uic={CultureInfo.CurrentCulture.Name}";
        request.Headers.Add("Cookie", $".AspNetCore.Culture={cultureCookie}");
#endif

        var response = await base.SendAsync(request, cancellationToken);

        if (response.StatusCode is HttpStatusCode.Unauthorized)
        {
            throw new UnauthorizedException();
        }

        if (response.IsSuccessStatusCode is false && response.Content.Headers.ContentType?.MediaType?.Contains("application/json", StringComparison.InvariantCultureIgnoreCase) is true)
        {
            if (response.Headers.TryGetValues("Request-ID", out IEnumerable<string>? values) && values is not null && values.Any())
            {
#pragma warning disable CS8600 // Converting null literal or possible null value to non-nullable type.
                RestErrorInfo restError = await response.Content.ReadFromJsonAsync(AppJsonContext.Default.RestErrorInfo);
#pragma warning restore CS8600 // Converting null literal or possible null value to non-nullable type.

#pragma warning disable CS8602 // Dereference of a possibly null reference.
#pragma warning disable CS8604 // Possible null reference argument.
                Type exceptionType = typeof(RestErrorInfo).Assembly.GetType(restError.ExceptionType) ?? typeof(UnknownException);
#pragma warning restore CS8604 // Possible null reference argument.
#pragma warning restore CS8602 // Dereference of a possibly null reference.

#pragma warning disable CS8604 // Possible null reference argument.
                var args = new List<object> { typeof(KnownException).IsAssignableFrom(exceptionType) ? new LocalizedString(restError.Key!, restError.Message!) : restError.Message };
#pragma warning restore CS8604 // Possible null reference argument.

                if (exceptionType == typeof(ResourceValidationException))
                {
                    args.Add(restError.Payload);
                }

#pragma warning disable CS8600 // Converting null literal or possible null value to non-nullable type.
                Exception exp = (Exception)Activator.CreateInstance(exceptionType, args.ToArray());
#pragma warning restore CS8600 // Converting null literal or possible null value to non-nullable type.

#pragma warning disable CS8597 // Thrown value may be null.
                throw exp;
#pragma warning restore CS8597 // Thrown value may be null.
            }
        }

        response.EnsureSuccessStatusCode();

        return response;
    }
}
