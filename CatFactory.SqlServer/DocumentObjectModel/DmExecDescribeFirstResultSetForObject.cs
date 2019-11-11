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
        /// Indicates if is hidden
        /// </summary>
        public bool IsHidden { get; set; }

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
        /// Gets or sets the system type ID
        /// </summary>
        public int SystemTypeId { get; set; }

        /// <summary>
        /// Gets or sets the system type name
        /// </summary>
        public string SystemTypeName { get; set; }

        /// <summary>
        /// Gets or sets the max length
        /// </summary>
        public short MaxLength { get; set; }

        /// <summary>
        /// Gets or sets the precision
        /// </summary>
        public byte Precision { get; set; }

        /// <summary>
        /// Gets or sets the scale
        /// </summary>
        public byte Scale { get; set; }

        /// <summary>
        /// Gets or sets the collation name
        /// </summary>
        public string CollationName { get; set; }

        /// <summary>
        /// Gets or sets the user type ID
        /// </summary>
        public int UserTypeId { get; set; }

        /// <summary>
        /// Gets or sets the user type database
        /// </summary>
        public string UserTypeDatabase { get; set; }

        /// <summary>
        /// Gets or sets the user type schema
        /// </summary>
        public string UserTypeSchema { get; set; }

        /// <summary>
        /// Gets or sets the user type name
        /// </summary>
        public string UserTypeName { get; set; }

        /// <summary>
        /// Gets or sets the assembly qualified type name
        /// </summary>
        public string AssemblyQualifiedTypeName { get; set; }

        /// <summary>
        /// Gets or sets the XML collection ID
        /// </summary>
        public int XmlCollectionId { get; set; }

        /// <summary>
        /// Gets or sets the XML collection database
        /// </summary>
        public string XmlCollectionDatabase { get; set; }

        /// <summary>
        /// Gets or sets the XML collection schema
        /// </summary>
        public string XmlCollectionSchema { get; set; }

        /// <summary>
        /// Gets or sets the XML collection name
        /// </summary>
        public string XmlCollectionName { get; set; }

        /// <summary>
        /// Indicates if is XML document
        /// </summary>
        public bool IsXmlDocument { get; set; }

        /// <summary>
        /// Indicates if is case sensitive
        /// </summary>
        public bool IsCaseSensitive { get; set; }

        /// <summary>
        /// Indicates if is fixed length CLR type
        /// </summary>
        public bool IsFixedLengthClrType { get; set; }

        /// <summary>
        /// Gets or sets the source server
        /// </summary>
        public string SourceServer { get; set; }

        /// <summary>
        /// Gets or sets the source database
        /// </summary>
        public string SourceDatabase { get; set; }

        /// <summary>
        /// Gets or sets the source schema
        /// </summary>
        public string SourceSchema { get; set; }

        /// <summary>
        /// Gets or sets the source table
        /// </summary>
        public string SourceTable { get; set; }

        /// <summary>
        /// Gets or sets the source column
        /// </summary>
        public string SourceColumn { get; set; }

        /// <summary>
        /// Indicates if is identity column
        /// </summary>
        public bool IsIdentityColumn { get; set; }

        /// <summary>
        /// Indicates if is part of unique key
        /// </summary>
        public bool IsPartOfUniqueKey { get; set; }

        /// <summary>
        /// Indicates if is updateable
        /// </summary>
        public bool IsUpdateable { get; set; }

        /// <summary>
        /// Indicates if is computed column
        /// </summary>
        public bool IsComputedColumn { get; set; }

        /// <summary>
        /// Indicates if is sparse column set
        /// </summary>
        public bool IsSparseColumnSet { get; set; }

        /// <summary>
        /// Gets or sets the ordinal in order by list
        /// </summary>
        public short OrdinalInOrderByList { get; set; }

        /// <summary>
        /// Indicates if order by is descending
        /// </summary>
        public bool OrderByIsDescending { get; set; }

        /// <summary>
        /// Gets or sets the order by list length
        /// </summary>
        public short OrderByListLength { get; set; }

        /// <summary>
        /// Gets or sets the error number
        /// </summary>
        public int ErrorNumber { get; set; }

        /// <summary>
        /// Gets or sets the error severity
        /// </summary>
        public int ErrorSeverity { get; set; }

        /// <summary>
        /// Gets or sets the error state
        /// </summary>
        public int ErrorState { get; set; }

        /// <summary>
        /// Gets or sets the error message
        /// </summary>
        public string ErrorMessage { get; set; }

        /// <summary>
        /// Gets or sets the error type
        /// </summary>
        public int ErrorType { get; set; }

        /// <summary>
        /// Gets or sets the error type description
        /// </summary>
        public string ErrorTypeDesc { get; set; }
    }
}
