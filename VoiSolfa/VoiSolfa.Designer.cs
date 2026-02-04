namespace VoiSolfa
{
    partial class VoiSolfa
    {
        /// <summary>
        ///  Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        ///  Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        ///  Required method for Designer support - do not modify
        ///  the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            TxtXmlPath = new TextBox();
            BtnRefer = new Button();
            BtnCreateXml = new Button();
            LblXmlPath = new Label();
            DlgOpenFile = new OpenFileDialog();
            BtnDebug = new Button();
            DlgSaveFile = new SaveFileDialog();
            LblSolfaSetting = new Label();
            CmbSolfaSetting = new ComboBox();
            CbxTransposeToConcertKey = new CheckBox();
            SuspendLayout();
            // 
            // TxtXmlPath
            // 
            TxtXmlPath.Location = new Point(115, 18);
            TxtXmlPath.Margin = new Padding(4);
            TxtXmlPath.Name = "TxtXmlPath";
            TxtXmlPath.Size = new Size(628, 27);
            TxtXmlPath.TabIndex = 1;
            TxtXmlPath.TextChanged += TxtXmlPath_TextChanged;
            // 
            // BtnRefer
            // 
            BtnRefer.Location = new Point(745, 18);
            BtnRefer.Margin = new Padding(4);
            BtnRefer.Name = "BtnRefer";
            BtnRefer.Size = new Size(34, 29);
            BtnRefer.TabIndex = 2;
            BtnRefer.Text = "...";
            BtnRefer.UseVisualStyleBackColor = true;
            BtnRefer.Click += BtnRefer_Click;
            // 
            // BtnCreateXml
            // 
            BtnCreateXml.Location = new Point(334, 96);
            BtnCreateXml.Margin = new Padding(4);
            BtnCreateXml.Name = "BtnCreateXml";
            BtnCreateXml.Size = new Size(174, 35);
            BtnCreateXml.TabIndex = 6;
            BtnCreateXml.Text = "Create Solfage Xml";
            BtnCreateXml.UseVisualStyleBackColor = true;
            BtnCreateXml.Click += BtnCreateXml_Click;
            // 
            // LblXmlPath
            // 
            LblXmlPath.AutoSize = true;
            LblXmlPath.Location = new Point(15, 22);
            LblXmlPath.Margin = new Padding(4, 0, 4, 0);
            LblXmlPath.Name = "LblXmlPath";
            LblXmlPath.Size = new Size(73, 20);
            LblXmlPath.TabIndex = 0;
            LblXmlPath.Text = "MusicXml";
            // 
            // DlgOpenFile
            // 
            DlgOpenFile.DefaultExt = "xml";
            // 
            // BtnDebug
            // 
            BtnDebug.Enabled = false;
            BtnDebug.Location = new Point(685, 99);
            BtnDebug.Name = "BtnDebug";
            BtnDebug.Size = new Size(94, 29);
            BtnDebug.TabIndex = 7;
            BtnDebug.Text = "Debug";
            BtnDebug.UseVisualStyleBackColor = true;
            BtnDebug.Click += BtnDebug_Click;
            // 
            // LblSolfaSetting
            // 
            LblSolfaSetting.AutoSize = true;
            LblSolfaSetting.Location = new Point(15, 61);
            LblSolfaSetting.Name = "LblSolfaSetting";
            LblSolfaSetting.Size = new Size(94, 20);
            LblSolfaSetting.TabIndex = 3;
            LblSolfaSetting.Text = "Solfa Setting";
            // 
            // CmbSolfaSetting
            // 
            CmbSolfaSetting.DropDownStyle = ComboBoxStyle.DropDownList;
            CmbSolfaSetting.FormattingEnabled = true;
            CmbSolfaSetting.Location = new Point(115, 58);
            CmbSolfaSetting.Name = "CmbSolfaSetting";
            CmbSolfaSetting.Size = new Size(628, 28);
            CmbSolfaSetting.TabIndex = 4;
            // 
            // CbxTransposeToConcertKey
            // 
            CbxTransposeToConcertKey.AutoSize = true;
            CbxTransposeToConcertKey.Checked = true;
            CbxTransposeToConcertKey.CheckState = CheckState.Checked;
            CbxTransposeToConcertKey.Location = new Point(115, 99);
            CbxTransposeToConcertKey.Name = "CbxTransposeToConcertKey";
            CbxTransposeToConcertKey.Size = new Size(197, 24);
            CbxTransposeToConcertKey.TabIndex = 5;
            CbxTransposeToConcertKey.Text = "Transpose to Concert Key";
            CbxTransposeToConcertKey.UseVisualStyleBackColor = true;
            // 
            // VoiSolfa
            // 
            AutoScaleDimensions = new SizeF(120F, 120F);
            AutoScaleMode = AutoScaleMode.Dpi;
            ClientSize = new Size(802, 142);
            Controls.Add(CbxTransposeToConcertKey);
            Controls.Add(CmbSolfaSetting);
            Controls.Add(LblSolfaSetting);
            Controls.Add(BtnDebug);
            Controls.Add(LblXmlPath);
            Controls.Add(BtnCreateXml);
            Controls.Add(BtnRefer);
            Controls.Add(TxtXmlPath);
            Margin = new Padding(4);
            Name = "VoiSolfa";
            Text = "VoiSolfa";
            Load += Main_Load;
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private TextBox TxtXmlPath;
        private Button BtnRefer;
        private Button BtnCreateXml;
        private Label LblXmlPath;
        private OpenFileDialog DlgOpenFile;
        private Button BtnDebug;
        private SaveFileDialog DlgSaveFile;
        private Label LblSolfaSetting;
        private ComboBox CmbSolfaSetting;
        private CheckBox CbxTransposeToConcertKey;
    }
}
