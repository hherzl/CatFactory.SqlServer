namespace CatFactory.SqlServer.Mocking
{
    /// <summary>
    /// 
    /// </summary>
    public static class EntityMocker
    {
        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="TModel"></typeparam>
        /// <param name="model"></param>
        /// <returns></returns>
        public static EntityMocker<TModel> Create<TModel>(TModel model) where TModel : class
            => new EntityMocker<TModel>();
    }
}
