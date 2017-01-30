using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace Nightcorify.Helpers
{
    public static class HttpContentHelper
    {
        public static StreamContent FromFile( FileStream filestream, string filename, string contentType )
        {
            var content = new StreamContent( filestream );
            content.Headers.ContentType = new MediaTypeHeaderValue( contentType );
            content.Headers.ContentDisposition = new ContentDispositionHeaderValue( "attachment" );
            content.Headers.ContentDisposition.FileName = filename;
            return content;
        }
    }
}
