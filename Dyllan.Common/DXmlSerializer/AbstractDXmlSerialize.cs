using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Dyllan.Common
{
    public class AbstractDXmlSerialize : AbstractFile
    {
        protected const string REFID = "_RefId";
        protected const string ID = "_Id";
        protected const string NULL = "null";

        public AbstractDXmlSerialize(string fileName)
            : base (fileName)
        {

        }

        protected bool NeedIgnoreProperty(PropertyInfo propertyInfo)
        {
            Attribute customerAtt = propertyInfo.GetCustomAttribute(typeof(XmlIngoreAttribute));
            return customerAtt != null;
        }

        protected string GetTypeName(object obj)
        {
            Type type = obj.GetType();
            return GetTypeName(type);
        }

        protected string GetTypeName(Type type)
        {
            string result = string.Empty;

            XmlNameAttribute customerAtt = (XmlNameAttribute)type.GetCustomAttribute(typeof(XmlNameAttribute));
            if (customerAtt != null)
            {
                result = customerAtt.Name;
            }
            else
            {
                result = type.Name;
            }
            return result;
        }

        protected string GetPropertyName(PropertyInfo propertyInfo)
        {
            string result = string.Empty;

            XmlNameAttribute customerAtt = (XmlNameAttribute)propertyInfo.GetCustomAttribute(typeof(XmlNameAttribute));
            if (customerAtt != null)
            {
                result = customerAtt.Name;
            }
            else
            {
                result = propertyInfo.Name;
            }
            return result;
        }

        protected bool IsPrimitiveType(Type type)
        {
            List<Type> basicTypes = new List<Type>();
            basicTypes.Add(typeof(int));
            basicTypes.Add(typeof(string));
            basicTypes.Add(typeof(bool));
            basicTypes.Add(typeof(long));
            basicTypes.Add(typeof(uint));
            basicTypes.Add(typeof(float));
            basicTypes.Add(typeof(double));
            basicTypes.Add(typeof(Enum));

            bool result = false;

            foreach (Type t in basicTypes)
            {
                if (type == t)
                {
                    result = true;
                    break;
                }
            }
            return result;
        }

        /// <summary>
        /// Doesnot support generic, Interface type in the collection.
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        protected bool IsCollectionType(Type type)
        {
            var collectionTypes = new[] { typeof(ICollection<>), typeof(IList<>), typeof(List<>) };
            return type.IsGenericType && collectionTypes.Any(t => t.IsAssignableFrom(type.GetGenericTypeDefinition()));
        }
    }
}
