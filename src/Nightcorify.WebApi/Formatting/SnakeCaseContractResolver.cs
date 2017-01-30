using Newtonsoft.Json.Serialization;
using Nightcorify.Helpers;

namespace Nightcorify.Formatting
{
    public class SnakeCaseContractResolver : DefaultContractResolver
    {
        protected override string ResolvePropertyName( string propertyName )
        {
            return propertyName.ToSnakeCase();
        }
    }
}
