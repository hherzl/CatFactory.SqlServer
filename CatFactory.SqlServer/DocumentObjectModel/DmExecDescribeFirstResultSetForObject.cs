namespace CatFactory.SqlServer.DocumentObjectModel
{
    /// <summary>
    /// 
    /// </summary>
    public class DescribeFirstResultSetForObjectResult
    {
        /// <summary>
        /// 
        /// </summary>
        public DescribeFirstResultSetForObjectResult()
        {
        }

        /// <summary>
        /// 
        /// </summary>
        public int ColumnOrdinal { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public bool IsNullable { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string SystemTypeName { get; set; }
    }
}
