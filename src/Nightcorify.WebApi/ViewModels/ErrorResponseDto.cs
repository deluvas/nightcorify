using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nightcorify.ViewModels
{
    public class ErrorResponseDto<TError>
    {
        public TError Error { get; set; }
    }
}
