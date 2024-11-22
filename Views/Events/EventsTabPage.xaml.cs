using TiAppMovi.Models;
using TiAppMovi.Services;
using Microsoft.Maui.Controls;
using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Maui.Storage; // Para usar Preferences
using Microsoft.Maui.Controls.Maps;
using Microsoft.Maui.Maps; // Para uso de mapas

namespace TiAppMovi.Views.Events
{
    public partial class EventsTabPage : ContentPage
    {
        // Recuperamos el correo del usuario desde las preferencias guardadas
        public string userEmail = Preferences.Get("user_email", string.Empty);

        private readonly EventService _eventService;

        public EventsTabPage()
        {
            InitializeComponent();
            _eventService = new EventService();
        }

        protected override async void OnAppearing()
        {
            base.OnAppearing();

            // Verificamos si el correo es válido antes de intentar cargar los eventos
            if (!string.IsNullOrEmpty(userEmail))
            {
                await LoadEventsAsync();
            }
            else
            {
                await DisplayAlert("Error", "No se ha encontrado un correo válido", "OK");
            }
        }

        private async Task LoadEventsAsync()
        {
            try
            {
                if (!string.IsNullOrEmpty(userEmail))
                {
                    // Llamamos al servicio para obtener los eventos del usuario
                    var events = await _eventService.GetEventsByUserEmail(userEmail);

                    if (events != null && events.Count > 0)
                    {
                        // Limpiamos el StackLayout antes de agregar eventos
                        EventsStackLayout.Children.Clear();

                        foreach (var eventItem in events)
                        {
                            var frame = new Frame
                            {
                                Padding = 20,
                                BackgroundColor = Colors.White,
                                CornerRadius = 10,
                                BorderColor = Colors.Teal
                            };

                            var grid = new Grid
                            {
                                ColumnDefinitions =
                                [
                                    new ColumnDefinition { Width = new GridLength(1, GridUnitType.Star) },
                                    new ColumnDefinition { Width = new GridLength(2, GridUnitType.Star) }
                                ],
                                RowDefinitions =
                                [
                                    new RowDefinition { Height = GridLength.Auto },
                                    new RowDefinition { Height = GridLength.Auto },
                                    new RowDefinition { Height = GridLength.Auto }
                                ]
                            };

                            var image = new Image
                            {
                                Source = "mapa.png",
                                WidthRequest = 80,
                                HeightRequest = 80,
                                HorizontalOptions = LayoutOptions.Center,
                                VerticalOptions = LayoutOptions.Center
                            };
                            image.SetValue(Grid.RowProperty, 0);
                            image.SetValue(Grid.ColumnProperty, 0);
                            image.SetValue(Grid.RowSpanProperty, 2);

                            var nameAndDateLayout = new VerticalStackLayout
                            {
                                Children =
                                {
                                    new Label
                                    {
                                        Text = eventItem.Nombre,
                                        FontSize = 18,
                                        FontAttributes = FontAttributes.Bold,
                                        TextColor = Colors.Teal
                                    },
                                    new Label
                                    {
                                        Text = eventItem.Fecha.ToString("D"),
                                        FontSize = 14,
                                        TextColor = Colors.Gray
                                    }
                                }
                            };
                            nameAndDateLayout.SetValue(Grid.RowProperty, 0);
                            nameAndDateLayout.SetValue(Grid.ColumnProperty, 1);

                            var locationLayout = new VerticalStackLayout
                            {
                                Children =
                                {
                                    new Label
                                    {
                                        Text = "Dirección:",
                                        FontAttributes = FontAttributes.Bold,
                                        FontSize = 14,
                                        TextColor = Colors.Teal
                                    },
                                    new Label
                                    {
                                        Text = eventItem.Ubicacion,
                                        FontSize = 14,
                                        TextColor = Colors.Gray
                                    }
                                }
                            };
                            locationLayout.SetValue(Grid.RowProperty, 1);
                            locationLayout.SetValue(Grid.ColumnProperty, 1);

                            var descriptionLayout = new VerticalStackLayout
                            {
                                Children =
                                {
                                    new Label
                                    {
                                        Text = "Descripción:",
                                        FontAttributes = FontAttributes.Bold,
                                        FontSize = 14,
                                        TextColor = Colors.Teal
                                    },
                                    new Label
                                    {
                                        Text = eventItem.Descripcion,
                                        FontSize = 14,
                                        TextColor = Colors.Gray
                                    }
                                }
                            };
                            descriptionLayout.SetValue(Grid.RowProperty, 2);
                            descriptionLayout.SetValue(Grid.ColumnProperty, 0);
                            descriptionLayout.SetValue(Grid.ColumnSpanProperty, 2);

                            grid.Children.Add(image);
                            grid.Children.Add(nameAndDateLayout);
                            grid.Children.Add(locationLayout);
                            grid.Children.Add(descriptionLayout);

                            frame.Content = grid;
                            EventsStackLayout.Children.Add(frame);
                        }
                    }
                    else
                    {
                        await DisplayAlert("Aviso", "No hay eventos disponibles para este usuario.", "OK");
                    }
                }
                else
                {
                    await DisplayAlert("Error", "No se encontró un correo válido.", "OK");
                }
            }
            catch (Exception ex)
            {
                await DisplayAlert("Error", $"No se pudieron cargar los eventos: {ex.Message}", "OK");
            }
        }

        private async void OnSearchButtonClicked(object sender, EventArgs e)
        {
            string address = SearchEntry.Text;

            if (!string.IsNullOrWhiteSpace(address))
            {
                try
                {
                    var locations = await Geocoding.GetLocationsAsync(address);
                    var location = locations?.FirstOrDefault();

                    if (location != null)
                    {
                        map.MoveToRegion(MapSpan.FromCenterAndRadius(
                            new Location(location.Latitude, location.Longitude),
                            Distance.FromKilometers(1)));
                    }
                    else
                    {
                        await DisplayAlert("Error", "No se encontró la dirección.", "OK");
                    }
                }
                catch (Exception ex)
                {
                    await DisplayAlert("Error", $"No se pudo realizar la búsqueda: {ex.Message}", "OK");
                }
            }
            else
            {
                await DisplayAlert("Aviso", "Por favor, ingrese una dirección.", "OK");
            }
        }
    }
}