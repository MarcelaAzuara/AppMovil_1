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
            // Guardamos en preferencias que el usuario ha aceptado los términos
            Preferences.Set("HasAcceptedTerms", true);

            // Cambiamos la MainPage a LoginPage después de aceptar los términos
            Application.Current.MainPage = new NavigationPage(new LoginPage());
        }

        private async void OnRejectClicked(object sender, EventArgs e)
        {
            // Informamos al usuario que debe aceptar los términos para continuar
            await DisplayAlert("Aviso", "Debes aceptar los términos y condiciones para continuar.", "OK");
        }
    }
}
