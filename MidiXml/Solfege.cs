using Developers.MidiXml.Configurations.Models;
using Developers.MidiXml.Elements;
using System.Diagnostics;
using System.Xml.Linq;

namespace Developers.MidiXml
{

    public class Solfege
    {

        #region "private properties"

        /// <summary>
        /// 設定ファイルマネージャ
        /// </summary>
        private Configurations.ConfigurationManager Configs { get; set; } = new Configurations.ConfigurationManager();
        /// <summary>
        /// Solfa設定名リスト
        /// </summary>
        public List<string> SofaSettingNames
        {
            get
            {
                List<string> RetVal = [];
                foreach (Solfa s in Configs.Solfas)
                {
                    RetVal.Add(s.Name);
                }
                return RetVal;
            }
        }
        /// <summary>
        /// ソルフェージュ歌詞リスト
        /// </summary>
        private List<List<string>> SolfaLyrics { get; set; }
        /// <summary>
        /// MusicXmlのノードリスト
        /// </summary>
        private List<MidiElement> MidiElms { get; set; } = [];
        /// <summary>
        /// MusicXmlのXDocument
        /// </summary>
        private XDocument? MusicXDocument { get; set; } = null;

        #endregion

        #region "constructors"

        /// <summary>
        /// コンストラクタ
        /// </summary>
        public Solfege()
        {
            //設定ファイルの読み取り
            Configs.Read();
            //デフォルトのソルファ設定をセット
            SolfaLyrics = Configs.DefaultSolfa.ToList();
        }

        #endregion

        #region "public nethods"

        /// <summary>
        /// MusicXmlファイルのインポート
        /// </summary>
        /// <param name="ImportPath"></param>
        public void XmlImport(string ImportPath)
        {
            //ノードリストの初期化
            this.MidiElms = [];
            //Xmlドキュメントの取得
            this.MusicXDocument = XDocument.Load(ImportPath);
            //<measure>のリストを取得(パートは無視)
            IEnumerable<XElement> ElmMeasures = MusicXDocument.Descendants("measure");
            foreach (XElement ElmMeasure in ElmMeasures)
            {
                //全ノード検索
                foreach (XElement ElmMeasureContent in ElmMeasure.Elements())
                {
                    //<attributes>
                    if (ElmMeasureContent.Name.LocalName.Equals("attributes"))
                    {
                        //<key>を含む場合のみ処理する
                        if (ElmMeasureContent.Element("key") != null)
                        {
                            MidiElms.Add(new Attributes(ElmMeasureContent));
                        }
                    }
                    //<harmony>
                    else if (ElmMeasureContent.Name.LocalName.Equals("harmony"))
                    {
                        MidiElms.Add(new Harmony(ElmMeasureContent));
                    }
                    //<note>
                    else if (ElmMeasureContent.Name.LocalName.Equals("note"))
                    {
                        MidiElms.Add(new Note(ElmMeasureContent));
                    }
                }
            }
            //アナリーゼを実行
            Analyze();
            //デバック出力
            DebugPrint("********** Import **********");
        }

        /// <summary>
        /// MusicXmlファイルのエクスポート
        /// </summary>
        /// <param name="ExportPath"></param>
        public void XmlExport(string ExportPath)
        {
            //デバック出力
            DebugPrint("********** Export **********");
            //XMLファイルの保存
            MusicXDocument?.Save(ExportPath);
        }

        /// <summary>
        /// ソルフェージュ歌詞の生成
        /// </summary>
        public void CreateLyrics(string SolfaSettingName = "")
        {
            //指定された名前のソルファ設定を適用
            if (Configs.Solfas.Exists(x => x.Name.Equals(SolfaSettingName)))
            {
                this.SolfaLyrics = Configs.Solfas.Find(x => x.Name.Equals(SolfaSettingName))!.ToList();
            }
            //ソルフェージュループ
            for (int i = 0; i < MidiElms.Count; i++)
            {
                //音符の場合は歌詞をセット
                if (MidiElms[i].GetType() == typeof(Note))
                {
                    ((Note)MidiElms[i]).SetLyrics(this.SolfaLyrics, Configs.Debug);
                }
            }
            //デバック出力
            DebugPrint("********** CreateLyrics **********");
        }

