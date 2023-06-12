namespace Tonrich.Shared.Dtos.TonApi;

[DtoResourceType(typeof(AppStrings))]
public class AccountNFT
{
    [JsonPropertyName("nft_items")]
    public List<NFTItem> NFTs { get; set; } = new();

    public override string ToString()
    {
        return string.Join(",", NFTs);
    }
}
