namespace CatFactory.SqlServer.DatabaseObjectModel
{
    /// <summary>
    /// Represents the result of 'fn_listextendedproperty' table function
    /// </summary>
    public class FnListExtendedPropertyResult
    {
        /// <summary>
        /// Gets or sets the object type
        /// </summary>
        public string ObjType { get; set; }

        /// <summary>
        /// Gets or sets the object name
        /// </summary>
        public string ObjName { get; set; }

        /// <summary>
        /// Gets or sets the name of extended property
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets the value of extended property
        /// </summary>
        public string Value { get; set; }
    }
}
