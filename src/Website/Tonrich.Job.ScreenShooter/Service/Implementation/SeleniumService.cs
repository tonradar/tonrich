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
    public async Task<MemoryStream> LoadTonrichWebsiteAsync(WebDriver driver, string walletId)
    {
        var address = configuration.GetValue<string>("TonrichAddress");
        driver.Navigate().GoToUrl(new Uri(new Uri(address), walletId));

        await Task.Delay(TimeSpan.FromSeconds(1));
        while (true)
        {
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
            catch (Exception ex)
            {
                await Task.Delay(TimeSpan.FromSeconds(1));
            }
        }

        ITakesScreenshot screenshotDriver = driver;
        Screenshot screenshot = screenshotDriver.GetScreenshot();

        var screenshotStream = new MemoryStream(screenshot.AsByteArray);

        return screenshotStream;
    }
}
