using System.Xml.Linq;

namespace Developers.MidiXml.Elements
{
    /// <summary>
    /// <pitch>の解析
    /// </summary>
    public class Pitch : MidiElement
    {
        #region "properties"

        /// <summary>
        /// 指定されたステップ
        /// </summary>
        public MidiDefs.Step RawStep { get; init; } = MidiDefs.Step.C;
        /// <summary>
        /// 指定されたオクターブ
        /// </summary>
        public int RawOctave { get; init; } = MidiDefs.OCTAVE_CENTER;
        /// <summary>
        /// 指定された半音操作
        /// </summary>
        public int RawAlter { get; init; } = MidiDefs.ALTER_NATURAL;
        /// <summary>
        /// ステップ(実音)
        /// </summary>
        public MidiDefs.Step Step { get; init; } = MidiDefs.Step.C;
        /// <summary>
        /// オクターブ(実音)
        /// </summary>
        public int Octave { get; private set; } = MidiDefs.OCTAVE_CENTER;
        /// <summary>
        /// 半音操作(実音)
        /// </summary>
        public int Alter { get; init; } = MidiDefs.ALTER_NATURAL;
        /// <summary>
        /// ピッチクラスインスタンス
        /// </summary>
        public PitchClass PitchClass
        {
            get
            {
                return new PitchClass(this.Step, this.Alter);
            }
        }

        #endregion

        #region "constructors"

        /// <summary>
        /// コンストラクタ(デフォルト)
        /// </summary>
        public Pitch()
            : this(MidiDefs.Step.C, MidiDefs.OCTAVE_CENTER, MidiDefs.ALTER_NATURAL)
        {
        }

        /// <summary>
        /// コンストラクタ(オクターブを含む全情報)
        /// </summary>
        /// <param name="Step"></param>
        /// <param name="octave"></param>
        /// <param name="Alter"></param>
        public Pitch(MidiDefs.Step Step, int octave, int Alter)
        {
            this.RawStep = Step;
            this.RawAlter = Alter;
            this.RawOctave = octave;
            //AlterをStepに変換して実音を求める
            MidiDefs.Step TempStep = this.RawStep;
            int TempOctave = this.RawOctave;
            int TempAlter = this.RawAlter;
            PitchUtil.AdjustToRealPitch(ref TempStep, ref TempOctave, ref TempAlter);
            this.Step = TempStep;
            this.Octave = TempOctave;
            this.Alter = TempAlter;
        }

        /// <summary>
        /// コンストラクタ(XDocument版)
        /// </summary>
        /// <param name="Node"></param>
        /// <exception cref="FormatException"></exception>
        /// <exception cref="ArgumentException"></exception>
        public Pitch(XElement Node)
        {
            //タグの読み取り
            XElement? StepNode = Node.Element("step");
            XElement? OctaveNode = Node.Element("octave");
            XElement? AlterNode = Node.Element("alter");
            //必須タグのチェック
            if (StepNode == null)
            {
                throw new FormatException("<pitch>: <step>: Not found.");
            }
            if (OctaveNode == null)
            {
                throw new FormatException("<pitch>: <octave> Not found.");
            }
            //必須データの正当性チェック
            string RawStep = StepNode.Value ?? "";
            if (!MidiDefs.StepMembers.Exists(x => x.Key.Equals(RawStep, StringComparison.OrdinalIgnoreCase)))
            {
                throw new ArgumentException("<pitch>: <step>: Invalid value.");
            }
            if (!int.TryParse(OctaveNode.Value, out int RawOctaveInt))
            {
                throw new ArgumentException("<pitch>: <octave>: Invalid value.");
            }
            //必須データのセット
            this.RawStep = MidiDefs.StepMembers.FirstOrDefault(x => x.Key.Equals(RawStep, StringComparison.CurrentCultureIgnoreCase)).Value;
            this.RawOctave = RawOctaveInt;
            //任意データの処理
            if (AlterNode != null)
            {
                //任意データのセット
                if (!int.TryParse(AlterNode.Value, out int RawAlterInt))
                {
                    throw new ArgumentException("<pitch>: <alter>: Invalid value.");
                }
                //サポートチェック
                if (RawAlterInt < MidiDefs.ALTER_DOUBLE_FLAT || MidiDefs.ALTER_DOUBLE_SHARP < RawAlterInt)
                {
                    throw new ArgumentException("<pitch>: <alter>: Unsupported value.");
                }
                //任意データのセット
                this.RawAlter = RawAlterInt;
            }
            //AlterをStepに変換して実音を求める
            MidiDefs.Step TempStep = this.RawStep;
            int TempOctave = this.RawOctave;
            int TempAlter = this.RawAlter;
            PitchUtil.AdjustToRealPitch(ref TempStep, ref TempOctave, ref TempAlter);
            this.Step = TempStep;
            this.Octave = TempOctave;
            this.Alter = TempAlter;
        }

        #endregion

        #region "public methods"

        /// <summary>
        /// クローンの作成
        /// </summary>
        /// <returns></returns>
        public Pitch Clone()
        {
            return new Pitch(this.RawStep, this.RawOctave, this.RawAlter);
        }

        /// <summary>
        /// ルート音を指定してクロマチックインデックスを取得する
        /// </summary>
        /// <param name="Root">ルート音</param>
        /// <returns></returns>
        public int GetChromaticIndex(PitchClass Root)
        {
            return PitchClass.GetChromaticIndex(Root);
        }

        /// <summary>
        /// オクターブ変更
        /// </summary>
        /// <param name="Alter"></param>
        public void AlterOctave(int Alter)
        {
            this.Octave += Alter;
        }
        
        /// <summary>
        /// 半音操作した新しいPitchの取得
        /// </summary>
        /// <param name="Alter"></param>
        /// <returns></returns>
        public Pitch GetAlteredPitch(int Alter)
        {
            return PitchUtil.GetAlteredPitch(this, Alter);
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

            Dump += "<pitch>";
            Dump += "<step>" + RawStep.ToString();
            Dump += "<alter>" + RawAlter.ToString();
            Dump += "<octave>" + RawOctave.ToString();
            Dump += "[real}";
            Dump += "<step>" + Step.ToString();
            Dump += "<alter>" + Alter.ToString();
            Dump += "<octave>" + Octave.ToString();
            return Dump;
        }

        #endregion
    }
}
