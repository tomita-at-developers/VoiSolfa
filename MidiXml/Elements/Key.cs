using System.Xml.Linq;

namespace Developers.MidiXml.Elements
{
    /// <summary>
    /// <key>情報(キー情報)
    /// </summary>
    public class Key : MidiElement
    {
        #region "properties"

        /// <summary>
        /// このインスタンスに対応するXElement
        /// </summary>
        public XElement? Source { get; init; } = null;
        /// <summary>
        /// ５度圏指定
        /// </summary>
        public int Fifths { get; init; } = 0;
        /// <summary>
        /// コードの種類
        /// </summary>
        public MidiDefs.Mode Mode { get; set; } = MidiDefs.Mode.Major;
        /// <summary>
        /// 調号のPithClass表見
        /// </summary>
        public PitchClass Signature { get; init; } = new PitchClass(MidiDefs.Step.C, MidiDefs.ALTER_NATURAL);

        #endregion

        #region "constructors"

        /// <summary>
        /// コンストラクタ(XDocument以外)
        /// </summary>
        /// <param name="Source"></param>
        /// <param name="Fifths"></param>
        /// <param name="Mode"></param>
        public Key(XElement? Source, int Fifths, MidiDefs.Mode Mode)
        {
            this.Source  = Source;
            this.Fifths = Fifths;
            this.Mode = Mode;
            //五度圏からステップの変換
            this.Signature = PitchUtil.FifthsToPitchClass(this.Fifths);
        }

        /// <summary>
        /// コンストラクタ(XDoxument版)
        /// </summary>
        /// <param name="Source"></param>
        /// <exception cref="ArgumentException"></exception>
        /// <exception cref="FormatException"></exception>
        public Key(XElement Source)
        {
            //ソース読み取り<key>系
            XElement? ElmFifths = Source.Element("fifths");
            XElement? ElmMode = Source.Element("mode");

            //ノード保存
            this.Source = Source;
            //<fifths>
            if (ElmFifths != null)
            {
                //数字化チェック
                if (!int.TryParse(ElmFifths.Value, out int RawKeyFifths))
                {
                    throw new ArgumentException("<attributes><key>: <fifths>: Invalid value.");
                }
                //サポートチェック(シャープ系は<F#>まで、フラット系は<Gb>まで
                if (RawKeyFifths < -7 || 7 < RawKeyFifths)
                {
                    throw new ArgumentException("<attributes><key>: <fifths>: [" + RawKeyFifths.ToString() + "] is unspported.");
                }
                this.Fifths = RawKeyFifths;
            }
            else
            {
                throw new FormatException("<attributes><key>: <fifths>: Not found.");
            }
            //<mode>
            if (ElmMode != null)
            {
                string RawKeyMode = ElmMode.Value ?? "";
                if (!MidiDefs.ModeMembers.Exists(x => x.Key.Equals(RawKeyMode, StringComparison.OrdinalIgnoreCase)))
                {
                    throw new ArgumentException("< attributes >< key >: <mode>: Invalid value.");
                }
                this.Mode = MidiDefs.ModeMembers.FirstOrDefault(x => x.Key.Equals(RawKeyMode, StringComparison.CurrentCultureIgnoreCase)).Value;
            }
            //五度圏からステップの変換
            this.Signature = PitchUtil.FifthsToPitchClass(this.Fifths);
        }

        #endregion

        #region "public methods"

        /// <summary>
        /// クローンの作成
        /// </summary>
        /// <returns></returns>
        public Key Clone()
        {
            return  new Key(this.Source, this.Fifths, this.Mode);
        }

        /// <summary>
        /// 移調楽器向けの表記を実音表記に転調する
        /// </summary>
        public void TransposeToConcertKey()
        {

        }

        #endregion

        #region "private methods"

        #endregion

        #region "debug methods"

        /// <summary>
        /// デバック用ダンプ
        /// </summary>
        /// <returns></returns>
        public override string DebugDump()
        {
            string Dump = string.Empty;

            Dump += "<key>";
            Dump += "<fifths>" + Fifths.ToString();
            Dump += "<mode>" + Mode.ToString();
            Dump += "[KeySignature}" + Signature.Step.ToString() + "(" + Signature.Alter.ToString() + ")";
            return Dump;
        }

        #endregion
    }
}
