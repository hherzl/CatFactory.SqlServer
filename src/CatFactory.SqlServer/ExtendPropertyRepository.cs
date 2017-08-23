using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Text;

namespace CatFactory.SqlServer
{
    public class ExtendPropertyRepository
    {
        public IEnumerable<ExtendProperty> GetExtendProperties(SqlConnection connection, String name, String level0type, String level0name, String level1type, String level1name, String level2type, String level2name)
        {
            using (var command = connection.CreateCommand())
            {
                var commandText = new StringBuilder();

                var parameters = new List<String>()
                {
                    String.IsNullOrEmpty(name) ? "default" : name, level0type, level0name, level1type, level1name, level2type, level2name
                };

                commandText.AppendFormat("select objtype, objname, name, value from fn_listextendedproperty ({0})", String.Join(",", parameters));

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

        public void AddExtendedProperty(SqlConnection connection, String name, String value, String level0type, String level0name, String level1type, String level1name, String level2type, String level2name)
        {
            using (var command = connection.CreateCommand())
            {
                var commandText = new StringBuilder();

                var parameters = new List<String>()
                {
                    String.Format("@name = N'{0}'", name),
                    String.Format("@value = N'{0}'", value)
                };

                if (!String.IsNullOrEmpty(level0type))
                {
                    parameters.Add(String.Format("@level0type = N'{0}'", level0type));
                }

                if (!String.IsNullOrEmpty(level0name))
                {
                    parameters.Add(String.Format("@level0name = N'{0}'", level0name));
                }

                if (!String.IsNullOrEmpty(level1type))
                {
                    parameters.Add(String.Format("@level1type = N'{0}'", level1type));
                }

                if (!String.IsNullOrEmpty(level1name))
                {
                    parameters.Add(String.Format("@level1name = N'{0}'", level1name));
                }

                if (!String.IsNullOrEmpty(level2type))
                {
                    parameters.Add(String.Format("@level2type = N'{0}'", level2type));
                }

                if (!String.IsNullOrEmpty(level2name))
                {
                    parameters.Add(String.Format("@level2name = N'{0}'", level2name));
                }

                commandText.AppendFormat("exec sys.sp_addextendedproperty {0} ", String.Join(", ", parameters));

                command.Connection = connection;
                command.CommandText = commandText.ToString();

                command.ExecuteNonQuery();
            }
        }

        public void UpdateExtendedProperty(SqlConnection connection, String name, String value, String level0type, String level0name, String level1type, String level1name, String level2type, String level2name)
        {
            using (var command = connection.CreateCommand())
            {
                var commandText = new StringBuilder();

                var parameters = new List<String>()
                {
                    String.Format("@name = N'{0}'", name),
                    String.Format("@value = N'{0}'", value)
                };

                if (!String.IsNullOrEmpty(level0type))
                {
                    parameters.Add(String.Format("@level0type = N'{0}'", level0type));
                }

                if (!String.IsNullOrEmpty(level0name))
                {
                    parameters.Add(String.Format("@level0name = N'{0}'", level0name));
                }

                if (!String.IsNullOrEmpty(level1type))
                {
                    parameters.Add(String.Format("@level1type = N'{0}'", level1type));
                }

                if (!String.IsNullOrEmpty(level1name))
                {
                    parameters.Add(String.Format("@level1name = N'{0}'", level1name));
                }

                if (!String.IsNullOrEmpty(level2type))
                {
                    parameters.Add(String.Format("@level2type = N'{0}'", level2type));
                }

                if (!String.IsNullOrEmpty(level2name))
                {
                    parameters.Add(String.Format("@level2name = N'{0}'", level2name));
                }

                commandText.AppendFormat("exec sys.sp_updateextendedproperty {0} ", String.Join(", ", parameters));

                command.Connection = connection;
                command.CommandText = commandText.ToString();

                command.ExecuteNonQuery();
            }
        }

        public void DropExtendedProperty(SqlConnection connection, String name, String level0type, String level0name, String level1type, String level1name, String level2type, String level2name)
        {
            using (var command = connection.CreateCommand())
            {
                var commandText = new StringBuilder();

                var parameters = new List<String>()
                {
                    String.Format("@name = N'{0}'", name)
                };

                if (!String.IsNullOrEmpty(level0type))
                {
                    parameters.Add(String.Format("@level0type = N'{0}'", level0type));
                }

                if (!String.IsNullOrEmpty(level0name))
                {
                    parameters.Add(String.Format("@level0name = N'{0}'", level0name));
                }

                if (!String.IsNullOrEmpty(level1type))
                {
                    parameters.Add(String.Format("@level1type = N'{0}'", level1type));
                }

                if (!String.IsNullOrEmpty(level1name))
                {
                    parameters.Add(String.Format("@level1name = N'{0}'", level1name));
                }

                if (!String.IsNullOrEmpty(level2type))
                {
                    parameters.Add(String.Format("@level2type = N'{0}'", level2type));
                }

                if (!String.IsNullOrEmpty(level2name))
                {
                    parameters.Add(String.Format("@level2name = N'{0}'", level2name));
                }

                commandText.AppendFormat("exec sys.sp_dropextendedproperty {0} ", String.Join(", ", parameters));

                command.Connection = connection;
                command.CommandText = commandText.ToString();

                command.ExecuteNonQuery();
            }
        }
    }
}
