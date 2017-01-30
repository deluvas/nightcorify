using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Owin;

namespace Nightcorify.WebApi.Middleware
{
    /// <summary>
    /// Replaces the request's remote IP address and host with the one provided
    /// by the reverse proxy via the X-Real-IP and X-Real-Host headers (when available)
    /// </summary>
    public class ProxyPassMiddleware : OwinMiddleware
    {
        public ProxyPassMiddleware( OwinMiddleware next )
            : base( next )
        {
        }

        public override async Task Invoke( IOwinContext context )
        {
            var request = context.Request; 
            var ipAddress = request.Headers.Get( "X-Real-IP" );
            var host = request.Headers.Get( "X-Real-Host" ); // it's made up if you're curious

            if ( !string.IsNullOrEmpty( ipAddress ) ) {
                request.RemoteIpAddress = ipAddress;
            }
            if ( !string.IsNullOrEmpty( host ) ) {
                request.Host = new HostString( host );
            }

            await Next.Invoke( context );
        }
    }
}
