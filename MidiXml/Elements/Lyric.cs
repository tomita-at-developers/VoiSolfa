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
        /// <param name="SourceElm"></param>
        /// <exception cref="ArgumentException"></exception>
        /// <exception cref="FormatException"></exception>
        public Lyric(XElement SourceElm)
        {
            //ソース読み取り
            XAttribute? NumberAtr = SourceElm.Attribute("number");
            XElement? SyllabicElm = SourceElm.Element("syllabic");
            XElement? TextElm = SourceElm.Element("text");

            //<lyric number>
            if (NumberAtr != null)
            {
                string RawNumber = NumberAtr.Value;
                //値の正当性チェック
                if (RawNumber.Length > 0)
                {
                    int RawNumberInt = -1;
                    if (!int.TryParse(RawNumber, out RawNumberInt))
                    {
                        throw new ArgumentException("<lyrics number>: Invalid value.");
                    }
                    this.Number = RawNumberInt;
                }
            }
            //<sylabric>
            if (SyllabicElm != null)
            {
                string RawSyllabic = SyllabicElm.Value ?? "";
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
            if (TextElm != null)
            {
                this.Text = TextElm.Value ?? "";
            }
        }

        /// <summary>
        /// XElementにシリアライズ
        /// </summary>
        /// <param name="Alter"></param>
        public XElement Serialize()
        {
            XElement RetVal = new XElement("lyric");
            RetVal.SetAttributeValue("number", this.Number.ToString());
            RetVal.Add(new XElement("syllabic", GetXmlString(this.Syllabic!)));
            RetVal.Add(new XElement("text", this.Text));
            return RetVal;
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
