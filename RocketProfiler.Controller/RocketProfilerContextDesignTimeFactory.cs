// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using Microsoft.EntityFrameworkCore.Infrastructure;

namespace RocketProfiler.Controller
{
    internal class RocketProfilerContextDesignTimeFactory : IDbContextFactory<RocketProfilerContext>
    {
        public RocketProfilerContext Create()
            => new RocketProfilerContext("design_time_database.db");
    }
}
