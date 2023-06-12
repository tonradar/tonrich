using HtmlAgilityPack;
using System.Linq;

namespace Tonrich.Shared.Services.Implementations;

public class FragmentProvider : IFragmentProvider
{
    public async Task<decimal> GetUserNamePriceAsync(string userName, CancellationToken cancellationToken = default)
    {
        var url = $@"https://fragment.com/username/{userName.Replace(".t.me", "").Replace(" ", "")}";
        return await GetNftPriceAsync(url, cancellationToken).ConfigureAwait(false);
    }


    public async Task<decimal> GetNumberPriceAsync(string number, CancellationToken cancellationToken = default)
    {
        var url = $@"https://fragment.com/number/{number.Replace(" ", "").Replace("+","")}";
        return  await GetNftPriceAsync(url, cancellationToken).ConfigureAwait(false);
    }

    private static async Task<decimal> GetNftPriceAsync(string url, CancellationToken cancellationToken)
    {
        var web = new HtmlWeb();
        var doc = await web.LoadFromWebAsync(url, cancellationToken).ConfigureAwait(false);
        var node = doc.DocumentNode.SelectSingleNode("//div[@class='table-cell-value tm-value icon-before icon-ton']");
        var innerText = node?.InnerText;
        _ = decimal.TryParse(innerText, out var price);
        return price;
    }
}
