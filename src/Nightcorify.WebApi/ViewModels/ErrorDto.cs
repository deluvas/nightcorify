using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nightcorify.ViewModels
{
    public class ErrorDto
    {
        public string Message { get; set; }
#if LOCAL
        public string DevMessage { get; set; }
#endif

        public ErrorDto( Exception ex )
        {
            Message = "An unexpected error has occurred.";
#if LOCAL
            DevMessage = ex.Message;

            var baseex = ex.GetBaseException();
            if ( baseex != null && ex != baseex ) {
                DevMessage = baseex.Message;
            }
#endif
        }
    }
}
