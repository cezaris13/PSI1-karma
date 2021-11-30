// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Karma.Services;
using Microsoft.Extensions.Configuration;
using Moq;
using Moq.Protected;
using NUnit.Framework;
using OpenCage.Geocode;

namespace KarmaTests
{
    public class WeatherForecastTests
    {
        [Test]
        public async Task Test_GetWeather()
        {
            var geocoderResponse = new GeocoderResponse()
            {
                Results = new Location[1]
                {
                    new Location()
                    {
                        Geometry = new Point
                        {
                            Latitude = 12,
                            Longitude = 13
                        }
                    }
                }
            };
            var geocoder = new Mock<IGeocoder>(MockBehavior.Strict);
            geocoder
                .Setup(p => p.Geocode("location", It.IsAny<string>(), It.IsAny<string>(), It.IsAny<Bounds>(), It.IsAny<bool>(), It.IsAny<int>(), It.IsAny<int>(), It.IsAny<bool>(), It.IsAny<bool>(), It.IsAny<bool>(), It.IsAny<bool>()))
                .Returns(geocoderResponse);
            var handlerMock = new Mock<HttpMessageHandler>();
            var response = new HttpResponseMessage
            {
                StatusCode = HttpStatusCode.OK,
                Content = new StringContent("{\"meta\":{\"message\":\"\",\"code\":\"200\"},\"response\":{\"precipitation\":{\"type_ext\":null,\"intensity\":0,\"correction\":null,\"amount\":0,\"duration\":0,\"type\":0},\"pressure\":{\"h_pa\":988,\"mm_hg_atm\":741,\"in_hg\":38.9},\"humidity\":{\"percent\":90},\"icon\":\"c3\",\"gm\":3,\"wind\":{\"direction\":{\"degree\":190,\"scale_8\":5},\"speed\":{\"km_h\":11,\"m_s\":3,\"mi_h\":6}},\"cloudiness\":{\"type\":3,\"percent\":100},\"date\":{\"UTC\":\"2021-11-28 10:20:00\",\"local\":\"2021-11-28 12:20:00\",\"time_zone_offset\":120,\"hr_to_forecast\":null,\"unix\":1638094800},\"radiation\":{\"uvb_index\":null,\"UVB\":null},\"city\":4230,\"kind\":\"Obs\",\"storm\":false,\"temperature\":{\"comfort\":{\"C\":-2.4,\"F\":27.7},\"water\":{\"C\":3,\"F\":37.4},\"air\":{\"C\":1,\"F\":33.8}},\"description\":{\"full\":\"Mainly cloudy\"}}}\n")
            };

            handlerMock
               .Protected()
               .Setup<Task<HttpResponseMessage>>("SendAsync", ItExpr.IsAny<HttpRequestMessage>(), ItExpr.IsAny<CancellationToken>())
               .ReturnsAsync(response);

            var httpClient = new HttpClient(handlerMock.Object);
            var httpFactory = new Mock<IHttpClientFactory>(MockBehavior.Strict);
            httpFactory
                .Setup(p => p.CreateClient(It.IsAny<string>()))
                .Returns(httpClient);

            var inMemorySettings = new Dictionary<string, string> {
                {"X-Gistmeteo-Token", "gismeteoTokenValue"},
            };

            IConfiguration configuration = new ConfigurationBuilder()
                .AddInMemoryCollection(inMemorySettings)
                .Build();

            var sut = new WeatherForecast(geocoder.Object, httpFactory.Object, configuration);
            string result = await sut.GetWeather("location");

            var cloudiness = "100";
            var temperature = "1";
            var humidity = "90";

            Assert.AreEqual($"Cloudiness : {cloudiness}% Temperature: {temperature}°C Humidity:{humidity}%", result);

            geocoder.VerifyAll();
            httpFactory.VerifyAll();
        }

        [Test]
        public void GetWeatherTest_ReturnsZeroResult_ThrowsException()
        {
            var location = "invalid location";
            var geocoderResponse = new GeocoderResponse()
            {
                Results = Array.Empty<Location>()
            };
            var geocoder = new Mock<IGeocoder>(MockBehavior.Strict);
            geocoder
                .Setup(p => p.Geocode(location, It.IsAny<string>(), It.IsAny<string>(), It.IsAny<Bounds>(), It.IsAny<bool>(), It.IsAny<int>(), It.IsAny<int>(), It.IsAny<bool>(), It.IsAny<bool>(), It.IsAny<bool>(), It.IsAny<bool>()))
                .Returns(geocoderResponse);
            var httpFactory = new Mock<IHttpClientFactory>(MockBehavior.Strict);

            var configuration = new Mock<IConfiguration>(MockBehavior.Strict);

            var sut = new WeatherForecast(geocoder.Object, httpFactory.Object, configuration.Object);
            InvalidAddressException result = Assert.ThrowsAsync<InvalidAddressException>(async () => await sut.GetWeather(location));

            Assert.AreEqual($"Invalid address: {location}", result.Message);

            geocoder.VerifyAll();
            httpFactory.VerifyAll();
            configuration.VerifyAll();
        }
    }
}
