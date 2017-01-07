using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using System.Xml;

namespace Dyllan.Common
{
    public class DXmlSerializer<T> : AbstractDXmlSerialize where T : class
    {
        private T serializeObj;
        private XmlDocument doc = new XmlDocument();
        private Dictionary<Object, Guid> objectIdentify = new Dictionary<object, Guid>();


        public DXmlSerializer(string fileName, T obj)
            : base (fileName)
        {
            this.serializeObj = obj;
            XmlDeclaration xmlDeclaration = doc.CreateXmlDeclaration("1.0", "UTF-8", null);
            doc.InsertBefore(xmlDeclaration, doc.DocumentElement);
        }

        private XmlElement CreateElement(string name, object obj, out bool isNeedCreated)
        {
            XmlElement element = doc.CreateElement(name);
            isNeedCreated = true;
            if (obj == null)
            {
                element.SetAttribute(ID, NULL);
                isNeedCreated = false;
            }
            else if (!IsPrimitiveType(obj.GetType()))
            {
                if (objectIdentify.ContainsKey(obj))
                {
                    element.SetAttribute(REFID, objectIdentify[obj].ToString("N"));
                    isNeedCreated = false;
                }
                else
                {
                    Guid newId = Guid.NewGuid();
                    element.SetAttribute(ID, newId.ToString("N"));
                    objectIdentify.Add(obj, newId);
                }
            }
            return element;
        }

        public void Serialize()
        {
            String typeName = GetTypeName(serializeObj);
            bool isNeedCreated;
            XmlElement rootEle = CreateElement(typeName, serializeObj, out isNeedCreated);
            doc.AppendChild(rootEle);
            AppendReferenceType(rootEle, serializeObj);
            Utils.WriteToFile(FileName, doc.OuterXml, false);
        }

        private void AppendContent(XmlElement parent, object obj)
        {
            Type objType = obj.GetType();
            if (IsPrimitiveType(objType))
            {
                AppendPrimitiveType(parent, obj);
            }
            else if (IsCollectionType(objType))
            {
                AppendCollectionType(parent, (ICollection)obj);
            }
            else
            {
                AppendReferenceType(parent, obj);
            }
        }

        private void AppendPrimitiveType(XmlElement parent, object obj)
        {
            XmlText text = doc.CreateTextNode(obj.ToString());
            parent.AppendChild(text);
        }

        private void AppendCollectionType(XmlElement parent, ICollection itemEnum)
        {
            bool isNeedCreated = false;
            foreach (object item in itemEnum)
            {
                XmlElement childElement = CreateElement(GetTypeName(item), item, out isNeedCreated);
                if (isNeedCreated)
                    AppendContent(childElement, item);
                parent.AppendChild(childElement);
            }
        }
        
        private void AppendReferenceType(XmlElement parent, object obj)
        {
            PropertyInfo[] properties = obj.GetType().GetProperties();
            bool isNeedCreated = false;
            foreach (PropertyInfo propertyInfo in properties)
            {
                if (NeedIgnoreProperty(propertyInfo))
                {
                    continue;
                }
                object propertyObj = propertyInfo.GetValue(obj);

                XmlElement element = CreateElement(GetPropertyName(propertyInfo), propertyObj, out isNeedCreated);
                if (isNeedCreated)
                    AppendContent(element, propertyObj);
                parent.AppendChild(element);
            }
        }
    }
}
