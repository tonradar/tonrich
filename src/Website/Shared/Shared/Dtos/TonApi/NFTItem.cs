namespace Tonrich.Shared.Dtos.TonApi;

public class NFTItem
{
    [JsonPropertyName("address")]
#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
    public string Address { get; set; }
#pragma warning restore CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.

    [JsonPropertyName("collection_address")]
    public string? CollectionAddress { get; set; }

    [JsonPropertyName("index")]
    public long Index { get; set; }

    [JsonPropertyName("init")]
    public bool Init { get; set; }

    [JsonPropertyName("metadata")]
    public Metadata? MetaData { get; set; }

    [JsonPropertyName("raw_individual_content")]
    public string? RawIndividualContent { get; set; }

    [JsonPropertyName("verified")]
    public bool Verified { get; set; }

    [JsonPropertyName("owner")]
    public NFTOwner? Owner { get; set; }

    [JsonPropertyName("previews")]
    public List<NFTItemPreview>? NFTItemPreviews { get; set; }

    [JsonPropertyName("sale")]
    public NFTSale? Sale { get; set; }

    public override string ToString()
    {
        return $"{MetaData?.Name}";
    }
}
