using Microsoft.Maui.Controls;
using System;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;
using ElSpot.Models;

namespace ElSpot
{
    public partial class CO2EmissionsPage : ContentPage
    {
        private readonly HttpClient _httpClient = new HttpClient
        {
            BaseAddress = new Uri("https://api.energidataservice.dk/")
        };

        public CO2EmissionsPage()
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
                string url = "dataset/CO2Emissions?filter={...}";
                string response = await _httpClient.GetStringAsync(url);

                var data = JsonSerializer.Deserialize<object>(response, new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                });

                // Bind data til UI her...
            }
            catch (Exception ex)
            {
                await DisplayAlert("Fejl", ex.Message, "OK");
            }
        }
    }
}
