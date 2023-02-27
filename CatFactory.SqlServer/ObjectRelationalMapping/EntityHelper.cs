using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Linq.Expressions;
using CatFactory.ObjectRelationalMapping;
using CatFactory.SqlServer.DatabaseObjectModel;

namespace CatFactory.SqlServer.ObjectRelationalMapping
{
    /// <summary>
    /// Contains extension methods to define entities (Tables) using lambda expressions
    /// </summary>
    public static class EntityHelper
    {
#pragma warning disable CS1591
        public static EntityResult<TModel> DefineEntity<TModel>(this SqlServerDatabase database, TModel model) where TModel : class
        {
            var result = new EntityResult<TModel>
            {
                Table = new Table
                {
                    Schema = database.DefaultSchema,
                    Name = model.GetType().Name
                },
                Database = database,
                Model = model
            };

            result.Table.ImportBag.ExtendedProperties = new Collection<ExtendedProperty>();

            foreach (var property in model.GetType().GetProperties())
            {
                var propType = property.PropertyType;

                var defType = database.DefaultTypeMaps.FirstOrDefault(item => item.Type == propType);

                var type = string.Empty;

                if (defType == null)
                {
                    var types = database.GetDatabaseTypeMaps(propType).ToList();

                    type = types.Count == 0 ? "" : types.First().DatabaseType;
                }
                else
                {
                    type = defType.DatabaseType;
                }

                result.Table.Columns.Add(new Column
                {
                    Name = property.Name,
                    Type = type
                });

                result.Table.Columns.Last().ImportBag.ExtendedProperties = new Collection<ExtendedProperty>();
            }

            database.Tables.Add(result.Table);

            return result;
        }

        private static string GetPropertyName<TModel, TProperty>(Expression<Func<TModel, TProperty>> selector)
        {
            var memberExpression = selector.Body as MemberExpression;

            if (memberExpression == null)
            {
                if (selector.Body is UnaryExpression unaryExpression)
                    memberExpression = (MemberExpression)unaryExpression.Operand;
            }

            return memberExpression.Member.Name;
        }

        private static IEnumerable<string> GetPropertyNames<TModel, TProperty>(Expression<Func<TModel, TProperty>> selector)
        {
            if (selector.Body is NewExpression newExpression)
            {
                foreach (Expression argument in newExpression.Arguments)
                {
                    var prop = (MemberExpression)argument;

                    yield return prop.Member.Name;
                }
            }
            else
            {
                var memberExpression = selector.Body as MemberExpression;

                if (memberExpression == null)
                {
                    if (selector.Body is UnaryExpression unaryExpression)
                        memberExpression = (MemberExpression)unaryExpression.Operand;
                }

                yield return memberExpression.Member.Name;
            }
        }

        public static EntityResult<TModel> SetNaming<TModel>(this EntityResult<TModel> result, string name, string schema = "") where TModel : class
        {
            result.Table.Name = name;
            result.Table.Schema = string.IsNullOrEmpty(schema) ? result.Database.DefaultSchema : schema;

            if (result.Database.DbObjects.Any(item => item.Name == result.Table.Name && item.Schema == result.Table.Schema))
                throw new ObjectRelationMappingException(string.Format("There is already object with name '{0}.{1}' in database", result.Table.Schema, result.Table.Name));

            result.Database.DbObjects.Add(new DbObject(result.Table.Schema, result.Table.Name));

            return result;
        }

        public static EntityResult<TModel> SetColumnFor<TModel>(this EntityResult<TModel> result, Expression<Func<TModel, bool>> selector, string type = "", bool nullable = false) where TModel : class
        {
            var column = new Column
            {
                Name = GetPropertyName(selector)
            };

            if (result.Table.Columns.Contains(column))
            {
                if (!string.IsNullOrEmpty(type))
                    result.Table[column.Name].Type = type;

                result.Table[column.Name].Nullable = nullable;
            }
            else
            {
                result.Table.Columns.Add(column);
            }

            return result;
        }

