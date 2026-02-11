using Developers.MusicXml.Configurations.Models;
using Developers.MusicXml.Elements;
using Microsoft.Extensions.Configuration;
using Serilog;
using System.Xml.Linq;

namespace Developers.MusicXml
{

    public class XScore
    {
        #region "fields"

        //SeriLog(設定ファイルを読み込んでログを設定)
        private readonly ILogger Log = new LoggerConfiguration()
            .ReadFrom.Configuration(
                new ConfigurationBuilder().AddJsonFile("MusicXmlLogSettings.json").Build()
            ).CreateLogger();

        #endregion

        #region "private properties"

        /// <summary>
        /// 設定ファイルマネージャ
        /// </summary>
        private Configurations.ConfigurationManager Configs { get; set; } = new Configurations.ConfigurationManager();
        /// <summary>
        /// Solfa設定名リスト
        /// </summary>
        public List<string> SofaSettingNames
        {
            get
            {
                List<string> RetVal = [];
                foreach (Solfa s in Configs.Solfas)
                {
                    RetVal.Add(s.Name);
                }
                return RetVal;
            }
        }
        /// <summary>
        /// ソルフェージュ歌詞リスト
        /// </summary>
        private List<List<string>> SolfaLyrics { get; set; }
        /// <summary>
        /// Xmlファイル名(フルパス)
        /// </summary>
        private string FileName { get; set; } = string.Empty;
        /// <summary>
        /// MusicXmlのXDocument
        /// </summary>
        private XDocument? DocScore { get; set; } = null;
        /// <summary>
        /// MusicXmlが保持するパートのリスト
        /// </summary>
        private List<XPart> XParts { get; set; } = [];
        /// <summary>
        /// MusicXmlが持つパートのノードリスト
        /// </summary>
        private List<KeyValuePair<string, List<MidiElement>>> PartElms { get; set; } = [];

        #endregion

        #region "public properties"

        /// <summary>
        /// MusicXmlが保持するパートのコレクション
        /// </summary>
        public Dictionary<string, string> ScoreParts
        {
            get
            {
                Dictionary<string, string> RetVal = [];
                foreach (XPart Part in XParts)
                {
                    RetVal.Add(Part.ID, Part.Name);
                }
                return RetVal;
            }
        }

        #endregion

        #region "constructors"

        /// <summary>
        /// コンストラクタ
        /// </summary>
        public XScore()
        {
            //自ライブラリの設定ファイル(ソルファ歌詞)を読み取む
            Configs.Read();
            //デフォルトのソルファ設定をセット
            SolfaLyrics = Configs.DefaultSolfa.ToList();
        }

        #endregion

        #region "public nethods"

        /// <summary>
        /// MusicXmlのロード
        /// </summary>
        /// <param name="ImportPath"></param>
        /// <returns></returns>
        public XScore Load(string ImportPath)
        {
            //インスタンス生成
            XScore Score = new XScore();
            //ファイル名保存
            Score.FileName = ImportPath;
            //パートリストの初期化
            this.XParts.Clear();
            //パートエレメントの初期化
            this.PartElms.Clear();
            //Xmlドキュメントのロード
            this.DocScore = XDocument.Load(Score.FileName);
            //<part-list>から<score-part>のリストを取得
            XElement? ElmPartList = DocScore.Element("part-list");
            if (ElmPartList == null)
            {
                //<score-part>にしたがいパートを読み取る
                IEnumerable<XElement> ElmScoreParts = DocScore.Descendants("score-part");
                foreach (XElement ElmScorePart in ElmScoreParts)
                {
                    //パート情報の読み取り
                    XPart Part = new XPart(DocScore, ElmScorePart);
                    //パート情報をPartsに追記
                    this.XParts.Add(Part);
                }
            }
            return Score;
        }

        /// <summary>
        /// パートの取得
        /// </summary>
        /// <param name="PartID"></param>
        /// <returns></returns>
        public XPart? XPart(string PartID)
        {
            return XParts.Find(x => x.ID.Equals(PartID));
        }

        /// <summary>
        /// MusicXmlファイルのエクスポート
        /// </summary>
        /// <param name="ExportPath"></param>
        public void XmlExport(string ExportPath)
        {
            //デバック出力
            foreach (XPart Part in this.XParts)
            {
                Part.DebugTrace("********** XScore.XmlExport **********");
            }
            //XMLファイルの保存
            DocScore?.Save(ExportPath);
        }

        #endregion

        #region "private methods"

        #endregion

        #region "debug methods"

        #endregion

    }

}
