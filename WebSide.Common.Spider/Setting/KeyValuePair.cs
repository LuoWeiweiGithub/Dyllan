using Dyllan.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebSide.Common.Spider
{
    public class KeyValuePair
    {
        private string key;
        private string value;

        public KeyValuePair()
        { }

        public KeyValuePair(string key, string value)
        {
            this.key = key;
            this.value = value;
        }

        [XmlName("ReplaceRegex")]
        public string Key
        {
            get
            {
                return key;
            }

            set
            {
                key = value;
            }
        }

        [XmlName("Replacement")]
        public string Value
        {
            get
            {
                return value;
            }

            set
            {
                this.value = value;
            }
        }
    }
}
