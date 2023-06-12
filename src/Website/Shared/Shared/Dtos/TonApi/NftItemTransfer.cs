namespace Tonrich.Shared.Dtos.TonApi;

public class NftItemTransfer
{
    [JsonPropertyName("comment")]
    public string? Comment { get; set; }

    [JsonPropertyName("nft")]
    public string? NFT { get; set; }

    [JsonPropertyName("payload")]
    public string? Payload { get; set; }

    [JsonPropertyName("recipient")]
    public Account? Recipient { get; set; }

    [JsonPropertyName("sender")]
    public Account? Sender { get; set; }

    [JsonPropertyName("refund")]
    public Refund? Refund { get; set; }
}
