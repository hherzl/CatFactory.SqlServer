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
    public class SqlCodeBuilder : CodeBuilder
    {
        /// <summary>
        /// Initializes a new instance of <see cref="SqlCodeBuilder"/> class
        /// </summary>
        public SqlCodeBuilder()
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
        /// Gets the output code for current <see cref="SqlCodeBuilder"/> instance
        /// </summary>
        protected string Code
        {
            get
            {
                var output = new StringBuilder();

                var schemas = Database.Tables.Select(item => item.Schema).Distinct().ToList();

                foreach (var schema in schemas)
                {
                    if (string.IsNullOrEmpty(schema))
                        continue;

                    output.AppendFormat("create schema {0}", schema);
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
                    output.AppendLine();
                }

                foreach (var table in Database.Tables)
                {
                    if (table.PrimaryKey != null)
                    {
                        var constraintName = Database.NamingConvention.GetPrimaryKeyConstraintName(table, table.PrimaryKey.Key.ToArray());

                        output.AppendFormat("alter table {0} add constraint {1} primary key ({2})", Database.GetObjectName(table), constraintName, string.Join(", ", table.PrimaryKey.Key));
                        output.AppendLine();

                        output.AppendFormat("go");
                        output.AppendLine();

                        output.AppendLine();
                    }

                    // todo: Add foreign key in script

                    foreach (var unique in table.Uniques)
                    {
                        var constraintName = Database.NamingConvention.GetUniqueConstraintName(table, unique.Key.ToArray());

                        output.AppendFormat("alter table {0} add constraint {1} unique ({2})", Database.GetObjectName(table), constraintName, string.Join(", ", unique.Key));
                        output.AppendLine();

                        output.AppendFormat("go");
                        output.AppendLine();
                    }
                }

                return output.ToString();
            }
        }
    }
}
