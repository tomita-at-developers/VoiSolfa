using Developers.MidiXml.Configurations.Models;
using Developers.MidiXml.Elements;
using System.Diagnostics;
using System.Xml.Linq;

namespace Developers.MidiXml
{

    public class Solfege
    {

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
        /// MusicXmlのノードリスト
        /// </summary>
        private List<MidiElement> MidiElms { get; set; } = [];

        /// <summary>
        /// MusicXmlのXDocument
        /// </summary>
        private XDocument? MusicXDocument { get; set; } = null;

        /// <summary>
        /// ソルフェージュ歌詞リスト
        /// </summary>
        private List<List<string>> SolfaLyrics { get; set; }

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

        /// <summary>
        /// MusicXmlファイルのインポート
        /// </summary>
        /// <param name="ImportPath"></param>
        public void XmlImport(string ImportPath)
        {
            //ノードリストの初期化
            this.MidiElms = [];
            KeyTranspose Key = new KeyTranspose();
            //Xmlドキュメントの取得
            this.MusicXDocument = XDocument.Load(ImportPath);
            //<measure>のリストを取得(パートは無視)
            IEnumerable<XElement> MeasureElms = MusicXDocument.Descendants("measure");
            foreach (XElement MeasureElm in MeasureElms)
            {
                //全ノード検索
                foreach (XElement ChildElm in MeasureElm.Elements())
                {
                    //<attributes>
                    if (ChildElm.Name.LocalName.Equals("attributes"))
                    {
                        //<key>があれば読み取り
                        if (ChildElm.Element("key") != null)
                        {
                            Key = new KeyTranspose(ChildElm);
                            MidiElms.Add(Key);
                        }
                    }
                    //<harmony>
                    else if (ChildElm.Name.LocalName.Equals("harmony"))
                    {
                        Harmony Chord = new Harmony(ChildElm);
                        MidiElms.Add(Chord);
                    }
                    //<note>
                    else if (ChildElm.Name.LocalName.Equals("note"))
                    {
                        Note Note = new Note(ChildElm, Key);
                        MidiElms.Add(Note);
                    }
                }
            }
            //デバック出力
            foreach (MidiElement node in MidiElms)
            {
                Debug.Print(node.DebugDump());
            }
        }

        /// <summary>
        /// MusicXmlファイルのエクスポート
        /// </summary>
        /// <param name="ExportPath"></param>
        public void XmlExport(string ExportPath)
        {
            //XMLファイルの保存
            MusicXDocument?.Save(ExportPath);
        }

        /// <summary>
        /// ソルフェージュ歌詞の生成
        /// </summary>
        public void CreateLyrics(string SolfaSettingName = "", bool OctaveLower = false)
        {
            //現在のキー、コード
            KeyTranspose Key = new KeyTranspose();
            Harmony? Chord = null;
            //指定された名前のソルファ設定を適用
            if (Configs.Solfas.Exists(x => x.Name.Equals(SolfaSettingName)))
            {
                this.SolfaLyrics = Configs.Solfas.Find(x => x.Name.Equals(SolfaSettingName))!.ToList();
            }
            //ソルフェージュループ
            for (int i = 0; i < MidiElms.Count; i++)
            {
                //キー
                if (MidiElms[i].GetType() == typeof(KeyTranspose))
                {
                    //キーの保存
                    Key = (KeyTranspose)MidiElms[i];
                }
                //コード
                else if (MidiElms[i].GetType() == typeof(Harmony))
                {
                    //コードのセット
                    Chord = (Harmony)MidiElms[i];
                }
                //音符
                else if (MidiElms[i].GetType() == typeof(Note))
                {
                    //歌詞のセット
                    SetLyrics(Key, Chord, i);
                    //オクターブ調整
                    if (OctaveLower)
                    {
//                        ((Note)MidiNodes[i]).AlterOctave(-1);
                    }
                }
            }
        }

