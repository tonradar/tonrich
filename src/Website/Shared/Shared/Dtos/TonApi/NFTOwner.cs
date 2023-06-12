namespace Tonrich.Shared.Dtos.TonApi;

public class NFTOwner
{
    [JsonPropertyName("address")]
    public string? Address { get; set; }

    [JsonPropertyName("icon")]
    public string? Icon { get; set; }

    [JsonPropertyName("is_scam")]
    public bool IsScam { get; set; }

    [JsonPropertyName("name")]
    public string? Name { get; set; }
}
