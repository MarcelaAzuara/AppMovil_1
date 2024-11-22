using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using TiAppMovi.Models;
using TiAppMovi.Services;  // Asegúrate de tener el servicio adecuado
using Microsoft.Maui.Controls;
using Microsoft.Maui.Storage; // Para usar Preferences

namespace TiAppMovi.Views
{
    public partial class HomePage : ContentPage
    {
        private readonly HomeService _homeService;
        public string UserEmail = Preferences.Get("user_email", string.Empty);

        public HomePage()
        {
            InitializeComponent();
            _homeService = new HomeService();
        }

        protected override async void OnAppearing()
        {
            base.OnAppearing();

            if (!string.IsNullOrEmpty(UserEmail))
            {
                await LoadHomeEventsAsync();
            }
            else
            {
                await DisplayAlert("Error", "No se ha encontrado un correo válido", "OK");
            }
        }

        private async Task LoadHomeEventsAsync()
        {
            try
            {
                var events = await _homeService.GetHomeEventsByUserEmail(UserEmail);

                if (events != null && events.Count > 0)
                {
                    EventosStackLayout.Children.Clear();

                    foreach (var eventItem in events)
                    {
                        var grid = new Grid
                        {
                            ColumnDefinitions =
                            [
                                new ColumnDefinition { Width = GridLength.Star },
                                new ColumnDefinition { Width = GridLength.Auto }
                            ]
                        };

                        // Texto del evento en la primera columna
                        var textLayout = new VerticalStackLayout
                        {
                            Spacing = 5,
                            Children =
                            {
                                new Label
                                {
                                    Text = eventItem.Nombre,
                                    FontSize = 18,
                                    FontAttributes = FontAttributes.Bold,
                                    TextColor = Color.FromArgb("#009b84") // Uso de FromArgb
                                },
                                new Label
                                {
                                    Text = eventItem.Fecha.ToString("D"),
                                    FontSize = 14,
                                    TextColor = Colors.Gray
                                }
                            }
                        };
                        grid.Children.Add(textLayout);
                        Grid.SetColumn(textLayout, 0);

                        // Botón de flecha
                        var imageButton = new ImageButton
                        {
                            Source = "dere.png",
                            WidthRequest = 90,
                            HeightRequest = 35,
                            BackgroundColor = Colors.Transparent,
                            VerticalOptions = LayoutOptions.Center,
                            HorizontalOptions = LayoutOptions.Center
                        };
                        grid.Children.Add(imageButton);
                        Grid.SetColumn(imageButton, 1); // Asignar la segunda columna

                        // Contenedor principal del evento
                        var frame = new Frame
                        {
                            Padding = 10,
                            BorderColor = Color.FromArgb("#009b84"), // Uso de FromArgb
                            CornerRadius = 10,
                            BackgroundColor = Colors.White,
                            HasShadow = true,
                            Content = grid // El Grid es el contenido del Frame
                        };

                        EventosStackLayout.Children.Add(frame);
                    }
                }
                else
                {
                    await DisplayAlert("Aviso", "No hay eventos disponibles.", "OK");
                }
            }
            catch (Exception ex)
            {
                await DisplayAlert("Error", $"Error al cargar eventos: {ex.Message}", "OK");
            }
        }
    }
}