        /// <summary>
        /// ソルファの取得
        /// </summary>
        /// <param name="Key"></param>
        /// <param name="Chord"></param>
        /// <param name="NodeIndex"></param>
        private void SetLyrics(KeyTranspose Key, Harmony? Chord, int NodeIndex)
        {
            //Noteの取得            
            Note Note = (Note)MidiElms[NodeIndex];

            //音符の場合
            if (Note.Pitch != null)
            {
                Lyric? Lyric = null;
                //LyricTextの取得
                string Text = GetSyllable(Key, Chord, NodeIndex);
                //タイの始まり
                if (Note.NoteChain == Note.ChainType.First)
                {
                    //lyricにはsyllabic:beginでtextをセット
                    Lyric = new Lyric(1, MidiDefs.Syllabic.Begin, Text);
                }
                //タイの途中
                else if (Note.NoteChain == Note.ChainType.Middle)
                {
                    //lyicはセットしない
                    Lyric = null;
                }
                //タイの終了
                else if (Note.NoteChain == Note.ChainType.Last)
                {
                    //lyricにはsyllabic:endでtextは空
                    Lyric = new Lyric(1, MidiDefs.Syllabic.End, "");
                }
                //単一
                else
                {
                    //lyricにはsyllabic:singleでtextをセット
                    Lyric = new Lyric(1, MidiDefs.Syllabic.Single, Text);
                }
                //Lyricの再セット
                Note.Lyrics.Clear();
                if (Lyric != null)
                {
                    Note.Lyrics.Add(Lyric);
                }
            }
            else
            {
                //lyicはセットしない
                Note.Lyrics.Clear();
            }
            Note.UpdateLyrics();
        }

        /// <summary>
        /// Lyricテキストの取得
        /// </summary>
        /// <param name="Root"></param>
        /// <param name="Chord"></param>
        /// <param name="Note"></param>
        /// <returns></returns>
        private string GetSyllable(KeyTranspose Key, Harmony? Chord, int NodeIndex)
        {
            const int SHARP = 0;
            const int FLAT = 1;

            string Text = string.Empty;

            //Noteの取得
            Note Note = (Note)MidiElms[NodeIndex];
            //念のためPitchがあることを確認
            if (Note.Pitch != null)
            {
                //【前のNote】の取得クロマチックインデックス取得(キー変更があっても無視)
                int PrevCromaticIndex = -1;
                Note? PrevNote = GetPreviousNote(NodeIndex);
                if (PrevNote != null)
                {
                    PrevCromaticIndex = PrevNote.Pitch!.GetChromaticIndex(Key.InstrumentalKey);
                }
                //【次Note】の取得クロマチックインデックス取得(キー変更があっても無視)
                int NextCromaticIndex = -1;
                Note? NextNote = GetNextNote(NodeIndex);
                if (NextNote != null)
                {
                    NextCromaticIndex = NextNote.Pitch!.GetChromaticIndex(Key.InstrumentalKey);
                }
                //クロマチックインデックスの取得
                int ChromaticIndex = Note.Pitch!.GetChromaticIndex(Key.InstrumentalKey);
                //シラブルリストの取得
                List<string> Syllables = this.SolfaLyrics[ChromaticIndex];
                //デフォルトはナチュラル(シャープと同じ位置)
                int Enharmonics = SHARP;
                string Reason = string.Empty;
                //同音異名があるとき
                if (Syllables.Count == 2)
                {
                    //もともとシャープ表現ならシャープ表現
                    if (Note.Pitch.Alter > 0)
                    {
                        if (Enharmonics != SHARP)
                        {
                            Reason = "1";
                            Enharmonics = SHARP;
                        }
                    }
                    //もともとフラット表現ならフラット表現
                    if (Note.Pitch.Alter < 0)
                    {
                        if (Enharmonics != FLAT)
                        {
                            Reason = "2";
                            Enharmonics = FLAT;
                        }
                    }
                    //シャープ系キーのナチュラル表現はフラット表現
                    if (Key.Fifths > 0 && Note.Pitch.Alter == 0)
                    {
                        if (Enharmonics != FLAT)
                        {
                            Reason = "3";
                            Enharmonics = FLAT;
                        }
                    }
                    //フラット系キーのナチュラル表現はシャープ表現
                    if (Key.Fifths < 0 && Note.Pitch.Alter == 0)
                    {
                        if (Enharmonics != SHARP)
                        {
                            Reason = "4";
                            Enharmonics = SHARP;
                        }
                    }
                    //次の音が半音上でダイアトニック音の場合ははシャープ表現
                    if (NextNote != null && NextCromaticIndex == ChromaticIndex + 1 && this.SolfaLyrics[NextCromaticIndex].Count == 1)
                    {
                        if (Enharmonics != SHARP)
                        {
                            Reason = "5";
                            Enharmonics = SHARP;
                        }
                    }
                    ////コードで判断できるとき
                    //if (Chord != null)
                    //{
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
                    //}
                }
                Text = Syllables[Enharmonics] + (this.Configs.Debug ? Reason : string.Empty);
            }
            return Text;
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
    }
}
