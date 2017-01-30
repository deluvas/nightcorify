using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nightcorify.ViewModels
{
    public class IdentifierDto<TId>
    {
        public TId Id { get; set; }

        public IdentifierDto( TId id )
        {
            Id = id;
        }
    }

    public class IntegerIdentifierDto : IdentifierDto<int>
    {
        public IntegerIdentifierDto( int id )
            : base( id )
        {
        }
    }
}
