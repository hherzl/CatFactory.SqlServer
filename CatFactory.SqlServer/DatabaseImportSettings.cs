using System.Collections.Generic;
using System.Diagnostics;

namespace CatFactory.SqlServer
{
    /// <summary>
    /// Represents settings for database import feature
    /// </summary>
    public class DatabaseImportSettings
    {
        /// <summary>
        /// Creates a new instance of <see cref="DatabaseImportSettings"/>
        /// </summary>
        /// <param name="name">Name</param>
        /// <param name="connectionString">Connection string</param>
        /// <param name="extendedProperties">Extended properties</param>
        /// <returns>The database import settings</returns>
        public static DatabaseImportSettings Create(string name, string connectionString, params string[] extendedProperties)
            => new()
            {
                Name = name,
                ConnectionString = connectionString,
                ExtendedProperties = new List<string>(extendedProperties)
            };

        /// <summary>
        /// Creates a new instance of <see cref="DatabaseImportSettings"/>
        /// </summary>
        /// <param name="name">Name</param>
        /// <param name="connectionString">Connection string</param>
        /// <param name="importTables">Import tables flag</param>
        /// <param name="importViews">Import views flag</param>
        /// <param name="extendedProperties">Extended properties</param>
        /// <returns>The database import settings</returns>
        public static DatabaseImportSettings Create(string name, string connectionString, bool importTables, bool importViews, params string[] extendedProperties)
            => new()
            {
                Name = name,
                ConnectionString = connectionString,
                ImportTables = importTables,
                ImportViews = importViews,
                ExtendedProperties = new List<string>(extendedProperties)
            };

        /// <summary>
        /// Creates a new instance of <see cref="DatabaseImportSettings"/>
        /// </summary>
        /// <param name="name">Name</param>
        /// <param name="connectionString">Connection string</param>
        /// <param name="importTables">Import tables flag</param>
        /// <param name="importViews">Import views flag</param>
        /// <param name="importScalarFunctions">Import scalar functions flag</param>
        /// <param name="importTableFunctions">Import table functions flag</param>
        /// <param name="importStoredProcedures">Import stored procedures flag</param>
        /// <param name="importSequences">Import sequences flag</param>
        /// <param name="extendedProperties">Extended properties</param>
        /// <returns>The database import settings</returns>
        public static DatabaseImportSettings Create(string name = "", string connectionString = "", bool importTables = true, bool importViews = true, bool importScalarFunctions = false, bool importTableFunctions = false, bool importStoredProcedures = false, bool importSequences = false, string[] extendedProperties = null)
            => new()
            {
                Name = name,
                ConnectionString = connectionString,
                ImportTables = importTables,
                ImportViews = importViews,
                ImportScalarFunctions = importScalarFunctions,
                ImportTableFunctions = importTableFunctions,
                ImportStoredProcedures = importStoredProcedures,
                ImportSequences = importSequences,
                ExtendedProperties = extendedProperties == null ? new List<string>() : new List<string>(extendedProperties)
            };

        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private List<string> m_exclusions;

        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private List<string> m_exclusionTypes;

        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private List<string> m_extendedProperties;

        /// <summary>
        /// Initializes a new instance of <see cref="DatabaseImportSettings"/> class
        /// </summary>
        public DatabaseImportSettings()
        {
            ImportCommandText = @"
				SELECT
					[schemas].[name] AS [schema_name],
					[objects].[name] AS [object_name],
					[type_desc] AS [object_type]
				FROM
					[sys].[objects] objects
					INNER JOIN [sys].[schemas] schemas ON [objects].[schema_id] = [schemas].[schema_id]
				WHERE
					[type] IN ('FN', 'IF', 'TF', 'U', 'V', 'T', 'P', 'SO')
				ORDER BY
					[object_type],
					[schema_name],
					[object_name]
			";

            ImportTables = true;
            ImportViews = true;
        }

        /// <summary>
        /// Gets or sets the name for database import settings
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets the connection string for database import settings
        /// </summary>
        public string ConnectionString { get; set; }

        /// <summary>
        /// Gets or sets the import command text for database import settings
        /// </summary>
        public string ImportCommandText { get; set; }

        /// <summary>
        /// Gets or sets a flag to import tables
        /// </summary>
        public bool ImportTables { get; set; }

        /// <summary>
        /// Gets or sets a flag to import views
        /// </summary>
        public bool ImportViews { get; set; }

        /// <summary>
        /// Gets or sets a flag to import scalar functions
        /// </summary>
        public bool ImportScalarFunctions { get; set; }

        /// <summary>
        /// Gets or sets a flag to import table functions
        /// </summary>
        public bool ImportTableFunctions { get; set; }

        /// <summary>
        /// Gets or sets a flag to import stored procedures
        /// </summary>
        public bool ImportStoredProcedures { get; set; }

        /// <summary>
        /// Gets or sets a flag to import sequences
        /// </summary>
        public bool ImportSequences { get; set; }

        /// <summary>
        /// Gets or sets exclusions (database objects) for database import settings
        /// </summary>
        public List<string> Exclusions
        {
            get => m_exclusions ??= new List<string>();
            set => m_exclusions = value;
        }

        /// <summary>
        /// Gets or sets exclusion types (database types) for database import settings
        /// </summary>
        public List<string> ExclusionTypes
        {
            get => m_exclusionTypes ??= new List<string>();
            set => m_exclusionTypes = value;
        }

        /// <summary>
        /// Gets or sets extended properties for database import settings
        /// </summary>
        public List<string> ExtendedProperties
        {
            get => m_extendedProperties ??= new List<string>();
            set => m_extendedProperties = value;
        }
    }
}
