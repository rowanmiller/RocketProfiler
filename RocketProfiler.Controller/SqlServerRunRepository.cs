// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;

namespace RocketProfiler.Controller
{
    public class SqlServerRunRepository : RunRepository
    {
        private readonly string _connectionString;

        public SqlServerRunRepository(string connectionString)
        {
            _connectionString = connectionString;
        }

        protected override RocketProfilerContext CreateContext()
            => new RocketProfilerSqlServerContext(_connectionString);

        public static string GetAvailableDbConnectionString(string databaseName) 
            => FindAzureConnectionString(databaseName)
            ?? GetLocalDbConnectionString(databaseName);

        public static string GetLocalDbConnectionString(string databaseName) 
            => $"Data Source=(localdb)\\MSSQLLocalDB;Initial Catalog={databaseName}";

        public static string GetAzureConnectionString(string databaseName)
        {
            var connectionString = FindAzureConnectionString(databaseName);
            if (connectionString == null)
            {
                throw new InvalidOperationException(
                    "Configure RocketProfilerServer, RocketProfilerUser, and RocketProfilerPassword to use Azure database.");
            }
            return connectionString;
        }

        public static string FindAzureConnectionString(string databaseName)
        {
            var server = Environment.GetEnvironmentVariable("RocketProfilerServer");
            var user = Environment.GetEnvironmentVariable("RocketProfilerUser");
            var password = Environment.GetEnvironmentVariable("RocketProfilerPassword");

            return server == null || user == null || password == null
                ? null
                : $"Server={server}; Database={databaseName}; User ID={user}; Password={password}; Trusted_Connection=False; Encrypt=True;";
        }
    }
}
