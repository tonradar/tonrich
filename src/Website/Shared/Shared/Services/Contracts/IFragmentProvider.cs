namespace Tonrich.Shared.Services.Contracts;

public interface IFragmentProvider
{
    Task<decimal> GetUserNamePriceAsync(string userName, CancellationToken cancellationToken = default);
    Task<decimal> GetNumberPriceAsync(string number, CancellationToken cancellationToken = default);
}
