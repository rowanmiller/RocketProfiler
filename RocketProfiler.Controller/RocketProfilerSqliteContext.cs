// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using Microsoft.EntityFrameworkCore;

namespace RocketProfiler.Controller
{
    public class RocketProfilerSqliteContext : RocketProfilerContext
    {
        private readonly string _datatbaseName;

        public RocketProfilerSqliteContext(string datatbaseName)
        {
            _datatbaseName = datatbaseName;
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
            => optionsBuilder.UseSqlite("DataSource=" + _datatbaseName);
    }
}
