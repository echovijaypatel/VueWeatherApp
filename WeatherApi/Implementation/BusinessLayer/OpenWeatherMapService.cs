using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using WeatherApi.Implementation.DataLayer;
using WeatherApi.Models;
using WeatherApi.Interfaces;
using WeatherApi.Models.OpenWeatherMap;
using WeatherApi.Models.WeatherApi;

namespace WeatherApi.Implementation.BusinessLayer
{
    /// <summary>
    /// Contains business logic for the weather information
    /// </summary>
    public class OpenWeatherMapService : IWeatherService
    {
        /// <summary>
        /// Object of ICacheService for caching operations
        /// </summary>
        private readonly ICacheService cacheService = null;
        
        /// <summary>
        /// Dependency injection for Object of ICacheService, so we can change from memory cache to anything in the future
        /// </summary>
        public OpenWeatherMapService(ICacheService cacheService)=>this.cacheService = cacheService;
        
        /// <summary>
        /// Get t + 5 days weather information for the particular city
        /// </summary>
        /// <param name="cityId">City id</param>
        /// <returns>Returns t + 5 days weather information for the city</returns>
        public WeatherApiResponse GetWeatherDataForCity(string cityId)
        {
            var weatherApiResponse = new WeatherApiResponse();
            try{
                var cachedData = this.cacheService.Get(cityId);
                //Check data is exist in the cache or not
                if(cachedData != null)
                {
                    return cachedData;
                }
                else
                {
                    //Getting data from open weather and converting to our data structure
                    var openWeatherResponse = new OpenWeatherMapApi().GetWeatherDataForCity(cityId);
                    weatherApiResponse.Status = openWeatherResponse.cod;
                    weatherApiResponse.Message = openWeatherResponse.message;
                    //Check http response is ok from openweather than only process
                    if(weatherApiResponse.Status == "200"){
                        weatherApiResponse.City = openWeatherResponse.city.name;
                        weatherApiResponse.Country = openWeatherResponse.city.country;
                        weatherApiResponse.WeatherInformation = 
                        openWeatherResponse.list.Select(x =>
                        new WeatherInformation(){
                            EpochTime = x.dt,
                            Time = Utility.UnixTimeStampToDateTime(x.dt),
                            Temprature = x.main.temp,
                            Humidity = x.main.humidity,
                            Wind = x.wind.speed
                        }).ToList();
                    }
                    this.cacheService.Add(cityId, weatherApiResponse);
                    return weatherApiResponse;
                }
            }catch{
                weatherApiResponse.Status = "500";
                weatherApiResponse.Message = Utility.SERVER_ERROR;
                return weatherApiResponse;
            }
        }
    }
}