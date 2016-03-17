// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using Microsoft.EntityFrameworkCore.Infrastructure;

namespace RocketProfiler.Controller
{
    internal class RocketProfilerContextDesignTimeFactory : IDbContextFactory<RocketProfilerSqliteContext>
    {
        public RocketProfilerSqliteContext Create()
            => new RocketProfilerSqliteContext("design_time_database.db");
    }
}
