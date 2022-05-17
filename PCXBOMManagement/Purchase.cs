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
using DevExpress.XtraGrid.Columns;
using DevExpress.Utils;                         // DXMouseEventArgs
using DevExpress.XtraEditors;                   // GridLookUpEdit

using CSI.Client.ProjectBaseForm;               // ProjectBaseForm Class
using CSI.PCC.PCX.Packages;                     // Package Class

namespace CSI.PCC.PCX
{
    public partial class Purchase : DevExpress.XtraEditors.XtraForm
    {
        public string Factory { get; set; }
        public string WorksheetNumbers { get; set; }
        public string EditType { get; set; }
        public string CSBOMStatus { get; set; }
        //public bool HasLockedBOM { get; set; }
        public int ParentRowhandle { get; set; }

        private static GridView ActiveViewBOM = null;
        private static GridView ActiveViewPMC = null;
        private static GridView ActiveView3P = null;

        private static GridControl ActiveControlBOM = null;
        private static GridControl ActiveControlPMC = null;
        private static GridControl ActiveControl3P = null;

        private static Font font = new Font("Tahoma", 9, FontStyle.Bold);

        private static List<string> SetOfMaterialFields = new List<string>() {
            "MXSXL_NUMBER", "PCX_SUPP_MAT_ID", "MCS_NUMBER", "PCX_MAT_ID",
            "MAT_CD", "MAT_NAME", "ENCODED_CMT", "VENDOR_NAME" };

        private static List<string> SetOfColorFields = new List<string>() {
            "PCX_COLOR_ID", "COLOR_CD", "COLOR_NAME" };

        private static List<string> SetOfPartFields = new List<string>() {
            "PART_NO", "PART_NAME", "PART_TYPE"};

        private static List<string> RequiredFields = new List<string>() {
            "PART_NAME", "PART_TYPE", "MAT_NAME", "COLOR_NAME" };

        public Purchase()
        {
            InitializeComponent();
        }

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);

            // Get each active gridcontrol.
            ActiveControlBOM = (this.EditType == "Single") ? grdBomSingle : grdBomMultiple;
            ActiveControlPMC = (this.EditType == "Single") ? grdPMCSingle : grdPMCMultiple;
            ActiveControl3P = (this.EditType == "Single") ? grd3PSingle : grd3PMultiple;

            // Get each active gridview.
            ActiveViewBOM = (this.EditType == "Single") ? gvwBomSingle : gvwBomMultiple;
            ActiveViewPMC = (this.EditType == "Single") ? gvwPMCSingle : gvwPMCMultiple;
            ActiveView3P = (this.EditType == "Single") ? gvw3PSingle : gvw3PMultiple;

            // Activate gridview.
            switch (this.EditType)
            {
                case "Single":

                    grdBomSingle.Dock = DockStyle.Fill;
                    grdPMCSingle.Dock = DockStyle.Fill;
                    grd3PSingle.Dock = DockStyle.Fill;

                    grdBomMultiple.Visible = false;
                    grdPMCMultiple.Visible = false;
                    grd3PMultiple.Visible = false;

                    break;

                case "Multiple":

                    grdBomMultiple.Dock = DockStyle.Fill;
                    grdPMCMultiple.Dock = DockStyle.Fill;
                    grd3PMultiple.Dock = DockStyle.Fill;

                    grdBomSingle.Visible = false;
                    grdPMCSingle.Visible = false;
                    grd3PSingle.Visible = false;

                    break;
            }

            // Write caption on the form.
            if (this.EditType == "Single")
            {
                PKG_INTG_BOM.SELECT_BOM_INFO_BY_CONFIRM pkgSelect = new PKG_INTG_BOM.SELECT_BOM_INFO_BY_CONFIRM();
                pkgSelect.ARG_WORK_TYPE = "BOMCaption";
                pkgSelect.ARG_FACTORY = this.Factory;
                pkgSelect.ARG_WS_NO = this.WorksheetNumbers;
                pkgSelect.ARG_PART_SEQ = "";
                pkgSelect.OUT_CURSOR = string.Empty;

                DataTable dataSource = Common.projectBaseForm.Exe_Select_PKG(pkgSelect).Tables[0];

                if (dataSource != null)
                {
                    string caption = string.Format("{0} / {1} / {2} / {3} / {4} / {5} / Rows : {6}",
                        dataSource.Rows[0]["CATEGORY"].ToString(),
                        dataSource.Rows[0]["SEASON"].ToString(),
                        dataSource.Rows[0]["DEV_NAME"].ToString(),
                        dataSource.Rows[0]["SAMPLE_TYPE"].ToString(),
                        dataSource.Rows[0]["DEV_STYLE_NUMBER"].ToString(),
                        dataSource.Rows[0]["DEV_COLORWAY_ID"].ToString(),
                        dataSource.Rows[0]["RN"].ToString());

                    this.Text = caption;
                }
            }

            DataTable sourceOfBom = GetDataSourceOfBom();

            if (sourceOfBom.Rows.Count == 0)
            {
                Common.ShowMessageBox("There is nothing to request an order in the BOM.", "E");
                this.Close();
            }
            else if (sourceOfBom.Rows[0]["DEV_COLORWAY_ID"].ToString() == "")
            {
                Common.ShowMessageBox("Dev Colorway ID is required to request order.", "E");
                this.Close();
            }

