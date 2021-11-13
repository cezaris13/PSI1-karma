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

        public string VolunteerName { get; set; }
        public string VolunteerSurname { get; set; }
        public List<CharityEvent> listOfCharityEvents = new();
        private KarmaContext m_karmaContext = new();
        public bool panelOpenState1 = true;
        public bool panelOpenState2 = true;
        public string CurrentUserId { get; set; }

        protected override void OnInitialized()
        {
            ClaimsPrincipal principal = m_httpContextAccessor.HttpContext.User;
            CurrentUserId = principal.FindFirstValue(ClaimTypes.NameIdentifier);
        }

        public void AddVolunteerToDB()
        {
            var vol = new Volunteer(VolunteerName, VolunteerSurname, Guid.NewGuid(), listOfCharityEvents);
            if (ObjectChecker.IsAnyNullOrEmpty(vol))
            {
                m_notifactionTransmitter.ShowMessage("There are some empty fields", MatToastType.Danger);
            }
            else
            {
                m_karmaContext.Volunteers.Add(vol);
                m_karmaContext.SaveChanges();
                m_uriHelper.NavigateTo("/volunteers");
                m_notifactionTransmitter.ShowMessage("The volunteer has been added", MatToastType.Success);
            }
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
