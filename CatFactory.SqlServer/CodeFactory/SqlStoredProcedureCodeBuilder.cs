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
            yield return new CodeLine("IF OBJECT_ID('{0}', 'P') IS NOT NULL", procedureName);

            yield return new CodeLine("{0}DROP PROCEDURE {1}", Indent(1), procedureName);

            yield return new CodeLine("GO");
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

            yield return new CodeLine("CREATE PROCEDURE {0}", procedureName);

            var constraints = Table.ForeignKeys.Where(constraint => constraint.Key != null && constraint.Key.Count == 1).ToList();

            for (var i = 0; i < constraints.Count; i++)
            {
                var foreignKey = constraints[i];
                var columns = Table.GetColumnsFromConstraint(foreignKey).ToList();

                if (columns.Count == 1)
                    yield return new CodeLine("{0}{1} {2} = NULL{3}", Indent(1), Database.GetParameterName(columns.First()), columns.First().Type, i < constraints.Count - 1 ? "," : string.Empty);
            }

            yield return new CodeLine("AS");

            yield return new CodeLine("{0}SELECT", Indent(1));

            for (var i = 0; i < Table.Columns.Count; i++)
            {
                var column = Table.Columns[i];

                yield return new CodeLine("{0}{1}{2}", Indent(2), Database.GetObjectName(column), i < Table.Columns.Count - 1 ? "," : string.Empty);
            }

            yield return new CodeLine("{0}FROM", Indent(1));

            yield return new CodeLine("{0}{1}", Indent(2), Database.GetObjectName(Table));

            if (constraints.Count > 0)
            {
                yield return new CodeLine("{0}WHERE", Indent(1));

                for (var i = 0; i < constraints.Count; i++)
                {
                    var foreignKey = constraints[i];
                    var columns = Table.GetColumnsFromConstraint(foreignKey).ToList();

                    if (columns.Count == 1)
                        yield return new CodeLine("{0}({1} IS NULL OR {2} = {1}){3}", Indent(2), Database.GetParameterName(columns.First()), Database.NamingConvention.GetObjectName(columns.First().Name), i < constraints.Count - 1 ? " AND" : string.Empty);
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

            yield return new CodeLine("CREATE PROCEDURE {0}", procedureName);

            if (Table.PrimaryKey != null)
            {
                for (var i = 0; i < Table.PrimaryKey.Key.Count; i++)
                {
                    var key = Table.PrimaryKey.Key[i];

                    var column = Table.GetColumnsFromConstraint(Table.PrimaryKey).First();

                    yield return new CodeLine("{0}{1} {2}", Indent(1), Database.GetParameterName(key), GetType(column));
                }
            }

            yield return new CodeLine("AS");

            yield return new CodeLine("{0}SELECT", Indent(1));

            for (var i = 0; i < Table.Columns.Count; i++)
            {
                var column = Table.Columns[i];

                yield return new CodeLine("{0}{1}{2}", Indent(2), Database.GetObjectName(column), i < Table.Columns.Count - 1 ? "," : string.Empty);
            }

            yield return new CodeLine("{0}FROM", Indent(1));

            yield return new CodeLine("{0}{1}", Indent(2), Database.GetObjectName(Table));

            yield return new CodeLine("{0}WHERE", Indent(1));

            if (Table.PrimaryKey != null)
            {
                for (var i = 0; i < Table.PrimaryKey.Key.Count; i++)
                {
                    var item = Table.PrimaryKey.Key[i];

                    yield return new CodeLine("{0}{1} = {2}{3}", Indent(2), Database.GetObjectName(item), Database.GetParameterName(item), i < Table.PrimaryKey.Key.Count - 1 ? "," : string.Empty);
                }
            }

            yield return new CodeLine("GO");
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

            yield return new CodeLine("CREATE PROCEDURE {0}", procedureName);

            for (var i = 0; i < Table.Columns.Count; i++)
            {
                var column = Table.Columns[i];

                yield return new CodeLine("{0}{1} {2}{3}{4}", Indent(1), Database.GetParameterName(column), GetType(column), Table.Identity != null && Table.Identity.Name == column.Name ? " OUTPUT" : string.Empty, i < Table.Columns.Count - 1 ? "," : string.Empty);
            }

            yield return new CodeLine("AS");

            yield return new CodeLine("{0}INSERT INTO {1}", Indent(1), Database.GetObjectName(Table));

            yield return new CodeLine("{0}(", Indent(1));

            var columns = Table.GetColumnsWithNoIdentity().ToList();

            for (var i = 0; i < columns.Count; i++)
            {
                var column = columns[i];

                yield return new CodeLine("{0}{1}{2}", Indent(2), Database.GetObjectName(column), i < columns.Count - 1 ? "," : string.Empty);
            }

            yield return new CodeLine("{0})", Indent(1));

            yield return new CodeLine("{0}VALUES", Indent(1));

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

                yield return new CodeLine("{0}SELECT {1} = SCOPE_IDENTITY()", Indent(1), Database.GetParameterName(Table.Identity.Name));
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

            yield return new CodeLine("CREATE PROCEDURE {0}", procedureName);

            for (var i = 0; i < Table.Columns.Count; i++)
            {
                var column = Table.Columns[i];

                yield return new CodeLine("{0}{1} {2}{3}", Indent(1), Database.GetParameterName(column), GetType(column), i < Table.Columns.Count - 1 ? "," : string.Empty);
            }

            yield return new CodeLine("AS");

            yield return new CodeLine("{0}UPDATE", Indent(1));

            yield return new CodeLine("{0}{1}", Indent(2), Database.GetObjectName(Table));

            yield return new CodeLine("{0}SET", Indent(1));

            var columns = Table.GetColumnsFromConstraint(Table.PrimaryKey).ToList();

            for (var i = 0; i < columns.Count; i++)
            {
                var column = columns[i];

                yield return new CodeLine("{0}{1} = {2}{3}", Indent(2), Database.GetObjectName(column), Database.GetParameterName(column), i < columns.Count - 1 ? "," : string.Empty);
            }

            yield return new CodeLine("{0}WHERE", Indent(1));

            if (Table.PrimaryKey != null)
            {
                for (var i = 0; i < Table.PrimaryKey.Key.Count; i++)
                {
                    var item = Table.PrimaryKey.Key[i];

                    yield return new CodeLine("{0}{1} = {2}{3}", Indent(2), Database.GetObjectName(item), Database.GetParameterName(item), i < Table.PrimaryKey.Key.Count - 1 ? "," : string.Empty);
                }
            }

            yield return new CodeLine("GO");
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

            yield return new CodeLine("CREATE PROCEDURE {0}", procedureName);

            if (Table.PrimaryKey != null)
            {
                for (var i = 0; i < Table.PrimaryKey.Key.Count; i++)
                {
                    var key = Table.PrimaryKey.Key[i];

                    var column = Table.GetColumnsFromConstraint(Table.PrimaryKey).First();

                    yield return new CodeLine("{0}{1} {2}", Indent(1), Database.GetParameterName(key), GetType(column));
                }
            }

            yield return new CodeLine("AS");

            yield return new CodeLine("{0}DELETE FROM", Indent(1));

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

            yield return new CodeLine("GO");
        }
    }
}
