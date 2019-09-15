using System;

namespace CatFactory.SqlServer.DocumentObjectModel
{
    /// <summary>
    /// 
    /// </summary>
    public class SysTable
    {
        /// <summary>
        /// 
        /// </summary>
        public SysTable()
        {
        }

        /// <summary>
        /// 
        /// </summary>
        public string  Name { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public int? ObjectId { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public int? PrincipalId { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public int? SchemaId { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public int? ParentObjectId { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string Type { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string TypeDesc { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public DateTime? CreateDate { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public DateTime? ModifyDate { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public bool? IsMsShipped { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public bool? IsPublished { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public bool? IsSchemaPublished { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public int? LobDataSpaceId { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public int? FilestreamDataSpaceId { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public int? MaxColumnIdUsed { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public bool? LockOnBulkLoad { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public bool? UsesAnsiNulls { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public bool? IsReplicated { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public bool? HasReplicationFilter { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public bool? IsMergePublished { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public bool? IsSyncTranSubscribed { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public bool? HasUncheckedAssemblyData { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public int? TextInRowLimit { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public bool? LargeValueTypesOutOfRow { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public bool? IsTrackedByCdc { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public byte? LockEscalation { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string LockEscalationDesc { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public bool? IsFiletable { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public bool? IsMemoryOptimized { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public byte? Durability { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string DurabilityDesc { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public byte? TemporalType { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string TemporalTypeDesc { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public int? HistoryTableId { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public bool? IsRemoteDataArchiveEnabled { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public bool? IsExternal { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public int? HistoryRetentionPeriod { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public int? HistoryRetentionPeriodUnit { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string HistoryRetentionPeriodUnitDesc { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public bool? IsNode { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public bool? IsEdge { get; set; }
    }
}
