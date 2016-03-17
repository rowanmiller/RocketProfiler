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
    }
}