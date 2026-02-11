using System.Xml.Linq;

namespace Developers.MusicXml.Elements
{
    /// <summary>
    /// <transpose>情報
    /// </summary>
    public class Transpose : MidiElement
    {
        #region "properties"

        /// <summary>
        /// このインスタンスに対応するXElement
        /// </summary>
        public XElement? Source { get; init; } = null;
        /// <summary>
        /// 転調のダイアトニック表現(Stepのみ)
        /// </summary>
        public int? Diatonic { get; private set; } = null;
        /// <summary>
        /// 転調のクロマチック表現
        /// </summary>
        public int Chromatic { get; private set; } = 0;
        /// <summary>
        /// オクターブ移調
        /// </summary>
        public int? OctaveChange { get; private set; } = null;
        /// <summary>
        /// オクターブ上
        /// </summary>
        public MidiDefs.YesNo? Double { get; private set; } = null;

        #endregion

        #region "constructors"

        /// <summary>
        /// コンストラクタ(XDocument以外)
        /// </summary>
        /// <param name="Source"></param>
        /// <param name="Diatonic"></param>
        /// <param name="Chromatic"></param>
        /// <param name="OctaveChange"></param>
        /// <param name="Double"></param>
        public Transpose(XElement? Source, int? Diatonic, int Chromatic, int? OctaveChange, MidiDefs.YesNo? Double)
        {
            this.Source = Source;
            this.Diatonic = Diatonic;
            this.Chromatic = Chromatic;
            this.OctaveChange = OctaveChange;
            this.Double = Double;
        }

        /// <summary>
        /// コンストラクタ(XDoxument版)
        /// </summary>
        /// <param name="SourceElm"></param>
        /// <exception cref="ArgumentException"></exception>
        /// <exception cref="FormatException"></exception>
        public Transpose(XElement SourceElm)
        {
            //ソース読み取り
            XElement? ElmDiatonic = SourceElm.Element("diatonic");
            XElement? ElmChromatic = SourceElm.Element("chromatic");
            XElement? ElmOctaveChange = SourceElm.Element("octave-change");
            XElement? ElmDouble = SourceElm.Element("double");

            //ノード保存
            this.Source = SourceElm;
            //<transpose><diatonic>
            if (ElmDiatonic != null)
            {
                if (!int.TryParse(ElmDiatonic.Value, out int RawTansDiatonicInt))
                {
                    throw new ArgumentException("<attributes><transpose>: <diatonic>: Invalid value.");
                }
                this.Diatonic = RawTansDiatonicInt;
            }
            //<transpose><chromatic>
            if (ElmChromatic != null)
            {
                if (!int.TryParse(ElmChromatic.Value, out int RawTansChromaticInt))
                {
                    throw new ArgumentException("<attributes><transpose>: <chromatic>: Invalid value.");
                }
                this.Chromatic = RawTansChromaticInt;
            }
            //<transpose><octave-change>
            if (ElmOctaveChange != null)
            {
                if (!int.TryParse(ElmOctaveChange.Value, out int RawOctaveChangeInt))
                {
                    throw new ArgumentException("<attributes><transpose>: <octave-change>: Invalid value.");
                }
                this.OctaveChange = RawOctaveChangeInt;
            }
            //<transpose><octave-change>
            if (ElmDouble != null)
            {
                string RawDouble = ElmDouble.Value ?? "";
                //値の正当性チェック
                if (!MidiDefs.DoubleMembers.Exists(x => x.Key.Equals(RawDouble, StringComparison.OrdinalIgnoreCase)))
                {
                    throw new ArgumentException("<attributes><transpose>: <double>: Invalid or unsupported value.");
                }
                //値のセット
                this.Double = MidiDefs.DoubleMembers.FirstOrDefault(x => x.Key.Equals(RawDouble, StringComparison.OrdinalIgnoreCase)).Value;
            }
        }

        #endregion

        #region "public methods"

        /// <summary>
        /// クローンの作成
        /// </summary>
        /// <returns></returns>
        public Transpose Clone()
        {
            return new Transpose(this.Source, this.Diatonic, this.Chromatic, this.OctaveChange, this.Double);
        }

        /// <summary>
        /// トランスポーズ情報の更新
        /// </summary>
        /// <param name="Transposition"></param>
        public void UpdateTranspose(Transposition Transposition)
        {
            //移調なしに更新
            this.Diatonic = Transposition.TransposedTranspose.Diatonic;
            this.Chromatic = Transposition.TransposedTranspose.Chromatic;
            //XDocumentに反映
            UpdateXml();
        }

        #endregion

        #region "private methods"

        /// <summary>
        /// XDocumentへの反映
        /// </summary>
        /// <exception cref="NullReferenceException"></exception>
        private void UpdateXml()
        {
            if (this.Source != null)
            {
                this.Source.SetElementValue("diatonic", this.Diatonic.ToString());
                this.Source.SetElementValue("chromatic", this.Chromatic.ToString());
            }
            else
            {
                throw new NullReferenceException("Instance has no source XElement.");
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

            Dump += "<transpose>";
            Dump += "<diatonic>" + this.Diatonic.ToString();
            Dump += "<chromatic>" + this.Chromatic.ToString();
            Dump += "<octave-change>" + this.OctaveChange.ToString();
            Dump += "<double>" + this.Double.ToString();
            return Dump;
        }

        #endregion
    }
}
