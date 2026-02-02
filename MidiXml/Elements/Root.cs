using System.Xml.Linq;

namespace Developers.MidiXml.Elements
{
    /// <summary>
    /// <harmonyで指定されるコードのルート音(AlterのStep繰り上げは行わない)
    /// </summary>
    public class Root : MidiElement
    {
        /// <summary>
        /// ステップ(オリジナル)
        /// </summary>
        public MidiDefs.Step Step { get; init; } = MidiDefs.Step.C;
        /// <summary>
        /// 半音操作(オリジナル)
        /// </summary>
        public int Alter { get; init; } = MidiDefs.ALTER_NATURAL;
        /// <summary>
        /// PitchClassインスタンスで表現されたRoot情報
        /// </summary>
        public PitchClass PitchClass
        {
            get
            {
                return new PitchClass(this.Step, this.Alter);
            }
        }

        /// <summary>
        /// コンストラクタ(デフォルト)
        /// </summary>
        public Root()
        {
        }

        /// <summary>
        /// コンストラクタ(XDocument版)
        /// </summary>
        /// <param name="SourceElm"></param>
        public Root(XElement SourceElm)
        {
            //ソース読み取り
            XElement? StepElm = SourceElm.Element("root-step");
            XElement? AlterElm = SourceElm.Element("root-alter");

            //必須タグのチェック
            if (StepElm == null)
            {
                throw new FormatException("<root>: <root-step>: Not found.");
            }
            //必須データの正当性チェック
            string RawStep = StepElm.Value ?? "";
            if (!MidiDefs.StepMembers.Exists(x => x.Key.Equals(RawStep, StringComparison.OrdinalIgnoreCase)))
            {
                throw new ArgumentException("<root>: <root-step>: Invalid value.");
            }
            //必須データのセット
            this.Step = MidiDefs.StepMembers.FirstOrDefault(x => x.Key.Equals(RawStep, StringComparison.CurrentCultureIgnoreCase)).Value;
            //任意データの処理
            if (AlterElm != null)
            {
                //任意データのセット
                if (!int.TryParse(AlterElm.Value, out int RawAlterInt))
                {
                    throw new ArgumentException("<root>: <root-alter>: Invalid value.");
                }
                //サポートチェック
                if (RawAlterInt < MidiDefs.ALTER_FLAT || MidiDefs.ALTER_SHARP < RawAlterInt)
                {
                    throw new ArgumentException("<root>: <root-alter>: Unsupported value.");
                }
                //任意データのセット
                this.Alter = RawAlterInt;
            }
        }

        /// <summary>
        /// デバック用ダンプ
        /// </summary>
        /// <returns></returns>
        public override string DebugDump()
        {
            string Dump = string.Empty;

            Dump += "<root>";
            Dump += "<root-step>" + Step.ToString();
            Dump += "<root-alter>" + Alter.ToString();
            return Dump;
        }
    }
}
