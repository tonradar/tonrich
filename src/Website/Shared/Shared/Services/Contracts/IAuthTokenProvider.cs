namespace Tonrich.Shared.Services.Contracts;

public interface IAuthTokenProvider
{
    Task<string?> GetAcccessTokenAsync();
}
