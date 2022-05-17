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
using CSI.PCC.PCX.Packages;                     // Package Class

namespace CSI.PCC.PCX
{
    public partial class PartListForPE : DevExpress.XtraEditors.XtraForm
    {
        //public ProjectBaseForm projectBaseForm = Common.projectBaseForm;

        public string Factory { get; set; }
        public string WorksheetNumber { get; set; }

        public PartListForPE()
        {
            InitializeComponent();
        }

        protected override void OnLoad(EventArgs e)
        {
            DataSet ds = null;

            base.OnLoad(e);

            if (IncludeNullCSPatternParts())
            {
                MessageBox.Show("There are parts of which CS pattern part is not entered.", "",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning, MessageBoxDefaultButton.Button1);

                this.Close();
            }

            // Load caption info.
            PKG_INTG_BOM.PART_LIST_FOR_PE pkgSelect = new PKG_INTG_BOM.PART_LIST_FOR_PE();
            pkgSelect.ARG_WORK_TYPE = "Caption";
            pkgSelect.ARG_FACTORY = Factory;
            pkgSelect.ARG_WS_NO = WorksheetNumber;
            pkgSelect.OUT_CURSOR = string.Empty;

            ds = Common.projectBaseForm.Exe_Select_PKG(pkgSelect);

            if (ds.Tables[0].Rows.Count > 0)
            {
                this.Text = string.Format("{0}_{1}_{2}_{3}_{4}_{5}",
                    ds.Tables[0].Rows[0]["CATEGORY"].ToString(),
                    ds.Tables[0].Rows[0]["SEASON"].ToString(),
                    ds.Tables[0].Rows[0]["SAMPLE_TYPE"].ToString(),
                    ds.Tables[0].Rows[0]["DEV_NAME"].ToString(),
                    ds.Tables[0].Rows[0]["DEV_STYLE_NUMBER"].ToString(),
                    ds.Tables[0].Rows[0]["COLOR_VER"].ToString());
            }

            PKG_INTG_BOM.PART_LIST_FOR_PE pkgSelect2 = new PKG_INTG_BOM.PART_LIST_FOR_PE();
            pkgSelect2.ARG_WORK_TYPE = "Data";
            pkgSelect2.ARG_FACTORY = Factory;
            pkgSelect2.ARG_WS_NO = WorksheetNumber;
            pkgSelect2.OUT_CURSOR = string.Empty;

            ds = Common.projectBaseForm.Exe_Select_PKG(pkgSelect2);

            if (ds.Tables[0].Rows.Count > 0)
                grdList.DataSource = ds.Tables[0];
        }

        /// <summary>
        /// Export data from view.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnExport_Click(object sender, EventArgs e)
        {
            SaveFileDialog dialog = new SaveFileDialog();
            dialog.Title = "Please specify the path you want to save.";
            dialog.Filter = "Excel File (*.xls)|*.xls|Excel File (*.xlsx)|*.xlsx";
            dialog.FileName = this.Text;
            dialog.OverwritePrompt = true;

            if (dialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                //grdList.ExportToExcelOld(dialog.FileName);
                grdList.ExportToXls(dialog.FileName);
            }
        }

        /// <summary>
        /// Confirm there are parts of which CS Pattern Part is not entered.
        /// </summary>
        /// <returns></returns>
        private bool IncludeNullCSPatternParts()
        {
            DataSet ds = null;

            PKG_INTG_BOM.PART_LIST_FOR_PE pkgSelect = new PKG_INTG_BOM.PART_LIST_FOR_PE();
            pkgSelect.ARG_WORK_TYPE = "Validate";
            pkgSelect.ARG_FACTORY = Factory;
            pkgSelect.ARG_WS_NO = WorksheetNumber;
            pkgSelect.OUT_CURSOR = string.Empty;

            ds = Common.projectBaseForm.Exe_Select_PKG(pkgSelect);

            return Convert.ToInt32(ds.Tables[0].Rows[0]["CNT"]) > 0 ? true : false;
        }

        /// <summary>
        /// Set grid style.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void gvwList_RowCellStyle(object sender, RowCellStyleEventArgs e)
        {
            GridView view = sender as GridView;

            switch (e.Column.FieldName)
            {
                case "PART_NAME":

                    if (view.IsCellSelected(e.RowHandle, e.Column) == false)
                        e.Appearance.BackColor = Color.Linen;

                    break;

                case "PART_LOC_NAME_KO":

                    if (view.IsCellSelected(e.RowHandle, e.Column) == false)
                        e.Appearance.BackColor = Color.Azure;

                    break;

                case "PROCESS":

                    if (view.IsCellSelected(e.RowHandle, e.Column) == false)
                        e.Appearance.BackColor = Color.MistyRose;

                    break;
            }
        }
    }
}