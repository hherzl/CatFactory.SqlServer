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

                output.AppendFormat("go");
                output.AppendLine();

                output.AppendLine();

                output.AppendFormat("use {0}", Database.GetObjectName(Database.Name));
                output.AppendLine();

                output.AppendFormat("go");
                output.AppendLine();

                output.AppendLine();

                var schemas = Database.Tables.Select(item => item.Schema).Distinct().ToList();

                foreach (var schema in schemas)
                {
                    if (string.IsNullOrEmpty(schema))
                        continue;

                    output.AppendFormat("create schema {0}", Database.GetObjectName(schema));
                    output.AppendLine();

                    output.AppendFormat("go");
                    output.AppendLine();

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

                        output.AppendFormat("{0}{1} {2}", Indent(1), Database.GetObjectName(column), column.Type);

                        if (column.Length > 0)
                            output.AppendFormat("({0})", column.Prec > 0 ? string.Format("{0}, {1}", column.Prec, column.Scale) : column.Length.ToString());

                        output.AppendFormat(" {0}", column.Nullable ? "null" : "not null");

                        if (table.Identity != null && table.Identity.Name == column.Name)
                            output.AppendFormat(" identity({0}, {1})", table.Identity.Seed, table.Identity.Increment);

                        if (i < table.Columns.Count - 1)
                            output.Append(",");

                        output.AppendLine();
                    }

                    output.AppendLine(")");

                    output.AppendFormat("go");
                    output.AppendLine();

                    output.AppendLine();
                }

                foreach (var table in Database.Tables)
                {
                    if (table.PrimaryKey != null)
                    {
                        var pk = table.PrimaryKey;

                        var constraintName = string.IsNullOrEmpty(pk.ConstraintName) ? Database.NamingConvention.GetPrimaryKeyConstraintName(table, pk.Key.ToArray()) : Database.GetObjectName(pk.ConstraintName);

                        output.AppendFormat("alter table {0} add constraint {1} primary key ({2})", Database.GetObjectName(table), constraintName, string.Join(", ", pk.Key.Select(item => Database.GetObjectName(item))));
                        output.AppendLine();

                        output.AppendFormat("go");
                        output.AppendLine();

                        output.AppendLine();
                    }

                    foreach (var unique in table.Uniques)
                    {
                        var constraintName = string.IsNullOrEmpty(unique.ConstraintName) ? Database.NamingConvention.GetUniqueConstraintName(table, unique.Key.ToArray()) : Database.GetObjectName(unique.ConstraintName);

                        output.AppendFormat("alter table {0} add constraint {1} unique ({2})", Database.GetObjectName(table), constraintName, string.Join(", ", unique.Key.Select(item => Database.GetObjectName(item))));
                        output.AppendLine();

                        output.AppendFormat("go");
                        output.AppendLine();

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

                                output.AppendFormat("go");
                                output.AppendLine();

                                output.AppendLine();
                            }
                        }
                    }
                }

                return output.ToString();
            }
        }
    }
}
