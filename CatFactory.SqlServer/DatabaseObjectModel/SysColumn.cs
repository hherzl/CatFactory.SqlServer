namespace CatFactory.SqlServer.DatabaseObjectModel
{
    /// <summary>
    /// Represents the model for sys.columns view
    /// </summary>
    public class SysColumn
    {
        /// <summary>
        /// Initializes a new instance of <see cref="SysColumn"/> class
        /// </summary>
        public SysColumn()
        {
        }

        /// <summary>
        /// Gets or sets the object ID
        /// </summary>
        public int? ObjectId { get; set; }

        /// <summary>
        /// Gets or sets the name
        /// </summary>
        public object Name { get; set; }

        /// <summary>
        /// Gets or sets the column ID
        /// </summary>
        public int? ColumnId { get; set; }

        /// <summary>
        /// Gets or sets the system type ID
        /// </summary>
        public byte? SystemTypeId { get; set; }

        /// <summary>
        /// Gets or sets the user type ID
        /// </summary>
        public int? UserTypeId { get; set; }

        /// <summary>
        /// Gets or sets the max length
        /// </summary>
        public short? MaxLength { get; set; }

        /// <summary>
        /// Gets or sets the precision
        /// </summary>
        public byte? Precision { get; set; }

        /// <summary>
        /// Gets or sets the scale
        /// </summary>
        public byte? Scale { get; set; }

        /// <summary>
        /// Gets or sets the collation name
        /// </summary>
        public string CollationName { get; set; }

        /// <summary>
        /// Indicates if is nullable
        /// </summary>
        public bool? IsNullable { get; set; }

        /// <summary>
        /// Indicates if is ANSI padded
        /// </summary>
        public bool? IsAnsiPadded { get; set; }

        /// <summary>
        /// Indicates if is row Guid col
        /// </summary>
        public bool? IsRowguidcol { get; set; }

        /// <summary>
        /// Indicates if is identity
        /// </summary>
        public bool? IsIdentity { get; set; }

        /// <summary>
        /// Indicates if is computed
        /// </summary>
        public bool? IsComputed { get; set; }

        /// <summary>
        /// Indicates if is file stream
        /// </summary>
        public bool? IsFilestream { get; set; }

        /// <summary>
        /// Indicated if is replicated
        /// </summary>
        public bool? IsReplicated { get; set; }

        /// <summary>
        /// Indicates if is non SQL subscribed
        /// </summary>
        public bool? IsNonSqlSubscribed { get; set; }

        /// <summary>
        /// Indicates if is merge published
        /// </summary>
        public bool? IsMergePublished { get; set; }

        /// <summary>
        /// Indicates if is DTS replicated
        /// </summary>
        public bool? IsDtsReplicated { get; set; }

        /// <summary>
        /// Indicates if is XML document
        /// </summary>
        public bool? IsXmlDocument { get; set; }

        /// <summary>
        /// Gets or sets the XML collection ID
        /// </summary>
        public int? XmlCollectionId { get; set; }

        /// <summary>
        /// Gets or sets the default object ID
        /// </summary>
        public int? DefaultObjectId { get; set; }

        /// <summary>
        /// Gets or sets the rule object ID
        /// </summary>
        public int? RuleObjectId { get; set; }

        /// <summary>
        /// Indicates if is sparse
        /// </summary>
        public bool? IsSparse { get; set; }

        /// <summary>
        /// Indicates if is column set
        /// </summary>
        public bool? IsColumnSet { get; set; }

        /// <summary>
        /// Gets or sets the generated always type
        /// </summary>
        public byte? GeneratedAlwaysType { get; set; }

        /// <summary>
        /// Gets or sets the generated always type description
        /// </summary>
        public string GeneratedAlwaysTypeDesc { get; set; }

        /// <summary>
        /// Gets or sets the encryption type
        /// </summary>
        public int? EncryptionType { get; set; }

        /// <summary>
        /// Gets or sets the encryption type description
        /// </summary>
        public string EncryptionTypeDesc { get; set; }

        /// <summary>
        /// Gets or sets the encryption algorithm name
        /// </summary>
        public string EncryptionAlgorithmName { get; set; }

        /// <summary>
        /// Gets or sets the column encryption key ID
        /// </summary>
        public int? ColumnEncryptionKeyId { get; set; }

        /// <summary>
        /// Gets or sets the column encryption key database name
        /// </summary>
        public string ColumnEncryptionKeyDatabaseName { get; set; }

        /// <summary>
        /// Indicates if is hidden
        /// </summary>
        public bool? IsHidden { get; set; }

        /// <summary>
        /// Indicates if is masked
        /// </summary>
        public bool? IsMasked { get; set; }

        /// <summary>
        /// Gets or sets the graph type
        /// </summary>
        public int? GraphType { get; set; }

        /// <summary>
        /// Gets or sets the graph type description
        /// </summary>
        public string GraphTypeDesc { get; set; }
    }
}
