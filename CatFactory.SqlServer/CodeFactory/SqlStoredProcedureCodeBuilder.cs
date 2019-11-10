using System.Collections.Generic;
using System.Linq;
using CatFactory.CodeFactory;
using CatFactory.ObjectRelationalMapping;

namespace CatFactory.SqlServer.CodeFactory
{
    /// <summary>
    /// Represents a code builder for stored procedures
    /// </summary>
    public class SqlStoredProcedureCodeBuilder : CodeBuilder
    {
        /// <summary>
        /// Initializes a new instance of <see cref="SqlStoredProcedureCodeBuilder"/> class
        /// </summary>
        public SqlStoredProcedureCodeBuilder()
            : base()
        {
        }

        /// <summary>
        /// Gets the file name
        /// </summary>
        public override string FileName
            => Table.FullName;

        /// <summary>
        /// Gets the file extension
        /// </summary>
        public override string FileExtension
            => "sql";

        /// <summary>
        /// Gets the database associated with current <see cref="SqlStoredProcedureCodeBuilder"/> instance
        /// </summary>
        public Database Database { get; set; }

        /// <summary>
        /// Gets the table associated with current <see cref="SqlStoredProcedureCodeBuilder"/> instance
        /// </summary>
        public ITable Table { get; set; }

        /// <summary>
        /// Gets an string that represents database type for column
        /// </summary>
        /// <param name="column">Instance of <see cref="Column"/> class</param>
        /// <returns></returns>
        protected virtual string GetType(IColumn column)
        {
            var databaseTypes = Database.DatabaseTypeMaps;

            if (Database.ColumnIsDecimal(column))
                return string.Format("{0}({1}, {2})", column.Type, column.Prec, column.Scale);
            else if (Database.ColumnIsString(column))
                return column.Length == 0 ? string.Format("{0}(max)", column.Type) : string.Format("{0}({1})", column.Type, column.Length);
            else
                return string.Format("{0}", column.Type);
        }

