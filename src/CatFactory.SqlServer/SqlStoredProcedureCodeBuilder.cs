using System;
using System.Linq;
using System.Text;
using CatFactory.CodeFactory;
using CatFactory.Mapping;

namespace CatFactory.SqlServer
{
    public class SqlStoredProcedureCodeBuilder : CodeBuilder
    {
        public SqlStoredProcedureCodeBuilder()
        {
        }

        public Table Table { get; set; }

        public override String FileName
        {
            get
            {
                return Table.FullName;
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

        public virtual String GetObjectName(String name)
        {
            return String.Format("[{0}]", name);
        }

        public virtual String GetParameterName(Column column)
        {
            return String.Format("@{0}", NamingConvention.GetCamelCase(column.Name));
        }

        public virtual String GetParameterName(String name)
        {
            return String.Format("@{0}", NamingConvention.GetCamelCase(name));
        }

        public virtual String GetProcedureName(String action)
        {
            return String.IsNullOrEmpty(Table.Schema) ? String.Format("[{0}]", Table.Name) : String.Format("[{0}].[{1}{2}]", Table.Schema, Table.Name, action);
        }

        public virtual String GetType(Column column)
        {
            if (column.Length == 0)
            {
                return String.Format("{0}", column.Type);
            }
            else
            {
                return column.Prec == 0 ? String.Format("{0}({1})", column.Type, column.Length) : String.Format("{0}({1}, {2})", column.Type, column.Length, column.Prec);
            }
        }

        public override String Code
        {
            get
            {
                var output = new StringBuilder();

                GetAllProcedure(output);
                output.AppendLine();

                GetProcedure(output);
                output.AppendLine();

                InsertProcedure(output);
                output.AppendLine();

                UpdateProcedure(output);
                output.AppendLine();

                DeleteProcedure(output);
                output.AppendLine();

                return output.ToString();
            }
        }

        protected virtual void DropProcedure(StringBuilder output, String procedureName)
        {
            output.AppendFormat("if object_id('{0}', 'P') is not null", procedureName.Replace("[", String.Empty).Replace("]", String.Empty));
            output.AppendLine();

            output.AppendFormat("{0}drop procedure {1}", Indent(1), procedureName);
            output.AppendLine();

            output.AppendFormat("go");
            output.AppendLine();
        }

        protected virtual void GetAllProcedure(StringBuilder output)
        {
            var procedureName = GetProcedureName("GetAll");

            DropProcedure(output, procedureName);

            output.AppendLine();

            output.AppendFormat("create procedure {0}", procedureName);
            output.AppendLine();

            output.AppendFormat("as");
            output.AppendLine();

            output.AppendFormat("{0}select", Indent(1));
            output.AppendLine();

            for (var i = 0; i < Table.Columns.Count; i++)
            {
                var column = Table.Columns[i];

                output.AppendFormat("{0}{1}", Indent(2), GetObjectName(column));

                if (i < Table.Columns.Count - 1)
                {
                    output.Append(",");
                }

                output.AppendLine();
            }

            output.AppendFormat("{0}from", Indent(1));
            output.AppendLine();

            output.AppendFormat("{0}{1}", Indent(2), GetObjectName(Table));
            output.AppendLine();

            output.AppendFormat("go");
            output.AppendLine();
        }

        protected virtual void GetProcedure(StringBuilder output)
        {
            var procedureName = GetProcedureName("Get");

            DropProcedure(output, procedureName);

            output.AppendLine();

            output.AppendFormat("create procedure {0}", procedureName);
            output.AppendLine();

            if (Table.PrimaryKey != null)
            {
                for (var i = 0; i < Table.PrimaryKey.Key.Count; i++)
                {
                    var key = Table.PrimaryKey.Key[i];

                    var column = Table.Columns.FirstOrDefault(x => x.Name == key);

                    output.AppendFormat("{0}{1} {2}", Indent(1), GetParameterName(key), GetType(column));
                }
            }

            output.AppendLine();

            output.AppendFormat("as");
            output.AppendLine();

            output.AppendFormat("{0}select", Indent(1));
            output.AppendLine();

            for (var i = 0; i < Table.Columns.Count; i++)
            {
                var column = Table.Columns[i];

                output.AppendFormat("{0}{1}", Indent(2), GetObjectName(column));

                if (i < Table.Columns.Count - 1)
                {
                    output.Append(",");
                }

                output.AppendLine();
            }

            output.AppendFormat("{0}from", Indent(1));
            output.AppendLine();

            output.AppendFormat("{0}{1}", Indent(2), GetObjectName(Table));
            output.AppendLine();

            output.AppendFormat("{0}where", Indent(1));
            output.AppendLine();

            if (Table.PrimaryKey != null)
            {
                for (var i = 0; i < Table.PrimaryKey.Key.Count; i++)
                {
                    var item = Table.PrimaryKey.Key[i];

                    output.AppendFormat("{0}{1} = {2}", Indent(2), GetObjectName(item), GetParameterName(item));

                    if (i < Table.PrimaryKey.Key.Count - 1)
                    {
                        output.Append(",");
                    }

                    output.AppendLine();
                }
            }

            output.AppendFormat("go");
            output.AppendLine();
        }

        protected virtual String InsertProcedure(StringBuilder output)
        {
            var procedureName = GetProcedureName("Add");

            DropProcedure(output, procedureName);

            output.AppendLine();

            output.AppendFormat("create procedure {0}", procedureName);
            output.AppendLine();

            for (var i = 0; i < Table.Columns.Count; i++)
            {
                var column = Table.Columns[i];

                output.AppendFormat("{0}{1} {2}", Indent(1), GetParameterName(column), GetType(column));

                if (Table.Identity != null && Table.Identity.Name == column.Name)
                {
                    output.AppendFormat(" output");
                }

                if (i < Table.Columns.Count - 1)
                {
                    output.Append(",");
                }

                output.AppendLine();
            }

            output.AppendFormat("as");
            output.AppendLine();

            output.AppendFormat("{0}insert into {1}", Indent(1), GetObjectName(Table));
            output.AppendLine();

            output.AppendFormat("{0}(", Indent(1));
            output.AppendLine();

            var columns = Table.GetColumnsWithOutIdentity().ToList();

            for (var i = 0; i < columns.Count; i++)
            {
                var column = columns[i];

                output.AppendFormat("{0}{1}", Indent(2), GetObjectName(column));

                if (i < columns.Count - 1)
                {
                    output.Append(",");
                }

                output.AppendLine();
            }

            output.AppendFormat("{0})", Indent(1));
            output.AppendLine();

            output.AppendFormat("{0}values", Indent(1));
            output.AppendLine();

            output.AppendFormat("{0}(", Indent(1));
            output.AppendLine();

            for (var i = 0; i < columns.Count; i++)
            {
                var column = columns[i];

                output.AppendFormat("{0}{1}", Indent(2), GetParameterName(column));

                if (i < columns.Count - 1)
                {
                    output.Append(",");
                }

                output.AppendLine();
            }

            output.AppendFormat("{0})", Indent(1));
            output.AppendLine();

            if (Table.Identity != null)
            {
                output.AppendLine();

                output.AppendFormat("{0}select {1} = @@identity", Indent(1), GetParameterName(Table.Identity.Name));
                output.AppendLine();
            }

            output.AppendFormat("go");
            output.AppendLine();

            return output.ToString();
        }

        protected virtual String UpdateProcedure(StringBuilder output)
        {
            var procedureName = GetProcedureName("Update");

            DropProcedure(output, procedureName);

            output.AppendLine();

            output.AppendFormat("create procedure {0}", procedureName);
            output.AppendLine();

            for (var i = 0; i < Table.Columns.Count; i++)
            {
                var column = Table.Columns[i];

                output.AppendFormat("{0}{1} {2}", Indent(1), GetParameterName(column), GetType(column));

                if (i < Table.Columns.Count - 1)
                {
                    output.Append(",");
                }

                output.AppendLine();
            }

            output.AppendFormat("as");
            output.AppendLine();

            output.AppendFormat("{0}update", Indent(1));
            output.AppendLine();

            output.AppendFormat("{0}{1}", Indent(2), GetObjectName(Table));
            output.AppendLine();

            output.AppendFormat("{0}set", Indent(1));
            output.AppendLine();

            var columns = Table.GetColumnsWithOutKey().ToList();

            for (var i = 0; i < columns.Count; i++)
            {
                var column = columns[i];

                output.AppendFormat("{0}{1} = {2}", Indent(2), GetObjectName(column), GetParameterName(column));

                if (i < columns.Count - 1)
                {
                    output.Append(",");
                }

                output.AppendLine();
            }

            output.AppendFormat("{0}where", Indent(1));
            output.AppendLine();

            if (Table.PrimaryKey != null)
            {
                for (var i = 0; i < Table.PrimaryKey.Key.Count; i++)
                {
                    var item = Table.PrimaryKey.Key[i];

                    output.AppendFormat("{0}{1} = {2}", Indent(2), GetObjectName(item), GetParameterName(item));

                    if (i < Table.PrimaryKey.Key.Count - 1)
                    {
                        output.Append(",");
                    }

                    output.AppendLine();
                }
            }

            output.AppendFormat("go");
            output.AppendLine();

            return output.ToString();
        }

        protected virtual String DeleteProcedure(StringBuilder output)
        {
            var procedureName = GetProcedureName("Delete");

            DropProcedure(output, procedureName);

            output.AppendLine();

            output.AppendFormat("create procedure {0}", procedureName);
            output.AppendLine();

            if (Table.PrimaryKey != null)
            {
                for (var i = 0; i < Table.PrimaryKey.Key.Count; i++)
                {
                    var key = Table.PrimaryKey.Key[i];

                    var column = Table.Columns.FirstOrDefault(x => x.Name == key);

                    output.AppendFormat("{0}{1} {2}", Indent(1), GetParameterName(key), GetType(column));
                }
            }

            output.AppendLine();

            output.AppendFormat("as");
            output.AppendLine();

            output.AppendFormat("{0}delete from", Indent(1));
            output.AppendLine();

            output.AppendFormat("{0}{1}", Indent(2), GetObjectName(Table));
            output.AppendLine();

            output.AppendFormat("{0}where", Indent(1));
            output.AppendLine();

            if (Table.PrimaryKey != null)
            {
                for (var i = 0; i < Table.PrimaryKey.Key.Count; i++)
                {
                    var item = Table.PrimaryKey.Key[i];

                    output.AppendFormat("{0}{1} = {2}", Indent(2), GetObjectName(item), GetParameterName(item));

                    if (i < Table.PrimaryKey.Key.Count - 1)
                    {
                        output.Append(",");
                    }

                    output.AppendLine();
                }
            }

            output.AppendFormat("go");
            output.AppendLine();

            return output.ToString();
        }
    }
}
