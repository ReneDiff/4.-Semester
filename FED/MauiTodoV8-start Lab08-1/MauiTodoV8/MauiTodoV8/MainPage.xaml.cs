using MauiTodoV8.ViewModels;

namespace MauiTodoV8
{
    public partial class MainPage : ContentPage
    {
        public MainPage()
        {
            InitializeComponent();
            BindingContext = new MainViewModel();
        }

        private async void SwipeItem_Invoked(object sender, EventArgs e)
        {
            var item = sender as SwipeItem;
            if (item != null)
            {
                if (App.Current != null)
                {
                    if (App.Current.MainPage != null) { 
                        await App.Current.MainPage.DisplayAlert(item.Text, $"You invoked the {item.Text} action.", "OK");
                    }
                }
            }
        }
    }

}
