namespace Tonrich.Server.Api.Controllers;

[AllowAnonymous]
[Route("api/[controller]/[action]")]
[ApiController]
public partial class NFTController : AppControllerBase
{
    [AutoInject] protected ITonService TonService;
    [HttpPost]
    public async Task<decimal> GetUserNamesPriceAsync(IEnumerable<string> userNames, CancellationToken cancellationToken)
    {
        return await TonService.GetUserNamesPriceAsync(userNames, cancellationToken);
    }

    [HttpPost]
    public async Task<decimal> GetNumbersPriceAsync(IEnumerable<string> numbers, CancellationToken cancellationToken)
    {
        return await TonService.GetNumbersPriceAsync(numbers, cancellationToken);
    }
}
