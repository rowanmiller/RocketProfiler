// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using Microsoft.EntityFrameworkCore;

namespace RocketProfiler.Controller
{
    public class RocketProfilerSqlServerContext : RocketProfilerContext
    {
        private readonly string _connectionString;

        // Just easy for Migrations. :-)
        public RocketProfilerSqlServerContext()
        {
            _connectionString = "Data Source=(localdb)\\MSSQLLocalDB;Initial Catalog=RocketProfilerTest";
        }

        public RocketProfilerSqlServerContext(string connectionString)
        {
            _connectionString = connectionString;
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
            => optionsBuilder.UseSqlServer(_connectionString);
    }
}
