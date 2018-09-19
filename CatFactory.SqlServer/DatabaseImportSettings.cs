using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace CatFactory.SqlServer
{
    /// <summary>
    /// 
    /// </summary>
    public class DatabaseImportSettings
    {
        /// <summary>
        /// 
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
        /// 
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string ConnectionString { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string ImportCommandText { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public bool ImportTables { get; set; } = true;

        /// <summary>
        /// 
        /// </summary>
        public bool ImportViews { get; set; } = true;

        /// <summary>
        /// 
        /// </summary>
        public bool ImportStoredProcedures { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public bool ImportScalarFunctions { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public bool ImportTableFunctions { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [Obsolete("Add extended properties in order to retrieve description")]
        public bool ImportMSDescription { get; set; } = true;

        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private List<string> m_exclusions;

        /// <summary>
        /// 
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
        /// 
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
        /// 
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