        public static EntityResult<TModel> SetColumnFor<TModel>(this EntityResult<TModel> result, Expression<Func<TModel, bool?>> selector, string type = "", bool nullable = false) where TModel : class
        {
            var column = new Column
            {
                Name = GetPropertyName(selector)
            };

            if (result.Table.Columns.Contains(column))
            {
                if (!string.IsNullOrEmpty(type))
                    result.Table[column.Name].Type = type;

                result.Table[column.Name].Nullable = nullable;
            }
            else
            {
                result.Table.Columns.Add(column);
            }

            return result;
        }

        public static EntityResult<TModel> SetColumnFor<TModel>(this EntityResult<TModel> result, Expression<Func<TModel, byte>> selector, string type = "", bool nullable = false) where TModel : class
        {
            var column = new Column
            {
                Name = GetPropertyName(selector)
            };

            if (result.Table.Columns.Contains(column))
            {
                if (!string.IsNullOrEmpty(type))
                    result.Table[column.Name].Type = type;

                result.Table[column.Name].Nullable = nullable;
            }
            else
            {
                result.Table.Columns.Add(column);
            }

            return result;
        }

        public static EntityResult<TModel> SetColumnFor<TModel>(this EntityResult<TModel> result, Expression<Func<TModel, byte?>> selector, string type = "", bool nullable = false) where TModel : class
        {
            var column = new Column
            {
                Name = GetPropertyName(selector)
            };

            if (result.Table.Columns.Contains(column))
            {
                if (!string.IsNullOrEmpty(type))
                    result.Table[column.Name].Type = type;

                result.Table[column.Name].Nullable = nullable;
            }
            else
            {
                result.Table.Columns.Add(column);
            }

            return result;
        }

        public static EntityResult<TModel> SetColumnFor<TModel>(this EntityResult<TModel> result, Expression<Func<TModel, byte[]>> selector, string type = "", bool nullable = false) where TModel : class
        {
            var column = new Column
            {
                Name = GetPropertyName(selector)
            };

            if (result.Table.Columns.Contains(column))
            {
                if (!string.IsNullOrEmpty(type))
                    result.Table[column.Name].Type = type;

                result.Table[column.Name].Nullable = nullable;
            }
            else
            {
                result.Table.Columns.Add(column);
            }

            return result;
        }

        public static EntityResult<TModel> SetColumnFor<TModel>(this EntityResult<TModel> result, Expression<Func<TModel, DateTime>> selector, string type = "", bool nullable = false) where TModel : class
        {
            var column = new Column
            {
                Name = GetPropertyName(selector)
            };

            if (result.Table.Columns.Contains(column))
            {
                if (!string.IsNullOrEmpty(type))
                    result.Table[column.Name].Type = type;

                result.Table[column.Name].Nullable = nullable;
            }
            else
            {
                result.Table.Columns.Add(column);
            }

            return result;
        }

        public static EntityResult<TModel> SetColumnFor<TModel>(this EntityResult<TModel> result, Expression<Func<TModel, DateTime?>> selector, string type = "", bool nullable = false) where TModel : class
        {
            var column = new Column
            {
                Name = GetPropertyName(selector)
            };

            if (result.Table.Columns.Contains(column))
            {
                if (!string.IsNullOrEmpty(type))
                    result.Table[column.Name].Type = type;

                result.Table[column.Name].Nullable = nullable;
            }
            else
            {
                result.Table.Columns.Add(column);
            }

            return result;
        }

        public static EntityResult<TModel> SetColumnFor<TModel>(this EntityResult<TModel> result, Expression<Func<TModel, DateTimeOffset>> selector, bool nullable = false) where TModel : class
        {
            var column = new Column
            {
                Name = GetPropertyName(selector)
            };

            if (result.Table.Columns.Contains(column))
                result.Table[column.Name].Nullable = nullable;
            else
                result.Table.Columns.Add(column);

            return result;
        }

        public static EntityResult<TModel> SetColumnFor<TModel>(this EntityResult<TModel> result, Expression<Func<TModel, DateTimeOffset?>> selector, bool nullable = false) where TModel : class
        {
            var column = new Column
            {
                Name = GetPropertyName(selector)
            };

            if (result.Table.Columns.Contains(column))
                result.Table[column.Name].Nullable = nullable;
            else
                result.Table.Columns.Add(column);

            return result;
        }

