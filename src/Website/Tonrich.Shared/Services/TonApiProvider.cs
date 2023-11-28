using System.Net.Http.Json;
using System.Text;
using Tonrich.Shared.Dtos.TonApi;

namespace Tonrich.Shared.Services.Implementations;

public partial class TonApiProvider : ITonApiProvider
{
    private HttpClient HttpClient { get; }
    public TonApiProvider(IHttpClientFactory httpClientFactory)
    {
        HttpClient = httpClientFactory.CreateClient("TonApi");
    }

    public async Task<AccountInfo?> GetAccountInfoAsync(string walletId, CancellationToken cancellationToken = default)
    {
        var response = await HttpClient.GetAsync($"account/getInfo?account={walletId}", cancellationToken);
        if (response.IsSuccessStatusCode && response.StatusCode == System.Net.HttpStatusCode.OK)
        {
            await using var stream = await response.Content.ReadAsStreamAsync(cancellationToken);
            var result = System.Text.Json.JsonSerializer.Deserialize<AccountInfo>(stream);
            return result;
        }
        else if (response.StatusCode == System.Net.HttpStatusCode.BadRequest)
        {
            return null;
        }
        else
        {
            response.EnsureSuccessStatusCode();
            return null;
        }
    }

    public async Task<BulkAccountInfo?> GetBulkAccountInfoAsync(IEnumerable<string> raws, CancellationToken cancellationToken = default)
     => await HttpClient.GetFromJsonAsync($"account/getBulkInfo?addresses={string.Join(",", raws)}", AppJsonContext.Default.BulkAccountInfo, cancellationToken);

    public async Task<AccountNFT?> GetNftsAsync(string walletId, string collectionId, CancellationToken cancellationToken = default)
    {
        var result = new AccountNFT
        {
            NFTs = new List<NFTItem>()
        };

        var limit = 1000;
        var pageNumber = 0;

        do
        {
            pageNumber++;
            var apiEndpoint = $"nft/searchItems?owner={walletId}&collection={collectionId}&limit={limit}&offset={limit * (pageNumber - 1)}";

            var NFTAccount = await HttpClient.GetFromJsonAsync(apiEndpoint, AppJsonContext.Default.AccountNFT, cancellationToken);

            if (NFTAccount?.NFTs is null)
                break;

            result.NFTs.AddRange(NFTAccount.NFTs);

            if (NFTAccount.NFTs.Count < limit)
            {
                break;
            }

        } while (true);

        return result;
    }

    public async Task<AccountEvents?> GetEventsAsync(string walletId, long from, long? to = null, CancellationToken cancellationToken = default)
    {
        var result = new AccountEvents
        {
            Events = new List<Event>()
        };

        var limit = 1000;
        var baseApiEndpoint = $"event/getAccountEvents?account={walletId}&limit={limit}&startDate={from}";
        if (to is not null)
        {
            baseApiEndpoint += $"&endDate={to}";
        }

        var apiEndpoint = baseApiEndpoint;

        do
        {
            var accountEvents = await HttpClient.GetFromJsonAsync(apiEndpoint, AppJsonContext.Default.AccountEvents, cancellationToken);

            if (accountEvents?.Events is null)
                break;

            result.Events.AddRange(accountEvents.Events);
            result.NextFrom = accountEvents.NextFrom;

            if (accountEvents.Events.Count < limit)
            {
                break;
            }

            apiEndpoint = $"{baseApiEndpoint}&beforeLt={accountEvents.NextFrom}";

        } while (true);

        return result;
    }

    public async Task<AccountTransactions?> GetTransactionsAsync(
        string account,
        long? minLt = null,
        long? maxLt = null,
        int limit = 100,
        CancellationToken cancellationToken = default)
    {
        var strBuilder = new StringBuilder($"blockchain/getTransactions?account={account}&limit={limit}");
        if (minLt.HasValue)
        {
            strBuilder.Append($"&minLt={minLt}");
        }

        if (maxLt.HasValue)
        {
            strBuilder.Append($"&maxLt={maxLt}");
        }

        return await HttpClient.GetFromJsonAsync(strBuilder.ToString(), AppJsonContext.Default.AccountTransactions, cancellationToken);
    }
}
