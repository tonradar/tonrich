using OpenQA.Selenium;
using Telegram.Bot.Types;

namespace Tonrich.Job.ScreenShooter.Service.Contract;

public interface ISeleniumService
{
    Task<MemoryStream> LoadTonrichWebsiteAsync(WebDriver driver, string walletId, CancellationToken cancellationToken);
}
