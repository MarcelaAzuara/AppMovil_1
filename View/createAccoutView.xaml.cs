using System;
using Microsoft.Maui.Controls;

namespace MauiApp1.View
{
    public partial class createAccoutView : ContentPage
    {
        public createAccoutView()
        {
            InitializeComponent();
        }

        private async void OnTermsCheckBoxCheckedChanged(object sender, CheckedChangedEventArgs e)
        {
            if (e.Value)
            {
                await Navigation.PushModalAsync(new TermsAndConditionsPage());
            }
        }
    }
}