        /// <summary>
        /// 移調楽器向けの表記を実音表記に移調する
        /// </summary>
        public void TransposeToConcerKey()
        {
            //移調情報
            Transposition? TransposeInfo = null;
            //転調ループ
            for (int i = 0; i < MidiElms.Count; i++)
            {
                //キーの移調
                if (MidiElms[i].GetType() == typeof(Attributes))
                {
                    //移調情報を作成する
                    Attributes Attr = (Attributes)MidiElms[i];
                    TransposeInfo = new Transposition(Attr);
                    //移調する
                    Attr.TransposeToConcertKey(TransposeInfo);
                }
                //コードの移調
                else if (MidiElms[i].GetType() == typeof(Harmony))
                {
                    if (TransposeInfo != null)
                    {
                        ((Harmony)MidiElms[i]).Transpose(TransposeInfo);
                    }
                }
                //音符の移調
                else if (MidiElms[i].GetType() == typeof(Note))
                {
                    if (TransposeInfo != null)
                    {
                        ((Note)MidiElms[i]).Transpose(TransposeInfo);
                    }
                }
            }
            //デバック出力
            DebugPrint("********** TransposeToConcertKey **********");
        }

        #endregion

        #region "private methods"

        /// <summary>
        /// MidiElmsの音楽的解析(NoteとHarmony)
        /// </summary>
        private void Analyze()
        {
            //現在のキー(初期値はC-Major)
            Key CurrentKey = new Key(null, 0, MidiDefs.Mode.Minor);
            //現在のコード(初期値はnull)
            Harmony? CurrentChord = null;
            //解析ループ
            for (int i = 0; i < MidiElms.Count; i++)
            {
                //キーの移調
                if (MidiElms[i].GetType() == typeof(Attributes))
                {
                    //キャスト
                    Attributes Attr = (Attributes)MidiElms[i];
                    //キーの保存
                    if (Attr.Key != null)
                    {
                        CurrentKey = Attr.Key;
                    }
                }
                //コード
                else if (MidiElms[i].GetType() == typeof(Harmony))
                {
                    //アナリーゼのセット
                    AnalyzeChord(CurrentKey, i);
                    //コードの保存
                    CurrentChord = (Harmony)MidiElms[i];
                }
                //音符
                else if (MidiElms[i].GetType() == typeof(Note))
                {
                    //アナリーゼのセット
                    AnalyzeNote(CurrentKey, CurrentChord, i);
                }
            }
        }

