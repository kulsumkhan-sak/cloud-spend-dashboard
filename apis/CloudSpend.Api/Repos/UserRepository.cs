using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;

namespace CloudSpend.Api.Repos
{
    public class UserRepository
    {
        private readonly string _connectionString;

        public UserRepository(IConfiguration configuration)
        {
            _connectionString =
                configuration.GetConnectionString("DefaultConnection")
                ?? throw new Exception("DefaultConnection is missing");
        }

        public string? GetPasswordHashByEmail(string email)
        {
            using var connection = new SqlConnection(_connectionString);
            connection.Open();

            var command = new SqlCommand(
                "SELECT PasswordHash FROM Users WHERE Email = @Email",
                connection
            );

            command.Parameters.AddWithValue("@Email", email);

            return command.ExecuteScalar() as string;
        }
    }
}