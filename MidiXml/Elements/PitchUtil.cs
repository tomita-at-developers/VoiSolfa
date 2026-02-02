namespace Developers.MidiXml.Elements
{
    public static class PitchUtil
    {
        #region "fields"

        //クロマチックテーブルのテンプレート(C開始)
        public static List<List<KeyValuePair<MidiDefs.Step, int>>> CBasedChromaticScale { get; } =
        [
            [
                new KeyValuePair<MidiDefs.Step, int>(MidiDefs.Step.C, MidiDefs.ALTER_NATURAL),
            ],
            [
                new KeyValuePair<MidiDefs.Step, int>(MidiDefs.Step.C, MidiDefs.ALTER_SHARP),
                new KeyValuePair<MidiDefs.Step, int>(MidiDefs.Step.D, MidiDefs.ALTER_FLAT),
            ],
            [
                new KeyValuePair<MidiDefs.Step, int>(MidiDefs.Step.D, MidiDefs.ALTER_NATURAL),
            ],
            [
                new KeyValuePair<MidiDefs.Step, int>(MidiDefs.Step.D, MidiDefs.ALTER_SHARP),
                new KeyValuePair<MidiDefs.Step, int>(MidiDefs.Step.E, MidiDefs.ALTER_FLAT),
            ],
            [
                new KeyValuePair<MidiDefs.Step, int>(MidiDefs.Step.E, MidiDefs.ALTER_NATURAL),
            ],
            [
                new KeyValuePair<MidiDefs.Step, int>(MidiDefs.Step.F, MidiDefs.ALTER_NATURAL),
            ],
            [
                new KeyValuePair<MidiDefs.Step, int>(MidiDefs.Step.F, MidiDefs.ALTER_SHARP),
                new KeyValuePair<MidiDefs.Step, int>(MidiDefs.Step.G, MidiDefs.ALTER_FLAT),
            ],
            [
                new KeyValuePair<MidiDefs.Step, int>(MidiDefs.Step.G, MidiDefs.ALTER_NATURAL),
            ],
            [
                new KeyValuePair<MidiDefs.Step, int>(MidiDefs.Step.G, MidiDefs.ALTER_SHARP),
                new KeyValuePair<MidiDefs.Step, int>(MidiDefs.Step.A, MidiDefs.ALTER_FLAT),
            ],
            [
                new KeyValuePair<MidiDefs.Step, int>(MidiDefs.Step.A, MidiDefs.ALTER_NATURAL),
            ],
            [
                new KeyValuePair<MidiDefs.Step, int>(MidiDefs.Step.A, MidiDefs.ALTER_SHARP),
                new KeyValuePair<MidiDefs.Step, int>(MidiDefs.Step.B, MidiDefs.ALTER_FLAT),
            ],
            [
                new KeyValuePair<MidiDefs.Step, int>(MidiDefs.Step.B, MidiDefs.ALTER_NATURAL),
            ],
        ];

        //Stepのクロマチックインデックス
        public static List<KeyValuePair<MidiDefs.Step, int>> StepIndexes { get; } =
        [
            new KeyValuePair<MidiDefs.Step, int>(MidiDefs.Step.C, 0),
            new KeyValuePair<MidiDefs.Step, int>(MidiDefs.Step.D, 2),
            new KeyValuePair<MidiDefs.Step, int>(MidiDefs.Step.E, 4),
            new KeyValuePair<MidiDefs.Step, int>(MidiDefs.Step.F, 5),
            new KeyValuePair<MidiDefs.Step, int>(MidiDefs.Step.G, 7),
            new KeyValuePair<MidiDefs.Step, int>(MidiDefs.Step.A, 9),
            new KeyValuePair<MidiDefs.Step, int>(MidiDefs.Step.B, 11),
        ];

        #endregion

        #region "public methods"

        /// <summary>
        /// Alterによって別のStepで表現できるか判定
        /// </summary>
        /// <param name="Pitch"></param>
        /// <returns></returns>
        public static bool HasAlternativeStep(Pitch Pitch)
        {
            bool RetVal = false;

            MidiDefs.Step TempStep = Pitch.Step;
            int TempOctave = Pitch.Octave;
            int TempAlter = Pitch.Alter;
            AdjustToSimplePitch(ref TempStep, ref TempOctave, ref TempAlter);
            if (Pitch.Step != TempStep)
            {
                RetVal = true;
            }
            return RetVal;
        }

        /// <summary>
        /// Alterによって別のOctave帯に変化するか判定
        /// </summary>
        /// <param name="Pitch"></param>
        /// <returns></returns>
        public static bool HasAlternativeOctave(Pitch Pitch)
        {
            bool RetVal = false;

            MidiDefs.Step TempStep = Pitch.Step;
            int TempOctave = Pitch.Octave;
            int TempAlter = Pitch.Alter;
            AdjustToSimplePitch(ref TempStep, ref TempOctave, ref TempAlter);
            if (Pitch.Octave != TempOctave)
            {
                RetVal = true;
            }
            return RetVal;
        }

        /// <summary>
        /// 指定されたAlterをStepに繰り上げてAlterを最低限に縮小する
        /// </summary>
        /// <param name="Step"></param>
        /// <param name="Octave"></param>
        /// <param name="Alter"></param>
        public static void AdjustToSimplePitch(ref MidiDefs.Step Step, ref int Octave, ref int Alter)
        {
            MidiDefs.Step RetStep = MidiDefs.Step.C;
            int RetOctave = 0;
            int RetAlter = 0;
            int StepedAlter = 0;

            //指定されたPitch.StepのOctave4内Indexを取得
            RetAlter = GetCBasedChromaticIndexOfStep(Step);
            //オクターブを考慮
            RetAlter += ((Octave - 4) * 12);
            //Alter操作
            RetAlter += Alter;
            //C4以上の時
            if (RetAlter >= 0)
            {
                RetOctave = 4 + (RetAlter / 12);
                RetAlter %= 12;
            }
            else
            {
                RetOctave = 3 - (RetAlter / 12);
                RetAlter = RetAlter % 12 + 12;
            }
            //StepIndexのうち割り当てられる最大のStepを求める
            foreach (KeyValuePair<MidiDefs.Step, int> StepInfo in StepIndexes)
            {
                if (RetAlter - StepInfo.Value < 0)
                {
                    break;
                }
                RetStep = StepInfo.Key;
                StepedAlter = StepInfo.Value;
            }
            //割り当てたStepの分だけAlterを減算する
            //この時点でAlterはプラス方向なのでシャープ表現になる
            RetAlter -= StepedAlter;
            //もともとフラット表現だったものがシャープ表現になっている場合
            if (Alter < 0 && RetAlter > 0)
            {
                if (RetStep == MidiDefs.Step.B)
                {
                    RetOctave--;
                }
                RetStep = GetNextStep(RetStep);
                RetAlter = -1;
            }
            Step = RetStep;
            Octave = RetOctave;
            Alter = RetAlter;
        }

        /// <summary>
        /// 五度圏指定の主音をPtichClassで取得
        /// </summary>
        /// <param name="Fifths"></param>
        /// <returns></returns>
        public static PitchClass FifthsToPitchClass(int Fifths)
        {
            //シャープ系
            // 0  1  2  3  4  5  6      7      8      9      10     11
            // C  G  D  A  E  B  F#/Gb  C#/Db  G#/Ab  D#/Eb  A#/Bb  F 
            //フラット系
            // 0  1  2      3      4      5      6      7  8  9  10 11
            // C  F  Bb/A#  Eb/D#  Ab/G#  Db/C#  Gb/F#  B  E  A  D  G

            return GetAlteredPitchClass(new PitchClass(MidiDefs.Step.C, MidiDefs.ALTER_NATURAL), (Fifths % 12) * 7);
        }

        /// <summary>
        /// 指定されたPithClassをAlterした新しいPitchClassインスタンスを取得する
        /// </summary>
        /// <param name="Source"></param>
        /// <param name="Alter"></param>
        /// <returns></returns>
        public static PitchClass GetAlteredPitchClass(PitchClass Source, int Alter)
        {

            Pitch RetPitch = GetAlteredPitch(new Pitch(Source.Step, MidiDefs.OCTAVE_CENTER, Source.Alter), Alter);

            return new PitchClass(RetPitch.Step, RetPitch.Alter);
        }

        /// <summary>
        /// 指定されたPithをAlterした新しいPitchインスタンスを取得する
        /// </summary>
        /// <param name="Source"></param>
        /// <param name="Alter"></param>
        /// <returns></returns>
        public static Pitch GetAlteredPitch(Pitch Source, int Alter)
        {
            //StepとOctaveはそのまま
            MidiDefs.Step RetStep = Source.Step;
            int RetOctave = Source.Octave;
            //Alterはここで加算する
            int RetAlter = Source.Alter + Alter;

            //アジャスト
            AdjustToSimplePitch(ref RetStep, ref RetOctave, ref RetAlter);
            //もともとフラット表現だったものがシャープ表現になっている場合
            if (Source.Alter < 0 && RetAlter > 0)
            {
                if (RetStep == MidiDefs.Step.B)
                {
                    RetOctave--;
                }
                RetStep = GetNextStep(RetStep);
                RetAlter = -1;
            }
            Pitch RetPitch = new Pitch(RetStep, RetOctave, RetAlter);
            return RetPitch;
        }

        /// <summary>
        /// 指定された主音をもとに半音階インデックスを取得する
        /// </summary>
        /// <param name="ChromaticScale"></param>
        /// <param name="PitchClass"></param>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        public static int GetChromaticIndex(PitchClass Root, PitchClass PitchClass)
        {
            return GetChromaticIndex(Root, PitchClass.Step, PitchClass.Alter);
        }

        /// <summary>
        /// 指定された主音をもとに半音階インデックスを取得する
        /// </summary>
        /// <param name="ChromaticScale"></param>
        /// <param name="PitchClass"></param>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        public static int GetChromaticIndex(PitchClass Root, Pitch Pitch)
        {
            return GetChromaticIndex(Root, Pitch.Step, Pitch.Alter);
        }

        #endregion

        #region "private methods"

        /// <summary>
        /// 指定された主音をもとに半音階インデックスを取得する
        /// </summary>
        /// <param name="ChromaticScale"></param>
        /// <param name="Step"></param>
        /// <param name="Alter"></param>
        /// <returns></returns>
        private static int GetChromaticIndex(PitchClass Root, MidiDefs.Step Step, int Alter)
        {
            int RetVal = 0;
            //主音のIndexをC-Basedで求める
            int RootIndex = GetCBasedChromaticIndex(Root.Step, Root.Alter);
            //Step,AlterのIndesxをC-Basedで求める
            int NoteIndex = GetCBasedChromaticIndex(Step, Alter);
            //指定キーにトランスポーズ
            RetVal = (NoteIndex - RootIndex);
            if (RetVal >= 0)
            {
                RetVal = RetVal % 12;
            }
            else
            {
                RetVal = ((RetVal + 1) % 12) + 11;
            }
            return RetVal;
        }

        /// <summary>
        /// 指定された音の半音階インデックスを取得する
        /// </summary>
        /// <param name="ChromaticScale"></param>
        /// <param name="PitchClass"></param>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        private static int GetCBasedChromaticIndex(MidiDefs.Step Step, int Alter)
        {
            //ステップの絶対インデックス
            int StepIndex = GetCBasedChromaticIndexOfStep(Step);
            int PitchIndex = StepIndex + Alter;
            if (PitchIndex >= 0)
            {
                PitchIndex %= 12;
            }
            else
            {
                PitchIndex = ((PitchIndex + 1) % 12) + 11;
            }
            return PitchIndex;
        }

        /// <summary>
        /// ステップのクロマチックインデックス取得
        /// </summary>
        /// <param name="Step"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentException"></exception>
        private static int GetCBasedChromaticIndexOfStep(MidiDefs.Step Step)
        {
            if (!StepIndexes.Exists(x => x.Key.Equals(Step)))
            {
                throw new ArgumentException("Invalid Step.");
            }
            return StepIndexes.FirstOrDefault(x => x.Key.Equals(Step)).Value;
        }

        /// <summary>
        /// 指定されたStepの次のStepを取得
        /// </summary>
        /// <param name="Step"></param>
        /// <returns></returns>
        private static MidiDefs.Step GetNextStep(MidiDefs.Step Step)
        {
            MidiDefs.Step RetVal = MidiDefs.Step.C;

            for (int i = 0; i < StepIndexes.Count; i++)
            {
                if (StepIndexes[i].Key == Step)
                {
                    if (i + 1 < StepIndexes.Count)
                    {
                        RetVal = StepIndexes[i + 1].Key;
                    }
                    else
                    {
                        RetVal = StepIndexes[0].Key;
                    }
                    break;
                }
            }
            return RetVal;
        }

        #endregion

        #region "debug methods"

        public static int DebugGetCBasedChromaticIndex(MidiDefs.Step Step, int Alter)
        {
            return GetCBasedChromaticIndex(Step, Alter);
        }

        #endregion

    }
}
