namespace Developers.MidiXml.Elements
{
    /// <summary>
    /// ピッチクラス情報(オクターブを意識しない音名)
    /// </summary>
    /// <remarks>
    /// コンストラクタ
    /// </remarks>
    /// <param name="Step"></param>
    /// <param name="Alter"></param>
    public class PitchClass(MidiDefs.Step Step, int Alter)
    {
        #region "properties"

        /// <summary>
        /// ステップ(オリジナル)
        /// </summary>
        public MidiDefs.Step Step { get; init; } = Step;
        /// <summary>
        /// 半音操作(オリジナル)
        /// </summary>
        public int Alter { get; init; } = Alter;
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

        //primary constructor only.

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
