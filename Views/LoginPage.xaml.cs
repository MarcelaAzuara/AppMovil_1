using Npgsql;
using Microsoft.Maui.Storage;  // Necesario para usar Preferences

namespace TiAppMovi.Views
{
    public partial class LoginPage : ContentPage
    {
        // Cadena de conexi�n a PostgreSQL
        private readonly string connectionString = "Host=dpg-csro3hbtq21c739n3otg-a.oregon-postgres.render.com;Port=5432;Database=eventnegotiation;Username=potaxie;Password=taWNv9D0ytl74GZU1b5IVhQloBncNKkT;";

        public LoginPage()
        {
            InitializeComponent();
        }

        private async void OnLoginButtonClicked(object sender, EventArgs e)
        {
            string email = GmailEntry.Text;
            string password = PasswordEntry.Text;

            if (string.IsNullOrEmpty(email) || string.IsNullOrEmpty(password))
            {
                await DisplayAlert("Error", "Por favor ingresa tu correo electr�nico y contrase�a", "OK");
                return;
            }

            bool loginSuccess = await ValidateUserCredentialsAsync(email, password);

            if (loginSuccess)
            {
                // Guardamos el correo electr�nico en Preferences
                Preferences.Set("user_email", email.Trim());  // Guardamos el correo en Preferences

                // Verifica que Application.Current no sea null antes de usarlo
                if (Application.Current != null)
                {
                    // Redirige al AppShell si el login es exitoso
                    Application.Current.MainPage = new AppShell();
                }
                else
                {
                    // Si Application.Current es null, muestra un mensaje de error
                    await DisplayAlert("Error", "No se puede acceder a la aplicaci�n", "OK");
                }
            }
            else
            {
                // Muestra un mensaje de error si las credenciales no son v�lidas
                await DisplayAlert("Error", "Usuario o contrase�a incorrectos, por favor intentalo de nuevo", "OK");
            }
        }

        private async Task<bool> ValidateUserCredentialsAsync(string email, string password)
        {
            try
            {
                using (var connection = new NpgsqlConnection(connectionString))
                {
                    await connection.OpenAsync();

                    string query = "SELECT password FROM usuario WHERE LOWER(email) = LOWER(@email)"; // Insensibilidad a may�sculas/min�sculas

                    using (var command = new NpgsqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("email", email.Trim()); // Elimina espacios innecesarios

                        var result = await command.ExecuteScalarAsync();

                        // Verifica si el resultado no es nulo y es una cadena
                        if (result is string hashedPassword)
                        {
                            // Verifica si la contrase�a ingresada coincide con el hash almacenado
                            bool isValid = BCrypt.Net.BCrypt.Verify(password.Trim(), hashedPassword); // Aseg�rate de que la contrase�a est� limpia

                            if (isValid)
                            {
                                return true; // Contrase�a correcta
                            }
                            else
                            {
                                await DisplayAlert("Error", "Contrase�a incorrecta", "OK");
                                return false; // Contrase�a incorrecta
                            }
                        }
                        else
                        {
                            await DisplayAlert("Error", "No se encontr� el usuario o la contrase�a est� mal almacenada", "OK");
                            return false; // Usuario no encontrado o la contrase�a es nula
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                await DisplayAlert("Error", $"Error de conexi�n: {ex.Message}", "OK");
                return false;
            }
        }

        private void OnRememberMeCheckedChanged(object sender, CheckedChangedEventArgs e)
        {
            // L�gica para recordar el usuario
        }

        private void OnTogglePasswordVisibility(object sender, EventArgs e)
        {
            PasswordEntry.IsPassword = !PasswordEntry.IsPassword;
        }
    }
}
