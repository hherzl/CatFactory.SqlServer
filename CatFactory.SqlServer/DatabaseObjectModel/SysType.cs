namespace CatFactory.SqlServer.DatabaseObjectModel
{
    /// <summary>
    /// Represents the model for sys.types view
    /// </summary>
    public class SysType
    {
        /// <summary>
        /// Initializes a new instance of <see cref="SysType"/> class
        /// </summary>
        public SysType()
        {
        }

        /// <summary>
        /// Gets or sets the name
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets the system type ID
        /// </summary>
        public byte? SystemTypeId { get; set; }

        /// <summary>
        /// Gets or sets the user type ID
        /// </summary>
        public int? UserTypeId { get; set; }

        /// <summary>
        /// Gets or sets the schema ID
        /// </summary>
        public int? SchemaId { get; set; }

        /// <summary>
        /// Gets or sets the principal ID
        /// </summary>
        public int? PrincipalId { get; set; }

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
        /// Indicates if current type is nullable
        /// </summary>
        public bool? IsNullable { get; set; }

        /// <summary>
        /// Indicates if current type is user defined
        /// </summary>
        public bool? IsUserDefined { get; set; }

        /// <summary>
        /// Indicates if current type is assembly type
        /// </summary>
        public bool? IsAssemblyType { get; set; }

        /// <summary>
        /// Gets or sets the default object ID
        /// </summary>
        public int? DefaultObjectId { get; set; }

        /// <summary>
        /// Gets or sets the rule object ID
        /// </summary>
        public int? RuleObjectId { get; set; }

        /// <summary>
        /// Indicates if current type is table type
        /// </summary>
        public bool? IsTableType { get; set; }
    }
}
