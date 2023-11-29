namespace Tonrich.Server.Controllers;

[AllowAnonymous]
[Route("api/[controller]/[action]")]
[ApiController]
public partial class ConfigController : AppControllerBase
{
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
