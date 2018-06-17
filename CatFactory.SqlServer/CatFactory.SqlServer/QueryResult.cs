using System.Collections.Generic;
using System.Diagnostics;

namespace CatFactory.SqlServer
{
    [DebuggerDisplay("Items={Items.Count}")]
    internal class QueryResult
    {
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private List<IDictionary<string, object>> m_items;

        public List<IDictionary<string, object>> Items
        {
            get
            {
                return m_items ?? (m_items = new List<IDictionary<string, object>>());
            }
            set
            {
                m_items = value;
            }
        }
    }
}
