using Developers.MidiXml.Configurations.Models;
using System.Text;
using System.Xml;
using System.Xml.Serialization;

namespace Developers.MidiXml.Configurations
{
    public class ConfigurationManager
    {
        /// <summary>
        /// 設定クラス
        /// </summary>
        private Models.Configurations Configs { get; set; } = new Models.Configurations();

        /// <summary>
        /// 設定クラスのDefaultSolfa
        /// </summary>
        public Solfa DefaultSolfa
        {
            get { return Configs.DefaultSolfa; }
        }

        /// <summary>
        /// 設定クラスのDefaultSolfas
        /// </summary>
        public List<Solfa> Solfas
        {
            get { return Configs.Solfas; }
        }

        /// <summary>
        /// 設定クラスのDebug
        /// </summary>
        public bool Debug
        {
            get { return Configs.Debug; } 
        }

        /// <summary>
        /// コンストラクタ
        /// </summary>
        public ConfigurationManager()
        {
            //設定ファイルの読み取り
            Read();
        }

        /// <summary>
        /// 設定ファイルの読み取り
        /// </summary>
        public void Read()
        {
            string Location = string.Empty;
            string ConfigPath = string.Empty;
            Location = System.Reflection.Assembly.GetExecutingAssembly().Location;
            ConfigPath = Path.Combine(Path.GetDirectoryName(Location)!, "MidiXml.config");
            if (File.Exists(ConfigPath))
            {
                XmlSerializer Serializer = new XmlSerializer(typeof(Models.Configurations));
                XmlReaderSettings Settings = new XmlReaderSettings()
                {
                    CheckCharacters = false,
                };
                using (StreamReader Reader = new StreamReader(ConfigPath, Encoding.UTF8))
                using (var xmlReader = XmlReader.Create(Reader, Settings))
                {
                    Models.Configurations? Temp = (Models.Configurations?)Serializer.Deserialize(xmlReader);
                    if (Temp != null)
                    {
                        this.Configs = Temp;
                    }
                }
            }
        }
    }
}
