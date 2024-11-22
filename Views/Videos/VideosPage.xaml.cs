using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.Maui.Controls;

namespace TiAppMovi.Views.Videos
{
    public partial class VideosPage : ContentPage
    {
        private readonly string apiKey = "AIzaSyC_WUyMEN5v2qG9vEwxC5GhVyYUMU35XyI";
        private readonly string baseUrl = "https://www.googleapis.com/youtube/v3/search";

        public VideosPage()
        {
            InitializeComponent();
        }

        private async void OnSearchButtonClicked(object sender, EventArgs e)
        {
            string query = SearchEntry?.Text;
            if (string.IsNullOrEmpty(query))
            {
                await DisplayAlert("Error", "Por favor, ingresa un término de búsqueda.", "OK");
                return;
            }
            query = query.Trim();

            await FetchVideosAsync(query);
        }

        private async Task FetchVideosAsync(string query)
        {
            try
            {
                using HttpClient client = new HttpClient();
                string requestUrl = $"{baseUrl}?part=snippet&type=video&q={query}&key={apiKey}&maxResults=5";

                HttpResponseMessage response = await client.GetAsync(requestUrl);
                if (response.IsSuccessStatusCode)
                {
                    string jsonResponse = await response.Content.ReadAsStringAsync();
                    var videos = JsonSerializer.Deserialize<YouTubeResponse>(jsonResponse);

                    if (videos?.Items != null && videos.Items.Count > 0)
                    {
                        VideosCollection.ItemsSource = videos.Items; // Asigna los resultados al CollectionView
                    }
                    else
                    {
                        await DisplayAlert("Información", "No se encontraron videos.", "OK");
                    }
                }
                else
                {
                    await DisplayAlert("Error", "No se pudieron cargar los videos.", "OK");
                }
            }
            catch (Exception ex)
            {
                await DisplayAlert("Error", $"Ocurrió un error: {ex.Message}", "OK");
            }
        }
    }

    // Clases para deserializar la respuesta de la API
    public class YouTubeResponse
    {
        public List<YouTubeVideo> Items { get; set; } = new List<YouTubeVideo>();
    }

    public class YouTubeVideo
    {
        public Snippet Snippet { get; set; } = new Snippet();
    }

    public class Snippet
    {
        public string Title { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public Thumbnails Thumbnails { get; set; } = new Thumbnails();
    }

    public class Thumbnails
    {
        public Thumbnail Default { get; set; } = new Thumbnail();
    }

    public class Thumbnail
    {
        public string Url { get; set; } = string.Empty;
    }
}