        /// <summary>
        /// NoteのAnalysisセット
        /// </summary>
        /// <param name="Key"></param>
        /// <param name="Chord"></param>
        /// <param name="ElementIndex"></param>
        private void AnalyzeNote(Key Key, Harmony? Chord, int ElementIndex)
        {
            const int SHARP = 0;
            const int FLAT = 1;

            //Noteの取得            
            Note Note = (Note)MidiElms[ElementIndex];
            //音符の場合のみ処理
            if (Note.Pitch != null)
            {
                //【前のNote】のクロマチックインデックス取得(キー変更があっても無視)
                int PrevCromaticIndex = -1;
                Note? PrevNote = GetPreviousNote(ElementIndex);
                if (PrevNote != null)
                {
                    PrevCromaticIndex = PrevNote.Pitch!.GetChromaticIndex(Key.Signature);
                }
                //【次Note】のクロマチックインデックス取得(キー変更があっても無視)
                int NextCromaticIndex = -1;
                Note? NextNote = GetNextNote(ElementIndex);
                if (NextNote != null)
                {
                    NextCromaticIndex = NextNote.Pitch!.GetChromaticIndex(Key.Signature);
                }
                //【自Note】のクロマチックインデックスの取得
                int ChromaticIndex = Note.Pitch!.GetChromaticIndex(Key.Signature);
                //デフォルトはナチュラル(シャープと同じ位置)
                int EnharmonicsIndex = SHARP;
                string Reason = string.Empty;
                //同音異名があるとき
                if (PitchUtil.CBasedChromaticScale[ChromaticIndex].Count == 2)
                {
                    //もともとシャープ表現ならシャープ表現
                    if (Note.Pitch.Alter > 0)
                    {
                        if (EnharmonicsIndex != SHARP)
                        {
                            Reason = "1";
                            EnharmonicsIndex = SHARP;
                        }
                    }
                    //もともとフラット表現ならフラット表現
                    if (Note.Pitch.Alter < 0)
                    {
                        if (EnharmonicsIndex != FLAT)
                        {
                            Reason = "2";
                            EnharmonicsIndex = FLAT;
                        }
                    }
                    //シャープ系キーのナチュラル表現はフラット表現
                    if (Key.Fifths > 0 && Note.Pitch.Alter == 0)
                    {
                        if (EnharmonicsIndex != FLAT)
                        {
                            Reason = "3";
                            EnharmonicsIndex = FLAT;
                        }
                    }
                    //フラット系キーのナチュラル表現はシャープ表現
                    if (Key.Fifths < 0 && Note.Pitch.Alter == 0)
                    {
                        if (EnharmonicsIndex != SHARP)
                        {
                            Reason = "4";
                            EnharmonicsIndex = SHARP;
                        }
                    }
                    //次の音が半音上でダイアトニック音の場合ははシャープ表現
                    if (NextNote != null && NextCromaticIndex == ChromaticIndex + 1 && this.SolfaLyrics[NextCromaticIndex].Count == 1)
                    {
                        if (EnharmonicsIndex != SHARP)
                        {
                            Reason = "5";
                            EnharmonicsIndex = SHARP;
                        }
                    }
                    //コードで判断できるとき
                    if (Chord != null)
                    {
                        //    //ド#/レbの特例
                        //    if (ChromaticIndex == 1)
                        //    {
                        //        //V|7の場合はシャープ系
                        //        if (Chord.Root.PitchClass.GetChromaticIndex(Root) == 9 &&
                        //           (Chord.KindString.StartsWith("dominant")|| (Chord.KindString.StartsWith("major"))))
                        //        {
                        //            Reason = "5";
                        //            Text = Syllables[0];
                        //        }
                        //        //I|m7の場合はシャープ系
                        //        if (Chord.Root.PitchClass.GetChromaticIndex(Root) == 2 &&
                        //           (Chord.KindString.StartsWith("minor") || (Chord.KindString.StartsWith("major"))))
                        //        {
                        //            Reason = "6";
                        //            Text = Syllables[0];
                        //        }
                        //    }
                        //    //ファ#/ソbの特例
                        //    if (ChromaticIndex == 8)
                        //    {
                        //        //II7の場合はシャープ系
                        //        if (Chord.Root.PitchClass.GetChromaticIndex(Root) == 2 &&
                        //           (Chord.KindString.StartsWith("dominant") || (Chord.KindString.StartsWith("major"))))
                        //        {
                        //            Reason = "7";
                        //            Text = Syllables[0];
                        //        }
                        //    }
                        //    //ソ#/ラbの特例
                        //    if (ChromaticIndex == 8)
                        //    {
                        //        //III7の場合はシャープ系
                        //        if (Chord.Root.PitchClass.GetChromaticIndex(Root) == 4 &&
                        //           (Chord.KindToString.StartsWith("dominant") || (Chord.KindString.StartsWith("major"))))
                        //        {
                        //            Reason = "8";
                        //            Text = Syllables[0];
                        //        }
                        //    }
                    }
                }
                //アナリーゼのセット
                Note.Analysis = new Analysis(ChromaticIndex, EnharmonicsIndex, Reason);
            }
        }

