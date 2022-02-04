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
        public ProjectBaseForm projectBaseForm = Common.projectBaseForm;
        public Dictionary<string, string> SOURCE_OF_MAT_INFO = null;

        public MaterialInformation()
        {
            InitializeComponent();
        }

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);

            string PDMSuppMatNumber = SOURCE_OF_MAT_INFO["MXSXL_NUMBER"];
            string materialCode = SOURCE_OF_MAT_INFO["MAT_CD"];

            #region 마스터에서 자재 정보를 가져옴
            
            PKG_INTG_BOM.SELECT_MATERIAL_INFO pkgSelect = new PKG_INTG_BOM.SELECT_MATERIAL_INFO();
            pkgSelect.ARG_WORK_TYPE = "PCC";
            pkgSelect.ARG_MXSXL_NUMBER = PDMSuppMatNumber;
            pkgSelect.ARG_MAT_CD = materialCode;
            pkgSelect.ARG_CS_CD = "";
            pkgSelect.OUT_CURSOR = string.Empty;
            
            DataTable dataSource = projectBaseForm.Exe_Select_PKG(pkgSelect).Tables[0];

            #endregion

            if (dataSource.Rows.Count == 0) 
                return;
            else if (dataSource.Rows.Count == 1)
            {
                this.Close();

                SOURCE_OF_MAT_INFO = new Dictionary<string, string>();
                SOURCE_OF_MAT_INFO.Add("MXSXL_NUMBER", dataSource.Rows[0]["MXSXL_NUMBER"].ToString().Trim());
                SOURCE_OF_MAT_INFO.Add("CS_CD", dataSource.Rows[0]["CS_CD"].ToString().Trim());

                CSMaterialInformation form = new CSMaterialInformation();
                form.SOURCE_OF_MAT_INFO = SOURCE_OF_MAT_INFO;

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
                MessageBox.Show("There is no material to show information.");
            }
        }

        /// <summary>
        /// 유저가 선택한 자재 정보를 띄움
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void gvwBase_DoubleClick(object sender, EventArgs e)
        {
            try
            {
                Dictionary<string, string> dic = new Dictionary<string, string>();
                dic.Add("MXSXL_NUMBER", gvwBase.GetFocusedRowCellValue("MXSXL_NUMBER").ToString());
                dic.Add("CS_CD", gvwBase.GetFocusedRowCellValue("CS_CD").ToString());

                CSMaterialInformation form = new CSMaterialInformation();
                form.SOURCE_OF_MAT_INFO = dic;

                form.ShowDialog();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
                return;
            }
        }
    }
}
