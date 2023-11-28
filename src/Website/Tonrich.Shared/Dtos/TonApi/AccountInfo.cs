namespace Tonrich.Shared.Dtos.TonApi;

[DtoResourceType(typeof(AppStrings))]
public class AccountInfo
{
    [JsonPropertyName("address")]
    public AccountAddress? AccountAddress { get; set; }

    [JsonPropertyName("balance")]
    public decimal Balance { get; set; }

    [JsonPropertyName("icon")]
    public string? Icon { get; set; }

    [JsonPropertyName("interfaces")]
    public List<string>? Interfaces { get; set; }

    [JsonPropertyName("is_scam")]
    public bool IsScam { get; set; }

    [JsonPropertyName("last_update")]
    public long LastUpdate { get; set; }

    [JsonPropertyName("memo_required")]
    public bool MemoRequired { get; set; }

    [JsonPropertyName("name")]
    public string? Name { get; set; }

    [JsonPropertyName("status")]
    public string? Status { get; set; }
}
