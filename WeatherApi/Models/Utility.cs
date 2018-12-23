using System;
using System.Net;

namespace WeatherApi.Models
{
    /// <summary>
    /// Contains generic functions used across the project.
    /// </summary>
    public class Utility
    {
        /// <summary>
        /// Error message to return in case of unexpected errors on the server.
        /// </summary>
        public const string SERVER_ERROR = "We are facing some temporary error(s), please try again or contact us if the issue persist.";

        /// <summary>
        /// Converts unix time to c# date object.
        /// </summary>
        public static DateTime UnixTimeStampToDateTime(double unixTimeStamp)
        {
            // Unix timestamp is seconds past epoch
            var dateTime = new DateTime(1970,1,1,0,0,0,0,System.DateTimeKind.Utc);
            dateTime = dateTime.AddSeconds(unixTimeStamp).ToLocalTime();
            return dateTime;
        }

        public static TimeSpan GetTodayExpiryTimespan()
        {
            DateTime now = DateTime.Now;
            DateTime dateTime = new DateTime(now.Year, now.Month, now.Day, 23, 59, 59); 
            return dateTime-now;
        }
    }
}