        /// <summary>
        /// HarmonyのAnalysisセット
        /// </summary>
        /// <param name="Key"></param>
        /// <param name="NodeIndex"></param>
        private void AnalyzeChord(Key Key, int ElementIndex)
        {
            const int SHARP = 0;
            const int FLAT = 1;

            //Noteの取得            
            Harmony Harmony = (Harmony)MidiElms[ElementIndex];
            //【前のHarmony】のクロマチックインデックス取得(キー変更があっても無視)
            int PrevCromaticIndex = -1;
            Harmony? PrevHarmony = GetPreviousHarmony(ElementIndex);
            if (PrevHarmony != null)
            {
                PrevCromaticIndex = PrevHarmony.Root.PitchClass.GetChromaticIndex(Key.Signature);
            }
            //【次のHarmony】のクロマチックインデックス取得(キー変更があっても無視)
            int NextCromaticIndex = -1;
            Harmony? NextHarmony = GetNextHarmony(ElementIndex);
            if (NextHarmony != null)
            {
                NextCromaticIndex = NextHarmony.Root.PitchClass.GetChromaticIndex(Key.Signature);
            }
            //【自Harmony】のクロマチックインデックスの取得
            int ChromaticIndex = Harmony.Root.PitchClass.GetChromaticIndex(Key.Signature);
            //デフォルトはナチュラル(シャープと同じ位置)
            int EnharmonicIndex = SHARP;
            string Reason = string.Empty;
            //同音異名があるとき
            if (PitchUtil.CBasedChromaticScale[ChromaticIndex].Count == 2)
            {
                //もともとシャープ表現ならシャープ表現
                if (Harmony.Root.PitchClass.Alter > 0)
                {
                    if (EnharmonicIndex != SHARP)
                    {
                        Reason = "1";
                        EnharmonicIndex = SHARP;
                    }
                }
                //もともとフラット表現ならフラット表現
                if (Harmony.Root.PitchClass.Alter < 0)
                {
                    if (EnharmonicIndex != FLAT)
                    {
                        Reason = "2";
                        EnharmonicIndex = FLAT;
                    }
                }
                //シャープ系キーのナチュラル表現はフラット表現
                if (Key.Fifths > 0 && Harmony.Root.PitchClass.Alter == 0)
                {
                    if (EnharmonicIndex != FLAT)
                    {
                        Reason = "3";
                        EnharmonicIndex = FLAT;
                    }
                }
                //フラット系キーのナチュラル表現はシャープ表現
                if (Key.Fifths < 0 && Harmony.Root.PitchClass.Alter == 0)
                {
                    if (EnharmonicIndex != SHARP)
                    {
                        Reason = "4";
                        EnharmonicIndex = SHARP;
                    }
                }
                //次の音が半音上でダイアトニック音の場合ははシャープ表現
                if (NextHarmony != null && NextCromaticIndex == ChromaticIndex + 1 && this.SolfaLyrics[NextCromaticIndex].Count == 1)
                {
                    if (EnharmonicIndex != SHARP)
                    {
                        Reason = "5";
                        EnharmonicIndex = SHARP;
                    }
                }
            }
            //アナリーゼのセット
            Harmony.Analysis = new Analysis(ChromaticIndex, EnharmonicIndex, Reason);
        }

        /// <summary>
        /// 次の音の取得
        /// </summary>
        /// <param name="CurrentIndex"></param>
        /// <param name="IucludeRest">休符を含む場合true</param>
        /// <param name="IncludeTied">タイで連結された後続Noteを含む場合true</param>
        /// <returns></returns>
        private Note? GetNextNote(int CurrentIndex, bool IucludeRest = false, bool IncludeTied = false)
        {
            Note? NextNote = null;
            //次ノートの検索
            for (int i = CurrentIndex + 1; i < MidiElms.Count; i++)
            {
                //Noteの場合
                if (MidiElms[i].GetType() == typeof(Note))
                {
                    if (NoteMatches((Note)MidiElms[i], IucludeRest, IncludeTied))
                    {
                        NextNote = (Note)MidiElms[i];
                        break;
                    }
                }
            }
            return NextNote;
        }

