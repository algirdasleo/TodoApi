using System.Data.SQLite;

namespace TodoApi.Helpers
{
    public class SQLConnectionFactory
    {
        private readonly string _connectionString;
        
        public SQLConnectionFactory(string connectionString)
        {
            _connectionString = connectionString;
        }

        public async Task<SQLiteConnection> CreateConnectionAsync()
        {
            var connection = new SQLiteConnection(_connectionString);
            await connection.OpenAsync();
            return connection;
        }
    }
}