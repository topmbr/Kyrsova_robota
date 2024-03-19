using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MySql.Data.MySqlClient;
using Newtonsoft.Json.Linq;


namespace ConsoleApp111
{
    public class WeatherDatabase
    {
        private string connectionString;

        public WeatherDatabase(string connectionString)
        {
            this.connectionString = connectionString;
        }

        private DateTime UnixTimeStampToDateTime(long unixTimeStamp)
        {
            DateTime dtDateTime = new DateTime(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc);
            dtDateTime = dtDateTime.AddSeconds(unixTimeStamp).ToLocalTime();
            return dtDateTime;
        }
        public void InsertWeather(string city, string weatherJson)
        {
            // Парсим JSON
            JObject weatherData = JObject.Parse(weatherJson);

            // Извлекаем нужные данные из JSON
            string country = (string)weatherData["sys"]["country"];
            double longitude = (double)weatherData["coord"]["lon"];
            double latitude = (double)weatherData["coord"]["lat"];
            string weatherMain = (string)weatherData["weather"][0]["main"];
            string weatherDescription = (string)weatherData["weather"][0]["description"];
            double temperature = (double)weatherData["main"]["temp"];
            double feelsLike = (double)weatherData["main"]["feels_like"];
            double minTemperature = (double)weatherData["main"]["temp_min"];
            double maxTemperature = (double)weatherData["main"]["temp_max"];
            int pressure = (int)weatherData["main"]["pressure"];
            int humidity = (int)weatherData["main"]["humidity"];
            int visibility = (int)weatherData["visibility"];
            double windSpeed = (double)weatherData["wind"]["speed"];
            int windDirection = (int)weatherData["wind"]["deg"];
            int cloudiness = (int)weatherData["clouds"]["all"];
            DateTime sunrise = UnixTimeStampToDateTime((long)weatherData["sys"]["sunrise"]);
            DateTime sunset = UnixTimeStampToDateTime((long)weatherData["sys"]["sunset"]);

            // Соединяемся с базой данных и вставляем данные
            using (var connection = new MySqlConnection(connectionString))
            {
                connection.Open();
                var query = "INSERT INTO Weather (City, Country, Longitude, Latitude, WeatherMain, WeatherDescription, Temperature, FeelsLike, MinTemperature, MaxTemperature, Pressure, Humidity, Visibility, WindSpeed, WindDirection, Cloudiness, Sunrise, Sunset) " +
                            "VALUES (@City, @Country, @Longitude, @Latitude, @WeatherMain, @WeatherDescription, @Temperature, @FeelsLike, @MinTemperature, @MaxTemperature, @Pressure, @Humidity, @Visibility, @WindSpeed, @WindDirection, @Cloudiness, @Sunrise, @Sunset)";
                using (var command = new MySqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@City", city);
                    command.Parameters.AddWithValue("@Country", country);
                    command.Parameters.AddWithValue("@Longitude", longitude);
                    command.Parameters.AddWithValue("@Latitude", latitude);
                    command.Parameters.AddWithValue("@WeatherMain", weatherMain);
                    command.Parameters.AddWithValue("@WeatherDescription", weatherDescription);
                    command.Parameters.AddWithValue("@Temperature", temperature);
                    command.Parameters.AddWithValue("@FeelsLike", feelsLike);
                    command.Parameters.AddWithValue("@MinTemperature", minTemperature);
                    command.Parameters.AddWithValue("@MaxTemperature", maxTemperature);
                    command.Parameters.AddWithValue("@Pressure", pressure);
                    command.Parameters.AddWithValue("@Humidity", humidity);
                    command.Parameters.AddWithValue("@Visibility", visibility);
                    command.Parameters.AddWithValue("@WindSpeed", windSpeed);
                    command.Parameters.AddWithValue("@WindDirection", windDirection);
                    command.Parameters.AddWithValue("@Cloudiness", cloudiness);
                    command.Parameters.AddWithValue("@Sunrise", sunrise);
                    command.Parameters.AddWithValue("@Sunset", sunset);
                    command.ExecuteNonQuery();
                }
            }
        }

    }


}
