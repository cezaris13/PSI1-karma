using System;
using Karma.Models;
using MatBlazor;
using Microsoft.AspNetCore.Components;
using Microsoft.EntityFrameworkCore;

namespace Karma.Pages
{
    public partial class VolunteerRegistration
    {
        [Inject]
        private IDbContextFactory<KarmaContext> m_contextFactory { get; set; }

        private string Name { get; set; } = "";

        private string Surname { get; set; } = "";

        private string PhoneNumber { get; set; } = "";

        private KarmaContext m_karmaContext;

        protected override void OnInitialized()
        {
            m_karmaContext = m_contextFactory.CreateDbContext();
        }

        private void CreatePendingVolunteer()
        {
            var volunteer = new PendingVolunteer(Name, Surname, PhoneNumber, Guid.NewGuid());
            m_karmaContext.PendingVolunteers.Add(volunteer);
            m_karmaContext.SaveChanges();
            m_notificationTransmitter.ShowMessage("Registration Succesful", MatToastType.Success);
            Name = "";
            Surname = "";
            PhoneNumber = "";
            m_uriHelper.NavigateTo("/");
        }
    }
}
