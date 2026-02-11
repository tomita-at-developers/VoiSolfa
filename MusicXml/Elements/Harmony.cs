using System.Xml.Linq;

namespace Developers.MusicXml.Elements
{
    /// <summary>
    /// <harmony>情報(コード情報)
    /// </summary>
    public class Harmony : MidiElement
    {
        #region "private properties"

        /// <summary>
        /// このインスタンスに対応するXElement
        /// </summary>
        private XElement Source { get; init; }
        /// <summary>
        /// 親<measure>の番号
        /// </summary>
        private string MeasureNumber { get; init; } = string.Empty;
        /// <summary>
        /// XDocumentから削除されたことを示すフラグ
        /// </summary>
        private bool Removed { get; set; } = false;

        #endregion

        #region "public properties"

        public Root Root { get; init; } = new Root(MidiDefs.Step.C, MidiDefs.ALTER_NATURAL);
        public MidiDefs.Kind Kind { get; init; } = MidiDefs.Kind.None;
        public List<Degree> Degrees { get; init; } = [];
        public string KindString
        {
            get
            {
                return MidiDefs.GetEnumDescription(this.Kind);
            }
        }
        public Analysis? Analysis { get; set; } = null!;

        #endregion

        #region "constructors"

        /// <summary>
        /// コンストラクタ(XDocument版)
        /// </summary>
        /// <param name="Source"></param>
        public Harmony(XElement Source)
        {
            //ソース読み取り
            XAttribute? AtrMeasureNumber = Source.Parent!.Attribute("number");
            XElement? ElmRoot = Source.Element("root");
            XElement? ElmKind = Source.Element("kind");
            IEnumerable<XElement> ElmDegrees = Source.Elements("degree");

            //ノード保存
            this.Source = Source;
            //削除済フラグ(初期値は未削除)
            this.Removed = false;
            //親ノードのnumber属性(小節番号)を取得
            if (AtrMeasureNumber != null)
            {
                this.MeasureNumber = AtrMeasureNumber.Value ?? string.Empty;
            }
            //<root>
            if (ElmRoot != null)
            {
                this.Root = new Root(ElmRoot);
            }
            else
            {
                throw new FormatException("<harmony>: <root>: Not found.");
            }
            //<kind>
            if (ElmKind != null)
            {
                string? RawKind = ElmKind.Value ?? "";
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
            foreach (XElement ElmDegree in ElmDegrees)
            {
                Degrees.Add(new Degree(ElmDegree));
            }
        }

        #endregion

        #region "public methods"

        /// <summary>
        /// トランスポーズ
        /// </summary>
        /// <param name="Transposition"></param>
        public void Transpose(Transposition Transposition)
        {
            //移調してXDocumentに反映
            this.Root.Transpose(Transposition, this.Analysis!.Context);
            UpdateXmlRoot();
        }

        /// <summary>
        /// XDocumentから削除する
        /// </summary>
        public void RemoveFromDocument()
        {
            RemoveXmlharmony();
            this.Removed = true;
        }

        #endregion

        #region "private methods"

        /// <summary>
        /// rootの更新
        /// </summary>
        private void UpdateXmlRoot()
        {
            //XDocumentに<pitch>が存在する場合のみ処理する
            XElement? RootElm = Source.Element("root");
            if (RootElm != null)
            {
                //一旦削除して追加
                RootElm.Remove();
                Source.Add(this.Root!.Serialize());
            }
        }

        /// <summary>
        /// <harmony>の削除
        /// </summary>
        private void RemoveXmlharmony()
        {
            //XElementを削除
            Source.Remove();
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

            Dump += "<measure id=" + this.MeasureNumber + "><harmony>";
            Dump += "[Removed=" + this.Removed.ToString() + "]";
            Dump += Root.DebugDump();
            Dump += "<kind>" + this.Kind.ToString();
            foreach (Degree degree in Degrees)
            {
                Dump += degree.DebugDump();
            }
            if (this.Analysis != null)
            {
                Dump += this.Analysis.DebugDump();
            }
            return Dump;
        }

        #endregion
    }
}
