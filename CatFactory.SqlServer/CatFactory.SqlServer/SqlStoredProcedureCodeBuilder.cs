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

        public ITable Table { get; set; }

        public override string FileName
            => Table.FullName;

        public override string FileExtension
            => "sql";

        public virtual string GetType(Column column)
        {
            switch (column.Type)
            {
                case "char":
                case "varchar":
                case "nvarchar":
                    return column.Length == 0 ? string.Format("{0}(max)", column.Type) : string.Format("{0}({1})", column.Type, column.Length);

                case "decimal":
                    return string.Format("{0}({1}, {2})", column.Type, column.Prec, column.Scale);

                default:
                    return string.Format("{0}", column.Type);
            }
        }

        public string Code
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

        protected virtual void DropProcedure(StringBuilder output, string procedureName)
        {
            output.AppendFormat("if object_id('{0}', 'P') is not null", procedureName.Replace("[", string.Empty).Replace("]", string.Empty));
            output.AppendLine();

            output.AppendFormat("{0}drop procedure {1}", Indent(1), procedureName);
            output.AppendLine();

            output.AppendFormat("go");
            output.AppendLine();
        }

        protected virtual void GetAllProcedure(StringBuilder output)
        {
            var procedureName = Table.GetProcedureName("GetAll");

            DropProcedure(output, procedureName);

            output.AppendLine();

            output.AppendFormat("create procedure {0}", procedureName);
            output.AppendLine();

            var constraints = Table.ForeignKeys.Where(constraint => constraint.Key != null && constraint.Key.Count == 1).ToList();

            for (var i = 0; i < constraints.Count; i++)
            {
                var foreignKey = constraints[i];
                var columns = Table.GetColumnsFromConstraint(foreignKey).ToList();

                if (columns.Count == 1)
                {
                    output.AppendFormat("{0}{1} {2} = null", Indent(1), columns.First().GetParameterName(), columns.First().Type);

                    if (i < constraints.Count - 1)
                    {
                        output.Append(",");
                    }

                    output.AppendLine();
                }
            }

            output.AppendFormat("as");
            output.AppendLine();

            output.AppendFormat("{0}select", Indent(1));
            output.AppendLine();

            for (var i = 0; i < Table.Columns.Count; i++)
            {
                var column = Table.Columns[i];

                output.AppendFormat("{0}{1}", Indent(2), column.GetObjectName());

                if (i < Table.Columns.Count - 1)
                {
                    output.Append(",");
                }

                output.AppendLine();
            }

            output.AppendFormat("{0}from", Indent(1));
            output.AppendLine();

            output.AppendFormat("{0}{1}", Indent(2), Table.GetObjectName());
            output.AppendLine();

            if (constraints .Count > 0)
            {
                output.AppendFormat("{0}where", Indent(1));
                output.AppendLine();

                for (var i = 0; i < constraints.Count; i++)
                {
                    var foreignKey = constraints[i];
                    var columns = Table.GetColumnsFromConstraint(foreignKey).ToList();

                    if (columns.Count == 1)
                    {
                        output.AppendFormat("{0}({1} is null or {2} = {1})", Indent(2), columns.First().GetParameterName(), columns.First().GetObjectName());

                        if (i < constraints.Count - 1)
                        {
                            output.Append(" and");
                        }

                        output.AppendLine();
                    }
                }
            }

            output.AppendFormat("go");
            output.AppendLine();
        }

        protected virtual void GetProcedure(StringBuilder output)
        {
            var procedureName = Table.GetProcedureName("Get");

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

                    output.AppendFormat("{0}{1} {2}", Indent(1), key.GetParameterName(), GetType(column));
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

                output.AppendFormat("{0}{1}", Indent(2), column.GetObjectName());

                if (i < Table.Columns.Count - 1)
                {
                    output.Append(",");
                }

                output.AppendLine();
            }

            output.AppendFormat("{0}from", Indent(1));
            output.AppendLine();

            output.AppendFormat("{0}{1}", Indent(2), Table.GetObjectName());
            output.AppendLine();

            output.AppendFormat("{0}where", Indent(1));
            output.AppendLine();

            if (Table.PrimaryKey != null)
            {
                for (var i = 0; i < Table.PrimaryKey.Key.Count; i++)
                {
                    var item = Table.PrimaryKey.Key[i];

                    output.AppendFormat("{0}{1} = {2}", Indent(2), item.GetObjectName(), item.GetParameterName());

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

        protected virtual string InsertProcedure(StringBuilder output)
        {
            var procedureName = Table.GetProcedureName("Add");

            DropProcedure(output, procedureName);

            output.AppendLine();

            output.AppendFormat("create procedure {0}", procedureName);
            output.AppendLine();

            for (var i = 0; i < Table.Columns.Count; i++)
            {
                var column = Table.Columns[i];

                output.AppendFormat("{0}{1} {2}", Indent(1), column.GetParameterName(), GetType(column));

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

            output.AppendFormat("{0}insert into {1}", Indent(1), Table.GetObjectName());
            output.AppendLine();

            output.AppendFormat("{0}(", Indent(1));
            output.AppendLine();

            var columns = Table.GetColumnsWithNoIdentity().ToList();

            for (var i = 0; i < columns.Count; i++)
            {
                var column = columns[i];

                output.AppendFormat("{0}{1}", Indent(2), column.GetObjectName());

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

                output.AppendFormat("{0}{1}", Indent(2), column.GetParameterName());

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

                output.AppendFormat("{0}select {1} = @@identity", Indent(1), Table.Identity.Name.GetParameterName());
                output.AppendLine();
            }

            output.AppendFormat("go");
            output.AppendLine();

            return output.ToString();
        }

        protected virtual string UpdateProcedure(StringBuilder output)
        {
            var procedureName = Table.GetProcedureName("Update");

            DropProcedure(output, procedureName);

            output.AppendLine();

            output.AppendFormat("create procedure {0}", procedureName);
            output.AppendLine();

            for (var i = 0; i < Table.Columns.Count; i++)
            {
                var column = Table.Columns[i];

                output.AppendFormat("{0}{1} {2}", Indent(1), column.GetParameterName(), GetType(column));

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

            output.AppendFormat("{0}{1}", Indent(2), Table.GetObjectName());
            output.AppendLine();

            output.AppendFormat("{0}set", Indent(1));
            output.AppendLine();

            var columns = Table.GetColumnsFromConstraint(Table.PrimaryKey).ToList();

            for (var i = 0; i < columns.Count; i++)
            {
                var column = columns[i];

                output.AppendFormat("{0}{1} = {2}", Indent(2), column.GetObjectName(), column.GetParameterName());

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

                    output.AppendFormat("{0}{1} = {2}", Indent(2), item.GetObjectName(), item.GetParameterName());

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

        protected virtual string DeleteProcedure(StringBuilder output)
        {
            var procedureName = Table.GetProcedureName("Delete");

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

                    output.AppendFormat("{0}{1} {2}", Indent(1), key.GetParameterName(), GetType(column));
                }
            }

            output.AppendLine();

            output.AppendFormat("as");
            output.AppendLine();

            output.AppendFormat("{0}delete from", Indent(1));
            output.AppendLine();

            output.AppendFormat("{0}{1}", Indent(2), Table.GetObjectName());
            output.AppendLine();

            output.AppendFormat("{0}where", Indent(1));
            output.AppendLine();

            if (Table.PrimaryKey != null)
            {
                for (var i = 0; i < Table.PrimaryKey.Key.Count; i++)
                {
                    var item = Table.PrimaryKey.Key[i];

                    output.AppendFormat("{0}{1} = {2}", Indent(2), item.GetObjectName(), item.GetParameterName());

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
