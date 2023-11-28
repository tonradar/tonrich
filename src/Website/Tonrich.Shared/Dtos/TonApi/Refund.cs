namespace Tonrich.Shared.Dtos.TonApi;

public class Refund
{
    [JsonPropertyName("origin")]
    public string? Origin { get; set; }

    [JsonPropertyName("type")]
    public string? Type { get; set; }
}
