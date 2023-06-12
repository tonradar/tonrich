namespace Tonrich.Shared.Dtos.TonApi;

public class SubscribeAction
{
    [JsonPropertyName("amount")]
    public long Amount { get; set; }

    [JsonPropertyName("beneficiary")]
    public Account? Beneficiary { get; set; }

    [JsonPropertyName("subscriber")]
    public Account? Subscriber { get; set; }

    [JsonPropertyName("initial")]
    public bool Initial { get; set; }

    [JsonPropertyName("subscription")]
    public string? Subscription { get; set; }
}
