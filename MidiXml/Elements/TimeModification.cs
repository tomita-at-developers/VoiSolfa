using System.Xml.Linq;

namespace Developers.MidiXml.Elements
{
    /// <summary>
    /// <time-modificatioin>の解析
    /// </summary>
    public class TimeModification : MidiElement
    {
        public int ActualNotes { get; init; } = 0;
        public int NormalNotes { get; init; } = 0;

        /// <summary>
        /// コンストラクタ(XDocument版)
        /// </summary>
        /// <param name="SourceElm"></param>
        /// <exception cref="ArgumentException"></exception>
        public TimeModification(XElement SourceElm)
        {
            //ソース読み取り
            XElement? ActualNotesElm = SourceElm!.Element("actual-notes");
            XElement? NormalNotesElm = SourceElm.Element("normal-notes");

            //<actual-notes>>
            if (ActualNotesElm != null)
            {
                if (!int.TryParse(ActualNotesElm.Value, out int RawActualNotesInt))
                {
                    throw new ArgumentException("<time-modification>: <actual-notes>: Invalid value.");
                }
                this.ActualNotes = RawActualNotesInt;
            }
            else
            {
                throw new ArgumentException("<time-modification><actual-notes>Not found.");
            }
            //<normal-notes>>
            if (NormalNotesElm != null)
            {
                if (!int.TryParse(NormalNotesElm.Value, out int RawNormalNotesInt))
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
    }
}
