namespace Tonrich.Shared.Dtos.TonApi;

public class AccountAddress
{
    [JsonPropertyName("bounceable")]
    public string? Bounceable { get; set; }

    [JsonPropertyName("non_bounceable")]
    public string? NonBounceable { get; set; }

    [JsonPropertyName("raw")]
    public string? Raw { get; set; }
}
