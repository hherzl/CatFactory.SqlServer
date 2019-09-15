using System;

namespace CatFactory.SqlServer.DocumentObjectModel
{
    /// <summary>
    /// 
    /// </summary>
    public class SysView
    {
        /// <summary>
        /// 
        /// </summary>
        public SysView()
        {
        }

        /// <summary>
        /// 
        /// </summary>
        public string Name { get; set; }

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
        public bool? IsReplicated { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public bool? HasReplicationFilter { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public bool? HasOpaqueMetadata { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public bool? HasUncheckedAssemblyData { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public bool? WithCheckOption { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public bool? IsDateCorrelationView { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public bool? IsTrackedByCdc { get; set; }
    }
}
