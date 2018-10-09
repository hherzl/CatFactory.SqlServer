using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;
using CatFactory.Mapping;

namespace CatFactory.SqlServer
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
        /// Gets extended properties
        /// </summary>
        /// <param name="extendedProperty">Search parameter</param>
        /// <returns>A sequence of <see cref="ExtendedProperty"/> class</returns>
        public IEnumerable<ExtendedProperty> GetExtendedProperties(ExtendedProperty extendedProperty)
        {
            using (var command = Connection.CreateCommand())
            {
                command.Connection = Connection;
                command.CommandText = " select [objtype], [objname], [name], [value] from [fn_listextendedproperty](@name, @level0type, @level0name, @level1type, @level1name, @level2type, @level2name) ";
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
        /// Adds an extended property
        /// </summary>
        /// <param name="extendedProperty">Instance of <see cref="ExtendedProperty"/> class to add</param>
        public void AddExtendedProperty(ExtendedProperty extendedProperty)
        {
            using (var command = Connection.CreateCommand())
            {
                command.Connection = Connection;
                command.CommandText = " exec [sys].[sp_addextendedproperty] @name, @value, @level0type, @level0name, @level1type, @level1name, @level2type, @level2name ";
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
        /// Updates an extended property
        /// </summary>
        /// <param name="extendedProperty">Instance of <see cref="ExtendedProperty"/> class to update</param>
        public void UpdateExtendedProperty(ExtendedProperty extendedProperty)
        {
            using (var command = Connection.CreateCommand())
            {
                command.Connection = Connection;
                command.CommandText = " exec [sys].[sp_updateextendedproperty] @name, @value, @level0type, @level0name, @level1type, @level1name, @level2type, @level2name ";
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
        /// Drops an extended property
        /// </summary>
        /// <param name="extendedProperty">Instance of <see cref="ExtendedProperty"/> class to drop</param>
        public void DropExtendedProperty(ExtendedProperty extendedProperty)
        {
            using (var command = Connection.CreateCommand())
            {
                command.Connection = Connection;
                command.CommandText = " exec [sys].[sp_dropextendedproperty] @name, @level0type, @level0name, @level1type, @level1name, @level2type, @level2name ";
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
