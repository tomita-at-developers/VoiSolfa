using System.Xml.Linq;

namespace Developers.MidiXml.Elements
{
    /// <summary>
    /// <root>情報(<harmonyで指定されるコードのルート音情報)
    /// </summary>
    public class Root : MidiElement
    {
        #region "public properties"

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

        #endregion

        #region "constructors"

        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="Step"></param>
        /// <param name="Alter"></param>
        public Root(MidiDefs.Step Step, int Alter)
        {
            this.Step = Step;
            this.Alter = Alter;
        }

        /// <summary>
        /// コンストラクタ(XDocument版)
        /// </summary>
        /// <param name="Source"></param>
        public Root(XElement Source)
        {
            //ソース読み取り
            XElement? ElmStep = Source.Element("root-step");
            XElement? ElmAlter = Source.Element("root-alter");

            //必須タグのチェック
            if (ElmStep == null)
            {
                throw new FormatException("<root>: <root-step>: Not found.");
            }
            //必須データの正当性チェック
            string RawStep = ElmStep.Value ?? "";
            if (!MidiDefs.StepMembers.Exists(x => x.Key.Equals(RawStep, StringComparison.OrdinalIgnoreCase)))
            {
                throw new ArgumentException("<root>: <root-step>: Invalid value.");
            }
            //必須データのセット
            this.Step = MidiDefs.StepMembers.FirstOrDefault(x => x.Key.Equals(RawStep, StringComparison.CurrentCultureIgnoreCase)).Value;
            //任意データの処理
            if (ElmAlter != null)
            {
                //任意データのセット
                if (!int.TryParse(ElmAlter.Value, out int RawAlterInt))
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

        #endregion

        #region "debug methods"

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

        #endregion
    }
}
