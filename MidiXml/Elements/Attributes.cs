using System.Xml.Linq;

namespace Developers.MidiXml.Elements
{
    /// <summary>
    /// 小節の属性
    /// </summary>
    public class Attributes : MidiElement
    {
        #region "public properties"

        private XElement? Source { get; set; } = null;
        public string MeasureNumber { get; init; } = string.Empty;
        public Key Key { get; init; } = new Key(null, 0, MidiDefs.Mode.Major);
        public Transpose Transpose { get; private set; } = new Transpose(null, 0, 0);

        #endregion

        #region "constructors"

        /// <summary>
        /// コンストラクタ(クローニング)
        /// </summary>
        /// <param name="Original"></param>
        public Attributes(Attributes Original)
        {
            this.Source = Original.Source;
            this.Key = Original.Key.Clone();
            this.Transpose = Original.Transpose.Clone();
        }

        /// <summary>
        /// コンストラクタ(XDocument版)
        /// </summary>
        /// <param name="Source"></param>
        public Attributes(XElement Source)
        {
            XAttribute? AtrMeasureNumber = Source.Parent!.Attribute("number");
            XElement? ElmKey = Source.Element("key");
            XElement? ElmTranspose = Source.Element("transpose");

            //ノード保存
            this.Source = Source;
            //親ノードのnumber属性(小節番号)を取得
            if (AtrMeasureNumber != null)
            {
                this.MeasureNumber = AtrMeasureNumber.Value ?? string.Empty;
            }
            //<key>があればセット
            if (ElmKey != null)
            {
                this.Key = new Key(ElmKey);
            }
            //<transpose>があればセット
            if (ElmTranspose != null)
            {
                this.Transpose = new Transpose(ElmTranspose);
            }
        }

        #endregion

        #region "public methods"

        /// <summary>
        /// クローンの作成
        /// </summary>
        /// <returns></returns>
        public Attributes Clone()
        {
            return new Attributes(this);
        }

        /// <summary>
        /// トランスポーズ
        /// </summary>
        /// <param name="Transposition"></param>
        public void TransposeToConcertKey(Transposition Transposition)
        {
            //キーの更新
            this.Key?.UpdateKey(Transposition);
            //移調情報の更新
            this.Transpose?.UpdateTranspose(Transposition);
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

            Dump += "<attributes>(measure=" + this.MeasureNumber + ")";
            Dump += this.Key != null ? this.Key.DebugDump() : "";
            Dump += this.Transpose != null ? this.Transpose.DebugDump() : "";
            return Dump;
        }

        #endregion
    }
}
