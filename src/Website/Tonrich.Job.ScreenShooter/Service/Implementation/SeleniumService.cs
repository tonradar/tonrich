using OpenQA.Selenium;
using Telegram.Bot.Types;
using Tonrich.Job.ScreenShooter.Service.Contract;

namespace Tonrich.Job.ScreenShooter.Service.Implementation;

public class SeleniumService : ISeleniumService
{
    private readonly IConfiguration configuration;

    public SeleniumService(IConfiguration configuration)
    {
        this.configuration = configuration;
    }
    public async Task<MemoryStream> LoadTonrichWebsiteAsync(WebDriver driver, string walletId, CancellationToken cancellationToken)
    {
        var address = configuration.GetValue<string>("TonrichAddress");
        driver.Navigate().GoToUrl(new Uri(new Uri(address), walletId));

        await Task.Delay(TimeSpan.FromSeconds(1), cancellationToken);
        while (true)
        {
            if (cancellationToken.IsCancellationRequested)
                throw new TaskCanceledException();

            try
            {
                driver.FindElement(By.CssSelector("[class*='loading']"));
            }
            catch
            {
                break;
            }

            try
            {
                driver.FindElement(By.CssSelector("[class*='loaded']"));
                break;
            }
            catch (Exception)
            {
                await Task.Delay(TimeSpan.FromSeconds(1), cancellationToken);
            }
        }

        ITakesScreenshot screenshotDriver = driver;
        Screenshot screenshot = screenshotDriver.GetScreenshot();

        var screenshotStream = new MemoryStream(screenshot.AsByteArray);

        return screenshotStream;
    }
}
