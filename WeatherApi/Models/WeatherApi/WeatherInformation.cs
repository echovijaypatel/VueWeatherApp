namespace WeatherApi
{
    public class WeatherInformation
    {
        public int EpochTime {get; set; }
        public System.DateTime Time {get; set; }
        public double Temprature { get; set; }
        public int Humidity { get; set; }
        public double Wind { get; set; }
    }
}