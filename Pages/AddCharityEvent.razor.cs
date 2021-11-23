// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System;
using System.Security.Claims;
using Karma.Models;
using Karma.Services;
using MatBlazor;
using Microsoft.AspNetCore.Components;

namespace Karma.Pages
{
    public partial class AddCharityEvent
    {
        [Inject]
        private IDBServiceProvider m_DBServiceProvider { get; set; }
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
            int result = m_DBServiceProvider.AddToDB(charityEvent);
            if (result == 0)
                m_uriHelper.NavigateTo("");
            else if (result == -1)
                m_notificationTransmitter.ShowMessage("An error occured while adding event to the database", MatToastType.Danger);
        }
    }
}
