using System.Collections.Generic;
using System.Data;
using CatFactory.Collections;
using CatFactory.Mapping;

namespace CatFactory.SqlServer
{
    /// <summary>
    /// 
    /// </summary>
    public class ExtendedPropertyRepository : IExtendedPropertyRepository
    {
        private IDbConnection connection;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="cnn"></param>
        public ExtendedPropertyRepository(IDbConnection cnn)
        {
            connection = cnn;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public IEnumerable<ExtendedProperty> GetExtendedProperties(ExtendedProperty model)
        {
            using (var command = connection.CreateCommand())
            {
                var parameters = new List<string>
                {
                    string.IsNullOrEmpty(model.Name) ? "default" : string.Format("'{0}'", model.Name),
                    string.IsNullOrEmpty(model.Level0Type) ? "default" : string.Format("'{0}'", model.Level0Type),
                    string.IsNullOrEmpty(model.Level0Name) ? "default" : string.Format("'{0}'", model.Level0Name),
                    string.IsNullOrEmpty(model.Level1Type) ? "default" : string.Format("'{0}'", model.Level1Type),
                    string.IsNullOrEmpty(model.Level1Name) ? "default" : string.Format("'{0}'", model.Level1Name),
                    string.IsNullOrEmpty(model.Level2Type) || model.Level2Type == "default" ? "default" : string.Format("'{0}'", model.Level2Type),
                    string.IsNullOrEmpty(model.Level2Name) || model.Level2Name == "default" ? "default" : string.Format("'{0}'", model.Level2Name)
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
                            Name = dataReader.GetString(2),
                            Value = dataReader.GetString(3)
                        };
                    }
                }
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="model"></param>
        public void AddExtendedProperty(ExtendedProperty model)
        {
            using (var command = connection.CreateCommand())
            {
                var parameters = new List<string>
                {
                    string.Format("@name = N'{0}'",  model.Name),
                    string.Format("@value = N'{0}'", model.Value)
                };

                parameters.Add(!string.IsNullOrEmpty(model.Level0Type), string.Format("@level0type = N'{0}'", model.Level0Type));
                parameters.Add(!string.IsNullOrEmpty(model.Level0Name), string.Format("@level0name = N'{0}'", model.Level0Name));
                parameters.Add(!string.IsNullOrEmpty(model.Level1Type), string.Format("@level1type = N'{0}'", model.Level1Type));
                parameters.Add(!string.IsNullOrEmpty(model.Level1Name), string.Format("@level1name = N'{0}'", model.Level1Name));
                parameters.Add(!string.IsNullOrEmpty(model.Level2Type), string.Format("@level2type = N'{0}'", model.Level2Type));
                parameters.Add(!string.IsNullOrEmpty(model.Level2Name), string.Format("@level2name = N'{0}'", model.Level2Name));

                var commandText = string.Format("exec sys.sp_addextendedproperty {0}", string.Join(", ", parameters));

                command.Connection = connection;
                command.CommandText = commandText;

                command.ExecuteNonQuery();
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="model"></param>
        public void UpdateExtendedProperty(ExtendedProperty model)
        {
            using (var command = connection.CreateCommand())
            {
                var parameters = new List<string>
                {
                    string.Format("@name = N'{0}'",  model.Name),
                    string.Format("@value = N'{0}'", model.Value)
                };

                parameters.Add(!string.IsNullOrEmpty(model.Level0Type), string.Format("@level0type = N'{0}'", model.Level0Type));
                parameters.Add(!string.IsNullOrEmpty(model.Level0Name), string.Format("@level0name = N'{0}'", model.Level0Name));
                parameters.Add(!string.IsNullOrEmpty(model.Level1Type), string.Format("@level1type = N'{0}'", model.Level1Type));
                parameters.Add(!string.IsNullOrEmpty(model.Level1Name), string.Format("@level1name = N'{0}'", model.Level1Name));
                parameters.Add(!string.IsNullOrEmpty(model.Level2Type), string.Format("@level2type = N'{0}'", model.Level2Type));
                parameters.Add(!string.IsNullOrEmpty(model.Level2Name), string.Format("@level2name = N'{0}'", model.Level2Name));

                var commandText = string.Format("exec sys.sp_updateextendedproperty {0} ", string.Join(", ", parameters));

                command.Connection = connection;
                command.CommandText = commandText;

                command.ExecuteNonQuery();
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="model"></param>
        public void DropExtendedProperty(ExtendedProperty model)
        {
            using (var command = connection.CreateCommand())
            {
                var parameters = new List<string>
                {
                    string.Format("@name = N'{0}'", model.Name)
                };

                parameters.Add(!string.IsNullOrEmpty(model.Level0Type), string.Format("@level0type = N'{0}'", model.Level0Type));
                parameters.Add(!string.IsNullOrEmpty(model.Level0Name), string.Format("@level0name = N'{0}'", model.Level0Name));
                parameters.Add(!string.IsNullOrEmpty(model.Level1Type), string.Format("@level1type = N'{0}'", model.Level1Type));
                parameters.Add(!string.IsNullOrEmpty(model.Level1Name), string.Format("@level1name = N'{0}'", model.Level1Name));
                parameters.Add(!string.IsNullOrEmpty(model.Level2Type), string.Format("@level2type = N'{0}'", model.Level2Type));
                parameters.Add(!string.IsNullOrEmpty(model.Level2Name), string.Format("@level2name = N'{0}'", model.Level2Name));

                var commandText = string.Format("exec sys.sp_dropextendedproperty {0} ", string.Join(", ", parameters));

                command.Connection = connection;
                command.CommandText = commandText;

                command.ExecuteNonQuery();
            }
        }
    }
}
