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
    public partial class TransferRecord : DevExpress.XtraEditors.XtraForm
    {
        // Data from parent form.
        public object INHERITANCE;
        
        public TransferRecord()
        {
            InitializeComponent();
        }

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);

            PKG_INTG_BOM.SELECT_TRNS_RECORD pkgSelect = new PKG_INTG_BOM.SELECT_TRNS_RECORD();
            pkgSelect.ARG_WORK_TYPE = "";
            pkgSelect.ARG_FACTORY = ((string[])INHERITANCE)[0];
            pkgSelect.ARG_WS_NO = ((string[])INHERITANCE)[1];
            pkgSelect.OUT_CURSOR = string.Empty;

            DataTable dtResult = Common.projectBaseForm.Exe_Select_PKG(pkgSelect).Tables[0];

            if (dtResult.Rows.Count > 0)
                grdData.DataSource = dtResult;
        }

        private void gvwData_RowCellStyle(object sender, RowCellStyleEventArgs e)
        {
            GridView view = sender as GridView;

            if (e.Column.FieldName == "PROC_STATUS")
            {
                string procStatus = view.GetRowCellValue(e.RowHandle, "PROC_STATUS").ToString();

                if (view.IsCellSelected(e.RowHandle, e.Column) == false)
                {
                    switch (procStatus)
                    {
                        case "Request":
                            e.Appearance.BackColor = Color.FromArgb(41, 124, 167);
                            e.Appearance.ForeColor = Color.White;
                            e.Appearance.Font = new Font(e.Appearance.Font, FontStyle.Bold);
                            break;

                        case "Cancel":
                            e.Appearance.BackColor = Color.FromArgb(252, 105, 53);
                            e.Appearance.ForeColor = Color.White;
                            e.Appearance.Font = new Font(e.Appearance.Font, FontStyle.Bold);
                            break;

                        case "Not Yet":
                            e.Appearance.BackColor = Color.FromArgb(135, 199, 86);
                            e.Appearance.ForeColor = Color.White;
                            e.Appearance.Font = new Font(e.Appearance.Font, FontStyle.Bold);
                            break;

                        default:
                            break;
                    }
                }
            }
            else if (e.Column.FieldName == "PCX_SUPP_MAT_ID" || e.Column.FieldName == "PCX_MAT_ID" ||
                e.Column.FieldName == "MAT_NAME" || e.Column.FieldName == "MAT_COMMENT")
            {
                string pcxMatID = view.GetRowCellValue(e.RowHandle, "PCX_MAT_ID").ToString();

                if (view.IsCellSelected(e.RowHandle, e.Column) == false)
                {
                    switch (pcxMatID)
                    {
                        case "100":
                            e.Appearance.ForeColor = Color.Red;
                            e.Appearance.Font = new Font(e.Appearance.Font, FontStyle.Bold);
                            break;

                        case "175225":
                            e.Appearance.ForeColor = Color.Green;
                            e.Appearance.Font = new Font(e.Appearance.Font, FontStyle.Bold);
                            break;

                        default:
                            break;
                    }
                }
            }
        }
    }
}