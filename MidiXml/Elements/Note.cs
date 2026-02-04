using System.Diagnostics.Metrics;
using System.Xml.Linq;
using static Developers.MidiXml.Elements.MidiDefs;

namespace Developers.MidiXml.Elements
{
    /// <summary>
    /// <note>情報
    /// </summary>
    public class Note : MidiElement
    {
        #region "definitions"

        /// <summary>
        /// Noteのチェイン状態(タイ状態)を示す
        /// </summary>
        public enum ChainType
        {
            Single = 0,
            First = 1,
            Middle = 2,
            Last = 3,
        }

        #endregion

        #region "public properties"

        /// <summary>
        /// このインスタンスに対応するXElement
        /// </summary>
        public XElement Source { get; init; }
        public string MeasureNumber { get; init; } = string.Empty;
        public Pitch? Pitch { get; set; } = null;
        public bool Rest { get; init; } = false;
        public int? Duration { get; init; } = null;
        public List<MidiDefs.StartStop> Ties { get; init; } = [];
        public MidiDefs.NoteType? Type { get; init; } = null;
        public int Dot { get; init; } = 0;
        public TimeModification? TimeModification { get; init; } = null;
        public Notations? Notations { get; init; } = null;
        public MidiDefs.Accidental? Accidental { get; init; } = null;
        public List<Lyric> Lyrics { get; init; } = [];
        public ChainType NoteChain { get; init; } = ChainType.Single;
        public Analysis? Analysis { get; set; } = null!;

        #endregion

        #region "constructors"

