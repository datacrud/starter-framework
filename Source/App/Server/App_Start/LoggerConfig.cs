using System.Web;
using Project.Core;
using Serilog;
using Serilog.Core;
using Serilog.Events;

namespace Project.Server
{
    public class LoggerConfig
    {
        public static void Register()
        {

            var logFilePath = HttpRuntime.AppDomainAppPath + AppConst.LoggerPath;

            Log.Logger = new LoggerConfiguration()
                .MinimumLevel.Verbose()
                //.WriteTo.Console()
                .WriteTo.File(logFilePath, rollingInterval: RollingInterval.Month,
                    restrictedToMinimumLevel: LogEventLevel.Verbose,
                    levelSwitch: new LoggingLevelSwitch(LogEventLevel.Verbose))
                .CreateLogger();

        }
    }
}