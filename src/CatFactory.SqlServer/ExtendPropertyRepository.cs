using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Text;

namespace CatFactory.SqlServer
{
    public class ExtendPropertyRepository
    {
        public ExtendPropertyRepository()
        {
        }

        public IEnumerable<ExtendProperty> GetExtendProperties(SqlConnection connection, String name, String level0type, String level0name, String level1type, String level1name, String level2type, String level2name)
        {
            using (var command = connection.CreateCommand())
            {
                var commandText = new StringBuilder();

                commandText.Append("select");
                commandText.Append(" objtype, objname, name, value ");
                commandText.Append("from");
                commandText.Append(" fn_listextendedproperty");
                commandText.Append("(");

                var parameters = new List<String>()
                {
                    String.IsNullOrEmpty(name) ? "default" : name,
                    level0type,
                    level0name,
                    level1type,
                    level1name,
                    level2type,
                    level2name
                };

                commandText.AppendFormat("{0}", String.Join(",", parameters));

                commandText.Append(")");

                command.Connection = connection;
                command.CommandText = commandText.ToString();

                using (var dataReader = command.ExecuteReader())
                {
                    while (dataReader.Read())
                    {
                        yield return new ExtendProperty
                        {
                            ObjType = dataReader.GetString(0),
                            ObjName = dataReader.GetString(1),
                            Name = dataReader.GetString(2),
                            Value = dataReader[3]
                        };
                    }
                }
            }
        }
    }
}
