using System;
using System.Collections.Generic;

namespace CatFactory.SqlServer.Mocking
{
#pragma warning disable CS1591
    public class EntitySetting
    {
        public EntitySetting()
        {
        }

        public object When { get; set; }

        public string Name { get; set; }

        public IEnumerable<object> Values { get; set; }

        public Func<DateTime> DateTimeFunc { get; set; }

        public Func<Decimal> DecimalFunc { get; set; }

        public Func<short> Int16Func { get; set; }

        public Func<int> Int32Func { get; set; }

        public Func<long> Int64Func { get; set; }

        public Func<string> StringFunc { get; set; }
    }
}
