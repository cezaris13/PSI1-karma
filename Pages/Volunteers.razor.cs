// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using Karma.Models;
using Karma.Services;
using MatBlazor;
using Microsoft.AspNetCore.Components;
using Microsoft.EntityFrameworkCore;

namespace Karma.Pages
{
    public partial class Volunteers
    {
        [Inject]
        private NavigationManager m_navigationManager { get; set; }

        [Inject]
        private IKarmaContextFactory m_karmaContextFactory { get; set; }

        public string FilterValue = "";

        private KarmaContext m_karmaContext;

        string CurrentUserId { get; set; }

        public IEnumerable<IVolunteer> GetVolunteers()
        {
            return m_karmaContext.Volunteers.Include(x => x.Events);
        }

        public void NavigateToIndividualEvent(Guid id)
        {
            m_navigationManager.NavigateTo($"event/{id}");
        }

        public void NavigateToAddVolunteer()
        {
            m_navigationManager.NavigateTo("addvolunteer");
        }

        public void RemoveVolunteer(Guid id)
        {
            m_karmaContext.Volunteers.Where(x => x.Id == id).DeleteFromQuery();
            m_karmaContext.SaveChanges();
            m_notificationTransmitter.ShowMessage("The volunteer has been removed", MatToastType.Success);
        }

        public void NavigateToManageVolunteer(Guid id)
        {
            m_navigationManager.NavigateTo($"volunteer/{id}");
        }

        protected override void OnInitialized()
        {
            m_karmaContext = m_karmaContextFactory.Create();
            ClaimsPrincipal principal = m_httpContextAccessor.HttpContext.User;
            CurrentUserId = principal.FindFirstValue(ClaimTypes.NameIdentifier);
        }
    }
}
