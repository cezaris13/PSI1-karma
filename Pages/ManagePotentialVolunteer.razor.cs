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
    public partial class ManagePotentialVolunteer
    {
        [Parameter]
        public Guid Id { get; set; }

        [Inject]
        public IJSRuntime m_jsRuntime { get; set; }

        [Inject]
        public IDBServiceProvider m_DBServiceProvider { get; set; }

        [Inject]
        public IKarmaContextFactory m_karmaContextFactory { get; set; }

        public PendingVolunteer potentialVolunteer;
        private KarmaContext m_karmaContext;
        private string FilterValue { get; set; } = "";
        public string CurrentUserId { get; set; }
        public string EquipmentName { get; set; }
        public bool panelOpenState1 = true;
        public bool panelOpenState2 = true;
        public bool panelOpenState3 = true;
        public List<CharityEvent> listOfCharityEvents = new();
        public List<ISpecialEquipment> listOfEquipment = new();

        protected override void OnInitialized()
        {
            m_karmaContext = m_karmaContextFactory.Create();
            potentialVolunteer = m_karmaContext.PendingVolunteers.Where(p => p.Id == Id).FirstOrDefault();
            ClaimsPrincipal principal = m_httpContextAccessor.HttpContext.User;
            CurrentUserId = principal.FindFirstValue(ClaimTypes.NameIdentifier);
        }

        public void AddVolunteerToDB()
        {
            var volunteer = new Volunteer(potentialVolunteer.Name, potentialVolunteer.Surname, Guid.NewGuid(), listOfCharityEvents);
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

        public IEnumerable<ISpecialEquipment> GetEquipmentOfThisVolunteer()
        {
            return listOfEquipment;
        }

        private void RemoveEquipment(ISpecialEquipment equipment)
        {
            listOfEquipment.Remove(equipment);
        }

        private void AddNewEquipmentToVolunteer()
        {
            var equipment = new SpecialEquipment(Guid.NewGuid(), EquipmentName);
            listOfEquipment.Add(equipment);
            EquipmentName = string.Empty;
        }

        public void AddANewVolunteer()
        {
            var volunteer = new Volunteer(potentialVolunteer.Name, potentialVolunteer.Surname, Guid.NewGuid(), listOfCharityEvents);
            if (listOfEquipment.Count > 0)
            {
                foreach (var equipment in listOfEquipment)
                {
                    equipment.Owner = volunteer;
                    m_karmaContext.SpecialEquipment.Add((SpecialEquipment) equipment);
                }
            }
            int result = m_DBServiceProvider.AddToDB(volunteer);
            if (result == 0)
                m_uriHelper.NavigateTo("/volunteers");
            else if (result == -1)
                m_notificationTransmitter.ShowMessage("An error occured while adding volunteer to the database", MatToastType.Danger);
            m_karmaContext.PendingVolunteers.Where(x => x.Id == potentialVolunteer.Id).DeleteFromQuery();
            m_karmaContext.SaveChanges();
        }

        public void RemovePotentialVolunteer()
        {
            m_karmaContext.PendingVolunteers.Where(x => x.Id == potentialVolunteer.Id).DeleteFromQuery();
            m_karmaContext.SaveChanges();
            m_notificationTransmitter.ShowMessage("The request has been deleted", MatToastType.Success);
        }
    }
}
