namespace Tonrich.Shared.Services.Contracts
{
    public interface ITonService
    {
        Task<AccountInfoDto?> GetAccountInfoAsync(string walletId, CancellationToken cancellationToken = default);
        Task<TransactionInfoDto?> GetTransactionsAsync(string raw, CancellationToken cancellationToken = default);
        Task<List<NFTDto>?> GetNFTsAsync(string walletId, string collectionAddress, CancellationToken cancellationToken = default);
    }
}
