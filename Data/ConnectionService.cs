using Microsoft.Extensions.Configuration;
using System;
using Npgsql;

namespace BlogProject.Data
{
    public class ConnectionService
    {
        // Public connection method. GetConnectionString of connectionString and databaseUrl
        // BuildConnectionString(databaseUrl) or if null, return DefaultConnection
        public static string GetConnectionString(IConfiguration configuration)
        {
            var connectionString = configuration.GetConnectionString("DefaultConnection");
            var databaseUrl = Environment.GetEnvironmentVariable("DATABASE_URL");
            return string.IsNullOrEmpty(databaseUrl) ? connectionString : BuildConnectionString(databaseUrl);
        }

        // Private string method. BuildConnectionString(databaseUrl) via NpgsqlConnectionStringBuilder()
        private static string BuildConnectionString(string databaseUrl)
        {
            var databaseUri = new Uri(databaseUrl);
            var userInfo = databaseUri.UserInfo.Split(':');

            // Build and return new NpgsqlConnectionStringBuilder() based on databaseUri and userInfo 
            return new NpgsqlConnectionStringBuilder()
            {
                Host = databaseUri.Host,
                Port = databaseUri.Port,
                Username = userInfo[0],
                Password = userInfo[1],
                Database = databaseUri.LocalPath.TrimStart('/'),
                SslMode = SslMode.Prefer,
                TrustServerCertificate = true
            }.ToString();
        }

    }
}
