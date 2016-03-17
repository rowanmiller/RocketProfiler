// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

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

        public static string BuildAzureConnectionString(string databaseName)
        {
            var server = System.Environment.GetEnvironmentVariable("RocketProfilerServer");
            var user = System.Environment.GetEnvironmentVariable("RocketProfilerUser");
            var password = System.Environment.GetEnvironmentVariable("RocketProfilerPassword");

            return $"Server={server}; Database={databaseName}; User ID={user}; Password={password}; Trusted_Connection=False; Encrypt=True;";
        }
    }
}
