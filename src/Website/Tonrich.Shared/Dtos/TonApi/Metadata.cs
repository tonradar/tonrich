namespace Tonrich.Shared.Dtos.TonApi;

[DtoResourceType(typeof(AppStrings))]
public class Metadata
{
    [JsonPropertyName("attributes")]
    public IEnumerable<IDictionary<string, object>>? Attributes { get; set; }

    [JsonPropertyName("description")]
    public string? Description { get; set; }

    [JsonPropertyName("image")]
    public string? Image { get; set; }

    [JsonPropertyName("marketplace")]
    public string? MarketPlace { get; set; }

    [JsonPropertyName("name")]
    public string? Name { get; set; }
}
