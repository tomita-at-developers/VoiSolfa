namespace Developers.MidiXml.Elements
{
    public class PitchClass
    {
        #region "properties"

        ///// <summary>
        ///// 指定されたステップ
        ///// </summary>
        //public MidiDefs.Step RawStep { get; init; } = MidiDefs.Step.C;
        ///// <summary>
        ///// 指定された半音操作
        ///// </summary>
        //public int RawAlter { get; init; } = MidiDefs.ALTER_NATURAL;
        /// <summary>
        /// ステップ(オリジナル)
        /// </summary>
        public MidiDefs.Step Step { get; init; } = MidiDefs.Step.C;
        /// <summary>
        /// 半音操作(オリジナル)
        /// </summary>
        public int Alter { get; init; } = MidiDefs.ALTER_NATURAL;
        /// <summary>
        /// Stepに対して冗長なAlterを短縮したPitchClass
        /// </summary>
        public PitchClass SimplePtichClass
        {
            get
            {
                MidiDefs.Step TempStep = this.Step;
                int TempOctave = MidiDefs.OCTAVE_CENTER;
                int TempAlter = this.Alter;
                PitchUtil.AdjustToSimplePitch(ref TempStep, ref TempOctave, ref TempAlter);
                return new PitchClass(TempStep, TempAlter);
            }
        }

        #endregion

        #region "constructors"

        /// <summary>
        /// コンストラクタ(デフォルト)
        /// </summary>
        public PitchClass()
            : this(MidiDefs.Step.C, MidiDefs.ALTER_NATURAL)
        {
        }

        /// <summary>
        /// コンストラクタ(完全)
        /// </summary>
        /// <param name="Step"></param>
        /// <param name="Alter"></param>
        public PitchClass(MidiDefs.Step Step, int Alter)
        {
            this.Step = Step;
            this.Alter = Alter;
        }

        #endregion

        #region "public methods"

        /// <summary>
        /// クローンの作成
        /// </summary>
        /// <returns></returns>
        public PitchClass Clone()
        {
            return new PitchClass(this.Step, this.Alter);
        }

        /// <summary>
        /// ルート音を指定してクロマチックインデックスを取得する
        /// </summary>
        /// <param name="Root">ルート音</param>
        /// <returns></returns>
        public int GetChromaticIndex(PitchClass Root)
        {
            return PitchUtil.GetChromaticIndex(Root, this);
        }

        /// <summary>
        /// AlterをStepに変換して実音を求める
        /// </summary>
        /// <param name="Alter"></param>
        /// <returns></returns>
        public PitchClass GetAlteredPitchClass(int Alter)
        {
            return PitchUtil.GetAlteredPitchClass(this, Alter);
        }

        #endregion

        #region "private methods"


        #endregion

        #region "debug methods"

        /// <summary>
        /// デバック用ダンプ
        /// </summary>
        /// <returns></returns>
        public string DebugDump()
        {
            string Dump = string.Empty;

            Dump += "[PitchClass]";
            Dump += "Step=" + Step.ToString();
            Dump += "Alter=" + Alter.ToString();
            return Dump;
        }

        #endregion

    }
}
