using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using MyBGList.Models;

namespace MyBGList.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class BoardGamesController : ControllerBase
    {
        private readonly string _connectionString = "Data Source=localhost,1433;Database=BoardGames;User Id=SA;Password=LavineDamp59;TrustServerCertificate=True";
        private readonly ILogger<BoardGamesController> _logger;
        public BoardGamesController(ILogger<BoardGamesController> logger)
        {
            _logger = logger;
        }

        [HttpGet(Name = "GetBoardGames")]
        public async Task<ActionResult<IEnumerable<BoardGame>>> Get()
        {
            var boardGames = new List<BoardGame>();

            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                string query = "SELECT Id, Name, Year FROM BoardGame;";
                SqlCommand command = new SqlCommand(query, connection);

                await connection.OpenAsync();
                using (SqlDataReader reader = await command.ExecuteReaderAsync())
                {
                    while (await reader.ReadAsync())
                    {
                        boardGames.Add(new BoardGame
                        {
                            Id = reader.GetInt32(0),
                            Name = reader.GetString(1),
                            Year = reader.GetInt32(2)
                        });
                    }
                }
            }

            return Ok(boardGames);
        }
    }
}
