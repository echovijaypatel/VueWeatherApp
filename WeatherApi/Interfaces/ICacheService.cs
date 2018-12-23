using System;
using WeatherApi.Models.WeatherApi;

namespace WeatherApi.Interfaces
{
    /// <summary>
    /// Cache information
    /// </summary>
    public interface ICacheService
    {
        /// <summary>
        /// Add object in memory
        /// </summary>
        /// <param name="key">unique key</param>
        /// <param name="value">weather data to be cached</param>       
        void Add(string key, WeatherApiResponse value);
        
        /// <summary>
        /// Get object from memory
        /// </summary>
        /// <param name="key">unique key</param>
        WeatherApiResponse Get(string key);
    }
}