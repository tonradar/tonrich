using System.Globalization;
using Tonrich.Shared.Util;

namespace Tonrich.Client.Shared.Pages;

public partial class WalletPage
{
    [Parameter] public required string WalletId { get; set; }

    [AutoInject] public ITonService TonService { get; set; } = default!;

    public List<(int Number, string Name)> Months = new();
    public AccountInfoDto? AccountInfo { get; set; }
    public TransactionInfoDto? TransactionInfo { get; set; }
    public List<NFTDto>? NFTs { get; set; }
    public int UserNamesCount { get; set; }
    public int NumbersCount { get; set; }
    public decimal Worth { get; set; }
    public decimal NFTPrice { get; set; }
    //private bool isLoading = true;
    private bool isPageBusy = true;
    private bool isAccountBoxBusy = true;

    protected override async Task OnInitAsync()
    {
        isAccountBoxBusy = true;
        AccountInfo = await TonService.GetAccountInfoAsync(WalletId);
        isAccountBoxBusy = false;

        if (AccountInfo?.Address is null)
            return;

        FillMonths();


        _ = Task.Run(async () =>
        {
            await Task.WhenAll(
                     Task.Run(async () =>
                     {
                         TransactionInfo = await TonService.GetTransactionsAsync(AccountInfo.Raw);
                         await InvokeAsync(StateHasChanged);
                     }),
                     Task.Run(async () =>
                     {
                         NFTs = new List<NFTDto>();
                         var telegramNumbers = await TonService.GetNFTsAsync(AccountInfo.Raw, AppSetting.TelegramAnonymousCollectionAddress);
                         if (telegramNumbers != null)
                         {
                             NumbersCount = telegramNumbers.Count;
                             NFTs.AddRange(telegramNumbers);
                             NFTPrice += await TonService.GetNumbersPriceAsync(telegramNumbers.Select(c => c.Name)!);
                             await InvokeAsync(StateHasChanged);
                         }

                         var telegramUserNames = await TonService.GetNFTsAsync(AccountInfo.Raw, AppSetting.TelegramUserNameCollectionAddress);

                         if (telegramUserNames != null)
                         {
                             UserNamesCount = telegramUserNames.Count;
                             NFTs.AddRange(telegramUserNames);
                             NFTPrice += await TonService.GetUserNamesPriceAsync(telegramUserNames.Select(c => c.Name)!);
                             await InvokeAsync(StateHasChanged);
                         }
                     })
                 );

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

    public WalletActivityDto GetActivity(int week, DayOfWeek dayOfWeek)
    {
        var dateFromWeekOfYearAndDayOfWeek = GetDateFromWeekOfYearAndDayOfWeek(week, dayOfWeek);
        var activity = TransactionInfo?.Activities.FirstOrDefault(d => d.ActivityDate.Date == dateFromWeekOfYearAndDayOfWeek.Date);

        activity ??= new WalletActivityDto() { ActivityDate = dateFromWeekOfYearAndDayOfWeek, ActivityAmount = 0 };
        return activity;
    }
    public static string GetActivityColor(decimal? activityAmount)
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