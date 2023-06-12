using Tonrich.Shared.Dtos.TonApi;
using Tonrich.Shared.Util;

namespace Tonrich.Shared.Services.Implementations;

public partial class TonService : ITonService
{
    [AutoInject] public ITonApiProvider TonApiProvider { get; set; }
    [AutoInject] public IFragmentProvider FragmentProvider { get; set; }

    public async Task<AccountInfoDto?> GetAccountInfoAsync(string walletId, CancellationToken cancellationToken = default)
    {
        var tonApiAccountInfo = await TonApiProvider.GetAccountInfoAsync(walletId, cancellationToken);
        if (tonApiAccountInfo == null)
            return null;

        var accountInfo = new AccountInfoDto(tonApiAccountInfo);
        return accountInfo;
    }

    public async Task<List<NFTDto>?> GetNFTsAsync(string walletId, string collectionAddress, CancellationToken cancellationToken = default)
    {
        var tonApiNFTs = await TonApiProvider.GetNftsAsync(walletId, collectionAddress, cancellationToken);
        if (tonApiNFTs?.NFTs == null)
            return null;

        var result = tonApiNFTs.NFTs.Select(c => new NFTDto(c)).ToList();
        return result;
    }
    
    public async Task<TransactionInfoDto?> GetTransactionsAsync(string raw, CancellationToken cancellationToken = default)
    {
        var dtNow = DateTimeOffset.Now;
        var from = DateTimeOffset.Now.AddMonths(-AppSetting.TransactionsTimePeriodPerMonth);
        var pageCount = 100;

        var lastMonth = DateTimeOffset.Now.AddMonths(-1);

        List<Transaction> transactions = new();

        long? maxLt = null;

        do
        {
            var tonApiTransactions = await TonApiProvider.GetTransactionsAsync(raw, minLt: null, maxLt, pageCount, cancellationToken);
            if (tonApiTransactions?.Transactions == null)
                break;

            tonApiTransactions!.Transactions = tonApiTransactions!.Transactions.Where(c => c.CreateDateTime >= from);

            transactions.AddRange(tonApiTransactions!.Transactions);

            if (tonApiTransactions?.Transactions.Where(c => c.CreateDateTime >= from).Count() < pageCount)
                break;

            maxLt = tonApiTransactions!.Transactions.Min(c => c.Lt);
        } while (true);

        if (!transactions.Any())
            return null;

        var lastMonthTransactions = transactions.Where(c => c.CreateDateTime >= lastMonth);

        var startDateTime = transactions.Min(c => c.CreateDateTime);
        var endDateTime = transactions.Max(c => c.CreateDateTime);

        var distance = DateTimeExtensions.MonthCount(endDateTime, startDateTime);

        var deposit = transactions.Sum(c => c.DepositTon(raw)) / AppSetting.TONDenominator;
        var spent = transactions.Sum(c => c.SpentTon(raw)) / AppSetting.TONDenominator;


        var result = new TransactionInfoDto
        {
            DepositLastMonth = lastMonthTransactions.Sum(c => c.DepositTon(raw)) / AppSetting.TONDenominator,
            SpentLastMonth = lastMonthTransactions.Sum(c => c.SpentTon(raw)) / AppSetting.TONDenominator,
            DepositRate = deposit / distance,
            SpentRate = spent / distance,
            Deposit = deposit,
            Spent = spent,
        };


        var activities = transactions
            .GroupBy(c => c.CreateDateTime.Date)
            .Select(c => new WalletActivityDto
            {
                ActivityDate = c.Key,
                ActivityAmount = c.Sum(c => c.Price(raw)) / AppSetting.TONDenominator
            });

        result.Activities.AddRange(activities);
        result.Transactions = transactions;
        return result;
       


    }

    public async Task<decimal> GetNumbersPriceAsync(IEnumerable<string> numbers, CancellationToken cancellationToken = default)
    {
        var tasks = new List<Task<decimal>>();
        foreach (var number in numbers)
        {
            tasks.Add(FragmentProvider.GetNumberPriceAsync(number, cancellationToken));
        }

        await Task.WhenAll(tasks);

        return tasks.Sum(c => c.Result);
    }

    public async Task<decimal> GetUserNamesPriceAsync(IEnumerable<string> userNames, CancellationToken cancellationToken = default)
    {
        var tasks = new List<Task<decimal>>();
        foreach (var userName in userNames)
        {
            tasks.Add(FragmentProvider.GetUserNamePriceAsync(userName, cancellationToken));
        }

        await Task.WhenAll(tasks);

        return tasks.Sum(c => c.Result);
    }
}
