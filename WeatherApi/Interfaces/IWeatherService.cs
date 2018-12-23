using WeatherApi.Models.WeatherApi;

namespace WeatherApi.Interfaces
{
    /// <summary>
    /// Weather information provider
    /// </summary>
    public interface IWeatherService
    {
        /// <summary>
        /// Get t + 5 days weather information for the particular city
        /// </summary>
        /// <param name="cityId">City id</param>
        /// <returns>Returns t + 5 days weather information for the city</returns>
        WeatherApiResponse GetWeatherDataForCity(string cityId);         
    }
}