using System.Collections.Generic;
using System.Linq;
using System.Text;
using CatFactory.CodeFactory;
using CatFactory.Mapping;

namespace CatFactory.SqlServer
{
    /// <summary>
    /// 
    /// </summary>
    public class SqlStoredProcedureCodeBuilder : CodeBuilder
    {
        private List<DatabaseTypeMap> Mappings;

        /// <summary>
        /// 
        /// </summary>
        public SqlStoredProcedureCodeBuilder()
        {
            Mappings = DatabaseTypeMapList.Definition;
        }

        /// <summary>
        /// 
        /// </summary>
        public Database Database { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public ITable Table { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public override string FileName
            => Table.FullName;

        /// <summary>
        /// 
        /// </summary>
        public override string FileExtension
            => "sql";

        /// <summary>
        /// 
        /// </summary>
        /// <param name="column"></param>
        /// <returns></returns>
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

        /// <summary>
        /// 
        /// </summary>
        public override void Translating()
        {
            Lines = new List<ILine>
            {
                new CodeLine(Code)
            };
        }

        /// <summary>
        /// 
        /// </summary>
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

        /// <summary>
        /// 
        /// </summary>
        /// <param name="output"></param>
        /// <param name="procedureName"></param>
        protected virtual void DropProcedure(StringBuilder output, string procedureName)
        {
            output.AppendFormat("if object_id('{0}', 'P') is not null", procedureName.Replace("[", string.Empty).Replace("]", string.Empty));
            output.AppendLine();

            output.AppendFormat("{0}drop procedure {1}", Indent(1), procedureName);
            output.AppendLine();

            output.AppendFormat("go");
            output.AppendLine();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="output"></param>
        protected virtual void GetAllProcedure(StringBuilder output)
        {
            var procedureName = Database.GetProcedureName(Table, "GetAll");

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
                    output.AppendFormat("{0}{1} {2} = null", Indent(1), Database.GetParameterName(columns.First()), columns.First().Type);

                    if (i < constraints.Count - 1)
                        output.Append(",");

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

                output.AppendFormat("{0}{1}", Indent(2), Database.GetObjectName(column));

                if (i < Table.Columns.Count - 1)
                    output.Append(",");

                output.AppendLine();
            }

            output.AppendFormat("{0}from", Indent(1));
            output.AppendLine();

            output.AppendFormat("{0}{1}", Indent(2), Database.GetObjectName(Table));
            output.AppendLine();

            if (constraints.Count > 0)
            {
                output.AppendFormat("{0}where", Indent(1));
                output.AppendLine();

                for (var i = 0; i < constraints.Count; i++)
                {
                    var foreignKey = constraints[i];
                    var columns = Table.GetColumnsFromConstraint(foreignKey).ToList();

                    if (columns.Count == 1)
                    {
                        output.AppendFormat("{0}({1} is null or {2} = {1})", Indent(2), Database.GetParameterName(columns.First()), Database.GetParameterName(columns.First()));

                        if (i < constraints.Count - 1)
                            output.Append(" and");

                        output.AppendLine();
                    }
                }
            }

            output.AppendFormat("go");
            output.AppendLine();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="output"></param>
        protected virtual void GetProcedure(StringBuilder output)
        {
            var procedureName = Database.GetProcedureName(Table, "Get");

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

                    output.AppendFormat("{0}{1} {2}", Indent(1), Database.GetParameterName(key), GetType(column));
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

                output.AppendFormat("{0}{1}", Indent(2), Database.GetObjectName(column));

                if (i < Table.Columns.Count - 1)
                    output.Append(",");

                output.AppendLine();
            }

            output.AppendFormat("{0}from", Indent(1));
            output.AppendLine();

            output.AppendFormat("{0}{1}", Indent(2), Database.GetObjectName(Table));
            output.AppendLine();

            output.AppendFormat("{0}where", Indent(1));
            output.AppendLine();

            if (Table.PrimaryKey != null)
            {
                for (var i = 0; i < Table.PrimaryKey.Key.Count; i++)
                {
                    var item = Table.PrimaryKey.Key[i];

                    output.AppendFormat("{0}{1} = {2}", Indent(2), Database.GetObjectName(item), Database.GetParameterName(item));

                    if (i < Table.PrimaryKey.Key.Count - 1)
                        output.Append(",");

                    output.AppendLine();
                }
            }

            output.AppendFormat("go");
            output.AppendLine();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="output"></param>
        /// <returns></returns>
        protected virtual string InsertProcedure(StringBuilder output)
        {
            var procedureName = Database.GetProcedureName(Table, "Add");

            DropProcedure(output, procedureName);

            output.AppendLine();

            output.AppendFormat("create procedure {0}", procedureName);
            output.AppendLine();

            for (var i = 0; i < Table.Columns.Count; i++)
            {
                var column = Table.Columns[i];

                output.AppendFormat("{0}{1} {2}", Indent(1), Database.GetParameterName(column), GetType(column));

                if (Table.Identity != null && Table.Identity.Name == column.Name)
                    output.AppendFormat(" output");

                if (i < Table.Columns.Count - 1)
                    output.Append(",");

                output.AppendLine();
            }

            output.AppendFormat("as");
            output.AppendLine();

            output.AppendFormat("{0}insert into {1}", Indent(1), Database.GetObjectName(Table));
            output.AppendLine();

            output.AppendFormat("{0}(", Indent(1));
            output.AppendLine();

            var columns = Table.GetColumnsWithNoIdentity().ToList();

            for (var i = 0; i < columns.Count; i++)
            {
                var column = columns[i];

                output.AppendFormat("{0}{1}", Indent(2), Database.GetObjectName(column));

                if (i < columns.Count - 1)
                    output.Append(",");

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

                output.AppendFormat("{0}{1}", Indent(2), Database.GetParameterName(column));

                if (i < columns.Count - 1)
                    output.Append(",");

                output.AppendLine();
            }

            output.AppendFormat("{0})", Indent(1));
            output.AppendLine();

            if (Table.Identity != null)
            {
                output.AppendLine();

                output.AppendFormat("{0}select {1} = @@identity", Indent(1), Database.GetParameterName(Table.Identity.Name));
                output.AppendLine();
            }

            output.AppendFormat("go");
            output.AppendLine();

            return output.ToString();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="output"></param>
        /// <returns></returns>
        protected virtual string UpdateProcedure(StringBuilder output)
        {
            var procedureName = Database.GetProcedureName(Table, "Update");

            DropProcedure(output, procedureName);

            output.AppendLine();

            output.AppendFormat("create procedure {0}", procedureName);
            output.AppendLine();

            for (var i = 0; i < Table.Columns.Count; i++)
            {
                var column = Table.Columns[i];

                output.AppendFormat("{0}{1} {2}", Indent(1), Database.GetParameterName(column), GetType(column));

                if (i < Table.Columns.Count - 1)
                    output.Append(",");

                output.AppendLine();
            }

            output.AppendFormat("as");
            output.AppendLine();

            output.AppendFormat("{0}update", Indent(1));
            output.AppendLine();

            output.AppendFormat("{0}{1}", Indent(2), Database.GetObjectName(Table));
            output.AppendLine();

            output.AppendFormat("{0}set", Indent(1));
            output.AppendLine();

            var columns = Table.GetColumnsFromConstraint(Table.PrimaryKey).ToList();

            for (var i = 0; i < columns.Count; i++)
            {
                var column = columns[i];

                output.AppendFormat("{0}{1} = {2}", Indent(2), Database.GetObjectName(column), Database.GetParameterName(column));

                if (i < columns.Count - 1)
                    output.Append(",");

                output.AppendLine();
            }

            output.AppendFormat("{0}where", Indent(1));
            output.AppendLine();

            if (Table.PrimaryKey != null)
            {
                for (var i = 0; i < Table.PrimaryKey.Key.Count; i++)
                {
                    var item = Table.PrimaryKey.Key[i];

                    output.AppendFormat("{0}{1} = {2}", Indent(2), Database.GetObjectName(item), Database.GetParameterName(item));

                    if (i < Table.PrimaryKey.Key.Count - 1)
                        output.Append(",");

                    output.AppendLine();
                }
            }

            output.AppendFormat("go");
            output.AppendLine();

            return output.ToString();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="output"></param>
        /// <returns></returns>
        protected virtual string DeleteProcedure(StringBuilder output)
        {
            var procedureName = Database.GetProcedureName(Table, "Delete");

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

                    output.AppendFormat("{0}{1} {2}", Indent(1), Database.GetParameterName(key), GetType(column));
                }
            }

            output.AppendLine();

            output.AppendFormat("as");
            output.AppendLine();

            output.AppendFormat("{0}delete from", Indent(1));
            output.AppendLine();

            output.AppendFormat("{0}{1}", Indent(2), Database.GetObjectName(Table));
            output.AppendLine();

            output.AppendFormat("{0}where", Indent(1));
            output.AppendLine();

            if (Table.PrimaryKey != null)
            {
                for (var i = 0; i < Table.PrimaryKey.Key.Count; i++)
                {
                    var item = Table.PrimaryKey.Key[i];

                    output.AppendFormat("{0}{1} = {2}", Indent(2), Database.GetObjectName(item), Database.GetParameterName(item));

                    if (i < Table.PrimaryKey.Key.Count - 1)
                        output.Append(",");

                    output.AppendLine();
                }
            }

            output.AppendFormat("go");
            output.AppendLine();

            return output.ToString();
        }
    }
}