        public static EntityResult<TModel> SetColumnFor<TModel>(this EntityResult<TModel> result, Expression<Func<TModel, decimal>> selector, string type = "", short prec = 0, short scale = 0, bool nullable = false) where TModel : class
        {
            var column = new Column
            {
                Name = GetPropertyName(selector)
            };

            if (result.Table.Columns.Contains(column))
            {
                if (!string.IsNullOrEmpty(type))
                    result.Table[column.Name].Type = type;

                result.Table[column.Name].Prec = prec;
                result.Table[column.Name].Scale = scale;
                result.Table[column.Name].Nullable = nullable;
            }
            else
            {
                result.Table.Columns.Add(column);
            }

            return result;
        }

        public static EntityResult<TModel> SetColumnFor<TModel>(this EntityResult<TModel> result, Expression<Func<TModel, decimal?>> selector, string type = "", short prec = 0, short scale = 0, bool nullable = false) where TModel : class
        {
            var column = new Column
            {
                Name = GetPropertyName(selector)
            };

            if (result.Table.Columns.Contains(column))
            {
                if (!string.IsNullOrEmpty(type))
                    result.Table[column.Name].Type = type;

                result.Table[column.Name].Prec = prec;
                result.Table[column.Name].Scale = scale;
                result.Table[column.Name].Nullable = nullable;
            }
            else
            {
                result.Table.Columns.Add(column);
            }

            return result;
        }

        public static EntityResult<TModel> SetColumnFor<TModel>(this EntityResult<TModel> result, Expression<Func<TModel, double>> selector, string type = "", short prec = 0, short scale = 0, bool nullable = false) where TModel : class
        {
            var column = new Column
            {
                Name = GetPropertyName(selector)
            };

            if (result.Table.Columns.Contains(column))
            {
                if (!string.IsNullOrEmpty(type))
                    result.Table[column.Name].Type = type;

                result.Table[column.Name].Prec = prec;
                result.Table[column.Name].Scale = scale;
                result.Table[column.Name].Nullable = nullable;
            }
            else
            {
                result.Table.Columns.Add(column);
            }

            return result;
        }

        public static EntityResult<TModel> SetColumnFor<TModel>(this EntityResult<TModel> result, Expression<Func<TModel, double?>> selector, string type = "", short prec = 0, short scale = 0, bool nullable = false) where TModel : class
        {
            var column = new Column
            {
                Name = GetPropertyName(selector)
            };

            if (result.Table.Columns.Contains(column))
            {
                if (!string.IsNullOrEmpty(type))
                    result.Table[column.Name].Type = type;

                result.Table[column.Name].Prec = prec;
                result.Table[column.Name].Scale = scale;
                result.Table[column.Name].Nullable = nullable;
            }
            else
            {
                result.Table.Columns.Add(column);
            }

            return result;
        }

        public static EntityResult<TModel> SetColumnFor<TModel>(this EntityResult<TModel> result, Expression<Func<TModel, float>> selector, string type = "", short prec = 0, short scale = 0, bool nullable = false) where TModel : class
        {
            var column = new Column
            {
                Name = GetPropertyName(selector)
            };

            if (result.Table.Columns.Contains(column))
            {
                if (!string.IsNullOrEmpty(type))
                    result.Table[column.Name].Type = type;

                result.Table[column.Name].Prec = prec;
                result.Table[column.Name].Scale = scale;
                result.Table[column.Name].Nullable = nullable;
            }
            else
            {
                result.Table.Columns.Add(column);
            }

            return result;
        }

        public static EntityResult<TModel> SetColumnFor<TModel>(this EntityResult<TModel> result, Expression<Func<TModel, float?>> selector, string type = "", short prec = 0, short scale = 0, bool nullable = false) where TModel : class
        {
            var column = new Column
            {
                Name = GetPropertyName(selector)
            };

            if (result.Table.Columns.Contains(column))
            {
                if (!string.IsNullOrEmpty(type))
                    result.Table[column.Name].Type = type;

                result.Table[column.Name].Prec = prec;
                result.Table[column.Name].Scale = scale;
                result.Table[column.Name].Nullable = nullable;
            }
            else
            {
                result.Table.Columns.Add(column);
            }

            return result;
        }

