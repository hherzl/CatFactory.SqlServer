using System;
using System.Diagnostics;

namespace CatFactory.SqlServer
{
    [DebuggerDisplay("ObjType={ObjType}, ObjName={ObjName}, Name={Name}")]
    public class ExtendProperty
    {
        public ExtendProperty()
        {
        }

        public String ObjType { get; set; }

        public String ObjName { get; set; }

        public String Name { get; set; }

        public Object Value { get; set; }
    }
}
