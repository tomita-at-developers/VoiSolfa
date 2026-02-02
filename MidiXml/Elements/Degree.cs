using System.Xml.Linq;

namespace Developers.MidiXml.Elements
{
    public class Degree : MidiElement
    {
        public int Value { get; init; } = 1;
        public int Alter { get; init; } = 0;
        public MidiDefs.DegreeType Type { get; set; } = MidiDefs.DegreeType.Add;

        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="SourceElm"></param>
        /// <exception cref="ArgumentException"></exception>
        /// <exception cref="FormatException"></exception>
        public Degree(XElement SourceElm)
        {
            //ソース読み取り
            XElement? ValueElm = SourceElm.Element("degree-value");
            XElement? AlterElm = SourceElm.Element("degree-alter");
            XElement? TypeElm = SourceElm.Element("degree-type");

            //<degree-value>
            if (ValueElm != null)
            {
                if (!int.TryParse(ValueElm.Value, out int RawValueInt))
                {
                    throw new ArgumentException("degree>: <degree-value>: Invalid value.");
                }
                this.Value = RawValueInt;
            }
            else
            {
                throw new FormatException("<degree>: <Degree-Value>: Not found.");
            }
            //<degree-alter>
            if (AlterElm != null)
            {
                if (!int.TryParse(AlterElm.Value, out int RawAlterInt))
                {
                    throw new ArgumentException("degree>: <degree-value>: Invalid value.");
                }
                if (RawAlterInt < MidiDefs.ALTER_FLAT || MidiDefs.ALTER_SHARP < RawAlterInt)
                {
                    throw new ArgumentException("degree>: <degree-value>: Invalid value.");
                }
                this.Value = RawAlterInt;
            }
            else
            {
                throw new ArgumentException("<degree>: <degree-alter>: Out of range.");
            }
            //<degree-type>
            if (TypeElm != null)
            {
                string? RawType = TypeElm.Value ?? "";
                if (!MidiDefs.DegreeTypeMembers.Exists(x => x.Key.Equals(RawType, StringComparison.OrdinalIgnoreCase)))
                {
                    throw new ArgumentException("<degree>: <degree-type>: Invalid value.");
                }
                this.Type = MidiDefs.DegreeTypeMembers.FirstOrDefault(x => x.Key.Equals(RawType, StringComparison.OrdinalIgnoreCase)).Value;
            }
            else
            {
                throw new ArgumentException("<degree>: <Degree-type>: Invalid value.");
            }
        }

        /// <summary>
        /// デバック用ダンプ
        /// </summary>
        /// <returns></returns>
        public override string DebugDump()
        {
            string Dump = string.Empty;

            Dump += "<degree>";
            Dump += "<degree-value>" + this.Value.ToString();
            Dump += "<degree-alter>" + this.Alter.ToString();
            Dump += "<degree-type>" + this.Type.ToString();
            return Dump;
        }
    }
}
