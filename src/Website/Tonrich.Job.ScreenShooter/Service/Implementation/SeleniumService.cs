using OpenQA.Selenium;
using Telegram.Bot.Types;
using Tonrich.Job.ScreenShooter.Service.Contract;

namespace Tonrich.Job.ScreenShooter.Service.Implementation;

public class SeleniumService : ISeleniumService
{
    public async Task<MemoryStream> LoadTonrichWebsiteAsync(WebDriver driver, string walletId)
    {
        driver.Navigate().GoToUrl($"https://dev.tonrich.app/wallet/{walletId}");

        await Task.Delay(TimeSpan.FromSeconds(1));
        while (true)
        {
            try
            {
                driver.FindElement(By.CssSelector("[class*='loading']"));
            }
            catch (Exception ex)
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
