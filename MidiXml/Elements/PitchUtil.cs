using Developers.MidiXml.Elements;
using System.Diagnostics;
using System.Diagnostics.Metrics;
using System.Net.Http.Headers;
using System.Reflection.PortableExecutable;
using static Developers.MidiXml.Elements.MidiDefs;

namespace Developers.MidiXml.Elements
{
    public static class PitchUtil
    {
        #region "fields"

        //クロマチックテーブルのテンプレート(C開始)
        //　PitchClassだとnewで循環参照する可能性があるのでKeyValuePairで表現
        public static readonly List<List<KeyValuePair<MidiDefs.Step, int>>> CBasedChromaticScale =
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
        public static readonly List<KeyValuePair<MidiDefs.Step, int>> StepChromaticIndexes =
        [
            new KeyValuePair<MidiDefs.Step, int>(MidiDefs.Step.C,  0),
            new KeyValuePair<MidiDefs.Step, int>(MidiDefs.Step.D,  2),
            new KeyValuePair<MidiDefs.Step, int>(MidiDefs.Step.E,  4),
            new KeyValuePair<MidiDefs.Step, int>(MidiDefs.Step.F,  5),
            new KeyValuePair<MidiDefs.Step, int>(MidiDefs.Step.G,  7),
            new KeyValuePair<MidiDefs.Step, int>(MidiDefs.Step.A,  9),
            new KeyValuePair<MidiDefs.Step, int>(MidiDefs.Step.B, 11),
        ];

        //ダイアトニックテーブルのテンプレート(C開始)
        private static readonly List<MidiDefs.Step> CBasedDiatonicScale =
        [
            MidiDefs.Step.C,
            MidiDefs.Step.D,
            MidiDefs.Step.E,
            MidiDefs.Step.F,
            MidiDefs.Step.G,
            MidiDefs.Step.A,
            MidiDefs.Step.B,
        ];

        #endregion

        #region "public methods"

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

            //return GetAlteredPitchClass(new PitchClass(MidiDefs.Step.C, MidiDefs.ALTER_NATURAL), (Fifths % 12) * 7);

            //FifthsMembersの検索
            KeyValuePair<MidiDefs.Step, int> Signature = MidiDefs.FifthsMembers.Find(x => x.Key == Fifths).Value;
            //FifthsMembersにない場合は例外
            if (Signature.Key == MidiDefs.Step.EnumDefault)
            {
                throw new ArgumentException("Specified fitfhs value is unsupported.");
            }
            //テーブルの値でPtichClassを生成しリターン
            return new PitchClass(Signature.Key, Signature.Value);
        }

        /// <summary>
        /// 五度圏指定の主音をPtichClassで取得
        /// </summary>
        /// <param name="Fifths"></param>
        /// <returns></returns>
        public static int PitchClassToFifths(PitchClass Signature)
        {
            //シャープ系
            // 0  1  2  3  4  5  6      7      8      9      10     11
            // C  G  D  A  E  B  F#/Gb  C#/Db  G#/Ab  D#/Eb  A#/Bb  F 
            //フラット系
            // 0  1  2      3      4      5      6      7  8  9  10 11
            // C  F  Bb/A#  Eb/D#  Ab/G#  Db/C#  Gb/F#  B  E  A  D  G

            //FifthsMembersの検索
            KeyValuePair<int, KeyValuePair<MidiDefs.Step, int>> FifthsMember = MidiDefs.FifthsMembers.Find(x => x.Value.Key == Signature.Step && x.Value.Value == Signature.Alter);
            //FifthsMembersにない場合は例外
            if (FifthsMember.Value.Key == MidiDefs.Step.EnumDefault)
            {
                throw new ArgumentException("Specified fitfhs value is unsupported.");
            }
            //テーブルの値でリターン
            return FifthsMember.Key;
        }

        /// <summary>
        /// 指定されたStepで同音異名を取得
        /// </summary>
        /// <param name="Original"></param>
        /// <param name="Step"></param>
        /// <returns></returns>
        public static PitchClass GetEnharmonic(PitchClass Original, MidiDefs.Step Step)
        {
            //仮に中央のオクターブでPitchクラズを作成しGetEnharmonicをコール(結果のPitchClassでリターン)
            return GetEnharmonic(new Pitch(Original.Step, MidiDefs.OCTAVE_CENTER, Original.Alter), Step).PitchClass;
        }

