using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using Karma.Models;
using Karma.Services;
using MatBlazor;
using Microsoft.AspNetCore.Components;

namespace Karma.Pages
{
    public partial class PendingVolunteersList
    {
        [Inject]
        public IDBServiceProvider m_DBServiceProvider { get; set; }

        public string FilterValue = "";

        private KarmaContext m_karmaContext = new();

        string CurrentUserId { get; set; }

        protected override void OnInitialized()
        {
            ClaimsPrincipal principal = m_httpContextAccessor.HttpContext.User;
            CurrentUserId = principal.FindFirstValue(ClaimTypes.NameIdentifier);
        }

        public IEnumerable<PendingVolunteer> GetPendingVolunteers()
        {
            return m_karmaContext.PendingVolunteers;
        }

        public void AddANewVolunteer(Guid id)
        {
            PendingVolunteer pendingVolunteer = m_karmaContext.PendingVolunteers.Where(x => x.Id == id).FirstOrDefault();
            var volunteer = new Volunteer(pendingVolunteer.Name, pendingVolunteer.Surname, Guid.NewGuid());
            int result = m_DBServiceProvider.AddToDB(volunteer);
            if (result == 0)
                m_notifactionTransmitter.ShowMessage("Volunteer added", MatToastType.Success);
            else if (result == -1)
                m_notifactionTransmitter.ShowMessage("An error occured while adding volunteer to the database", MatToastType.Danger);
            m_karmaContext.PendingVolunteers.Where(x => x.Id == id).DeleteFromQuery();
            m_karmaContext.SaveChanges();
        }
        public void RemovePotentialVolunteer(Guid id)
        {
            m_karmaContext.PendingVolunteers.Where(x => x.Id == id).DeleteFromQuery();
            m_karmaContext.SaveChanges();
            m_notificationTransmitter.ShowMessage("The request has been deleted", MatToastType.Success);
        }
    }
}
