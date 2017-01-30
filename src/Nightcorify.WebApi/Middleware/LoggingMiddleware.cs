using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Owin;
using NLog;
using NLog.Fluent;
using Owin;

namespace Nightcorify.Middleware
{
    public class LoggingMiddleware : OwinMiddleware
    {
        static readonly ILogger log = LogManager.GetCurrentClassLogger();

        Func<IOwinContext,IDictionary> _propertyLogMapper;

        public LoggingMiddleware( OwinMiddleware next, Func<IOwinContext,IDictionary> propertyLogMapper )
            : base( next )
        {
            _propertyLogMapper = propertyLogMapper;
        }

        public LoggingMiddleware( OwinMiddleware next )
            : this( next, null )
        {
        }

        public override async Task Invoke( IOwinContext context )
        {
            var stopwatch = new Stopwatch();
            stopwatch.Start();

            await Next.Invoke( context ); // await for response

            stopwatch.Stop();

            var builder = log
               .Debug()
               .Message( context.Request.Uri.PathAndQuery.Substring( 1 ) ) // trim / from beginning
               .Property( "time", $"{stopwatch.ElapsedMilliseconds}ms" );

            // Write custom properties to the log when available
            var props = _propertyLogMapper?.Invoke( context );
            if ( props != null ) {
                builder.Properties( props );
            }
               
            builder.Write();
        }
    }

    public static class LoggingMiddlewareExtensions
    {
        public static void UseLogging( this IAppBuilder app, Func<IOwinContext, IDictionary> propertyLogMapper )
        {
            app.Use<LoggingMiddleware>( propertyLogMapper );
        }

        public static void UseLogging( this IAppBuilder app )
        {
            UseLogging( app, null );
        }
    }
}
