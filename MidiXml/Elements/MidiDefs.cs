using System.ComponentModel;

namespace Developers.MidiXml.Elements
{
    /// <summary>
    /// MusicXmlに関する各種定義
    /// </summary>
    public static class MidiDefs
    {
        public const int ALTER_DOUBLE_SHARP = 2;
        public const int ALTER_SHARP = 1;
        public const int ALTER_NATURAL = 0;
        public const int ALTER_FLAT = -1;
        public const int ALTER_DOUBLE_FLAT = -2;

        public const int OCTAVE_CENTER = 4;

        //Step定義リスト
        public static readonly List<KeyValuePair<string, MidiDefs.Step>> StepMembers =
        [
            new KeyValuePair<string, MidiDefs.Step>("A", MidiDefs.Step.A),
            new KeyValuePair<string, MidiDefs.Step>("B", MidiDefs.Step.B),
            new KeyValuePair<string, MidiDefs.Step>("C", MidiDefs.Step.C),
            new KeyValuePair<string, MidiDefs.Step>("D", MidiDefs.Step.D),
            new KeyValuePair<string, MidiDefs.Step>("E", MidiDefs.Step.E),
            new KeyValuePair<string, MidiDefs.Step>("F", MidiDefs.Step.F),
            new KeyValuePair<string, MidiDefs.Step>("G", MidiDefs.Step.G)
        ];

        public static readonly List<KeyValuePair<string, MidiDefs.NoteType>> TypeMembers =
        [
            new KeyValuePair<string, MidiDefs.NoteType>("whole", MidiDefs.NoteType.Whole),
            new KeyValuePair<string, MidiDefs.NoteType>("half", MidiDefs.NoteType.Half),
            new KeyValuePair<string, MidiDefs.NoteType>("quarter", MidiDefs.NoteType.Quarter),
            new KeyValuePair<string, MidiDefs.NoteType>("eighth", MidiDefs.NoteType.Eighth),
            new KeyValuePair<string, MidiDefs.NoteType>("16th", MidiDefs.NoteType.N16),
            new KeyValuePair<string, MidiDefs.NoteType>("32nd", MidiDefs.NoteType.N32),
            new KeyValuePair<string, MidiDefs.NoteType>("64th", MidiDefs.NoteType.N64)
        ];

        public static readonly List<KeyValuePair<string, MidiDefs.StartStop>> TieTypeMembers =
        [
            new KeyValuePair<string, MidiDefs.StartStop>("start", MidiDefs.StartStop.Start),
            new KeyValuePair<string, MidiDefs.StartStop>("stop", MidiDefs.StartStop.Stop)
        ];

        public static readonly List<KeyValuePair<string, MidiDefs.TiedType>> TiedTypeMembers =
        [
            new KeyValuePair<string, MidiDefs.TiedType>("start",    MidiDefs.TiedType.Start),
            new KeyValuePair<string, MidiDefs.TiedType>("continue", MidiDefs.TiedType.Continue),
            new KeyValuePair<string, MidiDefs.TiedType>("stop",     MidiDefs.TiedType.Stop),
            new KeyValuePair<string, MidiDefs.TiedType>("let-ring", MidiDefs.TiedType.LetRing)
        ];

        public static readonly List<KeyValuePair<string, MidiDefs.StartStop>> TupletTypeMembers =
        [
            new KeyValuePair<string, MidiDefs.StartStop>("start",   MidiDefs.StartStop.Start),
            new KeyValuePair<string, MidiDefs.StartStop>("stop",    MidiDefs.StartStop.Stop)
        ];

        public static readonly List<KeyValuePair<string, MidiDefs.Accidental>> AccidentalMembers =
        [
            new KeyValuePair<string, MidiDefs.Accidental>("sharp-sharp",    MidiDefs.Accidental.DoubleSharp),
            new KeyValuePair<string, MidiDefs.Accidental>("double-sharp",   MidiDefs.Accidental.DoubleSharp),
            new KeyValuePair<string, MidiDefs.Accidental>("sharp",          MidiDefs.Accidental.Sharp),
            new KeyValuePair<string, MidiDefs.Accidental>("natural",        MidiDefs.Accidental.Natural),
            new KeyValuePair<string, MidiDefs.Accidental>("flat",           MidiDefs.Accidental.Flat),
            new KeyValuePair<string, MidiDefs.Accidental>("double-flat",    MidiDefs.Accidental.DoubleFlat),
            new KeyValuePair<string, MidiDefs.Accidental>("flat-flat",      MidiDefs.Accidental.DoubleFlat),
        ];

        public static readonly List<KeyValuePair<string, MidiDefs.Syllabic>> SyllabicMembers =
        [
            new KeyValuePair<string, MidiDefs.Syllabic>("single",   MidiDefs.Syllabic.Single),
            new KeyValuePair<string, MidiDefs.Syllabic>("begin",    MidiDefs.Syllabic.Begin),
            new KeyValuePair<string, MidiDefs.Syllabic>("middle",   MidiDefs.Syllabic.Middle),
            new KeyValuePair<string, MidiDefs.Syllabic>("end",      MidiDefs.Syllabic.End)
        ];

        public static readonly List<KeyValuePair<string, MidiDefs.Mode>> ModeMembers =
        [
            new KeyValuePair<string, MidiDefs.Mode>("major", MidiDefs.Mode.Major),
            new KeyValuePair<string, MidiDefs.Mode>("minor", MidiDefs.Mode.Minor)
        ];

