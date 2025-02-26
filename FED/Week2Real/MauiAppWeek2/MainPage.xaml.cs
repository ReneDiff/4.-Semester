namespace MauiAppWeek2;
using System.Collections.ObjectModel;
using Microsoft.Maui.Controls;
using Microsoft.Maui.Storage;
using MauiAppWeek2.Models;
using MauiAppWeek2.Database;
using System.Windows.Input;
using CommunityToolkit.Maui.Views;
using MauiAppWeek2.Views;


public partial class MainPage : ContentPage
{
	private string? _imagePath;
	public ObservableCollection<ImageInfo> Images {get; set;} = new();
	private readonly MauiAppWeek2.Database.Database _database;

	public MainPage()
	{
		InitializeComponent();
		BindingContext = this;

		// Initialiser databasen og indlæs eksisterende data
        _database = new MauiAppWeek2.Database.Database();
        LoadImagesFromDatabase();
	}

	private async void LoadImagesFromDatabase()
    {
        try
        {
            // Hent alle billeder fra databasen
            var images = await _database.GetImageInfos();
            foreach (var image in images)
            {
                Images.Add(image);
            }
        }
        catch (Exception ex)
        {
            await DisplayAlert("Error", $"Failed to load images: {ex.Message}", "OK");
        }
    }

	private async void OnAddButtonClicked(object sender, EventArgs e)
    {
        if (string.IsNullOrWhiteSpace(_imagePath) ||
            string.IsNullOrWhiteSpace(TitleEntry.Text) ||
            string.IsNullOrWhiteSpace(DescriptionEditor.Text))
        {
            await DisplayAlert("Error", "All fields must be filled out before adding an image.", "OK");
            return;
        }

        var newImage = new ImageInfo
        {
            Path = _imagePath,
            Title = TitleEntry.Text,
            Description = DescriptionEditor.Text
        };

        try
        {
            // Tilføj billedet til databasen
            await _database.AddImageInfo(newImage);

            // Tilføj billedet til ObservableCollection
            Images.Add(newImage);

            // Ryd felterne
            _imagePath = string.Empty;
            //SelectedImage.Source = null;
            TitleEntry.Text = string.Empty;
            DescriptionEditor.Text = string.Empty;

            await DisplayAlert("Success", "Image added successfully!", "OK");
        }
        catch (Exception ex)
        {
            await DisplayAlert("Error", $"Failed to save image: {ex.Message}", "OK");
        }
    }

	private async void OnSelectImageClicked(object sender, EventArgs e)
	{
		var image = await FilePicker.Default.PickAsync(new PickOptions
		{
			PickerTitle = "Pick Image",
			FileTypes = FilePickerFileType.Images
		});

		if (image != null)
		{
			_imagePath = image.FullPath;
			//SelectedImage.Source = _imagePath;
		}
	}

    private void OnImageTapped(object sender, TappedEventArgs e)
    {
        if (sender is Image image && image.BindingContext is ImageInfo imageInfo)
        {
            var popup = new ImagePopup(imageInfo.Path ?? "", imageInfo.Title ?? "", imageInfo.Description ?? "");
            this.ShowPopup(popup);
        }
    }
}


