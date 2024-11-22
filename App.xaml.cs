using Microsoft.Maui.Controls;
using Microsoft.Maui.Storage;
using TiAppMovi.Views.Legal; // Para acceder a la página de términos
using TiAppMovi.Views; // Para acceder a la página principal después de aceptar los términos

namespace TiAppMovi
{
    public partial class App : Application
    {
        public App()
        {
            InitializeComponent();

            // Verificamos si el usuario ha aceptado los términos
            if (Preferences.Get("HasAcceptedTerms", false)) // Si ya aceptó los términos
            {
                // Redirigir a la página principal (LoginPage o la que sea que uses como página principal)
                MainPage = new NavigationPage(new LoginPage());
            }
            else
            {
                // Si no ha aceptado, redirigimos a los términos y condiciones
                MainPage = new NavigationPage(new TermsAndConditionsPage());
            }
        }

        protected override void OnStart()
        {
            // Manejo del inicio de la aplicación, por ejemplo
        }

        protected override void OnSleep()
        {
            // Manejo cuando la app se pone en segundo plano
        }

        protected override void OnResume()
        {
            // Manejo cuando la app vuelve a primer plano
        }
    }
}
