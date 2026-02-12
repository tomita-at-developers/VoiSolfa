using Microsoft.Extensions.Configuration;

namespace Developers.MusicXml.Configurations.Models
{
    public class MusicConfigurations
    {
        //ソルファ設定リスト
        [ConfigurationKeyName("solfas:solfa")]
        public List<Solfa> Solfas { get; set; } = [];

        [ConfigurationKeyName("debug")]
        public bool Debug { get; set; } = false;

        public Solfa DefaultSolfa { get; init; } = new Solfa();
    }

    //[XmlRoot("configurations")]
    //public class Configurations
    //{
    //    //ソルファ設定リスト
    //    [XmlArray("solfas")]
    //    [XmlArrayItem("solfa")]
    //    public List<Solfa> Solfas { get; set; } = [];

    //    [XmlElement("debug")]
    //    public bool Debug { get; set; } = false;

    //    [XmlIgnore]
    //    public Solfa DefaultSolfa { get; init; } = new Solfa();
    //}

}
