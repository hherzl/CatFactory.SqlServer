namespace CatFactory.SqlServer.Mocking
{
#pragma warning disable CS1591
    public static class EntityMocker
    {
        public static EntityMocker<TModel> Create<TModel>(TModel model) where TModel : class
            => new();
    }
}
