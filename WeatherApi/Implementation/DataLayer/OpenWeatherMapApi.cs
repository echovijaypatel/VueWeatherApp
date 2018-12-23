using System.IO;
using System.Net;
using Newtonsoft.Json;
using WeatherApi.Models.OpenWeatherMap;

namespace WeatherApi.Implementation.DataLayer
{
    /// <summary>
    /// Contains method to get and make data set for the weather information using openweather api's
    /// </summary>
    public class OpenWeatherMapApi
    {
        /// <summary>
        /// Get t + 5 days weather information for the particular city
        /// </summary>
        /// <param name="cityId">City id</param>
        /// <returns>Returns t + 5 days weather information for the city</returns>
        public OpenWeatherResponse GetWeatherDataForCity(string cityId)
        {
            //We can include this in config file so we don't need to build code if we change key
            var apiKey = "fcadd28326c90c3262054e0e6ca599cd";
            var apiRequest =
            WebRequest.Create(@"http://api.openweathermap.org/data/2.5/forecast?id=" +
            cityId + "&appid=" + apiKey + "&units=metric") as HttpWebRequest;
            var apiResponse = string.Empty;
            using (HttpWebResponse response = apiRequest.GetResponse() as HttpWebResponse)
            {
                var reader = new StreamReader(response.GetResponseStream());
                apiResponse = reader.ReadToEnd();
            }

            var openWeatherResponse = JsonConvert.DeserializeObject<OpenWeatherResponse>(apiResponse);
            return openWeatherResponse;
        }
    }
}