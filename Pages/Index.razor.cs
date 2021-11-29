// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Karma.Models;
using Microsoft.AspNetCore.Components;
using Microsoft.EntityFrameworkCore;

namespace Karma.Pages
{
    public partial class Index
    {

        private int m_totalPageQuantity;
        private int m_currentPage = 1;
        public int perPage = 10;
        public string filterValue = "";
        public IEnumerable<IGenericKarmaItem> karmaEvents;

        [Inject]
        private IDbContextFactory<KarmaContext> m_contextFactory{ get; set; }

        private KarmaContext m_karmaContext;

        protected override void OnInitialized()
        {
            m_karmaContext = m_contextFactory.CreateDbContext();
            LoadEvents();
        }

        public void SelectedPage(int page)
        {
            m_currentPage = page;
            LoadEvents(page, perPage);
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
            var result = m_karmaContext.Events.ToList();
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
