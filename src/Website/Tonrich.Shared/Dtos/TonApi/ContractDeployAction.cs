namespace Tonrich.Shared.Dtos.TonApi;

public class ContractDeployAction
{
    [JsonPropertyName("address")]
    public string? Address { get; set; }

    [JsonPropertyName("deployer")]
    public Account? Deployer { get; set; }
}
