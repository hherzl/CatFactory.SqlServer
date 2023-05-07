using System;
using System.Data;
using System.Data.SqlClient;

namespace CatFactory.SqlServer.DatabaseObjectModel.Queries
{
#pragma warning disable CS1591
    public static class SqlConnectionExtensions
    {
        public static SqlParameter GetParameter(this SqlConnection connection, string name, SqlDbType sqlDbType, string value)
        {
            var parameter = new SqlParameter(name, sqlDbType);

            if (string.IsNullOrEmpty(value))
                parameter.Value = DBNull.Value;
            else
                parameter.Value = value;

            return parameter;
        }
    }
}
