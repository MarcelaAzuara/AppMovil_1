using Microsoft.Maui.Controls;
using Microsoft.Maui.Storage; // Para usar Preferences
using System;

namespace TiAppMovi.Views.Legal
{
    public partial class TermsAndConditionsPage : ContentPage
    {
        public TermsAndConditionsPage()
        {
            InitializeComponent();
        }

        private async void OnAcceptClicked(object sender, EventArgs e)
        {
            try
            {
                // Guardamos en preferencias que el usuario ha aceptado los t�rminos
                Preferences.Set("HasAcceptedTerms", true);

                // Cambiamos la MainPage a LoginPage despu�s de aceptar los t�rminos
                if (Application.Current != null)
                {
                    Application.Current.MainPage = new NavigationPage(new LoginPage()); // Redirige a la p�gina principal
                }
                else
                {
                    await DisplayAlert("Error", "No se pudo cambiar la p�gina principal", "OK");
                }
            }
            catch (Exception ex)
            {
                await DisplayAlert("Error", "Hubo un problema al aceptar los t�rminos: " + ex.Message, "OK");
            }
        }

        private async void OnRejectClicked(object sender, EventArgs e)
        {
            try
            {
                // Mostrar alerta si el usuario no acepta los t�rminos
                await DisplayAlert("Aviso", "Debes aceptar los t�rminos y condiciones para continuar.", "OK");
            }
            catch (Exception ex)
            {
                await DisplayAlert("Error", "Hubo un problema al rechazar los t�rminos: " + ex.Message, "OK");
            }
        }
    }
}
