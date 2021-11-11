// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using Karma.Models;
using MatBlazor;
using Microsoft.EntityFrameworkCore;

namespace Karma.Pages
{
    public partial class Volunteers
    {
        public string FilterValue = "";

        private KarmaContext m_karmaContext = new();

        string CurrentUserId { get; set; }

        public IEnumerable<IVolunteer> GetVolunteers()
        {
            return m_karmaContext.Volunteers.Include(x => x.Events);
        }

        public void NavigateToIndividualEvent(Guid id)
        {
            m_uriHelper.NavigateTo($"event/{id}");
        }

        public void NavigateToAddVolunteer()
        {
            m_uriHelper.NavigateTo("addvolunteer");
        }

        public void RemoveVolunteer(Guid id)
        {
            m_karmaContext.Volunteers.Where(x => x.Id == id).DeleteFromQuery();
            m_karmaContext.SaveChanges();
            m_notifactionTransmitter.ShowMessage("The volunteer has been removed", MatToastType.Success);
        }

        public void NavigateToManageVolunteer(Guid id)
        {
            m_uriHelper.NavigateTo($"volunteer/{id}");
        }

        protected override void OnInitialized()
        {
            ClaimsPrincipal principal = m_httpContextAccessor.HttpContext.User;
            CurrentUserId = principal.FindFirstValue(ClaimTypes.NameIdentifier);
        }
    }
}
