using System.Net.Http.Headers;
using System.Xml.Linq;

namespace Developers.MidiXml.Elements
{
    /// <summary>
    /// <lyric>情報(歌詞情報)
    /// </summary>
    public class Lyric : MidiElement
    {
        #region "public Properties"

        public int? Number { get; init; } = 0;
        public MidiDefs.Syllabic? Syllabic { get; init; } = MidiDefs.Syllabic.Single;
        public string Text { get; init; } = string.Empty;
        public string SyllabicString
        {
            get
            {
                string RetVal = string.Empty;
                if (Syllabic != null)
                {
                    RetVal = GetXmlString(this.Syllabic);
                }
                return RetVal;
            }
        }

        #endregion

        #region "constructors"

        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="number"></param>
        /// <param name="syllabic"></param>
        /// <param name="text"></param>
        public Lyric(int? Number, MidiDefs.Syllabic Syllabic, string Text)
        {
            this.Number = Number;
            this.Syllabic = Syllabic;
            this.Text = Text;
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

        #endregion

        /// <summary>
        /// XElementにシリアライズ
        /// </summary>
        /// <param name="Alter"></param>
        public XElement Serialize()
        {
            XElement RetVal = new XElement("lyric");
            RetVal.SetAttributeValue("number", this.Number.ToString());
            RetVal.Add(new XElement("syllabic", this.SyllabicString));
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
            Dump += "<syllabic>" + this.Syllabic.ToString();
            Dump += "<text>" + Text;
            return Dump;
        }
    }
}
