using TiAppMovi.Services;
using Npgsql;
using System.Collections.Generic;
using System.Threading.Tasks;
using TiAppMovi.Models;

namespace TiAppMovi.Services
{
    public class EventService
    {
        private const string ConnectionString = "Host=dpg-csro3hbtq21c739n3otg-a.oregon-postgres.render.com;Port=5432;Database=eventnegotiation;Username=potaxie;Password=taWNv9D0ytl74GZU1b5IVhQloBncNKkT";

        public async Task<List<Event>> GetEventsByUserEmail(string email)
        {
            var events = new List<Event>();

            using (var connection = new NpgsqlConnection(ConnectionString))
            {
                await connection.OpenAsync();

                // Ajustamos la consulta para hacer el JOIN entre 'eventos' y 'usuario'
                string query = @"
                SELECT e.nombre, e.fecha, e.ubicacion, e.descripcion
                FROM eventos e
                JOIN usuario u ON u.usuario_id = e.creador_id
                WHERE u.email = @email";

                using var command = new NpgsqlCommand(query, connection);
                command.Parameters.AddWithValue("@email", email);

                // Usamos un solo bloque using para ambos 'command' y 'reader'
                using var reader = await command.ExecuteReaderAsync();
                while (await reader.ReadAsync())
                {
                    events.Add(new Event(
                        reader.GetString(0),  // Nombre
                        reader.GetString(2),  // Ubicacion
                        reader.GetString(3),  // Descripcion
                        reader.GetDateTime(1) // Fecha
                    ));
                }
            }

            return events;
        }
    }
}
