using System.Collections.Generic;
using CatFactory.ObjectRelationalMapping;
using CatFactory.ObjectRelationalMapping.Programmability;

namespace CatFactory.SqlServer
{
    /// <summary>
    /// Represents the abstract model for SQL Server databases
    /// </summary>
    public interface ISqlServerDatabase : IDatabase
    {
        /// <summary>
        /// Gets or sets the scalar functions
        /// </summary>
        List<ScalarFunction> ScalarFunctions { get; set; }

        /// <summary>
        /// Gets or sets the table functions
        /// </summary>
        List<TableFunction> TableFunctions { get; set; }

        /// <summary>
        /// Gets or sets the store procedures
        /// </summary>
        List<StoredProcedure> StoredProcedures { get; set; }

        /// <summary>
        /// Gets or sets the sequences
        /// </summary>
        List<Sequence> Sequences { get; set; }
    }
}
