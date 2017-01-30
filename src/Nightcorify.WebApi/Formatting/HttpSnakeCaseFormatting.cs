using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Formatting;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http;
using Newtonsoft.Json;

namespace Nightcorify.Formatting
{
    public static class HttpSnakeCaseFormatting
    {
        public static void UseJsonSnakeCaseFormatter( this HttpConfiguration http )
        {
            http.Formatters.Clear();
            http.Formatters.Add( JsonFormatterFactory() );
        }

        static JsonMediaTypeFormatter JsonFormatterFactory()
        {
            return new JsonMediaTypeFormatter() {
                UseDataContractJsonSerializer = false,
                SerializerSettings = new JsonSerializerSettings {
                    Formatting = Newtonsoft.Json.Formatting.Indented,
                    ContractResolver = new SnakeCaseContractResolver() // :snake:
                }
            };
        }
    }
}
