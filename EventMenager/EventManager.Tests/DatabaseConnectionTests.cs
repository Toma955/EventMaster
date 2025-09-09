using Microsoft.Data.SqlClient;
using Xunit;

namespace EventManager.Tests
{
    public class DatabaseConnectionTests
    {
        [Fact]
        public void CanConnectToDatabase()
        {
            var connectionString = "Server=(localdb)\\mssqllocaldb;Database=EventManagerDB;Integrated Security=true;TrustServerCertificate=true;MultipleActiveResultSets=true";

            using (var connection = new SqlConnection(connectionString))
            {
                try
                {
                    connection.Open();
                    Assert.Equal(System.Data.ConnectionState.Open, connection.State);
                }
                catch (SqlException ex)
                {
                    Assert.Fail($"Ne može se spojiti na bazu: {ex.Message}");
                }
            }
        }
    }
} 