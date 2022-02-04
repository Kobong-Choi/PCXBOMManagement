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
    public partial class CSMaterialInformation : DevExpress.XtraEditors.XtraForm
    {
        public ProjectBaseForm projectBaseForm = Common.projectBaseForm;
        public Dictionary<string, string> SOURCE_OF_MAT_INFO = null;

        public CSMaterialInformation()
        {
            InitializeComponent();
        }

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);

            if (SOURCE_OF_MAT_INFO.Count > 0)
            {
                string PDMSuppMatNumber = SOURCE_OF_MAT_INFO["MXSXL_NUMBER"];
                string csCode = SOURCE_OF_MAT_INFO["CS_CD"];

                #region 마스터에서 자재 정보를 가져옴
                PKG_INTG_BOM.SELECT_MATERIAL_INFO pkgSelect = new PKG_INTG_BOM.SELECT_MATERIAL_INFO();
                pkgSelect.ARG_WORK_TYPE = "CS";
                pkgSelect.ARG_MXSXL_NUMBER = PDMSuppMatNumber;
                pkgSelect.ARG_MAT_CD = "";
                pkgSelect.ARG_CS_CD = csCode;
                pkgSelect.OUT_CURSOR = string.Empty;

                DataTable dataSource = projectBaseForm.Exe_Select_PKG(pkgSelect).Tables[0];
                #endregion

                BindDataSourceToControl(dataSource);
            }
        }

        /// <summary>
        /// 데이터베이스에서 가져온 데이터를 각 컨트롤에 바인딩한다.
        /// </summary>
        /// <param name="dataSource"></param>
        private void BindDataSourceToControl(DataTable dataSource)
        {
            try
            {
                txtMaterialName.Text = dataSource.Rows[0]["MAT_NAME"].ToString();
                txtComment.Text = dataSource.Rows[0]["MAT_COMMENT"].ToString();
                txtMaterialCode.Text = dataSource.Rows[0]["MXSXL_NUMBER"].ToString();
                txtMCS.Text = dataSource.Rows[0]["MCS_NUMBER"].ToString();
                txtMaterialType.Text = dataSource.Rows[0]["MAT_TYPE"].ToString();
                txtSuppCountry.Text = dataSource.Rows[0]["VEN_COUNTRY"].ToString();
                txtSuppName.Text = dataSource.Rows[0]["VENDOR_NAME"].ToString();
                txtStatus.Text = dataSource.Rows[0]["MCS_STATUS"].ToString();
                txtUsageStatus.Text = dataSource.Rows[0]["USAGE_STATUS"].ToString();
                txtRishClass.Text = dataSource.Rows[0]["RISK_CLASS"].ToString();
                txtUnit.Text = dataSource.Rows[0]["UOM"].ToString();
                txtWidth.Text = dataSource.Rows[0]["WIDTH"].ToString();
                seUnitPrice.Text = dataSource.Rows[0]["PRICE"].ToString();
                txtCurrency.Text = dataSource.Rows[0]["CURRENCY"].ToString();
                txtUpcharge.Text = dataSource.Rows[0]["CURRENCY"].ToString();
                seMSIScore.Text = dataSource.Rows[0]["MSI_POINT"].ToString();
                seSampleLeadTime.Text = dataSource.Rows[0]["SAMPLE_LT"].ToString();
                seProdLeadTime.Text = dataSource.Rows[0]["PROD_LT"].ToString();
                meSpecialComment.Text = dataSource.Rows[0]["SpecialComment"].ToString();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
                return;
            }
        }
    }
}
