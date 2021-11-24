// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System.Collections.Generic;
using Newtonsoft.Json;

namespace Karma.Models
{
    public class WeatherForecastData
    {
        [JsonProperty("meta")]
        public Dictionary<string, string> Meta { get; set; }

        [JsonProperty("Response")]
        public Dictionary<string, dynamic> Response
        {
            set
            {
                m_humidity = JsonConvert.DeserializeObject<Dictionary<string, int>>(value["humidity"].ToString());
                m_cloudiness = JsonConvert.DeserializeObject<Dictionary<string, int>>(value["cloudiness"].ToString());
                m_temperature = JsonConvert.DeserializeObject<Dictionary<string, Dictionary<string, double>>>(value["temperature"].ToString());
                // add more properties if needed
            }
        }

        private Dictionary<string, int> m_humidity;
        private Dictionary<string, int> m_cloudiness;
        private Dictionary<string, Dictionary<string, double>> m_temperature;

        public int Humidity
        {
            get
            {
                return m_humidity["percent"];
            }
        }

        public int Cloudiness
        {
            get
            {
                return m_cloudiness["percent"];
            }
        }

        public double Temperature
        {
            get
            {
                return m_temperature["air"]["C"];
            }
        }

    }
}