        /// <summary>
        /// 指定されたStepで同音異名を取得
        /// </summary>
        /// <param name="Original"></param>
        /// <param name="Step"></param>
        /// <returns></returns>
        public static Pitch GetEnharmonic(Pitch Original, MidiDefs.Step Step)
        {
            int OrigStepIndex = GetCBasedChromaticIndexOfStep(Original.Step);
            int TargStepIndex = GetCBasedChromaticIndexOfStep(Step);
            int TargAlter  = Original.Alter + (OrigStepIndex - TargStepIndex);
            int TargOctave = Original.Octave;
            //大きすぎる場合
            if (TargAlter > 6)
            {
                //オクターブ上のフラット表現に変更
                TargAlter -= 12;
                TargOctave += 1;
            }
            //小さすぎる場合
            else if (TargAlter < -6)
            {
                //オクターブ下のシャープ表現に変更
                TargAlter += 12;
                TargOctave -= 1;
            }
            return new Pitch(Step, TargOctave, TargAlter);
        }

        /// <summary>
        /// 指定されたPithClassをAlterした新しいPitchClassインスタンスを取得する
        /// </summary>
        /// <param name="Source"></param>
        /// <param name="Alter"></param>
        /// <returns></returns>
        public static PitchClass GetAlteredPitchClass(PitchClass Source, int Alter)
        {
            //仮に中央のオクターブでPitchクラズを作成しAlteredPitchをコール(結果のPitchClassでリターン)
            return GetAlteredPitch(new Pitch(Source.Step, MidiDefs.OCTAVE_CENTER, Source.Alter), Alter).PitchClass;
        }

        /// <summary>
        /// 指定されたPithをAlterした新しいPitchインスタンスを取得する
        /// </summary>
        /// <param name="Source"></param>
        /// <param name="Alter"></param>
        /// <returns></returns>
        public static Pitch GetAlteredPitch(Pitch Source, int Alter)
        {
            return AdjustPitch(new Pitch(Source.Step, Source.Octave, Source.Alter + Alter));
        }

        /// <summary>
        /// 指定されたPitchClassをコンテキストを保持しつつ移調する
        /// </summary>
        /// <param name="Source"></param>
        /// <param name="Transposition"></param>
        /// <param name="Context"></param>
        public static PitchClass TransposePitchClass(PitchClass Source, Transposition Transposition, PitchContext Context)
        {
            //仮に中央のオクターブでPitchクラズを作成しTranposePitchをコール(結果のPitchClassでリターン)
            return TransposePitch(new Pitch(Source.Step, MidiDefs.OCTAVE_CENTER, Source.Alter), Transposition, Context).PitchClass;
        }

        /// <summary>
        /// 指定されたPitchをコンテキストを保持しつつ移調する
        /// </summary>
        /// <param name="Source"></param>
        /// <param name="Transposition"></param>
        /// <param name="Context"></param>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        public static Pitch TransposePitch(Pitch Source, Transposition Transposition, PitchContext Context)
        {
            //移調前のMidiNoteNumberを取得(確認用)
            int OriginalMidiNoteNumber = PitchToMidiNoteNumber(Source);
            //仮にトランスポーズする
            Pitch TargPitch = AdjustPitch(new Pitch(Source.Step, Source.Octave, Source.Alter + Transposition.OriginalTranspose.Chromatic));
            //移調後のMidiNoteNumberを取得(確認用)
            int TransposedMidiNoteNumber = PitchToMidiNoteNumber(TargPitch);
            //移調の正当性確認
            if ((OriginalMidiNoteNumber + Transposition.OriginalTranspose.Chromatic) != TransposedMidiNoteNumber)
            {
                throw new Exception(
                    "TranposePitch() failed to transpopse." + 
                    Source.Step + Source.Octave + "(" + Source.Alter + ")" +
                    " == transpose(" + Transposition.OriginalTranspose.Chromatic + ") ==> " +
                    TargPitch.Step + TargPitch.Octave + "(" + TargPitch.Alter + ")");
            }
            //Context上のステップを取得する
            MidiDefs.Step TargStep = Transposition.ChromaticScale[Context.ChromaticIndex][Context.EnharmonicIndex].Step;
            //Contextと異なるステップの場合はステップ変更
            if (TargPitch.Step != TargStep)
            {
                TargPitch = GetEnharmonic(TargPitch, TargStep);
                //Step変更後のMidiNoteNumberを取得(確認用)
                int EnharmonicMidiNoteNumber = PitchToMidiNoteNumber(TargPitch);
                //Step変更の正当性確認
                if (TransposedMidiNoteNumber != EnharmonicMidiNoteNumber)
                {
                    throw new Exception(
                        "TranposePitch() failed to get enharmonice." +
                        Source.Step + Source.Octave + "(" + Source.Alter + ")" +
                        " == transpose(" + Transposition.OriginalTranspose.Chromatic + ") ==> " +
                        TargPitch.Step + TargPitch.Octave + "(" + TargPitch.Alter + ")");
                }
            }
            return TargPitch;
        }

