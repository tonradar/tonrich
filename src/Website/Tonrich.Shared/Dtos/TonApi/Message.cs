namespace Tonrich.Shared.Dtos.TonApi;

public class Message
{
    [JsonPropertyName("source")]
    public Account Source { get; set; } = default!;

    [JsonPropertyName("destination")]
    public Account Destination { get; set; } = default!;

    [JsonPropertyName("value")]
    public decimal Value { get; set; }
}
