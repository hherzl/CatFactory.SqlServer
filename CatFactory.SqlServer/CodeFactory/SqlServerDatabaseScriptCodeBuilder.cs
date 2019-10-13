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
        /// Creates a new script for database
        /// </summary>
        /// <param name="database">Instance of <see cref="Database"/> instance</param>
        /// <param name="outputDirectory">Output directory</param>
        /// <param name="forceOverwrite">Force overwrite</param>
        public static void CreateScript(Database database, string outputDirectory, bool forceOverwrite = false)
        {
            var codeBuilder = new SqlServerDatabaseScriptCodeBuilder
            {
                Database = database,
                OutputDirectory = outputDirectory,
                ForceOverwrite = forceOverwrite
            };

            codeBuilder.CreateFile();
        }

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
            Lines = new List<ILine>();

            Lines.AddRange(AddDatabaseCreation());

            Lines.AddRange(AddDatabaseExtendedProperties());

            Lines.AddRange(AddDatabaseSchemas());

            foreach (var table in Database.Tables)
            {
                Lines.AddRange(AddTable(table));

                Lines.AddRange(AddConstraints(table));
            }
        }

        /// <summary>
        /// Gets code lines for database creation
        /// </summary>
        /// <returns>A sequence of <see cref="ILine"/> that represents the database creation</returns>
        protected virtual IEnumerable<ILine> AddDatabaseCreation()
        {
            yield return new CodeLine("create database {0}", Database.GetObjectName(Database.Name));

            yield return new CodeLine("go");

            yield return new EmptyLine();

            yield return new CodeLine("use {0}", Database.GetObjectName(Database.Name));

            yield return new CodeLine("go");

            yield return new EmptyLine();
        }

        /// <summary>
        /// Gets code lines for database extended properties
        /// </summary>
        /// <returns>A sequence of <see cref="ILine"/> that represents the database creation</returns>
        protected virtual IEnumerable<ILine> AddDatabaseExtendedProperties()
        {
            if (Database.ExtendedProperties.Count > 0)
            {
                foreach (var extendedProperty in Database.ExtendedProperties)
                {
                    yield return new CodeLine("exec [sp_addextendedproperty]");
                    yield return new CodeLine("{0}@name = '{1}',", Indent(1), extendedProperty.Name);
                    yield return new CodeLine("{0}@value = '{1}',", Indent(1), extendedProperty.Value);
                    yield return new CodeLine("{0}@level0type = null,", Indent(1));
                    yield return new CodeLine("{0}@level0name = null,", Indent(1));
                    yield return new CodeLine("{0}@level1type = null,", Indent(1));
                    yield return new CodeLine("{0}@level1name = null,", Indent(1));
                    yield return new CodeLine("{0}@level2type = null,", Indent(1));
                    yield return new CodeLine("{0}@level2name = null", Indent(1));
                }

                yield return new CodeLine("go");

                yield return new EmptyLine();
            }
        }

        /// <summary>
        /// Gets code lines for database schemas
        /// </summary>
        /// <returns>A sequence of <see cref="ILine"/> that represents the database creation</returns>
        protected virtual IEnumerable<ILine> AddDatabaseSchemas()
        {
            var schemas = Database.Tables.Select(item => item.Schema).Distinct().ToList();

            foreach (var schema in schemas)
            {
                if (string.IsNullOrEmpty(schema) || Database.DefaultSchema == schema)
                    continue;

                yield return new CodeLine("create schema {0}", Database.GetObjectName(schema));

                yield return new EmptyLine();

                yield return new CodeLine("go");

                yield return new EmptyLine();
            }
        }

        /// <summary>
        /// Gets code lines for table creation
        /// </summary>
        /// <param name="table">Instance of <see cref="Table"/></param>
        /// <returns>A sequence of <see cref="ILine"/> that represents the table creation</returns>
        protected virtual IEnumerable<ILine> AddTable(Table table)
        {
            yield return new CodeLine("create table {0}", Database.GetObjectName(table));

            yield return new CodeLine("(");

            for (var i = 0; i < table.Columns.Count; i++)
            {
                var column = table.Columns[i];

                var output = new StringBuilder();

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
                    else
                        output.AppendFormat("{0}{1} {2}", Indent(1), Database.GetObjectName(column), column.Type);
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

                yield return new CodeLine(output.ToString());
            }

            yield return new CodeLine(")");

            yield return new CodeLine("go");

            yield return new EmptyLine();

            foreach (var extendedProperty in table.ExtendedProperties)
            {
                yield return new CodeLine("exec [sp_addextendedproperty]");
                yield return new CodeLine("{0}@name = '{1}',", Indent(1), extendedProperty.Name);
                yield return new CodeLine("{0}@value = '{1}',", Indent(1), extendedProperty.Value);
                yield return new CodeLine("{0}@level0type = '{1}',", Indent(1), "schema");
                yield return new CodeLine("{0}@level0name = '{1}', ", Indent(1), table.Schema);
                yield return new CodeLine("{0}@level1type = '{1}',", Indent(1), "table");
                yield return new CodeLine("{0}@level1name = '{1}',", Indent(1), table.Name);
                yield return new CodeLine("{0}@level2type = null,", Indent(1));
                yield return new CodeLine("{0}@level2name = null", Indent(1));
                yield return new CodeLine("go");
            }

            var columnsWithExtendedProperties = table.Columns.Where(item => item.ExtendedProperties.Count > 0).ToList();

            if (columnsWithExtendedProperties.Count > 0)
            {
                for (var i = 0; i < columnsWithExtendedProperties.Count; i++)
                {
                    var column = columnsWithExtendedProperties[i];

                    foreach (var extendedProperty in column.ExtendedProperties)
                    {
                        yield return new CodeLine("exec [sp_addextendedproperty]");
                        yield return new CodeLine("{0}@name = '{1}',", Indent(1), extendedProperty.Name);
                        yield return new CodeLine("{0}@value = '{1}',", Indent(1), extendedProperty.Value);
                        yield return new CodeLine("{0}@level0type = '{1}',", Indent(1), "schema");
                        yield return new CodeLine("{0}@level0name = '{1}',", Indent(1), table.Schema);
                        yield return new CodeLine("{0}@level1type = '{1}',", Indent(1), "table");
                        yield return new CodeLine("{0}@level1name = '{1}',", Indent(1), table.Name);
                        yield return new CodeLine("{0}@level2type = '{1}',", Indent(1), "column");
                        yield return new CodeLine("{0}@level2name = '{1}'", Indent(1), column.Name);
                        yield return new CodeLine("go");
                        yield return new EmptyLine();
                    }
                }
            }
        }

        /// <summary>
        /// Gets code lines for table constraints
        /// </summary>
        /// <param name="table">Instance of <see cref="Table"/></param>
        /// <returns>A sequence of <see cref="ILine"/> that represents the table constraints</returns>
        protected virtual IEnumerable<ILine> AddConstraints(Table table)
        {
            var nm = Database.NamingConvention;

            if (table.PrimaryKey != null)
            {
                var pk = table.PrimaryKey;

                var constraintName = string.IsNullOrEmpty(pk.ConstraintName) ? nm.GetPrimaryKeyConstraintName(table, pk.Key.ToArray()) : Database.GetObjectName(pk.ConstraintName);

                yield return new CodeLine("alter table {0} add constraint {1}", Database.GetObjectName(table), constraintName);

                yield return new CodeLine("{0}primary key ({1})", Indent(1), string.Join(", ", pk.Key.Select(item => Database.GetObjectName(item))));

                yield return new CodeLine("go");

                yield return new EmptyLine();
            }

            foreach (var unique in table.Uniques)
            {
                var constraintName = string.IsNullOrEmpty(unique.ConstraintName) ? nm.GetUniqueConstraintName(table, unique.Key.ToArray()) : Database.GetObjectName(unique.ConstraintName);

                yield return new CodeLine("alter table {0} add constraint {1}", Database.GetObjectName(table), constraintName);

                yield return new CodeLine("{0}unique ({1})", Indent(1), string.Join(", ", unique.Key.Select(item => Database.GetObjectName(item))));

                yield return new CodeLine("go");

                yield return new EmptyLine();
            }

            for (var i = 0; i < table.ForeignKeys.Count; i++)
            {
                var foreignKey = table.ForeignKeys[i];

                var references = Database.FindTable(foreignKey.References);

                if (references == null)
                {
                    yield return new CodeLine("/* There is not a table with name: {0} */", foreignKey.References);
                }
                else
                {
                    if (references.PrimaryKey != null)
                    {
                        var constraintName = string.IsNullOrEmpty(foreignKey.ConstraintName) ? nm.GetForeignKeyConstraintName(table, foreignKey.Key.ToArray(), references) : Database.GetObjectName(foreignKey.ConstraintName);

                        yield return new CodeLine("alter table {0} add constraint {1}", Database.GetObjectName(table), constraintName);

                        yield return new CodeLine("{0}foreign key ({1}) references {2}", Indent(1), string.Join(", ", foreignKey.Key.Select(item => Database.GetObjectName(item))), Database.GetObjectName(references));

                        yield return new CodeLine("go");

                        yield return new EmptyLine();
                    }
                }
            }
        }

        /// <summary>
        /// Gets the output code for current <see cref="SqlServerDatabaseScriptCodeBuilder"/> instance
        /// </summary>
        protected string Code
            => string.Empty;
    }
}
