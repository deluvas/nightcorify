using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Web.Cors;
using System.Web.Http;
using System.Web.Http.Routing;
using Microsoft.Owin.Cors;

namespace Nightcorify.Config
{
    public class DefaultCorsOptions
    {
        public static CorsOptions Instance
        {
            get
            {
                var policy = new CorsPolicy {
                    AllowAnyHeader = false,
                    AllowAnyMethod = false,
                    AllowAnyOrigin = true,
                    SupportsCredentials = true
                };
                //
                // Allowed methods
                //
                policy.Headers.Add( "Content-Type" );
                policy.Headers.Add( "Accept" );
                policy.Headers.Add( "Authorization" );
                //
                // Allowed methods
                //
                policy.Methods.Add( "GET" );
                policy.Methods.Add( "POST" );
                policy.Methods.Add( "PUT" );
                policy.Methods.Add( "DELETE" );
                policy.Methods.Add( "OPTIONS" );

                return new CorsOptions {
                    PolicyProvider = new CorsPolicyProvider {
                        PolicyResolver = ctx => Task.FromResult( policy )
                    }
                };
            }
        }
    }
}
