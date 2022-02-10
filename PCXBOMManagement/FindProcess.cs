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
    public partial class FindProcess : DevExpress.XtraEditors.XtraForm
    {
        public List<string> SelectedProcess = new List<string>();
        public string EnteredProcess { get; set; }

        public FindProcess()
        {
            InitializeComponent();
        }

        protected override void OnLoad(EventArgs e)
        {
            DataTable dataSource = null;
            string[] process = EnteredProcess.Split(',');   // from parent.

            base.OnLoad(e);
           
            // BE
            PKG_INTG_BOM.SELECT_MST_PROCESS pkgSelectBE = new PKG_INTG_BOM.SELECT_MST_PROCESS();
            pkgSelectBE.ARG_DIV = "BE";
            pkgSelectBE.OUT_CURSOR = string.Empty;

            dataSource = Common.projectBaseForm.Exe_Select_PKG(pkgSelectBE).Tables[0];
            grdBE.DataSource = dataSource;

            // MO
            PKG_INTG_BOM.SELECT_MST_PROCESS pkgSelectMO = new PKG_INTG_BOM.SELECT_MST_PROCESS();
            pkgSelectMO.ARG_DIV = "MO";
            pkgSelectMO.OUT_CURSOR = string.Empty;

            dataSource = Common.projectBaseForm.Exe_Select_PKG(pkgSelectMO).Tables[0];
            grdMO.DataSource = dataSource;

            // SL
            PKG_INTG_BOM.SELECT_MST_PROCESS pkgSelectSL = new PKG_INTG_BOM.SELECT_MST_PROCESS();
            pkgSelectSL.ARG_DIV = "SL";
            pkgSelectSL.OUT_CURSOR = string.Empty;

            dataSource = Common.projectBaseForm.Exe_Select_PKG(pkgSelectSL).Tables[0];
            grdSL.DataSource = dataSource;

            // UP
            PKG_INTG_BOM.SELECT_MST_PROCESS pkgSelectUP = new PKG_INTG_BOM.SELECT_MST_PROCESS();
            pkgSelectUP.ARG_DIV = "UP";
            pkgSelectUP.OUT_CURSOR = string.Empty;

            dataSource = Common.projectBaseForm.Exe_Select_PKG(pkgSelectUP).Tables[0];
            grdUP.DataSource = dataSource;

            // Check box for already entered process on grid.
            if (process.Length > 0)
            {
                Action<GridView> action = (view) =>
                {
                    for (int i = 0; i < view.RowCount; i++)
                    {
                        if (process.Contains(view.GetRowCellValue(i, "OP_NAME_EN").ToString()))
                            view.SetRowCellValue(i, "CHK", "Y");
                    }
                };

                action(gvwBE);
                action(gvwMO);
                action(gvwSL);
                action(gvwUP);
            }

            ShowSelectedProcess();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnSave_Click(object sender, EventArgs e)
        {
            Action<GridView> action = (view) =>
            {
                for (int i = 0; i < view.RowCount; i++)
                {
                    if (view.GetRowCellValue(i, "CHK").ToString().Equals("Y"))
                        SelectedProcess.Add(view.GetRowCellValue(i, "OP_NAME_EN").ToString());
                }
            };

            action(gvwBE);
            action(gvwMO);
            action(gvwSL);
            action(gvwUP);

            this.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.Close();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void CustomClick(object sender, EventArgs e)
        {
            GridView view = sender as GridView;

            if (view.GetRowCellValue(view.FocusedRowHandle, "CHK").ToString().Equals("Y"))
                view.SetRowCellValue(view.FocusedRowHandle, "CHK", "N");
            else
                view.SetRowCellValue(view.FocusedRowHandle, "CHK", "Y");

            ShowSelectedProcess();
        }

        /// <summary>
        /// 
        /// </summary>
        private void ShowSelectedProcess()
        {
            // Initialize
            lblConcatProcess.Text = "";

            Action<GridView> action = (view) =>
            {
                for (int i = 0; i < view.RowCount; i++)
                {
                    if (view.GetRowCellValue(i, "CHK").ToString().Equals("Y"))
                        lblConcatProcess.Text += string.Format("*{0}\n", view.GetRowCellValue(i, "OP_NAME_EN").ToString());
                }
            };

            action(gvwBE);
            action(gvwMO);
            action(gvwSL);
            action(gvwUP);
        }
    }
}