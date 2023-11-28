namespace Tonrich.Shared.Dtos.TonApi;

public class TonTransfer
{
    [JsonPropertyName("amount")]
    public long Amount { get; set; }


    [JsonPropertyName("recipient")]
    public Account? Recipient { get; set; }


    [JsonPropertyName("sender")]
    public Account? Sender { get; set; }
}
