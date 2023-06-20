﻿using Tonrich.Shared.Util;

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
    private bool isLoading = true;
    public List<IGrouping<DayOfWeek, (int WeekInMonth, DateTimeOffset DateTimeOffset)>> ActivityChartDates { get; set; } = default!;

    protected override void OnInitialized()
    {
        FillMonths();
        ActivityChartDates = GenerateActivityChartDates();
        base.OnInitialized();
    }
    protected override async Task OnInitAsync()
    {
        isLoading = true;
        AccountInfo = await TonService.GetAccountInfoAsync(WalletId);
        isLoading = false;

        if (AccountInfo?.Address is null)
            return;

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
        if (AccountInfo?.Address is null && isLoading == false)
        {
            NavigationManager.NavigateTo("/WalletNotFound", true);
        }

        await base.OnAfterRenderAsync(firstRender);
    }

    private static string GetActivityColor(decimal? activityAmount)
    {
        return activityAmount switch
        {
            decimal value when value == 0 => $"#EBEDF0",
            decimal value when value > 0 && value <= 10 => $"#9BE9A8",
            decimal value when value > 10 && value <= 100 => $"#40C463",
            decimal value when value > 100 && value <= 1000 => $"#30AB44",
            decimal value when value > 1000 => $"#216E39",
            _ => $"#EBEDF0",
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

    private void FillMonths()
    {
        var lastSixMonths = Enumerable.Range(0, 6)
            .Select(i => DateTimeOffset.Now.AddMonths(-i))
            .OrderBy(m => m)
            .Select(m => new { m.Month, MonthName = m.ToString("MMM") }).ToList();

        lastSixMonths.ForEach(m => Months.Add((m.Month, m.MonthName)));
    }

    private string GetNftBarInfoTitle()
    {
        return $"NFT({NumbersCount} Numbers + {UserNamesCount} Usernames)";
    }
}