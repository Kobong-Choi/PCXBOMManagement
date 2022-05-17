using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Collections;           // ArrayList

using CSI.Client.ProjectBaseForm;   // ProjectBaseForm Class
using CSI.PCC.PCX.Packages;         // Package Class

using DevExpress.XtraEditors;

namespace CSI.PCC.PCX
{
    public partial class ConfirmReason : DevExpress.XtraEditors.XtraForm
    {
        public string Factory { get; set; }
        public string WSNumber { get; set; }
        public bool hasNormallySaved = false;

        public ConfirmReason()
        {
            InitializeComponent();
        }

        protected override void OnLoad(EventArgs e)
        {
            DataTable dataSource = null;

            base.OnLoad(e);

            // Bind dataSource to Reason lookUpEdit.
            PKG_INTG_BOM.SELECT_BOM_INFO_BY_CONFIRM pkgSelect = new PKG_INTG_BOM.SELECT_BOM_INFO_BY_CONFIRM();
            pkgSelect.ARG_WORK_TYPE = "Reason";
            pkgSelect.ARG_FACTORY = Factory;
            pkgSelect.ARG_WS_NO = WSNumber;
            pkgSelect.ARG_PART_SEQ = "";
            pkgSelect.OUT_CURSOR = string.Empty;

            dataSource = Common.projectBaseForm.Exe_Select_PKG(pkgSelect).Tables[0];
            leReason.Properties.DataSource = dataSource;

            // Bind dataSource to Detail lookUpEdit.
            PKG_INTG_BOM.SELECT_BOM_INFO_BY_CONFIRM pkgSelect2 = new PKG_INTG_BOM.SELECT_BOM_INFO_BY_CONFIRM();
            pkgSelect2.ARG_WORK_TYPE = "Detail";
            pkgSelect2.ARG_FACTORY = Factory;
            pkgSelect2.ARG_WS_NO = WSNumber;
            pkgSelect2.ARG_PART_SEQ = "";
            pkgSelect2.OUT_CURSOR = string.Empty;

            dataSource = Common.projectBaseForm.Exe_Select_PKG(pkgSelect2).Tables[0];
            leDetail.Properties.DataSource = dataSource;
        }

        /// <summary>
        /// 변경된 Reason 코드에 해당하는 Detail 데이터만 룩업 에딧에 다시 바인딩
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void leReason_EditValueChanged(object sender, EventArgs e)
        {
            PKG_INTG_BOM.SELECT_BOM_INFO_BY_CONFIRM pkgSelect = new PKG_INTG_BOM.SELECT_BOM_INFO_BY_CONFIRM();
            pkgSelect.ARG_WORK_TYPE = "DetailChange";
            pkgSelect.ARG_FACTORY = Factory;
            pkgSelect.ARG_WS_NO = WSNumber;
            pkgSelect.ARG_PART_SEQ = leReason.EditValue.ToString();
            pkgSelect.OUT_CURSOR = string.Empty;

            DataTable dataSource = Common.projectBaseForm.Exe_Select_PKG(pkgSelect).Tables[0];
            leDetail.Properties.DataSource = dataSource;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnSave_Click(object sender, EventArgs e)
        {
            ArrayList list = new ArrayList();

            Action<LookUpEdit> action = (edit) =>
            {
                if (edit.EditValue.ToString() == "")
                {
                    MessageBox.Show("Please choose one.", "",
                        MessageBoxButtons.OK, MessageBoxIcon.Warning, MessageBoxDefaultButton.Button1);
                    return;
                }
            };

            action(leReason);
            action(leDetail);

            // Save the confirm history.
            PKG_INTG_BOM.INSERT_CS_BOM_CFM_HISTORY pkgInsert = new PKG_INTG_BOM.INSERT_CS_BOM_CFM_HISTORY();
            pkgInsert.ARG_WORK_TYPE = "ReConfirm";
            pkgInsert.ARG_FACTORY = Factory;
            pkgInsert.ARG_WS_NO = WSNumber;
            pkgInsert.ARG_CFM_REASON = leReason.EditValue.ToString();
            pkgInsert.ARG_CFM_DETAIL = leDetail.EditValue.ToString();
            pkgInsert.ARG_UPD_USER = Common.sessionID;
            
            list.Add(pkgInsert);
            
            if (Common.projectBaseForm.Exe_Modify_PKG(list) == null)
            {
                MessageBox.Show("Failed to re-confirm.", "",
                    MessageBoxButtons.OK, MessageBoxIcon.Error, MessageBoxDefaultButton.Button1);
                return;
            }

            hasNormallySaved = true;
            this.DialogResult = System.Windows.Forms.DialogResult.OK;
        }
    }
}