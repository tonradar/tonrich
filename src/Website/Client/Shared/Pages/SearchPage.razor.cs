using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Microsoft.AspNetCore.Components.Web;

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

        private void HandleOnKeyDownSearch(KeyboardEventArgs e)
        {
            if (e.Key == "Enter")
            {
                OnSearchWalletClick();
            }
        }
    }
}
