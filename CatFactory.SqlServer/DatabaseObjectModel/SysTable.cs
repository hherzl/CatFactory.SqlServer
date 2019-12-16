using System;

namespace CatFactory.SqlServer.DatabaseObjectModel
{
    /// <summary>
    /// Represents the model for sys.tables view
    /// </summary>
    public class SysTable
    {
        /// <summary>
        /// Initializes a new instance of <see cref="SysTable"/> class
        /// </summary>
        public SysTable()
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
        /// Gets or sets the lob data space ID
        /// </summary>
        public int? LobDataSpaceId { get; set; }

        /// <summary>
        /// Gets or sets the file stream data space Id
        /// </summary>
        public int? FilestreamDataSpaceId { get; set; }

        /// <summary>
        /// Gets or sets the max column Id used
        /// </summary>
        public int? MaxColumnIdUsed { get; set; }

        /// <summary>
        /// Indicates if is lock on bluk load
        /// </summary>
        public bool? LockOnBulkLoad { get; set; }

        /// <summary>
        /// Indicates if uses ANSI nulls
        /// </summary>
        public bool? UsesAnsiNulls { get; set; }

        /// <summary>
        /// Indicates if is replicated
        /// </summary>
        public bool? IsReplicated { get; set; }

        /// <summary>
        /// Indicates if has replication filter
        /// </summary>
        public bool? HasReplicationFilter { get; set; }

        /// <summary>
        /// Indicates if is merge published
        /// </summary>
        public bool? IsMergePublished { get; set; }

        /// <summary>
        /// Idicates if is sync transaction subscribed
        /// </summary>
        public bool? IsSyncTranSubscribed { get; set; }

        /// <summary>
        /// Indicates if has unchecked assembly data
        /// </summary>
        public bool? HasUncheckedAssemblyData { get; set; }

        /// <summary>
        /// Gets or sets the text in row limit
        /// </summary>
        public int? TextInRowLimit { get; set; }

        /// <summary>
        /// Indicates if large value types out of row
        /// </summary>
        public bool? LargeValueTypesOutOfRow { get; set; }

        /// <summary>
        /// Gets or sets if is tracked by Cdc
        /// </summary>
        public bool? IsTrackedByCdc { get; set; }

        /// <summary>
        /// Gets or sets the lock escalation
        /// </summary>
        public byte? LockEscalation { get; set; }

        /// <summary>
        /// Gets or sets the lock escalation description
        /// </summary>
        public string LockEscalationDesc { get; set; }

        /// <summary>
        /// Indicates if is file table
        /// </summary>
        public bool? IsFiletable { get; set; }

        /// <summary>
        /// Indicates if is memory optimized
        /// </summary>
        public bool? IsMemoryOptimized { get; set; }

        /// <summary>
        /// Gets or sets the durability
        /// </summary>
        public byte? Durability { get; set; }

        /// <summary>
        /// Gets or sets the durability description
        /// </summary>
        public string DurabilityDesc { get; set; }

        /// <summary>
        /// Gets or sets the temporal type
        /// </summary>
        public byte? TemporalType { get; set; }

        /// <summary>
        /// Gets or sets the temporal type description
        /// </summary>
        public string TemporalTypeDesc { get; set; }

        /// <summary>
        /// Gets or sets the history table ID
        /// </summary>
        public int? HistoryTableId { get; set; }

        /// <summary>
        /// Indicates if is remote data archive enabled
        /// </summary>
        public bool? IsRemoteDataArchiveEnabled { get; set; }

        /// <summary>
        /// Indicates if is external
        /// </summary>
        public bool? IsExternal { get; set; }

        /// <summary>
        /// Gets or sets the history retention period
        /// </summary>
        public int? HistoryRetentionPeriod { get; set; }

        /// <summary>
        /// Gets or sets the history retention period unit
        /// </summary>
        public int? HistoryRetentionPeriodUnit { get; set; }

        /// <summary>
        /// Gets or sets the history retention period unit description
        /// </summary>
        public string HistoryRetentionPeriodUnitDesc { get; set; }

        /// <summary>
        /// Indicates if is node
        /// </summary>
        public bool? IsNode { get; set; }

        /// <summary>
        /// Indicates if is edge
        /// </summary>
        public bool? IsEdge { get; set; }
    }
}
