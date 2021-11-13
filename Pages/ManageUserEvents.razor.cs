// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using Karma.Models;

namespace Karma.Pages
{
    public partial class ManageUserEvents
    {
        public string filterValue = "";
        private string m_currentUserId { get; set; }

        private KarmaContext m_karmaContext = new KarmaContext();

        public IEnumerable<IGenericKarmaItem> GetEvents()
        {
            return m_karmaContext.Events.Where(p => p.ManagerId == m_currentUserId);
        }

        public void NavigateToIndividualEvent(Guid id)
        {
            m_uriHelper.NavigateTo($"event/{id}");
        }

        public void NavigateToEditEvent(Guid id)
        {
            m_uriHelper.NavigateTo($"editcharityevent/{id}");
        }

        protected override void OnInitialized()
        {
            ClaimsPrincipal principal = m_httpContextAccessor.HttpContext.User;
            m_currentUserId = principal.FindFirstValue(ClaimTypes.NameIdentifier);
        }
    }
}
