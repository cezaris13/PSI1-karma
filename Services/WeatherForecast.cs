// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Karma.Models;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using OpenCage.Geocode;

namespace Karma.Services
{
    public class WeatherForecast : IWeatherForecast
    {
        private readonly IGeocoder m_geocoder;
        private readonly Lazy<HttpClient> m_httpClient;
        private readonly IConfiguration m_configuration;

        public WeatherForecast(IGeocoder geocoder, IHttpClientFactory httpClientFactory, IConfiguration configuration)
        {
            m_geocoder = geocoder;
            m_httpClient = new Lazy<HttpClient>(() => httpClientFactory.CreateClient());
            m_configuration = configuration;
        }

        public async Task<string> GetWeather(string location)
        {
            var forward = m_geocoder.Geocode(location);
            if (forward.Results.Length == 0)
            {
                throw new InvalidAddressException(location);
            }
            var point = forward.Results[0].Geometry;
            HttpRequestMessage message = new HttpRequestMessage
            {
                RequestUri = new Uri($"https://api.gismeteo.net/v2/weather/current/?latitude={point.Latitude}&longitude={point.Longitude}&lang=en"),
                Headers =
                    {
                        { "X-Gismeteo-Token", m_configuration["X-Gismeteo-Token"] }
                    },
                Method = HttpMethod.Get
            };
            var response = await m_httpClient.Value.SendAsync(message);
            var weatherForecast = await response.Content.ReadAsStringAsync();
            var result = JsonConvert.DeserializeObject<WeatherForecastData>(weatherForecast);

            return $"Cloudiness : {result.Cloudiness}% Temperature: {result.Temperature}°C Humidity:{result.Humidity}%";
        }
    }
}
