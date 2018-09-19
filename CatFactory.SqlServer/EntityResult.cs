using CatFactory.Mapping;

namespace CatFactory.SqlServer
{
    /// <summary>
    /// 
    /// </summary>
    /// <typeparam name="TModel"></typeparam>
    public class EntityResult<TModel>
    {
        /// <summary>
        /// 
        /// </summary>
        public Table Table { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public TModel Model { get; set; }
    }
}
