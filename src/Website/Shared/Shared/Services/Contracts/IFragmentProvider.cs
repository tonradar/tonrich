namespace Tonrich.Shared.Services.Contracts;

public interface IFragmentProvider
{
    Task<decimal> GetUserNamePriceAsync(string userName, CancellationToken cancellationToken = default);
    Task<decimal> GetNumberPriceAsync(string number, CancellationToken cancellationToken = default);
    Task<decimal> GetNumbersPriceAsync(IEnumerable<string> numbers, CancellationToken cancellationToken = default);
    Task<decimal> GetUserNamesPriceAsync(IEnumerable<string> userNames, CancellationToken cancellationToken = default);
}
