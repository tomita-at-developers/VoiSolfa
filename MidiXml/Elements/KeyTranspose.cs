using System.Xml.Linq;

namespace Developers.MidiXml.Elements
{
    /// <summary>
    /// キー情報クラス
    /// </summary>
    public class KeyTranspose : MidiElement
    {
        public int Fifths { get; init; } = 0;
        public MidiDefs.Mode Mode { get; set; } = MidiDefs.Mode.Major;
        public int TransposeDiatonic { get; init; } = 0;
        public int TransposeChromatic { get; init; } = 0;
        public PitchClass InstrumentalKey { get; init; } = new PitchClass();
        public PitchClass ConcertKey { get; init; } = new PitchClass();

        /// <summary>
        /// コンストラクタ(デフォルト)
        /// </summary>
        public KeyTranspose()
        {
        }

        /// <summary>
        /// コンストラクタ(XDoxument版)
        /// </summary>
        /// <param name="Node"></param>
        /// <exception cref="ArgumentException"></exception>
        /// <exception cref="FormatException"></exception>
        public KeyTranspose(XElement Node)
        {
            //参照ノード
            XElement? KeyFifthsNode = null;
            XElement? KeyModeNode = null;
            XElement? TransDiatonicNode = null;
            XElement? TransChromaticNode = null;

            //<key>
            XElement? KeyNode = Node.Element("key");
            if (KeyNode != null)
            {
                KeyFifthsNode = KeyNode.Element("fifths");
                KeyModeNode = KeyNode.Element("mode");
            }
            //<transpose>
            XElement? TransposeNode = Node.Element("transpose");
            if (TransposeNode != null)
            {
                TransDiatonicNode = TransposeNode.Element("diatonic");
                TransChromaticNode = TransposeNode.Element("chromatic");
            }
            //<key><fifths>
            if (KeyFifthsNode != null)
            {
                if (!int.TryParse(KeyFifthsNode.Value, out int RawKeyFifths))
                {
                    throw new ArgumentException("<attributes><key>: <fifths>: Invalid value.");
                }
                this.Fifths = RawKeyFifths;
            }
            else
            {
                throw new FormatException("<attributes><key>: <fifths>: Not found.");
            }
            //<key><mode>
            if (KeyModeNode != null)
            {
                string RawKeyMode = KeyModeNode.Value ?? "";
                if (!MidiDefs.ModeMembers.Exists(x => x.Key.Equals(RawKeyMode, StringComparison.OrdinalIgnoreCase)))
                {
                    throw new ArgumentException("< attributes >< key >: <mode>: Invalid value.");
                }
                this.Mode = MidiDefs.ModeMembers.FirstOrDefault(x => x.Key.Equals(RawKeyMode, StringComparison.CurrentCultureIgnoreCase)).Value;
            }
            //<transpose><diatonic>
            if (TransDiatonicNode != null)
            {
                if (!int.TryParse(TransDiatonicNode.Value, out int RawTansDiatonicInt))
                {
                    throw new ArgumentException("<attributes><transpose>: <diatonic>: Invalid value.");
                }
                this.TransposeDiatonic = RawTansDiatonicInt;
            }
            //<transpose><chromatic>
            if (TransChromaticNode != null)
            {
                if (!int.TryParse(TransChromaticNode.Value, out int RawTansChromaticInt))
                {
                    throw new ArgumentException("<attributes><transpose>: <chromatic>: Invalid value.");
                }
                this.TransposeChromatic = RawTansChromaticInt;
            }
            //五度圏からステップの変換
            this.InstrumentalKey = PitchUtil.FifthsToPitchClass(this.Fifths);
            //楽器キーからコンサートキーに変換
            this.ConcertKey = this.InstrumentalKey.GetAlteredPitchClass(this.TransposeChromatic);
        }

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
            Dump += "<transpose>";
            Dump += "<diatonic>" + TransposeDiatonic.ToString();
            Dump += "<crhomatic>" + TransposeChromatic.ToString();
            Dump += "[Instrumental}" + InstrumentalKey.Step.ToString() + "(" + InstrumentalKey.Alter.ToString() + ")";
            Dump += "[Concert}" + ConcertKey.Step.ToString() + "(" + ConcertKey.Alter.ToString() + ")";
            return Dump;
        }
    }
}
