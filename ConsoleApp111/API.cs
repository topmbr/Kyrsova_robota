using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApp111
{
    public class WeatherService
    {
        private string apiKey;

        public WeatherService(string apiKey)
        {
            this.apiKey = apiKey;
        }

        public string GetWeather(string city)
        {
            using (WebClient webClient = new WebClient())
            {
                string url = $"http://api.openweathermap.org/data/2.5/weather?q={city}&appid={apiKey}&units=metric";
                try
                {
                    return webClient.DownloadString(url);
                }
                catch (WebException ex)
                {
                    Console.WriteLine("Error accessing weather API: " + ex.Message);
                    return null;
                }
            }
        }
    }



}