        /// <summary>
        /// Translates object definition to a sequence of <see cref="ILine"/> interface
        /// </summary>
        public override void Translating()
        {
            Lines = new List<ILine>();

            Lines.AddRange(GetLinesForGetAllStoredProcedure());

            Lines.Add(new EmptyLine());

            Lines.AddRange(GetLinesForGetStoredProcedure());

            Lines.Add(new EmptyLine());

            Lines.AddRange(GetLinesForInsertIntoStoredProcedure());

            Lines.Add(new EmptyLine());

            Lines.AddRange(GetDropProcedureLines(Database.GetProcedureName(Table, "Update")));

            Lines.Add(new EmptyLine());

            Lines.AddRange(GetLinesForUpdateStoredProcedure());

            Lines.Add(new EmptyLine());

            Lines.AddRange(GetDropProcedureLines(Database.GetProcedureName(Table, "Delete")));

            Lines.Add(new EmptyLine());

            Lines.AddRange(GetLinesForDeleteStoredProcedure());
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="procedureName"></param>
        /// <returns></returns>
        protected IEnumerable<ILine> GetDropProcedureLines(string procedureName)
        {
            yield return new CodeLine("if object_id('{0}', 'P') is not null", procedureName);

            yield return new CodeLine("{0}drop procedure {1}", Indent(1), procedureName);

            yield return new CodeLine("go");
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        protected virtual IEnumerable<ILine> GetLinesForGetAllStoredProcedure()
        {
            var procedureName = Database.GetProcedureName(Table, "GetAll");

            Lines.AddRange(GetDropProcedureLines(procedureName).ToList());

            Lines.Add(new EmptyLine());

            yield return new CodeLine("create procedure {0}", procedureName);

            var constraints = Table.ForeignKeys.Where(constraint => constraint.Key != null && constraint.Key.Count == 1).ToList();

            for (var i = 0; i < constraints.Count; i++)
            {
                var foreignKey = constraints[i];
                var columns = Table.GetColumnsFromConstraint(foreignKey).ToList();

                if (columns.Count == 1)
                    yield return new CodeLine("{0}{1} {2} = null{3}", Indent(1), Database.GetParameterName(columns.First()), columns.First().Type, i < constraints.Count - 1 ? "," : string.Empty);
            }

            yield return new CodeLine("as");

            yield return new CodeLine("{0}select", Indent(1));

            for (var i = 0; i < Table.Columns.Count; i++)
            {
                var column = Table.Columns[i];

                yield return new CodeLine("{0}{1}{2}", Indent(2), Database.GetObjectName(column), i < Table.Columns.Count - 1 ? "," : string.Empty);
            }

            yield return new CodeLine("{0}from", Indent(1));

            yield return new CodeLine("{0}{1}", Indent(2), Database.GetObjectName(Table));

            if (constraints.Count > 0)
            {
                yield return new CodeLine("{0}where", Indent(1));

                for (var i = 0; i < constraints.Count; i++)
                {
                    var foreignKey = constraints[i];
                    var columns = Table.GetColumnsFromConstraint(foreignKey).ToList();

                    if (columns.Count == 1)
                        yield return new CodeLine("{0}({1} is null or {2} = {1}){3}", Indent(2), Database.GetParameterName(columns.First()), Database.GetParameterName(columns.First()), i < constraints.Count - 1 ? " and" : string.Empty);
                }
            }

            yield return new CodeLine("go");
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        protected virtual IEnumerable<ILine> GetLinesForGetStoredProcedure()
        {
            var procedureName = Database.GetProcedureName(Table, "Get");

            Lines.AddRange(GetDropProcedureLines(procedureName).ToList());

            Lines.Add(new EmptyLine());

            yield return new CodeLine("create procedure {0}", procedureName);

            if (Table.PrimaryKey != null)
            {
                for (var i = 0; i < Table.PrimaryKey.Key.Count; i++)
                {
                    var key = Table.PrimaryKey.Key[i];

                    var column = Table.GetColumnsFromConstraint(Table.PrimaryKey).First();

                    yield return new CodeLine("{0}{1} {2}", Indent(1), Database.GetParameterName(key), GetType(column));
                }
            }

            yield return new CodeLine("as");

            yield return new CodeLine("{0}select", Indent(1));

            for (var i = 0; i < Table.Columns.Count; i++)
            {
                var column = Table.Columns[i];

                yield return new CodeLine("{0}{1}{2}", Indent(2), Database.GetObjectName(column), i < Table.Columns.Count - 1 ? "," : string.Empty);
            }

            yield return new CodeLine("{0}from", Indent(1));

            yield return new CodeLine("{0}{1}", Indent(2), Database.GetObjectName(Table));

            yield return new CodeLine("{0}where", Indent(1));

            if (Table.PrimaryKey != null)
            {
                for (var i = 0; i < Table.PrimaryKey.Key.Count; i++)
                {
                    var item = Table.PrimaryKey.Key[i];

                    yield return new CodeLine("{0}{1} = {2}{3}", Indent(2), Database.GetObjectName(item), Database.GetParameterName(item), i < Table.PrimaryKey.Key.Count - 1 ? "," : string.Empty);
                }
            }

            yield return new CodeLine("go");
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        protected virtual IEnumerable<ILine> GetLinesForInsertIntoStoredProcedure()
        {
            var procedureName = Database.GetProcedureName(Table, "Add");

            Lines.AddRange(GetDropProcedureLines(procedureName));

            Lines.Add(new EmptyLine());

            yield return new CodeLine("create procedure {0}", procedureName);

            for (var i = 0; i < Table.Columns.Count; i++)
            {
                var column = Table.Columns[i];

                yield return new CodeLine("{0}{1} {2} {3}{4}", Indent(1), Database.GetParameterName(column), GetType(column), Table.Identity != null && Table.Identity.Name == column.Name ? " output" : string.Empty, i < Table.Columns.Count - 1 ? "," : string.Empty);
            }

            yield return new CodeLine("as");

            yield return new CodeLine("{0}insert into {1}", Indent(1), Database.GetObjectName(Table));

            yield return new CodeLine("{0}(", Indent(1));

            var columns = Table.GetColumnsWithNoIdentity().ToList();

            for (var i = 0; i < columns.Count; i++)
            {
                var column = columns[i];

                yield return new CodeLine("{0}{1}{2}", Indent(2), Database.GetObjectName(column), i < columns.Count - 1 ? "," : string.Empty);
            }

            yield return new CodeLine("{0})", Indent(1));

            yield return new CodeLine("{0}values", Indent(1));

            yield return new CodeLine("{0}(", Indent(1));

            for (var i = 0; i < columns.Count; i++)
            {
                var column = columns[i];

                yield return new CodeLine("{0}{1}{2}", Indent(2), Database.GetParameterName(column), i < columns.Count - 1 ? "," : string.Empty);
            }

            yield return new CodeLine("{0})", Indent(1));

            if (Table.Identity != null)
            {
                yield return new EmptyLine();

                yield return new CodeLine("{0}select {1} = scope_identity()", Indent(1), Database.GetParameterName(Table.Identity.Name));
            }

            yield return new CodeLine("go");
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        protected virtual IEnumerable<ILine> GetLinesForUpdateStoredProcedure()
        {
            var procedureName = Database.GetProcedureName(Table, "Update");

            Lines.AddRange(GetDropProcedureLines(procedureName));

            Lines.Add(new EmptyLine());

            yield return new CodeLine("create procedure {0}", procedureName);

            for (var i = 0; i < Table.Columns.Count; i++)
            {
                var column = Table.Columns[i];

                yield return new CodeLine("{0}{1} {2}{3}", Indent(1), Database.GetParameterName(column), GetType(column), i < Table.Columns.Count - 1 ? "," : string.Empty);
            }

            yield return new CodeLine("as");

            yield return new CodeLine("{0}update", Indent(1));

            yield return new CodeLine("{0}{1}", Indent(2), Database.GetObjectName(Table));

            yield return new CodeLine("{0}set", Indent(1));

            var columns = Table.GetColumnsFromConstraint(Table.PrimaryKey).ToList();

            for (var i = 0; i < columns.Count; i++)
            {
                var column = columns[i];

                yield return new CodeLine("{0}{1} = {2}{3}", Indent(2), Database.GetObjectName(column), Database.GetParameterName(column), i < columns.Count - 1 ? "," : string.Empty);
            }

            yield return new CodeLine("{0}where", Indent(1));

            if (Table.PrimaryKey != null)
            {
                for (var i = 0; i < Table.PrimaryKey.Key.Count; i++)
                {
                    var item = Table.PrimaryKey.Key[i];

                    yield return new CodeLine("{0}{1} = {2}{3}", Indent(2), Database.GetObjectName(item), Database.GetParameterName(item), i < Table.PrimaryKey.Key.Count - 1 ? "," : string.Empty);
                }
            }

            yield return new CodeLine("go");
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        protected virtual IEnumerable<ILine> GetLinesForDeleteStoredProcedure()
        {
            var procedureName = Database.GetProcedureName(Table, "Delete");

            Lines.AddRange(GetDropProcedureLines(procedureName));

            Lines.Add(new EmptyLine());

            yield return new CodeLine("create procedure {0}", procedureName);

            if (Table.PrimaryKey != null)
            {
                for (var i = 0; i < Table.PrimaryKey.Key.Count; i++)
                {
                    var key = Table.PrimaryKey.Key[i];

                    var column = Table.GetColumnsFromConstraint(Table.PrimaryKey).First();

                    yield return new CodeLine("{0}{1} {2}", Indent(1), Database.GetParameterName(key), GetType(column));
                }
            }

            yield return new CodeLine("as");

            yield return new CodeLine("{0}delete from", Indent(1));

            yield return new CodeLine("{0}{1}", Indent(2), Database.GetObjectName(Table));

            yield return new CodeLine("{0}where", Indent(1));

            if (Table.PrimaryKey != null)
            {
                for (var i = 0; i < Table.PrimaryKey.Key.Count; i++)
                {
                    var item = Table.PrimaryKey.Key[i];

                    yield return new CodeLine("{0}{1} = {2}{3}", Indent(2), Database.GetObjectName(item), Database.GetParameterName(item), i < Table.PrimaryKey.Key.Count - 1 ? "," : string.Empty);
                }
            }

            yield return new CodeLine("go");
        }
    }
}
