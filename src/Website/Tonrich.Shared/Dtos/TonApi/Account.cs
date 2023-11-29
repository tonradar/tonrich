namespace Tonrich.Shared.Dtos.TonApi;

public class Account
{
    [JsonPropertyName("address")]
    public string? Address { get; set; }

    [JsonPropertyName("is_scam")]
    public bool IsScam { get; set; }

    [JsonPropertyName("icon")]
    public string? ICon { get; set; }

    [JsonPropertyName("name")]
    public string? Name { get; set; }
}
