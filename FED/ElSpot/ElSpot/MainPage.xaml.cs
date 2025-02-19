namespace ElSpot;
using Microsoft.Maui.Controls;
using System;


public partial class MainPage : ContentPage
{
	public MainPage()
	{
		InitializeComponent();
	}

	 private async void OnElSpotPricesClicked(object sender, EventArgs e)
    {
        await Navigation.PushAsync(new ElSpotPricesPage());
    }

    private async void OnCO2EmissionsClicked(object sender, EventArgs e)
    {
        await Navigation.PushAsync(new CO2EmissionsPage());
    }

}

