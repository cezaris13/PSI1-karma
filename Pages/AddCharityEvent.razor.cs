// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System;
using System.Security.Claims;
using Karma.Models;
using Karma.Services;
using MatBlazor;

namespace Karma.Pages
{
    public partial class AddCharityEvent
    {
        private KarmaContext m_karmaContext = new();
        public string EventTitle { get; set; }
        public string EventDescription { get; set; }
        public string CurrentUserId { get; set; }
        public string EventAddress { get; set; }

        protected override void OnInitialized()
        {
            ClaimsPrincipal principal = m_httpContextAccessor.HttpContext.User;
            CurrentUserId = principal.FindFirstValue(ClaimTypes.NameIdentifier);
        }

        private void AddEvent()
        {
            var charityEvent = new CharityEvent(EventTitle, EventDescription, Guid.NewGuid(), CurrentUserId, EventAddress);
            if (ObjectChecker.IsAnyNullOrEmpty(charityEvent))
            {
                m_notifactionTransmitter.ShowMessage("There are some empty fields", MatToastType.Danger);
            }
            else
            {
                m_karmaContext.Events.Add(charityEvent);
                m_karmaContext.SaveChanges();
                m_uriHelper.NavigateTo("");
                m_notifactionTransmitter.ShowMessage("The event has been created", MatToastType.Success);
            }
        }
    }
}
