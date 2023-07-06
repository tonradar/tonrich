using System.Globalization;
using Tonrich.Client.Shared.Shared;
using Tonrich.Shared.Util;
using static System.Net.Mime.MediaTypeNames;

namespace Tonrich.Client.Shared.Pages;

public partial class WalletPage
{
    [Parameter] public required string WalletId { get; set; }
    [Parameter] private string theme { get; set; } = "light";
    [AutoInject] public ITonService TonService { get; set; } = default!;

    public List<(int Number, string Name)> Months = new();
    public AccountInfoDto? AccountInfo { get; set; }
    public TransactionInfoDto? TransactionInfo { get; set; }
    public List<NFTDto>? NFTs { get; set; }
    public int? UserNamesCount { get; set; }
    public int? NumbersCount { get; set; }
    public decimal? Worth { get; set; }
    public decimal? NFTPrice { get; set; }
    private bool isPageBusy = false;
    private bool isAccountBoxBusy = true;
    int[] loadingUints = { 11, 12, 13, 14 };
    private bool isDiagramBusy = true;
    private string? ToolTipCallerOrderName { get; set; }
    private bool IsOpendToolTip1 { get; set; }
    private List<IGrouping<DayOfWeek, (int WeekInMonth, DateTimeOffset DateTimeOffset)>> ActivityChartDates { get; set; } = default!;
    protected override void OnInitialized()
    {
        var query = new Uri(NavigationManager.Uri).Query;
        if (!string.IsNullOrWhiteSpace(query))
        {
            var parameters = query.Replace("?", "").Split('&');
            foreach (var param in parameters)
            {
                if (param.StartsWith("theme"))
                {
                    theme = param.Split('=')[1];
                }
            }
        }

        FillMonths();
        ActivityChartDates = GenerateActivityChartDates();
        base.OnInitialized();

    }

    protected override async Task OnInitAsync()
    {
        isAccountBoxBusy = true;
        AccountInfo = await TonService.GetAccountInfoAsync(WalletId);
        isAccountBoxBusy = false;

        if (AccountInfo?.Address is null)
            return;

        _ = Task.Run(async () =>
        {
            isAccountBoxBusy = true;
            await Task.WhenAll(
                     Task.Run(async () =>
                     {
                         TransactionInfo = await TonService.GetTransactionsAsync(AccountInfo.Raw);
                         await InvokeAsync(StateHasChanged);
                     }),
                     Task.Run(async () =>
                     {
                         var telegramNumbers = await TonService.GetNFTsAsync(AccountInfo.Raw, AppSetting.TelegramAnonymousCollectionAddress);
                         if (telegramNumbers != null)
                         {
                             NumbersCount = telegramNumbers.Count;
                             NFTs ??= new();
                             NFTs.AddRange(telegramNumbers);
                             NFTPrice ??= 0;
                             NFTPrice += await TonService.GetNumbersPriceAsync(telegramNumbers.Select(c => c.Name)!);
                             await InvokeAsync(StateHasChanged);
                         }

                         var telegramUserNames = await TonService.GetNFTsAsync(AccountInfo.Raw, AppSetting.TelegramUserNameCollectionAddress);

                         if (telegramUserNames != null)
                         {
                             UserNamesCount = telegramUserNames.Count;
                             NFTs ??= new();
                             NFTs.AddRange(telegramUserNames);
                             NFTPrice ??= 0;
                             NFTPrice += await TonService.GetUserNamesPriceAsync(telegramUserNames.Select(c => c.Name)!);
                             await InvokeAsync(StateHasChanged);
                         }
                     })
                 );
            isDiagramBusy = false;

            Worth = NFTPrice + AccountInfo?.Balance ?? 0;

            await InvokeAsync(StateHasChanged);
        });

        await base.OnInitAsync();
    }