        /// <summary>
        /// コンストラクタ(XDocument版)
        /// </summary>
        /// <param name="Source"></param>
        /// <exception cref="FormatException"></exception>
        /// <exception cref="ArgumentException"></exception>
        public Note(XElement Source)
        {
            //ソース読み取り
            XAttribute? AtrMeasureNumber = Source.Parent!.Attribute("number");
            XElement? ElmPitch = Source.Element("pitch");
            XElement? ElmRest = Source.Element("rest");
            XElement? ElmDuration = Source.Element("duration");
            IEnumerable<XElement> ElmTies = Source.Elements("tie");
            XElement? ElmType = Source.Element("type");
            IEnumerable<XElement> ElmDots = Source.Elements("dot");
            XElement? ElmTimeMOdification = Source.Element("time-modification");
            XElement? ElmNotations = Source.Element("notations");
            XElement? ElmAccidental = Source.Element("accidental");
            XElement? ElmGrace = Source.Element("grace");
            IEnumerable<XElement> ElmLyrics = Source.Elements("lyric");

            //ノード保存
            this.Source = Source;
            //親ノードのnumber属性(小節番号)を取得
            if (AtrMeasureNumber != null)
            {
                this.MeasureNumber = AtrMeasureNumber.Value ?? string.Empty;
            }
            //<pitch>が存在する場合
            if (ElmPitch != null)
            {
                //restが存在する場合
                if (ElmRest != null)
                {
                    //無視
                }
                //<pitch>の解析()
                this.Pitch = new Pitch(ElmPitch);
            }
            //<rest>が存在する場合
            else if (ElmRest != null)
            {
                this.Rest = true;
            }
            //pitchもrestもない場合は例外
            else
            {
                throw new FormatException("<note>: Invalid format.");
            }
            //<duration>
            if (ElmDuration != null)
            {
                if (!int.TryParse(ElmDuration.Value, out int RawDurationInt))
                {
                    throw new ArgumentException("<note>: <duration>: Invalid value.");
                }
                this.Duration = RawDurationInt;
            }
            else
            {
                //<grace>があれば<duration>が0であるとみなす。
                if (ElmGrace != null)
                {
                    this.Duration = 0;
                }
                else
                {
                    throw new FormatException("<note>: <duration>: Not found.");
                }
            }
            //<tie>
            foreach (XElement TieElm in ElmTies)
            {
                //type属性
                if (TieElm.Attribute("type") != null)
                {
                    string RawTieType = TieElm.Attribute("type")!.Value ?? "";
                    //値の正当性チェック
                    if (!MidiDefs.TieTypeMembers.Exists(x => x.Key.Equals(RawTieType, StringComparison.OrdinalIgnoreCase)))
                    {
                        throw new ArgumentException("<note>: <tie type>: Invalid value.");
                    }
                    //値のセット
                    this.Ties.Add(MidiDefs.TieTypeMembers.FirstOrDefault(x => x.Key.Equals(RawTieType, StringComparison.OrdinalIgnoreCase)).Value);
                }
                else
                {
                    throw new FormatException("<note>: <tie type>: Not found.");
                }
            }
            //<type>
            if (ElmType != null)
            {
                string RawType = ElmType.Value ?? "";
                //値の正当性チェック
                if (!MidiDefs.TypeMembers.Exists(x => x.Key.Equals(RawType, StringComparison.OrdinalIgnoreCase)))
                {
                    throw new ArgumentException("<note>: <type>: Invalid or unsupported value.");
                }
                //値のセット
                this.Type = MidiDefs.TypeMembers.FirstOrDefault(x => x.Key.Equals(RawType, StringComparison.OrdinalIgnoreCase)).Value;
            }
            //<dot>
            if (ElmDots != null && ElmDots.Any())
            {
                this.Dot = ElmDots.Count();
            }
            //<time-modification>
            if (ElmTimeMOdification != null)
            {
                this.TimeModification = new TimeModification(ElmTimeMOdification);
            }
            //<notations>
            if (ElmNotations != null)
            {
                this.Notations = new Notations(ElmNotations);
            }
            //<accidental>
            if (ElmAccidental != null)
            {
                string RawAccidental = ElmAccidental.Value ?? "";
                //値の正当性チェック
                if (!MidiDefs.AccidentalMembers.Exists(x => x.Key.Equals(RawAccidental, StringComparison.OrdinalIgnoreCase)))
                {
                    throw new ArgumentException("<note>: <accidental>: Invalid or unsupported value.");
                }
                //値のセット
                this.Accidental = MidiDefs.AccidentalMembers.FirstOrDefault(x => x.Key.Equals(RawAccidental, StringComparison.OrdinalIgnoreCase)).Value;
            }
            //<lyric>
            foreach (XElement ElmLyirc in ElmLyrics)
            {
                Lyrics.Add(new Lyric(ElmLyirc));
            }
            //タイ状態のセット
            string TieChain = string.Empty;
            foreach (MidiDefs.StartStop tie in Ties)
            {
                TieChain += tie.ToString();
            }
            if (TieChain == string.Empty)
            {
                NoteChain = ChainType.Single;
            }
            if (TieChain.StartsWith(MidiDefs.StartStop.Start.ToString()) && TieChain.StartsWith(MidiDefs.StartStop.Start.ToString()))
            {
                NoteChain = ChainType.First;
            }
            if (TieChain.StartsWith(MidiDefs.StartStop.Start.ToString()) && TieChain.StartsWith(MidiDefs.StartStop.Stop.ToString()))
            {
                NoteChain = ChainType.Single;
            }
            if (TieChain.StartsWith(MidiDefs.StartStop.Stop.ToString()) && TieChain.StartsWith(MidiDefs.StartStop.Start.ToString()))
            {
                NoteChain = ChainType.Middle;
            }
            if (TieChain.StartsWith(MidiDefs.StartStop.Stop.ToString()) && TieChain.StartsWith(MidiDefs.StartStop.Stop.ToString()))
            {
                NoteChain = ChainType.Last;
            }
        }

        #endregion

        #region "public methods"

        /// <summary>
        /// トランスポーズ
        /// </summary>
        /// <param name="OrigKey"></param>
        /// <param name="TargKey"></param>
        /// <param name="AlterOctave"></param>
        public void TransposeToConcertKey(Key Key)
        {
            //Pitchがあるときのみ処理(休符はスルー)
            if (Pitch != null)
            {
                ////移調記述されている場合はコンサートキーに変更
                //if (Key.TransposeChromatic != 0)
                //{
                //    this.Pitch.TransposeToConcertKey(Key);
                //    UpdateXmlPitch();
                //}
            }
        }

