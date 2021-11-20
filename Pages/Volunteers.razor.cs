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

namespace Karma.Pages
{
    public partial class Volunteers
    {
        [Inject]
        private NavigationManager m_navigationManager { get; set; }

        public string filterValue = "";

        public string FilterValue = "";

        private KarmaContext m_karmaContext;

        [Inject]
        public IDBServiceProvider m_DBServiceProvider { get; set; }

        private string CurrentUserId { get; set; }

        //Active volunteers tab:
        public IEnumerable<IVolunteer> GetVolunteers()
        {
            return m_karmaContext.Volunteers.Include(x => x.Events);
        }

        public void NavigateToIndividualEvent(Guid id)
        {
            m_navigationManager.NavigateTo($"event/{id}");
        }

        public void NavigateToAddVolunteer()
        {
            m_navigationManager.NavigateTo("addvolunteer");
        }

        public void RemoveVolunteer(Guid id)
        {
            m_karmaContext.SpecialEquipment.Where(x => x.Owner.Id == id).DeleteFromQuery();
            m_karmaContext.Volunteers.Where(x => x.Id == id).DeleteFromQuery();
            m_karmaContext.SaveChanges();
            m_notificationTransmitter.ShowMessage("The volunteer has been removed", MatToastType.Success);
        }

        public void NavigateToManageVolunteer(Guid id)
        {
            m_navigationManager.NavigateTo($"volunteer/{id}");
        }

        //Pending volunteers tab:
        protected override void OnInitialized()
        {
            m_karmaContext = m_DBServiceProvider.karmaContext;
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
            if (result == -1)
                m_notificationTransmitter.ShowMessage("An error occured while adding volunteer to the database", MatToastType.Danger);

            m_karmaContext.PendingVolunteers.Where(x => x.Id == id).DeleteFromQuery();
            m_karmaContext.SaveChanges();
        }

        public void RemovePotentialVolunteer(Guid id)
        {
            m_karmaContext.PendingVolunteers.Where(x => x.Id == id).DeleteFromQuery();
            m_karmaContext.SaveChanges();
            m_notificationTransmitter.ShowMessage("The request has been deleted", MatToastType.Success);
        }

        public void NavigateToVolunteerRequest(Guid id)
        {
            m_navigationManager.NavigateTo($"pendingvolunteer/{id}");
        }

        //Equipment tab:
        private IEnumerable<VolunteerAndEquipment> GetPairsOfVolunteersAndEquipment()
        {
            var volunteers = m_karmaContext.Volunteers.ToList();
            var equipment = m_karmaContext.SpecialEquipment.ToList();
            return volunteers
                .GroupJoin(
                    equipment,
                    owner => owner,
                    eq => eq.Owner,
                    (owner, collection) => new VolunteerAndEquipment(owner.Name, owner.Surname, collection.Select(x => x.Name)))
                .ToList()
                .OrderByEquipmentCount();
        }
    }
}
