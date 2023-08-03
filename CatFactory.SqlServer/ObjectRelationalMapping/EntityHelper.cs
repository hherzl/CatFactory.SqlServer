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
    public static partial class EntityHelper
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

        public static EntityResult<TModel> SetNaming<TModel>(this EntityResult<TModel> result, string name, string schema = null) where TModel : class
        {
            result.Table.Name = name;
            result.Table.Schema = string.IsNullOrEmpty(schema) ? result.Database.DefaultSchema : schema;

            if (result.Database.DbObjects.Any(item => item.Name == result.Table.Name && item.Schema == result.Table.Schema))
                throw new ObjectRelationMappingException(string.Format("There is already object with name '{0}' in database", result.Table.FullName));

            result.Database.DbObjects.Add(new(result.Table.Schema, result.Table.Name));

            return result;
        }
    }
}
