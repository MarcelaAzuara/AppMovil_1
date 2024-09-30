using Microsoft.Maui.Controls;

namespace MauiApp1.View
{
    public partial class LoginView : ContentPage
    {
        public LoginView()
        {
            InitializeComponent();
        }

        private async void OnCreateUserClicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new createAccoutView());
        }
    }
}
