namespace Tonrich.Shared.Dtos.TonApi;

public class AuctionBidAction
{
    [JsonPropertyName("amount")]
    public TokenPrice? Price { get; set; }

    [JsonPropertyName("nft")]
    public NftItemRepr? NFT { get; set; }

    [JsonPropertyName("beneficiary")]
    public Account? Beneficiary { get; set; }

    [JsonPropertyName("bidder")]
    public Account? Bidder { get; set; }
}
