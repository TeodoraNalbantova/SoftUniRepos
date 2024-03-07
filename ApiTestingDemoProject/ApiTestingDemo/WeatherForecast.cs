using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApiTestingDemo
{
    public class WeatherForecast
    {
        [JsonProperty("Date")]
        public DateTime Date { get; set; }
        public int TemperatureC { get; set; }
        
        public string Summary { get; set; }
    }
}
