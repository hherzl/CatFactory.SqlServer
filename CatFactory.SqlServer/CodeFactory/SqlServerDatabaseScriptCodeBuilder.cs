using System.Collections.Generic;
using System.Linq;
using System.Text;
using CatFactory.CodeFactory;
using CatFactory.ObjectRelationalMapping;

namespace CatFactory.SqlServer.CodeFactory
{
    /// <summary>
    /// Represents a code builder for tables
    /// </summary>
    public class SqlServerDatabaseScriptCodeBuilder : CodeBuilder
    {
        /// <summary>
        /// Initializes a new instance of <see cref="SqlServerDatabaseScriptCodeBuilder"/> class
        /// </summary>
        public SqlServerDatabaseScriptCodeBuilder()
            : base()
        {
        }

        /// <summary>
        /// Gets the file name
        /// </summary>
        public override string FileName
            => Database.Name;

        /// <summary>
        /// Gets the file extension
        /// </summary>
        public override string FileExtension
            => "sql";

        /// <summary>
        /// Gets or sets the database
        /// </summary>
        public Database Database { get; set; }

        /// <summary>
        /// Translates object definition to a sequence of <see cref="ILine"/> interface
        /// </summary>
        public override void Translating()
        {
            Lines = new List<ILine>
            {
                new CodeLine(Code)
            };
        }

