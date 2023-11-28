namespace Tonrich.Shared.Dtos.TonApi;

public class EventFee
{
    [JsonPropertyName("account")]
    public Account? Account { get; set; }

    [JsonPropertyName("deposit")]
    public long Deposit { get; set; }

    [JsonPropertyName("gas")]
    public long Gas { get; set; }

    [JsonPropertyName("refund")]
    public long Refund { get; set; }

    [JsonPropertyName("rent")]
    public long Rent { get; set; }

    [JsonPropertyName("total")]
    public long Total { get; set; }
}