        public static EntityResult<TModel> SetColumnFor<TModel>(this EntityResult<TModel> result, Expression<Func<TModel, Guid>> selector, bool nullable = false) where TModel : class
        {
            var column = new Column
            {
                Name = GetPropertyName(selector)
            };

            if (result.Table.Columns.Contains(column))
                result.Table[column.Name].Nullable = nullable;
            else
                result.Table.Columns.Add(column);

            return result;
        }

        public static EntityResult<TModel> SetColumnFor<TModel>(this EntityResult<TModel> result, Expression<Func<TModel, Guid?>> selector, bool nullable = false) where TModel : class
        {
            var column = new Column
            {
                Name = GetPropertyName(selector)
            };

            if (result.Table.Columns.Contains(column))
                result.Table[column.Name].Nullable = nullable;
            else
                result.Table.Columns.Add(column);

            return result;
        }

        public static EntityResult<TModel> SetColumnFor<TModel>(this EntityResult<TModel> result, Expression<Func<TModel, int>> selector, string type = "", bool nullable = false) where TModel : class
        {
            var column = new Column
            {
                Name = GetPropertyName(selector)
            };

            if (result.Table.Columns.Contains(column))
            {
                if (!string.IsNullOrEmpty(type))
                    result.Table[column.Name].Type = type;

                result.Table[column.Name].Nullable = nullable;
            }
            else
            {
                result.Table.Columns.Add(column);
            }

            return result;
        }

        public static EntityResult<TModel> SetColumnFor<TModel>(this EntityResult<TModel> result, Expression<Func<TModel, int?>> selector, string type = "", bool nullable = false) where TModel : class
        {
            var column = new Column
            {
                Name = GetPropertyName(selector)
            };

            if (result.Table.Columns.Contains(column))
            {
                if (!string.IsNullOrEmpty(type))
                    result.Table[column.Name].Type = type;

                result.Table[column.Name].Nullable = nullable;
            }
            else
            {
                result.Table.Columns.Add(column);
            }

            return result;
        }

        public static EntityResult<TModel> SetColumnFor<TModel>(this EntityResult<TModel> result, Expression<Func<TModel, long>> selector, string type = "", bool nullable = false) where TModel : class
        {
            var column = new Column
            {
                Name = GetPropertyName(selector)
            };

            if (result.Table.Columns.Contains(column))
            {
                if (!string.IsNullOrEmpty(type))
                    result.Table[column.Name].Type = type;

                result.Table[column.Name].Nullable = nullable;
            }
            else
            {
                result.Table.Columns.Add(column);
            }

            return result;
        }

        public static EntityResult<TModel> SetColumnFor<TModel>(this EntityResult<TModel> result, Expression<Func<TModel, long?>> selector, string type = "", bool nullable = false) where TModel : class
        {
            var column = new Column
            {
                Name = GetPropertyName(selector)
            };

            if (result.Table.Columns.Contains(column))
            {
                if (!string.IsNullOrEmpty(type))
                    result.Table[column.Name].Type = type;

                result.Table[column.Name].Nullable = nullable;
            }
            else
            {
                result.Table.Columns.Add(column);
            }

            return result;
        }

        public static EntityResult<TModel> SetColumnFor<TModel>(this EntityResult<TModel> result, Expression<Func<TModel, short>> selector, string type = "", bool nullable = false) where TModel : class
        {
            var column = new Column
            {
                Name = GetPropertyName(selector)
            };

            if (result.Table.Columns.Contains(column))
            {
                if (!string.IsNullOrEmpty(type))
                    result.Table[column.Name].Type = type;

                result.Table[column.Name].Nullable = nullable;
            }
            else
            {
                result.Table.Columns.Add(column);
            }

            return result;
        }

