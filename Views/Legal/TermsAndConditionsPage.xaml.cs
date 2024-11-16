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
            // Guardamos en preferencias que el usuario ha aceptado los t�rminos
            Preferences.Set("HasAcceptedTerms", true);

            // Cambiamos la MainPage a LoginPage despu�s de aceptar los t�rminos
            Application.Current.MainPage = new NavigationPage(new LoginPage());
        }

        private async void OnRejectClicked(object sender, EventArgs e)
        {
            // Informamos al usuario que debe aceptar los t�rminos para continuar
            await DisplayAlert("Aviso", "Debes aceptar los t�rminos y condiciones para continuar.", "OK");
        }
    }
}
