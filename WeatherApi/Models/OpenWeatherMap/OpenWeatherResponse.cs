using System.Collections.Generic;

//Generate using http://json2csharp.com/#
namespace WeatherApi.Models.OpenWeatherMap
{
    public class OpenWeatherResponse
    {
        public string cod { get; set; }
        public string message { get; set; }
        public int cnt { get; set; }
        public List<CityData> list { get; set; }
        public City city { get; set; }
    }
    
    // public class Weather
    // {
    //     public int id { get; set; }
    //     public string main { get; set; }
    //     public string description { get; set; }
    //     public string icon { get; set; }
    // }

    // public class Clouds
    // {
    //     public int all { get; set; }
    // }

    // public class Snow
    // {
    //     public double __invalid_name__3h { get; set; }
    // }

    // public class Sys
    // {
    //     public string pod { get; set; }
    // }

    // public class Coord
    // {
    //     public double lat { get; set; }
    //     public double lon { get; set; }
    // }
}