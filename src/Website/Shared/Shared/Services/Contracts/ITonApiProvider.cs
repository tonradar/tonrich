using Tonrich.Shared.Dtos.TonApi;

namespace Tonrich.Shared.Services.Contracts;

public interface ITonApiProvider
{
    Task<AccountInfo?> GetAccountInfoAsync(string walletId, CancellationToken cancellationToken = default);
    Task<BulkAccountInfo?> GetBulkAccountInfoAsync(IEnumerable<string> raws, CancellationToken cancellationToken = default);

    Task<AccountNFT?> GetNftsAsync(string walletId, string collectionAddress, CancellationToken cancellationToken = default);
    Task<AccountEvents?> GetEventsAsync(string walletId, long from, long? to = null, CancellationToken cancellationToken = default);
    Task<AccountTransactions?> GetTransactionsAsync(
        string account,
        long? minLt = null,
        long? maxLt = null,
        int limit = 100,
        CancellationToken cancellationToken = default);
}
