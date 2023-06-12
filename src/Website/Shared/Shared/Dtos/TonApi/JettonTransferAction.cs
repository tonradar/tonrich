namespace Tonrich.Shared.Dtos.TonApi;

public class JettonTransferAction
{
    [JsonPropertyName("amount")]
    public string? Amount { get; set; }

    [JsonPropertyName("recipient")]
    public Account? Recipient { get; set; }

    [JsonPropertyName("recipients_wallet")]
    public string? RecipientsWallet { get; set; }

    [JsonPropertyName("sender")]
    public Account? Sender { get; set; }

    [JsonPropertyName("senders_wallet")]
    public string? SendersWallet { get; set; }

    [JsonPropertyName("comment")]
    public string? Comment { get; set; }
}
