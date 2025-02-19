using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.Maui.Controls;
using ElSpot.Models;

namespace ElSpot
{
    public partial class ElSpotPricesPage : ContentPage
    {
        private readonly HttpClient _httpClient = new HttpClient
        {
            BaseAddress = new Uri("https://api.energidataservice.dk/")
        };

        public ElSpotPricesPage()
        {
            InitializeComponent();
        }

        protected override async void OnAppearing()
        {
            base.OnAppearing();
            await LoadData();
        }

        private async Task LoadData()
        {
            try
            {
                string url = "dataset/Elspotprices?filter={\"PriceArea\":[\"DK1\"]}&columns=HourDK,PriceArea,SpotPriceDKK&limit=24";
                string response = await _httpClient.GetStringAsync(url);

                var data = JsonSerializer.Deserialize<ElSpotPrices>(response, new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                });

                if (data?.records != null)
                {
                    priceListView.ItemsSource = data.records;
                }
            }
            catch (Exception ex)
            {
                await DisplayAlert("Fejl", ex.Message, "OK");
            }
        }
    }
}
