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
        public Func<DateTime> DateTimeFunc { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public Func<Decimal> DecimalFunc { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public Func<short> Int16Func { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public Func<int> Int32Func { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public Func<long> Int64Func { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public Func<string> StringFunc { get; set; }
    }
}
