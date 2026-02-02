using System.Xml.Linq;

namespace Developers.MidiXml.Elements
{
    /// <summary>
    /// <pitch>の解析
    /// </summary>
    public class Pitch : MidiElement//, IEquatable<Pitch?>
    {
        #region "properties"

        private KeyTranspose? Key { get; set; } = null;

        /// <summary>
        /// 指定されたステップ
        /// </summary>
        public MidiDefs.Step RawStep { get; set; } = MidiDefs.Step.C;
        /// <summary>
        /// 指定されたオクターブ
        /// </summary>
        public int RawOctave { get; set; } = MidiDefs.OCTAVE_CENTER;
        /// <summary>
        /// 指定された半音操作
        /// </summary>
        public int RawAlter { get; set; } = MidiDefs.ALTER_NATURAL;
        /// <summary>
        /// ステップ(実音)
        /// </summary>
        public MidiDefs.Step Step { get; private set; } = MidiDefs.Step.C;
        /// <summary>
        /// オクターブ(実音)
        /// </summary>
        public int Octave { get; private set; } = MidiDefs.OCTAVE_CENTER;
        /// <summary>
        /// 半音操作(実音)
        /// </summary>
        public int Alter { get; private set; } = MidiDefs.ALTER_NATURAL;
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
        /// コンストラクタ(デフォルト)
        /// </summary>
        public Pitch(MidiDefs.Step Step, int Octave, int Alter)
            : this(Step, Octave, Alter, null)
        {
        }

        /// <summary>
        /// コンストラクタ(オクターブを含む全情報)
        /// </summary>
        /// <param name="Step"></param>
        /// <param name="octave"></param>
        /// <param name="Alter"></param>
        public Pitch(MidiDefs.Step Step, int octave, int Alter, KeyTranspose? Key)
        {
            this.RawStep = Step;
            this.RawAlter = Alter;
            this.RawOctave = octave;
            this.Key = Key;
            //AlterをStepに変換して実音を求める
            PitchCalculation();
        }

        /// <summary>
        /// コンストラクタ(XDocument版)
        /// </summary>
        /// <param name="SourceElm"></param>
        /// <exception cref="FormatException"></exception>
        /// <exception cref="ArgumentException"></exception>
        public Pitch(XElement SourceElm, KeyTranspose Key)
        {
            //ソース読み取り
            XElement? StepNode = SourceElm.Element("step");
            XElement? OctaveNode = SourceElm.Element("octave");
            XElement? AlterNode = SourceElm.Element("alter");

            //キー情報の保存
            this.Key = Key;
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
            PitchCalculation();
        }

        #endregion

        #region "override-related methods"
        /*
        /// <summary>
        /// Eualsの実装
        /// </summary>
        /// <param name="other"></param>
        /// <returns></returns>
        public bool Equals(Pitch? other)
        {
            return other is not null &&
                   Step == other.Step &&
                   Octave == other.Octave &&
                   Alter == other.Alter;
        }

        /// <summary>
        /// Equalsの実装
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public override bool Equals(object? obj)
        {
            return Equals(obj as Pitch);
        }

        /// <summary>
        /// == 演算子の実装
        /// </summary>
        /// <param name="left"></param>
        /// <param name="right"></param>
        /// <returns></returns>
        public static bool operator ==(Pitch? left, Pitch? right)
        {
            return EqualityComparer<Pitch>.Default.Equals(left, right);
        }

        /// <summary>
        /// != 演算子の実装
        /// </summary>
        /// <param name="left"></param>
        /// <param name="right"></param>
        /// <returns></returns>
        public static bool operator !=(Pitch? left, Pitch? right)
        {
            return !(left == right);
        }

        /// <summary>
        /// GetHashCodeの実装
        /// </summary>
        /// <returns></returns>
        public override int GetHashCode()
        {
            return HashCode.Combine(this.Step, this.Alter, this.Octave);
        }
        */
        #endregion

        #region "public methods"

        /// <summary>
        /// クローンの作成
        /// </summary>
        /// <returns></returns>
        public Pitch Clone()
        {
            return new Pitch(this.RawStep, this.RawOctave, this.RawAlter, this.Key);
        }

        /// <summary>
        /// XElementにシリアライズ
        /// </summary>
        /// <param name="Alter"></param>
        public XElement Serialize()
        {
            XElement RetVal = new XElement("pitch");
            RetVal.Add(new XElement("<step>", this.Step));
            RetVal.Add(new XElement("<alter>", this.Alter));
            RetVal.Add(new XElement("<octave>", this.Octave));
            return RetVal;
        }

        public void Transpose(Pitch OrigKey, Pitch TargKey, int Direction)
        {

        }

        /// <summary>
        /// オクターブ変更
        /// </summary>
        /// <param name="Alter"></param>
        public void AlterOctave(int Alter)
        {
            this.RawOctave += Alter;
            //AlterをStepに変換して実音を求める
            PitchCalculation();
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
        /// 半音操作した新しいPitchの取得
        /// </summary>
        /// <param name="Alter"></param>
        /// <returns></returns>
        public Pitch GetAlteredPitch(int Alter)
        {
            return PitchUtil.GetAlteredPitch(this, Alter);
        }

        #endregion

        /// <summary>
        ///RawStep, RawAlter, RawOctaveを再計算してStep, Alter, Octaveを設定する
        /// </summary>
        private void PitchCalculation()
        {
            //AlterをStepに変換して実音を求める
            MidiDefs.Step TempStep = this.RawStep;
            int TempOctave = this.RawOctave;
            int TempAlter = this.RawAlter;
            PitchUtil.AdjustToRealPitch(ref TempStep, ref TempOctave, ref TempAlter);
            this.Step = TempStep;
            this.Octave = TempOctave;
            this.Alter = TempAlter;
        }



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
