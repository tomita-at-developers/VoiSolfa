using Developers.MidiXml;
using Developers.MidiXml.Elements;
using System.Diagnostics;

namespace VoiSolfa
{
    public partial class VoiSolfa : Form
    {
        private enum TieType
        {
            None = 0,
            Start = 1,
            Stop = 2,
            Continue = 3,
            LetRing = 4
        }

        public VoiSolfa()
        {
            InitializeComponent();
        }

        /// <summary>
        /// フォームロード時の処理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Main_Load(object sender, EventArgs e)
        {
            //コントロールの初期化
            this.TxtXmlPath.Text = string.Empty;
            this.BtnCreateXml.Enabled = false;
            this.BtnDebug.Enabled = true;
            //SolfaSettingコンボボックスの値設定
            Solfege Solfege = new Solfege();
            List<string> SettingNames = Solfege.SofaSettingNames;
            this.CmbSolfaSetting.Items.Add("");
            foreach (string SettingName in SettingNames)
            {
                this.CmbSolfaSetting.Items.Add(SettingName);
            }
            this.CmbSolfaSetting.SelectedIndex = 0;
        }

        /// <summary>
        /// 参照ボタンクリック
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BtnRefer_Click(object sender, EventArgs e)
        {
            try
            {
                //「ファイルを開く」ダイアログの表示
                this.DlgOpenFile.Filter = "MusixXmlファイル(*.xml;*.musicxml)|*.xml;*.musicxml|すべてのファイル(*.*)|*.*";
                this.DlgOpenFile.FilterIndex = 1;
                this.DlgOpenFile.CheckFileExists = true;
                this.DlgOpenFile.CheckPathExists = true;
                DialogResult DlgResult = this.DlgOpenFile.ShowDialog();
                //ファイルが指定されていればテキストボックスに表示
                if (DlgResult == DialogResult.OK)
                {
                    this.TxtXmlPath.Text = this.DlgOpenFile.FileName;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("[BtnRefer]" + Environment.NewLine + ex.Message);
            }
        }

        /// <summary>
        /// ファイルパス更新
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void TxtXmlPath_TextChanged(object sender, EventArgs e)
        {
            //指定されたファイルパスが存在すればXML生成ボタンを有効にする
            if (this.TxtXmlPath.Text.Length > 0 && File.Exists(this.TxtXmlPath.Text))
            {
                this.BtnCreateXml.Enabled = true;
            }
            else
            {
                this.BtnCreateXml.Enabled = false;
            }
        }

        /// <summary>
        /// XMLファイル作成ボタンクリック
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BtnCreateXml_Click(object sender, EventArgs e)
        {
            try
            {
                //フォーム上のコントロールを一旦無効化
                this.TxtXmlPath.Enabled = false;
                this.BtnRefer.Enabled = false;
                this.CmbSolfaSetting.Enabled = false;
                this.BtnCreateXml.Enabled = false;
                //ファイルが指定されていれば処理する
                if (this.TxtXmlPath.Text.Length > 0 && File.Exists(this.TxtXmlPath.Text))
                {
                    //出力ファイル名
                    string OutputFileName = string.Empty;
                    //「ファイル保存」ダイアログの表示
                    this.DlgSaveFile.InitialDirectory = Path.GetDirectoryName(this.TxtXmlPath.Text);
                    this.DlgSaveFile.FileName = DateTime.Now.ToString("yyyyMMddHHmmss") + "_" + Path.GetFileName(this.TxtXmlPath.Text);
                    this.DlgSaveFile.Filter = "MusixXmlファイル(*.xml;*.musicxml)|*.xml;*.musicxml|すべてのファイル(*.*)|*.*";
                    this.DlgSaveFile.FilterIndex = 1;
                    this.DlgSaveFile.OverwritePrompt = true;
                    DialogResult DlgResult = this.DlgSaveFile.ShowDialog();
                    //ＯＫクリックのとき
                    if (DlgResult == DialogResult.OK)
                    {
                        //出力ファイル名保存
                        OutputFileName = this.DlgSaveFile.FileName;
                        //Solgaの生成
                        Solfege Solfege = new Solfege();
                        Solfege.XmlImport(this.TxtXmlPath.Text);
                        Solfege.CreateLyrics(this.CmbSolfaSetting.SelectedItem.ToString(), this.CbxOctaveDown.Checked);
                        Solfege.XmlExport(OutputFileName);
                        MessageBox.Show("Music Xml file is saved." + Environment.NewLine + OutputFileName);
                    }
                }
                else
                {
                    MessageBox.Show("Cannot find the specified file." + Environment.NewLine + this.TxtXmlPath.Text);
                }

            }
            catch (Exception ex)
            {
                MessageBox.Show("[BtnCreateXml]" + Environment.NewLine + ex.Message);
            }
            finally
            {
                //フォーム上のコントロールを有効化
                this.TxtXmlPath.Enabled = true;
                this.BtnRefer.Enabled = true;
                this.CmbSolfaSetting.Enabled = true;
                this.BtnCreateXml.Enabled = true;
            }
        }

        /// <summary>
        /// デバック用処理のボタンクリック
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BtnDebug_Click(object sender, EventArgs e)
        {
            //DebugPitchClassTester();
            DebugPitchTester();
            //DebugPitchCalc00();
            //DebugPitchCalc01();
        }

        private void DebugPitchClassTester()
        {
            PitchClass Sample00 = new PitchClass(MidiDefs.Step.C, 1);
            PitchClass Sample01 = new PitchClass(MidiDefs.Step.C, 2);
            Debug.Print("");
        }

        private void DebugPitchCalc00()
        {
            int p0 = PitchUtil.DebugGetCBasedChromaticIndex(MidiDefs.Step.C, 0);
            int p1 = PitchUtil.DebugGetCBasedChromaticIndex(MidiDefs.Step.C, -1);
            int p2 = PitchUtil.DebugGetCBasedChromaticIndex(MidiDefs.Step.D, -1);
            int p3 = PitchUtil.DebugGetCBasedChromaticIndex(MidiDefs.Step.D, -3);
            Debug.Print("");
        }
        private void DebugPitchCalc01()
        {
            MidiDefs.Step Step;
            int Octave;
            int Alter;

            Step = MidiDefs.Step.F;
            Octave = 4;
            Alter = -1;
            PitchUtil.AdjustToRealPitch(ref Step, ref Octave, ref Alter);
            Step = MidiDefs.Step.F;
            Octave = 4;
            Alter = 2;
            PitchUtil.AdjustToRealPitch(ref Step, ref Octave, ref Alter);
            Step = MidiDefs.Step.E;
            Octave = 4;
            Alter = 1;
            PitchUtil.AdjustToRealPitch(ref Step, ref Octave, ref Alter);
            Step = MidiDefs.Step.E;
            Octave = 4;
            Alter = 13;
            PitchUtil.AdjustToRealPitch(ref Step, ref Octave, ref Alter);
            Step = MidiDefs.Step.B;
            Octave = 3;
            Alter = 1;
            PitchUtil.AdjustToRealPitch(ref Step, ref Octave, ref Alter);
            Step = MidiDefs.Step.C;
            Octave = 4;
            Alter = 0;
            PitchUtil.AdjustToRealPitch(ref Step, ref Octave, ref Alter);

            Step = MidiDefs.Step.C;
            Octave = 4;
            Alter = -1;
            PitchUtil.AdjustToRealPitch(ref Step, ref Octave, ref Alter);

            Step = MidiDefs.Step.D;
            Octave = 4;
            Alter = -1;
            PitchUtil.AdjustToRealPitch(ref Step, ref Octave, ref Alter);

            Step = MidiDefs.Step.D;
            Octave = 4;
            Alter = -3;
            PitchUtil.AdjustToRealPitch(ref Step, ref Octave, ref Alter);

            Debug.Print("");
        }

        private void DebugPitchTester()
        {
            Pitch Sample00 = new Pitch(MidiDefs.Step.C, MidiDefs.OCTAVE_CENTER, 1);
            Pitch Sample01 = new Pitch(MidiDefs.Step.C, MidiDefs.OCTAVE_CENTER, 2);
            Pitch Sample02 = new Pitch(MidiDefs.Step.D, MidiDefs.OCTAVE_CENTER, -3);
            Debug.Print("");
        }


    }
}
