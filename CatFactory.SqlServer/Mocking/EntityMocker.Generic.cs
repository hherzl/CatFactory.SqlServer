using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace CatFactory.SqlServer.Mocking
{
#pragma warning disable CS1591
    public class EntityMocker<TModel> where TModel : class
    {
        private static object GetDefaultValue(Type type)
            => type.IsValueType ? Activator.CreateInstance(type) : null;

        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private List<EntitySetting> m_entitySettings;

        public EntityMocker()
        {
        }

        public List<EntitySetting> EntitySettings
        {
            get => m_entitySettings ?? (m_entitySettings = new List<EntitySetting>());
            set => m_entitySettings = value;
        }

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
                            var childProperty = mockType.GetProperty(childSetting.Name);

                            var childIndex = new Random().Next(0, childSetting.Values.Count() - 1);

                            var childValue = childSetting.Values.ElementAt(childIndex);

                            childProperty.SetValue(mock, childValue);
                        }
                    }
                }

                yield return mock;
            }
        }
    }
}
