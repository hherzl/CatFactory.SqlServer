using System;
using System.Collections.Generic;

namespace CatFactory.SqlServer.Mocking
{
    /// <summary>
    /// 
    /// </summary>
    public class EntitySetting
    {
        /// <summary>
        /// 
        /// </summary>
        public EntitySetting()
        {
        }

        /// <summary>
        /// 
        /// </summary>
        public object When { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public IEnumerable<object> Values { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public Func<DateTime> DateTimeFunc;

        /// <summary>
        /// 
        /// </summary>
        public Func<int> Int32Func;
    }
}
