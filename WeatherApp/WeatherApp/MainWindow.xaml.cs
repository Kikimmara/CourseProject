using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Globalization;

 
namespace WeatherApp
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private readonly WeatherService _weatherService;
        public MainWindow()
        {
            InitializeComponent();
            _weatherService = new WeatherService(System.IO.File.ReadAllText("key.txt"));
        }

        private void ShowStatus(string message)
        {
            StatusText.Text = message;
            StatusText.Visibility = Visibility.Visible;
            WeatherPanel.Visibility = Visibility.Collapsed;
        }

        private static string ToTitleCase(string input)
        {
            if (string.IsNullOrEmpty(input))
                return input;

            return CultureInfo.CurrentCulture.TextInfo.ToTitleCase(input.ToLower());
        }

        private void DisplayWeather(WeatherResponse weather)
        {
            if (weather == null || weather.Weather == null || weather.Weather.Count == 0)
            {
                ShowStatus("No weather data available");
                return;
            }
            try
            {
                StatusText.Visibility = Visibility.Collapsed;
                WeatherPanel.Visibility = Visibility.Visible;

                LocationNameText.Text = $"{weather.Name}, {weather.Sys.Country}";
                TemperatureText.Text = $"{Math.Round(weather.Main.Temp)}°C";
                DescriptionText.Text = ToTitleCase(weather.Weather[0].Description);
                FeelsLikeText.Text = $"{Math.Round(weather.Main.FeelsLike)}°C";
                HumidityText.Text = $"{weather.Main.Humidity}%";
                WindSpeedText.Text = $"{weather.Wind.Speed} m/s";

                string iconUrl = WeatherService.GetWeatherIconUrl(weather.Weather[0].Icon);
                WeatherIcon.Source = new BitmapImage(new Uri(iconUrl));
            }
            catch (Exception ex) 
            {
                ShowStatus($"Error displaying weather data: {ex.Message}");
            }
        }

        private async void SearchButton_Click(object sender, RoutedEventArgs e)
        {
            string locationName = LocationTextBox.Text.Trim();

            if (string.IsNullOrEmpty(locationName))
                return;

            SearchButton.IsEnabled = false;
            SearchButton.Content = "Loading...";
            ShowStatus("Fetching weather data...");
            WeatherPanel.Visibility = Visibility.Collapsed;

            try
            {
                var weatherData = await _weatherService.GetWeatherAsync(locationName);
                DisplayWeather(weatherData);
            }
            catch (Exception ex)
            {
                ShowStatus($"Error: {ex.Message}");
            }
            finally
            {
                SearchButton.IsEnabled = true;
                SearchButton.Content = "Search";
            }
        }

        private void LocationTextBox_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter) 
                SearchButton_Click(sender, e);
        }
    }
}