using System;
using System.Threading;
using System.Threading.Tasks;
using System.Web.Http.ExceptionHandling;
using System.Web.Http.Results;

namespace Project.Server.Middlewares
{
    public class GlobalExceptionHandler : IExceptionHandler
    {
        public GlobalExceptionHandler(IExceptionHandler innerHandler)
        {
            if (innerHandler == null)
                throw new ArgumentNullException(nameof(innerHandler));

            InnerHandler = innerHandler;
        }

        public IExceptionHandler InnerHandler { get; }

        public Task HandleAsync(ExceptionHandlerContext context, CancellationToken cancellationToken)
        {
            Handle(context);

            return Task.FromResult<object>(null);
        }

        public void Handle(ExceptionHandlerContext context)
        {
            // Create your own custom result here...
            // In dev, you might want to null out the result
            // to display the YSOD.
            // context.Result = null;
            context.Result = new InternalServerErrorResult(context.Request);
        }
    }
}