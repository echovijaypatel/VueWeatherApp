using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using WeatherApi.Implementation;
using WeatherApi.Implementation.BusinessLayer;
using WeatherApi.Interfaces;
using WeatherApi.Models;
using WeatherApi.Models.OpenWeatherMap;
using WeatherApi.Models.WeatherApi;

namespace WeatherApi.Controllers
{
    /// <summary>
    /// Contains api for weather information
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    public class WeatherController : ControllerBase
    {
        /// <summary>
        /// Object of IWeatherService to fetch weather information
        /// </summary>
        private readonly IWeatherService weatherService = null;
        
        /// <summary>
        /// Dependency injection for Object of Iweather service, so we can change from openweather to any other service in the future
        /// </summary>
        public WeatherController(IWeatherService weatherService) => this.weatherService = weatherService;

        /// <summary>
        /// Get t + 5 days weather information for the particular city
        /// </summary>
        /// <param name="id">City id</param>
        /// <returns>Returns t + 5 days weather information for the city</returns>
        [HttpGet("{id}")]
        public WeatherApiResponse Get(string id)
        {
            return weatherService.GetWeatherDataForCity(id);
        }
    }
}
