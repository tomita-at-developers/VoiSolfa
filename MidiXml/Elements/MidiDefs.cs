using System.ComponentModel;
using System.Reflection;

namespace Developers.MidiXml.Elements
{
    /// <summary>
    /// MusicXmlに関する各種定義
    /// </summary>
    public static class MidiDefs
    {
        //半音操作
        public const int ALTER_DOUBLE_SHARP = 2;
        public const int ALTER_SHARP = 1;
        public const int ALTER_NATURAL = 0;
        public const int ALTER_FLAT = -1;
        public const int ALTER_DOUBLE_FLAT = -2;

        //オクターブ中央
        public const int OCTAVE_CENTER = 4;

        //ダイアトニック度数
        public const int DIATONIC_I = 0;
        public const int DIATONIC_II = 1;
        public const int DIATONIC_III = 2;
        public const int DIATONIC_IV = 3;
        public const int DIATONIC_V = 4;
        public const int DIATONIC_VI = 5;
        public const int DIATONIC_VII = 7;

        /// <summary>
        /// C-Basedのルート音
        /// </summary>
        public static readonly PitchClass CBasedRoot = new PitchClass(Step.C, ALTER_NATURAL);

        public static readonly List<KeyValuePair<string, Step>> StepMembers =
        [
            new KeyValuePair<string, Step>(GetEnumDescription(Step.C), Step.C),
            new KeyValuePair<string, Step>(GetEnumDescription(Step.D), Step.D),
            new KeyValuePair<string, Step>(GetEnumDescription(Step.E), Step.E),
            new KeyValuePair<string, Step>(GetEnumDescription(Step.F), Step.F),
            new KeyValuePair<string, Step>(GetEnumDescription(Step.G), Step.G),
            new KeyValuePair<string, Step>(GetEnumDescription(Step.A), Step.A),
            new KeyValuePair<string, Step>(GetEnumDescription(Step.B), Step.B),
        ];

        public static readonly List<KeyValuePair<string, NoteType>> TypeMembers =
        [
            new KeyValuePair<string, NoteType>(GetEnumDescription(NoteType.N1024th),    NoteType.N1024th),
            new KeyValuePair<string, NoteType>(GetEnumDescription(NoteType.N512th),     NoteType.N512th),
            new KeyValuePair<string, NoteType>(GetEnumDescription(NoteType.N256th),     NoteType.N256th),
            new KeyValuePair<string, NoteType>(GetEnumDescription(NoteType.N128th),     NoteType.N128th),
            new KeyValuePair<string, NoteType>(GetEnumDescription(NoteType.N64th),      NoteType.N64th),
            new KeyValuePair<string, NoteType>(GetEnumDescription(NoteType.N32nd),      NoteType.N32nd),
            new KeyValuePair<string, NoteType>(GetEnumDescription(NoteType.N16th),      NoteType.N16th),
            new KeyValuePair<string, NoteType>(GetEnumDescription(NoteType.Eighth),     NoteType.Eighth),
            new KeyValuePair<string, NoteType>(GetEnumDescription(NoteType.Quarter),    NoteType.Quarter),
            new KeyValuePair<string, NoteType>(GetEnumDescription(NoteType.Half),       NoteType.Half),
            new KeyValuePair<string, NoteType>(GetEnumDescription(NoteType.Whole),      NoteType.Whole),
            new KeyValuePair<string, NoteType>(GetEnumDescription(NoteType.Breve),      NoteType.Breve),
            new KeyValuePair<string, NoteType>(GetEnumDescription(NoteType.Long),       NoteType.Long),
            new KeyValuePair<string, NoteType>(GetEnumDescription(NoteType.Maxima),     NoteType.Maxima),
        ];

        public static readonly List<KeyValuePair<string, StartStop>> TieTypeMembers =
        [
            new KeyValuePair<string, StartStop>(GetEnumDescription(StartStop.Start),    StartStop.Start),
            new KeyValuePair<string, StartStop>(GetEnumDescription(StartStop.Stop),     StartStop.Stop),
        ];

