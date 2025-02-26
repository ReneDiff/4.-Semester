using CommunityToolkit.Maui.Views;
using MauiAppWeek2.Models;
using Microsoft.Maui.Controls;

namespace MauiAppWeek2.Views;

public partial class ImagePopup : Popup
{
    public ImagePopup(string imagePath, string title, string description)
    {
        InitializeComponent();
        
        // Sæt billedet, titel og beskrivelse manuelt
        PopupImage.Source = imagePath;
        PopupTitle.Text = title;
        PopupDescription.Text = description;
    }

    private void OnCloseButtonClicked(object sender, EventArgs e)
    {
        Close();
    }
}
