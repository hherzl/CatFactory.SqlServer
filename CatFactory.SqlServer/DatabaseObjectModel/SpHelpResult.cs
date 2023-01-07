using System.Collections.Generic;
using System.Diagnostics;

namespace CatFactory.SqlServer.DatabaseObjectModel
{
    /// <summary>
    /// Represents the result for execution of sp_help stored procedure in SQLServer
    /// </summary>
    public class SpHelpResult
    {
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private List<IDictionary<string, object>> m_items;

        /// <summary>
        /// Initializes a new instance of <see cref="SpHelpResult"/> class
        /// </summary>
        public SpHelpResult()
        {
        }

        /// <summary>
        /// Gets or sets the items for sp_help stored procedure result
        /// </summary>
        public List<IDictionary<string, object>> Items
        {
            get => m_items ??= new List<IDictionary<string, object>>();
            set => m_items = value;
        }
    }
}
