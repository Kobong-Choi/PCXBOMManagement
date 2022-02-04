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
    public partial class BOMSelection : DevExpress.XtraEditors.XtraForm
    {
        public ProjectBaseForm projectBaseForm = Common.projectBaseForm;
        public string FACTORY = string.Empty;
        public string CONCAT_WS_NO = string.Empty;
        public object FORM_RESULT;

        public BOMSelection()
        {
            InitializeComponent();
        }

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);

            PKG_INTG_BOM.SELECT_BOM_HEAD_DATA pkgSelectHeadData = new PKG_INTG_BOM.SELECT_BOM_HEAD_DATA();
            pkgSelectHeadData.ARG_FACTORY = FACTORY;
            pkgSelectHeadData.ARG_CONCAT_WS_NO = CONCAT_WS_NO;
            pkgSelectHeadData.OUT_CURSOR = string.Empty;

            DataTable dataSource = projectBaseForm.Exe_Select_PKG(pkgSelectHeadData).Tables[0];
            grdData.DataSource = dataSource;
        }

        /// <summary>
        /// 행을 더블 클릭하여 사용할 BOM 선택
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void gvwData_DoubleClick(object sender, EventArgs e)
        {
            GridView view = sender as GridView;
            PassChildDataToParent(view);
        }

        /// <summary>
        /// Enter 키를 입력하여 사용할 BOM 선택
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void gvwData_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                GridView view = sender as GridView;
                PassChildDataToParent(view);
            }
        }

        /// <summary>
        /// Add to a specific colorway.
        /// </summary>
        /// <param name="view"></param>
        private void PassChildDataToParent(GridView view)
        {
            List<string> result = new List<string> {
                view.GetFocusedRowCellValue("FACTORY").ToString(),
                view.GetFocusedRowCellValue("WS_NO").ToString(),
                view.GetFocusedRowCellValue("BOM_ID").ToString(),
                view.GetFocusedRowCellValue("DEV_COLORWAY_ID").ToString()
            };

            FORM_RESULT = result;

            this.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.Close();
        }

        /// <summary>
        /// Add to all colorway.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnAddAll_Click(object sender, EventArgs e)
        {
            DataTable dataSource = grdData.DataSource as DataTable;

            FORM_RESULT = dataSource;

            this.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.Close();
        }
    }
}