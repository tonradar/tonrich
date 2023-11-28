using Tonrich.Shared.Dtos.TonApi;

namespace Tonrich.Shared.Dtos;

public class TransactionInfoDto
{
    public decimal? DepositRate { get; set; }
    public decimal? SpentRate { get; set; }
    public decimal? Deposit { get; set; }
    public decimal? Spent { get; set; }
    public decimal? DepositLastMonth { get; set; }
    public decimal? SpentLastMonth { get; set; }

    public List<WalletActivityDto> Activities { get; set; } = new();
    public IEnumerable<Transaction>? Transactions { get; set; }
}
