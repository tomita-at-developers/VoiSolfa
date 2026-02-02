using System.Xml.Linq;

namespace Developers.MidiXml.Elements
{
    public class Lyric : MidiElement
    {
        public int? Number { get; init; } = null;
        public MidiDefs.Syllabic? Syllabic { get; init; } = null;
        public string Text { get; init; } = string.Empty;

        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="number"></param>
        /// <param name="syllabic"></param>
        /// <param name="text"></param>
        public Lyric(int? number, MidiDefs.Syllabic? syllabic, string text)
        {
            this.Number = number;
            this.Syllabic = syllabic;
            this.Text = text;
        }

        /// <summary>
        /// コンストラクタ(XDocument版)
        /// </summary>
        /// <param name="Node"></param>
        /// <exception cref="ArgumentException"></exception>
        /// <exception cref="FormatException"></exception>
        public Lyric(XElement Node)
        {
            int RawNumberInt = -1;

            //number属性
            if (Node.Attribute("number") != null)
            {
                string RawNumber = Node.Attribute("number")!.Value ?? "";
                //値の正当性チェック
                if (RawNumber.Length > 0)
                {
                    if (!int.TryParse(RawNumber, out RawNumberInt))
                    {
                        throw new ArgumentException("<lyrics number>: Invalid value.");
                    }
                    this.Number = RawNumberInt;
                }
            }
            //<sylabric>
            XElement? SyllabicNode = Node.Element("syllabic");
            if (SyllabicNode != null)
            {
                string RawSyllabic = SyllabicNode.Value ?? "";
                if (!MidiDefs.SyllabicMembers.Exists(x => x.Key.Equals(RawSyllabic, StringComparison.OrdinalIgnoreCase)))
                {
                    throw new ArgumentException("<lyrics>: <syllabic>: Invalid value.");
                }
                this.Syllabic = MidiDefs.SyllabicMembers.FirstOrDefault(x => x.Key.Equals(RawSyllabic, StringComparison.CurrentCultureIgnoreCase)).Value;
            }
            else
            {
                throw new FormatException("<lyrics>: <syllabic>: Not found.");
            }
            //<text>
            XElement? TextNode = Node.Element("text");
            if (TextNode != null)
            {
                this.Text = TextNode.Value ?? "";
            }
        }

        /// <summary>
        /// デバック用ダンプ
        /// </summary>
        /// <returns></returns>
        public override string DebugDump()
        {
            string Dump = string.Empty;

            Dump += "<lyric" + (Number != null ? " number=" + Number.ToString() : "") + ">";
            Dump += "<syllabic>" + Syllabic?.ToString();
            Dump += "<text>" + Text;
            return Dump;
        }
    }
}
