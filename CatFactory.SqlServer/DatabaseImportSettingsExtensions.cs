using System.Data.SqlClient;

namespace CatFactory.SqlServer
{
#pragma warning disable CS1591
    public static class DatabaseImportSettingsExtensions
    {
        public static SqlConnection GetConnection(this DatabaseImportSettings databaseImportSettings)
            => new(databaseImportSettings.ConnectionString);
    }
}
