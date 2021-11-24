// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Karma.Models;

namespace Karma.Pages
{
    public partial class ManageUserEvents
    {
        private int m_totalPageQuantity;
        private int m_currentPage = 1;
        public int perPage;
        public string filterValue = "";
        private string m_currentUserId { get; set; }

        public IEnumerable<IGenericKarmaItem> karmaEvents;

        private KarmaContext m_karmaContext = new();

        public void NavigateToIndividualEvent(Guid id)
        {
            m_uriHelper.NavigateTo($"event/{id}");
        }

        public void NavigateToEditEvent(Guid id)
        {
            m_uriHelper.NavigateTo($"editcharityevent/{id}");
        }

        public void SelectedPage(int page)
        {
            m_currentPage = page;
            LoadEvents(page, perPage);
        }

        private void LoadEvents(int page = 1, int elementsPerPage = 10)
        {
            var result = m_karmaContext.Events.Where(p => p.ManagerId == m_currentUserId).ToList();
            m_totalPageQuantity = Convert.ToInt32(Math.Ceiling(result.Count / (double) elementsPerPage));
            result.Sort();
            karmaEvents = result.Skip((page - 1) * elementsPerPage).Take(elementsPerPage);
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

        protected override void OnInitialized()
        {
            ClaimsPrincipal principal = m_httpContextAccessor.HttpContext.User;
            m_currentUserId = principal.FindFirstValue(ClaimTypes.NameIdentifier);
            LoadEvents();
        }
    }
}
