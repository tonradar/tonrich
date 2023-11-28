namespace Tonrich.Shared.Dtos.TonApi;

public class AccountTransactions
{
    [JsonPropertyName("transactions")]
    public IEnumerable<Transaction>? Transactions { get; set; }

    public decimal SumDepositPrice(string raw)
    {
        if (Transactions != null && Transactions.Any())
        {
            return Transactions.Sum(c => c.DepositTon(raw));
        }

        return 0;
    }


    public decimal SumSpentPrice(string raw)
    {
        if (Transactions != null && Transactions.Any())
        {
            return Transactions.Sum(c => c.SpentTon(raw));
        }

        return 0;
    }
}
