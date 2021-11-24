// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Karma.Models;
using Microsoft.AspNetCore.Components;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;

namespace Karma.Pages
{
    public partial class LocalEvents
    {
        [Inject]
        public IConfiguration m_configuration { get; set; }

        [Inject]
        public IHttpClientFactory m_httpClientFactory { get; set; }

        public IEnumerable<ICharityEvent> listOfLocalCharityEvents = new List<CharityEvent>();
        private IEnumerable<ICharityEvent> m_listOfLocalCharityEvents = new List<CharityEvent>();

        private string FilterValue { get; set; } = "";

        private string address { get; set; } = "";

        private readonly string m_baseUrl = "https://localhost:44336/";
        private int m_totalPageQuantity;
        private int m_currentPage = 1;
        public int perPage = 10;

        protected override async Task OnInitializedAsync()
        {
            await GetCityString();
            await GetNearbyEvents();
        }

        public void NavigateToIndividualEvent(Guid id)
        {
            m_uriHelper.NavigateTo($"event/{id}");
        }

        public void SelectedPage(int page)
        {
            m_currentPage = page;
            LoadEvents(page, perPage);
        }

        public async Task GetCityString()
        {
            HttpClient localClient = m_httpClientFactory.CreateClient("city");
            string city = await localClient.GetStringAsync($"https://api.ipdata.co/?api-key={m_configuration["IpDataKey"]}&fields=city");
            address = JsonConvert.DeserializeObject<JsonToString>(city).city;
        }

        public async Task GetNearbyEvents()
        {
            List<CharityEvent> givenEvents = new List<CharityEvent>();
            HttpClient localClient = m_httpClientFactory.CreateClient("events");
            localClient.BaseAddress = new Uri(m_baseUrl);
            HttpResponseMessage Res = await localClient.GetAsync($"api/Event?address={address}");
            if (Res.IsSuccessStatusCode)
            {
                string EmpResponse = await Res.Content.ReadAsStringAsync();
                givenEvents = JsonConvert.DeserializeObject<List<CharityEvent>>(EmpResponse);
            }
            m_listOfLocalCharityEvents = givenEvents;
            LoadEvents();
        }

        public async Task Refresh(int pageSize)
        {
            perPage = pageSize;
            LoadEvents(elementsPerPage: pageSize);
            try
            {
                await InvokeAsync(StateHasChanged);
            }
            catch { }
        }

        private void LoadEvents(int page = 1, int elementsPerPage = 10)
        {
            m_totalPageQuantity = Convert.ToInt32(Math.Ceiling(m_listOfLocalCharityEvents.Count() / (double) elementsPerPage));
            listOfLocalCharityEvents = m_listOfLocalCharityEvents.Skip((page - 1) * elementsPerPage).Take(elementsPerPage);
        }

        public class JsonToString
        {
            public string city = string.Empty;
        }
    }
}
