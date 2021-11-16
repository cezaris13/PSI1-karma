using System;
using Karma.Models;
using MatBlazor;

namespace Karma.Pages
{
    public partial class VolunteerRegistration
    {
        private string Name { get; set; } = "";

        private string Surname { get; set; } = "";

        private string PhoneNumber { get; set; } = "";

        private KarmaContext m_karmaConext = new();

        private void CreatePendingVolunteer()
        {
            var volunteer = new PendingVolunteer(Name, Surname, PhoneNumber, Guid.NewGuid());
            m_karmaConext.PendingVolunteers.Add(volunteer);
            m_karmaConext.SaveChanges();
            m_notifactionTransmitter.ShowMessage("Registration Succesful", MatToastType.Success);
            Name = "";
            Surname = "";
            PhoneNumber = "";
            m_uriHelper.NavigateTo("/");
        }
    }
}