    protected override async Task OnAfterRenderAsync(bool firstRender)
    {
        if (AccountInfo?.Address is null && isAccountBoxBusy == false)
        {
            NavigationManager.NavigateTo("/WalletNotFound", true);
        }

        await base.OnAfterRenderAsync(firstRender);
    }

    private void HandleToggleTooltipClicked(string toolTipCallerOrderName)
    {
        ToolTipCallerOrderName = toolTipCallerOrderName;
    }

    private async Task CopyAsync()
    {
        await JSRuntime.InvokeVoidAsync("window.App.copy", AccountInfo?.Address);
    }

    private static string GetActivityColor(decimal? activityAmount)
    {
        return activityAmount switch
        {
            decimal value when value == 0 => "zero",
            decimal value when value > 0 && value <= 10 => "one",
            decimal value when value > 10 && value <= 100 => "two",
            decimal value when value > 100 && value <= 1000 => "three",
            decimal value when value > 1000 => "four",
            _ => "zero",
        };
    }

    private WalletActivityDto GetActivity(DateTimeOffset dateTimeOffset)
    {
        var activity = TransactionInfo?.Activities.FirstOrDefault(d => d.ActivityDate.Date == dateTimeOffset.Date);

        activity ??= new WalletActivityDto() { ActivityDate = dateTimeOffset, ActivityAmount = 0 };
        return activity;
    }

    private static List<IGrouping<DayOfWeek, (int WeekInMonth, DateTimeOffset DateTime)>> GenerateActivityChartDates()
    {
        var now = DateTimeOffset.Now;
        int daysUntilFriday = ((int)DayOfWeek.Friday - (int)now.DayOfWeek + 7) % 7;
        var maxDate = now.AddDays(daysUntilFriday);
        var minDate = maxDate.AddDays(-AppSetting.TransactionsTimePeriodPerDays + 1);

        var dates = new List<(int WeekInMonth, DateTimeOffset DateTime)>();

        for (var date = minDate; date <= maxDate; date = date.AddDays(1))
        {
            var weekInMonth = (date.Day - 1) / 7 + 1;
            dates.Add((weekInMonth, date));
        }

        return dates.GroupBy(w => w.DateTime.DayOfWeek).ToList();
    }
    private void FillMonths()
    {
        var lastSixMonths = Enumerable.Range(0, 6)
            .Select(i => DateTimeOffset.Now.AddMonths(-i))
            .OrderBy(m => m)
            .Select(m => new { m.Month, MonthName = m.ToString("MMM") }).ToList();

        lastSixMonths.ForEach(m => Months.Add((m.Month, m.MonthName)));
    }

    private static bool ShowWeekTitle(DayOfWeek dayOfWeek)
    {
        return dayOfWeek switch
        {
            DayOfWeek.Monday => true,
            DayOfWeek.Wednesday => true,
            DayOfWeek.Friday => true,
            _ => false
        };
    }

    static DateTimeOffset GetDateFromWeekOfYearAndDayOfWeek(int weekOfYear, DayOfWeek dayOfWeek)
    {
        DateTimeOffset jan1 = new(DateTimeOffset.Now.Year, 1, 1, 0, 0, 0, TimeSpan.Zero);
        int daysOffset = (int)dayOfWeek - (int)jan1.DayOfWeek;
        DateTimeOffset firstThursday = jan1.AddDays(daysOffset);

        Calendar calendar = CultureInfo.CurrentCulture.Calendar;
        int firstWeek = calendar.GetWeekOfYear(firstThursday.DateTime,
            CalendarWeekRule.FirstFullWeek,
            DayOfWeek.Sunday);

        if (firstWeek <= 1)
        {
            weekOfYear -= 1;
        }

        DateTimeOffset result = firstThursday.AddDays(weekOfYear * 7);

        return result.AddDays((int)dayOfWeek - (int)result.DayOfWeek);
    }

    private string GetNftBarInfoTitle()
    {
        return $"NFT({NumbersCount} Numbers + {UserNamesCount} Usernames)";
    }
}