using System.Xml.Linq;

namespace Developers.MidiXml.Elements
{
    /// <summary>
    /// <notations>の解析
    /// </summary>
    public class Notations : MidiElement
    {
        public MidiDefs.TiedType? TiedType { get; init; } = null;
        public MidiDefs.StartStop? TupletType { get; init; } = null;


        /// <summary>
        /// コンストラクタ(XDocument版)
        /// </summary>
        /// <param name="SourceElm"></param>
        /// <exception cref="ArgumentException"></exception>
        /// <exception cref="FormatException"></exception>
        public Notations(XElement SourceElm)
        {
            //ソース読み取り
            XElement? TiedElm = SourceElm.Element("tied");
            XElement? TupletElm = SourceElm.Element("tuplet");

            //<tied>
            if (TiedElm != null)
            {
                if (TiedElm.Attribute("type") != null)
                {
                    string RawTiedType = TiedElm.Attribute("type")!.Value ?? "";
                    //値の正当性チェック
                    if (!MidiDefs.TiedTypeMembers.Exists(x => x.Key.Equals(RawTiedType, StringComparison.OrdinalIgnoreCase)))
                    {
                        throw new ArgumentException("<notification>: <tied type>: Invalid value.");
                    }
                    //値のセット
                    this.TiedType = MidiDefs.TiedTypeMembers.FirstOrDefault(x => x.Key.Equals(RawTiedType, StringComparison.OrdinalIgnoreCase)).Value;
                }
                else
                {
                    throw new FormatException("<notification><tied type>: Not found.");
                }
            }
            //<tuplet>
            if (TupletElm != null)
            {
                if (TupletElm.Attribute("type") != null)
                {
                    string RawTupletType = TupletElm.Attribute("type")!.Value ?? "";
                    //値の正当性チェック
                    if (!MidiDefs.TupletTypeMembers.Exists(x => x.Key.Equals(RawTupletType, StringComparison.OrdinalIgnoreCase)))
                    {
                        throw new ArgumentException("<notification>: <tuplet type>: Invalid value.");
                    }
                    //値のセット
                    this.TupletType = MidiDefs.TupletTypeMembers.FirstOrDefault(x => x.Key.Equals(RawTupletType, StringComparison.OrdinalIgnoreCase)).Value;
                }
                else
                {
                    throw new FormatException("<notification><tied type>: Not found.");
                }
            }
            else
            {
                if (!SourceElm.HasElements)
                {
                    throw new FormatException("<notations>: No ekement found.");
                }
            }
        }

        /// <summary>
        /// デバック用ダンプ
        /// </summary>
        /// <returns></returns>
        public override string DebugDump()
        {
            string Dump = string.Empty;

            Dump += "<notations>";
            Dump += (TiedType != null ? "<tied type=" + TiedType.ToString() + ">" : "");
            Dump += (TupletType != null ? "<tuplet type=" + TupletType.ToString() + ">" : "");
            return Dump;
        }
    }
}
