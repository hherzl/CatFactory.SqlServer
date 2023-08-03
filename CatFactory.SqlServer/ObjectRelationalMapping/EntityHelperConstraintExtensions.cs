using System;
using System.Linq;
using System.Linq.Expressions;
using CatFactory.ObjectRelationalMapping;

namespace CatFactory.SqlServer.ObjectRelationalMapping
{
    public static partial class EntityHelper
    {
#pragma warning disable CS1591
        public static EntityResult<TModel> SetPrimaryKey<TModel, TProp>(this EntityResult<TModel> result, Expression<Func<TModel, TProp>> selector, string constraintName = null) where TModel : class
        {
            var names = GetPropertyNames(selector).ToList();

            result.Table.PrimaryKey = new PrimaryKey(constraintName ?? result.Database.NamingConvention.GetPrimaryKeyConstraintName(result.Table, names.ToArray()), names.ToArray());

            return result;
        }

        public static EntityResult<TModel> AddUnique<TModel, TProp>(this EntityResult<TModel> result, Expression<Func<TModel, TProp>> selector, string constraintName = null) where TModel : class
        {
            var names = GetPropertyNames(selector).ToList();

            result.Table.Uniques.Add(new Unique(constraintName ?? result.Database.NamingConvention.GetUniqueConstraintName(result.Table, names.ToArray()), names.ToArray()));

            return result;
        }

        public static EntityResult<TModel> AddForeignKey<TModel, TProp>(this EntityResult<TModel> result, Expression<Func<TModel, TProp>> selector, Table references, string constraintName = null) where TModel : class
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
                Key = new()
                {
                    GetPropertyName(selector)
                },
                Value = value
            });

            return result;
        }

        public static EntityResult<TModel> AddCheck<TModel, TProperty>(this EntityResult<TModel> result, Expression<Func<TModel, TProperty>> selector, string expresion, string constraintName = null) where TModel : class
        {
            result.Table.Checks.Add(new Check
            {
                ConstraintName = constraintName ?? result.Database.NamingConvention.GetCheckConstraintName(result.Table, GetPropertyName(selector)),
                Key = new(),
                Expression = expresion
            });

            return result;
        }
    }
}
