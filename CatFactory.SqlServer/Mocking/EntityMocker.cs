using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace CatFactory.SqlServer.Mocking
{
    /// <summary>
    /// 
    /// </summary>
    /// <typeparam name="TModel"></typeparam>
    public class EntityMocker<TModel> where TModel : class, new()
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

        /// <summary>
        /// 
        /// </summary>
        /// <param name="count"></param>
        /// <returns></returns>
        public virtual IEnumerable<TModel> CreateMock(int count)
        {
            for (var i = 0; i < count; i++)
            {
                var mock = new TModel();
                var type = mock.GetType();

                foreach (var parentSetting in EntitySettings.Where(x => x.When == null))
                {
                    var parentProperty = type.GetProperty(parentSetting.Name);

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
                            var chilProperty = type.GetProperty(childSetting.Name);

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