        public static readonly List<KeyValuePair<string, TiedType>> TiedTypeMembers =
        [
            new KeyValuePair<string, TiedType>(GetEnumDescription(TiedType.Start),      TiedType.Start),
            new KeyValuePair<string, TiedType>(GetEnumDescription(TiedType.Stop),       TiedType.Stop),
            new KeyValuePair<string, TiedType>(GetEnumDescription(TiedType.Continue),   TiedType.Continue),
            new KeyValuePair<string, TiedType>(GetEnumDescription(TiedType.LetRing),    TiedType.LetRing),
        ];

        public static readonly List<KeyValuePair<string, StartStop>> TupletTypeMembers =
        [
            new KeyValuePair<string, StartStop>(GetEnumDescription(StartStop.Start),    StartStop.Start),
            new KeyValuePair<string, StartStop>(GetEnumDescription(StartStop.Stop),     StartStop.Stop)
        ];

        public static readonly List<KeyValuePair<string, Accidental>> AccidentalMembers =
        [
            new KeyValuePair<string, Accidental>(GetEnumDescription(Accidental.DoubleSharp),    Accidental.DoubleSharp),
            new KeyValuePair<string, Accidental>(GetEnumDescription(Accidental.SharpSharp),     Accidental.SharpSharp),
            new KeyValuePair<string, Accidental>(GetEnumDescription(Accidental.Sharp),          Accidental.Sharp),
            new KeyValuePair<string, Accidental>(GetEnumDescription(Accidental.Natural),        Accidental.Natural),
            new KeyValuePair<string, Accidental>(GetEnumDescription(Accidental.Flat),           Accidental.Flat),
            new KeyValuePair<string, Accidental>(GetEnumDescription(Accidental.FlatFlat),       Accidental.FlatFlat),
        ];

        public static readonly List<KeyValuePair<string, Syllabic>> SyllabicMembers =
        [
            new KeyValuePair<string, Syllabic>(GetEnumDescription(Syllabic.Begin),  Syllabic.Begin),
            new KeyValuePair<string, Syllabic>(GetEnumDescription(Syllabic.End),    Syllabic.End),
            new KeyValuePair<string, Syllabic>(GetEnumDescription(Syllabic.Middle), Syllabic.Middle),
            new KeyValuePair<string, Syllabic>(GetEnumDescription(Syllabic.Single), Syllabic.Single),
        ];

        public static readonly List<KeyValuePair<string, Mode>> ModeMembers =
        [
            new KeyValuePair<string, Mode>(GetEnumDescription(Mode.Major),      Mode.Major),
            new KeyValuePair<string, Mode>(GetEnumDescription(Mode.Minor),      Mode.Minor),
            new KeyValuePair<string, Mode>(GetEnumDescription(Mode.Ionian),     Mode.Ionian),
            new KeyValuePair<string, Mode>(GetEnumDescription(Mode.Dorian),     Mode.Dorian),
            new KeyValuePair<string, Mode>(GetEnumDescription(Mode.Phrygian),   Mode.Phrygian),
            new KeyValuePair<string, Mode>(GetEnumDescription(Mode.Lydian),     Mode.Lydian),
            new KeyValuePair<string, Mode>(GetEnumDescription(Mode.Mixolydian), Mode.Mixolydian),
            new KeyValuePair<string, Mode>(GetEnumDescription(Mode.Aeolian),    Mode.Aeolian),
            new KeyValuePair<string, Mode>(GetEnumDescription(Mode.Locrian),    Mode.Locrian),
            new KeyValuePair<string, Mode>(GetEnumDescription(Mode.None),       Mode.None),
        ];

