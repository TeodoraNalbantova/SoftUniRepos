
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json.Serialization;
using System;
using System.Text.Json.Serialization;

namespace ApiTestingDemo
{
    class Program
    {
        static void Main(string[] args)
        {
            // using JSON.NET
            //WeatherForecast weatherForecast = new WeatherForecast()
            //{
            //    Date = DateTime.Now,
            //    TemperatureC = 32,
            //    Summary = "New Random Test Summary"
            //};

            //string weatherForecastJson = JsonConvert.SerializeObject(weatherForecast,Formatting.Indented);
            //Console.WriteLine(weatherForecastJson);

            //==========================
            //▪ Deserializing to anonymous types
            //string jsonString = File.ReadAllText(Path.Combine(Environment.CurrentDirectory) + "/../../../people.json");

            ////анонимен обект
            //var person = new
            //{
            //    FirstName = string.Empty,
            //    LastName = string.Empty,
            //    JobTitle = string.Empty,
            //};

            //var weatherForecastObject = JsonConvert.DeserializeAnonymousType(jsonString, person);

            //=================================

            //string jsonString = File.ReadAllText(Path.Combine(Environment.CurrentDirectory) + "/../../../WeatherForecast.json");

            //var weatherForecastObject = JsonConvert.DeserializeObject<List<WeatherForecast>>(jsonString);

            ////////////JSON.NET Parsing of Objects
            //    WeatherForecast weatherForecast = new WeatherForecast()
            //    {
            //        Date = DateTime.Now,
            //        TemperatureC = 32,
            //        Summary = "New Random Test Summary"
            //    };

            //    DefaultContractResolver contractResolver =
            // new DefaultContractResolver()
            // {
            //     NamingStrategy = new SnakeCaseNamingStrategy()
            // };
            //    var serialized = JsonConvert.SerializeObject(weatherForecast,
            //    new JsonSerializerSettings()
            //    {
            //        ContractResolver = contractResolver,
            //        Formatting = Formatting.Indented
            //    });
            //=============LINQ-to-JSON ====================
            string jsonString = File.ReadAllText(Path.Combine(Environment.CurrentDirectory) + "/../../../people.json");
            var person = JObject.Parse(jsonString);
            Console.WriteLine(person["firstName"]);
            Console.WriteLine(person["lastName"]);
        }
    }
}
