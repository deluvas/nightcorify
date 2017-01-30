using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Owin;

namespace Nightcorify.Helpers
{
    public static class LogHelper
    {
        public static IDictionary ParseHttpLogEntry( IOwinContext context )
        {
            var dict = new Dictionary<string, object>();
            var request = context.Request;
            var response = context.Response;

            var useragent = request.Headers.Get( "user-agent" );
            var contentEncoding = request.Headers.Get( "content-encoding" );

            dict.Add( "method", request.Method );
            dict.Add( "code", response.StatusCode );
            if ( request.ContentType != null ) {
                dict.Add( "contentType", request.ContentType );
            }
            if ( contentEncoding != null ) {
                dict.Add( "contentEncoding", contentEncoding );
            }
            if ( useragent != null ) {
                dict.Add( "userAgent", useragent.Substring( 0, 15 ) + "[..]" );
            }
            dict.Add( "ip", request.RemoteIpAddress );

            return dict;
        }
    }
}
