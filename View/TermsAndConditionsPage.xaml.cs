using System;
using Microsoft.Maui.Controls;

namespace MauiApp1.View
{
    public partial class TermsAndConditionsPage : ContentPage
    {
        public TermsAndConditionsPage()
        {
            InitializeComponent();
        }

        private async void OnAcceptButtonClicked(object sender, EventArgs e)
        {
            await Navigation.PopModalAsync();
        }
    }
}