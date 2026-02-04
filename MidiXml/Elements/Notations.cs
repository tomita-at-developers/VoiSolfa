using System.Xml.Linq;

namespace Developers.MidiXml.Elements
{
    /// <summary>
    /// <notations>情報(tie, tupletのみ)
    /// </summary>
    public class Notations : MidiElement
    {
        #region "public properties"

        public MidiDefs.TiedType? TiedType { get; init; } = null;
        public MidiDefs.StartStop? TupletType { get; init; } = null;

        #endregion

        #region "constructors"

        /// <summary>
        /// コンストラクタ(XDocument版)
        /// </summary>
        /// <param name="Source"></param>
        /// <exception cref="ArgumentException"></exception>
        /// <exception cref="FormatException"></exception>
        public Notations(XElement Source)
        {
            //ソース読み取り
            XElement? ElmTied = Source.Element("tied");
            XElement? ElmTuplet = Source.Element("tuplet");

            //<tied>
            if (ElmTied != null)
            {
                if (ElmTied.Attribute("type") != null)
                {
                    string RawTiedType = ElmTied.Attribute("type")!.Value ?? "";
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
            if (ElmTuplet != null)
            {
                if (ElmTuplet.Attribute("type") != null)
                {
                    string RawTupletType = ElmTuplet.Attribute("type")!.Value ?? "";
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
                if (!Source.HasElements)
                {
                    throw new FormatException("<notations>: No ekement found.");
                }
            }
        }

        #endregion

        #region "debug methods"

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

        #endregion
    }
}
