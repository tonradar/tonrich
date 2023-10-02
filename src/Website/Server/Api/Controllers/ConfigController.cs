namespace Tonrich.Server.Api.Controllers;

[AllowAnonymous]
[Route("api/[controller]/[action]")]
[ApiController]
public partial class ConfigController : AppControllerBase
{
    [AutoInject] private IConfiguration Configuration { get; set; } = default!;
    
    [HttpGet]
    public ConfigDto GetConfig(CancellationToken cancellationToken)
    {
        var configDto = new ConfigDto
        {
            TonRichPluginUrl = AppSettings.TonRichPluginUrl,
            TonRichTelegramBotUrl = AppSettings.TonRichTelegramBotUrl
        };
        
        return configDto;
    }
}