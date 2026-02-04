using System.Xml.Linq;

namespace Developers.MidiXml.Elements
{
    /// <summary>
    /// <degree>情報(テンション情報)
    /// </summary>
    public class Degree : MidiElement
    {
        #region "properties"

        public int Value { get; init; } = 1;
        public int Alter { get; init; } = 0;

        #endregion

        #region "constructors"

        public MidiDefs.DegreeType Type { get; set; } = MidiDefs.DegreeType.Add;

        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="Source"></param>
        /// <exception cref="ArgumentException"></exception>
        /// <exception cref="FormatException"></exception>
        public Degree(XElement Source)
        {
            //ソース読み取り
            XElement? ElmValue = Source.Element("degree-value");
            XElement? ElmAlter = Source.Element("degree-alter");
            XElement? ElmType = Source.Element("degree-type");

            //<degree-value>
            if (ElmValue != null)
            {
                if (!int.TryParse(ElmValue.Value, out int RawValueInt))
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
            if (ElmAlter != null)
            {
                if (!int.TryParse(ElmAlter.Value, out int RawAlterInt))
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
            if (ElmType != null)
            {
                string? RawType = ElmType.Value ?? "";
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

        #endregion

        #region "debug methods"

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

        #endregion
    }
}