        /// <summary>
        /// 指定されたStepをダイアトニックにAlterした新しいStepを取得する
        /// </summary>
        /// <param name="Step"></param>
        /// <param name="DiatonicAlter"></param>
        /// <returns></returns>
        public static MidiDefs.Step GetDiatonicAlteredStep(MidiDefs.Step Step, int DiatonicAlter)
        {
            //StepのDiatonicIndexを取得
            int DiatonicIndex = CBasedDiatonicScale.IndexOf(Step);
            //DiatonicAlter計算
            DiatonicIndex += DiatonicAlter;
            //0以上ならば7の剰余系
            if (DiatonicIndex >= 0)
            {
                DiatonicIndex %= 7;
            }
            //マイナスならば7の剰余系+7
            else
            {
                DiatonicIndex = DiatonicIndex % 7 + 7;
            }
            return CBasedDiatonicScale[DiatonicIndex];
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

        /// <summary>
        /// 指定されたルート音からのクロマチックスケールを作成する
        /// </summary>
        /// <param name="Root"></param>
        /// <returns></returns>
        public static List<List<PitchClass>> CreateChromaticScale(PitchClass Root)
        {
            // I
            PitchClass I = Root.Clone();
            PitchClass Is   = GetEnharmonic(new PitchClass(I.Step, I.Alter +  1), I.Step);
            // II
            PitchClass IIf  = GetEnharmonic(new PitchClass(I.Step, I.Alter +  1), GetNextStep(I.Step));
            PitchClass II   = GetEnharmonic(new PitchClass(I.Step, I.Alter +  2), GetNextStep(I.Step));
            PitchClass IIs  = GetEnharmonic(new PitchClass(I.Step, I.Alter +  3), GetNextStep(I.Step));
            // III
            PitchClass IIIf = GetEnharmonic(new PitchClass(I.Step, I.Alter +  3), GetNextStep(II.Step));
            PitchClass III  = GetEnharmonic(new PitchClass(I.Step, I.Alter +  4), GetNextStep(II.Step));
            // IV
            PitchClass IV   = GetEnharmonic(new PitchClass(I.Step, I.Alter +  5), GetNextStep(III.Step));
            PitchClass IVs  = GetEnharmonic(new PitchClass(I.Step, I.Alter +  6), GetNextStep(III.Step));
            // V
            PitchClass Vf   = GetEnharmonic(new PitchClass(I.Step, I.Alter +  6), GetNextStep(IV.Step));
            PitchClass V    = GetEnharmonic(new PitchClass(I.Step, I.Alter +  7), GetNextStep(IV.Step));
            PitchClass Vs   = GetEnharmonic(new PitchClass(I.Step, I.Alter +  8), GetNextStep(IV.Step));
            // VI
            PitchClass VIf  = GetEnharmonic(new PitchClass(I.Step, I.Alter +  8), GetNextStep(V.Step));
            PitchClass VI   = GetEnharmonic(new PitchClass(I.Step, I.Alter +  9), GetNextStep(V.Step));
            PitchClass VIs  = GetEnharmonic(new PitchClass(I.Step, I.Alter + 10), GetNextStep(V.Step));
            // VII
            PitchClass VIIf = GetEnharmonic(new PitchClass(I.Step, I.Alter + 10), GetNextStep(VI.Step));
            PitchClass VII  = GetEnharmonic(new PitchClass(I.Step, I.Alter + 11), GetNextStep(VI.Step));
            //テーブルにセット
            List<List<PitchClass>> RetVal = [
                [I],
                [Is, IIf],
                [II],
                [IIs, IIIf],
                [III],
                [IV],
                [IVs, Vf],
                [V],
                [Vs, VIf],
                [VI],
                [VIs, VIIf],
                [VII],
            ];
            return RetVal;
        }

        #endregion

        #region "private methods"

        /// <summary>
        /// Pitchの調整(余剰のAlterをStepに繰り上げる)
        /// </summary>
        /// <param name="Pitch"></param>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        private static Pitch AdjustPitch(Pitch Pitch)
        {
            //MidiNoteNumberに変換
            int MidiNoteNumber = PitchToMidiNoteNumber(Pitch);
            //Pitchに再変換
            Pitch RetPitch = MidiNoteNumberToPitch(MidiNoteNumber);
            //念のため再変換したPitchをMidiNoteNumberに変換
            int EvalNoteNumber = PitchToMidiNoteNumber(RetPitch);
            //最初のNumberと２度目のNumvberがとこなればシステムエラー
            if (MidiNoteNumber != EvalNoteNumber)
            {
                throw new Exception(
                    "AdjusitPitch():System Error. The note number of adjusted pitch(" + MidiNoteNumber.ToString() + ") " +
                    "deffers from the note number of specified pitch(" + EvalNoteNumber.ToString() + ").");
            }
            return RetPitch;
        }

        /// <summary>
        /// 指定されたPitchのMidiノート番号を取得
        /// </summary>
        /// <param name="Pitch"></param>
        /// <returns></returns>
        private static int PitchToMidiNoteNumber(Pitch Pitch)
        {
            //Stepのオクターブ内Indexを算出(Alterは0で算出)
            int C4Index = GetCBasedChromaticIndex(Pitch.Step, 0);
            //オクターブ値をIndexに展開
            int OctaveAlter = 60 + (Pitch.Octave - 4) * 12;
            //合算値でリターン(Alterは最後に加算)
            return C4Index + OctaveAlter + Pitch.Alter;
        }

        /// <summary>
        /// 指定されたMidiノート番号のPitchを取得
        /// </summary>
        /// <param name="MidiNoteNumber"></param>
        /// <param name="Flatted">可能ならフラットで表現する(オプション)</param>
        /// <returns></returns>
        private static Pitch MidiNoteNumberToPitch(int MidiNoteNumber, bool Flatted = false)
        {
            //オクターブ番号を算出
            int Octave = (MidiNoteNumber / 12) -1;
            //オクターブ内のIndexを算出
            int RawAlter = MidiNoteNumber % 12;
            //オクターブ内のIndexをStepに繰り上げ
            MidiDefs.Step AsignedStep = MidiDefs.Step.EnumDefault;
            int SteppedAlter = 0;
            foreach (KeyValuePair<MidiDefs.Step, int> StepInfo in StepChromaticIndexes)
            {
                //フラット表現指定
                if (Flatted)
                {
                    //検索ステップIndexが割り当てIndexと一致または超えたとき
                    if (RawAlter - StepInfo.Value <= 0)
                    {
                        //このステップの値を採用(一致も含まれる)
                        AsignedStep = StepInfo.Key;
                        SteppedAlter = StepInfo.Value;
                        break;
                    }
                }
                //フラット表現指定なし
                else
                {
                    //検索ステップIndexが割り当てIndexを超えたとき
                    if (RawAlter - StepInfo.Value < 0)
                    {
                        //前のステップの値を採用(一致も含まれる)
                        break;
                    }
                    AsignedStep = StepInfo.Key;
                    SteppedAlter = StepInfo.Value;
                }
            }
            return new Pitch(AsignedStep, Octave, RawAlter - SteppedAlter);
        }

        /// <summary>
        /// 指定された主音をもとに半音階インデックスを取得する
        /// </summary>
        /// <param name="ChromaticScale"></param>
        /// <param name="Step"></param>
        /// <param name="Alter"></param>
        /// <returns></returns>
        private static int GetChromaticIndex(PitchClass Root, MidiDefs.Step Step, int Alter)
        {
            //主音のIndexをC-Basedで求める
            int RootIndex = GetCBasedChromaticIndex(Root.Step, Root.Alter);
            //Step,AlterのIndesxをC-Basedで求める
            int NoteIndex = GetCBasedChromaticIndex(Step, Alter);
            //指定キーにトランスポーズ
            int RetVal = (NoteIndex - RootIndex);
            if (RetVal >= 0)
            {
                RetVal %= 12;
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
            if (!StepChromaticIndexes.Exists(x => x.Key.Equals(Step)))
            {
                throw new ArgumentException("Invalid Step.");
            }
            return StepChromaticIndexes.FirstOrDefault(x => x.Key.Equals(Step)).Value;
        }

        /// <summary>
        /// 指定されたStepの次のStepを取得
        /// </summary>
        /// <param name="Step"></param>
        /// <returns></returns>
        private static MidiDefs.Step GetNextStep(MidiDefs.Step Step)
        {
            MidiDefs.Step RetVal = MidiDefs.Step.C;

            for (int i = 0; i < StepChromaticIndexes.Count; i++)
            {
                if (StepChromaticIndexes[i].Key == Step)
                {
                    if (i + 1 < StepChromaticIndexes.Count)
                    {
                        RetVal = StepChromaticIndexes[i + 1].Key;
                    }
                    else
                    {
                        RetVal = StepChromaticIndexes[0].Key;
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

        public static int DebugPitchToMidiNoteNumber(Pitch Pitch)
        {
            return PitchToMidiNoteNumber(Pitch);
        }

        public static Pitch DebugMidiNoteNumberToPitch(int MidiNoteNumber)
        {
            return MidiNoteNumberToPitch(MidiNoteNumber);
        }

        #endregion
    }
}
