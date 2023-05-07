using System;

namespace CatFactory.SqlServer.ObjectRelationalMapping
{
    /// <summary>
    /// Represents a map between CLR type and Database type
    /// </summary>
    public class DefaultTypeMap
    {
        /// <summary>
        /// Initializes a new instance of <see cref="DefaultTypeMap"/> class
        /// </summary>
        public DefaultTypeMap()
        {
        }
        /// <summary>
        /// Initializes a new instance of <see cref="DefaultTypeMap"/> class
        /// </summary>
        /// <param name="type">CLR Type</param>
        /// <param name="databaseType">Database type</param>
        public DefaultTypeMap(Type type, string databaseType)
        {
            Type = type;
            DatabaseType = databaseType;
        }

        /// <summary>
        /// Gets or sets the CLR type
        /// </summary>
        public Type Type { get; set; }

        /// <summary>
        /// Gets or sets the database type
        /// </summary>
        public string DatabaseType { get; set; }
    }
}