        /// <summary>
        /// 前の音の取得
        /// </summary>
        /// <param name="CurrentIndex"></param>
        /// <param name="IucludeRest">休符を含む場合true</param>
        /// <param name="IncludeTied">タイで連結された後続Noteを含む場合true</param>
        /// <returns></returns>
        private Note? GetPreviousNote(int CurrentIndex, bool IucludeRest = false, bool IncludeTied = false)
        {
            Note? NextNote = null;
            //次ノートの検索
            for (int i = CurrentIndex - 1; i >= 0; i--)
            {
                //Noteの場合
                if (MidiElms[i].GetType() == typeof(Note))
                {
                    if (NoteMatches((Note)MidiElms[i], IucludeRest, IncludeTied))
                    {
                        NextNote = (Note)MidiElms[i];
                        break;
                    }
                }
            }
            return NextNote;
        }

        /// <summary>
        /// Noteが条件にマッチするか判定する
        /// </summary>
        /// <param name="Note">判定対象Note</param>
        /// <param name="IucludeRest">休符を含む場合true</param>
        /// <param name="IncludeTied">タイで連結された後続Noteを含む場合true</param>
        /// <returns></returns>
        private bool NoteMatches(Note Note, bool IucludeRest, bool IncludeTied)
        {
            bool RetVal = false;
            //音符の場合
            if (Note.Pitch != null)
            {
                //タイ要素も含む設定の場合
                if (IncludeTied)
                {
                    RetVal = true;
                }
                else
                {
                    //単音、またはタイの先頭なら受け入れる
                    if (Note.NoteChain == Note.ChainType.Single || Note.NoteChain == Note.ChainType.First)
                    {
                        RetVal = true;
                    }
                }
            }
            //休符の場合
            else if (Note.Rest)
            {
                //休符を含む設定の場合
                if (IucludeRest)
                {
                    RetVal = true;
                }
            }
            return RetVal;
        }

        /// <summary>
        /// 次のコードの取得
        /// </summary>
        /// <param name="CurrentIndex"></param>
        /// <returns></returns>
        private Harmony? GetNextHarmony(int CurrentIndex)
        {
            Harmony? NextHarmony = null;
            //次ノートの検索
            for (int i = CurrentIndex + 1; i < MidiElms.Count; i++)
            {
                //コードの場合
                if (MidiElms[i].GetType() == typeof(Harmony))
                {
                    NextHarmony = (Harmony)MidiElms[i];
                    break;
                }
            }
            return NextHarmony;
        }

        /// <summary>
        /// 前のコードの取得
        /// </summary>
        /// <param name="CurrentIndex"></param>
        /// <returns></returns>
        private Harmony? GetPreviousHarmony(int CurrentIndex)
        {
            Harmony? PrevHarmony = null;
            //前ノートの検索
            for (int i = CurrentIndex - 1; i >= 0; i--)
            {
                //コードの場合
                if (MidiElms[i].GetType() == typeof(Harmony))
                {
                    PrevHarmony = (Harmony)MidiElms[i];
                    break;
                }
            }
            return PrevHarmony;
        }

        #endregion

        #region "debug methods"

        /// <summary>
        /// デバック用のダンプ出力
        /// </summary>
        private void DebugPrint(string Separetor)
        {
            if (!string.IsNullOrEmpty(Separetor))
            {
                Debug.Print(Separetor);
            }
            //デバック出力
            foreach (MidiElement node in MidiElms)
            {
                Debug.Print(node.DebugDump());
            }
        }

        #endregion
    }
}
