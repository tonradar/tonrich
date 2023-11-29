using Tonrich.Shared.Dtos.TonApi.Enum;

namespace Tonrich.Shared.Dtos;

public class TransactionDto
{
    public decimal Amount { get; set; }
    public string? Address { get; set; }
    public bool IsSpent { get; set; }
    public string? NFTAddress { get; set; }

    public EventActionType Type { get; set; }
    public DateTimeOffset TransactionDateTime { get; set; }
}