        public static EntityResult<TModel> SetColumnFor<TModel>(this EntityResult<TModel> result, Expression<Func<TModel, short?>> selector, string type = "", bool nullable = false) where TModel : class
        {
            var column = new Column
            {
                Name = GetPropertyName(selector)
            };

            if (result.Table.Columns.Contains(column))
            {
                if (!string.IsNullOrEmpty(type))
                    result.Table[column.Name].Type = type;

                result.Table[column.Name].Nullable = nullable;
            }
            else
            {
                result.Table.Columns.Add(column);
            }

            return result;
        }

        public static EntityResult<TModel> SetColumnFor<TModel>(this EntityResult<TModel> result, Expression<Func<TModel, string>> selector, string type = "", int length = 0, bool nullable = false, string collation = "") where TModel : class
        {
            var column = new Column
            {
                Name = GetPropertyName(selector)
            };

            if (result.Table.Columns.Contains(column))
            {
                if (!string.IsNullOrEmpty(type))
                    result.Table[column.Name].Type = type;

                result.Table[column.Name].Length = length;
                result.Table[column.Name].Nullable = nullable;

                if (!string.IsNullOrEmpty(collation))
                    result.Table[column.Name].Collation = collation;
            }
            else
            {
                result.Table.Columns.Add(column);
            }

            return result;
        }

        public static EntityResult<TModel> SetColumnFor<TModel>(this EntityResult<TModel> result, Expression<Func<TModel, TimeSpan>> selector, bool nullable = false) where TModel : class
        {
            var column = new Column
            {
                Name = GetPropertyName(selector)
            };

            if (result.Table.Columns.Contains(column))
                result.Table[column.Name].Nullable = nullable;
            else
                result.Table.Columns.Add(column);

            return result;
        }

        public static EntityResult<TModel> SetColumnFor<TModel>(this EntityResult<TModel> result, Expression<Func<TModel, TimeSpan?>> selector, bool nullable = false) where TModel : class
        {
            var column = new Column
            {
                Name = GetPropertyName(selector)
            };

            if (result.Table.Columns.Contains(column))
                result.Table[column.Name].Nullable = nullable;
            else
                result.Table.Columns.Add(column);

            return result;
        }

        public static EntityResult<TModel> SetIdentity<TModel>(this EntityResult<TModel> result, Expression<Func<TModel, decimal>> selector, int seed = 1, int increment = 1) where TModel : class
        {
            result.Table.Identity = new Identity(GetPropertyName(selector), seed, increment);

            return result;
        }

        public static EntityResult<TModel> SetIdentity<TModel>(this EntityResult<TModel> result, Expression<Func<TModel, decimal?>> selector, int seed = 1, int increment = 1) where TModel : class
        {
            result.Table.Identity = new Identity(GetPropertyName(selector), seed, increment);

            return result;
        }

        public static EntityResult<TModel> SetIdentity<TModel>(this EntityResult<TModel> result, Expression<Func<TModel, int>> selector, int seed = 1, int increment = 1) where TModel : class
        {
            result.Table.Identity = new Identity(GetPropertyName(selector), seed, increment);

            return result;
        }

        public static EntityResult<TModel> SetIdentity<TModel>(this EntityResult<TModel> result, Expression<Func<TModel, int?>> selector, int seed = 1, int increment = 1) where TModel : class
        {
            result.Table.Identity = new Identity(GetPropertyName(selector), seed, increment);

            return result;
        }

        public static EntityResult<TModel> SetIdentity<TModel>(this EntityResult<TModel> result, Expression<Func<TModel, long>> selector, int seed = 1, int increment = 1) where TModel : class
        {
            result.Table.Identity = new Identity(GetPropertyName(selector), seed, increment);

            return result;
        }

        public static EntityResult<TModel> SetIdentity<TModel>(this EntityResult<TModel> result, Expression<Func<TModel, long?>> selector, int seed = 1, int increment = 1) where TModel : class
        {
            result.Table.Identity = new Identity(GetPropertyName(selector), seed, increment);

            return result;
        }

        public static EntityResult<TModel> SetIdentity<TModel>(this EntityResult<TModel> result, Expression<Func<TModel, short>> selector, int seed = 1, int increment = 1) where TModel : class
        {
            result.Table.Identity = new Identity(GetPropertyName(selector), seed, increment);

            return result;
        }

