namespace CatFactory.SqlServer.DocumentObjectModel
{
    /// <summary>
    /// 
    /// </summary>
    public class SysColumn
    {
        /// <summary>
        /// 
        /// </summary>
        public SysColumn()
        {
        }

        /// <summary>
        /// 
        /// </summary>
        public int? ObjectId { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public object Name { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public int? ColumnId { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public byte? SystemTypeId { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public int? UserTypeId { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public short? MaxLength { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public byte? Precision { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public byte? Scale { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public object CollationName { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public bool? IsNullable { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public bool? IsAnsiPadded { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public bool? IsRowguidcol { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public bool? IsIdentity { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public bool? IsComputed { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public bool? IsFilestream { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public bool? IsReplicated { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public bool? IsNonSqlSubscribed { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public bool? IsMergePublished { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public bool? IsDtsReplicated { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public bool? IsXmlDocument { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public int? XmlCollectionId { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public int? DefaultObjectId { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public int? RuleObjectId { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public bool? IsSparse { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public bool? IsColumnSet { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public byte? GeneratedAlwaysType { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string GeneratedAlwaysTypeDesc { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public int? EncryptionType { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string EncryptionTypeDesc { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string EncryptionAlgorithmName { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public int? ColumnEncryptionKeyId { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string ColumnEncryptionKeyDatabaseName { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public bool? IsHidden { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public bool? IsMasked { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public int? GraphType { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string GraphTypeDesc { get; set; }
    }
}
