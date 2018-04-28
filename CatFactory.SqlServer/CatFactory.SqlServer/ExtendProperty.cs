using System.Diagnostics;

namespace CatFactory.SqlServer
{
    [DebuggerDisplay("ObjType={ObjType}, ObjName={ObjName}, Name={Name}")]
    public class ExtendProperty
    {
        public ExtendProperty()
        {
        }

        public string ObjType { get; set; }

        public string ObjName { get; set; }

        public string Name { get; set; }

        public object Value { get; set; }
    }
}
