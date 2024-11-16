namespace TiAppMovi.Views
{
    public partial class LoginPage : ContentPage
    {
        public LoginPage()
        {
            InitializeComponent();
        }

        private async void OnLoginButtonClicked(object sender, EventArgs e)
        {
            // Aqu�, debes validar las credenciales del usuario. Esto es solo un ejemplo.
            bool loginSuccess = true; // Simulando un login exitoso. Debes implementar la l�gica real aqu�.

            if (loginSuccess)
            {
                // Si el login es exitoso, redirige al AppShell
                Application.Current.MainPage = new AppShell();
            }
            else
            {
                // Si las credenciales son incorrectas, muestra un mensaje de error.
                await DisplayAlert("Error", "Credenciales incorrectas", "OK");
            }
        }

        private void OnRememberMeCheckedChanged(object sender, CheckedChangedEventArgs e)
        {
            // Aqu� puedes agregar l�gica para recordar al usuario, si lo deseas.
            if (e.Value)
            {
                // Acci�n cuando "Recordar" est� activado
            }
            else
            {
                // Acci�n cuando "Recordar" est� desactivado
            }
        }
    }
}
