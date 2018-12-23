using System;
using System.Collections.Generic;

namespace WeatherApi.Models.WeatherApi
{
    public class WeatherApiResponse
    {
        public string Status { get; set; }
        public string Message { get; set; }
        public string City { get; set; }
        public string Country { get; set; }
        public List<WeatherInformation> WeatherInformation { get; set; }
    }
}