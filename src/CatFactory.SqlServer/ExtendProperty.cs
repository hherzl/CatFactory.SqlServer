using System;

namespace CatFactory.SqlServer
{
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