        public static readonly List<KeyValuePair<string, Kind>> KindMembers =
        [
            new KeyValuePair<string, Kind>(GetEnumDescription(Kind.Augmented),          Kind.Augmented),
            new KeyValuePair<string, Kind>(GetEnumDescription(Kind.AugmentedSeventh),   Kind.AugmentedSeventh),
            new KeyValuePair<string, Kind>(GetEnumDescription(Kind.Diminished),         Kind.Diminished),
            new KeyValuePair<string, Kind>(GetEnumDescription(Kind.DiminishedSeventh),  Kind.DiminishedSeventh),
            new KeyValuePair<string, Kind>(GetEnumDescription(Kind.Dominant),           Kind.Dominant),
            new KeyValuePair<string, Kind>(GetEnumDescription(Kind.Dominant11th),       Kind.Dominant11th),
            new KeyValuePair<string, Kind>(GetEnumDescription(Kind.Dominant13th),       Kind.Dominant13th),
            new KeyValuePair<string, Kind>(GetEnumDescription(Kind.DominantNinth),      Kind.DominantNinth),
            new KeyValuePair<string, Kind>(GetEnumDescription(Kind.French),             Kind.French),
            new KeyValuePair<string, Kind>(GetEnumDescription(Kind.German),             Kind.German),
            new KeyValuePair<string, Kind>(GetEnumDescription(Kind.HalfDiminished),     Kind.HalfDiminished),
            new KeyValuePair<string, Kind>(GetEnumDescription(Kind.Italian ),           Kind.Italian ),
            new KeyValuePair<string, Kind>(GetEnumDescription(Kind.Major),              Kind.Major),
            new KeyValuePair<string, Kind>(GetEnumDescription(Kind.Major11th),          Kind.Major11th),
            new KeyValuePair<string, Kind>(GetEnumDescription(Kind.Major13th),          Kind.Major13th),
            new KeyValuePair<string, Kind>(GetEnumDescription(Kind.MajorMinor),         Kind.MajorMinor),
            new KeyValuePair<string, Kind>(GetEnumDescription(Kind.MajorNinth),         Kind.MajorNinth),
            new KeyValuePair<string, Kind>(GetEnumDescription(Kind.MajorSeventh),       Kind.MajorSeventh),
            new KeyValuePair<string, Kind>(GetEnumDescription(Kind.MajorSixth),         Kind.MajorSixth),
            new KeyValuePair<string, Kind>(GetEnumDescription(Kind.Minor),              Kind.Minor),
            new KeyValuePair<string, Kind>(GetEnumDescription(Kind.Minor11th),          Kind.Minor11th),
            new KeyValuePair<string, Kind>(GetEnumDescription(Kind.Minor13th),          Kind.Minor13th),
            new KeyValuePair<string, Kind>(GetEnumDescription(Kind.MinorNinth),         Kind.MinorNinth),
            new KeyValuePair<string, Kind>(GetEnumDescription(Kind.MinorSeventh),       Kind.MinorSeventh),
            new KeyValuePair<string, Kind>(GetEnumDescription(Kind.MinorSixth),         Kind.MinorSixth),
            new KeyValuePair<string, Kind>(GetEnumDescription(Kind.Neapolitan),         Kind.Neapolitan),
            new KeyValuePair<string, Kind>(GetEnumDescription(Kind.None),               Kind.None),
            new KeyValuePair<string, Kind>(GetEnumDescription(Kind.Other),              Kind.Other),
            new KeyValuePair<string, Kind>(GetEnumDescription(Kind.Pedal),              Kind.Pedal),
            new KeyValuePair<string, Kind>(GetEnumDescription(Kind.Power),              Kind.Power),
            new KeyValuePair<string, Kind>(GetEnumDescription(Kind.SuspendedFourth),    Kind.SuspendedFourth),
            new KeyValuePair<string, Kind>(GetEnumDescription(Kind.SuspendedSecond),    Kind.SuspendedSecond),
            new KeyValuePair<string, Kind>(GetEnumDescription(Kind.Tristan),            Kind.Tristan),
        ];

