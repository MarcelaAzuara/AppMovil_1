using TiAppMovi.Models;
using TiAppMovi.Services;
using Microsoft.Maui.Controls;
using System;
using System.Threading.Tasks;
using Microsoft.Maui.Storage; // Para usar Preferences

namespace TiAppMovi.Views.Profile
{
    public partial class ProfilePage : ContentPage
    {
        // Recuperamos el correo del usuario desde las preferencias guardadas
        public string userEmail = Preferences.Get("user_email", string.Empty); // Recupera el correo almacenado en Preferences

        public ProfilePage()
        {
            InitializeComponent();
        }

        protected override async void OnAppearing()
        {
            base.OnAppearing();
            // Verificamos si el correo es válido antes de intentar cargar los datos del usuario
            if (!string.IsNullOrEmpty(userEmail))
            {
                await LoadUserDataAsync();
            }
            else
            {
                await DisplayAlert("Error", "No se ha encontrado un correo válido", "OK");
            }
        }

        private async Task LoadUserDataAsync()
        {
            try
            {
                // Verificamos explícitamente si userEmail no es null o vacío
                if (!string.IsNullOrEmpty(userEmail))
                {
                    // Llamamos al servicio estático para obtener los datos del usuario
                    User user = await ProfileService.GetUserDataAsync(userEmail!); // Operador de aserción de nulidad '!' 

                    if (user != null)
                    {
                        // Asignar los datos al perfil
                        NombreLabel.Text = user.Nombre;
                        CorreoLabel.Text = user.Email;
                        TelefonoLabel.Text = user.Telefono;
                    }
                    else
                    {
                        await DisplayAlert("Error", "No se encontraron datos para este usuario.", "OK");
                    }
                }
                else
                {
                    await DisplayAlert("Error", "No se encontró un correo válido.", "OK");
                }
            }
            catch (Exception ex)
            {
                await DisplayAlert("Error", "Hubo un problema al cargar los datos: " + ex.Message, "OK");
            }
        }

        private async void OnLogoutClicked(object sender, EventArgs e)
        {
            try
            {
                // Eliminar solo las preferencias relacionadas con el correo (usuario)
                Preferences.Remove("user_email");

                // Mostrar mensaje de éxito
                await DisplayAlert("Cerrar sesión", "Sesión cerrada correctamente", "OK");

                // Redirigir al login
                if (Application.Current != null)
                {
                    Application.Current.MainPage = new NavigationPage(new LoginPage());
                }
                else
                {
                    // Manejar el caso donde Application.Current sea null, si es necesario
                    await DisplayAlert("Error", "No se puede redirigir al login. La aplicación no está completamente cargada.", "OK");
                }
            }
            catch (Exception ex)
            {
                await DisplayAlert("Error", "Hubo un problema al cerrar sesión: " + ex.Message, "OK");
            }
        }
    }
}
