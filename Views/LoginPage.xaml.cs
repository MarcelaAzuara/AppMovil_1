using Npgsql;
using Microsoft.Maui.Storage;  // Necesario para usar Preferences

namespace TiAppMovi.Views
{
    public partial class LoginPage : ContentPage
    {
        // Cadena de conexión a PostgreSQL
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
                await DisplayAlert("Error", "Por favor ingresa tu correo electrónico y contraseña", "OK");
                return;
            }

            bool loginSuccess = await ValidateUserCredentialsAsync(email, password);

            if (loginSuccess)
            {
                // Guardamos el correo electrónico en Preferences
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
                    await DisplayAlert("Error", "No se puede acceder a la aplicación", "OK");
                }
            }
            else
            {
                // Muestra un mensaje de error si las credenciales no son válidas
                await DisplayAlert("Error", "Usuario o contraseña incorrectos, por favor intentalo de nuevo", "OK");
            }
        }

        private async Task<bool> ValidateUserCredentialsAsync(string email, string password)
        {
            try
            {
                using (var connection = new NpgsqlConnection(connectionString))
                {
                    await connection.OpenAsync();

                    string query = "SELECT password FROM usuario WHERE LOWER(email) = LOWER(@email)"; // Insensibilidad a mayúsculas/minúsculas

                    using (var command = new NpgsqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("email", email.Trim()); // Elimina espacios innecesarios

                        var result = await command.ExecuteScalarAsync();

                        // Verifica si el resultado no es nulo y es una cadena
                        if (result is string hashedPassword)
                        {
                            // Verifica si la contraseña ingresada coincide con el hash almacenado
                            bool isValid = BCrypt.Net.BCrypt.Verify(password.Trim(), hashedPassword); // Asegúrate de que la contraseña esté limpia

                            if (isValid)
                            {
                                return true; // Contraseña correcta
                            }
                            else
                            {
                                await DisplayAlert("Error", "Contraseña incorrecta", "OK");
                                return false; // Contraseña incorrecta
                            }
                        }
                        else
                        {
                            await DisplayAlert("Error", "No se encontró el usuario o la contraseña está mal almacenada", "OK");
                            return false; // Usuario no encontrado o la contraseña es nula
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                await DisplayAlert("Error", $"Error de conexión: {ex.Message}", "OK");
                return false;
            }
        }

        private void OnRememberMeCheckedChanged(object sender, CheckedChangedEventArgs e)
        {
            // Lógica para recordar el usuario
        }

        private void OnTogglePasswordVisibility(object sender, EventArgs e)
        {
            PasswordEntry.IsPassword = !PasswordEntry.IsPassword;
        }
    }
}
