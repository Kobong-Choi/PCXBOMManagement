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
    public partial class TypeSelection_BOM : DevExpress.XtraEditors.XtraForm
    {
        public string SelectedType { get; set; }
        private bool hasConfirmed;

        public TypeSelection_BOM()
        {
            InitializeComponent();
        }

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);
            
            rdTypes.SelectedIndex = 2; // Default is JSON.
        }

        private void btnConfirm_Click(object sender, EventArgs e)
        {
            SelectedType = rdTypes.EditValue.ToString();
            hasConfirmed = true;
            DialogResult = DialogResult.OK;
        }

        private void TypeSelection_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (hasConfirmed == true)
            {
                /* Normal Close. */
            }
            else
            {
                /* Ask if the user wants to end the form without uploading the BOM. */

                if (MessageBox.Show("Don't you want to upload BOM?", "",
                        MessageBoxButtons.OKCancel, MessageBoxIcon.Question, MessageBoxDefaultButton.Button1) == DialogResult.OK)
                {
                    DialogResult = System.Windows.Forms.DialogResult.No;
                }
                else
                {
                    MessageBox.Show("Please choose one.", "",
                        MessageBoxButtons.OK, MessageBoxIcon.Information, MessageBoxDefaultButton.Button1);

                    e.Cancel = true;
                }
            }
        }
    }
}
