using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.ExceptionHandling;

namespace Nightcorify.Helpers
{
    public static class HttpConfigurationExtensions
    {
        public static void UseErrorHandler( this HttpConfiguration http, IExceptionHandler errorHandler )
        {
            http.Services.Replace( typeof( IExceptionHandler ), errorHandler );
        }
    }
}
