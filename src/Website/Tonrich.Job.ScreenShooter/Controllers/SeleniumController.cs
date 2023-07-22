using Microsoft.AspNetCore.Mvc;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using System.Drawing;
using System.Reflection;
using Telegram.Bot.Types;
using Tonrich.Job.ScreenShooter.Service.Contract;
using Tonrich.Job.ScreenShooter.Service.Implementation;

namespace Tonrich.Job.ScreenShooter.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class SeleniumController : ControllerBase
    {
        private readonly ISeleniumService _seleniumService;
        private readonly IConfiguration _configuration;

        public static List<(Guid Id, WebDriver webDriver)>? WebDrivers { get; set; }


        public SeleniumController(ISeleniumService seleniumService, IConfiguration configuration)

        {
            _seleniumService = seleniumService;
            this._configuration = configuration;
        }

        [HttpGet]
        public string Test()
        {
            return "Hi!";
        }

        [HttpGet]
        public async Task<IEnumerable<Guid>> InitAsync()
        {
            if (WebDrivers is not null)
            {
                return WebDrivers.Select(c => c.Id).ToList();
            }

            var MaxSeleniumInstance = _configuration.GetValue<int>("MaxSeleniumInstance");
            WebDrivers = new List<(Guid Id, WebDriver webDriver)>();
            var browserDriverPath = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
            for (int i = 0; i < MaxSeleniumInstance; i++)
            {
                var driverId = Guid.NewGuid();
                var chromeOptions = new ChromeOptions();
                chromeOptions.AddArgument("--headless");
                chromeOptions.AddArgument("--disable-gpu");
                chromeOptions.AddArgument("--disable-extensions");
                chromeOptions.AddArgument("--no-sandbox");
                chromeOptions.AddArgument("--disable-dev-shm-usage");
                var driver = new ChromeDriver(browserDriverPath, chromeOptions);
                driver.Manage().Window.Size = new Size(426, 680);

                await _seleniumService.LoadTonrichWebsiteAsync(driver, "EQCFLPL8WqFYzJuftSDrO-dxtK5JRt1zNK6PziXuDnHVdcpR");
                WebDrivers.Add((driverId, driver));
            }

            return WebDrivers.Select(c => c.Id).ToList();
        }

        [HttpGet]
        public async Task<MemoryStream> TakeScreenShootAsync(Guid driverId, string walletId)
        {
            var driver = WebDrivers.Where(c => c.Id == driverId).First().webDriver;
            return await _seleniumService.LoadTonrichWebsiteAsync(driver, walletId);
        }
    }
}
