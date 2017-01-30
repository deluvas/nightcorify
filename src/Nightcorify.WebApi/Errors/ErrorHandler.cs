using System;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http.ExceptionHandling;
using NLog;
using NLog.Fluent;
using System.Net.Http;
using Nightcorify.WebApi.Errors;
using Nightcorify.ViewModels;

namespace Nightcorify.WebApi
{
    /// <summary>
    /// Modifies the response to include an error message and a corresponding code
    /// depending on the exception. 
    /// Logs the exception.
    /// </summary>
    public class ErrorHandler : ExceptionHandler
    {
        static readonly ILogger log = LogManager.GetCurrentClassLogger();

        public override void Handle( ExceptionHandlerContext context )
        {
            var httpContext = context.Request.GetOwinContext();
            var actionDescriptor = context.Request.GetActionDescriptor();

            log.Error()
                .Exception( context.Exception )
                .Message( context.Exception.Message )
                .Write();

            context.Result = new ErrorResponseMessage<ErrorResponseDto<ErrorDto>>( 
                context.Exception, 
                context.Request,
                ( ex ) => new ErrorResponseDto<ErrorDto> {
                    Error = new ErrorDto( ex )
                }
            );
        }

        public override bool ShouldHandle( ExceptionHandlerContext context )
        {
            return true;
        }
    }
}
