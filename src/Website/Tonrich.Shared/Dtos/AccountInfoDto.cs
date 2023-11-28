using Tonrich.Shared.Dtos.TonApi;


namespace Tonrich.Shared.Dtos;

public class AccountInfoDto
{
    public AccountInfoDto(AccountInfo accountInfo)
    {
        Raw = accountInfo.AccountAddress!.Raw!;
        Address = accountInfo.AccountAddress?.Bounceable;
        Name = accountInfo.Name;
        Balance = accountInfo.Balance / FixedConfig.TONDenominator;
    }

    public string Raw { get; set; }
    public string? Address { get; set; }
    public string? Name { get; set; }
    public decimal Balance { get; set; }
}
