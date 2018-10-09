using System;
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
        /// Initializes a new instance of <see cref="DatabaseImportSettings"/> class
        /// </summary>
        public DatabaseImportSettings()
        {
            ImportCommandText = @"
				select
					[schemas].[name] as [schema_name],
					[objects].[name] as [object_name],
					[type_desc] as [object_type]
				from
					[sys].[objects] objects
					inner join [sys].[schemas] schemas on [objects].[schema_id] = [schemas].[schema_id]
				where
					[type] in ('FN', 'IF', 'TF', 'U', 'V', 'T', 'P')
				order by
					[object_type],
					[schema_name],
					[object_name]
			";
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
        public bool ImportTables { get; set; } = true;

        /// <summary>
        /// Gets or sets a flag to import views
        /// </summary>
        public bool ImportViews { get; set; } = true;

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
        /// Gets or sets a flag to import MS_Description extended property
        /// </summary>
        [Obsolete("Add extended properties in order to retrieve description")]
        public bool ImportMSDescription { get; set; } = true;

        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private List<string> m_exclusions;

        /// <summary>
        /// Gets or sets exclusions (database objects) for database import settings
        /// </summary>
        public List<string> Exclusions
        {
            get
            {
                return m_exclusions ?? (m_exclusions = new List<string>());
            }
            set
            {
                m_exclusions = value;
            }
        }

        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private List<string> m_exclusionTypes;

        /// <summary>
        /// Gets or sets exclusion types (database types) for database import settings
        /// </summary>
        public List<string> ExclusionTypes
        {
            get
            {
                return m_exclusionTypes ?? (m_exclusionTypes = new List<string>());
            }
            set
            {
                m_exclusionTypes = value;
            }
        }

        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private List<string> m_extendedProperties;

        /// <summary>
        /// Gets or sets extended properties for database import settings
        /// </summary>
        public List<string> ExtendedProperties
        {
            get
            {
                return m_extendedProperties ?? (m_extendedProperties = new List<string>());
            }
            set
            {
                m_extendedProperties = value;
            }
        }
    }
}
