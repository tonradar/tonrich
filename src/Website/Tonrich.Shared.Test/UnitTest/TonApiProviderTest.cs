using Tonrich.Shared.Test.Common;

namespace Tonrich.Shared.Test.UnitTest;

[TestClass]
public class TonApiProviderTest : TestBase
{
    public static ITonApiProvider TonApiProvider { get; set; } = default!;
    public static HttpClient HttpClient { get; set; } = default!;
    private const string walletId = "EQCFLPL8WqFYzJuftSDrO-dxtK5JRt1zNK6PziXuDnHVdcpR";

    [ClassInitialize]
    public static void Init(TestContext testContext)
    {
        TonApiProvider = ServiceProvider.GetRequiredService<ITonApiProvider>();
        var httpClientFactory = ServiceProvider.GetRequiredService<IHttpClientFactory>();
        HttpClient = httpClientFactory.CreateClient("TonApi");
    }

    [TestMethod]
    public async Task GetAccountInfo_MustWork()
    {
        var accountInfo = await TonApiProvider.GetAccountInfoAsync(walletId);
        Assert.IsNotNull(accountInfo);
    }

    [TestMethod]
    [DataRow("0:80d78a35f955a14b679faa887ff4cd5bfc0f43b4a4eea2a7e6927f3701b273c2")]
    [DataRow("0:0e41dc1dc3c9067ed24248580e12b3359818d83dee0304fabcf80845eafafdb2")]
    public async Task GetNfts_MustWork(string collectionId)
    {
        var accountInfo = await TonApiProvider.GetAccountInfoAsync(walletId);
        var nfts = await TonApiProvider.GetNftsAsync(accountInfo!.AccountAddress!.Raw!, collectionId);
        Assert.IsNotNull(nfts);
    }

    [TestMethod]
    [DataRow("0:80d78a35f955a14b679faa887ff4cd5bfc0f43b4a4eea2a7e6927f3701b273c2")]
    [DataRow("0:0e41dc1dc3c9067ed24248580e12b3359818d83dee0304fabcf80845eafafdb2")]
    public async Task NFTPrices_MustHasValue(string collectionId)
    {
        var accountInfo = await TonApiProvider.GetAccountInfoAsync(walletId);
        var nfts = await TonApiProvider.GetNftsAsync(accountInfo!.AccountAddress!.Raw!, collectionId);

        var adresses = nfts?.NFTs?.Select(c => c.Address)?.ToList();

        if (adresses is null)
            return;
        decimal sumPrice = 0;
        foreach (var address in adresses)
        {
            var info = await TonApiProvider.GetAccountInfoAsync(address);
            if (info is null) continue;

            sumPrice += info.Balance;
        }

        var accounts = await TonApiProvider.GetBulkAccountInfoAsync(adresses);

#pragma warning disable CS8604 // Possible null reference argument.
#pragma warning disable CS8602 // Dereference of a possibly null reference.
        var balance = accounts.Accounts.Sum(c => c.Balance);
#pragma warning restore CS8602 // Dereference of a possibly null reference.
#pragma warning restore CS8604 // Possible null reference argument.

        Assert.IsNotNull(balance);

        Assert.AreEqual(balance, sumPrice);
    }


    [TestMethod]
    public async Task GetEvents_MustWork()
    {
        var accountInfo = await TonApiProvider.GetAccountInfoAsync(walletId);

        var from = DateTimeOffset.Now.AddMinutes(-6).ToUnixTimeSeconds();
        var events = await TonApiProvider.GetEventsAsync(accountInfo!.AccountAddress!.Raw!, from);
        Assert.IsNotNull(events);
    }

    [TestMethod]
    public async Task GetTransactions_MustWork()
    {
        var accountInfo = await TonApiProvider.GetAccountInfoAsync(walletId);
        Assert.IsNotNull(accountInfo?.AccountAddress?.Raw);

        var result = await TonApiProvider.GetTransactionsAsync(accountInfo.AccountAddress.Raw, 10);
        Assert.IsNotNull(result?.Transactions);
    }
}
