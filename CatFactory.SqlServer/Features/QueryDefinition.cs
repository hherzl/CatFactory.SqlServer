using System.Collections.Generic;
using System.Diagnostics;

namespace CatFactory.SqlServer.Features
{
#pragma warning disable CS1591
    public class QueryDefinition<TModel>
    {
        public static QueryDefinition<TModel> Create()
            => new();

        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private List<string> m_columns;

        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private List<QueryCondition> m_conditions;

        public QueryDefinition()
        {
        }

        public List<string> Columns
        {
            get => m_columns ??= new List<string>();
            set => m_columns = value;
        }

        public List<QueryCondition> Conditions
        {
            get => m_conditions ??= new List<QueryCondition>();
            set => m_conditions = value;
        }
    }
}
