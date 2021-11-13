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
    public partial class ManageIndividualVolunteer
    {
        [Parameter]
        public Guid Id { get; set; }
        [Inject]
        public IJSRuntime m_jsRuntime { get; set; }
        public Volunteer volunteer;
        private KarmaContext m_karmaContext = new();
        private string FilterValue { get; set; } = "";
        public string CurrentUserId { get; set; }
        public string EquipmentName { get; set; }
        public bool panelOpenState1 = true;
        public bool panelOpenState2 = true;
        public bool panelOpenState3 = true;

        private void UpdateVolunteerData()
        {
            if (ObjectChecker.IsAnyNullOrEmpty(volunteer))
            {
                m_notifactionTransmitter.ShowMessage("There are some empty fields", MatToastType.Danger);
            }
            else
            {
                m_karmaContext.Volunteers.Update(volunteer);
                m_karmaContext.SaveChanges();
                m_uriHelper.NavigateTo("/volunteers");
                m_notifactionTransmitter.ShowMessage("The volunteer has been updated", MatToastType.Success);
            }
        }

        protected override void OnInitialized()
        {
            volunteer = m_karmaContext.Volunteers.Include(p => p.Events).Where(p => p.Id == Id).FirstOrDefault();
            ClaimsPrincipal principal = m_httpContextAccessor.HttpContext.User;
            CurrentUserId = principal.FindFirstValue(ClaimTypes.NameIdentifier);
        }

        public IEnumerable<ICharityEvent> GetEventsOfThisVolunteer()
        {
            return m_karmaContext.Volunteers.Include(p => p.Events).Where(p => p.Id == Id).Select(p => p.Events).SelectMany(p => p);
        }

        public IEnumerable<ICharityEvent> GetEventsNotOfThisVolunteer()
        {
            return m_karmaContext.Events.Include(p => p.Volunteers).Where(p => !p.Volunteers.Contains(volunteer));
        }

        private void AddEventToVolunteerList(Guid id)
        {
            volunteer.Events.Add(m_karmaContext.Events.Where(p => p.Id == id).FirstOrDefault());
            m_karmaContext.SaveChanges();
        }

        private void RemoveEventFromList(Guid id)
        {
            volunteer.Events.Remove(m_karmaContext.Events.Where(p => p.Id == id).FirstOrDefault());
            m_karmaContext.SaveChanges();
        }

        public IEnumerable<ISpecialEquipment> GetEquipmentOfThisVolunteer()
        {
            return m_karmaContext.SpecialEquipment.Include(p => p.Owner).Where(p => p.Owner.Id == Id);
        }

        private void RemoveEquipment(Guid id)
        {
            m_karmaContext.Remove(m_karmaContext.SpecialEquipment.Where(p => p.Id == id).FirstOrDefault());
            m_karmaContext.SaveChanges();
        }

        private void AddNewEquipmentToVolunteer()
        {
            var equipment = new Karma.Models.SpecialEquipment(Guid.NewGuid(), EquipmentName, volunteer);
            m_karmaContext.SpecialEquipment.Add(equipment);
            EquipmentName = string.Empty;
            m_karmaContext.SaveChanges();
        }
    }
}
