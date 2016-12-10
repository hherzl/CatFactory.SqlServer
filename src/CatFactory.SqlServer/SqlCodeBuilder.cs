using System;
using System.Linq;
using System.Text;
using CatFactory.CodeFactory;
using CatFactory.Mapping;

namespace CatFactory.SqlServer
{
    public class SqlCodeBuilder : CodeBuilder
    {
        public SqlCodeBuilder()
        {
        }

        public Database Database { get; set; }

        public override String FileName
        {
            get
            {
                return Database.Name;
            }
        }

        public override String FileExtension
        {
            get
            {
                return "sql";
            }
        }

        public virtual String GetObjectName(Table table)
        {
            return String.IsNullOrEmpty(table.Schema) ? String.Format("[{0}]", table.Name) : String.Format("[{0}].[{1}]", table.Schema, table.Name);
        }

        public virtual String GetObjectName(Column column)
        {
            return String.Format("[{0}]", column.Name);
        }

        public virtual String GetObjectName(String value)
        {
            return String.Format("[{0}]", value);
        }

        public override String Code
        {
            get
            {
                var output = new StringBuilder();

                var schemas = Database.Tables.Select(item => item.Schema).Distinct().ToList();

                foreach (var schema in schemas)
                {
                    if (String.IsNullOrEmpty(schema))
                    {
                        continue;
                    }

                    output.AppendFormat("create schema {0}", schema);
                    output.AppendLine();

                    output.AppendFormat("go");
                    output.AppendLine();

                    output.AppendLine();
                }

                foreach (var table in Database.Tables)
                {
                    output.AppendFormat("create table {0}", GetObjectName(table));
                    output.AppendLine();

                    output.AppendLine("(");

                    for (var i = 0; i < table.Columns.Count; i++)
                    {
                        var column = table.Columns[i];

                        output.AppendFormat("{0}{1} {2}", Indent(1), GetObjectName(column), column.Type);

                        if (column.Length > 0)
                        {
                            output.AppendFormat("({0})", column.Prec > 0 ? String.Format("{0}, {1}", column.Length, column.Prec) : column.Length.ToString());
                        }

                        output.AppendFormat(" {0}", column.Nullable ? "null" : "not null");

                        if (table.Identity != null && table.Identity.Name == column.Name)
                        {
                            output.AppendFormat(" identity({0}, {1})", table.Identity.Seed, table.Identity.Increment);
                        }

                        if (i < table.Columns.Count - 1)
                        {
                            output.Append(",");
                        }

                        output.AppendLine();
                    }

                    output.AppendLine(")");
                    output.AppendLine();
                }

                foreach (var table in Database.Tables)
                {
                    if (table.PrimaryKey != null)
                    {
                        output.AppendFormat("alter table {0} add constraint {1}_PK primary key ({2})", GetObjectName(table), table.FullName.Replace(".", "_"), String.Join(", ", table.PrimaryKey.Key));
                        output.AppendLine();

                        output.AppendFormat("go");
                        output.AppendLine();

                        output.AppendLine();
                    }
                }
                
                return output.ToString();
            }
        }
    }
}
