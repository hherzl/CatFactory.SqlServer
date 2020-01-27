using System.Collections.Generic;
using System.Diagnostics;
using CatFactory.ObjectRelationalMapping;

namespace CatFactory.SqlServer.ObjectRelationalMapping
{
    /// <summary>
    /// Represents an entity result (Definition for entity)
    /// </summary>
    /// <typeparam name="TModel">Anonymous type</typeparam>
    public class EntityResult<TModel> where TModel : class
    {
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private List<TModel> m_data;

        /// <summary>
        /// Initializes a new instance of <see cref="EntityHelper"/> class
        /// </summary>
        public EntityResult()
        {
        }

        /// <summary>
        /// Gets or sets the table associated with current entity result
        /// </summary>
        public Table Table { get; set; }

        /// <summary>
        /// Gets or sets the database for current entity result
        /// </summary>
        public Database Database { get; set; }

        /// <summary>
        /// Gets or sets the model associated with current entity result
        /// </summary>
        public TModel Model { get; set; }

        /// <summary>
        /// Gets or sets the data related for current entity result
        /// </summary>
        public List<TModel> Data
        {
            get => m_data ?? (m_data = new List<TModel>());
            set => m_data = value;
        }
    }
}
