using Microsoft.AspNetCore.Mvc;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using System.Drawing;
using Tonrich.Job.ScreenShooter.Service.Contract;

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
        public async Task<IEnumerable<Guid>> InitAsync(CancellationToken cancellationToken)
        {
            if (WebDrivers is not null)
            {
                return WebDrivers.Select(c => c.Id).ToList();
            }

            var MaxSeleniumInstance = _configuration.GetValue<int>("MaxSeleniumInstance");
            WebDrivers = new List<(Guid Id, WebDriver webDriver)>();
            for (int i = 0; i < MaxSeleniumInstance; i++)
            {
                if (cancellationToken.IsCancellationRequested)
                    throw new TaskCanceledException();

                var driverId = Guid.NewGuid();
                await InitalDriver(driverId, cancellationToken);
            }

            return WebDrivers.Select(c => c.Id).ToList();
        }

        private async Task InitalDriver(Guid driverId, CancellationToken cancellationToken)
        {
            //var browserDriverPath = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);

            var chromeOptions = new ChromeOptions();
            chromeOptions.AddArgument("--headless");
            chromeOptions.AddArgument("--disable-gpu");
            chromeOptions.AddArgument("--disable-extensions");
            chromeOptions.AddArgument("--no-sandbox");
            chromeOptions.AddArgument("--disable-dev-shm-usage");
            var driver = new ChromeDriver("C://", chromeOptions);
            driver.Manage().Window.Size = new Size(930, 710);

            await _seleniumService.LoadTonrichWebsiteAsync(driver, "EQCFLPL8WqFYzJuftSDrO-dxtK5JRt1zNK6PziXuDnHVdcpR", cancellationToken);
            WebDrivers?.Add((driverId, driver));
        }

        [HttpGet]
        public async Task<MemoryStream?> TakeScreenShootAsync(Guid driverId, string walletId, CancellationToken cancellationToken)
        {
            if (WebDrivers is null || !WebDrivers.Any(c => c.Id == driverId))
            {
                WebDrivers ??= new();
                await InitalDriver(driverId, cancellationToken);
            }

            var driver = WebDrivers.Where(c => c.Id == driverId).First().webDriver;
            return await _seleniumService.LoadTonrichWebsiteAsync(driver, walletId, cancellationToken);
        }
    }
}
