using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

using System.Collections;                       // ArrayList
using DevExpress.XtraGrid;                      // GridControl
using DevExpress.XtraGrid.Columns;              // GridColumn
using DevExpress.XtraGrid.Views.Base;           // GridCell
using DevExpress.XtraGrid.Views.Grid;           // GridView
using DevExpress.XtraGrid.Views.Grid.ViewInfo;  // GridHitInfo
using DevExpress.Utils;                         // DXMouseEventArgs
using DevExpress.XtraEditors;                   // GridLookUpEdit
using CSI.Client.ProjectBaseForm;               // ProjectBaseForm Class
using CSI.PCC.PCX.COM;                          // Common Class
using CSI.PCC.PCX.PACKAGE;                      // Package Class

namespace CSI.PCC.PCX
{
    public partial class BOMEditing_FakeBOM : DevExpress.XtraEditors.XtraForm
    {
        public ProjectBaseForm projectBaseForm = Common.projectBaseForm;
        public string[] SOURCE_OF_BOM;
        
        PictureEdit[] editUppers = null;
        TextEdit[] editTexts = null;

        public BOMEditing_FakeBOM()
        {
            InitializeComponent();
        }

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);

            string factory = SOURCE_OF_BOM[0];
            string wsNo = SOURCE_OF_BOM[1];

            if (factory == "DS")
            {
                tpUpperImage_Ovs.PageVisible = false;
                editUppers = new PictureEdit[] { peUpperHQ1, peUpperHQ2, peUpperHQ3, peUpperHQ4 };
                editTexts = new TextEdit[] { teUpperHQ1, teUpperHQ2, teUpperHQ3, teUpperHQ4 };
            }
            else
            {
                tpUpperImage.PageVisible = false;
                editUppers = new PictureEdit[] { peUpperOVS1, peUpperOVS2, peUpperOVS3, peUpperOVS4, peUpperOVS5, peUpperOVS6 };
                editTexts = new TextEdit[] { teUpperOVS1, teUpperOVS2, teUpperOVS3, teUpperOVS4, teUpperOVS5, teUpperOVS6 };
            }

            string bomInfo = ReturnCurrentBOMInfo(factory, wsNo, "CBD");
            if (bomInfo == "")
                MessageBox.Show("Failed to load data");
            else
                this.Text = bomInfo;

            BindDataSourceToGridView(grdMaster, "TAIL", factory, wsNo, "UPPER");
            BindDataSourceToGridView(grdMidsole, "TAIL", factory, wsNo, "MIDSOLE");
            BindDataSourceToGridView(grdOutsole, "TAIL", factory, wsNo, "OUTSOLE");
        }

        #region 사용자 정의 함수
        /// <summary>
        /// 현재 작업중인 BOM 정보를 가져옴
        /// </summary>
        /// <param name="factory"></param>
        /// <param name="wsNo"></param>
        /// <param name="workType"></param>
        /// <returns></returns>
        private string ReturnCurrentBOMInfo(string factory, string wsNo, string workType)
        {
            try
            {
                // 패키지 매개변수 입력
                PKG_INTG_BOM.SELECT_FAKE_BOM_DATA pkgSelect = new PKG_INTG_BOM.SELECT_FAKE_BOM_DATA();
                pkgSelect.ARG_WORK_TYPE = workType;
                pkgSelect.ARG_FACTORY = factory;
                pkgSelect.ARG_WS_NO = wsNo;
                pkgSelect.ARG_PART_TYPE = "";
                pkgSelect.OUT_CURSOR = string.Empty;
                // 패키지 호출
                DataTable dataSource = projectBaseForm.Exe_Select_PKG(pkgSelect).Tables[0];
                if (dataSource.Rows.Count > 0)
                {
                    return dataSource.Rows[0]["BOM_NAME"].ToString();
                }
                return "";
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
                return "";
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="control"></param>
        /// <param name="workType"></param>
        /// <param name="factory"></param>
        /// <param name="wsNo"></param>
        /// <param name="partType"></param>
        private void BindDataSourceToGridView(GridControl control, string workType, string factory, string wsNo, string partType)
        {
            try
            {
                // 패키지 매개변수 입력
                PKG_INTG_BOM.SELECT_FAKE_BOM_DATA pkgSelect = new PKG_INTG_BOM.SELECT_FAKE_BOM_DATA();
                pkgSelect.ARG_WORK_TYPE = workType;
                pkgSelect.ARG_FACTORY = factory;
                pkgSelect.ARG_WS_NO = wsNo;
                pkgSelect.ARG_PART_TYPE = partType;
                pkgSelect.OUT_CURSOR = string.Empty;
                // 패키지 호출
                DataTable dataSource = projectBaseForm.Exe_Select_PKG(pkgSelect).Tables[0];
                control.DataSource = dataSource;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
                return;
            }
        }
        #endregion

        #region 버튼 이벤트
        private void btnSave_Click(object sender, EventArgs e)
        {

        }
        #endregion
    }
}
