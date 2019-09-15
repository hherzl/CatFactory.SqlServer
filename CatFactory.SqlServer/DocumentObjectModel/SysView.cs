using System;

namespace CatFactory.SqlServer.DocumentObjectModel
{
    /// <summary>
    /// Represents the model for sys.views view
    /// </summary>
    public class SysView
    {
        /// <summary>
        /// Initializes a new instance of <see cref="SysView"/> class
        /// </summary>
        public SysView()
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
        /// Indicates if is Ms shipped
        /// </summary>
        public bool? IsMsShipped { get; set; }

        /// <summary>
        /// Indicates if is published
        /// </summary>
        public bool? IsPublished { get; set; }

        /// <summary>
        /// Indicates if is schema published
        /// </summary>
        public bool? IsSchemaPublished { get; set; }

        /// <summary>
        /// Indicates if is replicated
        /// </summary>
        public bool? IsReplicated { get; set; }

        /// <summary>
        /// Indicates if has replication filter
        /// </summary>
        public bool? HasReplicationFilter { get; set; }

        /// <summary>
        /// Indicates if has opaque metadata
        /// </summary>
        public bool? HasOpaqueMetadata { get; set; }

        /// <summary>
        /// Indicates if has unchecked assembly data
        /// </summary>
        public bool? HasUncheckedAssemblyData { get; set; }

        /// <summary>
        /// Indicates if has with check option
        /// </summary>
        public bool? WithCheckOption { get; set; }

        /// <summary>
        /// Indicates if is date correlation view
        /// </summary>
        public bool? IsDateCorrelationView { get; set; }

        /// <summary>
        /// Indicates if is tracked by Cdc
        /// </summary>
        public bool? IsTrackedByCdc { get; set; }
    }
}
