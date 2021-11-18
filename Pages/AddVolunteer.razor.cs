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
using Microsoft.JSInterop;

namespace Karma.Pages
{
    public partial class AddVolunteer
    {
        [Inject]
        public IJSRuntime m_jsRuntime { get; set; }

        [Inject]
        public IDBServiceProvider m_DBServiceProvider { get; set; }

        [Inject]
        public IKarmaContextFactory m_karmaContextFactory { get; set; }

        public string VolunteerName { get; set; }
        public string VolunteerSurname { get; set; }
        public List<CharityEvent> listOfCharityEvents = new();
        private KarmaContext m_karmaContext;
        public bool panelOpenState1 = true;
        public bool panelOpenState2 = true;
        public string CurrentUserId { get; set; }

        protected override void OnInitialized()
        {
            m_karmaContext = m_karmaContextFactory.Create();
            ClaimsPrincipal principal = m_httpContextAccessor.HttpContext.User;
            CurrentUserId = principal.FindFirstValue(ClaimTypes.NameIdentifier);
        }

        public void AddVolunteerToDB()
        {
            var volunteer = new Volunteer(VolunteerName, VolunteerSurname, Guid.NewGuid(), listOfCharityEvents);
            int result = m_DBServiceProvider.AddToDB(volunteer);
            if (result == 0)
                m_uriHelper.NavigateTo("/volunteers");
            else if (result == -1)
                m_notificationTransmitter.ShowMessage("An error occured while adding volunteer to the database", MatToastType.Danger);
        }

        public IEnumerable<ICharityEvent> GetEventsOfThisVolunteer()
        {
            return listOfCharityEvents;
        }

        public IEnumerable<ICharityEvent> GetEventsNotOfThisVolunteer()
        {
            return m_karmaContext.Events.Include(p => p.Volunteers);
        }

        public void AddEventToVolunteerList(Guid id)
        {
            listOfCharityEvents.Add(m_karmaContext.Events.Where(p => p.Id == id).FirstOrDefault());
        }

        public void RemoveEventFromList(Guid id)
        {
            listOfCharityEvents.Remove(m_karmaContext.Events.Where(p => p.Id == id).FirstOrDefault());
        }
    }
}
