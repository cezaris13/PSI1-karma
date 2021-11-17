using System.Collections.Generic;
using System.Threading.Tasks;
using Karma.Models;
using Microsoft.AspNetCore.Components;

namespace Karma.Pages
{
    public partial class Pagination
    {
        [Parameter] public int CurrentPage { get; set; } = 1;
        [Parameter] public int TotaPagesQuantity { get; set; }
        [Parameter] public int Radius { get; set; } = 3;
        [Parameter] public EventCallback<int> SelectedPage { get; set; }
        public List<LinkModel> links;

        protected override void OnParametersSet()
        {
            LoadPages();
        }

        private async Task SelectedPageInternal(LinkModel link)
        {
            if (link.Page == CurrentPage || !link.Enabled)
                return;

            CurrentPage = link.Page;
            await SelectedPage.InvokeAsync(link.Page);
        }

        private void LoadPages()
        {
            links = new List<LinkModel>();
            bool isPreviousPageLinkEnabled = CurrentPage != 1;
            int previousPage = CurrentPage - 1;
            links.Add(new LinkModel(previousPage, isPreviousPageLinkEnabled, "Previous"));

            for (int i = 1; i <= TotaPagesQuantity; i++)
            {
                if (i >= CurrentPage - Radius && i <= CurrentPage + Radius)
                    links.Add(new LinkModel(i) { Active = CurrentPage == i });
            }

            bool isNextPageLinkEnabled = CurrentPage != TotaPagesQuantity;
            int nextPage = CurrentPage + 1;
            links.Add(new LinkModel(nextPage, isNextPageLinkEnabled, "Next"));
        }
    }
}
