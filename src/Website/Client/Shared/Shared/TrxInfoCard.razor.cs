namespace Tonrich.Client.Shared.Shared;

public partial class TrxInfoCard
{
    [Parameter] public string? IconName { get; set; }
    [Parameter] public string? Title { get; set; }
    [Parameter] public decimal? Value { get; set; }
    [Parameter] public string? Unit { get; set; }
    [Parameter] public string? Description { get; set; }
    [Parameter] public bool IsBusy { get; set; } = true;
    [Parameter] public string? OrderName { get; set; }
    [Parameter] public EventCallback<string> OnToggleTooltipClicked { get; set; }

    [Parameter] public bool IsTooltipOpen { get; set; }

    protected override void OnParametersSet()
    {
        base.OnParametersSet();
    }

    protected void ToggleTrxInfo()
    {
        OnToggleTooltipClicked.InvokeAsync(OrderName);
        IsTooltipOpen = !IsTooltipOpen;
    }

    private void CloseToolTip()
    {
        IsTooltipOpen = false;
        Console.WriteLine("Test");
    }
}
