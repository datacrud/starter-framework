using System.Web.Http.ExceptionHandling;

namespace Project.Server.Middlewares
{
    public class GlobalExceptionLogger : ExceptionLogger
    {
        public override void Log(ExceptionLoggerContext context)
        {
            if (context.Exception != null) Serilog.Log.Error(context.Exception.ToString());
        }
    }
}