using System.Collections.Generic;
using System.Threading.Tasks;
using CatFactory.SqlServer.DatabaseObjectModel;

namespace CatFactory.SqlServer.Features
{
    /// <summary>
    /// Contains operations to read and write extended properties
    /// </summary>
    public interface IExtendedPropertyRepository
    {
        /// <summary>
        /// Gets extended properties
        /// </summary>
        /// <param name="extendedProperty">Search parameter</param>
        /// <returns>A task representing the asynchronous operation</returns>
        Task<List<ExtendedProperty>> GetAsync(ExtendedProperty extendedProperty);

        /// <summary>
        /// Adds an extended property
        /// </summary>
        /// <param name="extendedProperty">Instance of <see cref="ExtendedProperty"/> class to add</param>
        /// <returns>A task representing the asynchronous operation</returns>
        Task AddAsync(ExtendedProperty extendedProperty);

        /// <summary>
        /// Updates an extended property
        /// </summary>
        /// <param name="extendedProperty">Instance of <see cref="ExtendedProperty"/> class to update</param>
        /// <returns>A task representing the asynchronous operation</returns>
        Task UpdateAsync(ExtendedProperty extendedProperty);

        /// <summary>
        /// Drops an extended property
        /// </summary>
        /// <param name="extendedProperty">Instance of <see cref="ExtendedProperty"/> class to drop</param>
        /// <returns>A task representing the asynchronous operation</returns>
        Task DropAsync(ExtendedProperty extendedProperty);
    }
}
