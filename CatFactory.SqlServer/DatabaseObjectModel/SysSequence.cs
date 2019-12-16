using System;

namespace CatFactory.SqlServer.DatabaseObjectModel
{
    /// <summary>
    /// Represents the model for sys.sequences view
    /// </summary>
    public class SysSequence
    {
        /// <summary>
        /// Initializes a new instance of <see cref="SysSequence"/> class
        /// </summary>
        public SysSequence()
        {
        }

        /// <summary>
        /// Gets or sets the name
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets the object ID
        /// </summary>
        public int? ObjectId { get; set; }

        /// <summary>
        /// Gets or sets the principal ID
        /// </summary>
        public int? PrincipalId { get; set; }

        /// <summary>
        /// Gets or sets the schema ID
        /// </summary>
        public int? SchemaId { get; set; }

        /// <summary>
        /// Gets or sets the parent object ID
        /// </summary>
        public int? ParentObjectId { get; set; }

        /// <summary>
        /// Gets or sets the type
        /// </summary>
        public string Type { get; set; }

        /// <summary>
        /// Gets or sets the type description
        /// </summary>
        public string TypeDesc { get; set; }

        /// <summary>
        /// Gets or sets the create date
        /// </summary>
        public DateTime? CreateDate { get; set; }

        /// <summary>
        /// Gets or sets the modify date
        /// </summary>
        public DateTime? ModifyDate { get; set; }

        /// <summary>
        /// Indicates if current sequence is MS shipped
        /// </summary>
        public bool? IsMsShipped { get; set; }

        /// <summary>
        /// Indicates if current sequence is published
        /// </summary>
        public bool? IsPublished { get; set; }

        /// <summary>
        /// Indicates if current sequence is schema published
        /// </summary>
        public bool? IsSchemaPublished { get; set; }

        /// <summary>
        /// Gets or sets the start value
        /// </summary>
        public object StartValue { get; set; }

        /// <summary>
        /// Gets or sets the increment
        /// </summary>
        public object Increment { get; set; }

        /// <summary>
        /// Gets or sets the minimum value
        /// </summary>
        public object MinimumValue { get; set; }

        /// <summary>
        /// Gets or sets the maximum value
        /// </summary>
        public object MaximumValue { get; set; }

        /// <summary>
        /// Indicates if current sequence is cycling
        /// </summary>
        public bool? IsCycling { get; set; }

        /// <summary>
        /// Indicates if current sequence is cached
        /// </summary>
        public bool? IsCached { get; set; }

        /// <summary>
        /// Gets or sets the cache size
        /// </summary>
        public int? CacheSize { get; set; }

        /// <summary>
        /// Gets or sets the system type ID
        /// </summary>
        public byte? SystemTypeId { get; set; }

        /// <summary>
        /// Gets or sets the user type ID
        /// </summary>
        public int? UserTypeId { get; set; }

        /// <summary>
        /// Gets or sets the precision
        /// </summary>
        public byte? Precision { get; set; }

        /// <summary>
        /// Gets or sets the scale
        /// </summary>
        public byte? Scale { get; set; }

        /// <summary>
        /// Gets or sets the current value
        /// </summary>
        public object CurrentValue { get; set; }

        /// <summary>
        /// Indicates if current sequence is exhausted
        /// </summary>
        public bool? IsExhausted { get; set; }

        /// <summary>
        /// Gets or sets the last used value
        /// </summary>
        public object LastUsedValue { get; set; }
    }
}
