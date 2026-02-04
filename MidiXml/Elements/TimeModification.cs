using System.Xml.Linq;

namespace Developers.MidiXml.Elements
{
    /// <summary>
    /// <time-modificatioin>情報
    /// </summary>
    public class TimeModification : MidiElement
    {
        #region "public properties"

        public int ActualNotes { get; init; } = 0;
        public int NormalNotes { get; init; } = 0;

        #endregion

        #region "constructors"

        /// <summary>
        /// コンストラクタ(XDocument版)
        /// </summary>
        /// <param name="Source"></param>
        /// <exception cref="ArgumentException"></exception>
        public TimeModification(XElement Source)
        {
            //ソース読み取り
            XElement? ElmActualNotes = Source!.Element("actual-notes");
            XElement? ElmNormalNotes = Source.Element("normal-notes");

            //<actual-notes>
            if (ElmActualNotes != null)
            {
                if (!int.TryParse(ElmActualNotes.Value, out int RawActualNotesInt))
                {
                    throw new ArgumentException("<time-modification>: <actual-notes>: Invalid value.");
                }
                this.ActualNotes = RawActualNotesInt;
            }
            else
            {
                throw new ArgumentException("<time-modification><actual-notes>Not found.");
            }
            //<normal-notes>
            if (ElmNormalNotes != null)
            {
                if (!int.TryParse(ElmNormalNotes.Value, out int RawNormalNotesInt))
                {
                    throw new ArgumentException("<time-modification>: <normal-notes>: Invalid value.");
                }
                this.NormalNotes = RawNormalNotesInt;
            }
            else
            {
                throw new ArgumentException("<time-modification>: <normal-notes>: Not found.");
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

            Dump += "<time-modification>";
            Dump += "<actual-notes>" + ActualNotes.ToString();
            Dump += "<normal-notes>" + NormalNotes.ToString();
            return Dump;
        }

        #endregion
    }
}
