using ConsoleApp111;
using Newtonsoft.Json.Linq;
using System;

class Program
{
    static void Main(string[] args)
    {
        Console.WriteLine("Enter the city name:");
        string city = Console.ReadLine();

        WeatherService weatherService = new WeatherService("2a74c2951a81c8d4d20d9a502ece070e");
        string weatherJson = weatherService.GetWeather(city);

        if (!string.IsNullOrEmpty(weatherJson))
        {
            // Парсинг JSON
            JObject data = JObject.Parse(weatherJson);

            // Вывод данных по столбцам
            Console.WriteLine($"City: {data["name"]}");
            Console.WriteLine($"Country: {data["sys"]["country"]}");
            Console.WriteLine($"Coordinates: Longitude - {data["coord"]["lon"]}, Latitude - {data["coord"]["lat"]}");

            JArray weatherArray = (JArray)data["weather"];
            foreach (var weather in weatherArray)
            {
                Console.WriteLine($"Weather: {weather["main"]}, Description: {weather["description"]}");
            }

            Console.WriteLine($"Temperature: {data["main"]["temp"]}°C");
            Console.WriteLine($"Feels Like: {data["main"]["feels_like"]}°C");
            Console.WriteLine($"Minimum Temperature: {data["main"]["temp_min"]}°C");
            Console.WriteLine($"Maximum Temperature: {data["main"]["temp_max"]}°C");
            Console.WriteLine($"Pressure: {data["main"]["pressure"]} hPa");
            Console.WriteLine($"Humidity: {data["main"]["humidity"]}%");
            Console.WriteLine($"Visibility: {data["visibility"]} meters");
            Console.WriteLine($"Wind Speed: {data["wind"]["speed"]} m/s");
            Console.WriteLine($"Wind Direction: {data["wind"]["deg"]}°");
            Console.WriteLine($"Cloudiness: {data["clouds"]["all"]}%");
            Console.WriteLine($"Sunrise: {UnixTimeStampToDateTime((long)data["sys"]["sunrise"])}");
            Console.WriteLine($"Sunset: {UnixTimeStampToDateTime((long)data["sys"]["sunset"])}");

            // Сохранение данных в базу данных
            WeatherDatabase db = new WeatherDatabase("server=localhost;port=3300;user=root;password=8josd12M;database=WeatherDB;");
            db.InsertWeather(city, weatherJson);
        }
        else
        {
            Console.WriteLine($"Weather forecast for {city} not available.");
        }
    }

    static DateTime UnixTimeStampToDateTime(long unixTimeStamp)
    {
        DateTime dtDateTime = new DateTime(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc);
        dtDateTime = dtDateTime.AddSeconds(unixTimeStamp).ToLocalTime();
        return dtDateTime;
    }
}
