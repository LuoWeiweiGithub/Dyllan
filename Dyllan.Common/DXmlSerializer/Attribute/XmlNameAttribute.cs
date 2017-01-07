using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dyllan.Common
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Property, Inherited = false, AllowMultiple = false)]
    public class XmlNameAttribute : Attribute
    {
        private string name;

        public string Name
        {
            get
            {
                return name;
            }
        }

        public XmlNameAttribute(string name)
        {
            this.name = name;
        }
    }
}
