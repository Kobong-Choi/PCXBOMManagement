using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

using CSI.Client.ProjectBaseForm;               // ProjectBaseForm Class
using CSI.PCC.PCX.PACKAGE;                      // Package Class
using CSI.PCC.PCX.COM;                          // Common Class

namespace CSI.PCC.PCX
{
    public partial class MaterialInformation : DevExpress.XtraEditors.XtraForm
    {
        public Dictionary<string, string> MaterialInfo { get; set; }

        public MaterialInformation()
        {
            InitializeComponent();
        }

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);

            PKG_INTG_BOM.SELECT_MATERIAL_INFO pkgSelect = new PKG_INTG_BOM.SELECT_MATERIAL_INFO();
            pkgSelect.ARG_WORK_TYPE = "PCC";
            pkgSelect.ARG_MXSXL_NUMBER = MaterialInfo["MXSXL_NUMBER"];
            pkgSelect.ARG_MAT_CD = MaterialInfo["MAT_CD"];
            pkgSelect.ARG_CS_CD = "";
            pkgSelect.OUT_CURSOR = string.Empty;
            
            DataTable dataSource = Common.projectBaseForm.Exe_Select_PKG(pkgSelect).Tables[0];

            if (dataSource.Rows.Count == 0)
            {
                return;
            }
            else if (dataSource.Rows.Count == 1)
            {
                this.Close();

                MaterialInfo.Clear();
                MaterialInfo.Add("MXSXL_NUMBER", dataSource.Rows[0]["MXSXL_NUMBER"].ToString().Trim());
                MaterialInfo.Add("CS_CD", dataSource.Rows[0]["CS_CD"].ToString().Trim());

                CSMaterialInformation form = new CSMaterialInformation() { MaterialInfo = this.MaterialInfo };
                form.ShowDialog();
            }
            else if (dataSource.Rows.Count > 1)
            {
                dataSource.AcceptChanges();
                grdBase.DataSource = dataSource;
            }
            else
            {
                this.Close();
                Common.ShowMessageBox("There is no material to show information.", "E");
            }
        }

        /// <summary>
        /// 유저가 선택한 자재 정보를 띄움
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void gvwBase_DoubleClick(object sender, EventArgs e)
        {
            MaterialInfo.Clear();
            MaterialInfo.Add("MXSXL_NUMBER", gvwBase.GetFocusedRowCellValue("MXSXL_NUMBER").ToString());
            MaterialInfo.Add("CS_CD", gvwBase.GetFocusedRowCellValue("CS_CD").ToString());

            CSMaterialInformation form = new CSMaterialInformation() { MaterialInfo = this.MaterialInfo };
            form.ShowDialog();
        }
    }
}
