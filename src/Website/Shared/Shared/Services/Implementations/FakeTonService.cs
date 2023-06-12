namespace Tonrich.Shared.Services.Implementations;

public class FakeTonService : ITonService
{
    public Task<AccountInfoDto?> GetAccountInfoAsync(string walletId, CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }

    public Task<List<NFTDto>?> GetNFTsAsync(string walletId, string collectionAddress, CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }

  
    public Task<TransactionInfoDto?> GetTransactionsAsync(string raw, CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }

    public Task<decimal> GetNumbersPriceAsync(IEnumerable<string> numbers, CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }


    public Task<decimal> GetUserNamesPriceAsync(IEnumerable<string> userNames, CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }
}