        public static EntityResult<TModel> SetIdentity<TModel>(this EntityResult<TModel> result, Expression<Func<TModel, short?>> selector, int seed = 1, int increment = 1) where TModel : class
        {
            result.Table.Identity = new Identity(GetPropertyName(selector), seed, increment);

            return result;
        }

        public static EntityResult<TModel> SetPrimaryKey<TModel, TProperty>(this EntityResult<TModel> result, Expression<Func<TModel, TProperty>> selector, string constraintName = null) where TModel : class
        {
            var names = GetPropertyNames(selector).ToList();

            result.Table.PrimaryKey = new PrimaryKey(constraintName ?? result.Database.NamingConvention.GetPrimaryKeyConstraintName(result.Table, names.ToArray()), names.ToArray());

            return result;
        }

        public static EntityResult<TModel> AddUnique<TModel, TProperty>(this EntityResult<TModel> result, Expression<Func<TModel, TProperty>> selector, string constraintName = null) where TModel : class
        {
            var names = GetPropertyNames(selector).ToList();

            result.Table.Uniques.Add(new Unique(constraintName ?? result.Database.NamingConvention.GetUniqueConstraintName(result.Table, names.ToArray()), names.ToArray()));

            return result;
        }

        public static EntityResult<TModel> AddForeignKey<TModel, TProperty>(this EntityResult<TModel> result, Expression<Func<TModel, TProperty>> selector, Table references, string constraintName = null) where TModel : class
        {
            var names = GetPropertyNames(selector).ToList();

            if (references.PrimaryKey == null)
                throw new ObjectRelationMappingException(string.Format("The '{0}.{1}' table doesn't have a definition for primary key", references.Schema, references.Name));

            if (names.Count == 1 && references.PrimaryKey?.Key.Count == 1)
            {
                var fk = result.Table[names.First()];
                var pk = references.GetColumnsFromConstraint(references.PrimaryKey).First();

                if (fk.Type != pk.Type)
                    throw new ObjectRelationMappingException(string.Format("The columns '{0}' and '{1}' have different data types: '{2}', '{3}'", fk.Name, pk.Name, fk.Type, pk.Type));
            }

            result.Table.ForeignKeys.Add(new ForeignKey
            {
                ConstraintName = constraintName ?? result.Database.NamingConvention.GetForeignKeyConstraintName(result.Table, names.ToArray(), references),
                Key = GetPropertyNames(selector).ToList(),
                References = references.FullName
            });

            return result;
        }

        public static EntityResult<TModel> AddDefault<TModel, TProperty>(this EntityResult<TModel> result, Expression<Func<TModel, TProperty>> selector, string value, string constraintName = null) where TModel : class
        {
            result.Table.Defaults.Add(new Default
            {
                ConstraintName = constraintName ?? result.Database.NamingConvention.GetDefaultConstraintName(result.Table, GetPropertyName(selector)),
                Key = new List<string> { GetPropertyName(selector) },
                Value = value
            });

            return result;
        }

        public static EntityResult<TModel> AddCheck<TModel, TProperty>(this EntityResult<TModel> result, Expression<Func<TModel, TProperty>> selector, string expresion, string constraintName = null) where TModel : class
        {
            result.Table.Checks.Add(new Check
            {
                ConstraintName = constraintName ?? result.Database.NamingConvention.GetCheckConstraintName(result.Table, GetPropertyName(selector)),
                Key = new List<string>(),
                Expression = expresion
            });

            return result;
        }

        public static EntityResult<TModel> AddExtendedProperty<TModel>(this EntityResult<TModel> result, string name, string value) where TModel : class
        {
            result.Table.ImportBag.ExtendedProperties.Add(new ExtendedProperty(name, value));

            return result;
        }

        public static EntityResult<TModel> AddExtendedProperty<TModel, TProperty>(this EntityResult<TModel> result, Expression<Func<TModel, TProperty>> selector, string name, string value) where TModel : class
        {
            result.Table[GetPropertyName(selector)].ImportBag.ExtendedProperties.Add(new ExtendedProperty(name, value));

            return result;
        }
    }
}
