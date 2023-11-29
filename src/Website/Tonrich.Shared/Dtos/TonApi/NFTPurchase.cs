namespace Tonrich.Shared.Dtos.TonApi;

public class NFTPurchase
{
    [JsonPropertyName("amount")]
    public TokenPrice? Amount { get; set; }

    [JsonPropertyName("buyer")]
    public Account? Buyer { get; set; }

    [JsonPropertyName("seller")]
    public Account? Seller { get; set; }

    [JsonPropertyName("nft")]
    public NFTItem? NFT { get; set; }

    [JsonPropertyName("purchase_type")]
    public string? PurchaseType { get; set; }
}
