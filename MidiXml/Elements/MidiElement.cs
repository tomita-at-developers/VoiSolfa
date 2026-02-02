using System.ComponentModel;
using System.Reflection;

namespace Developers.MidiXml.Elements
{
    abstract public class MidiElement
    {

        /// <summary>
        /// numの要素からDescription属性の文字列を取得する
        /// </summary>
        /// <param name="Value"></param>
        /// <returns></returns>
        public string GetXmlString(object Value)
        {
            string Description = string.Empty;
            FieldInfo? FieldInfo = Value.GetType().GetField(Value.ToString()!);
            if (FieldInfo != null)
            {
                Attribute? attr = Attribute.GetCustomAttribute(FieldInfo, typeof(DescriptionAttribute));
                if (attr != null)
                {
                    DescriptionAttribute descAttr = (DescriptionAttribute)attr;
                    Description = descAttr.Description;
                }
            }
            return Description;
        }

        abstract public string DebugDump();
    }
}
