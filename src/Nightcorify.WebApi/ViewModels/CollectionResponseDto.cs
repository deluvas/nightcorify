using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Nightcorify.WebApi.ViewModels
{
    public class CollectionResponseDto<T>
    {
        public IEnumerable<T> Data { get; set; }
    }
}