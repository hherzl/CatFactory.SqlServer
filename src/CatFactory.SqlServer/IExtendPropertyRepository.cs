using System.Collections.Generic;
using System.Data.SqlClient;

namespace CatFactory.SqlServer
{
    public interface IExtendPropertyRepository
    {
        IEnumerable<ExtendProperty> GetExtendProperties(SqlConnection connection, string name, string level0type, string level0name, string level1type, string level1name, string level2type, string level2name);

        void AddExtendedProperty(SqlConnection connection, string name, string value, string level0type, string level0name, string level1type, string level1name, string level2type, string level2name);

        void UpdateExtendedProperty(SqlConnection connection, string name, string value, string level0type, string level0name, string level1type, string level1name, string level2type, string level2name);

        void DropExtendedProperty(SqlConnection connection, string name, string level0type, string level0name, string level1type, string level1name, string level2type, string level2name);
    }
}
