using Tonrich.Shared.Test.Common;

namespace Tonrich.Shared.Test.UnitTest;

[TestClass]
public class TonServiceTest : TestBase
{
    public static ITonService TonService { get; set; } = default!;
    public static HttpClient HttpClient { get; set; } = default!;
    private const string walletId = "EQCFLPL8WqFYzJuftSDrO-dxtK5JRt1zNK6PziXuDnHVdcpR";

    [ClassInitialize]
    public static void Init(TestContext testContext)
    {
        TonService = ServiceProvider.GetRequiredService<ITonService>();
    }

    [TestMethod]
    public async Task GetAccountInfo_MustWork()
    {
        var result = await TonService.GetAccountInfoAsync(walletId);
        Assert.IsNotNull(result?.Address);
        Assert.AreEqual(walletId, result?.Address.ToString());
    }

    [TestMethod]
    public async Task GetTransactions_MustWork()
    {
        var account = await TonService.GetAccountInfoAsync(walletId);

        var result = await TonService.GetTransactionsAsync(account!.Raw);

        Assert.IsNotNull(result);
        var activities = result.Activities;
    }

    [TestMethod]
    [DataRow("0:80d78a35f955a14b679faa887ff4cd5bfc0f43b4a4eea2a7e6927f3701b273c2")]
    [DataRow("0:0e41dc1dc3c9067ed24248580e12b3359818d83dee0304fabcf80845eafafdb2")]
    public async Task GetNFTs_MustWork(string collectionAddress)
    {
        var account = await TonService.GetAccountInfoAsync(walletId);

        var result = await TonService.GetNFTsAsync(account!.Raw, collectionAddress);
        Assert.IsNotNull(result);
    }
}
