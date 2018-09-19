using System.Collections.Generic;
using CatFactory.Mapping;

namespace CatFactory.SqlServer
{
    /// <summary>
    /// 
    /// </summary>
    public interface IExtendedPropertyRepository
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        IEnumerable<ExtendedProperty> GetExtendedProperties(ExtendedProperty model);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="model"></param>
        void AddExtendedProperty(ExtendedProperty model);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="model"></param>
        void UpdateExtendedProperty(ExtendedProperty model);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="model"></param>
        void DropExtendedProperty(ExtendedProperty model);
    }
}
