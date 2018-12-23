using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Newtonsoft.Json;
using WeatherApi.Implementation.DataLayer;
using WeatherApi.Models;
using WeatherApi.Models.OpenWeatherMap;
using WeatherApi.Models.WeatherApi;

namespace WeatherApi.Implementation.BusinessLayer
{
    /// <summary>
    /// Contains business logic for the city information
    /// </summary>
    public class CityService
    {
        /// <summary>
        /// Get list of cities
        /// </summary>
        /// <returns>Returns list of cities</returns>
        public IEnumerable<City> GetCitiesOfGermany(){
            //Reading cities json data from the file
            var strCities = File.ReadAllText(@"..\WeatherApi\App_Data\city.list.json");
            var cities = JsonConvert.DeserializeObject<List<City>>(strCities);
            return cities;
        }
    }
}