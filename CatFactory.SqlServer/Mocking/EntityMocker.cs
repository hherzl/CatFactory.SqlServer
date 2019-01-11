using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace CatFactory.SqlServer.Mocking
{
    /// <summary>
    /// 
    /// </summary>
    public static class EntityMocker
    {
        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="TModel"></typeparam>
        /// <param name="model"></param>
        /// <returns></returns>
        public static EntityMocker<TModel> Create<TModel>(TModel model) where TModel : class
            => new EntityMocker<TModel>();
    }

    /// <summary>
    /// 
    /// </summary>
    /// <typeparam name="TModel"></typeparam>
    public class EntityMocker<TModel> where TModel : class
    {
        /// <summary>
        /// 
        /// </summary>
        public EntityMocker()
        {
        }

        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private List<EntitySetting> m_entitySettings;

        /// <summary>
        /// 
        /// </summary>
        public List<EntitySetting> EntitySettings
        {
            get
            {
                return m_entitySettings ?? (m_entitySettings = new List<EntitySetting>());
            }
            set
            {
                m_entitySettings = value;
            }
        }

        private static object GetDefaultValue(Type type)
            => type.IsValueType ? Activator.CreateInstance(type) : null;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="count"></param>
        /// <returns></returns>
        public virtual IEnumerable<TModel> CreateMocks(int count)
        {
            for (var i = 0; i < count; i++)
            {
                var mockType = typeof(TModel);
                var mock = Activator.CreateInstance<TModel>();

                foreach (var parentSetting in EntitySettings.Where(x => x.When == null))
                {
                    var parentProperty = mockType.GetProperty(parentSetting.Name);

                    if (parentSetting.DateTimeFunc != null)
                    {
                        parentProperty.SetValue(mock, parentSetting.DateTimeFunc());
                        continue;
                    }

                    if (parentSetting.Int32Func != null)
                    {
                        parentProperty.SetValue(mock, parentSetting.Int32Func());
                        continue;
                    }

                    if (parentSetting.Values != null)
                    {
                        var index = new Random().Next(0, parentSetting.Values.Count());

                        var value = parentSetting.Values.ElementAt(index);

                        parentProperty.SetValue(mock, value);

                        foreach (var childSetting in EntitySettings.Where(x => x.When == value))
                        {
                            var chilProperty = mockType.GetProperty(childSetting.Name);

                            var childIndex = new Random().Next(0, childSetting.Values.Count() - 1);

                            var childValue = childSetting.Values.ElementAt(childIndex);

                            chilProperty.SetValue(mock, childValue);
                        }
                    }
                }

                yield return mock;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="count"></param>
        /// <returns></returns>
        public virtual IEnumerable<TModel> CreateAnonymousMocks(int count)
        {
            for (var i = 0; i < count; i++)
            {
                var mockType = typeof(TModel);
                var mockConstructors = mockType.GetConstructors().ToList();
                TModel mock = null;

                if (mockConstructors.Count == 1)
                {
                    var constructor = mockConstructors.First();
                    var parameters = constructor.GetParameters();

                    if (parameters.Count() == 0)
                    {
                        mock = Activator.CreateInstance<TModel>();
                    }
                    else
                    {
                        var args = new object[parameters.Length];

                        for (var j = 0; j < parameters.Length; j++)
                        {
                            args[j] = GetDefaultValue(parameters[j].GetType());
                        }

                        mock = (TModel)Activator.CreateInstance(mockType, args);
                    }
                }

                foreach (var parentSetting in EntitySettings.Where(x => x.When == null))
                {
                    var parentProperty = mockType.GetProperty(parentSetting.Name);

                    if (parentSetting.DateTimeFunc != null)
                    {
                        parentProperty.SetValue(mock, parentSetting.DateTimeFunc());
                        continue;
                    }

                    if (parentSetting.Int32Func != null)
                    {
                        parentProperty.SetValue(mock, parentSetting.Int32Func());
                        continue;
                    }

                    if (parentSetting.Values != null)
                    {
                        var index = new Random().Next(0, parentSetting.Values.Count());

                        var value = parentSetting.Values.ElementAt(index);

                        parentProperty.SetValue(mock, value);

                        foreach (var childSetting in EntitySettings.Where(x => x.When == value))
                        {
                            var chilProperty = mockType.GetProperty(childSetting.Name);

                            var childIndex = new Random().Next(0, childSetting.Values.Count() - 1);

                            var childValue = childSetting.Values.ElementAt(childIndex);

                            chilProperty.SetValue(mock, childValue);
                        }
                    }
                }

                yield return mock;
            }
        }
    }
}
