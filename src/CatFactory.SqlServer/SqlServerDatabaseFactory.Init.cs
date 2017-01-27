namespace CatFactory.SqlServer
{
    public partial class SqlServerDatabaseFactory
    {
		public void Init()
        {
            ImportMSDescription = true;
            ImportCommandText = @"
				select
					schemas.name as schema_name,
					objects.name as object_name,
					type_desc as object_type
				from
					sys.objects objects
					inner join sys.schemas schemas on objects.schema_id = schemas.schema_id
				where
					[type] in ('FN', 'IF', 'TF', 'U', 'V', 'T', 'P')
				order by
					[object_type],
					[schema_name],
					[object_name]
			";
        }
    }
}
