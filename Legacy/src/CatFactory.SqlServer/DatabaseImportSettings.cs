using System.Collections.Generic;
using System.Diagnostics;

namespace CatFactory.SqlServer
{
    public class DatabaseImportSettings
    {
        public DatabaseImportSettings()
        {
            ImportCommandText = @"
				select
					schemas.name as [schema_name],
					objects.name as [object_name],
					[type_desc] as [object_type]
				from
					sys.objects objects
					inner join [sys].[schemas] schemas on [objects].[schema_id] = [schemas].[schema_id]
				where
					[type] in ('FN', 'IF', 'TF', 'U', 'V', 'T', 'P')
				order by
					[object_type],
					[schema_name],
					[object_name]
			";
        }

        public string ImportCommandText { get; set; }

        public bool ImportTables { get; set; } = true;

        public bool ImportViews { get; set; } = true;

        public bool ImportStoredProcedures { get; set; }

        public bool ImportScalarFunctions { get; set; }

        public bool ImportTableFunctions { get; set; }

        public bool ImportMSDescription { get; set; }

        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private List<string> m_exclusions;

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
    }
}
