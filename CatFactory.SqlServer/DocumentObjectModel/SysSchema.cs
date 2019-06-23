namespace CatFactory.SqlServer.DocumentObjectModel
{
    /// <summary>
    /// 
    /// </summary>
    public class SysSchema
    {
        /// <summary>
        /// Initializes a new instance of <see cref="SysSchema"/> class
        /// </summary>
        public SysSchema()
        {
        }

        /// <summary>
        /// Gets or sets the name
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets the schema ID
        /// </summary>
        public int? SchemaId { get; set; }

        /// <summary>
        /// Gets or sets the principal ID
        /// </summary>
        public int? PrincipalId { get; set; }
    }
}