        public static readonly List<KeyValuePair<string, DegreeType>> DegreeTypeMembers =
        [
            new KeyValuePair<string, DegreeType>(GetEnumDescription(DegreeType.Add),        DegreeType.Add),
            new KeyValuePair<string, DegreeType>(GetEnumDescription(DegreeType.Alter),      DegreeType.Alter),
            new KeyValuePair<string, DegreeType>(GetEnumDescription(DegreeType.Subtract),   DegreeType.Subtract)
        ];

        public static readonly List<KeyValuePair<int, KeyValuePair<Step, int>>> FifthsMembers =
        [
             new KeyValuePair<int, KeyValuePair<Step, int>>( 0, new KeyValuePair<Step, int>(Step.C,  0)),
             new KeyValuePair<int, KeyValuePair<Step, int>>( 1, new KeyValuePair<Step, int>(Step.G,  0)),
             new KeyValuePair<int, KeyValuePair<Step, int>>( 2, new KeyValuePair<Step, int>(Step.D,  0)),
             new KeyValuePair<int, KeyValuePair<Step, int>>( 3, new KeyValuePair<Step, int>(Step.A,  0)),
             new KeyValuePair<int, KeyValuePair<Step, int>>( 4, new KeyValuePair<Step, int>(Step.E,  0)),
             new KeyValuePair<int, KeyValuePair<Step, int>>( 5, new KeyValuePair<Step, int>(Step.B,  0)),
             new KeyValuePair<int, KeyValuePair<Step, int>>( 6, new KeyValuePair<Step, int>(Step.F,  1)),
             new KeyValuePair<int, KeyValuePair<Step, int>>( 7, new KeyValuePair<Step, int>(Step.C,  1)),
             new KeyValuePair<int, KeyValuePair<Step, int>>(-1, new KeyValuePair<Step, int>(Step.F,  0)),
             new KeyValuePair<int, KeyValuePair<Step, int>>(-2, new KeyValuePair<Step, int>(Step.B, -1)),
             new KeyValuePair<int, KeyValuePair<Step, int>>(-3, new KeyValuePair<Step, int>(Step.E, -1)),
             new KeyValuePair<int, KeyValuePair<Step, int>>(-4, new KeyValuePair<Step, int>(Step.A, -1)),
             new KeyValuePair<int, KeyValuePair<Step, int>>(-5, new KeyValuePair<Step, int>(Step.D, -1)),
             new KeyValuePair<int, KeyValuePair<Step, int>>(-6, new KeyValuePair<Step, int>(Step.G, -1)),
             new KeyValuePair<int, KeyValuePair<Step, int>>(-7, new KeyValuePair<Step, int>(Step.C, -1)),
        ];


        public enum Mode
        {
            EnumDefault = 0,
            [Description("major")]
            Major = 1,
            [Description("minor")]
            Minor = 2,
            [Description("ionian")]
            Ionian = 3,
            [Description("dorian")]
            Dorian = 4,
            [Description("phrygian")]
            Phrygian = 5,
            [Description("lydian")]
            Lydian = 6,
            [Description("mixolydian")]
            Mixolydian = 7,
            [Description("aeolian")]
            Aeolian = 8,
            [Description("locrian")]
            Locrian = 9,
            [Description("none")]
            None = 10,
        }

        public enum Step
        {
            EnumDefault = 0,
            [Description("A")]
            A = 1,
            [Description("B")]
            B = 2,
            [Description("C")]
            C = 3,
            [Description("D")]
            D = 4,
            [Description("E")]
            E = 5,
            [Description("F")]
            F = 6,
            [Description("G")]
            G = 7,
        }

