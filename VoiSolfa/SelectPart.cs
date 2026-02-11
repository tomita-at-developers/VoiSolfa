using System.ComponentModel;

namespace VoiSolfa
{
    public partial class SelectPart : Form
    {
        /// <summary>
        /// 選択対象のパートリスト
        /// </summary>
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public Dictionary<string, string> Parts { get; set; } = [];
        /// <summary>
        /// 選択されたパート名
        /// </summary>
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public string SelectedPartName { get; set; } = string.Empty;
        /// <summary>
        /// 終了状態
        /// </summary>
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public DialogResult Result { get; private set; } = DialogResult.Cancel;

        /// <summary>
        /// コンストラクタ
        /// </summary>
        public SelectPart()
        {
            InitializeComponent();
        }

        /// <summary>
        /// フォームロード
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void SelectPart_Load(object sender, EventArgs e)
        {
            //リストボックスのアイテムセット
            foreach (KeyValuePair<string, string> Part in Parts)
            {
                this.LstPart.Items.Add(Part);
            }
            //ＯＫボタンは無効化しておく
            this.BtnOK.Enabled = false;
        }

        /// <summary>
        /// ＯＫボタンクリック
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BtnOK_Click(object sender, EventArgs e)
        {
            //一つ選択されている場合(設定もSelectionMode=One)
            if (this.LstPart.SelectedItems.Count == 1)
            {
                //選択情報の保存
                this.Result = DialogResult.OK;
                this.SelectedPartName = ((KeyValuePair<string, string>)this.LstPart.SelectedItem).Key;
                //フォームを閉じる
                this.Close();
            }
            else
            {
                MessageBox.Show("Plese select the part to process.");
            }
        }

        /// <summary>
        /// キャンセルボタンクリック
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BtnCancel_Click(object sender, EventArgs e)
        {
            //選択情報のクリア
            this.Result = DialogResult.Cancel;
            this.SelectedPartName = string.Empty;
            //フォームを閉じる
            this.Close();
        }

        /// <summary>
        /// リストボックスの選択値変更
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void LstPart_SelectedValueChanged(object sender, EventArgs e)
        {
            //一つ選択されている場合(設定もSelectionMode=One)
            if (this.LstPart.SelectedItems.Count == 1)
            {
                //ＯＫボタン有効化
                this.BtnOK.Enabled = true;
            }
            else
            {
                //ＯＫボタン無効化
                this.BtnOK.Enabled = false;
            }
        }
    }
}
