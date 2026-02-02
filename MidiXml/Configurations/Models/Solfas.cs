using System.Xml.Serialization;

namespace Developers.MidiXml.Configurations.Models
{

    public class Solfas
    {
        [XmlArrayItem("solfa")]
        private List<Solfa> Settings { get; set; } = [];

        /// <summary>
        /// 指定されたソルファ設定があるか確認する
        /// </summary>
        /// <param name="Name"></param>
        /// <returns></returns>
        public bool Exists(string Name)
        {
            return (Settings.Exists(x => x.Name.Equals(Name)));
        }

        /// <summary>
        /// 指定されたソルファ設定を取得する
        /// </summary>
        /// <param name="Name"></param>
        /// <returns></returns>
        public Solfa GetSolfa(string Name)
        {
            Solfa RetVal = new Solfa();
            if (Settings.Exists(x => x.Name.Equals(Name)))
            {
                RetVal = Settings.FirstOrDefault(x => x.Name.Equals(Name))!;
            }
            return RetVal;
        }
    }

    /// <summary>
    /// Solfaの設定
    /// </summary>
    public class Solfa
    {
        [XmlAttribute("name")] public string Name { get; set; } = string.Empty;
        [XmlElement("do")] public string Do { get; set; } = "do";
        [XmlElement("do-sharp")] public string DoSharp { get; set; } = "di";
        [XmlElement("re-flat")] public string ReFlat { get; set; } = "ra";
        [XmlElement("re")] public string Re { get; set; } = "re";
        [XmlElement("re-sharp")] public string ReSharp { get; set; } = "ri";
        [XmlElement("mi-flat")] public string MiFlat { get; set; } = "me";
        [XmlElement("mi")] public string Mi { get; set; } = "mi";
        [XmlElement("fa")] public string Fa { get; set; } = "fa";
        [XmlElement("fa-sharp")] public string FaSharp { get; set; } = "fi";
        [XmlElement("sol-flat")] public string SolFlat { get; set; } = "se";
        [XmlElement("sol")] public string Sol { get; set; } = "sol";
        [XmlElement("sol-sharp")] public string SolSharp { get; set; } = "si";
        [XmlElement("la-flat")] public string LaFlat { get; set; } = "le";
        [XmlElement("la")] public string La { get; set; } = "la";
        [XmlElement("la-sharp")] public string LaSharp { get; set; } = "li";
        [XmlElement("ti-flat")] public string TiFlat { get; set; } = "te";
        [XmlElement("ti")] public string Ti { get; set; } = "ti";

        public List<List<string>> ToList()
        {
            List<List<string>> RetVal =
            [
                [Do],
                [DoSharp, ReFlat],
                [Re],
                [ReSharp, MiFlat],
                [Mi],
                [Fa],
                [FaSharp, SolFlat],
                [Sol],
                [SolSharp, LaFlat],
                [La],
                [LaSharp, TiFlat],
                [Ti],
            ];
            return RetVal;
        }
    }
}
