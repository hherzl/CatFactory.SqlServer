using System.Collections.Generic;
using System.Linq;
using CatFactory.SqlServer.DatabaseObjectModel;

namespace CatFactory.SqlServer.Features
{
#pragma warning disable CS1591
    public static class IEnumerableExtensions
    {
        public static ExtendedProperty GetByName(this IEnumerable<ExtendedProperty> sequence, string name)
            => sequence.FirstOrDefault(item => item.Name == name);
    }
}
