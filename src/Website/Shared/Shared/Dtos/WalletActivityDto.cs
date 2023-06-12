namespace Tonrich.Shared.Dtos;

public class WalletActivityDto
{
    public DateTimeOffset ActivityDate { get; set; }
    public decimal ActivityAmount { get; set; } = 0;

    public override string ToString()
    {
        return $"{ActivityDate} - {ActivityAmount}";
    }
}