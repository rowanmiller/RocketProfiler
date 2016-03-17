// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

namespace RocketProfiler.Controller
{
    public class SqliteRunRepository : RunRepository
    {
        private readonly string _datatbaseName;

        public SqliteRunRepository(string filename)
        {
            _datatbaseName = filename;
        }

        protected override RocketProfilerContext CreateContext()
            => new RocketProfilerSqliteContext(_datatbaseName);
    }
}
