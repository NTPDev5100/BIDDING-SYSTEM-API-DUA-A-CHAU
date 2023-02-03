using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Utilities
{
    public static class ReflectionHelper
    {
        public static object GetPropValue(this object obj, string propName)
        {
            string[] nameParts = propName.Split('.');
            if (nameParts.Length == 1)
            {
                if (obj.GetType().GetProperty(propName) == null)
                    return null;
                return obj.GetType().GetProperty(propName).GetValue(obj, null);
            }

            foreach (string part in nameParts)
            {
                if (obj == null) { return null; }

                Type type = obj.GetType();
                PropertyInfo info = type.GetProperty(part);
                if (info == null) { return null; }

                obj = info.GetValue(obj, null);
            }
            return obj;
        }
    }

    public class ReflectionUtilities
    {
        private class PropertiesCustom
        {
            public string Key { get; set; }
            public string Value { get; set; }
            public string Description { get; set; }
        }
        /// <summary>
        /// Lấy Key, Value, Description tùy biến
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public static object GetPropertiesCustom(object obj)
        {
            if (obj != null)
            {
                List<PropertiesCustom> data = new List<PropertiesCustom>();
                foreach (PropertyInfo item in obj.GetType().GetProperties())
                {
                    string description = string.Empty;
                    string value = string.Empty;
                    string key = item.Name;
                    if (key == "TotalItem" || key == "Created" || key == "CreatedBy" || key == "Updated" || key == "UpdatedBy" || key == "Deleted" || key == "Active" || key == "IsAdmin" || key == "Password")
                        continue;
                    var descriptions = (DescriptionAttribute[])item.GetCustomAttributes(typeof(DescriptionAttribute), false);
                    if (descriptions.Length > 0)
                    {
                        description = descriptions[0].Description;
                    }
                    if (item.GetValue(obj) != null)
                    {
                        value = item.GetValue(obj).ToString();
                    }
                    data.Add(new PropertiesCustom()
                    {
                        Key = key,
                        Value = value,
                        Description = description
                    });
                }
                return data;
            }
            return null;
        }

        public static string GetDescriptions(PropertyInfo propertyInfo)
        {
            if (propertyInfo.GetCustomAttribute(typeof(DescriptionAttribute)) is DescriptionAttribute attribute)
            {
                return attribute.Description;
            }
            return string.Empty;
        }

        public static string GetClassDescription(Type type)
        {
            var descriptions = (DescriptionAttribute[])
                type.GetCustomAttributes(typeof(DescriptionAttribute), false);

            if (descriptions.Length == 0)
            {
                return null;
            }
            return descriptions[0].Description;
        }

        public static object FollowPropertyPath(object value, string path)
        {
            if (value == null) throw new ArgumentNullException("value");
            if (path == null) throw new ArgumentNullException("path");

            Type currentType = value.GetType();

            object obj = value;
            foreach (string propertyName in path.Split('.'))
            {
                if (currentType != null)
                {
                    PropertyInfo property = null;
                    int brackStart = propertyName.IndexOf("[");
                    int brackEnd = propertyName.IndexOf("]");

                    property = currentType.GetProperty(brackStart > 0 ? propertyName.Substring(0, brackStart) : propertyName);
                    if (property != null)
                    {
                        obj = property.GetValue(obj, null);

                        if (brackStart > 0)
                        {
                            string index = propertyName.Substring(brackStart + 1, brackEnd - brackStart - 1);
                            foreach (Type iType in obj.GetType().GetInterfaces())
                            {
                                if (iType.IsGenericType && iType.GetGenericTypeDefinition() == typeof(IDictionary<,>))
                                {
                                    obj = typeof(ReflectionUtilities).GetMethod("GetDictionaryElement")
                                                         .MakeGenericMethod(iType.GetGenericArguments())
                                                         .Invoke(null, new object[] { obj, index });
                                    break;
                                }
                                if (iType.IsGenericType && iType.GetGenericTypeDefinition() == typeof(IList<>))
                                {
                                    obj = typeof(ReflectionUtilities).GetMethod("GetListElement")
                                        .MakeGenericMethod(iType.GetGenericArguments())
                                        .Invoke(null, new object[] { obj, index });
                                    break;
                                }
                            }
                        }

                        currentType = obj?.GetType();
                    }
                    else return null;
                }
                else return null;
            }
            return obj;
        }

        public static TValue GetDictionaryElement<TKey, TValue>(IDictionary<TKey, TValue> dict, object index)
        {
            TKey key = (TKey)Convert.ChangeType(index, typeof(TKey), null);
            return dict[key];
        }

        public static T GetListElement<T>(IList<T> list, object index)
        {
            int m_Index = Convert.ToInt32(index);
            T m_T = list.Count > m_Index ? list[m_Index] : default(T);

            return m_T;
        }

        /// <summary>
        /// Lấy danh sách tất cả thuộc tính của object
        /// </summary>
        /// <param name="obj">đối tượng cần lấy danh sách thuộc tính </param>
        /// <returns>
        /// Danh sách các thuộc tính PropertyInfo[] 
        /// </returns>
        public static PropertyInfo[] GetProperties(object obj)
        {
            return obj.GetType().GetProperties();
        }
    }
}
