using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using CatFactory.Mapping;

namespace CatFactory.SqlServer
{
    /// <summary>
    /// 
    /// </summary>
    public class ExtendedPropertyRepository : IExtendedPropertyRepository
    {
        private IDbConnection Connection;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="connection"></param>
        public ExtendedPropertyRepository(IDbConnection connection)
        {
            Connection = connection;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="extendedProperty"></param>
        /// <returns></returns>
        public IEnumerable<ExtendedProperty> GetExtendedProperties(ExtendedProperty extendedProperty)
        {
            using (var command = Connection.CreateCommand())
            {
                command.Connection = Connection;
                command.CommandText = "select [objtype], [objname], [name], [value] from [fn_listextendedproperty](@name, @level0type, @level0name, @level1type, @level1name, @level2type, @level2name)";

                command.Parameters.Add(new SqlParameter("@name", extendedProperty.Name));

                var level0type = new SqlParameter("@level0type", SqlDbType.VarChar);

                if (string.IsNullOrEmpty(extendedProperty.Level0Type))
                    level0type.Value = DBNull.Value;
                else
                    level0type.Value = extendedProperty.Level0Type;

                command.Parameters.Add(level0type);

                var level0name = new SqlParameter("@level0name", SqlDbType.VarChar);

                if (string.IsNullOrEmpty(extendedProperty.Level0Name))
                    level0name.Value = DBNull.Value;
                else
                    level0name.Value = extendedProperty.Level0Name;

                command.Parameters.Add(level0name);

                var level1type = new SqlParameter("@level1type", SqlDbType.VarChar);

                if (string.IsNullOrEmpty(extendedProperty.Level1Type))
                    level1type.Value = DBNull.Value;
                else
                    level1type.Value = extendedProperty.Level1Type;

                command.Parameters.Add(level1type);

                var level1name = new SqlParameter("@level1name", SqlDbType.VarChar);

                if (string.IsNullOrEmpty(extendedProperty.Level1Name))
                    level1name.Value = DBNull.Value;
                else
                    level1name.Value = extendedProperty.Level1Name;

                command.Parameters.Add(level1name);

                var level2type = new SqlParameter("@level2type", SqlDbType.VarChar);

                if (string.IsNullOrEmpty(extendedProperty.Level2Type))
                    level2type.Value = DBNull.Value;
                else
                    level2type.Value = extendedProperty.Level2Type;

                command.Parameters.Add(level2type);

                var level2name = new SqlParameter("@level2name", SqlDbType.VarChar);

                if (string.IsNullOrEmpty(extendedProperty.Level2Name))
                    level2name.Value = DBNull.Value;
                else
                    level2name.Value = extendedProperty.Level2Name;

                command.Parameters.Add(level2name);

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
        /// <param name="extendedProperty"></param>
        public void AddExtendedProperty(ExtendedProperty extendedProperty)
        {
            using (var command = Connection.CreateCommand())
            {
                command.Connection = Connection;
                command.CommandText = "exec [sys].[sp_addextendedproperty] @name, @value, @level0type, @level0name, @level1type, @level1name, @level2type, @level2name";

                command.Parameters.Add(new SqlParameter("@name", extendedProperty.Name));
                command.Parameters.Add(new SqlParameter("@value", extendedProperty.Value));

                var level0type = new SqlParameter("@level0type", SqlDbType.VarChar);

                if (string.IsNullOrEmpty(extendedProperty.Level0Type))
                    level0type.Value = DBNull.Value;
                else
                    level0type.Value = extendedProperty.Level0Type;

                command.Parameters.Add(level0type);

                var level0name = new SqlParameter("@level0name", SqlDbType.VarChar);

                if (string.IsNullOrEmpty(extendedProperty.Level0Name))
                    level0name.Value = DBNull.Value;
                else
                    level0name.Value = extendedProperty.Level0Name;

                command.Parameters.Add(level0name);

                var level1type = new SqlParameter("@level1type", SqlDbType.VarChar);

                if (string.IsNullOrEmpty(extendedProperty.Level1Type))
                    level1type.Value = DBNull.Value;
                else
                    level1type.Value = extendedProperty.Level1Type;

                command.Parameters.Add(level1type);

                var level1name = new SqlParameter("@level1name", SqlDbType.VarChar);

                if (string.IsNullOrEmpty(extendedProperty.Level1Name))
                    level1name.Value = DBNull.Value;
                else
                    level1name.Value = extendedProperty.Level1Name;

                command.Parameters.Add(level1name);

                var level2type = new SqlParameter("@level2type", SqlDbType.VarChar);

                if (string.IsNullOrEmpty(extendedProperty.Level2Type))
                    level2type.Value = DBNull.Value;
                else
                    level2type.Value = extendedProperty.Level2Type;

                command.Parameters.Add(level2type);

                var level2name = new SqlParameter("@level2name", SqlDbType.VarChar);

                if (string.IsNullOrEmpty(extendedProperty.Level2Name))
                    level2name.Value = DBNull.Value;
                else
                    level2name.Value = extendedProperty.Level2Name;

                command.Parameters.Add(level2name);

                command.ExecuteNonQuery();
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="extendedProperty"></param>
        public void UpdateExtendedProperty(ExtendedProperty extendedProperty)
        {
            using (var command = Connection.CreateCommand())
            {
                command.Connection = Connection;
                command.CommandText = "exec [sys].[sp_updateextendedproperty] @name, @value, @level0type, @level0name, @level1type, @level1name, @level2type, @level2name";

                command.Parameters.Add(new SqlParameter("@name", extendedProperty.Name));
                command.Parameters.Add(new SqlParameter("@value", extendedProperty.Value));

                var level0type = new SqlParameter("@level0type", SqlDbType.VarChar);

                if (string.IsNullOrEmpty(extendedProperty.Level0Type))
                    level0type.Value = DBNull.Value;
                else
                    level0type.Value = extendedProperty.Level0Type;

                command.Parameters.Add(level0type);

                var level0name = new SqlParameter("@level0name", SqlDbType.VarChar);

                if (string.IsNullOrEmpty(extendedProperty.Level0Name))
                    level0name.Value = DBNull.Value;
                else
                    level0name.Value = extendedProperty.Level0Name;

                command.Parameters.Add(level0name);

                var level1type = new SqlParameter("@level1type", SqlDbType.VarChar);

                if (string.IsNullOrEmpty(extendedProperty.Level1Type))
                    level1type.Value = DBNull.Value;
                else
                    level1type.Value = extendedProperty.Level1Type;

                command.Parameters.Add(level1type);

                var level1name = new SqlParameter("@level1name", SqlDbType.VarChar);

                if (string.IsNullOrEmpty(extendedProperty.Level1Name))
                    level1name.Value = DBNull.Value;
                else
                    level1name.Value = extendedProperty.Level1Name;

                command.Parameters.Add(level1name);

                var level2type = new SqlParameter("@level2type", SqlDbType.VarChar);

                if (string.IsNullOrEmpty(extendedProperty.Level2Type))
                    level2type.Value = DBNull.Value;
                else
                    level2type.Value = extendedProperty.Level2Type;

                command.Parameters.Add(level2type);

                var level2name = new SqlParameter("@level2name", SqlDbType.VarChar);

                if (string.IsNullOrEmpty(extendedProperty.Level2Name))
                    level2name.Value = DBNull.Value;
                else
                    level2name.Value = extendedProperty.Level2Name;

                command.Parameters.Add(level2name);

                command.ExecuteNonQuery();
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="extendedProperty"></param>
        public void DropExtendedProperty(ExtendedProperty extendedProperty)
        {
            using (var command = Connection.CreateCommand())
            {
                command.Connection = Connection;
                command.CommandText = "exec [sys].[sp_dropextendedproperty] @name, @level0type, @level0name, @level1type, @level1name, @level2type, @level2name";

                command.Parameters.Add(new SqlParameter("@name", extendedProperty.Name));

                var level0type = new SqlParameter("@level0type", SqlDbType.VarChar);

                if (string.IsNullOrEmpty(extendedProperty.Level0Type))
                    level0type.Value = DBNull.Value;
                else
                    level0type.Value = extendedProperty.Level0Type;

                command.Parameters.Add(level0type);

                var level0name = new SqlParameter("@level0name", SqlDbType.VarChar);

                if (string.IsNullOrEmpty(extendedProperty.Level0Name))
                    level0name.Value = DBNull.Value;
                else
                    level0name.Value = extendedProperty.Level0Name;

                command.Parameters.Add(level0name);

                var level1type = new SqlParameter("@level1type", SqlDbType.VarChar);

                if (string.IsNullOrEmpty(extendedProperty.Level1Type))
                    level1type.Value = DBNull.Value;
                else
                    level1type.Value = extendedProperty.Level1Type;

                command.Parameters.Add(level1type);

                var level1name = new SqlParameter("@level1name", SqlDbType.VarChar);

                if (string.IsNullOrEmpty(extendedProperty.Level1Name))
                    level1name.Value = DBNull.Value;
                else
                    level1name.Value = extendedProperty.Level1Name;

                command.Parameters.Add(level1name);

                var level2type = new SqlParameter("@level2type", SqlDbType.VarChar);

                if (string.IsNullOrEmpty(extendedProperty.Level2Type))
                    level2type.Value = DBNull.Value;
                else
                    level2type.Value = extendedProperty.Level2Type;

                command.Parameters.Add(level2type);

                var level2name = new SqlParameter("@level2name", SqlDbType.VarChar);

                if (string.IsNullOrEmpty(extendedProperty.Level2Name))
                    level2name.Value = DBNull.Value;
                else
                    level2name.Value = extendedProperty.Level2Name;

                command.Parameters.Add(level2name);

                command.ExecuteNonQuery();
            }
        }
    }
}
