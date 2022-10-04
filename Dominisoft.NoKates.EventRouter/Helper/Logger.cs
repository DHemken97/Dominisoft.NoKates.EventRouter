using Dominisoft.Nokates.Common.Infrastructure.Configuration;
using Dominisoft.Nokates.Common.Infrastructure.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Dominisoft.NoKates.EventRouter.Helper
{
    public static class Logger
    {
        public static void LogDebug(string message)
        {
            ConfigurationValues.TryGetValue<bool>(out var IsDebug, "LogDebugMessages");
            if (IsDebug)
            LoggingHelper.LogMessage(message);
        }
    }
}
