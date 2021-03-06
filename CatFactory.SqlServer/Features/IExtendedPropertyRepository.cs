﻿using System.Collections.Generic;
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
        /// <returns>A sequence of <see cref="ExtendedProperty"/> class</returns>
        IEnumerable<ExtendedProperty> GetExtendedProperties(ExtendedProperty extendedProperty);

        /// <summary>
        /// Adds an extended property
        /// </summary>
        /// <param name="extendedProperty">Instance of <see cref="ExtendedProperty"/> class to add</param>
        void AddExtendedProperty(ExtendedProperty extendedProperty);

        /// <summary>
        /// Updates an extended property
        /// </summary>
        /// <param name="extendedProperty">Instance of <see cref="ExtendedProperty"/> class to update</param>
        void UpdateExtendedProperty(ExtendedProperty extendedProperty);

        /// <summary>
        /// Drops an extended property
        /// </summary>
        /// <param name="extendedProperty">Instance of <see cref="ExtendedProperty"/> class to drop</param>
        void DropExtendedProperty(ExtendedProperty extendedProperty);
    }
}
