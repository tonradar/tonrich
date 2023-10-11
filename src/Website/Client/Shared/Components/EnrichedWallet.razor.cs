using Microsoft.AspNetCore.Components.Web;
using System.Text.Json;
using Tonrich.Shared.Util;

namespace Tonrich.Client.Shared.Components;

public partial class EnrichedWallet
{
    [Parameter] public required string WalletId { get; set; }
    [Parameter] public EventCallback<bool> OnAccountLoad { get; set; }

    [AutoInject] public ITonService TonService { get; set; } = default!;

    public List<(int Number, string Name)> Months = new();
    public AccountInfoDto? AccountInfo { get; set; }
    public TransactionInfoDto? TransactionInfo { get; set; }
    public List<NFTDto>? NFTs { get; set; }
    public int? UserNamesCount { get; set; }
    public int? NumbersCount { get; set; }
    public decimal? Worth { get; set; }
    public decimal? NFTPrice { get; set; }
    private readonly bool isPageBusy = false;
    readonly int[] loadingUints = { 11, 12, 13, 14 };
    private bool isDiagramBusy = true;
    private string? ToolTipCallerOrderName { get; set; }
    public string copyTooltipPosition = "";
    private bool IsWalletNotFound { get; set; } = false;

    private List<IGrouping<DayOfWeek, (int WeekInMonth, DateTimeOffset DateTimeOffset)>> ActivityChartDates { get; set; } = default!;
    protected override void OnInitialized()
    {
        FillMonths();
        ActivityChartDates = GenerateActivityChartDates();
        base.OnInitialized();
    }

    protected override async Task OnAfterFirstRenderAsync()
    {
        await base.OnAfterFirstRenderAsync();

        AccountInfo = await TonService.GetAccountInfoAsync(WalletId);

        if (AccountInfo?.Address is null)
        {
            IsWalletNotFound = true;
            isDiagramBusy = false;
            await OnAccountLoad.InvokeAsync(true);
            await InvokeAsync(StateHasChanged);
            return;
        }

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
                         var response = await HttpClient.PostAsJsonAsync<IEnumerable<string>>("NFT/GetNumbersPrice", telegramNumbers.Select(c => c.Name)!);
                         if (response != null)
                         {
                             var content = await response.Content.ReadAsStreamAsync();
                             NFTPrice += await JsonSerializer.DeserializeAsync<decimal>(content);
                         }
                         await InvokeAsync(StateHasChanged);
                     }

                     var telegramUserNames = await TonService.GetNFTsAsync(AccountInfo.Raw, AppSetting.TelegramUserNameCollectionAddress);

                     if (telegramUserNames != null)
                     {
                         UserNamesCount = telegramUserNames.Count;
                         NFTs ??= new();
                         NFTs.AddRange(telegramUserNames);
                         NFTPrice ??= 0;
                         var response = await HttpClient.PostAsJsonAsync<IEnumerable<string>>("NFT/GetUserNamesPrice", telegramUserNames.Select(c => c.Name)!);
                         if (response != null)
                         {
                             var content = await response.Content.ReadAsStreamAsync();
                             NFTPrice += await JsonSerializer.DeserializeAsync<decimal>(content);
                         }
                         await InvokeAsync(StateHasChanged);
                     }
                 })
             );
        isDiagramBusy = false;

        Worth = NFTPrice + AccountInfo?.Balance ?? 0;
        await OnAccountLoad.InvokeAsync(true);
        await InvokeAsync(StateHasChanged);
    }

    private void MouseMove(MouseEventArgs e)
    {
        copyTooltipPosition = $"top: {e.ClientY + 15}px; left: {e.ClientX + 15}px;";
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
}
