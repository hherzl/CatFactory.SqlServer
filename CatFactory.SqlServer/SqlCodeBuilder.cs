using System.Linq;
using System.Text;
using CatFactory.CodeFactory;
using CatFactory.Mapping;

namespace CatFactory.SqlServer
{
    /// <summary>
    /// 
    /// </summary>
    public class SqlCodeBuilder : CodeBuilder
    {
        /// <summary>
        /// 
        /// </summary>
        public SqlCodeBuilder()
        {
        }

        /// <summary>
        /// 
        /// </summary>
        public Database Database { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public override string FileName
            => Database.Name;

        /// <summary>
        /// 
        /// </summary>
        public override string FileExtension
            => "sql";

        /// <summary>
        /// 
        /// </summary>
        public string Code
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
