namespace VoiSolfa
{
    partial class SelectPart
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
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
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            LstPart = new ListBox();
            BtnOK = new Button();
            BtnCancel = new Button();
            label1 = new Label();
            SuspendLayout();
            // 
            // LstPart
            // 
            LstPart.FormattingEnabled = true;
            LstPart.HorizontalScrollbar = true;
            LstPart.Location = new Point(12, 32);
            LstPart.Name = "LstPart";
            LstPart.Size = new Size(678, 144);
            LstPart.TabIndex = 1;
            LstPart.SelectedValueChanged += LstPart_SelectedValueChanged;
            // 
            // BtnOK
            // 
            BtnOK.Location = new Point(242, 183);
            BtnOK.Margin = new Padding(4);
            BtnOK.Name = "BtnOK";
            BtnOK.Size = new Size(107, 35);
            BtnOK.TabIndex = 2;
            BtnOK.Text = "OK";
            BtnOK.UseVisualStyleBackColor = true;
            BtnOK.Click += BtnOK_Click;
            // 
            // BtnCancel
            // 
            BtnCancel.Location = new Point(357, 183);
            BtnCancel.Margin = new Padding(4);
            BtnCancel.Name = "BtnCancel";
            BtnCancel.Size = new Size(107, 35);
            BtnCancel.TabIndex = 3;
            BtnCancel.Text = "Cancel";
            BtnCancel.UseVisualStyleBackColor = true;
            BtnCancel.Click += BtnCancel_Click;
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Location = new Point(12, 9);
            label1.Name = "label1";
            label1.Size = new Size(224, 20);
            label1.TabIndex = 0;
            label1.Text = "Please select the part to process.";
            // 
            // SelectPart
            // 
            AutoScaleDimensions = new SizeF(120F, 120F);
            AutoScaleMode = AutoScaleMode.Dpi;
            ClientSize = new Size(705, 224);
            Controls.Add(label1);
            Controls.Add(BtnCancel);
            Controls.Add(BtnOK);
            Controls.Add(LstPart);
            FormBorderStyle = FormBorderStyle.FixedDialog;
            Name = "SelectPart";
            StartPosition = FormStartPosition.CenterParent;
            Text = "Select Part";
            Load += SelectPart_Load;
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private ListBox LstPart;
        private Button BtnOK;
        private Button BtnCancel;
        private Label label1;
    }
}