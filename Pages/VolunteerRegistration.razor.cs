using System;
using Karma.Models;
using MatBlazor;

namespace Karma.Pages
{
    public partial class VolunteerRegistration
    {
        private String Name { get; set; } = "";

        private String Surname { get; set; } = "";

        private String PhoneNumber { get; set; } = "";

        KarmaContext KarmaContext = new KarmaContext();

        private void CreatePendingVolunteer()
        {
            var volunteer = new PendingVolunteer(Name, Surname, PhoneNumber, Guid.NewGuid());
            KarmaContext.PendingVolunteers.Add(volunteer);
            KarmaContext.SaveChanges();
            m_notifactionTransmitter.ShowMessage("Registration Succesful", MatToastType.Success);
            Name = "";
            Surname = "";
            PhoneNumber = "";
            NavigateToMain();
        }

        public void NavigateToMain()
        {
            m_uriHelper.NavigateTo("/");
        }
    }
}
