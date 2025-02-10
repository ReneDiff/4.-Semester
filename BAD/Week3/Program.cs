using System;
using System.Data.SqlClient;
using System.Threading.Tasks;
using Microsoft.Data.SqlClient;

class Program
{
    static async Task Main()
    {
        // Opdater adgangskoden til din database
        string connectionString = "Data Source=localhost,1433;Database=Movies;User Id=SA;Password=LavineDamp59;TrustServerCertificate=True";

        string queryString = "SELECT Title, Year FROM Movies";

        try
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                await connection.OpenAsync();
                Console.WriteLine("Forbindelse til databasen oprettet!");

                using (SqlCommand command = new SqlCommand(queryString, connection))
                using (SqlDataReader reader = await command.ExecuteReaderAsync())
                {
                    Console.WriteLine("\nFilm i databasen:");
                    while (await reader.ReadAsync())
                    {
                        Console.WriteLine($"{reader["Title"]}, {reader["Year"]}");
                    }
                }
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Fejl: {ex.Message}");
        }
    }
}