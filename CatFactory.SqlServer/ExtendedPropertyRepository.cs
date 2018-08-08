using System.Collections.Generic;
using System.Data.Common;
using CatFactory.Collections;

namespace CatFactory.SqlServer
{
    public class ExtendedPropertyRepository : IExtendedPropertyRepository
    {
        public IEnumerable<ExtendedProperty> GetExtendedProperties(DbConnection connection, ExtendedPropertyModel model)
        {
            using (var command = connection.CreateCommand())
            {
                var parameters = new List<string>
                {
                    string.IsNullOrEmpty(model.Name) ? "default" : string.Format("'{0}'", model.Name),
                    string.IsNullOrEmpty(model.Level0type) ? "default" : string.Format("'{0}'", model.Level0type),
                    string.IsNullOrEmpty(model.Level0name) ? "default" : string.Format("'{0}'", model.Level0name),
                    string.IsNullOrEmpty(model.Level1type) ? "default" : string.Format("'{0}'", model.Level1type),
                    string.IsNullOrEmpty(model.Level1name) ? "default" : string.Format("'{0}'", model.Level1name),
                    string.IsNullOrEmpty(model.Level2type) || model.Level2type == "default" ? "default" : string.Format("'{0}'", model.Level2type),
                    string.IsNullOrEmpty(model.Level2name) || model.Level2name == "default" ? "default" : string.Format("'{0}'", model.Level2name)
                };

                var commandText = string.Format("select objtype, objname, name, value from fn_listextendedproperty ({0})", string.Join(",", parameters));

                command.Connection = connection;
                command.CommandText = commandText;

                using (var dataReader = command.ExecuteReader())
                {
                    while (dataReader.Read())
                    {
                        yield return new ExtendedProperty
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

        public void AddExtendedProperty(DbConnection connection, ExtendedPropertyModel model)
        {
            using (var command = connection.CreateCommand())
            {
                var parameters = new List<string>
                {
                    string.Format("@name = N'{0}'",  model.Name),
                    string.Format("@value = N'{0}'", model.Value)
                };

                parameters.Add(!string.IsNullOrEmpty(model.Level0type), string.Format("@level0type = N'{0}'", model.Level0type));
                parameters.Add(!string.IsNullOrEmpty(model.Level0name), string.Format("@level0name = N'{0}'", model.Level0name));
                parameters.Add(!string.IsNullOrEmpty(model.Level1type), string.Format("@level1type = N'{0}'", model.Level1type));
                parameters.Add(!string.IsNullOrEmpty(model.Level1name), string.Format("@level1name = N'{0}'", model.Level1name));
                parameters.Add(!string.IsNullOrEmpty(model.Level2type), string.Format("@level2type = N'{0}'", model.Level2type));
                parameters.Add(!string.IsNullOrEmpty(model.Level2name), string.Format("@level2name = N'{0}'", model.Level2name));

                var commandText = string.Format("exec sys.sp_addextendedproperty {0}", string.Join(", ", parameters));

                command.Connection = connection;
                command.CommandText = commandText;

                command.ExecuteNonQuery();
            }
        }

        public void UpdateExtendedProperty(DbConnection connection, ExtendedPropertyModel model)
        {
            using (var command = connection.CreateCommand())
            {
                var parameters = new List<string>
                {
                    string.Format("@name = N'{0}'",  model.Name),
                    string.Format("@value = N'{0}'", model.Value)
                };

                parameters.Add(!string.IsNullOrEmpty(model.Level0type), string.Format("@level0type = N'{0}'", model.Level0type));
                parameters.Add(!string.IsNullOrEmpty(model.Level0name), string.Format("@level0name = N'{0}'", model.Level0name));
                parameters.Add(!string.IsNullOrEmpty(model.Level1type), string.Format("@level1type = N'{0}'", model.Level1type));
                parameters.Add(!string.IsNullOrEmpty(model.Level1name), string.Format("@level1name = N'{0}'", model.Level1name));
                parameters.Add(!string.IsNullOrEmpty(model.Level2type), string.Format("@level2type = N'{0}'", model.Level2type));
                parameters.Add(!string.IsNullOrEmpty(model.Level2name), string.Format("@level2name = N'{0}'", model.Level2name));

                var commandText = string.Format("exec sys.sp_updateextendedproperty {0} ", string.Join(", ", parameters));

                command.Connection = connection;
                command.CommandText = commandText;

                command.ExecuteNonQuery();
            }
        }

        public void DropExtendedProperty(DbConnection connection, ExtendedPropertyModel model)
        {
            using (var command = connection.CreateCommand())
            {
                var parameters = new List<string>
                {
                    string.Format("@name = N'{0}'", model.Name)
                };

                parameters.Add(!string.IsNullOrEmpty(model.Level0type), string.Format("@level0type = N'{0}'", model.Level0type));
                parameters.Add(!string.IsNullOrEmpty(model.Level0name), string.Format("@level0name = N'{0}'", model.Level0name));
                parameters.Add(!string.IsNullOrEmpty(model.Level1type), string.Format("@level1type = N'{0}'", model.Level1type));
                parameters.Add(!string.IsNullOrEmpty(model.Level1name), string.Format("@level1name = N'{0}'", model.Level1name));
                parameters.Add(!string.IsNullOrEmpty(model.Level2type), string.Format("@level2type = N'{0}'", model.Level2type));
                parameters.Add(!string.IsNullOrEmpty(model.Level2name), string.Format("@level2name = N'{0}'", model.Level2name));

                var commandText = string.Format("exec sys.sp_dropextendedproperty {0} ", string.Join(", ", parameters));

                command.Connection = connection;
                command.CommandText = commandText;

                command.ExecuteNonQuery();
            }
        }
    }
}
