using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Text;

namespace Developers.MidiXml.Elements
{
    /// <summary>
    /// 移調情報クラス
    /// </summary>
    public class Transposition
    {
        #region "fields"

        //サポートする調号の一覧
        public static readonly List<List<PitchClass>> SupportedSignaturesList =
        [
            // C
            [
                new PitchClass(MidiDefs.Step.C, MidiDefs.ALTER_NATURAL),
            ],
            // C#, Db　(Db優先としIndex=0に配置)
            [
                new PitchClass(MidiDefs.Step.D, MidiDefs.ALTER_FLAT),
                new PitchClass(MidiDefs.Step.C, MidiDefs.ALTER_SHARP),
            ],
            // D
            [
                new PitchClass(MidiDefs.Step.D, MidiDefs.ALTER_NATURAL),
            ],
            // Eb
            [
                new PitchClass(MidiDefs.Step.E, MidiDefs.ALTER_FLAT),
            ],
            // E
            [
                new PitchClass(MidiDefs.Step.E, MidiDefs.ALTER_NATURAL),
            ],
            // F
            [
                new PitchClass(MidiDefs.Step.F, MidiDefs.ALTER_NATURAL),
            ],
            // F#, Gb　(Gb優先としIndex=0に配置)
            [
                new PitchClass(MidiDefs.Step.G, MidiDefs.ALTER_FLAT),
                new PitchClass(MidiDefs.Step.F, MidiDefs.ALTER_SHARP),
            ],
            // G
            [
                new PitchClass(MidiDefs.Step.G, MidiDefs.ALTER_NATURAL),
            ],
            // Ab
            [
                new PitchClass(MidiDefs.Step.A, MidiDefs.ALTER_FLAT),
            ],
            // A
            [
                new PitchClass(MidiDefs.Step.A, MidiDefs.ALTER_NATURAL),
            ],
            // Bb
            [
                new PitchClass(MidiDefs.Step.B, MidiDefs.ALTER_FLAT),
            ],
            // B, Cb　(B優先としIndex=0に配置)    
            [
                new PitchClass(MidiDefs.Step.B, MidiDefs.ALTER_NATURAL),
                new PitchClass(MidiDefs.Step.C, MidiDefs.ALTER_FLAT),
            ],
        ];

        #endregion

        #region "properties"

        /// <summary>
        /// 移調前のキー情報
        /// </summary>
        public Key OriginalKey { get; init; }
        /// <summary>
        /// 移調前の移調情報
        /// </summary>
        public Transpose OriginalTranspose { get; init; }
        /// <summary>
        /// 移調後のキー情報
        /// </summary>
        public Key TransposedKey { get; init; }
        /// <summary>
        /// 移調後の移調情報
        /// </summary>
        public Transpose TransposedTranspose { get; init; }
        /// <summary>
        /// 移調後のクロマチックスケール
        /// </summary>
        public List<List<PitchClass>> ChromaticScale { get; init; }

        #endregion

        #region "constructors"

        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="Attributes"></param>
        public Transposition(Attributes Attributes)
        {
            //移調前の情報保存
            this.OriginalKey = Attributes.Key.Clone();
            this.OriginalTranspose = Attributes.Transpose.Clone(); 
            //移調後の情報を生成
            this.TransposedKey = GenerateTransposedKey(this.OriginalKey);
            this.TransposedTranspose = new Transpose(null, 0, 0);
            //移調後のスケールを生成
           this. ChromaticScale = PitchUtil.CreateChromaticScale(TransposedKey.Signature);
        }

        #endregion

        #region "private methods"

        /// <summary>
        /// 移調後のキー情報を生成
        /// </summary>
        /// <param name="Key"></param>
        /// <returns></returns>
        private Key GenerateTransposedKey(Key Key)
        {
            //仮にTranspose.Chromaticから新しいキーを取得
            PitchClass TempSignature = PitchUtil.GetAlteredPitchClass(Key.Signature, this.OriginalTranspose.Chromatic);
            //Transpose.Diatonicが指定されてい場合はそれに従う
            if (this.OriginalTranspose.Diatonic != null)
            {
                //指定されたStepを取得
                MidiDefs.Step TransStep = PitchUtil.GetDiatonicAlteredStep(this.OriginalKey.Signature.Step, (int)this.OriginalTranspose.Diatonic);
                //仮計算したPitchとStepが異なる場合
                if (TempSignature.Step != TransStep)
                {
                    //指定されたStepのPitchClassに変更
                    TempSignature = PitchUtil.GetEnharmonic(TempSignature, TransStep);
                }
                //サポート外ならサポートしているキーに変更(Transpose.Diatonicの指定であっても変更)
                if (!IsSupportedSignature(TempSignature))
                {
                    TempSignature = GetPreferrableSignature(TempSignature);
                }
            }
            //Transpose.Diatonicが指定されてない場合は好ましいキーを選択
            else
            {
                TempSignature = GetPreferrableSignature(TempSignature);
            }
            //Keyクラスインスタンスを生成してリターン
            return new Key(null, PitchUtil.PitchClassToFifths(TempSignature), MidiDefs.Mode.Major);
        }

        /// <summary>
        /// 指定された調号がサポートされているか判定
        /// </summary>
        /// <param name="Signature"></param>
        /// <returns></returns>
        private bool IsSupportedSignature(PitchClass Signature)
        {
            bool RetVal = false;

            //調号のルート音のC-Basedクロマチックインデックス取得
            int CBasedChromaticIndex = Signature.GetChromaticIndex(MidiDefs.CBasedRoot);
            //当該インデックスのサポート調号と比較
            foreach (PitchClass SupportedSignature in SupportedSignaturesList[CBasedChromaticIndex])
            {
                //一致するものがある場合はサポート対象
                if (Signature.Step == SupportedSignature.Step || Signature.Alter == SupportedSignature.Alter)
                {
                    RetVal = true;
                    break;
                }
            }
            return RetVal;
        }

        /// <summary>
        /// 他に表現可能な調号がある場合は好きな調号を取得
        /// </summary>
        /// <param name="Signature"></param>
        /// <returns></returns>
        private PitchClass GetPreferrableSignature(PitchClass Signature)
        {
            //調号のルート音のC-Basedクロマチックインデックス取得
            int ChromaticIndex = Signature.GetChromaticIndex(MidiDefs.CBasedRoot);
            //当該インデックスの優先調号でリターン
            return SupportedSignaturesList[ChromaticIndex][0].Clone();
        }

        #endregion
    }
}
