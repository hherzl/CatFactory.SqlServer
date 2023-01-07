using System.Collections.Generic;
using System.Linq;
using System.Text;
using CatFactory.CodeFactory;
using CatFactory.ObjectRelationalMapping;
using CatFactory.SqlServer.DatabaseObjectModel;

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
        /// <param name="database">Instance of <see cref="SqlServerDatabase"/> class</param>
        /// <param name="outputDirectory">Output directory</param>
        /// <param name="forceOverwrite">Force overwrite</param>
        /// <param name="addDropIfExists">Add drop if exists statement</param>
        public static void CreateScript(SqlServerDatabase database, string outputDirectory, bool forceOverwrite = false, bool addDropIfExists = false)
        {
            var codeBuilder = new SqlServerDatabaseScriptCodeBuilder
            {
                Database = database,
                OutputDirectory = outputDirectory,
                ForceOverwrite = forceOverwrite,
                AddDropIfExists = addDropIfExists
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
        public SqlServerDatabase Database { get; set; }

        /// <summary>
        /// Indicates if script includes [drop] statements in script
        /// </summary>
        public bool AddDropIfExists { get; set; }

        /// <summary>
        /// Translates object definition to a sequence of <see cref="ILine"/> interface
        /// </summary>
        public override void Translating()
        {
            Lines = new List<ILine>();

            Lines.AddRange(AddDatabaseCreation());

            Lines.AddRange(AddDatabaseExtendedProperties());

            Lines.AddRange(AddDatabaseSchemas());

            if (AddDropIfExists)
            {
                for (var i = Database.Tables.Count - 1; i >= 0; i--)
                {
                    var table = Database.Tables[i];

                    Lines.AddRange(DropTableIfExists(table));
                }
            }

            foreach (var table in Database.Tables)
            {
                Lines.AddRange(AddTable(table));
            }

            foreach (var table in Database.Tables)
            {
                Lines.AddRange(AddTableExtendedProperties(table));
            }

            foreach (var table in Database.Tables)
            {
                Lines.AddRange(AddConstraints(table));
            }
        }

        /// <summary>
        /// Gets code lines for database creation
        /// </summary>
        /// <returns>A sequence of <see cref="ILine"/> that represents the database creation</returns>
        protected virtual IEnumerable<ILine> AddDatabaseCreation()
        {
            yield return new CodeLine("CREATE DATABASE {0}", Database.GetObjectName(Database.Name));
            yield return new CodeLine("GO");

            yield return new EmptyLine();

            yield return new CodeLine("USE {0}", Database.GetObjectName(Database.Name));
            yield return new CodeLine("GO");

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
                    yield return new CodeLine("EXEC [sp_addextendedproperty]");
                    yield return new CodeLine("{0}@name = '{1}',", Indent(1), extendedProperty.Name);
                    yield return new CodeLine("{0}@value = '{1}',", Indent(1), extendedProperty.Value);
                    yield return new CodeLine("{0}@level0type = null,", Indent(1));
                    yield return new CodeLine("{0}@level0name = null,", Indent(1));
                    yield return new CodeLine("{0}@level1type = null,", Indent(1));
                    yield return new CodeLine("{0}@level1name = null,", Indent(1));
                    yield return new CodeLine("{0}@level2type = null,", Indent(1));
                    yield return new CodeLine("{0}@level2name = null", Indent(1));
                }

                yield return new CodeLine("GO");

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

                if (AddDropIfExists)
                {
                    yield return new CodeLine("IF NOT EXISTS (SELECT 1 FROM [sys].[schemas] WHERE [name] = '{0}')", schema);
                    yield return new CodeLine("{0}BEGIN", Indent(1));
                    yield return new CodeLine("{0}EXEC ('CREATE SCHEMA {1}')", Indent(2), Database.GetObjectName(schema));
                    yield return new CodeLine("{0}END", Indent(1));
                }
                else
                {
                    yield return new CodeLine("{0}CREATE SCHEMA {1}", Indent(2), Database.GetObjectName(schema));
                    yield return new CodeLine("{0}GO", Indent(1));
                }

                yield return new EmptyLine();
            }
        }

        /// <summary>
        /// Gets code lines for drop table if exists
        /// </summary>
        /// <param name="table">Instance of <see cref="Table"/></param>
        /// <returns>A sequence of <see cref="ILine"/> that represents the table dropping</returns>
        protected virtual IEnumerable<ILine> DropTableIfExists(Table table)
        {
            yield return new CodeLine("IF OBJECT_ID('{0}') IS NOT NULL", table.FullName);
            yield return new CodeLine("{0}DROP TABLE {1}", Indent(1), Database.NamingConvention.GetObjectName(table.Schema, table.Name));
            yield return new CodeLine("GO");
            yield return new EmptyLine();
        }

        /// <summary>
        /// Gets code lines for table creation
        /// </summary>
        /// <param name="table">Instance of <see cref="Table"/></param>
        /// <returns>A sequence of <see cref="ILine"/> that represents the table creation</returns>
        protected virtual IEnumerable<ILine> AddTable(Table table)
        {
            yield return new CodeLine("CREATE TABLE {0}", Database.GetObjectName(table));

            yield return new CodeLine("(");

            for (var i = 0; i < table.Columns.Count; i++)
            {
                var column = table.Columns[i];

                var output = new StringBuilder();

                var type = column.Type.ToUpper();

                if (Database.ColumnIsString(column))
                {
                    if (column.Length == 0)
                        output.AppendFormat("{0}{1} {2}({3})", Indent(1), Database.GetObjectName(column), type, "MAX");
                    else
                        output.AppendFormat("{0}{1} {2}({3})", Indent(1), Database.GetObjectName(column), type, column.Length);
                }
                else if (Database.ColumnIsNumber(column))
                {
                    if (column.Prec > 0 && column.Scale > 0)
                        output.AppendFormat("{0}{1} {2}({3}, {4})", Indent(1), Database.GetObjectName(column), type, column.Prec, column.Scale);
                    else if (column.Prec > 0)
                        output.AppendFormat("{0}{1} {2}({3})", Indent(1), Database.GetObjectName(column), type, column.Prec);
                    else
                        output.AppendFormat("{0}{1} {2}", Indent(1), Database.GetObjectName(column), type);
                }
                else
                {
                    output.AppendFormat("{0}{1} {2}", Indent(1), Database.GetObjectName(column), type);
                }

                output.AppendFormat(" {0}", column.Nullable ? "NULL" : "NOT NULL");

                if (table.Identity != null && table.Identity.Name == column.Name)
                    output.AppendFormat(" IDENTITY({0}, {1})", table.Identity.Seed, table.Identity.Increment);

                if (i < table.Columns.Count - 1)
                    output.Append(',');

                yield return new CodeLine(output.ToString());
            }

            yield return new CodeLine(")");
            yield return new CodeLine("GO");
            yield return new EmptyLine();
        }

        /// <summary>
        /// Gets code lines for table extended properties
        /// </summary>
        /// <param name="table">Instance of <see cref="Table"/></param>
        /// <returns>A sequence of <see cref="ILine"/> that represents the table dropping</returns>
        protected virtual IEnumerable<ILine> AddTableExtendedProperties(Table table)
        {
            foreach (ExtendedProperty extendedProperty in table.ImportBag.ExtendedProperties)
            {
                yield return new CodeLine("EXEC [sp_addextendedproperty]");
                yield return new CodeLine("{0}@name = '{1}',", Indent(1), extendedProperty.Name);
                yield return new CodeLine("{0}@value = '{1}',", Indent(1), extendedProperty.Value);
                yield return new CodeLine("{0}@level0type = '{1}',", Indent(1), "schema");
                yield return new CodeLine("{0}@level0name = '{1}', ", Indent(1), table.Schema);
                yield return new CodeLine("{0}@level1type = '{1}',", Indent(1), "table");
                yield return new CodeLine("{0}@level1name = '{1}',", Indent(1), table.Name);
                yield return new CodeLine("{0}@level2type = null,", Indent(1));
                yield return new CodeLine("{0}@level2name = null", Indent(1));
                yield return new CodeLine("GO");
                yield return new EmptyLine();
            }

            for (var i = 0; i < table.Columns.Count; i++)
            {
                
            }

            var columnsWithExtendedProps = table.Columns.Where(item => item.ImportBag.ExtendedProperties.Count > 0).ToList();

            if (columnsWithExtendedProps.Count > 0)
            {
                for (var i = 0; i < columnsWithExtendedProps.Count; i++)
                {
                    var column = columnsWithExtendedProps[i];

                    foreach (ExtendedProperty extendedProperty in column.ImportBag.ExtendedProperties)
                    {
                        yield return new CodeLine("EXEC [sp_addextendedproperty]");
                        yield return new CodeLine("{0}@name = '{1}',", Indent(1), extendedProperty.Name);
                        yield return new CodeLine("{0}@value = '{1}',", Indent(1), extendedProperty.Value);
                        yield return new CodeLine("{0}@level0type = '{1}',", Indent(1), "schema");
                        yield return new CodeLine("{0}@level0name = '{1}',", Indent(1), table.Schema);
                        yield return new CodeLine("{0}@level1type = '{1}',", Indent(1), "table");
                        yield return new CodeLine("{0}@level1name = '{1}',", Indent(1), table.Name);
                        yield return new CodeLine("{0}@level2type = '{1}',", Indent(1), "column");
                        yield return new CodeLine("{0}@level2name = '{1}'", Indent(1), column.Name);
                        yield return new CodeLine("GO");
                        yield return new EmptyLine();
                    }
                }
            }
        }

        ///// <summary>
        ///// Gets code lines for table extended properties
        ///// </summary>
        ///// <param name="table">Instance of <see cref="Table"/>class</param>
        ///// <param name="column">Instance of <see cref="Column"/> class</param>
        ///// <returns>A sequence of <see cref="ILine"/> that represents the table dropping</returns>
        //protected virtual IEnumerable<ILine> AddTableExtendedProperties(Table table, Column column)
        //{
        //}

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

                yield return new CodeLine("ALTER TABLE {0} ADD CONSTRAINT {1}", Database.GetObjectName(table), constraintName);

                yield return new CodeLine("{0}PRIMARY KEY ({1})", Indent(1), string.Join(", ", pk.Key.Select(item => Database.GetObjectName(item))));
                yield return new CodeLine("GO");

                yield return new EmptyLine();
            }

            foreach (var unique in table.Uniques)
            {
                var constraintName = string.IsNullOrEmpty(unique.ConstraintName) ? nm.GetUniqueConstraintName(table, unique.Key.ToArray()) : Database.GetObjectName(unique.ConstraintName);

                yield return new CodeLine("ALTER TABLE {0} ADD CONSTRAINT {1}", Database.GetObjectName(table), constraintName);

                yield return new CodeLine("{0}UNIQUE ({1})", Indent(1), string.Join(", ", unique.Key.Select(item => Database.GetObjectName(item))));
                yield return new CodeLine("GO");

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

                        yield return new CodeLine("ALTER TABLE {0} ADD CONSTRAINT {1}", Database.GetObjectName(table), constraintName);

                        yield return new CodeLine("{0}FOREIGN KEY ({1}) REFERENCES {2}", Indent(1), string.Join(", ", foreignKey.Key.Select(item => Database.GetObjectName(item))), Database.GetObjectName(references));
                        yield return new CodeLine("GO");

                        yield return new EmptyLine();
                    }
                }
            }

            foreach (var def in table.Defaults)
            {
                var constraintName = string.IsNullOrEmpty(def.ConstraintName) ? nm.GetDefaultConstraintName(table, def.Key.First()) : Database.GetObjectName(def.ConstraintName);

                yield return new CodeLine("ALTER TABLE {0} ADD CONSTRAINT {1}", Database.GetObjectName(table), constraintName);

                yield return new CodeLine("{0}DEFAULT {1} FOR {2}", Indent(1), def.Value, def.Key.First());
                yield return new CodeLine("GO");

                yield return new EmptyLine();
            }

            foreach (var check in table.Checks)
            {
                var constraintName = string.IsNullOrEmpty(check.ConstraintName) ? nm.GetCheckConstraintName(table, check.Key.First()) : Database.GetObjectName(check.ConstraintName);

                yield return new CodeLine("ALTER TABLE {0} ADD CONSTRAINT {1}", Database.GetObjectName(table), constraintName);

                yield return new CodeLine("{0}CHECK ({1})", Indent(1), check.Expression);
                yield return new CodeLine("GO");

                yield return new EmptyLine();
            }
        }

        /// <summary>
        /// Gets the output code for current <see cref="SqlServerDatabaseScriptCodeBuilder"/> instance
        /// </summary>
        protected string Code
            => string.Empty;
    }
}
