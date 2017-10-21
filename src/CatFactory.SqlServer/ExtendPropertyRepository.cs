using System.Collections.Generic;
using System.Data.SqlClient;
using System.Text;
using CatFactory.Collections;

namespace CatFactory.SqlServer
{
    public class ExtendPropertyRepository : IExtendPropertyRepository
    {
        public IEnumerable<ExtendProperty> GetExtendProperties(SqlConnection connection, string name, string level0type, string level0name, string level1type, string level1name, string level2type, string level2name)
        {
            using (var command = connection.CreateCommand())
            {
                var commandText = new StringBuilder();

                var parameters = new List<string>()
                {
                    string.IsNullOrEmpty(name) ? "default" : name, level0type, level0name, level1type, level1name, level2type, level2name
                };

                commandText.AppendFormat("select objtype, objname, name, value from fn_listextendedproperty ({0})", string.Join(",", parameters));

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

        public void AddExtendedProperty(SqlConnection connection, string name, string value, string level0type, string level0name, string level1type, string level1name, string level2type, string level2name)
        {
            using (var command = connection.CreateCommand())
            {
                var commandText = new StringBuilder();

                var parameters = new List<string>()
                {
                    string.Format("@name = N'{0}'", name),
                    string.Format("@value = N'{0}'", value)
                };

                parameters.Add(!string.IsNullOrEmpty(level0type), string.Format("@level0type = N'{0}'", level0type));
                parameters.Add(!string.IsNullOrEmpty(level0name), string.Format("@level0name = N'{0}'", level0name));
                parameters.Add(!string.IsNullOrEmpty(level1type), string.Format("@level1type = N'{0}'", level1type));
                parameters.Add(!string.IsNullOrEmpty(level1name), string.Format("@level1name = N'{0}'", level1name));
                parameters.Add(!string.IsNullOrEmpty(level2type), string.Format("@level2type = N'{0}'", level2type));
                parameters.Add(!string.IsNullOrEmpty(level2name), string.Format("@level2name = N'{0}'", level2name));

                commandText.AppendFormat("exec sys.sp_addextendedproperty {0} ", string.Join(", ", parameters));

                command.Connection = connection;
                command.CommandText = commandText.ToString();

                command.ExecuteNonQuery();
            }
        }

        public void UpdateExtendedProperty(SqlConnection connection, string name, string value, string level0type, string level0name, string level1type, string level1name, string level2type, string level2name)
        {
            using (var command = connection.CreateCommand())
            {
                var commandText = new StringBuilder();

                var parameters = new List<string>()
                {
                    string.Format("@name = N'{0}'", name),
                    string.Format("@value = N'{0}'", value)
                };

                parameters.Add(!string.IsNullOrEmpty(level0type), string.Format("@level0type = N'{0}'", level0type));
                parameters.Add(!string.IsNullOrEmpty(level0name), string.Format("@level0name = N'{0}'", level0name));
                parameters.Add(!string.IsNullOrEmpty(level1type), string.Format("@level1type = N'{0}'", level1type));
                parameters.Add(!string.IsNullOrEmpty(level1name), string.Format("@level1name = N'{0}'", level1name));
                parameters.Add(!string.IsNullOrEmpty(level2type), string.Format("@level2type = N'{0}'", level2type));
                parameters.Add(!string.IsNullOrEmpty(level2name), string.Format("@level2name = N'{0}'", level2name));

                commandText.AppendFormat("exec sys.sp_updateextendedproperty {0} ", string.Join(", ", parameters));

                command.Connection = connection;
                command.CommandText = commandText.ToString();

                command.ExecuteNonQuery();
            }
        }

        public void DropExtendedProperty(SqlConnection connection, string name, string level0type, string level0name, string level1type, string level1name, string level2type, string level2name)
        {
            using (var command = connection.CreateCommand())
            {
                var commandText = new StringBuilder();

                var parameters = new List<string>()
                {
                    string.Format("@name = N'{0}'", name)
                };

                parameters.Add(!string.IsNullOrEmpty(level0type), string.Format("@level0type = N'{0}'", level0type));
                parameters.Add(!string.IsNullOrEmpty(level0name), string.Format("@level0name = N'{0}'", level0name));
                parameters.Add(!string.IsNullOrEmpty(level1type), string.Format("@level1type = N'{0}'", level1type));
                parameters.Add(!string.IsNullOrEmpty(level1name), string.Format("@level1name = N'{0}'", level1name));
                parameters.Add(!string.IsNullOrEmpty(level2type), string.Format("@level2type = N'{0}'", level2type));
                parameters.Add(!string.IsNullOrEmpty(level2name), string.Format("@level2name = N'{0}'", level2name));

                commandText.AppendFormat("exec sys.sp_dropextendedproperty {0} ", string.Join(", ", parameters));

                command.Connection = connection;
                command.CommandText = commandText.ToString();

                command.ExecuteNonQuery();
            }
        }
    }
}
