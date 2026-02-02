using System.ComponentModel;
using System.Reflection;
using System.Xml.Linq;

namespace Developers.MidiXml.Elements
{
    public class Harmony : MidiElement
    {
        public Root Root { get; init; } = new Root();
        public MidiDefs.Kind Kind { get; init; } = MidiDefs.Kind.None;
        public List<Degree> Degrees { get; init; } = [];
        public string KindString
        {
            get
            {
                string Description = string.Empty;
                FieldInfo? FieldInfo = this.Kind.GetType().GetField(this.Kind.ToString());
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
        }

        /// <summary>
        /// コンストラクタ(XDocument版)
        /// </summary>
        /// <param name="Node"></param>
        public Harmony(XElement Node)
        {
            //タグの読み取り
            XElement? RootNode = Node.Element("root");
            XElement? KindNode = Node.Element("kind");
            IEnumerable<XElement> DegreeNodes = Node.Elements("degree");
            //<root>
            if (RootNode != null)
            {
                this.Root = new Root(RootNode);
            }
            else
            {
                throw new FormatException("<harmony>: <root>: Not found.");
            }
            //<kind>
            if (KindNode != null)
            {
                string? RawKind = KindNode.Value ?? "";
                if (!MidiDefs.KindMembers.Exists(x => x.Key.Equals(RawKind, StringComparison.OrdinalIgnoreCase)))
                {
                    throw new ArgumentException("<harmony>: <kind>: Invalid value.");
                }
                this.Kind = MidiDefs.KindMembers.FirstOrDefault(x => x.Key.Equals(RawKind, StringComparison.OrdinalIgnoreCase)).Value;
            }
            else
            {
                throw new ArgumentException("<degree>: <Degree-type>: Invalid value.");
            }
            foreach (XElement degree in DegreeNodes)
            {
                Degrees.Add(new Degree(degree));
            }
        }

        /// <summary>
        /// デバック用ダンプ
        /// </summary>
        /// <returns></returns>
        public override string DebugDump()
        {
            string Dump = string.Empty;

            Dump += "<harmony>";
            Dump += Root.DebugDump();
            Dump += "<kind>" + this.Kind.ToString();
            foreach (Degree degree in Degrees)
            {
                Dump += degree.DebugDump();
            }
            return Dump;
        }
    }
}