            ActiveControlBOM.DataSource = sourceOfBom;
            ActiveControlPMC.DataSource = GetDataSourceOfPurchase("PMC");
            ActiveControl3P.DataSource = GetDataSourceOfPurchase("THREE_P");
        }

        #region Button Events

        /// <summary>
        /// Expand gridview of BOM.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnLeftExtend_Click(object sender, EventArgs e)
        {
            pnlControl1.Width = Convert.ToInt32(this.Size.Width * 0.9);
            pnlControl3.Width = Convert.ToInt32(this.Size.Width * 0.1);
        }

        /// <summary>
        /// Expand Gridview of Purchase.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnRightExtend_Click(object sender, EventArgs e)
        {
            pnlControl1.Width = Convert.ToInt32(this.Size.Width * 0.1);
            pnlControl3.Width = Convert.ToInt32(this.Size.Width * 0.9);
        }

        /// <summary>
        /// Expand gridview of PMC.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnPMCExpand_Click(object sender, EventArgs e)
        {
            groupControl2.Height = Convert.ToInt32(this.Size.Height * 0.7);
            groupControl3.Height = Convert.ToInt32(this.Size.Height * 0.3);
        }

        /// <summary>
        /// Expand gridview of 3P.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btn3PExpand_Click(object sender, EventArgs e)
        {
            groupControl2.Height = Convert.ToInt32(this.Size.Height * 0.1);
            groupControl3.Height = Convert.ToInt32(this.Size.Height * 0.9);
        }

        /// <summary>
        /// Create temporary order data.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnMove_Click(object sender, EventArgs e)
        {
            ArrayList list = new ArrayList();
            List<DataRow> listDataRow = new List<DataRow>();
            bool isCheckedPMC;
            bool isChecked3P;

            Func<GridControl, bool> haveAllRowSaved = (control) =>
            {
                if ((control.DataSource as DataTable).AsEnumerable().Where(
                    x => x["ROW_STATUS"].ToString().Equals("U")).Count() > 0)
                {
                    Common.ShowMessageBox("Please save first.", "W");
                    return false;
                }
                else
                    return true;
            };

            // Validate that changed data was saved.
            if (!haveAllRowSaved(ActiveControlPMC)) return;
            if (!haveAllRowSaved(ActiveControl3P)) return;

            // Make purchase check status to 'Y' to create temporary order data.
            for (int rowHandle = 0; rowHandle < ActiveViewBOM.RowCount; rowHandle++)
            {
                // Get each check status.
                isCheckedPMC = ActiveViewBOM.GetRowCellValue(rowHandle, "PMC_CHK").ToString().Equals("Y");
                isChecked3P = ActiveViewBOM.GetRowCellValue(rowHandle, "THREE_P_CHK").ToString().Equals("Y");

                // There are no materials that can be ordered to two locations at the same time.
                if (isCheckedPMC && isChecked3P)
                {
                    Common.ShowMessageBox("Please select only one place to order.", "E");
                    Common.FocusCell(ActiveViewBOM, rowHandle, "PART_NAME", false);
                    return;
                }
                else if (isCheckedPMC != isChecked3P)
                {
                    // In case of 3P, Skip overseas factory .
                    if (isChecked3P && !ActiveViewBOM.GetRowCellValue(rowHandle, "FACTORY").ToString().Equals("DS"))
                        continue;

                    PKG_INTG_BOM_PURCHASE.UPDATE_PUR_CHK_TO_Y pkgUpdate = new PKG_INTG_BOM_PURCHASE.UPDATE_PUR_CHK_TO_Y();
                    pkgUpdate.ARG_FACTORY = ActiveViewBOM.GetRowCellValue(rowHandle, "FACTORY").ToString();
                    pkgUpdate.ARG_WS_NO = ActiveViewBOM.GetRowCellValue(rowHandle, "WS_NO").ToString();
                    pkgUpdate.ARG_PART_SEQ = ActiveViewBOM.GetRowCellValue(rowHandle, "PART_SEQ").ToString();
                    pkgUpdate.ARG_TYPE = isCheckedPMC ? "PMC" : "THREE_P";

                    list.Add(pkgUpdate);
                }
            }

            if (list.Count > 0)
            {
                if (Common.projectBaseForm.Exe_Modify_PKG(list) == null)
                    return;
            }

            // Create temporary order data.
            PKG_INTG_BOM_PURCHASE.ADD_TO_SHOP_BASKET pkgInsertReq = new PKG_INTG_BOM_PURCHASE.ADD_TO_SHOP_BASKET();
            pkgInsertReq.ARG_FACTORY = this.Factory;
            pkgInsertReq.ARG_CONCAT_WS_NO = this.WorksheetNumbers;
            pkgInsertReq.ARG_DEVELOPER = Common.sessionID;

            list.Clear();
            list.Add(pkgInsertReq);

            if (Common.projectBaseForm.Exe_Modify_PKG(list) == null)
            {
                Common.ShowMessageBox("Failed to create temporary order data.", "E");
                return;
            }

            // Change checked status from 'Y' to 'N'.
            listDataRow = (ActiveControlBOM.DataSource as DataTable).AsEnumerable().Where(
                x => x["PMC_CHK"].ToString().Equals("Y") || x["THREE_P_CHK"].ToString().Equals("Y")).ToList();

            foreach (DataRow row in listDataRow)
            {
                if (row["PMC_CHK"].ToString().Equals("Y"))
                    row["PMC_CHK"] = "N";
                else
                    row["THREE_P_CHK"] = "N";
            }

            // Make purchase check status in the database to 'N'.
            PKG_INTG_BOM_PURCHASE.UPDATE_PUR_CHK_TO_N pkgUpdatePurChk_N = new PKG_INTG_BOM_PURCHASE.UPDATE_PUR_CHK_TO_N();
            pkgUpdatePurChk_N.ARG_FACTORY = this.Factory;
            pkgUpdatePurChk_N.ARG_CONCAT_WS_NO = this.WorksheetNumbers;

            list.Clear();
            list.Add(pkgUpdatePurChk_N);

            if (Common.projectBaseForm.Exe_Modify_PKG(list) == null)
            {
                Common.ShowMessageBox("Failed to return purchase check status.", "E");
                return;
            }

            // Re-bind datasource into each gridview.
            ActiveControlPMC.DataSource = GetDataSourceOfPurchase("PMC");
            ActiveControl3P.DataSource = GetDataSourceOfPurchase("THREE_P");
        }

        /// <summary>
        /// Clear rows on the gridview of PMC.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnPmcEmpty_Click(object sender, EventArgs e)
        {
            ClearRowsOnTheGridView(ActiveViewPMC, ActiveControlPMC, "PMC");
        }

        /// <summary>
        /// Clear rows on the gridview of 3P.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btn3PEmpty_Click(object sender, EventArgs e)
        {
            ClearRowsOnTheGridView(ActiveView3P, ActiveControl3P, "THREE_P");
        }

        /// <summary>
        /// Create manual order data of PMC.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnPmcAdd_Click(object sender, EventArgs e)
        {
            AddNewRowToPurchase(ActiveControlPMC);
        }

        /// <summary>
        /// Create manual order data of 3P.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btn3PAdd_Click(object sender, EventArgs e)
        {
            AddNewRowToPurchase(ActiveControl3P);
        }

        /// <summary>
        /// Update temporary order data before confirming it.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnSave_Click(object sender, EventArgs e)
        {
            ArrayList arrList = new ArrayList();

            // Requried Validation.
            if (!IsRowExisting()) return;
            if (!AreAllEnrolled()) return;

            // Add an item to the list to update database.
            Action<GridView> action = (view) =>
            {
                for (int rowHandle = 0; rowHandle < view.RowCount; rowHandle++)
                {
                    PKG_INTG_BOM_PURCHASE.UPDATE_BOM pkgUpdate = new PKG_INTG_BOM_PURCHASE.UPDATE_BOM();
                    pkgUpdate.ARG_WORK_TYPE = "Tmpr";
                    pkgUpdate.ARG_FACTORY = view.GetRowCellValue(rowHandle, "FACTORY").ToString();
                    pkgUpdate.ARG_WS_NO = view.GetRowCellValue(rowHandle, "WS_NO").ToString();
                    pkgUpdate.ARG_PART_SEQ = view.GetRowCellValue(rowHandle, "PART_SEQ").ToString();
                    pkgUpdate.ARG_PART_NAME = view.GetRowCellValue(rowHandle, "PART_NAME").ToString();
                    pkgUpdate.ARG_PART_TYPE = view.GetRowCellValue(rowHandle, "PART_TYPE").ToString();
                    pkgUpdate.ARG_MXSXL_NUMBER = view.GetRowCellValue(rowHandle, "MXSXL_NUMBER").ToString();
                    pkgUpdate.ARG_PCX_SUPP_MAT_ID = view.GetRowCellValue(rowHandle, "PCX_SUPP_MAT_ID").ToString();
                    pkgUpdate.ARG_MAT_CD = view.GetRowCellValue(rowHandle, "MAT_CD").ToString();
                    pkgUpdate.ARG_MAT_NAME = view.GetRowCellValue(rowHandle, "MAT_NAME").ToString();
                    pkgUpdate.ARG_MAT_COMMENT = view.GetRowCellValue(rowHandle, "MAT_COMMENT").ToString();
                    pkgUpdate.ARG_PCX_COLOR_ID = view.GetRowCellValue(rowHandle, "PCX_COLOR_ID").ToString();
                    pkgUpdate.ARG_COLOR_CD = view.GetRowCellValue(rowHandle, "COLOR_CD").ToString();
                    pkgUpdate.ARG_COLOR_NAME = view.GetRowCellValue(rowHandle, "COLOR_NAME").ToString();
                    pkgUpdate.ARG_COLOR_COMMENT = view.GetRowCellValue(rowHandle, "COLOR_COMMENT").ToString();
                    pkgUpdate.ARG_PCX_MAT_ID = view.GetRowCellValue(rowHandle, "PCX_MAT_ID").ToString();
                    //pkgUpdate.ARG_CS_CD = view.GetRowCellValue(rowHandle, "CS_CD").ToString();
                    pkgUpdate.ARG_CS_CD = "";
                    pkgUpdate.ARG_VENDOR_NAME = view.GetRowCellValue(rowHandle, "VENDOR_NAME").ToString();
                    pkgUpdate.ARG_UPD_USER = Common.sessionID;
                    pkgUpdate.ARG_ENCODED_CMT = view.GetRowCellValue(rowHandle, "ENCODED_CMT").ToString();

                    arrList.Add(pkgUpdate);
                }
            };

            action(ActiveViewPMC);
            action(ActiveView3P);

            if (Common.projectBaseForm.Exe_Modify_PKG(arrList) == null)
            {
                Common.ShowMessageBox("Failed to save.", "E");
                return;
            }

            // Re-bind dataSource to gridView.
            ActiveControlPMC.DataSource = GetDataSourceOfPurchase("PMC");
            ActiveControl3P.DataSource = GetDataSourceOfPurchase("THREE_P");

            MessageBox.Show("Complete.");
        }

        /// <summary>
        /// Request purchase order 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnPurchase_Click(object sender, EventArgs e)
        {
            ArrayList list = new ArrayList();

            // Essential Validation.
            if (!IsRowExisting()) return;
            if (!AreAllEnrolled()) return;
            if (!AreAllSaved()) return;
            if (!ValidateFields()) return;

            // Validate the color of PCX material '62499' or '62496'.
            DataTable dt = ActiveControlPMC.DataSource as DataTable;

            if (dt.AsEnumerable().Where(x => (x["PCX_MAT_ID"].ToString().Equals("62499") || x["PCX_MAT_ID"].ToString().Equals("62496"))
                   && x["COLOR_CD"].ToString().Equals("10A")).Count() > 0)
            {
                Common.ShowMessageBox("Please change the color of PCX material '62499' or '62496'\nto '99J'.", "W");
                return;
            }

            // Apply the changed data to the BOM.
            Action<GridView> action = (view) =>
            {
                for (int rowHandle = 0; rowHandle < view.RowCount; rowHandle++)
                {
                    if (view.GetRowCellValue(rowHandle, "MANUAL_ADD").ToString().Equals("Y"))
                        continue;

                    PKG_INTG_BOM_PURCHASE.UPDATE_BOM pkgUpdate = new PKG_INTG_BOM_PURCHASE.UPDATE_BOM();
                    pkgUpdate.ARG_WORK_TYPE = "BOM";
                    pkgUpdate.ARG_FACTORY = view.GetRowCellValue(rowHandle, "FACTORY").ToString();
                    pkgUpdate.ARG_WS_NO = view.GetRowCellValue(rowHandle, "WS_NO").ToString();
                    pkgUpdate.ARG_PART_SEQ = view.GetRowCellValue(rowHandle, "PART_SEQ").ToString();
                    pkgUpdate.ARG_PART_NAME = "";
                    pkgUpdate.ARG_PART_TYPE = "";
                    pkgUpdate.ARG_MXSXL_NUMBER = view.GetRowCellValue(rowHandle, "MXSXL_NUMBER").ToString();
                    pkgUpdate.ARG_PCX_SUPP_MAT_ID = view.GetRowCellValue(rowHandle, "PCX_SUPP_MAT_ID").ToString();
                    pkgUpdate.ARG_MAT_CD = view.GetRowCellValue(rowHandle, "MAT_CD").ToString();
                    pkgUpdate.ARG_MAT_NAME = view.GetRowCellValue(rowHandle, "MAT_NAME").ToString();
                    pkgUpdate.ARG_MAT_COMMENT = view.GetRowCellValue(rowHandle, "MAT_COMMENT").ToString();
                    pkgUpdate.ARG_PCX_COLOR_ID = view.GetRowCellValue(rowHandle, "PCX_COLOR_ID").ToString();
                    pkgUpdate.ARG_COLOR_CD = view.GetRowCellValue(rowHandle, "COLOR_CD").ToString();
                    pkgUpdate.ARG_COLOR_NAME = view.GetRowCellValue(rowHandle, "COLOR_NAME").ToString();
                    pkgUpdate.ARG_COLOR_COMMENT = view.GetRowCellValue(rowHandle, "COLOR_COMMENT").ToString();
                    pkgUpdate.ARG_PCX_MAT_ID = view.GetRowCellValue(rowHandle, "PCX_MAT_ID").ToString();
                    //pkgUpdate.ARG_CS_CD = view.GetRowCellValue(rowHandle, "CS_CD").ToString();
                    pkgUpdate.ARG_CS_CD = "";
                    pkgUpdate.ARG_VENDOR_NAME = view.GetRowCellValue(rowHandle, "VENDOR_NAME").ToString();
                    pkgUpdate.ARG_UPD_USER = Common.sessionID;
                    pkgUpdate.ARG_ENCODED_CMT = view.GetRowCellValue(rowHandle, "ENCODED_CMT").ToString();

                    list.Add(pkgUpdate);
                }
            };

            action(ActiveViewPMC);
            action(ActiveView3P);

            if (Common.projectBaseForm.Exe_Modify_PKG(list) == null)
            {
                MessageBox.Show("Failed to match changed data with this BOM.");
                return;
            }

            // Purchase orders.
            PKG_INTG_BOM_PURCHASE.REQUEST_PURCHASE_ORDER pkgInsert = new PKG_INTG_BOM_PURCHASE.REQUEST_PURCHASE_ORDER();
            pkgInsert.ARG_FACTORY = this.Factory;
            pkgInsert.ARG_CONCAT_WS_NO = this.WorksheetNumbers;
            pkgInsert.ARG_PUR_USER = Common.sessionID;

            list.Clear();
            list.Add(pkgInsert);

            if (Common.projectBaseForm.Exe_Modify_PKG(list) == null)
            {
                Common.ShowMessageBox("Failed to purchase.", "E");
                return;
            }

            // Clear all rows of each gridview.
            (ActiveControlPMC.DataSource as DataTable).Rows.Clear();
            (ActiveControl3P.DataSource as DataTable).Rows.Clear();

            ActiveControlBOM.DataSource = GetDataSourceOfBom();

            MessageBox.Show("Complete.");
        }

        #endregion

        #region Context Menus Events

        /// <summary>
        /// Context Menu Event 처리
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ContextMenuItem_Click(object sender, EventArgs e)
        {
            // 선택된 메뉴 아이템
            System.Windows.Forms.ToolStripMenuItem menuItem = sender as System.Windows.Forms.ToolStripMenuItem;

            switch (menuItem.Name)
            {
                #region BOM Single

                case "UprAllChk_Single":
                    TickAllBox(gvwBomSingle, "PMC_CHK");
                    break;

                case "BtmAllChk_Single":
                    TickAllBox(gvwBomSingle, "THREE_P_CHK");
                    break;

                case "UprMulChk_Single":
                    TickSelectedBox(gvwBomSingle, "PMC_CHK");
                    break;

                case "BtmMulChk_Single":
                    TickSelectedBox(gvwBomSingle, "THREE_P_CHK");
                    break;

                #endregion

                #region BOM Multiple

                case "UprAllChk_Multiple":
                    TickAllBox(gvwBomMultiple, "PMC_CHK");
                    break;

                case "BtmAllChk_Multiple":
                    TickAllBox(gvwBomMultiple, "THREE_P_CHK");
                    break;

                case "UprMulChk_Multiple":
                    TickSelectedBox(gvwBomMultiple, "PMC_CHK");
                    break;

                case "BtmMulChk_Multiple":
                    TickSelectedBox(gvwBomMultiple, "THREE_P_CHK");
                    break;

                #endregion

                #region Find Code

                case "FindCode_PMC_Single":
                    FindCodeFromLibrary(gvwPMCSingle);
                    break;

                case "FindCode_PMC_Multiple":
                    FindCodeFromLibrary(gvwPMCMultiple);
                    break;

                case "FindCode_3P_Single":
                    FindCodeFromLibrary(gvw3PSingle);
                    break;

                case "FindCode_3P_Multiple":
                    FindCodeFromLibrary(gvw3PMultiple);
                    break;

                #endregion

                #region Generate Comment

                case "genCmt_Single":
                    ShowCommentForm(gvwPMCSingle);
                    break;

                case "genCmt_Multiple":
                    ShowCommentForm(gvwPMCMultiple);
                    break;

                case "genCmt3P_Single":
                    ShowCommentForm(gvw3PSingle);
                    break;

                case "genCmt3P_Multiple":
                    ShowCommentForm(gvw3PMultiple);
                    break;

                #endregion

                #region Enroll

                case "Enroll_PMC_Single":
                    EnrollManuallyAddedMaterails(gvwPMCSingle, grdPMCSingle, "PMC");
                    break;

                case "Enroll_PMC_Multiple":
                    EnrollManuallyAddedMaterails(gvwPMCMultiple, grdPMCMultiple, "PMC");
                    break;

                case "Enroll_3P_Single":
                    EnrollManuallyAddedMaterails(gvw3PSingle, grd3PSingle, "THREE_P");
                    break;

                case "Enroll_3P_Multiple":
                    EnrollManuallyAddedMaterails(gvw3PMultiple, grd3PMultiple, "THREE_P");
                    break;

                #endregion

                #region Release

                case "Release_PMC_Single":
                    ReleaseManuallyAddedMaterials(gvwPMCSingle);
                    break;

                case "Release_PMC_Multiple":
                    ReleaseManuallyAddedMaterials(gvwPMCMultiple);
                    break;

                case "Release_3P_Single":
                    ReleaseManuallyAddedMaterials(gvw3PSingle);
                    break;

                case "Release_3P_Multiple":
                    ReleaseManuallyAddedMaterials(gvw3PMultiple);
                    break;

                #endregion

                #region Delete

                case "DeleteRow_PMC_Single":
                    DeleteMaterialsToPurchase(gvwPMCSingle, grdPMCSingle);
                    break;

                case "DeleteRow_PMC_Multiple":
                    DeleteMaterialsToPurchase(gvwPMCMultiple, grdPMCMultiple);
                    break;

                case "DeleteRow_3P_Single":
                    DeleteMaterialsToPurchase(gvw3PSingle, grd3PSingle);
                    break;

                case "DeleteRow_3P_Multiple":
                    DeleteMaterialsToPurchase(gvw3PMultiple, grd3PMultiple);
                    break;

                #endregion

                default:
                    break;
            }
        }

        /// <summary>
        /// Tick all of the boxes for the tick type.
        /// </summary>
        /// <param name="view"></param>
        /// <param name="tickType"></param>
        private void TickAllBox(GridView view, string tickType)
        {
            for (int rowHandle = 0; rowHandle < view.RowCount; rowHandle++)
            {
                if (view.GetRowCellValue(rowHandle, "MANUAL_ADD").ToString().Equals("Y"))
                    continue;

                view.SetRowCellValue(rowHandle, tickType,
                    (view.GetRowCellValue(rowHandle, tickType).ToString() == "Y") ? "N" : "Y");
            }
        }

        /// <summary>
        /// Tick selected boxes for the tick type.
        /// </summary>
        /// <param name="view"></param>
        /// <param name="tickType"></param>
        private void TickSelectedBox(GridView view, string tickType)
        {
            foreach (int rowHandle in view.GetSelectedRows())
            {
                if (view.GetRowCellValue(rowHandle, "MANUAL_ADD").ToString().Equals("Y"))
                    continue;

                view.SetRowCellValue(rowHandle, tickType,
                    (view.GetRowCellValue(rowHandle, tickType).ToString() == "Y") ? "N" : "Y");
            }
        }

        /// <summary>
        /// PCX 라이브러리에서 코드를 찾아 선택한 행에 입력
        /// </summary>
        /// <param name="_type"></param>
        private void FindCodeFromLibrary(GridView view)
        {
            string keyword = view.GetFocusedRowCellValue(view.FocusedColumn).ToString();
            int initSearchType = 0;

            switch (view.FocusedColumn.FieldName)
            {
                case "PART_NAME":
                case "PART_TYPE":

                    initSearchType = 0;
                    break;

                case "MXSXL_NUMBER":
                case "MCS_NUMBER":
                case "PCX_SUPP_MAT_ID":
                case "PCX_MAT_ID":
                case "MAT_CD":
                case "MAT_NAME":
                case "MAT_COMMENT":
                case "VENDOR_NAME":

                    // First of all PCX,
                    initSearchType = 1;
                    break;

                case "PCX_COLOR_ID":
                case "COLOR_CD":
                case "COLOR_NAME":

                    initSearchType = 2;
                    break;
            }

            object[] parameters = new object[] { initSearchType, keyword, "" };

            FindCode form = new FindCode(parameters);

            if (form.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                if (form.FORM_RESULT == null) return;

                string[] result = (string[])form.FORM_RESULT;

                try
                {
                    view.CellValueChanged -= new CellValueChangedEventHandler(CustomCellValueChanged);

                    foreach (int rowHandle in view.GetSelectedRows())
                    {
                        if (view.GetRowCellValue(rowHandle, "COLOR_VER").ToString().Equals("Enrolled") ||
                            view.GetRowCellValue(rowHandle, "LOCK_YN").ToString().Equals("Y"))
                            continue;

                        if (result[0] == "Part")
                        {
                            view.SetRowCellValue(rowHandle, "PART_NAME", result[1]);
                            view.SetRowCellValue(rowHandle, "PART_TYPE", result[2]);
                        }
                        else if (result[0] == "PCX_Material")
                        {
                            view.SetRowCellValue(rowHandle, "PCX_MAT_ID", result[1]);
                            view.SetRowCellValue(rowHandle, "MAT_CD", result[2]);
                            view.SetRowCellValue(rowHandle, "MAT_NAME", result[3]);
                            view.SetRowCellValue(rowHandle, "MXSXL_NUMBER", result[4]);
                            view.SetRowCellValue(rowHandle, "MCS_NUMBER", result[5]);
                            view.SetRowCellValue(rowHandle, "VENDOR_NAME", result[6]);
                            //view.SetRowCellValue(rowHandle, "CS_CD", result[7]);
                            view.SetRowCellValue(rowHandle, "PCX_SUPP_MAT_ID", result[8]);
                        }
                        else if (result[0] == "Color")
                        {
                            view.SetRowCellValue(rowHandle, "PCX_COLOR_ID", result[1]);
                            view.SetRowCellValue(rowHandle, "COLOR_CD", result[2]);
                            view.SetRowCellValue(rowHandle, "COLOR_NAME", result[3]);
                        }

                        #region Close PCC Material

                        //else if (result[0] == "PCC_Material")
                        //{
                        //    view.SetRowCellValue(rowHandle, "PCX_MAT_ID", result[1]);
                        //    view.SetRowCellValue(rowHandle, "MAT_CD", result[2]);
                        //    view.SetRowCellValue(rowHandle, "MAT_NAME", result[3]);
                        //    view.SetRowCellValue(rowHandle, "MXSXL_NUMBER", result[4]);
                        //    view.SetRowCellValue(rowHandle, "MCS_NUMBER", result[5]);
                        //    view.SetRowCellValue(rowHandle, "VENDOR_NAME", result[6]);
                        //    view.SetRowCellValue(rowHandle, "CS_CD", result[7]);
                        //    view.SetRowCellValue(rowHandle, "PCX_SUPP_MAT_ID", result[8]);
                        //}
                        //else if (result[0] == "CS_Material")
                        //{
                        //    view.SetRowCellValue(rowHandle, "MXSXL_NUMBER", result[1]);
                        //    view.SetRowCellValue(rowHandle, "MAT_CD", result[2]);
                        //    view.SetRowCellValue(rowHandle, "MAT_NAME", result[3]);
                        //    view.SetRowCellValue(rowHandle, "MCS_NUMBER", "");
                        //    view.SetRowCellValue(rowHandle, "CS_CD", "CS");
                        //    view.SetRowCellValue(rowHandle, "PCX_SUPP_MAT_ID", "100");
                        //    view.SetRowCellValue(rowHandle, "PCX_MAT_ID", "100");
                        //}

                        #endregion

                        view.SetRowCellValue(rowHandle, "ROW_STATUS", "U");
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.ToString());
                    return;
                }
                finally
                {
                    // 이벤트 다시 연결
                    view.CellValueChanged += new CellValueChangedEventHandler(CustomCellValueChanged);
                }
            }
        }

        /// <summary>
        /// 개별 요청 등록한 자재를 PCC_PUR_REQ_TMPR 테이블에 삽입
        /// </summary>
        /// <param name="_type"></param>
        private void EnrollManuallyAddedMaterails(GridView view, GridControl control, string location)
        {
            int[] rowHandles = view.GetSelectedRows();
            List<string> requriedFieldNames = new List<string>() { "PART_NAME", "PART_TYPE", "MAT_NAME", "COLOR_NAME" };

            foreach (int rowHandle in rowHandles)
            {
                // Validate that it is the type of row that can be enrolled.
                if (view.GetRowCellValue(rowHandle, "COLOR_VER").ToString() != "Manual")
                {
                    MessageBox.Show("A row that can't be enrolled was included in the selection.", "",
                        MessageBoxButtons.OK, MessageBoxIcon.Error, MessageBoxDefaultButton.Button1);

                    Common.FocusCell(view, rowHandle, view.FocusedColumn.FieldName, true);
                    return;
                }

                // Validate required fields have been input.
                Func<string, bool> func = (fieldName) =>
                {
                    if (view.GetRowCellValue(rowHandle, fieldName).ToString().Equals(""))
                    {
                        MessageBox.Show(fieldName + " is required.", "",
                            MessageBoxButtons.OK, MessageBoxIcon.Error, MessageBoxDefaultButton.Button1);

                        FocusOnSelectedCell(rowHandle, fieldName, view);
                        view.ShowEditor();
                        return false;
                    }

                    return true;
                };

                foreach (string fieldName in requriedFieldNames)
                {
                    if (func(fieldName) == false) return;
                }
            }

            foreach (int rowHandle in rowHandles)
            {
                PKG_INTG_BOM_PURCHASE.CALCULATE_NEXT_PART_SEQ pkgSelectPartSeq = new PKG_INTG_BOM_PURCHASE.CALCULATE_NEXT_PART_SEQ();
                pkgSelectPartSeq.ARG_FACTORY = view.GetRowCellValue(rowHandle, "FACTORY").ToString();
                pkgSelectPartSeq.ARG_WS_NO = view.GetRowCellValue(rowHandle, "WS_NO").ToString();
                pkgSelectPartSeq.OUT_CURSOR = string.Empty;

                DataTable result = Common.projectBaseForm.Exe_Select_PKG(pkgSelectPartSeq).Tables[0];
                string partSeq = result.Rows[0]["PART_SEQ"].ToString();

                ArrayList arrayList = new ArrayList();

                PKG_INTG_BOM_PURCHASE.ENROLL_MANUAL_MATERIALS pkgInsert = new PKG_INTG_BOM_PURCHASE.ENROLL_MANUAL_MATERIALS();
                pkgInsert.ARG_FACTORY = view.GetRowCellValue(rowHandle, "FACTORY").ToString();
                pkgInsert.ARG_WS_NO = view.GetRowCellValue(rowHandle, "WS_NO").ToString();
                pkgInsert.ARG_PART_SEQ = partSeq;
                pkgInsert.ARG_PART_NAME = view.GetRowCellValue(rowHandle, "PART_NAME").ToString();
                pkgInsert.ARG_PART_TYPE = view.GetRowCellValue(rowHandle, "PART_TYPE").ToString();
                pkgInsert.ARG_MXSXL_NUMBER = view.GetRowCellValue(rowHandle, "MXSXL_NUMBER").ToString();
                pkgInsert.ARG_PCX_SUPP_MAT_ID = view.GetRowCellValue(rowHandle, "PCX_SUPP_MAT_ID").ToString();
                pkgInsert.ARG_MAT_CD = view.GetRowCellValue(rowHandle, "MAT_CD").ToString();
                pkgInsert.ARG_MAT_NAME = view.GetRowCellValue(rowHandle, "MAT_NAME").ToString();
                pkgInsert.ARG_MAT_COMMENT = view.GetRowCellValue(rowHandle, "MAT_COMMENT").ToString();
                pkgInsert.ARG_PCX_COLOR_ID = view.GetRowCellValue(rowHandle, "PCX_COLOR_ID").ToString();
                pkgInsert.ARG_COLOR_CD = view.GetRowCellValue(rowHandle, "COLOR_CD").ToString();
                pkgInsert.ARG_COLOR_NAME = view.GetRowCellValue(rowHandle, "COLOR_NAME").ToString();
                pkgInsert.ARG_OWNER = Common.sessionID;
                pkgInsert.ARG_LOCATION = location;
                pkgInsert.ARG_COLOR_COMMENT = view.GetRowCellValue(rowHandle, "COLOR_COMMENT").ToString();
                pkgInsert.ARG_PCX_MAT_ID = view.GetRowCellValue(rowHandle, "PCX_MAT_ID").ToString();
                pkgInsert.ARG_VENDOR_NAME = view.GetRowCellValue(rowHandle, "VENDOR_NAME").ToString();
                //pkgInsert.ARG_CS_CD = view.GetRowCellValue(rowHandle, "CS_CD").ToString();
                pkgInsert.ARG_CS_CD = "";
                pkgInsert.ARG_ENCODED_CMT = view.GetRowCellValue(rowHandle, "ENCODED_CMT").ToString();

                arrayList.Add(pkgInsert);

                // Call line by line to create a new part sequence in real time.
                if (Common.projectBaseForm.Exe_Modify_PKG(arrayList) == null)
                {
                    MessageBox.Show("Failed to enroll.", "",
                        MessageBoxButtons.OK, MessageBoxIcon.Error, MessageBoxDefaultButton.Button1);

                    return;
                }

                view.CellValueChanged -= new CellValueChangedEventHandler(CustomCellValueChanged);

                view.SetRowCellValue(rowHandle, "PART_SEQ", partSeq);
                view.SetRowCellValue(rowHandle, "COLOR_VER", "Enrolled");
                view.SetRowCellValue(rowHandle, "ROW_STATUS", "N");

                view.CellValueChanged += new CellValueChangedEventHandler(CustomCellValueChanged);
            }

            view.RefreshData();
        }

        /// <summary>
        /// 이미 등록한 개별 요청 자재의 수정을 위해 릴리즈
        /// </summary>
        /// <param name="view"></param>
        /// <param name="_control"></param>
        /// <param name="_location"></param>
        private void ReleaseManuallyAddedMaterials(GridView view)
        {
            try
            {
                int[] rowHandles = view.GetSelectedRows();

                foreach (int rowHandle in rowHandles)
                {
                    // 개별 요청 자재인지 확인
                    if (view.GetRowCellValue(rowHandle, "MANUAL_ADD").ToString() != "Y")
                    {
                        MessageBox.Show("Only manually added materials can be released.");
                        FocusOnSelectedCell(rowHandle, view.FocusedColumn.FieldName, view);
                        return;
                    }

                    // 이미 등록되었는지 확인
                    if (view.GetRowCellValue(rowHandle, "COLOR_VER").ToString() == "Manual")
                    {
                        MessageBox.Show("There are already released materials.");
                        FocusOnSelectedCell(rowHandle, view.FocusedColumn.FieldName, view);
                        return;
                    }
                }

                ArrayList arrayList = new ArrayList();

                foreach (int rowHandle in rowHandles)
                {
                    PKG_INTG_BOM_PURCHASE.RELEASE_MANUAL_MATERIALS pkgDelete = new PKG_INTG_BOM_PURCHASE.RELEASE_MANUAL_MATERIALS();
                    pkgDelete.ARG_FACTORY = view.GetRowCellValue(rowHandle, "FACTORY").ToString();
                    pkgDelete.ARG_WS_NO = view.GetRowCellValue(rowHandle, "WS_NO").ToString();
                    pkgDelete.ARG_PART_SEQ = view.GetRowCellValue(rowHandle, "PART_SEQ").ToString();

                    arrayList.Add(pkgDelete);

                    view.CellValueChanged -= new CellValueChangedEventHandler(CustomCellValueChanged);
                    view.SetRowCellValue(rowHandle, "COLOR_VER", "Manual");
                    view.CellValueChanged += new CellValueChangedEventHandler(CustomCellValueChanged);
                }

                Common.projectBaseForm.Exe_Modify_PKG(arrayList);
                view.RefreshData();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
                return;
            }
        }

        /// <summary>
        /// 등록된 자재 중 선택한 자재를 삭제한다.
        /// </summary>
        private void DeleteMaterialsToPurchase(GridView view, GridControl control)
        {
            ArrayList list = new ArrayList();
            int numDeletedRows = 0;

            foreach (int rowHandle in view.GetSelectedRows())
            {
                PKG_INTG_BOM_PURCHASE.RELEASE_MANUAL_MATERIALS pkgDelete = new PKG_INTG_BOM_PURCHASE.RELEASE_MANUAL_MATERIALS();
                pkgDelete.ARG_FACTORY = view.GetRowCellValue(rowHandle - numDeletedRows, "FACTORY").ToString();
                pkgDelete.ARG_WS_NO = view.GetRowCellValue(rowHandle - numDeletedRows, "WS_NO").ToString();
                pkgDelete.ARG_PART_SEQ = view.GetRowCellValue(rowHandle - numDeletedRows, "PART_SEQ").ToString();

                list.Add(pkgDelete);

                view.DeleteRow(rowHandle - numDeletedRows);
                numDeletedRows++;
            }

            if (list.Count > 0)
            {
                if (Common.projectBaseForm.Exe_Modify_PKG(list) == null)
                {
                    Common.ShowMessageBox("Failed to delete.", "E");
                    return;
                }

                (control.DataSource as DataTable).AcceptChanges();
            }
        }

        /// <summary>
        /// 
        /// </summary>
        private void ShowCommentForm(GridView view)
        {
            PCXComment form = new PCXComment()
            {
                Mode = "EDIT",
                BaseForm = Common.projectBaseForm,
                EncodedComment = view.GetRowCellValue(view.FocusedRowHandle, "ENCODED_CMT").ToString(),
                Comment = string.Empty
            };

            if (form.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                try
                {
                    view.CellValueChanged -= new CellValueChangedEventHandler(CustomCellValueChanged);

                    foreach (int rowHandle in view.GetSelectedRows())
                    {
                        if (view.GetRowCellValue(rowHandle, "PCX_MAT_ID").ToString().Equals("100"))
                            view.SetRowCellValue(rowHandle, "MAT_NAME", form.Comment);
                        else
                            view.SetRowCellValue(rowHandle, "MAT_COMMENT", form.Comment);

                        view.SetRowCellValue(rowHandle, "ENCODED_CMT", form.EncodedComment);
                        view.SetRowCellValue(rowHandle, "ROW_STATUS", "U");
                    }
                }
                catch (Exception ex)
                {
                    Common.ShowMessageBox(ex.ToString(), "E");
                }
                finally
                {
                    view.CellValueChanged += new CellValueChangedEventHandler(CustomCellValueChanged);
                }
            }
        }

        #endregion

        #region  Grid Events

        /// <summary>
        /// 그리드 셀 스타일 설정
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void gvwBomSingle_RowCellStyle(object sender, RowCellStyleEventArgs e)
        {
            GridView view = sender as GridView;

            // 개별 요청 여부
            string isManual = view.GetRowCellValue(e.RowHandle, "MANUAL_ADD").ToString();

            // 개별 요청한 자재는 스타일 따로 적용
            if (isManual == "Y")
            {
                if (e.Column.FieldName != "PUR_STATUS")
                {
                    if (view.IsCellSelected(e.RowHandle, e.Column) == false)
                    {
                        e.Appearance.BackColor = Color.DarkSeaGreen;
                    }
                }
                else if (e.Column.FieldName == "PUR_STATUS")
                {
                    // 구매 상태 값에 따라 각각 스타일 적용
                    string statusFlag = e.CellValue.ToString();

                    if (statusFlag == "Request")
                    {
                        if (view.IsCellSelected(e.RowHandle, e.Column) == false)
                        {
                            e.Appearance.BackColor = Color.Yellow;
                        }
                    }
                    else if (statusFlag == "Purchase")
                    {
                        if (view.IsCellSelected(e.RowHandle, e.Column) == false)
                        {
                            e.Appearance.BackColor = Color.Cyan;
                        }
                    }
                    else if (statusFlag == "Cancel")
                    {
                        if (view.IsCellSelected(e.RowHandle, e.Column) == false)
                        {
                            e.Appearance.BackColor = Color.DarkGray;
                            e.Appearance.ForeColor = Color.White;
                        }
                    }
                }
            }
            else
            {
                if (e.Column.FieldName == "PUR_STATUS")
                {
                    // 구매 상태 값에 따라 각각 스타일 적용
                    string statusFlag = e.CellValue.ToString();

                    if (statusFlag == "Not Yet")
                    {
                        // IsCellSelected : Indicates whether the cell is selected. true is the cell is selected; otherwise, false. 
                        if (view.IsCellSelected(e.RowHandle, e.Column) == false)
                        {
                            e.Appearance.BackColor = Color.Salmon;
                        }
                    }
                    else if (statusFlag == "Request")
                    {
                        if (view.IsCellSelected(e.RowHandle, e.Column) == false)
                        {
                            e.Appearance.BackColor = Color.Yellow;
                        }
                    }
                    else if (statusFlag == "Purchase")
                    {
                        if (view.IsCellSelected(e.RowHandle, e.Column) == false)
                        {
                            e.Appearance.BackColor = Color.Cyan;
                        }
                    }
                    else if (statusFlag == "Cancel")
                    {
                        if (view.IsCellSelected(e.RowHandle, e.Column) == false)
                        {
                            e.Appearance.BackColor = Color.DarkGray;
                            e.Appearance.ForeColor = Color.White;
                        }
                    }
                    else if (statusFlag == "Release")
                    {
                        if (view.IsCellSelected(e.RowHandle, e.Column) == false)
                        {
                            e.Appearance.BackColor = Color.Coral;
                            e.Appearance.ForeColor = Color.White;
                        }
                    }
                    else if (statusFlag == "Ongoing")
                    {
                        if (view.IsCellSelected(e.RowHandle, e.Column) == false)
                        {
                            e.Appearance.BackColor = Color.Lime;
                            e.Appearance.ForeColor = Color.White;
                        }
                    }
                }
                else if ((e.Column.FieldName == "PCX_SUPP_MAT_ID") || (e.Column.FieldName == "PCX_MAT_ID"))
                {
                    string pcxCode = gvwBomSingle.GetRowCellValue(e.RowHandle, e.Column).ToString();

                    if (pcxCode == "100")
                    {
                        if (view.IsCellSelected(e.RowHandle, e.Column) == false)
                        {
                            e.Appearance.ForeColor = Color.Red;
                            e.Appearance.Font = new Font(e.Appearance.Font, FontStyle.Bold);
                        }
                    }
                }
                else if (e.Column.FieldName == "MAT_NAME")
                {
                    string matName = gvwBomSingle.GetRowCellValue(e.RowHandle, "MAT_NAME").ToString();
                    string pcxMaterialID = gvwBomSingle.GetRowCellValue(e.RowHandle, "PCX_MAT_ID").ToString();

                    if (matName == "PLACEHOLDER")
                    {
                        if (view.IsCellSelected(e.RowHandle, e.Column) == false)
                        {
                            e.Appearance.ForeColor = Color.Red;
                            e.Appearance.Font = new Font(e.Appearance.Font, FontStyle.Bold);
                        }
                    }
                    else if (pcxMaterialID == "78730" || pcxMaterialID == "78728" || pcxMaterialID == "79466")
                    {
                        /* <LAMINATION>
                         * Lamination  HA 700L - DS
                         * Lamination - DM-629-MD - QD, VJ
                         * Lamination - WSM-170 - JJ */

                        if (view.IsCellSelected(e.RowHandle, e.Column) == false)
                        {
                            e.Appearance.ForeColor = Color.LimeGreen;
                            e.Appearance.Font = new Font(e.Appearance.Font, FontStyle.Bold);
                        }
                    }
                    else if (pcxMaterialID == "54638" || pcxMaterialID == "79341")
                    {
                        /* <LOGO>
                         * GENERIC SCREENED_TRANSFER LOGO
                         * GENERIC SCREEN PRINT TRANSFER */

                        if (view.IsCellSelected(e.RowHandle, e.Column) == false)
                        {
                            e.Appearance.ForeColor = Color.DeepSkyBlue;
                            e.Appearance.Font = new Font(e.Appearance.Font, FontStyle.Bold);
                        }
                    }
                    else if (pcxMaterialID == "87031" || pcxMaterialID == "78733" || pcxMaterialID == "79467")
                    {
                        /* <STICKER>
                         * Lamination - HA-710S - DS
                         * Lamination - DM58T - QD, VJ
                         * Lamination - WSM-100 - JJ */

                        if (view.IsCellSelected(e.RowHandle, e.Column) == false)
                        {
                            e.Appearance.ForeColor = Color.DarkOrange;
                            e.Appearance.Font = new Font(e.Appearance.Font, FontStyle.Bold);
                        }
                    }
                }
            }
        }

        /// <summary>
        /// 그리드 셀 스타일 적용
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void gvwBomMultiple_RowCellStyle(object sender, RowCellStyleEventArgs e)
        {
            GridView view = sender as GridView;

            // 개별 요청 여부
            string isManual = view.GetRowCellValue(e.RowHandle, "MANUAL_ADD").ToString();

            // 개별 요청한 자재는 스타일 따로 적용
            if (isManual == "Y")
            {
                if (e.Column.FieldName != "PUR_STATUS")
                {
                    if (view.IsCellSelected(e.RowHandle, e.Column) == false)
                    {
                        e.Appearance.BackColor = Color.DarkSeaGreen;
                    }
                }
                else if (e.Column.FieldName == "PUR_STATUS")
                {
                    // 구매 상태 값에 따라 각각 스타일 적용
                    string statusFlag = e.CellValue.ToString();

                    if (statusFlag == "Request")
                    {
                        if (view.IsCellSelected(e.RowHandle, e.Column) == false)
                        {
                            e.Appearance.BackColor = Color.Yellow;
                        }
                    }
                    else if (statusFlag == "Purchase")
                    {
                        if (view.IsCellSelected(e.RowHandle, e.Column) == false)
                        {
                            e.Appearance.BackColor = Color.Cyan;
                        }
                    }
                    else if (statusFlag == "Cancel")
                    {
                        if (view.IsCellSelected(e.RowHandle, e.Column) == false)
                        {
                            e.Appearance.BackColor = Color.DarkGray;
                            e.Appearance.ForeColor = Color.White;
                        }
                    }
                }
            }
            else // BOM 라인 아이템 스타일 적용
            {
                // PUR_STATUS, CHK 컬럼 외
                if (e.Column.FieldName != "PUR_STATUS" && e.Column.FieldName != "PMC_CHK" && e.Column.FieldName != "THREE_P_CHK")
                {
                    // 기준이 되는 Colorway는 스타일 따로 적용
                    string colorVersion = view.GetRowCellValue(e.RowHandle, "COLOR_VER").ToString();

                    if (colorVersion == "VER1")
                    {
                        if (view.IsCellSelected(e.RowHandle, e.Column) == false)
                        {
                            e.Appearance.BackColor = Color.Tan;
                            e.Appearance.Font = new Font(e.Appearance.Font, FontStyle.Bold);
                            return;
                        }
                    }

                    if (e.Column.FieldName == "PCX_SUPP_MAT_ID" || e.Column.FieldName == "PCX_MAT_ID")
                    {
                        string pcxCode = view.GetRowCellValue(e.RowHandle, e.Column).ToString();

                        if (pcxCode == "100")
                        {
                            if (view.IsCellSelected(e.RowHandle, e.Column) == false)
                            {
                                e.Appearance.ForeColor = Color.Red;
                                e.Appearance.Font = new Font(e.Appearance.Font, FontStyle.Bold);
                            }
                        }
                    }
                    else if (e.Column.FieldName == "MAT_NAME")
                    {
                        string matName = view.GetRowCellValue(e.RowHandle, "MAT_NAME").ToString();
                        string pcxMaterialID = view.GetRowCellValue(e.RowHandle, "PCX_MAT_ID").ToString();

                        if (matName == "PLACEHOLDER")
                        {
                            if (view.IsCellSelected(e.RowHandle, e.Column) == false)
                            {
                                e.Appearance.ForeColor = Color.Red;
                                e.Appearance.Font = new Font(e.Appearance.Font, FontStyle.Bold);
                            }
                        }
                        else if (pcxMaterialID == "78730" || pcxMaterialID == "78728" || pcxMaterialID == "79466")
                        {
                            /* <LAMINATION>
                             * Lamination  HA 700L - DS
                             * Lamination - DM-629-MD - QD, VJ
                             * Lamination - WSM-170 - JJ */

                            if (view.IsCellSelected(e.RowHandle, e.Column) == false)
                            {
                                e.Appearance.ForeColor = Color.LimeGreen;
                                e.Appearance.Font = new Font(e.Appearance.Font, FontStyle.Bold);
                            }
                        }
                        else if (pcxMaterialID == "54638" || pcxMaterialID == "79341")
                        {
                            /* <LOGO>
                             * GENERIC SCREENED_TRANSFER LOGO
                             * GENERIC SCREEN PRINT TRANSFER */

                            if (view.IsCellSelected(e.RowHandle, e.Column) == false)
                            {
                                e.Appearance.ForeColor = Color.DeepSkyBlue;
                                e.Appearance.Font = new Font(e.Appearance.Font, FontStyle.Bold);
                            }
                        }
                        else if (pcxMaterialID == "87031" || pcxMaterialID == "78733" || pcxMaterialID == "79467")
                        {
                            /* <STICKER>
                             * Lamination - HA-710S - DS
                             * Lamination - DM58T - QD, VJ
                             * Lamination - WSM-100 - JJ */

                            if (view.IsCellSelected(e.RowHandle, e.Column) == false)
                            {
                                e.Appearance.ForeColor = Color.DarkOrange;
                                e.Appearance.Font = new Font(e.Appearance.Font, FontStyle.Bold);
                            }
                        }
                    }
                }
                else if (e.Column.FieldName == "PUR_STATUS")
                {
                    // 구매 상태 값에 따라 각각 스타일 적용
                    string statusFlag = e.CellValue.ToString();

                    if (statusFlag == "Not Yet")
                    {
                        // IsCellSelected : Indicates whether the cell is selected. true is the cell is selected; otherwise, false. 
                        if (view.IsCellSelected(e.RowHandle, e.Column) == false)
                        {
                            e.Appearance.BackColor = Color.Salmon;
                        }
                    }
                    else if (statusFlag == "Request")
                    {
                        if (view.IsCellSelected(e.RowHandle, e.Column) == false)
                        {
                            e.Appearance.BackColor = Color.Yellow;
                        }
                    }
                    else if (statusFlag == "Purchase")
                    {
                        if (view.IsCellSelected(e.RowHandle, e.Column) == false)
                        {
                            e.Appearance.BackColor = Color.Cyan;
                        }
                    }
                    else if (statusFlag == "Cancel")
                    {
                        if (view.IsCellSelected(e.RowHandle, e.Column) == false)
                        {
                            e.Appearance.BackColor = Color.DarkGray;
                            e.Appearance.ForeColor = Color.White;
                        }
                    }
                }
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void CustomRowCellStyle(object sender, RowCellStyleEventArgs e)
        {
            GridView view = sender as GridView;
            bool isManuallyAdded = view.GetRowCellValue(e.RowHandle, "MANUAL_ADD").ToString().Equals("Y");
            string colorVersion = view.GetRowCellValue(e.RowHandle, "COLOR_VER").ToString();

            if (isManuallyAdded)
            {
                switch (colorVersion)
                {
                    case "Manual":  // Before enroll.

                        if (view.IsCellSelected(e.RowHandle, e.Column) == false)
                            e.Appearance.BackColor = Color.GreenYellow;

                        return;

                    case "Enrolled": // After enroll.

                        if (view.IsCellSelected(e.RowHandle, e.Column) == false)
                            e.Appearance.BackColor = Color.DarkSeaGreen;

                        break;
                }
            }
            else
            {
                if (view == gvwPMCMultiple || view == gvw3PMultiple)
                {
                    if (colorVersion == "VER1")
                    {
                        if (view.IsCellSelected(e.RowHandle, e.Column) == false)
                        {
                            e.Appearance.BackColor = Color.Tan;
                            e.Appearance.Font = font;

                            return;
                        }
                    }
                }
            }

            if (SetOfMaterialFields.Contains(e.Column.FieldName))
            {
                // Draw for PLACEHOLDER.
                if (view.GetRowCellValue(e.RowHandle, "MAT_NAME").ToString().Equals("PLACEHOLDER"))
                {
                    if (view.IsCellSelected(e.RowHandle, e.Column) == false)
                    {
                        e.Appearance.ForeColor = Color.Red;
                        e.Appearance.Font = font;
                    }
                }
            }

            if (RequiredFields.Contains(e.Column.FieldName))
            {
                // Draw for the value which is required.
                if (view.GetRowCellValue(e.RowHandle, e.Column.FieldName).ToString().Equals(""))
                {
                    if (view.IsCellSelected(e.RowHandle, e.Column) == false)
                        e.Appearance.BackColor = Color.Red;
                }
            }
        }

        /// <summary>
        /// PMC/3P 체크 표기를 마우스 클릭 한 번으로 가능하도록
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void CustomMouseDown(object sender, MouseEventArgs e)
        {
            GridView view = sender as GridView;

            // GridHitInfo : Contains information about a specific point within a Grid View.
            GridHitInfo hitInfo = view.CalcHitInfo(e.Location);

            // InRowCell : Gets a value indicating whether the test point is within a cell.
            if (hitInfo.InRowCell)
            {
                // RealColumnEdit : Gets the repository item that actually represents the column's editor.
                if (hitInfo.Column.RealColumnEdit is DevExpress.XtraEditors.Repository.RepositoryItemCheckEdit)
                {
                    // 에디터 생성 전 행/열 선택
                    view.FocusedColumn = hitInfo.Column;
                    view.FocusedRowHandle = hitInfo.RowHandle;
                    // ShowEditor : Creates an editor for the cell
                    view.ShowEditor();
                    // ActiveEditor : Gets a View's active editor.
                    CheckEdit edit = view.ActiveEditor as CheckEdit;

                    if (edit == null)
                        return;

                    edit.Toggle();
                    DXMouseEventArgs.GetMouseArgs(e).Handled = true;
                }
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void CustomDoubleClick(object sender, EventArgs e)
        {
            GridView view = sender as GridView;

            if (view.FocusedColumn.FieldName == "PUR_STATUS")
            {
                PurchaseRecords form = new PurchaseRecords();
                form.Factory = view.GetFocusedRowCellValue("FACTORY").ToString();
                form.WSNo = view.GetFocusedRowCellValue("WS_NO").ToString();
                form.PartSeq = view.GetFocusedRowCellValue("PART_SEQ").ToString();
                form.Delimiter = view.GetFocusedRowCellValue("DELIMITER").ToString();

                form.ShowDialog();
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void CustomCellValueChanged(object sender, CellValueChangedEventArgs e)
        {
            GridView view = sender as GridView;
            GridCell[] cells = view.GetSelectedCells();

            try
            {
                string pcxColorID = string.Empty;
                string pdmColorCode = string.Empty;
                string pdmColorName = string.Empty;

                string partCode = string.Empty;
                string partName = string.Empty;
                string partType = string.Empty;

                string mxsxlNumber = string.Empty;
                string pcxSuppMatID = string.Empty;
                //string csCode = string.Empty;
                string mcsNumber = string.Empty;
                string pcxMatID = string.Empty;
                string pdmMatCode = string.Empty;
                string pdmMatName = string.Empty;
                string vendorName = string.Empty;

                // To avoid infinite loop.
                view.CellValueChanged -= new CellValueChangedEventHandler(CustomCellValueChanged);

                // If there are no changes, finish event.
                var currValue = view.ActiveEditor.EditValue;
                var oldValue = view.ActiveEditor.OldEditValue;

                if (currValue.ToString() == oldValue.ToString())
                    return;

                string value = currValue.ToString();

                if (view.FocusedColumn.FieldName == "COLOR_CD")
                {
                    #region PDM 컬러코드 다이렉트 입력 시, 다른 속성 자동 입력을 위해 코드 정보를 가져옴
                    if (value != "")
                    {
                        // 패키지 매개변수 입력
                        PKG_INTG_BOM.SELECT_FOR_DIRECT_INPUT pkgSelect = new PKG_INTG_BOM.SELECT_FOR_DIRECT_INPUT();
                        pkgSelect.ARG_TYPE = "Color";
                        pkgSelect.ARG_CODE = value;
                        pkgSelect.ARG_NAME = "";
                        pkgSelect.OUT_CURSOR = string.Empty;
                        // 패키지 호출하여 PCX Color 정보를 가져옴
                        DataTable dataSource = Common.projectBaseForm.Exe_Select_PKG(pkgSelect).Tables[0];
                        if (dataSource.Rows.Count > 0)
                        {
                            pdmColorCode = dataSource.Rows[0][0].ToString();
                            pdmColorName = dataSource.Rows[0][1].ToString();
                            pcxColorID = dataSource.Rows[0][2].ToString();
                        }
                        else
                        {
                            pcxColorID = "";
                            pdmColorCode = "";
                            pdmColorName = "";
                        }
                    }
                    #endregion
                }
                else if (view.FocusedColumn.FieldName == "COLOR_NAME")
                {
                    #region PDM 컬러코드 다이렉트 입력 시, 다른 속성 자동 입력을 위해 코드 정보를 가져옴
                    if (value != "")
                    {
                        // 패키지 매개변수 입력
                        PKG_INTG_BOM.SELECT_FOR_DIRECT_INPUT pkgSelect = new PKG_INTG_BOM.SELECT_FOR_DIRECT_INPUT();
                        pkgSelect.ARG_TYPE = "Color";
                        pkgSelect.ARG_CODE = "";
                        pkgSelect.ARG_NAME = value;
                        pkgSelect.OUT_CURSOR = string.Empty;
                        // 패키지 호출하여 PCX Color 정보를 가져옴
                        DataTable dataSource = Common.projectBaseForm.Exe_Select_PKG(pkgSelect).Tables[0];
                        if (dataSource.Rows.Count > 0)
                        {
                            pdmColorCode = dataSource.Rows[0][0].ToString();
                            pdmColorName = dataSource.Rows[0][1].ToString();
                            pcxColorID = dataSource.Rows[0][2].ToString();
                        }
                        else
                        {
                            pcxColorID = "";
                            pdmColorCode = "";
                            pdmColorName = "";
                        }
                    }
                    #endregion
                }
                else if (view.FocusedColumn.FieldName == "PART_NAME")
                {
                    #region Part Name 다이렉트 입력 시, 다른 속성 자동 입력을 위해 코드 정보를 가져옴

                    if (value != "")
                    {
                        if (this.CSBOMStatus != "F")
                        {
                            PKG_INTG_BOM.SELECT_FOR_DIRECT_INPUT pkgSelect = new PKG_INTG_BOM.SELECT_FOR_DIRECT_INPUT();
                            pkgSelect.ARG_TYPE = "Part";
                            pkgSelect.ARG_CODE = value;
                            pkgSelect.ARG_NAME = "";
                            pkgSelect.OUT_CURSOR = string.Empty;

                            DataTable dataSource = Common.projectBaseForm.Exe_Select_PKG(pkgSelect).Tables[0];
                            if (dataSource.Rows.Count > 0)
                            {
                                //partCode = dataSource.Rows[0][0].ToString();
                                partName = dataSource.Rows[0][1].ToString();
                                partType = dataSource.Rows[0][2].ToString();
                            }
                            else
                            {
                                //partCode = "";
                                partName = "";
                                partType = "";
                            }
                        }
                        else
                        {
                            partName = value;
                        }
                    }

                    #endregion
                }
                else if (view.FocusedColumn.FieldName == "MXSXL_NUMBER")
                {
                    #region MxSxL Number 다이렉트 입력 시, 다른 속성 자동 입력을 위해 코드 정보를 가져옴

                    if (value != "")
                    {
                        // Get the material info. from PCX library.
                        PKG_INTG_BOM.SELECT_FOR_DIRECT_INPUT pkgSelect = new PKG_INTG_BOM.SELECT_FOR_DIRECT_INPUT();
                        pkgSelect.ARG_TYPE = "Material";
                        pkgSelect.ARG_CODE = value;
                        pkgSelect.ARG_NAME = "Code";
                        pkgSelect.OUT_CURSOR = string.Empty;

                        DataTable dataSource = Common.projectBaseForm.Exe_Select_PKG(pkgSelect).Tables[0];

                        if (dataSource.Rows.Count == 1)
                        {
                            mxsxlNumber = dataSource.Rows[0]["MXSXL_NUMBER"].ToString();
                            pdmMatCode = dataSource.Rows[0]["MAT_CD"].ToString();
                            pdmMatName = dataSource.Rows[0]["MAT_NAME"].ToString();
                            vendorName = dataSource.Rows[0]["VENDOR_NAME"].ToString();
                            mcsNumber = dataSource.Rows[0]["MCS_NUMBER"].ToString();
                            pcxMatID = dataSource.Rows[0]["PCX_MTL_NUMBER"].ToString();
                            pcxSuppMatID = dataSource.Rows[0]["PCX_SUPP_MTL_NUMBER"].ToString();
                            //csCode = dataSource.Rows[0]["CS_CD"].ToString();
                        }
                        else
                        {
                            pcxMatID = "100";
                            pcxSuppMatID = "100";
                            pdmMatName = "PLACEHOLDER";

                            Common.ShowMessageBox("Unregistered code.", "E");
                        }
                    }

                    #endregion
                }
                else if (view.FocusedColumn.FieldName == "MAT_NAME")
                {
                    #region 자재명 다이렉트 입력 시, 다른 속성 자동 입력을 위해 코드 정보를 가져옴

                    if (value != "")
                    {
                        if (!Common.IsFakeBOM(CSBOMStatus))
                        {
                            PKG_INTG_BOM.SELECT_FOR_DIRECT_INPUT pkgSelect = new PKG_INTG_BOM.SELECT_FOR_DIRECT_INPUT();
                            pkgSelect.ARG_TYPE = "Material";
                            pkgSelect.ARG_CODE = value.ToUpper();
                            pkgSelect.ARG_NAME = "Name";
                            pkgSelect.OUT_CURSOR = string.Empty;

                            DataTable dataSource = Common.projectBaseForm.Exe_Select_PKG(pkgSelect).Tables[0];

                            if (dataSource.Rows.Count == 1)
                            {
                                mxsxlNumber = dataSource.Rows[0]["MXSXL_NUMBER"].ToString();
                                pdmMatCode = dataSource.Rows[0]["MAT_CD"].ToString();
                                pdmMatName = dataSource.Rows[0]["MAT_NAME"].ToString();
                                vendorName = dataSource.Rows[0]["VENDOR_NAME"].ToString();
                                mcsNumber = dataSource.Rows[0]["MCS_NUMBER"].ToString();
                                pcxMatID = dataSource.Rows[0]["PCX_MTL_NUMBER"].ToString();
                                pcxSuppMatID = dataSource.Rows[0]["PCX_SUPP_MTL_NUMBER"].ToString();
                                //csCode = dataSource.Rows[0]["CS_CD"].ToString();
                            }
                            else if (dataSource.Rows.Count > 1)
                            {
                                object[] parameters = new object[] { 1, value, "" };

                                FindCode form = new FindCode(parameters);

                                if (form.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                                {
                                    if (form.FORM_RESULT == null) return;

                                    // Convert returned data to string[].
                                    string[] results = form.FORM_RESULT as string[];

                                    mxsxlNumber = results[4];
                                    pdmMatCode = results[2];
                                    pdmMatName = results[3];
                                    vendorName = results[6];
                                    mcsNumber = results[5];
                                    pcxMatID = results[1];
                                    pcxSuppMatID = results[8];
                                    //csCode = results[7];
                                }
                            }
                            else
                            {
                                pcxSuppMatID = "100";
                                pcxMatID = "100";
                                pdmMatName = "PLACEHOLDER";

                                Common.ShowMessageBox("Unregistered code.", "E");
                            }
                        }
                        else
                        {
                            // For the Fake BOM, type it manually.
                            pdmMatName = value;
                        }
                    }

                    #endregion
                }

                foreach (GridCell cell in cells)
                {
                    if (view.GetRowCellValue(cell.RowHandle, "COLOR_VER").ToString().Equals("Enrolled") ||
                        view.GetRowCellValue(cell.RowHandle, "LOCK_YN").ToString().Equals("Y"))
                        continue;

                    switch (view.FocusedColumn.FieldName)
                    {
                        case "COLOR_CD":
                        case "COLOR_NAME":

                            view.SetRowCellValue(cell.RowHandle, "PCX_COLOR_ID", pcxColorID);
                            view.SetRowCellValue(cell.RowHandle, "COLOR_CD", pdmColorCode);
                            view.SetRowCellValue(cell.RowHandle, "COLOR_NAME", pdmColorName);
                            break;

                        case "PART_NAME":

                            view.SetRowCellValue(cell.RowHandle, "PART_NAME", partName);
                            view.SetRowCellValue(cell.RowHandle, "PART_TYPE", partType);
                            break;

                        case "MXSXL_NUMBER":
                        case "MAT_NAME":

                            view.SetRowCellValue(cell.RowHandle, "MXSXL_NUMBER", mxsxlNumber);
                            //view.SetRowCellValue(cell.RowHandle, "CS_CD", csCode);
                            view.SetRowCellValue(cell.RowHandle, "MAT_CD", pdmMatCode);
                            view.SetRowCellValue(cell.RowHandle, "MAT_NAME", pdmMatName);
                            view.SetRowCellValue(cell.RowHandle, "VENDOR_NAME", vendorName);
                            view.SetRowCellValue(cell.RowHandle, "MCS_NUMBER", mcsNumber);
                            view.SetRowCellValue(cell.RowHandle, "PCX_MAT_ID", pcxMatID);
                            view.SetRowCellValue(cell.RowHandle, "PCX_SUPP_MAT_ID", pcxSuppMatID);
                            break;

                        case "MAT_COMMENT":

                            view.SetRowCellValue(cell.RowHandle, "MAT_COMMENT", value);

                            // When an user manually types data into material comment,
                            // clear the encoded comment to avoid discrepancy in data.
                            view.SetRowCellValue(cell.RowHandle, "ENCODED_CMT", "");
                            break;

                        default:

                            view.SetRowCellValue(cell.RowHandle, view.FocusedColumn.FieldName, value);
                            break;

                    }

                    view.SetRowCellValue(cell.RowHandle, "ROW_STATUS", "U");
                }

                view.RefreshData();
            }
            catch (Exception ex)
            {
                Common.ShowMessageBox(ex.ToString(), "E");
            }
            finally
            {
                view.CellValueChanged += new CellValueChangedEventHandler(CustomCellValueChanged);
            }
        }

        /// <summary>
        /// Delete data on the selected cells.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void CustomKeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Delete)
            {
                GridView view = sender as GridView;

                try
                {
                    Action<List<string>, int> action = (set, rowHandle) =>
                    {
                        foreach (string field in set)
                        {
                            view.SetRowCellValue(rowHandle, field, "");
                            view.SetRowCellValue(rowHandle, "ROW_STATUS", "U");
                        }
                    };

                    view.CellValueChanged -= new CellValueChangedEventHandler(CustomCellValueChanged);

                    foreach (GridCell cell in view.GetSelectedCells())
                    {
                        switch (view.GetRowCellValue(cell.RowHandle, "MANUAL_ADD").ToString())
                        {
                            case "Y":

                                if (view.GetRowCellValue(cell.RowHandle, "COLOR_VER").ToString().Equals("Enrolled"))
                                {
                                    return;
                                }
                                else if (SetOfPartFields.Contains(cell.Column.FieldName))
                                {
                                    action(SetOfPartFields, cell.RowHandle);
                                }

                                break;

                            case "N":

                                if (view.GetRowCellValue(cell.RowHandle, "LOCK_YN").ToString().Equals("Y"))
                                    return;

                                break;
                        }

                        if (SetOfMaterialFields.Contains(cell.Column.FieldName))
                        {
                            action(SetOfMaterialFields, cell.RowHandle);
                        }
                        else if (SetOfColorFields.Contains(cell.Column.FieldName))
                        {
                            action(SetOfColorFields, cell.RowHandle);
                        }
                        else // In case of comment.
                        {
                            view.SetRowCellValue(cell.RowHandle, cell.Column.FieldName, "");

                            if (cell.Column.FieldName.Equals("MAT_COMMENT"))
                                view.SetRowCellValue(cell.RowHandle, "ENCODED_CMT", "");

                            view.SetRowCellValue(cell.RowHandle, "ROW_STATUS", "U");
                        }
                    }
                }
                finally
                {
                    view.CellValueChanged += new CellValueChangedEventHandler(CustomCellValueChanged);
                }
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void CustomShowingEditor(object sender, CancelEventArgs e)
        {
            GridView view = sender as GridView;
            string[] uneditableFieldName = new string[] { "PART_NAME", "PART_TYPE" };

            switch (view.GetRowCellValue(view.FocusedRowHandle, "MANUAL_ADD").ToString())
            {
                case "Y":
                    if (view.GetRowCellValue(view.FocusedRowHandle, "COLOR_VER").ToString().Equals("Enrolled"))
                        e.Cancel = true;

                    break;

                case "N":

                    if (view.GetRowCellValue(view.FocusedRowHandle, "LOCK_YN").ToString().Equals("Y"))
                        e.Cancel = true;
                    else
                    {
                        if (uneditableFieldName.Contains(view.FocusedColumn.FieldName))
                            e.Cancel = true;
                    }

                    break;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void CustomDrawRowIndicator(object sender, RowIndicatorCustomDrawEventArgs e)
        {
            GridView view = sender as GridView;

            if (e.Info.IsRowIndicator && e.RowHandle >= 0)
            {
                string rowStatus = view.GetRowCellValue(e.RowHandle, "ROW_STATUS").ToString();

                if (rowStatus == "D")
                {
                    // 삭제
                    e.Appearance.Font = new Font("SimSun", 11, FontStyle.Bold);
                    e.Appearance.ForeColor = Color.DimGray;
                    e.Appearance.TextOptions.HAlignment = HorzAlignment.Center;
                    e.Info.DisplayText = "D";
                }
                else if (rowStatus == "I")
                {
                    // 삽입
                    e.Appearance.Font = new Font("SimSun", 11, FontStyle.Bold);
                    e.Appearance.ForeColor = Color.Green;
                    e.Appearance.TextOptions.HAlignment = HorzAlignment.Center;
                    e.Info.DisplayText = "I";
                }
                else if (rowStatus == "U")
                {
                    // 수정
                    e.Appearance.Font = new Font("SimSun", 11, FontStyle.Bold);
                    e.Appearance.ForeColor = Color.Blue;
                    e.Appearance.TextOptions.HAlignment = HorzAlignment.Center;
                    e.Info.DisplayText = "U";
                }
            }
        }

        #region 3P Multiple

        private void gvw3PMultiple_RowCellStyle(object sender, RowCellStyleEventArgs e)
        {
            GridView view = sender as GridView;

            string colorVersion = view.GetRowCellValue(e.RowHandle, "COLOR_VER").ToString();
            string isManual = view.GetRowCellValue(e.RowHandle, "MANUAL_ADD").ToString();

            if (isManual == "N")
            {
                if (colorVersion == "VER1")
                {
                    if (view.IsCellSelected(e.RowHandle, e.Column) == false)
                    {
                        e.Appearance.BackColor = Color.Tan;
                        e.Appearance.Font = new Font(e.Appearance.Font, FontStyle.Bold);
                    }
                }
            }
            else if (isManual == "Y")   // 개별적으로 추가한 자재의 경우 색상 따로 표시
            {
                if (colorVersion == "Manual")
                {
                    // 등록 전
                    if (view.IsCellSelected(e.RowHandle, e.Column) == false)
                        e.Appearance.BackColor = Color.GreenYellow;
                }
                else
                {
                    // 등록 후
                    if (view.IsCellSelected(e.RowHandle, e.Column) == false)
                        e.Appearance.BackColor = Color.DarkSeaGreen;
                }
            }
        }

        /// <summary>
        /// 사용 안 함 - 3P는 개별 발주를 하지 않음
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void gvw3PMultiple_ShowingEditor(object sender, CancelEventArgs e)
        {
            GridView view = sender as GridView;

            string colorVersion = view.GetRowCellValue(view.FocusedRowHandle, "COLOR_VER").ToString();

            if (colorVersion != "Manual")
                e.Cancel = true;
        }

        /// <summary>
        /// 사용 안 함 - 3P는 개별 발주를 하지 않음
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void gvw3PMultiple_KeyDown(object sender, KeyEventArgs e)
        {
            GridView view = sender as GridView;

            GridCell[] cells = view.GetSelectedCells();

            if (e.KeyCode == Keys.Delete)
            {
                foreach (GridCell cell in cells)
                {
                    string colorVersion = view.GetRowCellValue(cell.RowHandle, "COLOR_VER").ToString();

                    if (colorVersion == "Manual" && colorVersion == "Manual" && cell.Column.FieldName != "DEV_COLORWAY_ID")
                        view.SetRowCellValue(cell.RowHandle, cell.Column, "");
                }
            }
        }

        #endregion

        private void Purchase_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (this.EditType == "Single")
            {
                Common.RefreshHeaderInfo(Common.viewPCXBOMManagement, this.ParentRowhandle);
            }
            else
            {
                this.DialogResult = System.Windows.Forms.DialogResult.OK;
            }
        }

        #endregion

        #region Functions defined by user

        /// <summary>
        /// Get datasource for the gridview of BOM.
        /// </summary>
        private DataTable GetDataSourceOfBom()
        {
            PKG_INTG_BOM_PURCHASE.LOAD_MATERIALS_TO_PURCHASE pkgSelect = new PKG_INTG_BOM_PURCHASE.LOAD_MATERIALS_TO_PURCHASE();
            pkgSelect.ARG_FACTORY = this.Factory;
            pkgSelect.ARG_CONCAT_WS_NO = this.WorksheetNumbers;
            pkgSelect.OUT_CURSOR = string.Empty;

            return Common.projectBaseForm.Exe_Select_PKG(pkgSelect).Tables[0];
        }

        /// <summary>
        /// Get datasource for the gridview which is selected by the location.
        /// </summary>
        private DataTable GetDataSourceOfPurchase(string location)
        {
            PKG_INTG_BOM_PURCHASE.LOAD_SHOP_BASKET_BY_LOC pkgSelect = new PKG_INTG_BOM_PURCHASE.LOAD_SHOP_BASKET_BY_LOC();
            pkgSelect.ARG_FACTORY = this.Factory;
            pkgSelect.ARG_CONCAT_WS_NO = this.WorksheetNumbers;
            pkgSelect.ARG_LOCATION = location;
            pkgSelect.OUT_CURSOR = string.Empty;

            DataTable dataSource = Common.projectBaseForm.Exe_Select_PKG(pkgSelect).Tables[0];
            dataSource.Columns["PART_SEQ"].AllowDBNull = true;

            return dataSource;
        }

        /// <summary>
        /// Clear rows on the gridview.
        /// </summary>
        /// <param name="view"></param>
        /// <param name="control"></param>
        /// <param name="location"></param>
        private void ClearRowsOnTheGridView(GridView view, GridControl control, string location)
        {
            // Avoid exception.
            if (view.RowCount == 0)
            {
                MessageBox.Show("There are no materials to delete.");
                return;
            }

            if (MessageBox.Show("Do you really want to delete all of materials below?", "",
                MessageBoxButtons.OKCancel, MessageBoxIcon.Question, MessageBoxDefaultButton.Button1) == System.Windows.Forms.DialogResult.OK)
            {
                ArrayList arrayList = new ArrayList();

                // Clear rows saved in the database.
                PKG_INTG_BOM_PURCHASE.EMPTY_SHOP_BASKET pkgDelete = new PKG_INTG_BOM_PURCHASE.EMPTY_SHOP_BASKET();
                pkgDelete.ARG_FACTORY = this.Factory;
                pkgDelete.ARG_CONCAT_WS_NO = this.WorksheetNumbers;
                pkgDelete.ARG_LOCATION = location;

                arrayList.Add(pkgDelete);

                if (Common.projectBaseForm.Exe_Modify_PKG(arrayList) == null)
                {
                    MessageBox.Show("Failed to empty.", "",
                        MessageBoxButtons.OK, MessageBoxIcon.Error, MessageBoxDefaultButton.Button1);

                    return;
                }

                // Clear rows on the visible gridview.
                (control.DataSource as DataTable).Rows.Clear();

                MessageBox.Show("Complete.");
            }
        }

        /// <summary>
        /// Add a new row on the gridview.
        /// </summary>
        /// <param name="gridControl"></param>
        private void AddNewRowToPurchase(GridControl gridControl)
        {
            DataTable dtCopied = (gridControl.DataSource as DataTable).Copy();

            if (this.EditType == "Single")
            {
                DataRow newRow = dtCopied.NewRow();

                newRow["FACTORY"] = gvwBomSingle.GetRowCellValue(0, "FACTORY").ToString();
                newRow["WS_NO"] = gvwBomSingle.GetRowCellValue(0, "WS_NO").ToString();
                newRow["BOM_ID"] = gvwBomSingle.GetRowCellValue(0, "BOM_ID").ToString();
                newRow["DEV_COLORWAY_ID"] = gvwBomSingle.GetRowCellValue(0, "DEV_COLORWAY_ID").ToString();
                newRow["COLOR_VER"] = "Manual";
                newRow["MANUAL_ADD"] = "Y";

                dtCopied.Rows.Add(newRow);
            }
            else if (this.EditType == "Multiple")
            {
                // Select colorway to add a row.
                BOMSelection selectionForm = new BOMSelection();
                selectionForm.FACTORY = this.Factory;
                selectionForm.CONCAT_WS_NO = this.WorksheetNumbers;

                if (selectionForm.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                {
                    if (selectionForm.FORM_RESULT.GetType().Name == "DataTable")
                    {
                        /* Add to all colorways. */

                        DataTable dtColorwayList = selectionForm.FORM_RESULT as DataTable;

                        foreach (DataRow row in dtColorwayList.Rows)
                        {
                            DataRow newRow = dtCopied.NewRow();

                            newRow["FACTORY"] = row["FACTORY"].ToString();
                            newRow["WS_NO"] = row["WS_NO"].ToString();
                            newRow["BOM_ID"] = row["BOM_ID"].ToString();
                            newRow["DEV_COLORWAY_ID"] = row["DEV_COLORWAY_ID"].ToString();
                            newRow["COLOR_VER"] = "Manual";
                            newRow["MANUAL_ADD"] = "Y";

                            dtCopied.Rows.Add(newRow);
                        }
                    }
                    else
                    {
                        /* Add to a specific colorway. */

                        var result = selectionForm.FORM_RESULT as List<string>;

                        DataRow newRow = dtCopied.NewRow();

                        newRow["FACTORY"] = result[0];
                        newRow["WS_NO"] = result[1];
                        newRow["BOM_ID"] = result[2];
                        newRow["DEV_COLORWAY_ID"] = result[3];
                        newRow["COLOR_VER"] = "Manual";         // 개별 추가의 경우 컬러 버전 Manual
                        newRow["MANUAL_ADD"] = "Y";             // 개별 추가의 경우 매뉴얼 여부 입력

                        dtCopied.Rows.Add(newRow);
                    }
                }
            }

            gridControl.DataSource = dtCopied;
            (gridControl.DefaultView as GridView).MoveLast();
        }

        /// <summary>
        /// Validate there are materials to order on the gridview.
        /// </summary>
        /// <param name="editType"></param>
        /// <returns></returns>
        private bool IsRowExisting()
        {
            if (ActiveViewPMC.RowCount == 0 && ActiveView3P.RowCount == 0)
            {
                Common.ShowMessageBox("There are no materials to order.", "W");
                return false;
            }
            else
                return true;
        }

        /// <summary>
        /// Validate all of materials manually added have been enrolled in database.
        /// </summary>
        /// <param name="editType"></param>
        /// <returns></returns>
        private bool AreAllEnrolled()
        {
            Func<GridControl, bool> hasNotEnrolled = (control) =>
            {
                if ((control.DataSource as DataTable).AsEnumerable().Where(
                    x => x["COLOR_VER"].ToString().Equals("Manual")).Count() > 0)
                {
                    Common.ShowMessageBox("There are materials which have not been enrolled yet. Please enroll first.", "E");
                    return true;
                }
                else
                    return false;
            };

            if (hasNotEnrolled(ActiveControlPMC)) return false;
            if (hasNotEnrolled(ActiveControl3P)) return false;

            return true;
        }

        /// <summary>
        /// Validate some fields required before finishing orders.
        /// </summary>
        /// <returns></returns>
        private bool ValidateFields()
        {
            // Exception part which doesn't have a color.
            List<string> listOfExceptionPart = new List<string> {
                "CO LABEL", "UPPER CEMENT", "THREAD", "WET CHEMISTRY" };
            bool isPass = true;

            // Validate from gridview of PMC.
            for (int rowHandle = 0; rowHandle < ActiveViewPMC.RowCount; rowHandle++)
            {
                string partName = ActiveViewPMC.GetRowCellValue(rowHandle, "PART_NAME").ToString();

                if (partName == "")
                {
                    MessageBox.Show("Part Name is required.", "",
                        MessageBoxButtons.OK, MessageBoxIcon.Error, MessageBoxDefaultButton.Button1);

                    FocusOnSelectedCell(rowHandle, "PART_NAME", ActiveViewPMC);
                    isPass = false;

                    return isPass;
                }
                else if (ActiveViewPMC.GetRowCellValue(rowHandle, "PART_TYPE").ToString() == "")
                {
                    MessageBox.Show("Part Type is required.", "",
                        MessageBoxButtons.OK, MessageBoxIcon.Error, MessageBoxDefaultButton.Button1);

                    FocusOnSelectedCell(rowHandle, "PART_TYPE", ActiveViewPMC);
                    isPass = false;

                    return isPass;
                }
                else if (ActiveViewPMC.GetRowCellValue(rowHandle, "MAT_NAME").ToString() == "")
                {
                    MessageBox.Show("Material Name is required.", "",
                        MessageBoxButtons.OK, MessageBoxIcon.Error, MessageBoxDefaultButton.Button1);

                    FocusOnSelectedCell(rowHandle, "MAT_NAME", ActiveViewPMC);
                    isPass = false;

                    return isPass;
                }
                else if (ActiveViewPMC.GetRowCellValue(rowHandle, "COLOR_NAME").ToString() == ""
                    && listOfExceptionPart.Contains(partName) == false)
                {
                    MessageBox.Show("Color Name is required.", "",
                        MessageBoxButtons.OK, MessageBoxIcon.Error, MessageBoxDefaultButton.Button1);

                    FocusOnSelectedCell(rowHandle, "COLOR_NAME", ActiveViewPMC);
                    isPass = false;

                    return isPass;
                }
            }

            // Validate from gridview of 3P.
            for (int rowHandle = 0; rowHandle < ActiveView3P.RowCount; rowHandle++)
            {
                if (ActiveView3P.GetRowCellValue(rowHandle, "MAT_NAME").ToString() == "")
                {
                    MessageBox.Show("Material Name is required.", "",
                        MessageBoxButtons.OK, MessageBoxIcon.Error, MessageBoxDefaultButton.Button1);

                    FocusOnSelectedCell(rowHandle, "MAT_NAME", ActiveView3P);
                    isPass = false;

                    return isPass;
                }
                else if (ActiveView3P.GetRowCellValue(rowHandle, "COLOR_NAME").ToString() == "")
                {
                    MessageBox.Show("Color Name is requried.", "",
                        MessageBoxButtons.OK, MessageBoxIcon.Error, MessageBoxDefaultButton.Button1);

                    FocusOnSelectedCell(rowHandle, "COLOR_NAME", ActiveView3P);
                    isPass = false;

                    return isPass;
                }
            }

            return isPass;
        }

        /// <summary>
        /// Check whether changed data has been applied to database.
        /// </summary>
        /// <param name="editType"></param>
        /// <returns></returns>
        private bool AreAllSaved()
        {
            Func<GridControl, bool> hasUnsaved = (control) =>
            {
                if ((control.DataSource as DataTable).AsEnumerable().Where(
                    x => !x["ROW_STATUS"].ToString().Equals("N")).Count() > 0)
                {
                    Common.ShowMessageBox("You must save the data changed first.", "E");
                    return true;
                }
                else
                    return false;
            };

            if (hasUnsaved(ActiveControlPMC)) return false;
            if (hasUnsaved(ActiveControl3P)) return false;

            return true;
        }

        /// <summary>
        /// Focus on the cell selected by user.
        /// </summary>
        /// <param name="rowHandle"></param>
        /// <param name="columnName"></param>
        /// <param name="view"></param>
        private void FocusOnSelectedCell(int rowHandle, string columnName, GridView view)
        {
            view.UnselectCell(view.FocusedRowHandle, view.FocusedColumn);
            view.SelectCell(rowHandle, view.Columns[columnName]);
            view.FocusedRowHandle = rowHandle;
            view.FocusedColumn = view.Columns[columnName];
        }

        /// <summary>
        /// Be deleted function.
        /// </summary>
        /// <returns></returns>
        private bool UpdateBOMHeadInformation()
        {
            try
            {
                ArrayList arrayList = new ArrayList();

                PKG_INTG_BOM_PURCHASE.SET_BOM_CFM_DATE pkgUpdate = new PKG_INTG_BOM_PURCHASE.SET_BOM_CFM_DATE();
                pkgUpdate.ARG_FACTORY = this.Factory;
                pkgUpdate.ARG_CHAINED_WS_NO = this.WorksheetNumbers;
                pkgUpdate.ARG_UPD_USER = Common.sessionID;

                arrayList.Add(pkgUpdate);

                if (Common.projectBaseForm.Exe_Modify_PKG(arrayList) == null)
                {
                    MessageBox.Show("Failed to change purhcase status");
                    return false;
                }

                return true;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
                return false;
            }
        }

        #endregion
    }
}