using Tonrich.Shared.Dtos.TonApi;

namespace Tonrich.Shared.Dtos;

public class NFTDto
{
    public NFTDto(NFTItem NFTItem)
    {
        Address = NFTItem.Address;
        Name = NFTItem.MetaData?.Name;
    }
    public string Address { get; set; }
    public decimal? Balance { get; set; }
    public string? Name { get; set; }
}
