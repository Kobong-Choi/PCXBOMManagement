using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace CSI.PCC.PCX
{
    public partial class TypeSelection_WSTmplt : DevExpress.XtraEditors.XtraForm
    {
        public string SELECTED_TYPE;
        bool CLICKED = false;

        public TypeSelection_WSTmplt()
        {
            InitializeComponent();
        }

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);
            // 기본 선택 타입 : Style
            rdTypes.SelectedIndex = 1;
        }

        private void btnConfirm_Click(object sender, EventArgs e)
        {
            SELECTED_TYPE = rdTypes.EditValue.ToString();
            CLICKED = true;
            DialogResult = System.Windows.Forms.DialogResult.OK;
            this.Close();
        }

        private void TypeSelection_WSTmplt_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (CLICKED == false)
            {
                if (MessageBox.Show("Are you sure you want to cancel?", "Question",
                    MessageBoxButtons.OKCancel, MessageBoxIcon.Question, MessageBoxDefaultButton.Button1)
                    == System.Windows.Forms.DialogResult.OK)
                    DialogResult = System.Windows.Forms.DialogResult.No;
            }
        }
    }
}
