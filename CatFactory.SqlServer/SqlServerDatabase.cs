using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using CatFactory.ObjectRelationalMapping;
using CatFactory.SqlServer.DatabaseObjectModel;
using CatFactory.SqlServer.ObjectRelationalMapping;

namespace CatFactory.SqlServer
{
    /// <summary>
    /// Represents the model for SQL Server databases
    /// </summary>
    public class SqlServerDatabase : Database, ISqlServerDatabase
    {
        /// <summary>
        /// Gets a database with default values (Default schema, support transactions, database type maps and naming convention)
        /// </summary>
        /// <param name="name">Name</param>
        /// <param name="defaultSchema">Default schema</param>
        /// <param name="databaseTypeMaps">Database type maps</param>
        /// <param name="namingConvention">Database naming convention</param>
        /// <returns>An instance of <see cref="Database"/> class</returns>
        public static SqlServerDatabase CreateWithDefaults(string name, string defaultSchema = "dbo", List<DatabaseTypeMap> databaseTypeMaps = null, IDatabaseNamingConvention namingConvention = null)
            => new()
            {
                Name = name,
                DefaultSchema = defaultSchema,
                SupportTransactions = true,
                DatabaseTypeMaps = databaseTypeMaps ?? SqlServerDatabaseTypeMaps.DatabaseTypeMaps.ToList(),
                NamingConvention = namingConvention ?? new SqlServerDatabaseNamingConvention()
            };

        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private List<ExtendedProperty> m_extendedProperties;

        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private List<DefaultTypeMap> m_defaultTypeMaps;

        /// <summary>
        /// Initializes a new instance of <see cref="SqlServerDatabase"/> class
        /// </summary>
        public SqlServerDatabase()
            : base()
        {
        }

        /// <summary>
        /// Gets or sets the extended properties
        /// </summary>
        public List<ExtendedProperty> ExtendedProperties
        {
            get => m_extendedProperties ??= new List<ExtendedProperty>();
            set => m_extendedProperties = value;
        }

        /// <summary>
        /// Gets or sets the default type maps
        /// </summary>
        public List<DefaultTypeMap> DefaultTypeMaps
        {
            get => m_defaultTypeMaps ??= new List<DefaultTypeMap>();
            set => m_defaultTypeMaps = value;
        }
    }
}
