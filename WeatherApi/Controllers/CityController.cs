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
using WeatherApi.Models;
using WeatherApi.Models.OpenWeatherMap;
using WeatherApi.Models.WeatherApi;

namespace WeatherApi.Controllers
{
    /// <summary>
    /// Contains api for city information
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    public class CityController : ControllerBase
    {
        /// <summary>
        /// Get list of city
        /// </summary>
        /// <returns>list of city</returns>
        [HttpGet()]
        public IEnumerable<City> Get()
        {
            //Haven't done much here please refer WeatherController to know more about my coding standards :)!
            return new CityService().GetCitiesOfGermany();
        }
    }
}
