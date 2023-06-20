using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tonrich.Client.Shared.Shared
{
    public partial class TrxInfoCard
    {
        [Parameter] public string? IconName { get; set; }
        [Parameter] public string? Title { get; set; }
        [Parameter] public decimal? Value { get; set; }
        [Parameter] public string? Unit { get; set; }
        [Parameter] public string? Description { get; set; }
        [Parameter] public bool IsBusy { get; set; } = true;
        [Parameter] public EventCallback<bool> ToggleTooltipClicked { get; set; }

        private bool IsTooltipOpen = false;

        protected void ToggleTrxInfo() {
            IsTooltipOpen = !IsTooltipOpen;
        }
    }
}
