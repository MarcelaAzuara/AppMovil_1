using System;
using System.Threading.Tasks;
using Npgsql;
using TiAppMovi.Models; // Asegúrate de que 'User' está en este espacio de nombres

namespace TiAppMovi.Services
{
    public class ProfileService
    {
        private const string connectionString = "Host=dpg-csro3hbtq21c739n3otg-a.oregon-postgres.render.com;Port=5432;Database=eventnegotiation;Username=potaxie;Password=taWNv9D0ytl74GZU1b5IVhQloBncNKkT";

        // Método estático para obtener los datos del usuario desde la base de datos
        public static async Task<User?> GetUserDataAsync(string email)
        {
            try
            {
                if (string.IsNullOrEmpty(email))
                {
                    throw new ArgumentException("El correo electrónico no puede ser nulo o vacío.");
                }

                using (var conn = new NpgsqlConnection(connectionString))
                {
                    await conn.OpenAsync();

                    string query = "SELECT nombre, email, telefono FROM usuario WHERE email = @email";

                    using (var cmd = new NpgsqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("email", email);

                        using (var reader = await cmd.ExecuteReaderAsync())
                        {
                            if (await reader.ReadAsync())
                            {
                                return new User
                                {
                                    Nombre = reader.GetString(0),
                                    Email = reader.GetString(1),
                                    Telefono = reader.GetString(2)
                                };
                            }
                            else
                            {
                                return null;  // Permite retornar null si no se encuentra al usuario
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error al obtener los datos del usuario: " + ex.Message);
                throw new Exception("No se pudieron cargar los datos del usuario", ex);
            }
        }
    }
}