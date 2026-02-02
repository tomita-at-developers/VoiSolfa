using System.Xml.Linq;
using static Developers.MidiXml.Elements.MidiDefs;

namespace Developers.MidiXml.Elements
{
    /// <summary>
    /// <note>の解析
    /// </summary>
    public class Note : MidiElement
    {
        public enum ChainType
        {
            Single = 0,
            First = 1,
            Middle = 2,
            Last = 3,
        }

        public string MeasureNumber { get; init; } = string.Empty;
        public Pitch? Pitch { get; init; } = null;
        public bool Rest { get; init; } = false;
        public int? Duration { get; init; } = null;
        public List<MidiDefs.StartStop> Ties { get; init; } = [];
        public MidiDefs.NoteType? Type { get; init; } = null;
        public int Dot { get; init; } = 0;
        public TimeModification? TimeModification { get; init; } = null;
        public Notations? Notations { get; init; } = null;
        public MidiDefs.Accidental? Accidental { get; init; } = null;
        public List<Lyric> Lyrics { get; private set; } = [];
        public ChainType NoteChain { get; init; } = ChainType.Single;
        public XElement MyElement { get; init; }

        /// <summary>
        /// コンストラクタ(XDocument版)
        /// </summary>
        /// <param name="Node"></param>
        /// <exception cref="FormatException"></exception>
        /// <exception cref="ArgumentException"></exception>
        public Note(XElement Node)
        {
            //ノード保存
            this.MyElement = Node;
            //親ノードのnumber属性(小節番号)を取得
            XAttribute? MeasureNumberAttribute = Node.Parent!.Attribute("number");
            if (MeasureNumberAttribute != null)
            {
                this.MeasureNumber = MeasureNumberAttribute.Value ?? string.Empty;
            }
            //pitch/restの読み取り
            XElement? PitchNode = Node.Element("pitch");
            XElement? RestNode = Node.Element("rest");
            //pitchが存在する場合
            if (PitchNode != null)
            {
                //restが存在する場合
                if (RestNode != null)
                {
                    //無視
                }
                //<pitch>の解析()
                this.Pitch = new Pitch(PitchNode);
            }
            //restが存在する場合
            else if (RestNode != null)
            {
                this.Rest = true;
            }
            //pitchもrestもない場合は例外
            else
            {
                throw new FormatException("<note>: Invalid format.");
            }
            //<duration>
            XElement? DurationNode = Node.Element("duration");
            if (DurationNode != null)
            {
                if (!int.TryParse(DurationNode.Value, out int RawDurationInt))
                {
                    throw new ArgumentException("<note>: <duration>: Invalid value.");
                }
                this.Duration = RawDurationInt;
            }
            else
            {
                //<grace>があれば<duration>が0であるとみなす。
                XElement? GraceNode = Node.Element("grace");
                if (GraceNode != null)
                {
                    this.Duration = 0;
                }
                else
                {
                    throw new FormatException("<note>: <duration>: Not found.");
                }
            }
            //<tie>
            IEnumerable<XElement> TieNodes = Node.Elements("tie");
            foreach (XElement TieNode in TieNodes)
            {
                //type属性
                if (TieNode.Attribute("type") != null)
                {
                    string RawTieType = TieNode.Attribute("type")!.Value ?? "";
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
            XElement? TypeNode = Node.Element("type");
            if (TypeNode != null)
            {
                string RawType = TypeNode.Value ?? "";
                //値の正当性チェック
                if (!MidiDefs.TypeMembers.Exists(x => x.Key.Equals(RawType, StringComparison.OrdinalIgnoreCase)))
                {
                    throw new ArgumentException("<note>: <type>: Invalid or unsupported value.");
                }
                //値のセット
                this.Type = MidiDefs.TypeMembers.FirstOrDefault(x => x.Key.Equals(RawType, StringComparison.OrdinalIgnoreCase)).Value;
            }
            //else
            //{
            //    throw new FormatException("<type>Not found,");
            //}
            //<dot>
            IEnumerable<XElement> XDotNodes = Node.Elements("dot");
            if (XDotNodes != null && XDotNodes.Any())
            {
                this.Dot = XDotNodes.Count();
            }
            //<time-modification>
            XElement? TimeMOdificationNode = Node.Element("time-modification");
            if (TimeMOdificationNode != null)
            {
                this.TimeModification = new TimeModification(TimeMOdificationNode);
            }
            //<notations>
            XElement? NotationsNode = Node.Element("notations");
            if (NotationsNode != null)
            {
                this.Notations = new Notations(NotationsNode);
            }
            //<accidental>
            XElement? AccidentalNode = Node.Element("accidental");
            if (AccidentalNode != null)
            {
                string RawAccidental = AccidentalNode.Value ?? "";
                //値の正当性チェック
                if (!MidiDefs.AccidentalMembers.Exists(x => x.Key.Equals(RawAccidental, StringComparison.OrdinalIgnoreCase)))
                {
                    throw new ArgumentException("<note>: <accidental>: Invalid or unsupported value.");
                }
                //値のセット
                this.Accidental = MidiDefs.AccidentalMembers.FirstOrDefault(x => x.Key.Equals(RawAccidental, StringComparison.OrdinalIgnoreCase)).Value;
            }
            //<lyric>
            IEnumerable<XElement> LyricNodes = Node.Elements("lyric");
            foreach (XElement lyric in LyricNodes)
            {
                Lyrics.Add(new Lyric(lyric));
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
        /// オクターブ調整
        /// </summary>
        /// <param name="Alter"></param>
        public void AlterOctave(int Alter)
        {
            if (this.Pitch != null)
            {
                Pitch.AlterOctave(Alter);
                XElement? PitchNode = MyElement.Element("pitch");
                if (PitchNode != null)
                {
                    PitchNode.SetElementValue("octave", this.Pitch.Octave);
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
