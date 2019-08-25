namespace CatFactory.SqlServer.DocumentObjectModel
{
    /// <summary>
    /// Represents the result of [sys].[dm_exec_describe_first_result_set_for_object] stored procedure
    /// </summary>
    public class DmExecDescribeFirstResultSetForObject
    {
        /// <summary>
        /// Initializes a new instance of <see cref="DmExecDescribeFirstResultSetForObject"/> class
        /// </summary>
        public DmExecDescribeFirstResultSetForObject()
        {
        }

        /// <summary>
        /// Gets or sets the column ordinal
        /// </summary>
        public int ColumnOrdinal { get; set; }

        /// <summary>
        /// Gets or sets the name
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Indicates if is nullable
        /// </summary>
        public bool IsNullable { get; set; }

        /// <summary>
        /// Gets or sets the system type name
        /// </summary>
        public string SystemTypeName { get; set; }
    }
}