        /// <summary>
        /// ソルファの取得
        /// </summary>
        /// <param name="Key"></param>
        /// <param name="Chord"></param>
        /// <param name="NodeIndex"></param>
        public void SetLyrics(List<List<string>> SolfaLyrics, bool Debug)
        {
            //音符の場合
            if (this.Pitch != null)
            {
                Lyric? Lyric = null;
                //歌詞文字列の取得
                string Text = SolfaLyrics[this.Analysis!.ChromaticIndex][this.Analysis!.EnharmonicIndex];
                //デバック時はDescriptionを追記
                Text += Debug ? this.Analysis!.Description : string.Empty;

                //タイの始まり
                if (this.NoteChain == Note.ChainType.First)
                {
                    //lyricにはsyllabic:beginでtextをセット
                    Lyric = new Lyric(1, MidiDefs.Syllabic.Begin, Text);
                }
                //タイの途中
                else if (this.NoteChain == Note.ChainType.Middle)
                {
                    //lyicはセットしない
                    Lyric = null;
                }
                //タイの終了
                else if (this.NoteChain == Note.ChainType.Last)
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
                this.Lyrics.Clear();
                if (Lyric != null)
                {
                    this.Lyrics.Add(Lyric);
                }
            }
            else
            {
                //lyicはセットしない
                this.Lyrics.Clear();
            }
            //XDocumentへの反映
            UpdateXmlLyrics();
        }

        ///// <summary>
        ///// オクターブ変更
        ///// </summary>
        ///// <param name="Alter"></param>
        //public void AlterXmlOctave(int Alter)
        //{
        //    //Pitchがあるときのみ処理(休符はスルー)
        //    if (Pitch != null)
        //    {
        //        this.Pitch.AlterOctave(Alter);
        //        UpdateXmlPitch();
        //    }
        //}

        #endregion

        #region "private methods"

        /// <summary>
        /// pitchの更新
        /// </summary>
        private void UpdateXmlPitch()
        {
            XElement? PitchElm = Source.Element("pitch");
            if (PitchElm != null)
            {
                PitchElm.Remove();
                if (this.Pitch != null)
                {
                    Source.Add(this.Pitch.Serialize());
                }
            }
        }

        /// <summary>
        /// LyricをXDoucmentにシリアライズする
        /// </summary>
        private void UpdateXmlLyrics()
        {
            //このNoteノードが持つすべての<lyric>を削除する
            IEnumerable<XElement> LyricElms = this.Source!.Elements("lyric");
            foreach (XElement LyricElm in LyricElms)
            {
                LyricElm.RemoveAll();
            }
            //Lyricリストに存在するLyricインスタンスを<lyric>ノードとしてこのNoteノードに追加する
            foreach (Lyric Lyric in this.Lyrics)
            {
                XElement LyricElm = Lyric.Serialize();
                this.Source.Add(LyricElm);
            }
        }

        #endregion

        #region "public methods"

        /// <summary>
        /// デバック用ダンプ
        /// </summary>
        /// <returns></returns>
        public override string DebugDump()
        {
            string Dump = string.Empty;

            Dump += "<note>";
            Dump += Pitch != null ? Pitch.DebugDump() : "";
            Dump += Rest ? "<rest/>" : "";
            Dump += Duration != null ? "<duration>" + Duration.ToString() : "";
            foreach (MidiDefs.StartStop tie in Ties)
            {
                Dump += "<tie type=" + tie.ToString() + ">";
            }
            Dump += Type != null ? "<type>" + Type.ToString() : "";
            Dump += string.Concat(Enumerable.Repeat("<dot/>", (int)Dot));
            Dump += TimeModification != null ? TimeModification.DebugDump() : "";
            Dump += Notations != null ? Notations.DebugDump() : "";
            Dump += Accidental != null ? "<accidental>" + Accidental.ToString() : "";
            foreach (Lyric l in Lyrics)
            {
                Dump += l.DebugDump();
            }
            if (this.Analysis != null)
            {
                Dump += this.Analysis.DebugDump();
            }
            return Dump;
        }

        #endregion
    }
}
