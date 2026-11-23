using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.Json.Serialization;

namespace WeatherApp
{
    public class WeatherResponse
    {
        [JsonPropertyName("coord")]
        public required Coord  Coord {  get; set; }

        [JsonPropertyName("weather")]
        public required List<Weather> Weather { get; set; }

        [JsonPropertyName("main")]
        public required Main Main { get; set; }

        [JsonPropertyName("visibility")]
        public int Visibility { get; set; }

        [JsonPropertyName("wind")]
        public required Wind Wind { get; set; }

        [JsonPropertyName("clouds")]
        public required Clouds Clouds { get; set; }

        [JsonPropertyName("dt")]
        public required dt dt { get; set; }
    }
}
