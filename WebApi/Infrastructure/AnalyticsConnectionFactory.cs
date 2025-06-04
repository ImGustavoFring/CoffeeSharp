using Npgsql;
using System.Data;
using Microsoft.Extensions.Options;
using WebApi.Configurations; 
namespace WebApi.Infrastructure
{
    public interface IAnalyticsConnectionFactory
    {
        IDbConnection CreateConnection();
    }

    public class AnalyticsConnectionFactory : IAnalyticsConnectionFactory
    {
        private readonly string _connectionString;

        public AnalyticsConnectionFactory(IOptions<DatabaseSettings> dbSettingsOptions)
        {
            var dbSettings = dbSettingsOptions.Value;
            _connectionString = dbSettings.DefaultConnection;
        }

        public IDbConnection CreateConnection() => new NpgsqlConnection(_connectionString);
    }
}