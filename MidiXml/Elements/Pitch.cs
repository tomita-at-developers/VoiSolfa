using System.Xml.Linq;

namespace Developers.MidiXml.Elements
{
    /// <summary>
    /// <pitch>情報
    /// </summary>
    public class Pitch : MidiElement//, IEquatable<Pitch?>
    {
        #region "properties"

        /// <summary>
        /// ステップ(オリジナル)
        /// </summary>
        public MidiDefs.Step Step { get; private set; } = MidiDefs.Step.C;
        /// <summary>
        /// オクターブ(オリジナル)
        /// </summary>
        public int Octave { get; private set; } = MidiDefs.OCTAVE_CENTER;
        /// <summary>
        /// 半音操作(オリジナル)
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
        /// <summary>
        /// Stepに対して冗長なAlterを短縮したPitch
        /// </summary>
        public Pitch SimplePitch
        {
            get
            {
                MidiDefs.Step TempStep = this.Step;
                int TempOctave = this.Octave;
                int TempAlter = this.Alter;
                PitchUtil.AdjustToSimplePitch(ref TempStep, ref TempOctave, ref TempAlter);
                return new Pitch(TempStep, TempOctave, TempAlter);
            }
        }
        #endregion

        #region "constructors"

        /// <summary>
        /// コンストラクタ(オクターブを含む全情報)
        /// </summary>
        /// <param name="Step"></param>
        /// <param name="octave"></param>
        /// <param name="Alter"></param>
        public Pitch(MidiDefs.Step Step, int octave, int Alter)
        {
            this.Step = Step;
            this.Alter = Alter;
            this.Octave = octave;
        }

        /// <summary>
        /// コンストラクタ(XDocument版)
        /// </summary>
        /// <param name="Source"></param>
        /// <exception cref="FormatException"></exception>
        /// <exception cref="ArgumentException"></exception>
        public Pitch(XElement Source)
        {
            //ソース読み取り
            XElement? ElmStep = Source.Element("step");
            XElement? ElmOctave = Source.Element("octave");
            XElement? AlmAlter = Source.Element("alter");

            //必須タグのチェック
            if (ElmStep == null)
            {
                throw new FormatException("<pitch>: <step>: Not found.");
            }
            if (ElmOctave == null)
            {
                throw new FormatException("<pitch>: <octave> Not found.");
            }
            //必須データの正当性チェック
            string RawStep = ElmStep.Value ?? "";
            if (!MidiDefs.StepMembers.Exists(x => x.Key.Equals(RawStep, StringComparison.OrdinalIgnoreCase)))
            {
                throw new ArgumentException("<pitch>: <step>: Invalid value.");
            }
            if (!int.TryParse(ElmOctave.Value, out int RawOctaveInt))
            {
                throw new ArgumentException("<pitch>: <octave>: Invalid value.");
            }
            //必須データのセット
            this.Step = MidiDefs.StepMembers.FirstOrDefault(x => x.Key.Equals(RawStep, StringComparison.CurrentCultureIgnoreCase)).Value;
            this.Octave = RawOctaveInt;
            //任意データの処理
            if (AlmAlter != null)
            {
                //任意データのセット
                if (!int.TryParse(AlmAlter.Value, out int RawAlterInt))
                {
                    throw new ArgumentException("<pitch>: <alter>: Invalid value.");
                }
                //サポートチェック
                if (RawAlterInt < MidiDefs.ALTER_DOUBLE_FLAT || MidiDefs.ALTER_DOUBLE_SHARP < RawAlterInt)
                {
                    throw new ArgumentException("<pitch>: <alter>: Unsupported value.");
                }
                //任意データのセット
                this.Alter = RawAlterInt;
            }
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
            return new Pitch(this.Step, this.Octave, this.Alter);
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

        /// <summary>
        /// 移調楽器向けの記述をコンサートキーでの記述に変更
        /// </summary>
        /// <param name="Key"></param>
        public void TransposeToConcertKey(Key Key)
        {

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

        #region "debug methods"

        /// <summary>
        /// デバック用ダンプ
        /// </summary>
        /// <returns></returns>
        public override string DebugDump()
        {
            string Dump = string.Empty;

            Dump += "<pitch>";
            Dump += "<step>" + Step.ToString();
            Dump += "<alter>" + Alter.ToString();
            Dump += "<octave>" + Octave.ToString();
            return Dump;
        }

        #endregion
    }
}
