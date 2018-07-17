using CatFactory.Mapping;

namespace CatFactory.SqlServer
{
    public class EntityResult<TModel>
    {
        public Table Table { get; set; }

        public TModel Model { get; set; }
    }
}
