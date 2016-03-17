// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using Microsoft.EntityFrameworkCore.Infrastructure;

namespace RocketProfiler.Controller
{
    class RocketProfilerContextDesignTimeFactory : IDbContextFactory<RocketProfilerContext>
    {
        public RocketProfilerContext Create()
        {
            return new RocketProfilerContext("design_time_database.db", new Type[] { typeof(DesignTimeSensor) });
        }

        // Used only to introduce inheritance into the model so that discriminator columns are created
        private class DesignTimeSensor : Sensor
        {
            public override double MaxValue => 5;

            public override string Units => "None";

            public override SensorValue DoRead()
            {
                throw new NotImplementedException();
            }
        }
    }
}
