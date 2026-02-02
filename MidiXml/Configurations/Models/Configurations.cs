using System.Xml.Serialization;

namespace Developers.MidiXml.Configurations.Models
{
    [XmlRoot("configurations")]
    public class Configurations
    {
        //ソルファ設定リスト
        [XmlArray("solfas")]
        [XmlArrayItem("solfa")]
        public List<Solfa> Solfas { get; set; } = [];

        [XmlElement("debug")]
        public bool Debug { get; set; } = false;

        [XmlIgnore]
        public Solfa DefaultSolfa { get; init; } = new Solfa();
    }
}
