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
using Microsoft.AspNetCore.Components.Web;
using Microsoft.EntityFrameworkCore;
using Microsoft.JSInterop;
using Z.EntityFramework.Plus;

namespace Karma.Pages
{
    public partial class EditCharityEvent
    {
        [Parameter]
        public Guid Id { get; set; }
        [Inject]
        public IJSRuntime m_jsRuntime { get; set; }
        private KarmaContext m_karmaContext = new();
        public CharityEvent charityEvent;
        public string filterValue = "";
        public string errorMessage = "";
        public int VolunteerCount { get; set; }
        public bool panelOpenState1 = true;
        public bool panelOpenState2 = true;
        public string CurrentUserId { get; set; }

        private void OnFocusHandler(FocusEventArgs e)
        {
            errorMessage = "";
        }

        private void OnFocusOutHandler(FocusEventArgs e)
        {
            int value = VolunteerCount;
            if (value > 0)
            {
                if (value < charityEvent.Volunteers.Count)
                    errorMessage = "Illegal volunteer count, please choose a value greater than currently assigned volunteeer count or reduce the number of volunteers in this event.";
                else
                    charityEvent.MaxVolunteers = value;
            }
            else
            {
                errorMessage = "Illegal volunteer count, please choose a positive number.";
            }
        }

        public void UpdateEvent()
        {
            if (ObjectChecker.IsAnyNullOrEmpty(charityEvent))
            {
                m_notifactionTransmitter.ShowMessage("There are some empty fields", MatToastType.Danger);
            }
            else
            {
                try
                {
                    m_karmaContext.Events.Update(charityEvent);
                    m_karmaContext.SaveChanges();
                    m_uriHelper.NavigateTo("");
                    m_notifactionTransmitter.ShowMessage("The event has been updated", MatToastType.Success);
                }
                catch (DbUpdateException)
                {
                    m_notifactionTransmitter.ShowMessage("An error occured while updating the event", MatToastType.Danger);
                }
            }
        }

        public void DeleteEvent()
        {
            m_karmaContext.Events.Where(x => x.Id == Id).Delete();
            m_karmaContext.SaveChanges();
            m_uriHelper.NavigateTo("");
            m_notifactionTransmitter.ShowMessage("The event has been deleted", MatToastType.Success);
        }

        protected override void OnInitialized()
        {
            charityEvent = m_karmaContext.Events.Include(p => p.Volunteers).Where(p => p.Id == Id).FirstOrDefault();
            ClaimsPrincipal principal = m_httpContextAccessor.HttpContext.User;
            CurrentUserId = principal.FindFirstValue(ClaimTypes.NameIdentifier);
            VolunteerCount = charityEvent.Volunteers.Count;
        }

        public IEnumerable<IVolunteer> GetVolunteersInThisEvent()
        {
            return m_karmaContext.Events.Include(p => p.Volunteers).Where(p => p.Id == Id).Select(p => p.Volunteers).SelectMany(p => p);
        }

        public IEnumerable<IVolunteer> GetVolunteersNotInThisEvent()
        {
            return m_karmaContext.Volunteers.Include(p => p.Events).Where(p => !p.Events.Contains(charityEvent));
        }

        public void AddVolunteerToEvent(Guid id)
        {
            charityEvent.Volunteers.Add(m_karmaContext.Volunteers.Where(p => p.Id == id).FirstOrDefault());
            m_karmaContext.SaveChanges();
        }

        public void RemoveVolunteerFromEvent(Guid id)
        {
            charityEvent.Volunteers.Remove(m_karmaContext.Volunteers.Where(p => p.Id == id).FirstOrDefault());
            m_karmaContext.SaveChanges();
        }

        public int GetNumberOfEquipmentOfThisVolunteer(Guid volunteerId)
        {
            return m_karmaContext.SpecialEquipment.Include(p => p.Owner).Where(p => p.Owner.Id == volunteerId).Count();
        }
    }
}
