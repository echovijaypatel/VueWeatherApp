using System;
using Microsoft.Extensions.Caching.Memory;
using Newtonsoft.Json;
using WeatherApi.Interfaces;
using WeatherApi.Models;
using WeatherApi.Models.WeatherApi;

namespace WeatherApi.Implementation.BusinessLayer
{
    /// <summary>
    /// Cache weather information in memory
    /// </summary>
    public class MemoryCacheService : ICacheService
    {
        /// <summary>
        /// Object of the IMemoryCache
        /// </summary>
        private IMemoryCache memoryCache = null;
        public MemoryCacheService(IMemoryCache memoryCache) => this.memoryCache = memoryCache;

        /// <summary>
        /// Add object in memory
        /// </summary>
        /// <param name="key">unique key</param>
        /// <param name="value">weather data to be cached</param>
        public void Add(string key, WeatherApiResponse value){
            memoryCache.Set(key, value, Utility.GetTodayExpiryTimespan());
        }
        
        /// <summary>
        /// Get object from memory
        /// </summary>
        /// <param name="key">unique key</param>
        public WeatherApiResponse Get(string key){
            var cachedData = memoryCache.Get(key);
            if(cachedData != null){
                return cachedData as WeatherApiResponse;
            }else{
                return null;
            }
        }
    }
}