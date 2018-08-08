using System.Collections.Generic;
using System.Data.Common;

namespace CatFactory.SqlServer
{
    public interface IExtendedPropertyRepository
    {
        IEnumerable<ExtendedProperty> GetExtendedProperties(DbConnection connection, ExtendedPropertyModel model);

        void AddExtendedProperty(DbConnection connection, ExtendedPropertyModel model);

        void UpdateExtendedProperty(DbConnection connection, ExtendedPropertyModel model);

        void DropExtendedProperty(DbConnection connection, ExtendedPropertyModel model);
    }
}
