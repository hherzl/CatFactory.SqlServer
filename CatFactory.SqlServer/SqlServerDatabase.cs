using CatFactory.ObjectRelationalMapping;

namespace CatFactory.SqlServer
{
    /// <summary>
    /// Represents the model for SQL Server databases
    /// </summary>
    public class SqlServerDatabase : Database, ISqlServerDatabase
    {
        /// <summary>
        /// Initializes a new instance of <see cref="SqlServerDatabase"/> class
        /// </summary>
        public SqlServerDatabase()
            : base()
        {
        }
    }
}