        /// <summary>
        /// Gets the output code for current <see cref="SqlServerDatabaseScriptCodeBuilder"/> instance
        /// </summary>
        protected string Code
        {
            get
            {
                var output = new StringBuilder();

                output.AppendFormat("create database {0}", Database.GetObjectName(Database.Name));
                output.AppendLine();

                output.AppendLine("go");

                output.AppendLine();

                output.AppendFormat("use {0}", Database.GetObjectName(Database.Name));
                output.AppendLine();

                output.AppendLine("go");

                if (Database.ExtendedProperties.Count > 0)
                {
                    output.AppendLine();

                    foreach (var extendedProperty in Database.ExtendedProperties)
                    {
                        output.AppendLine("exec [sp_addextendedproperty]");

                        output.AppendFormat("{0}@name = '{1}', @value = '{2}', ", Indent(1), extendedProperty.Name, extendedProperty.Value);
                        output.Append("@level0type = null, @level0name = null, ");
                        output.Append("@level1type = null, @level1name = null, ");
                        output.Append("@level2type = null, @level2name = null");
                        output.AppendLine();
                    }

                    output.AppendLine("go");
                }

                output.AppendLine();

                var schemas = Database.Tables.Select(item => item.Schema).Distinct().ToList();

                foreach (var schema in schemas)
                {
                    if (string.IsNullOrEmpty(schema) || Database.DefaultSchema == schema)
                        continue;

                    output.AppendFormat("create schema {0}", Database.GetObjectName(schema));
                    output.AppendLine();

                    output.AppendLine("go");

                    output.AppendLine();
                }

                foreach (var table in Database.Tables)
                {
                    output.AppendFormat("create table {0}", Database.GetObjectName(table));
                    output.AppendLine();

                    output.AppendLine("(");

                    for (var i = 0; i < table.Columns.Count; i++)
                    {
                        var column = table.Columns[i];

                        if (Database.ColumnIsString(column))
                        {
                            if (column.Length == 0)
                                output.AppendFormat("{0}{1} {2}({3})", Indent(1), Database.GetObjectName(column), column.Type, "max");
                            else
                                output.AppendFormat("{0}{1} {2}({3})", Indent(1), Database.GetObjectName(column), column.Type, column.Length);
                        }
                        else if (Database.ColumnIsNumber(column))
                        {
                            if (column.Prec > 0 && column.Scale > 0)
                                output.AppendFormat("{0}{1} {2}({3}, {4})", Indent(1), Database.GetObjectName(column), column.Type, column.Prec, column.Scale);
                            else if (column.Prec > 0)
                                output.AppendFormat("{0}{1} {2}({3})", Indent(1), Database.GetObjectName(column), column.Type, column.Prec);
                        }
                        else
                        {
                            output.AppendFormat("{0}{1} {2}", Indent(1), Database.GetObjectName(column), column.Type);
                        }

                        output.AppendFormat(" {0}", column.Nullable ? "null" : "not null");

                        if (table.Identity != null && table.Identity.Name == column.Name)
                            output.AppendFormat(" identity({0}, {1})", table.Identity.Seed, table.Identity.Increment);

                        if (i < table.Columns.Count - 1)
                            output.Append(",");

                        output.AppendLine();
                    }

                    output.AppendLine(")");

                    output.AppendLine("go");

                    output.AppendLine();

                    foreach (var extendedProperty in table.ExtendedProperties)
                    {
                        output.AppendLine("exec [sp_addextendedproperty]");

                        output.AppendFormat("{0}@name = '{1}', @value = '{2}', ", Indent(1), extendedProperty.Name, extendedProperty.Value);
                        output.AppendFormat("@level0type = '{0}', @level0name = '{1}', ", "schema", table.Schema);
                        output.AppendFormat("@level1type = '{0}', @level1name = '{1}', ", "table", table.Name);
                        output.AppendLine("@level2type = null, @level2name = null");

                        output.AppendLine("go");
                        output.AppendLine();
                    }
                }

                foreach (var table in Database.Tables)
                {
                    if (table.PrimaryKey != null)
                    {
                        var pk = table.PrimaryKey;

                        var constraintName = string.IsNullOrEmpty(pk.ConstraintName) ? Database.NamingConvention.GetPrimaryKeyConstraintName(table, pk.Key.ToArray()) : Database.GetObjectName(pk.ConstraintName);

                        output.AppendFormat("alter table {0} add constraint {1} primary key ({2})", Database.GetObjectName(table), constraintName, string.Join(", ", pk.Key.Select(item => Database.GetObjectName(item))));
                        output.AppendLine();

                        output.AppendLine("go");

                        output.AppendLine();
                    }

                    foreach (var unique in table.Uniques)
                    {
                        var constraintName = string.IsNullOrEmpty(unique.ConstraintName) ? Database.NamingConvention.GetUniqueConstraintName(table, unique.Key.ToArray()) : Database.GetObjectName(unique.ConstraintName);

                        output.AppendFormat("alter table {0} add constraint {1} unique ({2})", Database.GetObjectName(table), constraintName, string.Join(", ", unique.Key.Select(item => Database.GetObjectName(item))));
                        output.AppendLine();

                        output.AppendLine("go");

                        output.AppendLine();
                    }

                    for (var i = 0; i < table.ForeignKeys.Count; i++)
                    {
                        var foreignKey = table.ForeignKeys[i];

                        var references = Database.FindTable(foreignKey.References);

                        if (references == null)
                        {
                            output.AppendFormat("/* There is not a table with name: {0} */", foreignKey.References);
                            output.AppendLine();
                        }
                        else
                        {
                            if (references.PrimaryKey != null)
                            {
                                var constraintName = string.IsNullOrEmpty(foreignKey.ConstraintName) ? Database.NamingConvention.GetForeignKeyConstraintName(table, foreignKey.Key.ToArray(), references) : Database.GetObjectName(foreignKey.ConstraintName);

                                output.AppendFormat("alter table {0} add constraint {1} foreign key ({2}) references {3}", Database.GetObjectName(table), constraintName, string.Join(", ", foreignKey.Key.Select(item => Database.GetObjectName(item))), Database.GetObjectName(references));
                                output.AppendLine();

                                output.AppendLine("go");

                                output.AppendLine();
                            }
                        }
                    }

                    var columnsWithExtendedProperties = table.Columns.Where(item => item.ExtendedProperties.Count > 0).ToList();

                    if (columnsWithExtendedProperties.Count > 0)
                    {
                        for (var i = 0; i < columnsWithExtendedProperties.Count; i++)
                        {
                            var column = columnsWithExtendedProperties[i];

                            foreach (var extendedProperty in column.ExtendedProperties)
                            {
                                output.AppendLine("exec [sp_addextendedproperty]");

                                output.AppendFormat("{0}@name = '{1}', @value = '{2}', ", Indent(1), extendedProperty.Name, extendedProperty.Value);
                                output.AppendFormat("@level0type = '{0}', @level0name = '{1}', ", "schema", table.Schema);
                                output.AppendFormat("@level1type = '{0}', @level1name = '{1}', ", "table", table.Name);
                                output.AppendFormat("@level2type = '{0}', @level2name = '{1}'", "column", column.Name);
                                output.AppendLine();
                            }
                        }

                        output.AppendLine("go");

                        output.AppendLine();
                    }
                }

                return output.ToString();
            }
        }
    }
}
