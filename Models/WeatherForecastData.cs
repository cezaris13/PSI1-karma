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
                humidity = JsonConvert.DeserializeObject<Dictionary<string, int>>(value["humidity"].ToString());
                cloudiness = JsonConvert.DeserializeObject<Dictionary<string, int>>(value["cloudiness"].ToString());
                temperature = JsonConvert.DeserializeObject<Dictionary<string, Dictionary<string, double>>>(value["temperature"].ToString());
                // add more properties if needed
            }
        }

        private Dictionary<string, int> humidity;
        private Dictionary<string, int> cloudiness;
        private Dictionary<string, Dictionary<string, double>> temperature;

        public int Humidity
        {
            get
            {
                return humidity["percent"];
            }
        }

        public int Cloudiness
        {
            get
            {
                return cloudiness["percent"];
            }
        }

        public double Temperature
        {
            get
            {
                return temperature["air"]["C"];
            }
        }

    }
}
