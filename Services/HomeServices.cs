using Npgsql;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using TiAppMovi.Models;

namespace TiAppMovi.Services
{
    public class HomeService
    {
        private const string ConnectionString = "Host=dpg-csro3hbtq21c739n3otg-a.oregon-postgres.render.com;Port=5432;Database=eventnegotiation;Username=potaxie;Password=taWNv9D0ytl74GZU1b5IVhQloBncNKkT";

        public async Task<List<Home>> GetHomeEventsByUserEmail(string email)
        {
            var events = new List<Home>();

            using (var connection = new NpgsqlConnection(ConnectionString))
            {
                await connection.OpenAsync();

                string query = @"
                SELECT e.nombre, e.fecha 
                FROM eventos e 
                JOIN usuario u ON u.usuario_id = e.creador_id 
                WHERE u.email = @email;";

                using var command = new NpgsqlCommand(query, connection);
                command.Parameters.AddWithValue("@email", email);

                using var reader = await command.ExecuteReaderAsync();
                while (await reader.ReadAsync())
                {
                    events.Add(new Home(
                        reader.GetString(0),  // Nombre
                        reader.GetDateTime(1) // Fecha
                    ));
                }
            }

            return events;
        }
    }
}

