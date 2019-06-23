using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;
using CatFactory.ObjectRelationalMapping;

namespace CatFactory.SqlServer.Features
{
    /// <summary>
    /// Implements operations to manipulate extended properties
    /// </summary>
    public class ExtendedPropertyRepository : IExtendedPropertyRepository
    {
        private DbConnection Connection;

        /// <summary>
        /// Initializes a new instance of <see cref="ExtendedPropertyRepository"/> class
        /// </summary>
        /// <param name="connection">Connection to database</param>
        public ExtendedPropertyRepository(DbConnection connection)
        {
            Connection = connection;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="name"></param>
        /// <param name="sqlDbType"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        protected virtual SqlParameter GetParameter(string name, SqlDbType sqlDbType, string value)
        {
            var parameter = new SqlParameter(name, sqlDbType);

            if (string.IsNullOrEmpty(value))
                parameter.Value = DBNull.Value;
            else
                parameter.Value = value;

            return parameter;
        }

        /// <summary>
        /// Gets extended properties
        /// </summary>
        /// <param name="extendedProperty">Search parameter</param>
        /// <returns>A sequence of <see cref="ExtendedProperty"/> class</returns>
        public IEnumerable<ExtendedProperty> GetExtendedProperties(ExtendedProperty extendedProperty)
        {
            using (var command = Connection.CreateCommand())
            {
                command.Connection = Connection;
                command.CommandType = CommandType.Text;
                command.CommandText = @"
                select
                    [objtype], [objname], [name], [value]
                from
                    [fn_listextendedproperty]
                    (
                        @name, @level0type, @level0name, @level1type, @level1name, @level2type, @level2name
                    )
                ";

                command.Parameters.Add(new SqlParameter("@name", extendedProperty.Name));
                command.Parameters.Add(GetParameter("@level0type", SqlDbType.VarChar, extendedProperty.Level0Type));
                command.Parameters.Add(GetParameter("@level0name", SqlDbType.VarChar, extendedProperty.Level0Name));
                command.Parameters.Add(GetParameter("@level1type", SqlDbType.VarChar, extendedProperty.Level1Type));
                command.Parameters.Add(GetParameter("@level1name", SqlDbType.VarChar, extendedProperty.Level1Name));
                command.Parameters.Add(GetParameter("@level2type", SqlDbType.VarChar, extendedProperty.Level2Type));
                command.Parameters.Add(GetParameter("@level2name", SqlDbType.VarChar, extendedProperty.Level2Name));

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
        /// Adds an extended property
        /// </summary>
        /// <param name="extendedProperty">Instance of <see cref="ExtendedProperty"/> class to add</param>
        public void AddExtendedProperty(ExtendedProperty extendedProperty)
        {
            using (var command = Connection.CreateCommand())
            {
                command.Connection = Connection;
                command.CommandType = CommandType.Text;
                command.CommandText = " exec [sys].[sp_addextendedproperty] @name, @value, @level0type, @level0name, @level1type, @level1name, @level2type, @level2name ";

                command.Parameters.Add(new SqlParameter("@name", extendedProperty.Name));
                command.Parameters.Add(new SqlParameter("@value", extendedProperty.Value));
                command.Parameters.Add(GetParameter("@level0type", SqlDbType.VarChar, extendedProperty.Level0Type));
                command.Parameters.Add(GetParameter("@level0name", SqlDbType.VarChar, extendedProperty.Level0Name));
                command.Parameters.Add(GetParameter("@level1type", SqlDbType.VarChar, extendedProperty.Level1Type));
                command.Parameters.Add(GetParameter("@level1name", SqlDbType.VarChar, extendedProperty.Level1Name));
                command.Parameters.Add(GetParameter("@level2type", SqlDbType.VarChar, extendedProperty.Level2Type));
                command.Parameters.Add(GetParameter("@level2name", SqlDbType.VarChar, extendedProperty.Level2Name));

                command.ExecuteNonQuery();
            }
        }

        /// <summary>
        /// Updates an extended property
        /// </summary>
        /// <param name="extendedProperty">Instance of <see cref="ExtendedProperty"/> class to update</param>
        public void UpdateExtendedProperty(ExtendedProperty extendedProperty)
        {
            using (var command = Connection.CreateCommand())
            {
                command.Connection = Connection;
                command.CommandType = CommandType.Text;
                command.CommandText = " exec [sys].[sp_updateextendedproperty] @name, @value, @level0type, @level0name, @level1type, @level1name, @level2type, @level2name ";

                command.Parameters.Add(new SqlParameter("@name", extendedProperty.Name));
                command.Parameters.Add(new SqlParameter("@value", extendedProperty.Value));
                command.Parameters.Add(GetParameter("@level0type", SqlDbType.VarChar, extendedProperty.Level0Type));
                command.Parameters.Add(GetParameter("@level0name", SqlDbType.VarChar, extendedProperty.Level0Name));
                command.Parameters.Add(GetParameter("@level1type", SqlDbType.VarChar, extendedProperty.Level1Type));
                command.Parameters.Add(GetParameter("@level1name", SqlDbType.VarChar, extendedProperty.Level1Name));
                command.Parameters.Add(GetParameter("@level2type", SqlDbType.VarChar, extendedProperty.Level2Type));
                command.Parameters.Add(GetParameter("@level2name", SqlDbType.VarChar, extendedProperty.Level2Name));

                command.ExecuteNonQuery();
            }
        }

        /// <summary>
        /// Drops an extended property
        /// </summary>
        /// <param name="extendedProperty">Instance of <see cref="ExtendedProperty"/> class to drop</param>
        public void DropExtendedProperty(ExtendedProperty extendedProperty)
        {
            using (var command = Connection.CreateCommand())
            {
                command.Connection = Connection;
                command.CommandType = CommandType.Text;
                command.CommandText = " exec [sys].[sp_dropextendedproperty] @name, @level0type, @level0name, @level1type, @level1name, @level2type, @level2name ";

                command.Parameters.Add(new SqlParameter("@name", extendedProperty.Name));
                command.Parameters.Add(GetParameter("@level0type", SqlDbType.VarChar, extendedProperty.Level0Type));
                command.Parameters.Add(GetParameter("@level0name", SqlDbType.VarChar, extendedProperty.Level0Name));
                command.Parameters.Add(GetParameter("@level1type", SqlDbType.VarChar, extendedProperty.Level1Type));
                command.Parameters.Add(GetParameter("@level1name", SqlDbType.VarChar, extendedProperty.Level1Name));
                command.Parameters.Add(GetParameter("@level2type", SqlDbType.VarChar, extendedProperty.Level2Type));
                command.Parameters.Add(GetParameter("@level2name", SqlDbType.VarChar, extendedProperty.Level2Name));

                command.ExecuteNonQuery();
            }
        }
    }
}
