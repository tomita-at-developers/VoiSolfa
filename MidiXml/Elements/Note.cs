using System.Diagnostics.Metrics;
using System.Xml.Linq;
using static Developers.MidiXml.Elements.MidiDefs;

namespace Developers.MidiXml.Elements
{
    /// <summary>
    /// <note>の解析
    /// </summary>
    public class Note : MidiElement
    {
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

        /// <summary>
        /// このインスタンスに対応するXElement
        /// </summary>
        public XElement SourceElm { get; init; }
        private KeyTranspose? Key { get; init; } = null;
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

        /// <summary>
        /// コンストラクタ(XDocument版)
        /// </summary>
        /// <param name="SourceElm"></param>
        /// <param name="Key"></param>
        /// <exception cref="FormatException"></exception>
        /// <exception cref="ArgumentException"></exception>
        public Note(XElement SourceElm, KeyTranspose Key)
        {
            //ソース読み取り
            XAttribute? MeasureNumberAtr = SourceElm.Parent!.Attribute("number");
            XElement? PitchElm = SourceElm.Element("pitch");
            XElement? RestElm = SourceElm.Element("rest");
            XElement? DurationElm = SourceElm.Element("duration");
            IEnumerable<XElement> TieElms = SourceElm.Elements("tie");
            XElement? TypeElm = SourceElm.Element("type");
            IEnumerable<XElement> DotElms = SourceElm.Elements("dot");
            XElement? TimeMOdificationElm = SourceElm.Element("time-modification");
            XElement? NotationsElm = SourceElm.Element("notations");
            XElement? AccidentalElm = SourceElm.Element("accidental");
            IEnumerable<XElement> LyricElms = SourceElm.Elements("lyric");

            //ノード保存
            this.SourceElm = SourceElm;
            //キー情報の保存
            this.Key = Key;
            //親ノードのnumber属性(小節番号)を取得
            if (MeasureNumberAtr != null)
            {
                this.MeasureNumber = MeasureNumberAtr.Value ?? string.Empty;
            }
            //<pitch>が存在する場合
            if (PitchElm != null)
            {
                //restが存在する場合
                if (RestElm != null)
                {
                    //無視
                }
                //<pitch>の解析()
                this.Pitch = new Pitch(PitchElm, Key);
            }
            //<rest>が存在する場合
            else if (RestElm != null)
            {
                this.Rest = true;
            }
            //pitchもrestもない場合は例外
            else
            {
                throw new FormatException("<note>: Invalid format.");
            }
            //<duration>
            if (DurationElm != null)
            {
                if (!int.TryParse(DurationElm.Value, out int RawDurationInt))
                {
                    throw new ArgumentException("<note>: <duration>: Invalid value.");
                }
                this.Duration = RawDurationInt;
            }
            else
            {
                //<grace>があれば<duration>が0であるとみなす。
                XElement? GraceElm = SourceElm.Element("grace");
                if (GraceElm != null)
                {
                    this.Duration = 0;
                }
                else
                {
                    throw new FormatException("<note>: <duration>: Not found.");
                }
            }
            //<tie>
            foreach (XElement TieElm in TieElms)
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
            if (TypeElm != null)
            {
                string RawType = TypeElm.Value ?? "";
                //値の正当性チェック
                if (!MidiDefs.TypeMembers.Exists(x => x.Key.Equals(RawType, StringComparison.OrdinalIgnoreCase)))
                {
                    throw new ArgumentException("<note>: <type>: Invalid or unsupported value.");
                }
                //値のセット
                this.Type = MidiDefs.TypeMembers.FirstOrDefault(x => x.Key.Equals(RawType, StringComparison.OrdinalIgnoreCase)).Value;
            }
            //<dot>
            if (DotElms != null && DotElms.Any())
            {
                this.Dot = DotElms.Count();
            }
            //<time-modification>
            if (TimeMOdificationElm != null)
            {
                this.TimeModification = new TimeModification(TimeMOdificationElm);
            }
            //<notations>
            if (NotationsElm != null)
            {
                this.Notations = new Notations(NotationsElm);
            }
            //<accidental>
            if (AccidentalElm != null)
            {
                string RawAccidental = AccidentalElm.Value ?? "";
                //値の正当性チェック
                if (!MidiDefs.AccidentalMembers.Exists(x => x.Key.Equals(RawAccidental, StringComparison.OrdinalIgnoreCase)))
                {
                    throw new ArgumentException("<note>: <accidental>: Invalid or unsupported value.");
                }
                //値のセット
                this.Accidental = MidiDefs.AccidentalMembers.FirstOrDefault(x => x.Key.Equals(RawAccidental, StringComparison.OrdinalIgnoreCase)).Value;
            }
            //<lyric>
            foreach (XElement LyircElm in LyricElms)
            {
                Lyrics.Add(new Lyric(LyircElm));
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

        /// <summary>
        /// トランスポーズ
        /// </summary>
        /// <param name="OrigKey"></param>
        /// <param name="TargKey"></param>
        /// <param name="AlterOctave"></param>
        public void Transpose(Pitch OrigKey, Pitch TargKey, int Direction)
        {
            //Pitchがあるときのみ処理(休符はスルー)
            if (Pitch != null)
            {
                this.Pitch.Transpose(OrigKey, TargKey, Direction);
                UpdatePitch();
            }
        }

        /// <summary>
        /// オクターブ変更
        /// </summary>
        /// <param name="Alter"></param>
        public void AlterOctave(int Alter)
        {
            //Pitchがあるときのみ処理(休符はスルー)
            if (Pitch != null)
            {
                this.Pitch.AlterOctave(Alter);
                UpdatePitch();
            }
        }

        /// <summary>
        /// LyricをXDoucmentにシリアライズする
        /// </summary>
        public void UpdateLyrics()
        {
            //このNoteノードが持つすべての<lyric>を削除する
            IEnumerable<XElement> LyricElms = this.SourceElm!.Elements("lyric");
            foreach (XElement LyricElm in LyricElms)
            {
                LyricElm.RemoveAll();
            }
            //Lyricリストに存在するLyricインスタンスを<lyric>ノードとしてこのNoteノードに追加する
            foreach (Lyric Lyric in this.Lyrics)
            {
                XElement LyricElm = Lyric.Serialize();
                this.SourceElm.Add(LyricElm);
            }
        }

        /// <summary>
        /// pitchの更新
        /// </summary>
        private void UpdatePitch()
        {
            XElement? PitchElm = SourceElm.Element("pitch");
            if (PitchElm != null)
            {
                PitchElm.Remove();
                if (this.Pitch != null)
                {
                    SourceElm.Add(this.Pitch.Serialize());
                }
            }
        }


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
            return Dump;
        }
    }
}
