using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Security.Policy;
using System.Text;
using System.Threading.Tasks;
using System.Text.Json; 

namespace WeatherApp
{
    public class WeatherService(string apiKey)
    {
        private readonly HttpClient _httpClient = new();
        private readonly string _apiKey = apiKey;
        private const string BaseUrl = "https://api.openweathermap.org/data/2.5/weather";


        private static string ConvertQParamsToString(Dictionary<string, string> queryParams)
        {
            return string.Join(
                "&",
                queryParams.Select(kvp =>
                    $"{Uri.EscapeDataString(kvp.Key)} = {Uri.EscapeDataString(kvp.Value)}"
                )
            );
        }

        public async Task<WeatherResponse> GetWeatherAsync(string locationName)
        {
            try
            {
                string url = $"{BaseUrl}?" + ConvertQParamsToString(new Dictionary<string, string>
                {
                    {"q", locationName },
                    {"appid", _apiKey},
                    {"units", "metric" }
                });
                var response = await _httpClient.GetAsync(url);
                response.EnsureSuccessStatusCode();

                string Json = await response.Content.ReadAsStringAsync();
                var weatherResponse = JsonSerializer.Deserialize<WeatherResponse>( Json );

                return weatherResponse ?? throw new Exception("Failed to deserialize weather data."); 

            }
            catch (Exception ex) 
            {
                throw new Exception($"Error fetching weather data: {ex.Message}");
            }
        }

        public static string GetWeatherIconUrl(string iconCode)
        {
            return $"https://openweathermap.org/img/wn/{iconCode}@2x.png";
        }
    }
}
