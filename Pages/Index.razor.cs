// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Karma.Models;

namespace Karma.Pages
{
    public partial class Index
    {

        private int m_totalPageQuantity;
        private int m_currentPage = 1;
        public int perPage = 5;
        public string filterValue = "";
        public IEnumerable<IGenericKarmaItem> karmaEvents;

        private KarmaContext m_karmaContext = new();

        protected override void OnInitialized()
        {
            LoadEvents(elementsPerPage: perPage);
        }

        public void SelectedPage(int page)
        {
            m_currentPage = page;
            LoadEvents(page, perPage);
        }

        public IEnumerable<IGenericKarmaItem> SearchForEvents()
        {
            LoadEvents(m_currentPage, perPage);
            return karmaEvents;
        }

        public async Task Refresh(int pageSize)
        {
            perPage = pageSize;
            LoadEvents(elementsPerPage: pageSize);
            try
            {
                await InvokeAsync(StateHasChanged);
            }
            catch { }
        }

        private void LoadEvents(int page = 1, int elementsPerPage = 10)
        {
            var temp = m_karmaContext.Events.ToList();
            List<CharityEvent> result = new List<CharityEvent>();
            foreach (var karmaEvent in temp)
            {
                var regex = new Regex($"(.*){filterValue}(.*)", RegexOptions.Compiled | RegexOptions.IgnoreCase);
                var matches = regex.Matches(karmaEvent.Name + karmaEvent.Description + karmaEvent.Id.ToString());
                if (matches.Count > 0)
                {
                    result.Add(karmaEvent);
                }
            }
            m_totalPageQuantity = Convert.ToInt32(Math.Ceiling(result.Count / (double) elementsPerPage));
            result.Sort();
            karmaEvents = result.Skip((page - 1) * elementsPerPage).Take(elementsPerPage);
        }

        public void NavigateToIndividualEvent(Guid id)
        {
            m_uriHelper.NavigateTo($"event/{id}");
        }
    }
}
