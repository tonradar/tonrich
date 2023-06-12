namespace Tonrich.Shared.Dtos.TonApi;

public class EventActionSimplePreview
{
    [JsonPropertyName("full_description")]
    public string? FullDescription { get; set; }

    [JsonPropertyName("short_description")]
    public string? ShortDescription { get; set; }

    [JsonPropertyName("name")]
    public string? Name { get; set; }
}
