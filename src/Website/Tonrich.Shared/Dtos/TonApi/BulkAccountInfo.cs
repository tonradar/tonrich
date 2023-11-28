namespace Tonrich.Shared.Dtos.TonApi;

public class BulkAccountInfo
{
    [JsonPropertyName("accounts")]
    public List<AccountInfo>? Accounts { get; set; }
}
