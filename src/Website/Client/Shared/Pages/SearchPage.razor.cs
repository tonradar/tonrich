using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tonrich.Client.Shared.Pages
{
    public partial class SearchPage
    {
        private string? SearchWalletText { get; set; }

        private void OnSearchWalletClick()
        {
            if (string.IsNullOrWhiteSpace(SearchWalletText))
                return;

            NavigationManager.NavigateTo($"/wallet/{SearchWalletText}");
        }
    }
}
