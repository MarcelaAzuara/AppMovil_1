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
            // Aquí, debes validar las credenciales del usuario. Esto es solo un ejemplo.
            bool loginSuccess = true; // Simulando un login exitoso. Debes implementar la lógica real aquí.

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
            // Aquí puedes agregar lógica para recordar al usuario, si lo deseas.
            if (e.Value)
            {
                // Acción cuando "Recordar" está activado
            }
            else
            {
                // Acción cuando "Recordar" está desactivado
            }
        }
    }
}