        public enum Accidental
        {
            EnumDefault = 0,
            [Description("sharp")]
            Sharp = 2,
            [Description("natural")]
            Natural = 1,
            [Description("flat")]
            Flat = 3,
            [Description("double-sharp")]
            DoubleSharp = 4,
            [Description("sharp-sharp")]
            SharpSharp = 5,
            [Description("flat-flat")]
            FlatFlat = 6,
            [Description("natural-sharp")]
            NaturalSharp = 7,
            [Description("natural-flat")]
            NaturalFlat = 8,
            [Description("quarter-flat")]
            QuarterFlat = 9,
            [Description("quarter-sharp")]
            QuarterSharp = 10,
            [Description("three-quarters-flat")]
            ThreeQuarterFlat = 11,
            [Description("three-quarters-sharp")]
            ThreeQuarterSharp = 12,
            [Description("sharp-down")]
            SharpDown = 13,
            [Description("sharp-up")]
            SharpUp = 14,
            [Description("natural-down")]
            NaturalDown = 15,
            [Description("natural-up")]
            NaturalUp = 16,
            [Description("flat-down")]
            FlatDown = 17,
            [Description("flat-up")]
            FlatUp = 18,
            [Description("double-sharp-down")]
            DoubleSharpDown = 19,
            [Description("double-sharp-up")]
            DoubleSharpUp = 20,
            [Description("flat-flat-down")]
            FlatFlatDown = 21,
            [Description("flat-flat-up")]
            FlatFlatUp = 22,
            [Description("arrow-down")]
            ArrowDown = 23,
            [Description("arrow-up")]
            ArrowUp = 24,
            [Description("triple-sharp")]
            TripleSharpp = 25,
            [Description("triple-flat")]
            TripleFlat = 26,
            [Description("slash-quarter-sharp")]
            SlashQuarterSharp = 27,
            [Description("slash-sharp")]
            SlashSharp = 28,
            [Description("slash-flat")]
            SlashFlat = 29,
            [Description("double-slash-flat")]
            DoubleSlashFlat = 30,
            [Description("sharp-1")]
            Sharp1 = 31,
            [Description("sharp-2")]
            Sharp2 = 32,
            [Description("sharp-3")]
            Sharp3 = 33,
            [Description("sharp-5")]
            Sharp5 = 34,
            [Description("flat-1")]
            Flat1 = 35,
            [Description("flat-2")]
            Flat2 = 36,
            [Description("flat-3")]
            Flat3 = 37,
            [Description("flat-4")]
            Flat4 = 38,
            [Description("sori")]
            Sori = 39,
            [Description("koron")]
            Koron = 40,
            [Description("other")]
            Other = 41,
        }

        public enum NoteType
        {
            EnumDefault = 0,
            [Description("1024th")]
            N1024th = 1,
            [Description("512th")]
            N512th = 2,
            [Description("256th")]
            N256th = 3,
            [Description("128th")]
            N128th = 4,
            [Description("64th")]
            N64th = 5,
            [Description("32nd")]
            N32nd = 6,
            [Description("16th")]
            N16th = 7,
            [Description("eighth")]
            Eighth = 8,
            [Description("quarter")]
            Quarter = 9,
            [Description("half")]
            Half = 10,
            [Description("whole")]
            Whole = 11,
            [Description("breve")]
            Breve = 12,
            [Description("long")]
            Long = 13,
            [Description("maxima")]
            Maxima = 14,
        }

        public enum StartStop
        {
            EnumDefault = 0,
            [Description("start")]
            Start = 1,
            [Description("stop")]
            Stop = 2,
        }

        public enum TiedType
        {
            EnumDefault = 0,
            [Description("start")]
            Start = 1,
            [Description("stop")]
            Stop = 2,
            [Description("continue")]
            Continue = 3,
            [Description("let-ring")]
            LetRing = 4,
        }

        public enum Syllabic
        {
            EnumDefault = 0,
            [Description("begin")]
            Begin = 1,
            [Description("end")]
            End = 2,
            [Description("middle")]
            Middle = 3,
            [Description("single")]
            Single = 4,
        }

