// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading;
using System.Threading.Tasks;
using Karma.Models;
using Karma.Services;
using MatBlazor;
using Microsoft.AspNetCore.Components;
using Microsoft.EntityFrameworkCore;
using Microsoft.JSInterop;
using OpenCage.Geocode;

namespace Karma.Pages
{
    public partial class ShowIndividualEvent
    {
        [Inject]
        public IJSRuntime m_jsRuntime { get; set; }

        [Inject]
        public IWeatherForecast m_weatherForecast { get; set; }

        [Inject]
        public IGeocoder m_geocoder { get; set; }

        [Parameter]
        public Guid Id { get; set; }

        public CharityEvent charityEvent;
        public string CurrentUserId { get; set; }
        public string weatherForecast = "loading weather forecast";
        public int currentParticipants = 0;
        public int neededParticipants = 0;
        public double participantsProgress = 0;
        public List<string> images = new();
        public List<string> equipment = new() { "Loading..." };
        public List<string> volunteers = new() { "Loading..." };
        private List<Thread> m_listOfThreads = new();
        private ConcurrentBag<string> m_statusMessages = new();
        private int m_totalLoadedTasks = 0;

        protected override async Task OnInitializedAsync()
        {
            var karmaContext = new KarmaContext();
            charityEvent = karmaContext.Events.Where(p => p.Id == Id).FirstOrDefault();
            var principal = m_httpContextAccessor.HttpContext.User;
            CurrentUserId = principal.FindFirstValue(ClaimTypes.NameIdentifier);
            try
            {
                weatherForecast = await m_weatherForecast.GetWeather(charityEvent.Address);
                var location = m_geocoder.Geocode(charityEvent.Address);
                var coords = location.Results[0].Geometry;
                await m_jsRuntime.InvokeVoidAsync("GetMap", coords.Latitude, coords.Longitude);
            }
            catch (InvalidAddressException ex)
            {
                m_notifactionTransmitter.ShowMessage("could not provide weather forecast:" + ex.Message, MatToastType.Danger);
                weatherForecast = "failed";
            }

            m_listOfThreads.Add(new Thread(() => GetImages()));
            m_listOfThreads.Add(new Thread(() => GetVolunteers()));
            m_listOfThreads.Add(new Thread(() => GetEquipment()));
            m_listOfThreads.Add(new Thread(() => CalculateParticipants()));

            foreach (var thread in m_listOfThreads)
            {
                thread.Start();
            }
        }

        public void NavigateToEditEvent()
        {
            m_uriHelper.NavigateTo($"editcharityevent/{Id}");
        }

        private void GetImages()
        {
            try
            {
                var karmaContext = new KarmaContext();
                images = karmaContext.EventImages
                    .Where(p => p.EventId == Id)
                    .Select(p => p.ImageUrl)
                    .ToList();
                m_totalLoadedTasks++;
                InvokeAsync(() => StateHasChanged());
            }
            catch (DbUpdateException)
            {
                m_statusMessages.Add("failed to load images");
            }
            CheckThreads();
        }

        private void GetVolunteers()
        {
            try
            {
                var karmaContext = new KarmaContext();
                volunteers = karmaContext.Events.Include(p => p.Volunteers).Where(p => p.Id == Id).Select(p => p.Volunteers).SelectMany(p => p).Select(p => $"{p.Name} {p.Surname}").ToList();
                m_totalLoadedTasks++;
                InvokeAsync(() => StateHasChanged());
            }
            catch (DbUpdateException)
            {
                m_statusMessages.Add("failed to load volunteers");
            }
            CheckThreads();
        }

        private void GetEquipment()
        {
            try
            {
                var karmaContext = new KarmaContext();
                var eventEquipment = new List<string>();
                var volunteers = karmaContext.Events.Include(e => e.Volunteers).Where(x => x.Id == Id).FirstOrDefault().Volunteers;
                equipment = karmaContext.SpecialEquipment.Where(x => volunteers.Contains(x.Owner)).Select(se => se.Name).ToList();
                m_totalLoadedTasks++;
                InvokeAsync(() => StateHasChanged());
            }
            catch (DbUpdateException)
            {
                m_statusMessages.Add("failed to load equipment");
            }
            CheckThreads();
        }

        private void CalculateParticipants()
        {
            var karmaContext = new KarmaContext();
            var events = karmaContext.Events.Include(x => x.Volunteers).FirstOrDefault(y => y.Id == Id);
            currentParticipants = events.Volunteers.Count;
            neededParticipants = events.MaxVolunteers;
            participantsProgress = (double) currentParticipants / neededParticipants;
            m_totalLoadedTasks++;
            InvokeAsync(() => StateHasChanged());
            CheckThreads();
        }

        private void CheckThreads()
        {
            if (m_totalLoadedTasks == m_listOfThreads.Count && weatherForecast != "failed")
            {
                m_notifactionTransmitter.ShowMessage("Loaded all elements of the event", MatToastType.Info);
            }
            if (m_statusMessages.Count > 0)
            {
                string val = null;
                while (val != "")
                {
                    m_statusMessages.TryTake(out val);
                    m_notifactionTransmitter.ShowMessage(val, MatToastType.Danger);
                }
            }
        }
    }
}
