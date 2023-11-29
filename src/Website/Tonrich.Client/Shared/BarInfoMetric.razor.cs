namespace Tonrich.Client.Shared;

public partial class BarInfoMetric
{
    [Parameter]
    public string? Title { get; set; }
    [Parameter]
    public string? TitleDescription { get; set; }
    [Parameter]
    public decimal? Value { get; set; }
    [Parameter]
    public string? ValueDescription { get; set; }
    [Parameter]
    public string? BarColor { get; set; }
    [Parameter]
    public string? TooltipText { get; set; }
}
