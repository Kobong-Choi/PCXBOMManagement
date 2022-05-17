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
using CSI.PCC.PCX.Packages;                     // Package Class

namespace CSI.PCC.PCX
{
    public partial class PurchaseRecords : DevExpress.XtraEditors.XtraForm
    {
        public ProjectBaseForm projectBaseForm = Common.projectBaseForm;
        
        public string Factory { get; set; }
        public string WSNo { get; set; }
        public string PartSeq { get; set; }
        public string Delimiter { get; set; }

        public PurchaseRecords()
        {
            InitializeComponent();
        }

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);
            
            // Bind datasource to gridview.
            PKG_INTG_BOM.SELECT_MATERIAL_HISTORY pkgSelectHistory = new PKG_INTG_BOM.SELECT_MATERIAL_HISTORY();
            pkgSelectHistory.ARG_FACTORY = Factory;
            pkgSelectHistory.ARG_WS_NO = WSNo;
            pkgSelectHistory.ARG_PART_SEQ = PartSeq;
            pkgSelectHistory.ARG_DELIMITER = Delimiter;
            pkgSelectHistory.OUT_CURSOR = string.Empty;
            
            grdRecords.DataSource = projectBaseForm.Exe_Select_PKG(pkgSelectHistory).Tables[0];
        }

        /// <summary>
        /// Highlight the latest order.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void gvwRecords_RowCellStyle(object sender, RowCellStyleEventArgs e)
        {
            try
            {
                GridView view = sender as GridView;

                if (e.Column.FieldName == "PUR_STATUS")
                {
                    string purStatus = view.GetRowCellValue(e.RowHandle, "PUR_STATUS").ToString();

                    if (purStatus == "Cancel")
                    {
                        if (view.IsCellSelected(e.RowHandle, e.Column) == false)
                        {
                            e.Appearance.BackColor = Color.DarkGray;
                            e.Appearance.ForeColor = Color.White;
                        }
                    }
                    else if (purStatus == "Request")
                    {
                        if (view.IsCellSelected(e.RowHandle, e.Column) == false)
                            e.Appearance.BackColor = Color.Yellow;
                    }
                    else if (purStatus == "Purchase")
                    {
                        if (view.IsCellSelected(e.RowHandle, e.Column) == false)
                            e.Appearance.BackColor = Color.Cyan;
                    }
                    else if (purStatus == "Ongoing")    // 3P Only
                    {
                        if (view.IsCellSelected(e.RowHandle, e.Column) == false)
                            e.Appearance.BackColor = Color.Lime;
                    }
                    else if (purStatus == "Release")    // 3P Only
                    {
                        if (view.IsCellSelected(e.RowHandle, e.Column) == false)
                            e.Appearance.BackColor = Color.Coral;
                    }
                }
                else
                {
                    if (view.GetRowCellValue(e.RowHandle, "LATEST_YN").ToString().Equals("Y"))
                    {
                        if (view.IsCellSelected(e.RowHandle, e.Column) == false)
                        {
                            e.Appearance.BackColor = Color.SkyBlue;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
                return;
            }
        }
    }
}