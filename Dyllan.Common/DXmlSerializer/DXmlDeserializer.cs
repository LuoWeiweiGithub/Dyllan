using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace Dyllan.Common
{
    public class DXmlDeserializer<T> : AbstractDXmlSerialize where T : class
    {
        private T deserializeObj;
        private XmlDocument doc;
        private Dictionary<string, Object> objectIdentify = new Dictionary<string, Object>();

        public DXmlDeserializer(string fileName)
            : base(fileName)
        {
            doc = new XmlDocument();
            doc.Load(fileName);
        }

        public T Deserialize()
        {
            string path = typeof(T).Name;
            deserializeObj = (T)ExtractReference(path, typeof(T));
            return deserializeObj;
        }

        private object ExtractContent(string path, Type type)
        {
            object result = null;
            if (IsPrimitiveType(type))
            {
                result = ExtractPrimitiveType(path, type);
            }
            else if (IsCollectionType(type))
            {
                result = ExtractCollectionType(path, type);
            }
            else
            {
                result = ExtractReference(path, type);
            }
            return result;
        }

        /// <summary>
        /// Support integer, float, double, enum, string.
        /// </summary>
        /// <param name="path"></param>
        /// <param name="type"></param>
        /// <returns></returns>
        private object ExtractPrimitiveType(string path, Type type)
        {
            object result = null;
            string content = doc.SelectSingleNode(path).InnerText;
            if (type == typeof(string))
            {
                XmlElement element = (XmlElement)doc.SelectSingleNode(path);
                if (element != null && GetID(element) == null)
                {
                    result = null;
                }
                else
                {
                    result = content;
                }
            }
            else if (type == typeof(int))
            {
                result = int.Parse(content);
            }
            else if (type == typeof(Enum))
            {
                result = Enum.Parse(type, content);
            }
            else if (type == typeof(bool))
            {
                result = bool.Parse(content);
            }
            else if (type == typeof(double))
            {
                result = double.Parse(content);
            }
            else if (type == typeof(long))
            {
                result = long.Parse(content);
            }
            else if (type == typeof(float))
            {
                result = float.Parse(content);
            }
            else if (type == typeof(uint))
            {
                result = uint.Parse(content);
            }
            else
            {
                // nothing to do.
            }
            return result;
        }

        private object ExtractCollectionType(string path, Type type)
        {
            object result = null;
            XmlElement element = (XmlElement)doc.SelectSingleNode(path);
            bool isGot = TryGetReferanceObject(element, out result);
            if (isGot)
            {
                return result;
            }

            result = CreateObject(element, type);
            XmlNode parentNode = doc.SelectSingleNode(path);
            XmlNodeList nodeList = parentNode.ChildNodes;
            if (nodeList != null && nodeList.Count > 0)
            {
                Type itemBaseType = type.GetGenericArguments()[0];
                string itemBaseTypeName = GetTypeName(itemBaseType);
                var addMethod = type.GetMethod("Add", new Type[] { itemBaseType});
                if (itemBaseType.IsValueType || itemBaseType.IsSealed)
                {
                    foreach (XmlNode childNode in nodeList)
                    {
                        XmlElement childElement = childNode as XmlElement;
                        if (childElement == null)
                            continue;

                        string childPath = string.Format("{0}/{1}[@{2}='{3}']", path, itemBaseTypeName, ID, GetID(childElement));
                        object childObj = ExtractContent(childPath, itemBaseType);
                        addMethod.Invoke(result, new object[] { childObj });
                    }
                }
                else
                {
                    List<Type> childrenTypes = (from t in Assembly.GetAssembly(itemBaseType).GetTypes() where t.IsSubclassOf(itemBaseType) select t).ToList();
                    foreach (XmlNode childNode in nodeList)
                    {
                        XmlElement childElement = childNode as XmlElement;
                        if (childElement == null)
                            continue;
                        Type itemConcreteType = itemBaseType;
                        
                        if (!itemBaseTypeName.Equals(childElement.Name))
                        {
                            itemConcreteType = childrenTypes.First(t => GetTypeName(t).Equals(childElement.Name));
                        }

                        string childPath = string.Format("{0}/{1}[@{2}='{3}']", path, GetTypeName(itemConcreteType), ID, GetID(childElement));
                        object childObj = ExtractContent(childPath, itemConcreteType);
                        addMethod.Invoke(result, new object[] { childObj });
                    }
                }
            }
            return result;
        }

        private object ExtractReference(string path, Type type)
        {
            XmlElement element = (XmlElement)doc.SelectSingleNode(path);

            object obj = null;
            bool isGot = TryGetReferanceObject(element, out obj);
            if (isGot)
            {
                return obj;
            }
            obj = CreateObject(element, type);
            PropertyInfo[] properties = type.GetProperties();

            foreach (PropertyInfo propertyInfo in properties)
            {
                if (NeedIgnoreProperty(propertyInfo))
                {
                    continue;
                }
                object propertyObj = ExtractContent(GetXPath(path, GetPropertyName(propertyInfo)), propertyInfo.PropertyType);
                propertyInfo.SetMethod.Invoke(obj, new object[] { propertyObj });
            }

            return obj;
        }

        private bool TryGetReferanceObject(XmlElement element, out object obj)
        {
            bool result = false;
            obj = null;
            XmlAttribute att = element.GetAttributeNode(REFID);

            if (att != null)
            {
                string id = att.InnerText;
                obj = objectIdentify[id];
                result = true;
            }
            else
            {
                string id = GetID(element);
                if (id == null)
                {
                    obj = null;
                    result = true;
                }
            }

            return result;
        }

        private string GetXPath(string path, string name)
        {
            return string.Format("{0}/{1}", path, name);
        }

        private string GetID(XmlElement element)
        {
            XmlAttribute att = element.GetAttributeNode(ID);
            return att == null ? "" : att.InnerText;
        }

        private object CreateObject(XmlElement element, Type type)
        {
            object obj = Activator.CreateInstance(type);
            objectIdentify.Add(GetID(element), obj);
            return obj;
        }
    }
}