        public enum Kind
        {
            EnumDefault = 0,
            [Description("augmented")]
            Augmented = 1,                      //Triad: major third, augmented fifth.
            [Description("augmented-seventh")]
            AugmentedSeventh = 2,               //Seventh: augmented triad, minor seventh.
            [Description("diminished")]
            Diminished = 3,                     //Triad: minor third, diminished fifth.
            [Description("diminished-seventh")]
            DiminishedSeventh = 4,              //Seventh: diminished triad, diminished seventh.
            [Description("dominant")]
            Dominant = 5,                       //Seventh: major triad, minor seventh.
            [Description("dominant-11th")]
            Dominant11th = 6,		            //11th: dominant-ninth, perfect 11th.
            [Description("dominant-13th")]
            Dominant13th = 7,		            //13th: dominant-11th, major 13th.
            [Description("dominant-ninth")]
            DominantNinth = 8,                  //Ninth: dominant, major ninth.
            [Description("French")]
            French = 9,                         //Functional French sixth.
            [Description("German")]
            German = 10,                        //Functional German sixth.
            [Description("half-diminished")]
            HalfDiminished = 11,                //Seventh: diminished triad, minor seventh.
            [Description("Italian")]
            Italian = 12,                       //Functional Italian sixth.
            [Description("major")]
            Major = 13,                         //Triad: major third, perfect fifth.
            [Description("major-11th")]
            Major11th = 14,		                //11th: major-ninth, perfect 11th.
            [Description("major-13th")]
            Major13th = 15,		                //13th: major-11th, major 13th.
            [Description("major-minor")]
            MajorMinor = 16,                    //Seventh: minor triad, major seventh.
            [Description("major-ninth")]
            MajorNinth = 17,                    //Ninth: major-seventh, major ninth.
            [Description("major-seventh")]
            MajorSeventh = 18,                  //Seventh: major triad, major seventh.
            [Description("major-sixth")]
            MajorSixth = 19,                    //Sixth: major triad, added sixth.
            [Description("minor")]
            Minor = 20,                         //Triad: minor third, perfect fifth.
            [Description("minor-11th")]
            Minor11th = 21,		                //11th: minor-ninth, perfect 11th.
            [Description("minor-13th")]
            Minor13th = 22,		                //13th: minor-11th, major 13th.
            [Description("minor-ninth")]
            MinorNinth = 23,                    //Ninth: minor-seventh, major ninth.
            [Description("minor-seventh")]
            MinorSeventh = 24,                  //Seventh: minor triad, minor seventh.
            [Description("minor-sixth")]
            MinorSixth = 25,                    //Sixth: minor triad, added sixth.
            [Description("Neapolitan")]
            Neapolitan = 26,                     //Functional Neapolitan sixth.
            [Description("none")]
            None = 27,                          //Used to explicitly encode the absence of chords or functional harmony.
            [Description("other")]
            Other = 28,                         //Used when the harmony is entirely composed of add elements.
            [Description("pedal")]
            Pedal = 29,                         //Pedal-point bass
            [Description("power")]
            Power = 30,                         //Perfect fifth.
            [Description("suspended-fourth")]
            SuspendedFourth = 31,               //Suspended: perfect fourth, perfect fifth.
            [Description("suspended-second")]
            SuspendedSecond = 32,               //Suspended: major second, perfect fifth.
            [Description("Tristan")]
            Tristan = 33,                       //Augmented fourth, augmented sixth, augmented ninth.
        }

        public enum DegreeType
        {
            EnumDefault = 0,
            [Description("add")]
            Add = 1,
            [Description("alter")]
            Alter = 2,
            [Description("subtract")]
            Subtract = 3,
        }

        /// <summary>
        /// numの要素からDescription属性の文字列を取得する
        /// </summary>
        /// <param name="Value"></param>
        /// <returns></returns>
        public static string GetEnumDescription(object Value)
        {
            string Description = string.Empty;
            FieldInfo? FieldInfo = Value.GetType().GetField(Value.ToString()!);
            if (FieldInfo != null)
            {
                Attribute? attr = Attribute.GetCustomAttribute(FieldInfo, typeof(DescriptionAttribute));
                if (attr != null)
                {
                    DescriptionAttribute descAttr = (DescriptionAttribute)attr;
                    Description = descAttr.Description;
                }
            }
            return Description;
        }
    }
}