        public static readonly List<KeyValuePair<string, MidiDefs.Kind>> KindMembers =
        [
            new KeyValuePair<string, MidiDefs.Kind>("augmented",            MidiDefs.Kind.Augmented),
            new KeyValuePair<string, MidiDefs.Kind>("augmented-seventh",    MidiDefs.Kind.AugmentedSeventh),
            new KeyValuePair<string, MidiDefs.Kind>("diminished",           MidiDefs.Kind.Diminished),
            new KeyValuePair<string, MidiDefs.Kind>("diminished-seventh",   MidiDefs.Kind.DiminishedSeventh),
            new KeyValuePair<string, MidiDefs.Kind>("dominant",             MidiDefs.Kind.Dominant),
            new KeyValuePair<string, MidiDefs.Kind>("dominant-11th",        MidiDefs.Kind.Dominant11th),
            new KeyValuePair<string, MidiDefs.Kind>("dominant-13th",        MidiDefs.Kind.Dominant13th),
            new KeyValuePair<string, MidiDefs.Kind>("dominant-ninth",       MidiDefs.Kind.DominantNinth),
            new KeyValuePair<string, MidiDefs.Kind>("French",               MidiDefs.Kind.French),
            new KeyValuePair<string, MidiDefs.Kind>("German",               MidiDefs.Kind.German),
            new KeyValuePair<string, MidiDefs.Kind>("half-diminished",      MidiDefs.Kind.HalfDiminished),
            new KeyValuePair<string, MidiDefs.Kind>("Italian",              MidiDefs.Kind.Italian ),
            new KeyValuePair<string, MidiDefs.Kind>("major",                MidiDefs.Kind.Major),
            new KeyValuePair<string, MidiDefs.Kind>("major-11th",           MidiDefs.Kind.Major11th),
            new KeyValuePair<string, MidiDefs.Kind>("major-13th",           MidiDefs.Kind.Major13th),
            new KeyValuePair<string, MidiDefs.Kind>("major-minor",          MidiDefs.Kind.MajorMinor),
            new KeyValuePair<string, MidiDefs.Kind>("major-ninth",          MidiDefs.Kind.MajorNinth),
            new KeyValuePair<string, MidiDefs.Kind>("major-seventh",        MidiDefs.Kind.MajorSeventh),
            new KeyValuePair<string, MidiDefs.Kind>("major-sixth",          MidiDefs.Kind.MajorSixth),
            new KeyValuePair<string, MidiDefs.Kind>("minor",                MidiDefs.Kind.Minor),
            new KeyValuePair<string, MidiDefs.Kind>("minor-11th",           MidiDefs.Kind.Minor11th),
            new KeyValuePair<string, MidiDefs.Kind>("minor-13th",           MidiDefs.Kind.Minor13th),
            new KeyValuePair<string, MidiDefs.Kind>("minor-ninth",          MidiDefs.Kind.MinorNinth),
            new KeyValuePair<string, MidiDefs.Kind>("minor-seventh",        MidiDefs.Kind.MinorSeventh),
            new KeyValuePair<string, MidiDefs.Kind>("minor-sixth",          MidiDefs.Kind.MinorSixth),
            new KeyValuePair<string, MidiDefs.Kind>("Neapolitan",           MidiDefs.Kind.Neapolitan),
            new KeyValuePair<string, MidiDefs.Kind>("none",                 MidiDefs.Kind.None),
            new KeyValuePair<string, MidiDefs.Kind>("other",                MidiDefs.Kind.Other),
            new KeyValuePair<string, MidiDefs.Kind>("pedal",                MidiDefs.Kind.Pedal),
            new KeyValuePair<string, MidiDefs.Kind>("power",                MidiDefs.Kind.Power),
            new KeyValuePair<string, MidiDefs.Kind>("suspended-fourth",     MidiDefs.Kind.SuspendedFourth),
            new KeyValuePair<string, MidiDefs.Kind>("suspended-second",     MidiDefs.Kind.SuspendedSecond),
            new KeyValuePair<string, MidiDefs.Kind>("Tristan",              MidiDefs.Kind.Tristan)
        ];

        public static readonly List<KeyValuePair<string, MidiDefs.DegreeType>> DegreeTypeMembers =
        [
            new KeyValuePair<string, MidiDefs.DegreeType>("add",        MidiDefs.DegreeType.Add),
            new KeyValuePair<string, MidiDefs.DegreeType>("alter",      MidiDefs.DegreeType.Alter),
            new KeyValuePair<string, MidiDefs.DegreeType>("subtract",   MidiDefs.DegreeType.Subtract)
        ];

        public enum Mode
        {
            Major = 1,
            Minor = 2,
            Ionian = 3,
            Dorian = 4,
            Phrygian = 5,
            Lydian = 6,
            Mixolydian = 7,
            Aeolian = 8,
            Locrian = 9
        }

        public enum Step
        {
            A = 1,
            B = 2,
            C = 3,
            D = 4,
            E = 5,
            F = 6,
            G = 7
        }

        public enum Accidental
        {
            Natural = 1,
            Sharp = 2,
            Flat = 3,
            DoubleSharp = 4,
            DoubleFlat = 5
        }

        public enum NoteType
        {
            Whole = 1,
            Half = 2,
            Quarter = 3,
            Eighth = 4,
            N16 = 5,
            N32 = 6,
            N64 = 7
        }

        public enum StartStop
        {
            None = 1,
            Start = 2,
            Stop = 3,
        }

        public enum TiedType
        {
            None = 1,
            Start = 2,
            Stop = 3,
            Continue = 4,
            LetRing = 5
        }

        public enum Syllabic
        {
            [Description("single")]
            Single = 1,
            [Description("begin")]
            Begin = 2,
            [Description("end")]
            End = 3,
            [Description("middle")]
            Middle = 4
        }

        public enum Kind
        {
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
            Add = 1,
            Alter = 2,
            Subtract = 3
        }
    }
}
