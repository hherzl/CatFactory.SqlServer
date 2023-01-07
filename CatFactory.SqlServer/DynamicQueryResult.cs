using System.Collections.Generic;
using System.Diagnostics;

namespace CatFactory.SqlServer
{
    /// <summary>
    /// Represents a dynamic query result
    /// </summary>
    [DebuggerDisplay("Items={Items.Count}")]
    internal class DynamicQueryResult
    {
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private List<IDictionary<string, object>> m_items;

        /// <summary>
        /// Initializes a new instance of <see cref="DynamicQueryResult"/> class
        /// </summary>
        public DynamicQueryResult()
        {
        }

        /// <summary>
        /// Gets or sets the items for dynamic result
        /// </summary>
        public List<IDictionary<string, object>> Items
        {
            get => m_items ??= new List<IDictionary<string, object>>();
            set => m_items = value;
        }
    }
}
