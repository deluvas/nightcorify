using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace Nightcorify.Helpers
{
    public static class StringHelper
    {
        // TODO: Optimize me
        public static string ToSnakeCase( this string str )
        {
            var snakeCased = Regex.Match( str, @"^_+" );
            return snakeCased + Regex.Replace( str, @"([A-Z])", "_$1" )
                .ToLower().TrimStart( '_' );
        }

        public static string ToCapsCase( this string str )
        {
            var snakeCased = Regex.Match( str, @"^_+" );
            return snakeCased + Regex.Replace( str, @"([A-Z])", "_$1" )
                .ToUpper().TrimStart( '_' );
        }
    }
}
