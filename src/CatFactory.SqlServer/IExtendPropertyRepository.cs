using System.Collections.Generic;
using System.Data.Common;

namespace CatFactory.SqlServer
{
    public interface IExtendPropertyRepository
    {
        IEnumerable<ExtendProperty> GetExtendProperties(DbConnection connection, string name, string level0type, string level0name, string level1type, string level1name, string level2type, string level2name);

        void AddExtendedProperty(DbConnection connection, string name, string value, string level0type, string level0name, string level1type, string level1name, string level2type, string level2name);

        void UpdateExtendedProperty(DbConnection connection, string name, string value, string level0type, string level0name, string level1type, string level1name, string level2type, string level2name);

        void DropExtendedProperty(DbConnection connection, string name, string level0type, string level0name, string level1type, string level1name, string level2type, string level2name);
    }
}
