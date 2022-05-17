using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Collections;                       // ArrayList
using System.Diagnostics;                       // Process
using System.IO;
using System.Text.RegularExpressions;
using System.Net.Mail;
using System.Net;
using System.Net.Mime;

using DevExpress.XtraGrid;                      // GridControl
using DevExpress.XtraGrid.Columns;              // GridColumn
using DevExpress.XtraGrid.Views.Base;           // GridCell
using DevExpress.XtraGrid.Views.Grid;           // GridView
using DevExpress.XtraGrid.Views.Grid.ViewInfo;  // GridHitInfo
using DevExpress.Utils;                         // DXMouseEventArgs
using DevExpress.XtraEditors;                   // GridLookUpEdit
using DevExpress.XtraSplashScreen;              // XtraSplashScreen

using Excel = Microsoft.Office.Interop.Excel;

using CSI.Client.ProjectBaseForm;
using CSI.PCC.PCX.Packages;

using JPlatform.Client.JBaseForm;

namespace CSI.PCC.PCX
{
    public partial class BOMEditing : DevExpress.XtraEditors.XtraForm
    {
        /*************************
         * CS BOM Status
         * 1. N : Before Confirm.
         * 2. W : W/S Confirm.
         * 3. C : Confirm.
         * 4. F : Fake BOM.
         *************************/

        public string Factory { get; set; }
        public string WSNumber { get; set; }
        public string CSBOMStatus { get; set; }
        public int ParentRowhandle { get; set; }
        public string EditType { get; set; }
        public int NumSelectedBOMs { get; set; }

        private GridView view;
        private bool isLocked = false;  // for single edit.
        private bool hasLock = false;   // for multi edit.

        public BOMEditing()
        {
            InitializeComponent();
        }

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);

            DataTable dataSource;

            // Get a current activating view.
            view = EditType.Equals("Single") ? gvwSingleEdit : gvwMultipleEdit;

            // For only fake BOM.
            if (Common.IsFakeBOM(CSBOMStatus))
                HideColumnsFakeBOM();

            switch (EditType)
            {
                case "Single":

                    dataSource = ReturnBOMInfo("BOMCaption");

                    if (dataSource != null)
                    {
                        // Write caption on this form.
                        this.Text = string.Format("{0} / {1} / {2} / {3} / {4} / {5} / {6} / Rows : {7}",
                            dataSource.Rows[0]["CATEGORY"].ToString(),
                            dataSource.Rows[0]["SEASON"].ToString(),
                            dataSource.Rows[0]["DEV_NAME"].ToString(),
                            dataSource.Rows[0]["SAMPLE_TYPE"].ToString(),
                            dataSource.Rows[0]["DEV_STYLE_NUMBER"].ToString(),
                            dataSource.Rows[0]["DEV_COLORWAY_ID"].ToString(),
                            dataSource.Rows[0]["SUB_TYPE_REMARK"].ToString(),
                            dataSource.Rows[0]["RN"].ToString());

                        // Permit access to functions for a BOM of which type is inline and official sample type.
                        if (dataSource.Rows[0]["SUB_ST_CD"].ToString().Equals("NA") && !Common.IsFakeBOM(CSBOMStatus))
                        {
                            btnImport.Enabled = true;
                            btnConfirm.Enabled = true;
                        }

                        btnCBDChk.Checked = dataSource.Rows[0]["CBD_YN"].ToString().Equals("Y") ? true : false;
                    }

                    // Bind data to lookUpEdit for part type.
                    repositoryItemLookUpEdit1.DataSource = Common.CreatePartTypeLookUpSource();

                    // Hide a function used for stock info in case of overseas factory.
                    if (Factory != "DS")
                        StockInfo_Single.Visible = false;

                    // Change lock status to 'ongoing' to prevent lock from others.
                    PKG_INTG_BOM.BOM_LOCK pkgSelect = new PKG_INTG_BOM.BOM_LOCK();
                    pkgSelect.ARG_WORK_TYPE = "OCCUPY";
                    pkgSelect.ARG_FACTORY = Factory;
                    pkgSelect.ARG_WS_NO = WSNumber;
                    pkgSelect.OUT_CURSOR = string.Empty;

                    dataSource = Common.projectBaseForm.Exe_Select_PKG(pkgSelect).Tables[0];
                    isLocked = dataSource.Rows[0]["LOCK_YN"].ToString().Equals("Y") ? true : false;

                    if (isLocked)
                    {
                        Common.MakeColumnsUneditable(view);

                        if (!Common.adminUser.Contains(Common.sessionID))
                        {
                            btnReplace.Enabled = false;
                            btnReset.Enabled = false;
                            btnWSConfirm.Enabled = false;
                            btnConfirm.Enabled = false;
                        }
                    }

                    grdMultipleEdit.Visible = false;
                    grdSingleEdit.Dock = DockStyle.Fill;
                    grdSingleEdit.DataSource = BindDataSourceGridView();

                    break;

                case "Multiple":

                    repositoryItemLookUpEdit2.DataSource = Common.CreatePartTypeLookUpSource();

                    // Change lock status to 'ongoing' to prevent lock from others.
                    PKG_INTG_BOM.BOM_LOCK pkgSelect2 = new PKG_INTG_BOM.BOM_LOCK();
                    pkgSelect2.ARG_WORK_TYPE = "OCCUPY";
                    pkgSelect2.ARG_FACTORY = Factory;
                    pkgSelect2.ARG_WS_NO = WSNumber;
                    pkgSelect2.OUT_CURSOR = string.Empty;

                    dataSource = Common.projectBaseForm.Exe_Select_PKG(pkgSelect2).Tables[0];

                    if (dataSource.AsEnumerable().Where(x => x["LOCK_YN"].ToString().Equals("Y")).Count() > 0)
                        hasLock = true;

                    // Hide some buttons to prevent improper actions.
                    btnReplace.Visible = false;
                    btnReset.Visible = false;
                    btnImport.Visible = false;
                    btnCBDChk.Visible = false;
                    btnConfirm.Visible = false;
                    btnWSConfirm.Visible = false;

                    grdSingleEdit.Visible = false;
                    grdMultipleEdit.Dock = DockStyle.Fill;
                    grdMultipleEdit.DataSource = BindDataSourceGridView();

                    if (!AreNumPartsSame())
                        this.Close();

                    break;
            }
        }

        #region Conetext Menu Events

        /// <summary>
        /// 컨텍스트 메뉴 아이템 클릭 시, 선택한 아이템에 맞는 함수 호출
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ContextMenu_Click(object sender, EventArgs e)
        {
            System.Windows.Forms.ToolStripMenuItem menuItem = sender as System.Windows.Forms.ToolStripMenuItem;

            switch (menuItem.Name)
            {
                case "RowUp_Single":

                    if (Common.IsFilterApplied(view))
                        break;

                    if (isLocked)
                        break;

                    MoveRowUp();
                    break;

                case "RowUp_Multiple":

                    if (Common.IsFilterApplied(view))
                        break;

                    if (hasLock)
                    {
                        Common.ShowMessageBox("A locked BOM is included in the selection.", "E");
                        return;
                    }

                    if (!AllowMultiFunction())
                        return;

                    MoveRowUp_Multi();
                    break;

                case "RowDown_Single":

                    if (Common.IsFilterApplied(view))
                        break;

                    if (isLocked)
                        break;

                    MoveRowDown();
                    break;

                case "RowDown_Multiple":

                    if (Common.IsFilterApplied(view))
                        break;

                    if (hasLock)
                    {
                        Common.ShowMessageBox("A locked BOM is included in the selection.", "E");
                        return;
                    }

                    if (!AllowMultiFunction())
                        return;

                    MoveRowDown_Multi();
                    break;

                case "AddRow_Single":

                    if (Common.IsFilterApplied(view))
                        break;

                    if (isLocked)
                        break;

                    AddNewRow();
                    break;

                case "AddRow_Multiple":

                    if (Common.IsFilterApplied(view))
                        break;

                    if (hasLock)
                    {
                        Common.ShowMessageBox("A locked BOM is included in the selection.", "E");
                        return;
                    }

                    if (!AllowMultiFunction())
                        return;

                    AddNewRow_Multi();
                    break;

                case "DeleteRow_Single":

                    if (isLocked)
                        break;

                    DeleteRows();
                    break;

                case "DeleteRow_Multiple":

                    if (hasLock)
                    {
                        Common.ShowMessageBox("A locked BOM is included in the selection.", "E");
                        return;
                    }

                    if (!AllowMultiFunction())
                        return;

                    DeleteRows_Multi();
                    break;

                case "FindCode_Single":
                case "FindCode_Multiple":

                    FindCodeFromLibrary();
                    break;

                case "FindProcess_Single":
                case "FindProcess_Multiple":

                    FindProcessFromMaster();
                    break;

                case "FillColor_Single":
                case "FillColor_Multiple":

                    HighlightCell();
                    break;

                case "genComment_Single":
                case "genComment_Multiple":

                    ShowCommentForm();
                    break;

                case "BindPtrnTop_Single":
                case "BindPtrnTop_Multiple":

                    if (isLocked)
                        break;

                    if (Common.IsFilterApplied(view))
                        break;

                    BindPtrnPartName("Top");
                    break;

                case "BindPtrnEach_Single":
                case "BindPtrnEach_Multiple":

                    if (isLocked)
                        break;

                    if (Common.IsFilterApplied(view))
                        break;

                    BindPtrnPartName("Each");
                    break;

                case "CopyPtrnV1N4_Single":

                    if (isLocked)
                        break;

                    CopyFromCSPatternPart("New");
                    break;

                case "CopyPtrnF1F4_Single":

                    if (isLocked)
                        break;

                    CopyFromCSPatternPart("Old");
                    break;

                case "CopyPtrnV1N4_Multiple":

                    CopyFromCSPatternPart("New");
                    break;

                case "CopyPtrnF1F4_Multiple":

                    CopyFromCSPatternPart("Old");
                    break;

                case "MulChkComb_Single":
                case "MulChkComb_Multiple":

                    if (isLocked)
                        break;

                    MultiCheck("COMBINE_YN");
                    break;

                case "MulChkStc_Single":
                case "MulChkStc_Multiple":

                    if (isLocked)
                        break;

                    MultiCheck("STICKER_YN");
                    break;

                case "MulChkCode_Single":
                case "MulChkCode_Multiple":

                    if (view.Columns["CDMKR_YN"].FilterInfo.FilterString != "")
                    {
                        Common.ShowMessageBox("Please try again after releasing filter for 'Code' column.", "W");
                        return;
                    }

                    MultiCheck("CDMKR_YN");
                    break;

                case "MulChkMdsl_Single":
                case "MulChkMdsl_Multiple":

                    if (isLocked)
                        break;

                    MultiCheck("MDSL_CHK");
                    break;

                case "MulChkOtsl_Single":
                case "MulChkOtsl_Multiple":

                    if (isLocked)
                        break;

                    MultiCheck("OTSL_CHK");
                    break;

                case "Transfer_Single":
                case "Transfer_Multiple":

                    if (Common.IsFilterApplied(view))
                        return;

                    if (Common.HasLineitemUnsaved(view))
                        return;

                    TransferToCodemaker();
                    break;

                case "trnsRecord_Single":
                case "trnsRecord_Multiple":

                    ShowTransferRecord();
                    break;

                case "MatInfo_Single":
                case "MatInfo_Multiple":

                    ShowMaterialInfo();
                    break;

                case "ShowFocusedRow_Single":
                case "ShowFocusedRow_Multiple":

                    ShowFocusedRow();
                    break;

                case "MDMPartAdd_Single":
                case "MDMPartAdd_Multiple":

                    OpenPartMDM();
                    break;

                case "AddAggregate_Single":

                    if (isLocked)
                        break;

                    AddAggregatePart(gvwSingleEdit);
                    break;

                case "StockInfo_Single":

                    ShowStockInformation(gvwSingleEdit);
                    break;

                case "BlackList_Single":
                case "BlackList_Multiple":

                    ShowBlackListMaterialInfo();
                    break;

                case "exportToExcel":

                    ExportBOMToExcel();
                    break;

                default:
                    break;
            }
        }

        /// <summary>
        /// 선택한 행을 올림
        /// </summary>
        private void MoveRowUp()
        {
            DataTable dataSource = grdSingleEdit.DataSource as DataTable;
            DataRow rowBeInserted = null;
            DataRow rowBeDeleted = null;
            int[] rowHandles = view.GetSelectedRows();

            if (view.FocusedRowHandle.Equals(0) || view.FocusedRowHandle < 0)
            {
                Common.ShowMessageBox("Not proper position.", "E");
                return;
            }

            foreach (int rowHandle in rowHandles)
            {
                rowBeInserted = dataSource.NewRow();
                rowBeDeleted = view.GetDataRow(rowHandle);

                rowBeInserted.ItemArray = rowBeDeleted.ItemArray;
                dataSource.Rows.Remove(rowBeDeleted);

                try
                {
                    dataSource.Rows.InsertAt(rowBeInserted, rowHandle - 1);
                }
                catch (IndexOutOfRangeException e)
                {
                    Common.ShowMessageBox(e.ToString(), "E");
                    dataSource.Rows.InsertAt(rowBeDeleted, rowHandle);
                    return;
                }
            }

            // -1 means that the row was moved up one space. (=focused position was also moved up.)
            view.SelectCells(rowHandles[0] - 1, view.FocusedColumn, rowHandles[0] + rowHandles.Length - 2, view.FocusedColumn);
            view.FocusedRowHandle = rowHandles[0] - 1;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="control"></param>
        /// <param name="view"></param>
        private void MoveRowUp_Multi()
        {
            DataTable dataSource = grdMultipleEdit.DataSource as DataTable;
            DataRow rowBeInserted = null;
            DataRow rowBeDeleted = null;
            int focusedRowHandle = view.FocusedRowHandle;

            if (focusedRowHandle.Equals(0) || focusedRowHandle < 0)
            {
                Common.ShowMessageBox("Not proper position.", "E");
                return;
            }

            for (int i = 0; i < NumSelectedBOMs; i++)
            {
                rowBeInserted = dataSource.NewRow();
                rowBeDeleted = view.GetDataRow(focusedRowHandle + i);

                rowBeInserted.ItemArray = rowBeDeleted.ItemArray;
                dataSource.Rows.Remove(rowBeDeleted);

                try
                {
                    dataSource.Rows.InsertAt(rowBeInserted, focusedRowHandle + i - NumSelectedBOMs);
                }
                catch (IndexOutOfRangeException e)
                {
                    Common.ShowMessageBox(e.ToString(), "E");
                    dataSource.Rows.InsertAt(rowBeDeleted, focusedRowHandle + i);
                    return;
                }
            }

            view.SelectCell(focusedRowHandle - NumSelectedBOMs, view.FocusedColumn);
            view.FocusedRowHandle = focusedRowHandle - NumSelectedBOMs;
        }

        /// <summary>
        /// 선택한 행을 내림
        /// </summary>
        private void MoveRowDown()
        {
            DataTable dataSource = grdSingleEdit.DataSource as DataTable;
            DataRow rowBeInserted = null;
            DataRow rowBeDeleted = null;
            int firstRowHandle = view.GetSelectedRows()[0];
            int length = view.GetSelectedRows().Length;

            for (int i = 0; i < length; i++)
            {
                rowBeInserted = dataSource.NewRow();
                rowBeDeleted = view.GetDataRow(firstRowHandle);

                rowBeInserted.ItemArray = rowBeDeleted.ItemArray;
                dataSource.Rows.Remove(rowBeDeleted);

                try
                {
                    dataSource.Rows.InsertAt(rowBeInserted, firstRowHandle + length);
                }
                catch (IndexOutOfRangeException e)
                {
                    Common.ShowMessageBox(e.ToString(), "E");
                    dataSource.Rows.InsertAt(rowBeDeleted, firstRowHandle);
                    return;
                }
            }

            // +1 means that the row was moved down one space. (=focused position was also moved up.)
            view.SelectCells(firstRowHandle + 1, view.FocusedColumn, firstRowHandle + length, view.FocusedColumn);
            view.FocusedRowHandle = firstRowHandle + 1;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="control"></param>
        /// <param name="view"></param>
        private void MoveRowDown_Multi()
        {
            DataTable dataSource = grdMultipleEdit.DataSource as DataTable;
            DataRow rowBeInserted = null;
            DataRow rowBeDeleted = null;
            int focusedRowHandle = view.FocusedRowHandle;

            if (focusedRowHandle.Equals(0) || focusedRowHandle < 0)
            {
                Common.ShowMessageBox("Not proper position.", "E");
                return;
            }

            for (int i = 0; i < NumSelectedBOMs; i++)
            {
                rowBeInserted = dataSource.NewRow();
                rowBeDeleted = view.GetDataRow(focusedRowHandle);

                rowBeInserted.ItemArray = rowBeDeleted.ItemArray;
                dataSource.Rows.Remove(rowBeDeleted);

                try
                {
                    dataSource.Rows.InsertAt(rowBeInserted, focusedRowHandle + (NumSelectedBOMs * 2) - 1);
                }
                catch (IndexOutOfRangeException e)
                {
                    Common.ShowMessageBox(e.ToString(), "E");
                    dataSource.Rows.InsertAt(rowBeDeleted, focusedRowHandle);
                    return;
                }
            }

            view.SelectCell(focusedRowHandle + NumSelectedBOMs, gvwMultipleEdit.FocusedColumn);
            view.FocusedRowHandle = focusedRowHandle + NumSelectedBOMs;
        }

        /// <summary>
        /// 신규 행 추가
        /// </summary>
        /// <param name="_type"></param>
        private void AddNewRow()
        {
            DataTable dataSource = grdSingleEdit.DataSource as DataTable;
            DataRow newRow = dataSource.NewRow();
            int focusedRowHandle = view.FocusedRowHandle;

            if (view.GetSelectedCells().Length > 1)
            {
                Common.ShowMessageBox("Please select only one cell.", "W");
                return;
            }

            // Enter the key of the row.
            newRow["FACTORY"] = Factory;
            newRow["WS_NO"] = WSNumber;

            if (focusedRowHandle < 0)
            {
                // In case of the first row on grid.
                newRow["PART_SEQ"] = "1";
                newRow["PART_TYPE"] = "UPPER";
                newRow["ROW_STATUS"] = "I";
                newRow["PRVS_STATUS"] = "I";
                newRow["COMBINE_YN"] = "N";
                newRow["STICKER_YN"] = "N";
                newRow["MDSL_CHK"] = "N";
                newRow["OTSL_CHK"] = "N";

                try
                {
                    dataSource.Rows.InsertAt(newRow, 0);
                }
                catch (IndexOutOfRangeException e)
                {
                    Common.ShowMessageBox(e.ToString(), "E");
                    return;
                }

                view.SelectCell(0, view.FocusedColumn);
                view.FocusedRowHandle = 0;
            }
            else
            {
                view.UnselectCell(focusedRowHandle, view.FocusedColumn);

                newRow["PART_SEQ"] = "1";   // calculated when called.
                newRow["PART_TYPE"] = view.GetRowCellValue(focusedRowHandle, "PART_TYPE").ToString();   // Follow the part type of upper row.
                newRow["ROW_STATUS"] = "I";
                newRow["PRVS_STATUS"] = "I";
                newRow["COMBINE_YN"] = "N";
                newRow["STICKER_YN"] = "N";
                newRow["MDSL_CHK"] = "N";
                newRow["OTSL_CHK"] = "N";

                try
                {
                    dataSource.Rows.InsertAt(newRow, focusedRowHandle + 1);
                }
                catch (IndexOutOfRangeException e)
                {
                    Common.ShowMessageBox(e.ToString(), "E");
                    return;
                }

                view.SelectCell(focusedRowHandle + 1, view.FocusedColumn);
                view.FocusedRowHandle = focusedRowHandle + 1;
            }

            #region backup

            //try
            //{
            //    // 셀을 하나만 선택하였는지 확인(포커스의 모호함을 없애기 위함)
            //    GridCell[] cells = gvwSingleEdit.GetSelectedCells();

            //    if (cells.Length > 1)
            //    {
            //        MessageBox.Show("Please select only one cell", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Information);
            //        return;
            //    }

            //    // 현재 그리드의 데이터소스
            //    DataTable dataSource = (DataTable)grdSingleEdit.DataSource;

            //    // 신규 행 생성
            //    DataRow newRow = dataSource.NewRow();

            //    // 키 값 입력
            //    newRow["FACTORY"] = Factory;
            //    newRow["WS_NO"] = WSNumber;

            //    // 현재 포커스된 행의 위치
            //    int focusedRowHandle = gvwSingleEdit.FocusedRowHandle;

            //    // 뷰에 행이 하나도 없는 경우
            //    if (focusedRowHandle < 0)
            //    {
            //        newRow["PART_SEQ"] = "1";       // 첫 번째 행으로 삽입(DB에서 다시 생성)
            //        newRow["PART_TYPE"] = "UPPER";  // 첫 번째 행의 파트타입은 UPPER로 고정
            //        newRow["ROW_STATUS"] = "I";     // 인디케이터 변경
            //        newRow["PRVS_STATUS"] = "I";
            //        newRow["COMBINE_YN"] = "N";
            //        newRow["STICKER_YN"] = "N";
            //        newRow["MDSL_CHK"] = "N";
            //        newRow["OTSL_CHK"] = "N";

            //        // 첫 행에 삽입
            //        dataSource.Rows.InsertAt(newRow, 0);

            //        // 신규로 추가한 행을 포커싱
            //        gvwSingleEdit.SelectCell(0, gvwSingleEdit.FocusedColumn);
            //        gvwSingleEdit.FocusedRowHandle = 0;
            //    }
            //    else
            //    {
            //        // 신규 행 삽입 시, 상위 행의 파트타입을 기본값으로 설정
            //        string prevRowPartType = gvwSingleEdit.GetRowCellValue(focusedRowHandle, "PART_TYPE").ToString();

            //        // 신규로 추가할 행의 파트 시퀀스 채번
            //        string partSequence = CalcNextPartSequence();

            //        // 이전에 선택한 행의 포커싱 취소
            //        gvwSingleEdit.UnselectCell(focusedRowHandle, gvwSingleEdit.FocusedColumn);

            //        // 파트 시퀀스 입력
            //        newRow["PART_SEQ"] = partSequence;  // (DB에서 다시 생성)
            //        newRow["PART_TYPE"] = prevRowPartType;
            //        // 인디케이터 변경
            //        newRow["ROW_STATUS"] = "I";
            //        newRow["PRVS_STATUS"] = "I";
            //        newRow["COMBINE_YN"] = "N";
            //        newRow["STICKER_YN"] = "N";
            //        newRow["MDSL_CHK"] = "N";
            //        newRow["OTSL_CHK"] = "N";

            //        // 포커스된 행의 아래에 삽입
            //        dataSource.Rows.InsertAt(newRow, focusedRowHandle + 1);

            //        // 신규로 추가한 행을 포커싱
            //        gvwSingleEdit.SelectCell(focusedRowHandle + 1, gvwSingleEdit.FocusedColumn);
            //        gvwSingleEdit.FocusedRowHandle = focusedRowHandle + 1;
            //    }
            //}
            //catch (Exception ex)
            //{
            //    MessageBox.Show(ex.ToString());
            //    return;
            //}
            #endregion
        }

        /// <summary>
        /// 
        /// </summary>
        private void AddNewRow_Multi()
        {
            DataTable dataSource = grdMultipleEdit.DataSource as DataTable;
            DataRow newRow;
            int focusedRowHandle = view.FocusedRowHandle;

            for (int i = 0; i < NumSelectedBOMs; i++)
            {
                newRow = dataSource.NewRow();

                newRow["FACTORY"] = view.GetRowCellValue(focusedRowHandle + i, "FACTORY").ToString();
                newRow["WS_NO"] = view.GetRowCellValue(focusedRowHandle + i, "WS_NO").ToString();
                newRow["DEV_COLORWAY_ID"] = view.GetRowCellValue(focusedRowHandle + i, "DEV_COLORWAY_ID").ToString();
                newRow["COLOR_VER"] = view.GetRowCellValue(focusedRowHandle + i, "COLOR_VER").ToString();
                newRow["PART_SEQ"] = "1";   // calculated when called.
                newRow["PART_TYPE"] = view.GetRowCellValue(focusedRowHandle + i, "PART_TYPE").ToString();
                newRow["ROW_STATUS"] = "I";
                newRow["PRVS_STATUS"] = "I";
                newRow["COMBINE_YN"] = "N";
                newRow["STICKER_YN"] = "N";
                newRow["MDSL_CHK"] = "N";
                newRow["OTSL_CHK"] = "N";

                try
                {
                    dataSource.Rows.InsertAt(newRow, focusedRowHandle + NumSelectedBOMs + i);
                }
                catch (IndexOutOfRangeException e)
                {
                    Common.ShowMessageBox(e.ToString(), "E");
                    return;
                }
            }

            view.UnselectCell(focusedRowHandle, view.FocusedColumn);
            view.SelectCell(focusedRowHandle + NumSelectedBOMs, view.FocusedColumn);
            view.FocusedRowHandle = focusedRowHandle + NumSelectedBOMs;

            #region backup

            //try
            //{
            //    // 현재 그리드의 데이터소스
            //    DataTable dataSource = (DataTable)grdMultipleEdit.DataSource;

            //    // 유저가 선택한 행의 위치
            //    int focusedRowHandle = gvwMultipleEdit.FocusedRowHandle;

            //    // 각 BOM별 추가될 파트 시퀀스
            //    int[] partSeq = CalcNextPartSequence_Multi();    // DB에서 다시 생성함

            //    // 유저가 선택한 BOM의 개수만큼 반복
            //    for (int i = 0; i < NumSelectedBOMs; i++)
            //    {
            //        string factory = gvwMultipleEdit.GetRowCellValue(focusedRowHandle + i, "FACTORY").ToString();
            //        string wsNo = gvwMultipleEdit.GetRowCellValue(focusedRowHandle + i, "WS_NO").ToString();
            //        string colorwayID = gvwMultipleEdit.GetRowCellValue(focusedRowHandle + i, "DEV_COLORWAY_ID").ToString();
            //        string colorVersion = gvwMultipleEdit.GetRowCellValue(focusedRowHandle + i, "COLOR_VER").ToString();
            //        string partTYpe = gvwMultipleEdit.GetRowCellValue(focusedRowHandle + i, "PART_TYPE").ToString();

            //        DataRow newRow = dataSource.NewRow();       // 신규 행 생성
            //        newRow["FACTORY"] = factory;
            //        newRow["WS_NO"] = wsNo;
            //        newRow["DEV_COLORWAY_ID"] = colorwayID;
            //        newRow["COLOR_VER"] = colorVersion;

            //        newRow["PART_SEQ"] = partSeq[i];            // DB에서 다시 생성함
            //        newRow["PART_TYPE"] = partTYpe;

            //        newRow["ROW_STATUS"] = "I";
            //        newRow["PRVS_STATUS"] = "I";
            //        newRow["COMBINE_YN"] = "N";
            //        newRow["STICKER_YN"] = "N";
            //        newRow["MDSL_CHK"] = "N";
            //        newRow["OTSL_CHK"] = "N";
            //        // 다음 파트 묶음 아래에 삽입
            //        dataSource.Rows.InsertAt(newRow, focusedRowHandle + NumSelectedBOMs + i);
            //    }
            //    // 이전에 선택한 행의 포커싱 취소
            //    gvwMultipleEdit.UnselectCell(focusedRowHandle, gvwMultipleEdit.FocusedColumn);
            //    // 신규로 추가한 행을 포커싱
            //    gvwMultipleEdit.SelectCell(focusedRowHandle + NumSelectedBOMs, gvwMultipleEdit.FocusedColumn);
            //    gvwMultipleEdit.FocusedRowHandle = focusedRowHandle + NumSelectedBOMs;
            //}
            //catch (Exception ex)
            //{
            //    MessageBox.Show(ex.ToString());
            //    return;
            //}
            #endregion
        }

        /// <summary>
        /// 행의 상태 값
        /// 1. N : 변경 없음
        /// 2. I : 신규
        /// 3. U : 수정
        /// 4. D : 삭제
        /// 선택한 행의 상태 값을 'D'로 변경
        /// </summary>
        private void DeleteRows()
        {
            if (view.RowCount.Equals(0))
                return;

            try
            {
                view.CellValueChanged -= new CellValueChangedEventHandler(CustomCellValueChanged);

                foreach (int rowHandle in view.GetSelectedRows())
                {
                    if (!view.GetRowCellValue(rowHandle, "ROW_STATUS").ToString().Equals("D"))
                    {
                        view.SetRowCellValue(rowHandle, "PRVS_STATUS", view.GetRowCellValue(rowHandle, "ROW_STATUS").ToString());
                        view.SetRowCellValue(rowHandle, "ROW_STATUS", "D");
                    }
                    else
                    {
                        view.SetRowCellValue(rowHandle, "ROW_STATUS", view.GetRowCellValue(rowHandle, "PRVS_STATUS").ToString());
                    }
                }
            }
            finally
            {
                view.RefreshData();
                view.CellValueChanged += new CellValueChangedEventHandler(CustomCellValueChanged);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        private void DeleteRows_Multi()
        {
            int rowHandle = 0;

            try
            {
                view.CellValueChanged -= new CellValueChangedEventHandler(CustomCellValueChanged);

                for (int i = 0; i < NumSelectedBOMs; i++)
                {
                    rowHandle = view.FocusedRowHandle + i;

                    if (!view.GetRowCellValue(rowHandle, "ROW_STATUS").ToString().Equals("D"))
                    {
                        view.SetRowCellValue(rowHandle, "PRVS_STATUS", view.GetRowCellValue(rowHandle, "ROW_STATUS").ToString());
                        view.SetRowCellValue(rowHandle, "ROW_STATUS", "D");
                    }
                    else
                    {
                        view.SetRowCellValue(rowHandle, "ROW_STATUS", view.GetRowCellValue(rowHandle, "PRVS_STATUS").ToString());
                    }
                }
            }
            finally
            {
                view.RefreshData();
                view.CellValueChanged += new CellValueChangedEventHandler(CustomCellValueChanged);
            }
        }

        /// <summary>
        /// 라이브러리에서 코드를 찾아 셀에 입력
        /// </summary>
        /// <param name="_type"></param>
        private void FindCodeFromLibrary()
        {
            if (Common.HasLineitemDeleted(view))
                return;

            // Parameters to send to the childForm.
            int initSearchType = 0;
            string keyword = view.GetFocusedRowCellValue(view.FocusedColumn).ToString();
            string partDelimiter = string.Empty;

            // Get the initial search type from the focused column.
            switch (view.FocusedColumn.FieldName)
            {
                case "PART_NAME":
                case "PART_TYPE":
                case "PART_CD":
                case "PTRN_PART_NAME":

                    initSearchType = 0;

                    if (view.FocusedColumn.FieldName.Equals("PTRN_PART_NAME"))
                        partDelimiter = "Pattern";

                    break;

                case "MXSXL_NUMBER":
                case "MCS_NUMBER":
                case "PCX_SUPP_MAT_ID":
                case "PCX_MAT_ID":
                case "MAT_CD":
                case "MAT_NAME":
                case "MAT_COMMENT":
                case "VENDOR_NAME":

                    initSearchType = 1;
                    break;

                case "PCX_COLOR_ID":
                case "COLOR_CD":
                case "COLOR_NAME":

                    initSearchType = 2;
                    break;

                // Close PCC Material.
                //case "CS_CD":

                //    initSearchType = 4;
                //    break;

                default:
                    return;
            }

            object[] parameters = new object[] { initSearchType, keyword, partDelimiter };

            FindCode form = new FindCode(parameters);

            if (form.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                if (form.FORM_RESULT == null) return;

                // Convert returned data to string[].
                string[] results = form.FORM_RESULT as string[];

                try
                {
                    // To avoid infinite loop.
                    view.CellValueChanged -= new CellValueChangedEventHandler(CustomCellValueChanged);

                    List<DataRow> selectedRows = new List<DataRow>();

                    foreach (int rowHandle in view.GetSelectedRows())
                        selectedRows.Add(view.GetDataRow(rowHandle));

                    foreach (DataRow row in selectedRows)
                    {
                        if (!row["LOCK_YN"].ToString().Equals("Y"))
                        {
                            if (results[0] == "Part")
                            {
                                string oldValue = row["PART_NAME"].ToString();

                                // When the part name is changed from 'Lamination' to general part.
                                if (oldValue.Contains("LAMINATION") && !results[1].Contains("LAMINATION"))
                                {
                                    // An user doesn't need to set nike pattern part name for general part.
                                    row["PTRN_PART_NAME"] = "";
                                    row["PTRN_PART_CD"] = "";
                                }

                                row["PART_NAME"] = results[1];
                                row["PART_CD"] = results[3];

                                // Set part type by each case.
                                if (results[1].Contains("AIRBAG"))
                                {
                                    // If the part name contains 'AIRBAG', fix BOM section to 'AIRBAG'.
                                    row["PART_TYPE"] = "AIRBAG";
                                    row["MDSL_CHK"] = "Y";
                                    row["OTSL_CHK"] = "N";
                                }
                                else if (results[1].Contains("LAMINATION"))
                                {
                                    // If the part name contains 'AIRBAG', fix BOM section to 'UPPER'.
                                    row["PART_TYPE"] = "UPPER";
                                    row["MDSL_CHK"] = "N";
                                    row["OTSL_CHK"] = "N";
                                }
                                else if (results[2] == "MIDSOLE" || results[2] == "OUTSOLE")
                                {
                                    // If the part type is 'MIDSOLE' or 'OUTSOLE', fix BOM section to it.
                                    row["PART_TYPE"] = results[2];
                                    row["MDSL_CHK"] = (results[2] == "MIDSOLE") ? "Y" : "N";
                                    row["OTSL_CHK"] = (results[2] == "OUTSOLE") ? "Y" : "N";
                                }
                                else if (results[4] == "3389" || results[4] == "3390")
                                {
                                    // Fix BOM section to 'UPPER' for WET CHEMISTRY & WET CHEMISTRY-MULTI parts.
                                    row["PART_TYPE"] = "UPPER";
                                    row["MDSL_CHK"] = "N";
                                    row["OTSL_CHK"] = "N";
                                }
                                else
                                {
                                    // Follow existing BOM section(When a row is created, the BOM section follows the upper row).
                                    row["PART_TYPE"] = results[2];
                                    row["MDSL_CHK"] = "N";
                                    row["OTSL_CHK"] = "N";
                                }

                                row["PCX_PART_ID"] = results[4];

                                Common.BindDefaultMaterialByNCFRule(view, row, results[4]);
                            }
                            else if (results[0] == "PatternPart")
                            {
                                row["PTRN_PART_NAME"] = results[1];
                                row["PTRN_PART_CD"] = results[3];
                            }
                            else if (results[0] == "PCX_Material")
                            {
                                if (results[1] == "87031" || results[1] == "78733" || results[1] == "79467")
                                    row["STICKER_YN"] = "Y";    // Lamination.
                                else if (results[1] == "54638" || results[1] == "79341")
                                    row["COMBINE_YN"] = "Y";    // Transfer logo.
                                else
                                    row["STICKER_YN"] = "N";

                                row["PCX_MAT_ID"] = results[1];
                                row["MAT_CD"] = results[2];
                                row["MAT_NAME"] = results[3];
                                row["MXSXL_NUMBER"] = results[4];
                                row["MCS_NUMBER"] = results[5];
                                row["VENDOR_NAME"] = results[6];
                                //row["CS_CD"] = results[7];
                                row["PCX_SUPP_MAT_ID"] = results[8];
                                row["NIKE_MS_STATE"] = results[9];
                                row["BLACK_LIST_YN"] = results[10];
                                row["MDM_NOT_EXST"] = results[11];

                                // Automatically tick for 'Code Transfer'.
                                if (results[11].Equals("Y"))
                                    row["CDMKR_YN"] = "Y";
                                else
                                    row["CDMKR_YN"] = "N";
                            }
                            else if (results[0] == "Color")
                            {
                                row["PCX_COLOR_ID"] = results[1];
                                row["COLOR_CD"] = results[2];
                                row["COLOR_NAME"] = results[3];
                            }

                            #region Close PCC Material

                            //else if (results[0] == "PCC_Material")
                            //{
                            //    // Automatically tick for sticker & combine columns.
                            //    if (results[1] == "87031" || results[1] == "78733" || results[1] == "79467")
                            //    {
                            //        row["STICKER_YN"] = "Y";
                            //    }
                            //    else if (results[1] == "54638" || results[1] == "79341")
                            //    {
                            //        row["COMBINE_YN"] = "Y";
                            //    }
                            //    else
                            //    {
                            //        row["STICKER_YN"] = "N";
                            //    }

                            //    row["PCX_MAT_ID"] = results[1];
                            //    row["MAT_CD"] = results[2];
                            //    row["MAT_NAME"] = results[3];
                            //    row["MXSXL_NUMBER"] = results[4];
                            //    row["MCS_NUMBER"] = results[5];
                            //    row["VENDOR_NAME"] = results[6];
                            //    row["CS_CD"] = results[7];
                            //    row["PCX_SUPP_MAT_ID"] = results[8];
                            //    row["NIKE_MS_STATE"] = results[9];
                            //    row["BLACK_LIST_YN"] = results[10];

                            //    // If an user finds material from 'PCC Material', it means the code of this material was already created.
                            //    row["MDM_NOT_EXST"] = "N";
                            //    row["CDMKR_YN"] = "N";
                            //}
                            //else if (results[0] == "CS_Material")
                            //{
                            //    row["PCX_MAT_ID"] = "100";
                            //    row["MAT_CD"] = results[2];
                            //    row["MAT_NAME"] = results[3];
                            //    row["MXSXL_NUMBER"] = results[1];
                            //    row["MCS_NUMBER"] = "";
                            //    row["VENDOR_NAME"] = "";
                            //    row["CS_CD"] = "CS";
                            //    row["PCX_SUPP_MAT_ID"] = "100";
                            //}

                            #endregion

                            if (!row["ROW_STATUS"].ToString().Equals("I"))
                                row["ROW_STATUS"] = "U";
                        }
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.ToString());
                }
                finally
                {
                    view.CellValueChanged += new CellValueChangedEventHandler(CustomCellValueChanged);
                }
            }
        }

        /// <summary>
        /// 프로세스 마스터에서 프로세스를 선택하여 저장
        /// </summary>
        private void FindProcessFromMaster()
        {
            List<DataRow> rows = new List<DataRow>();

            foreach (int rowHandle in view.GetSelectedRows())
                rows.Add(view.GetDataRow(rowHandle));

            if (rows.AsEnumerable().Where(x => x["ROW_STATUS"].ToString().Equals("D")).Count() > 0)
            {
                Common.ShowMessageBox("There are rows to be deleted.", "W");
                return;
            }

            FindProcess findForm = new FindProcess()
            {
                EnteredProcess = view.GetRowCellValue(view.FocusedRowHandle, "PROCESS").ToString().ToUpper()
            };

            if (findForm.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                try
                {
                    view.CellValueChanged -= new CellValueChangedEventHandler(CustomCellValueChanged);

                    // List process like process1, process2, process3,...
                    string process = string.Join(",", findForm.SelectedProcess.ToArray());

                    foreach (DataRow row in rows)
                    {
                        row["PROCESS"] = process;

                        if (!row["ROW_STATUS"].ToString().Equals("I"))
                            row["ROW_STATUS"] = "U";
                    }
                }
                finally
                {
                    view.CellValueChanged += new CellValueChangedEventHandler(CustomCellValueChanged);
                }
            }
        }

        /// <summary>
        /// 타입에 맞게 패턴파트 적용
        /// </summary>
        /// <param name="type"></param>
        private void BindPtrnPartName(string type)
        {
            int[] rowHandles = view.GetSelectedRows();
            string partName = string.Empty;
            string partCode = string.Empty;
            string rowStatus = string.Empty;
            string lockYN = string.Empty;

            try
            {
                view.CellValueChanged -= new CellValueChangedEventHandler(CustomCellValueChanged);

                foreach (int rowHandle in rowHandles)
                {
                    partName = type.Equals("Top") ? view.GetRowCellValue(rowHandles[0], "PART_NAME").ToString()
                        : view.GetRowCellValue(rowHandle, "PART_NAME").ToString();

                    partCode = type.Equals("Top") ? view.GetRowCellValue(rowHandles[0], "PART_CD").ToString()
                        : view.GetRowCellValue(rowHandle, "PART_CD").ToString();

                    rowStatus = view.GetRowCellValue(rowHandle, "ROW_STATUS").ToString();
                    lockYN = view.GetRowCellValue(rowHandle, "LOCK_YN").ToString();

                    if (!rowStatus.Equals("D") && !lockYN.Equals("Y"))
                    {
                        view.SetRowCellValue(rowHandle, "CS_PTRN_NAME", partName);
                        view.SetRowCellValue(rowHandle, "CS_PTRN_CD", partCode);

                        if (!rowStatus.Equals("I"))
                            view.SetRowCellValue(rowHandle, "ROW_STATUS", "U");
                    }
                }
            }
            finally
            {
                view.RefreshData();
                view.CellValueChanged += new CellValueChangedEventHandler(CustomCellValueChanged);
            }

            #region backup

            //try
            //{
            //    // 이벤트 꼬임을 방지하기 위해 CellValueChanged 이벤트 끊음
            //    view.CellValueChanged -= new CellValueChangedEventHandler(CustomCellValueChanged);

            //    // 유저가 선택한 행의 로우핸들 배열
            //    int[] rowHandles = view.GetSelectedRows();

            //    if (type == "Top")
            //    {
            //        // 가장 상단의 행으로 일괄 적용
            //        string partName = view.GetRowCellValue(rowHandles[0], "PART_NAME").ToString();
            //        string partCode = view.GetRowCellValue(rowHandles[0], "PART_CD").ToString();

            //        // 선택한 행의 개수만큼 반복
            //        foreach (int rowHandle in rowHandles)
            //        {
            //            // 선택한 행의 현재 상태 값
            //            string rowStatus = view.GetRowCellValue(rowHandle, "ROW_STATUS").ToString();

            //            if (rowStatus != "D")
            //            {
            //                view.SetRowCellValue(rowHandle, "CS_PTRN_NAME", partName);
            //                view.SetRowCellValue(rowHandle, "CS_PTRN_CD", partCode);
            //            }

            //            // 인디케이터를 "U"로 변경, 신규 행과 삭제할 행은 제외
            //            if (rowStatus != "I" && rowStatus != "D")
            //                view.SetRowCellValue(rowHandle, "ROW_STATUS", "U");
            //        }
            //    }
            //    else if (type == "Each")
            //    {
            //        // 각 행의 파트명으로 적용
            //        foreach (int rowHandle in rowHandles)
            //        {
            //            string partName = view.GetRowCellValue(rowHandle, "PART_NAME").ToString();
            //            string partCode = view.GetRowCellValue(rowHandle, "PART_CD").ToString();

            //            // 선택한 행의 현재 상태 값
            //            string rowStatus = view.GetRowCellValue(rowHandle, "ROW_STATUS").ToString();

            //            if (rowStatus != "D")
            //            {
            //                view.SetRowCellValue(rowHandle, "CS_PTRN_NAME", partName);
            //                view.SetRowCellValue(rowHandle, "CS_PTRN_CD", partCode);
            //            }

            //            // 인디케이터를 "U"로 변경, 신규 행과 삭제할 행은 제외
            //            if (rowStatus != "I" && rowStatus != "D")
            //                view.SetRowCellValue(rowHandle, "ROW_STATUS", "U");
            //        }
            //    }

            //    // 스타일을 새로 적용하기 위해 DataSource Refresh
            //    view.RefreshData();

            //    // 이벤트 다시 연결
            //    view.CellValueChanged += new CellValueChangedEventHandler(CustomCellValueChanged);
            //}
            //catch (Exception ex)
            //{
            //    MessageBox.Show(ex.ToString());
            //    return;
            //}

            #endregion
        }

        /// <summary>
        /// 여러 셀 선택 후 한 번에 Combine 체크 표기
        /// </summary>
        /// <param name="view"></param>
        private void MultiCheck(string fieldName)
        {
            string rowStatus = string.Empty;
            string lockYN = string.Empty;

            try
            {
                view.CellValueChanged -= new CellValueChangedEventHandler(CustomCellValueChanged);

                foreach (int rowHandle in view.GetSelectedRows())
                {
                    rowStatus = view.GetRowCellValue(rowHandle, "ROW_STATUS").ToString();
                    lockYN = view.GetRowCellValue(rowHandle, "LOCK_YN").ToString();

                    if (!rowStatus.Equals("D") && !lockYN.Equals("Y"))
                    {
                        if (view.GetRowCellValue(rowHandle, fieldName).ToString().Equals("Y"))
                            view.SetRowCellValue(rowHandle, fieldName, "N");
                        else
                            view.SetRowCellValue(rowHandle, fieldName, "Y");

                        if (!rowStatus.Equals("I") && !fieldName.Equals("CDMKR_YN"))
                            view.SetRowCellValue(rowHandle, "ROW_STATUS", "U");
                    }
                }
            }
            finally
            {
                view.RefreshData();
                view.CellValueChanged += new CellValueChangedEventHandler(CustomCellValueChanged);
            }
        }

        /// <summary>
        /// Transfer data selected by user to codemaker.
        /// </summary>
        /// <param name="view"></param>
        private void TransferToCodemaker()
        {
            ArrayList list = new ArrayList();
            DataTable dataSource = ((view.Equals(gvwSingleEdit)) ? grdSingleEdit.DataSource : grdMultipleEdit.DataSource) as DataTable;
            List<DataRow> rows = dataSource.AsEnumerable().Where(r => r["CDMKR_YN"].ToString().Equals("Y")).ToList();

            foreach (DataRow row in rows)
            {
                PKG_INTG_BOM.TRANSFER_DATA_TO_CDMKR pkgInsert = new PKG_INTG_BOM.TRANSFER_DATA_TO_CDMKR();
                pkgInsert.ARG_PART_NAME = row["PART_NAME"].ToString();
                pkgInsert.ARG_MXSXL_NUMBER = row["MXSXL_NUMBER"].ToString();
                pkgInsert.ARG_MAT_NAME = row["MAT_NAME"].ToString();
                pkgInsert.ARG_MAT_COMMENT = row["MAT_COMMENT"].ToString();
                pkgInsert.ARG_MCS_NUMBER = row["MCS_NUMBER"].ToString();
                pkgInsert.ARG_COLOR_CD = row["COLOR_CD"].ToString();
                pkgInsert.ARG_COLOR_NAME = row["COLOR_NAME"].ToString();
                pkgInsert.ARG_COLOR_COMMENT = row["COLOR_COMMENT"].ToString();
                pkgInsert.ARG_PCX_MAT_ID = row["PCX_MAT_ID"].ToString();
                pkgInsert.ARG_PCX_SUPP_MAT_ID = row["PCX_SUPP_MAT_ID"].ToString();
                pkgInsert.ARG_REQ_USER = Common.sessionID;
                pkgInsert.ARG_FACTORY = row["FACTORY"].ToString();
                pkgInsert.ARG_WS_NO = row["WS_NO"].ToString();
                pkgInsert.ARG_PART_SEQ = row["PART_SEQ"].ToString();

                list.Add(pkgInsert);
            }

            if (Common.projectBaseForm.Exe_Modify_PKG(list) == null)
            {
                Common.ShowMessageBox("Failed to transfer data.", "E");
                return;
            }

            MessageBox.Show("Complete.");
        }

        /// <summary>
        /// Show records of data transferred to codemaker.
        /// </summary>
        private void ShowTransferRecord()
        {
            TransferRecord formRecord = new TransferRecord();

            formRecord.INHERITANCE = new string[] { Factory, WSNumber };

            if (formRecord.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            { }
        }

        /// <summary>
        /// 
        /// </summary>
        private void ShowMaterialInfo()
        {
            using (CSI.PCC.Common.MaterialInformation form = new PCC.Common.MaterialInformation()
            {
                PCXMatID = view.GetRowCellValue(view.FocusedRowHandle, "PCX_MAT_ID").ToString(),
                PCXSuppMatID = view.GetRowCellValue(view.FocusedRowHandle, "PCX_SUPP_MAT_ID").ToString(),
                BaseForm = Common.projectBaseForm
            })
            {
                form.ShowDialog();
            }
        }

        /// <summary>
        /// 선택된 셀이 포함된 행을 선택
        /// </summary>
        private void SelectFocusedRow(GridView view)
        {
            int[] rowHandles = view.GetSelectedRows();
            if (rowHandles.Length > 0)
            {
                foreach (int rowHandle in rowHandles)
                {
                    view.SelectRow(rowHandle);
                }
            }
        }

        /// <summary>
        /// 
        /// </summary>
        private void ShowFocusedRow()
        {
            int[] rowHandles = view.GetSelectedRows();

            if (rowHandles.Length == 0)
            {
                view.SelectRow(view.FocusedRowHandle);
            }
            else if (rowHandles.Length == 1)
            {
                // 1개일 경우 선택인지 취소인지 구분
                if (IsSelected(view.FocusedRowHandle))
                    view.UnselectRow(view.FocusedRowHandle);
                else
                    view.SelectRow(view.FocusedRowHandle);
            }
            else
            {
                // 1개 이상일 경우 선택인지 취소인지 구분
                string type = "Select";
                GridCell[] cells = view.GetSelectedCells();

                if (cells.Length > 25)
                    type = "Cancel";
                else
                    type = "Select";

                if (type == "Select")
                {
                    // 선택한 행 모두 전체 선택
                    foreach (int rowHandle in rowHandles)
                    {
                        if (IsSelected(rowHandle))
                            view.UnselectRow(rowHandle);
                        else
                            view.SelectRow(rowHandle);
                    }
                }
                else if (type == "Cancel")
                {
                    // 포커스된 행만 전체 선택 취소
                    if (IsSelected(view.FocusedRowHandle))
                        view.UnselectRow(view.FocusedRowHandle);
                    else
                        view.SelectRow(view.FocusedRowHandle);
                }
            }
        }

        /// <summary>
        /// 타겟 행이 현재 전체 선택되었는지 아닌지 반환
        /// </summary>
        /// <param name="view"></param>
        /// <param name="targetRowHandle"></param>
        /// <returns></returns>
        private bool IsSelected(int targetRowHandle)
        {
            GridCell[] cells = view.GetSelectedCells();
            int count = 0;

            foreach (GridCell cell in cells)
            {
                if (cell.RowHandle == targetRowHandle)
                    count++;
            }

            if (count > 5)
                return true;
            else
                return false;
        }

        /// <summary>
        /// 
        /// </summary>
        private void OpenPartMDM()
        {
            MDMCommunicator mdm = MDMCommunicator.getInstance(Common.sessionID);
            //MDM.MDMCommunicator mdm = MDM.MDMCommunicator.getInstance("nakyunv");
            if (null != mdm)
            {
                string sysID = "PCC";
                string menuID = "M212FB";
                string dictionaryID = "PART";
                string classID = "";
                string mastID = "";
                string procDefID = "PART_CINR_C_01";
                string strParam = "";

                mdm.openMDM(sysID, menuID, dictionaryID, classID, mastID, procDefID, strParam);
            }
            else
            {
                MessageBox.Show("MDM connection object acquisition failed.");
                return;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        private void MultiCheckOutsole(GridView view)
        {
            try
            {
                // 이벤트 꼬임을 방지하기 위해 CellValueChanged 이벤트 끊음
                view.CellValueChanged -= new CellValueChangedEventHandler(CustomCellValueChanged);

                int[] rowHandles = view.GetSelectedRows();

                foreach (int rowHandle in rowHandles)
                {
                    string rowStatus = view.GetRowCellValue(rowHandle, "ROW_STATUS").ToString();
                    if (rowStatus != "D")
                    {
                        string currentRowValue = view.GetRowCellValue(rowHandle, "OTSL_CHK").ToString();
                        if (currentRowValue == "Y")
                            view.SetRowCellValue(rowHandle, "OTSL_CHK", "N");
                        else
                            view.SetRowCellValue(rowHandle, "OTSL_CHK", "Y");
                    }
                    // 인디케이터를 "U"로 변경, 신규 행과 삭제할 행은 제외
                    if (rowStatus != "I" && rowStatus != "D")
                        view.SetRowCellValue(rowHandle, "ROW_STATUS", "U");
                }

                // 스타일을 새로 적용하기 위해 DataSource Refresh
                view.RefreshData();
                // 이벤트 다시 연결
                view.CellValueChanged += new CellValueChangedEventHandler(CustomCellValueChanged);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
                return;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="partType"></param>
        /// <returns></returns>
        private int ConvertBOMSection(string partType)
        {
            int bomSectionNumber = 0;

            switch (partType)
            {
                case "UPPER":
                    bomSectionNumber = 1;
                    break;

                case "SOCKLINER":
                    bomSectionNumber = 1;
                    break;

                case "LACE":
                    bomSectionNumber = 1;
                    break;

                case "KNIT":
                    bomSectionNumber = 1;
                    break;

                case "MIDSOLE":
                    bomSectionNumber = 2;
                    break;

                case "OUTSOLE":
                    bomSectionNumber = 3;
                    break;

                case "AIRBAG":
                    bomSectionNumber = 4;
                    break;

                case "PACK":
                    bomSectionNumber = 5;
                    break;

                case "OTHER":
                    bomSectionNumber = 0;
                    break;
            }

            return bomSectionNumber;
        }

        /// <summary>
        /// Aggregate 파트 추가
        /// </summary>
        private void AddAggregatePart(GridView view)
        {
            if (view == gvwSingleEdit)
            {
                DataTable dtToInsert = EliminateDuplicateMaterial(view);

                foreach (DataRow row in dtToInsert.Rows)
                {
                    DataTable grdSource = grdSingleEdit.DataSource as DataTable;

                    // Verify that there is already an aggregate part with the same key.
                    List<DataRow> list = grdSource.AsEnumerable().Where(x => x["PCX_SUPP_MAT_ID"].ToString().Equals(row["PCX_SUPP_MAT_ID"].ToString())
                        && x["PCX_COLOR_ID"].ToString().Equals(row["PCX_COLOR_ID"].ToString()) && x["PART_CD"].ToString().Contains("AGGR")).ToList();

                    if (list.Count > 0)
                        continue;
                    else
                    {
                        #region Create an aggregate part

                        DataRow newRow = grdSource.NewRow();

                        newRow["FACTORY"] = grdSource.Rows[0]["FACTORY"].ToString();
                        newRow["WS_NO"] = grdSource.Rows[0]["WS_NO"].ToString();
                        newRow["BOM_ID"] = grdSource.Rows[0]["BOM_ID"].ToString();
                        newRow["DEV_COLORWAY_ID"] = grdSource.Rows[0]["DEV_COLORWAY_ID"].ToString();
                        newRow["COLOR_VER"] = grdSource.Rows[0]["COLOR_VER"].ToString();
                        //newRow["PART_SEQ"] = CalcNextPartSequence();
                        newRow["PART_SEQ"] = "1";   // calculated when called.

                        // Get the last generated aggregate part.
                        var lastAggrPart = grdSource.AsEnumerable().LastOrDefault(x => x["PART_CD"].ToString().Contains("AGGR") && x["ROW_STATUS"].ToString() != "D");

                        string[] partCodes = new string[] { };

                        // Split the part code to get the last sequence.
                        if (lastAggrPart != null)
                            partCodes = lastAggrPart["PART_CD"].ToString().Split('-');

                        string nxtSeq = Convert.ToString((lastAggrPart != null) ? Convert.ToInt32(partCodes[1]) + 1 : 1);

                        PKG_INTG_BOM.SELECT_FOR_DIRECT_INPUT pkgSelect = new PKG_INTG_BOM.SELECT_FOR_DIRECT_INPUT();
                        pkgSelect.ARG_TYPE = "Part";
                        pkgSelect.ARG_CODE = "LEATHER AGGREGATE " + nxtSeq;
                        pkgSelect.ARG_NAME = "";
                        pkgSelect.OUT_CURSOR = string.Empty;

                        DataTable partInfo = Common.projectBaseForm.Exe_Select_PKG(pkgSelect).Tables[0];

                        newRow["PCX_PART_ID"] = partInfo.Rows[0]["NIKE_IDENTIFIER"].ToString();
                        newRow["PART_CD"] = "AGGR-" + nxtSeq;
                        newRow["PART_NAME"] = partInfo.Rows[0]["PART_NAME"].ToString();

                        // Do not input the pattern part name to the aggregate part.
                        newRow["PTRN_PART_CD"] = "";
                        newRow["PTRN_PART_NAME"] = "";

                        newRow["CS_PTRN_CD"] = newRow["PART_CD"].ToString();
                        newRow["CS_PTRN_NAME"] = newRow["PART_NAME"].ToString();

                        newRow["PART_TYPE"] = "UPPER";
                        newRow["MXSXL_NUMBER"] = row["MXSXL_NUMBER"].ToString();
                        //newRow["CS_CD"] = row["CS_CD"].ToString();
                        newRow["MCS_NUMBER"] = row["MCS_NUMBER"].ToString();
                        newRow["PCX_SUPP_MAT_ID"] = row["PCX_SUPP_MAT_ID"].ToString();
                        newRow["MAT_CD"] = row["MAT_CD"].ToString();
                        newRow["MAT_NAME"] = row["MAT_NAME"].ToString();
                        newRow["VENDOR_NAME"] = row["VENDOR_NAME"].ToString();
                        newRow["PCX_COLOR_ID"] = row["PCX_COLOR_ID"].ToString();
                        newRow["COLOR_CD"] = row["COLOR_CD"].ToString();
                        newRow["COLOR_NAME"] = row["COLOR_NAME"].ToString();
                        newRow["PCX_MAT_ID"] = row["PCX_MAT_ID"].ToString();
                        newRow["ROW_STATUS"] = "I";
                        newRow["PRVS_STATUS"] = "I";
                        newRow["COMBINE_YN"] = "N";
                        newRow["STICKER_YN"] = "N";
                        newRow["MDSL_CHK"] = "N";
                        newRow["OTSL_CHK"] = "N";

                        #endregion

                        grdSource.Rows.InsertAt(newRow, Convert.ToInt32(nxtSeq) - 1);
                    }
                }

                // Move focus to the first row.
                gvwSingleEdit.SelectCell(0, gvwSingleEdit.FocusedColumn);
                gvwSingleEdit.FocusedRowHandle = 0;
            }
        }

        /// <summary>
        /// <para>1. 유저가 선택한 가죽 자재에서 중복을 제거한다.</para>
        /// <para>2. 중복 기준 : PCX SUPP MAT ID + PCX COLOR ID</para> 
        /// </summary>
        /// <param name="view"></param>
        /// <returns></returns>
        private DataTable EliminateDuplicateMaterial(GridView view)
        {
            // Datasource which has no duplication.
            DataTable dataSource = new DataTable();
            Dictionary<string, string> keys = new Dictionary<string, string>();

            if (view == gvwSingleEdit)
                dataSource = ((DataTable)grdSingleEdit.DataSource).Clone();
            else if (view == gvwMultipleEdit)
                dataSource = ((DataTable)grdMultipleEdit.DataSource).Clone();

            foreach (int rowHandle in view.GetSelectedRows())
            {
                DataRow row = view.GetDataRow(rowHandle);

                string suppMatID = row["PCX_SUPP_MAT_ID"].ToString();
                string colorID = row["PCX_COLOR_ID"].ToString();

                if (keys.Count == 0)
                {
                    // When the dictionary is empty.
                    // Key : PCX Supp. Mat. ID + Color ID
                    keys.Add(suppMatID + colorID, "Y");
                    dataSource.ImportRow(row);
                }
                else
                {
                    // Verify that the same key exists.
                    if (keys.ContainsKey(suppMatID + colorID))
                        continue;
                    else
                    {
                        keys.Add(suppMatID + colorID, "Y");
                        dataSource.ImportRow(row);
                    }
                }
            }

            return dataSource;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="view"></param>
        private void ShowStockInformation(GridView view)
        {
            List<string> pairs = new List<string>();

            foreach (int rowHandle in gvwSingleEdit.GetSelectedRows())
            {
                pairs.Add(
                    String.Format("{0}${1}",
                    gvwSingleEdit.GetRowCellValue(rowHandle, "MAT_NAME").ToString(),
                    gvwSingleEdit.GetRowCellValue(rowHandle, "COLOR_CD").ToString())
                    );
            }

            object returnObj = Common.projectBaseForm.OpenChildForm(@"\POPUP\CSI.PCC.PURCHASE.P_PCC_Stock_MatList.dll",
                pairs, JPlatform.Client.Library.interFace.OpenType.Modal);
        }

        /// <summary>
        /// Check required parts are existing on this BOM.
        /// </summary>
        private bool CanPassNCFRule()
        {
            PKG_INTG_BOM.SELECT_NCF_RULE_PASS pkgSelect = new PKG_INTG_BOM.SELECT_NCF_RULE_PASS();
            pkgSelect.ARG_WORK_TYPE = "NCFRuleCheck";
            pkgSelect.ARG_FACTORY = Factory;
            pkgSelect.ARG_WS_NO = WSNumber;
            pkgSelect.OUT_CURSOR = string.Empty;

            DataTable dataSource = Common.projectBaseForm.Exe_Select_PKG(pkgSelect).Tables[0];

            if (dataSource.Rows.Count > 0)
            {
                string type = dataSource.Rows[0]["ERROR_CODE"].ToString();

                if (type == "")
                    return true;
                else
                {
                    string[] errorCodes = type.Split('_');
                    string errorMessage = "<Please check required parts.>\n\n";

                    foreach (string code in errorCodes)
                    {
                        if (code == "THREAD")
                            errorMessage += "-> Please add 'THREAD' part or check whether the pcx material id of this part is '83694'.\n\n";
                        else if (code == "CEMENT")
                            errorMessage += "-> Please add 'UPPER CEMENT' part or check whether the pcx material id of this part is '78312'.\n\n";
                        else if (code == "CHEMISTRY")
                            errorMessage += "-> Please add 'WET CHEMISTRY' part.\n\n";
                        else if (code == "CARTON")
                            errorMessage += "-> Please add 'OUTER CARTON' part or check whether the pcx material id of this part is '101347'.\n\n";
                        else if (code == "SUBLIMATION")
                            errorMessage += "-> Please replace '54650'(SUBLIMATION PRINTING LOGO)' with 175225'(Outsourced Material Code).";
                    }

                    errorMessage += "\n\n" + "** Do you want to proceed?";

                    if (MessageBox.Show(errorMessage, "",
                        MessageBoxButtons.OKCancel, MessageBoxIcon.Warning, MessageBoxDefaultButton.Button1) == System.Windows.Forms.DialogResult.OK)
                        return true;
                    else
                        return false;
                }
            }
            else
            {
                MessageBox.Show("Failed to load NCF rule data.", "",
                    MessageBoxButtons.OK, MessageBoxIcon.Error, MessageBoxDefaultButton.Button1);
                return false;
            }
        }

        /// <summary>
        /// 셀에 유저가 입력한 컬러를 배경색으로 표기한다.
        /// </summary>
        /// <param name="view"></param>
        private void HighlightCell()
        {
            // 새 컬러 선택
            ColorDialog dialog = new ColorDialog();
            dialog.AllowFullOpen = false;
            dialog.FullOpen = false;
            dialog.ShowHelp = false;
            dialog.Color = Color.White;

            if (dialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                foreach (GridCell cell in view.GetSelectedCells())
                {
                    Dictionary<string, string> colorList = new Dictionary<string, string>();

                    /*************************************************
                     * <컬러 포맷>
                     * 컬럼명1/컬러1,컬럼명2/컬러2,컬럼명3/컬러3,...
                    *************************************************/

                    string columnName = cell.Column.FieldName;
                    string colorFormat = view.GetRowCellValue(cell.RowHandle, "CELL_COLOR").ToString();

                    if (colorFormat.Length > 0)
                    {
                        /* 현 행에 이미 적용된 컬러 포맷이 있는 경우 */

                        string[] items = colorFormat.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);

                        foreach (string item in items)
                        {
                            /* 컬러 마스터를 만듦 */

                            string[] tmp = item.Split('/');
                            string _columnName = tmp[0];
                            string _colorName = tmp[1];

                            colorList.Add(_columnName, _colorName);
                        }

                        if (colorList.ContainsKey(columnName))  // 현 컬럼에 이미 등록된 컬러가 있는 경우 기존 것 삭제
                            colorList.Remove(columnName);
                    }

                    string selectedColor = dialog.Color.ToArgb().ToString();

                    if (selectedColor != "-1")                      // 흰색 선택 시 배경 삭제
                        colorList.Add(columnName, selectedColor);   // 신규 컬러 등록

                    colorFormat = string.Empty; // 새로운 컬러 포맷을 만들기 위해 초기화

                    foreach (KeyValuePair<string, string> kvp in colorList)
                    {
                        /* 신규 포맷 생성 */

                        string pair = kvp.Key + "/" + kvp.Value;
                        colorFormat += pair + ",";
                    }

                    view.CellValueChanged -= new CellValueChangedEventHandler(CustomCellValueChanged);
                    view.SetRowCellValue(cell.RowHandle, "CELL_COLOR", colorFormat);    // 신규 포맷 등록
                    view.CellValueChanged += new CellValueChangedEventHandler(CustomCellValueChanged);
                }
            }
        }

        /// <summary>
        /// 입력된 CS 패턴 파트와 동일하게 Nike 패턴 파트에 자동 입력한다.
        /// </summary>
        private void CopyFromCSPatternPart(string type)
        {
            DataTable dataSource = (view == gvwSingleEdit) ? grdSingleEdit.DataSource as DataTable : grdMultipleEdit.DataSource as DataTable;

            view.CellValueChanged -= new CellValueChangedEventHandler(CustomCellValueChanged);

            switch (type)
            {
                case "New":

                    List<string> exceptionList = new List<string> {
                        "79341", "54638", "85643", "79158", "85644", "58374", "85406", "175225" };

                    // Group by CS Pattern Part Name.
                    var queryByCSPattern =
                        from row in dataSource.AsEnumerable()
                        group row by new
                        {
                            FACTORY = row.Field<string>("FACTORY"),
                            WS_NO = row.Field<string>("WS_NO"),
                            CS_PTRN_NAME = row.Field<string>("CS_PTRN_NAME"),
                        }
                            into rowGroup
                            // Consider only materials bound to more than one pattern part.
                            where rowGroup.Count() > 1
                            select rowGroup;

                    foreach (var package in queryByCSPattern)
                    {
                        bool isExcept = false;
                        bool isEmpty = false;

                        foreach (var row in package)
                        {
                            if (exceptionList.Contains(row["PCX_MAT_ID"].ToString()))
                                isExcept = true;
                            else if (row["PCX_SUPP_MAT_ID"].ToString().Equals(""))
                                isEmpty = true;
                        }

                        // There are materials with no PCX Supp. Mat. ID.
                        if (isEmpty)
                            continue;
                        // General exception case. (in case of combine check)
                        else if (package.Count() == 2 && isExcept)
                            continue;
                        else
                        {
                            foreach (var row in package)
                            {
                                string rowStatus = row["ROW_STATUS"].ToString();
                                string lockYN = row["LOCK_YN"].ToString();

                                // In case of 'Lamination' + 'Combine'.
                                if (row["COMBINE_YN"].ToString().Equals("Y"))
                                    continue;

                                if (!rowStatus.Equals("D") && !lockYN.Equals("Y"))
                                {
                                    // Update when Nike Pattern Part Name is different from CS Pattern Part Name.
                                    if (row["PTRN_PART_NAME"].ToString() != row["CS_PTRN_NAME"].ToString())
                                    {
                                        row["PTRN_PART_NAME"] = row["CS_PTRN_NAME"].ToString();
                                        row["PTRN_PART_CD"] = row["CS_PTRN_CD"].ToString();

                                        // Update row indicator.
                                        if (rowStatus != "I")
                                            row["ROW_STATUS"] = "U";
                                    }
                                }
                            }
                        }
                    }

                    break;

                case "Old":

                    // Extract rows whose part name includes 'Lamination'.
                    List<DataRow> rows = dataSource.AsEnumerable().Where(x => x["PART_NAME"].ToString().Contains("LAMINATION") == true).ToList();

                    foreach (DataRow row in rows)
                    {
                        string rowStatus = row["ROW_STATUS"].ToString();
                        string lockYN = row["LOCK_YN"].ToString();

                        if (!rowStatus.Equals("D") && !lockYN.Equals("Y"))
                        {
                            // Do When Nike Pattern Part and CS Pattern Part are different.
                            if (row["PTRN_PART_NAME"].ToString() != row["CS_PTRN_NAME"].ToString())
                            {
                                row["PTRN_PART_NAME"] = row["CS_PTRN_NAME"].ToString();
                                row["PTRN_PART_CD"] = row["CS_PTRN_CD"].ToString();

                                // Update indicator of row status.
                                if (rowStatus != "I")
                                    row["ROW_STATUS"] = "U";
                            }
                            else
                                continue;
                        }
                    }

                    break;
            }

            view.RefreshData();
            view.CellValueChanged += new CellValueChangedEventHandler(CustomCellValueChanged);
        }

        /// <summary>
        /// BlackList 자재 정보 확인을 위한 신규 팝업 표기
        /// </summary>
        /// <param name="view"></param>
        private void ShowBlackListMaterialInfo()
        {
            object returnObj = Common.projectBaseForm.OpenChildForm(@"\POPUP\CSI.PCC.PMM.P_BlackList.dll",
                view.GetFocusedRowCellValue("MAT_CD").ToString(), JPlatform.Client.Library.interFace.OpenType.Modal);
        }

        /// <summary>
        /// BOM 데이터를 엑셀로 출력한다.
        /// </summary>
        private void ExportBOMToExcel()
        {
            Stream myStream;
            SaveFileDialog dialog = new SaveFileDialog();

            dialog.Title = "Please specify the path you want to save.";
            dialog.Filter = "Excel File (*.xls)|*.xls|Excel File (*.xlsx)|*.xlsx";

            // 기본 파일명
            dialog.FileName = "BOM Data";

            dialog.OverwritePrompt = true;

            if (dialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                if ((myStream = dialog.OpenFile()) != null)
                {
                    grdSingleEdit.ExportToXls(myStream);
                    myStream.Close();
                }
            }
        }

        /// <summary>
        /// 
        /// </summary>
        private void ShowCommentForm()
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
                        view.SetRowCellValue(rowHandle, "MAT_COMMENT", form.Comment);
                        view.SetRowCellValue(rowHandle, "ENCODED_CMT", form.EncodedComment);

                        if (view.GetRowCellValue(rowHandle, "ROW_STATUS").ToString() != "I")
                            view.SetRowCellValue(rowHandle, "ROW_STATUS", "U");
                    }
                }
                finally
                {
                    view.CellValueChanged += new CellValueChangedEventHandler(CustomCellValueChanged);
                }
            }
        }

        #endregion

        #region Grid Events

        private void CustomCellValueChanged(object sender, CellValueChangedEventArgs e)
        {
            try
            {
                // for part info.
                string partCode = string.Empty;
                string partName = string.Empty;
                string partType = string.Empty;
                string pcxPartId = string.Empty;

                // for material info.
                string mxsxlNumber = string.Empty;
                string pcxSuppMatID = string.Empty;
                //string csCode = string.Empty;
                string mcsNumber = string.Empty;
                string pcxMatID = string.Empty;
                string pdmMatCode = string.Empty;
                string pdmMatName = string.Empty;
                string vendorName = string.Empty;
                string nikeMSState = string.Empty;
                string blackListYN = "N";
                string mdmNotExisting = "N";

                // for color info.
                string pcxColorID = string.Empty;
                string pdmColorCode = string.Empty;
                string pdmColorName = string.Empty;

                // To avoid infinite loop.
                view.CellValueChanged -= new CellValueChangedEventHandler(CustomCellValueChanged);

                var currValue = view.ActiveEditor.EditValue;
                var oldValue = view.ActiveEditor.OldEditValue;

                // If there are no changes, finish event.
                if (currValue.ToString() == oldValue.ToString())
                    return;

                string value = currValue.ToString();

                if (view.FocusedColumn.FieldName == "COLOR_CD")
                {
                    #region PDM 컬러코드 다이렉트 입력 시, 다른 속성 자동 입력을 위해 코드 정보를 가져옴

                    if (value != "")
                    {
                        PKG_INTG_BOM.SELECT_FOR_DIRECT_INPUT pkgSelect = new PKG_INTG_BOM.SELECT_FOR_DIRECT_INPUT();
                        pkgSelect.ARG_TYPE = "Color";
                        pkgSelect.ARG_CODE = value;
                        pkgSelect.ARG_NAME = "";
                        pkgSelect.OUT_CURSOR = string.Empty;

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
                        if (CSBOMStatus != "F")
                        {
                            PKG_INTG_BOM.SELECT_FOR_DIRECT_INPUT pkgSelect = new PKG_INTG_BOM.SELECT_FOR_DIRECT_INPUT();
                            pkgSelect.ARG_TYPE = "Color";
                            pkgSelect.ARG_CODE = "";
                            pkgSelect.ARG_NAME = value;
                            pkgSelect.OUT_CURSOR = string.Empty;

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
                    }

                    #endregion
                }
                else if (view.FocusedColumn.FieldName == "PART_NAME")
                {
                    #region Part Name 다이렉트 입력 시, 다른 속성 자동 입력을 위해 코드 정보를 가져옴

                    if (value != "")
                    {
                        if (CSBOMStatus != "F")
                        {
                            PKG_INTG_BOM.SELECT_FOR_DIRECT_INPUT pkgSelect = new PKG_INTG_BOM.SELECT_FOR_DIRECT_INPUT();
                            pkgSelect.ARG_TYPE = "Part";
                            pkgSelect.ARG_CODE = value;
                            pkgSelect.ARG_NAME = "";
                            pkgSelect.OUT_CURSOR = string.Empty;

                            DataTable dataSource = Common.projectBaseForm.Exe_Select_PKG(pkgSelect).Tables[0];

                            if (dataSource.Rows.Count > 0)
                            {
                                partCode = dataSource.Rows[0][0].ToString();
                                partName = dataSource.Rows[0][1].ToString();
                                partType = dataSource.Rows[0][2].ToString();
                                pcxPartId = dataSource.Rows[0][3].ToString();
                            }
                            else
                            {
                                partCode = "";
                                partName = "";
                                partType = "";
                                pcxPartId = "";
                            }
                        }
                        else
                        {
                            // For the Fake BOM, type it manually.
                            partName = value;
                        }
                    }

                    #endregion
                }
                else if (view.FocusedColumn.FieldName == "PTRN_PART_NAME")
                {
                    #region Part Name 다이렉트 입력 시, 다른 속성 자동 입력을 위해 코드 정보를 가져옴
                    if (value != "")
                    {
                        if (CSBOMStatus != "F")
                        {
                            PKG_INTG_BOM.SELECT_FOR_DIRECT_INPUT pkgSelect = new PKG_INTG_BOM.SELECT_FOR_DIRECT_INPUT();
                            pkgSelect.ARG_TYPE = "Part";
                            pkgSelect.ARG_CODE = value;
                            pkgSelect.ARG_NAME = "";
                            pkgSelect.OUT_CURSOR = string.Empty;

                            DataTable dataSource = Common.projectBaseForm.Exe_Select_PKG(pkgSelect).Tables[0];

                            if (dataSource.Rows.Count > 0)
                            {
                                partCode = dataSource.Rows[0][0].ToString();
                                partName = dataSource.Rows[0][1].ToString();
                                partType = dataSource.Rows[0][2].ToString();
                                pcxPartId = dataSource.Rows[0][3].ToString();
                            }
                            else
                            {
                                partCode = "";
                                partName = "";
                                partType = "";
                                pcxPartId = "";
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
                    #region MXSXL Number 다이렉트 입력 시, 다른 속성 자동 입력을 위해 코드 정보를 가져옴

                    if (value != "")
                    {
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
                            nikeMSState = dataSource.Rows[0]["NIKE_MS_STATE"].ToString();
                            //csCode = dataSource.Rows[0]["CS_CD"].ToString();
                            blackListYN = dataSource.Rows[0]["BLACK_LIST_YN"].ToString();
                            mdmNotExisting = dataSource.Rows[0]["MDM_NOT_EXST"].ToString();
                        }
                        else
                        {
                            pcxMatID = "100";
                            pcxSuppMatID = "100";
                            pdmMatName = "PLACEHOLDER";
                            mdmNotExisting = "Y";

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
                            if (value.ToUpper().Equals("PLACEHOLDER") || value.ToUpper().Equals("N/A"))
                            {
                                bool isPlaceholder = value.ToUpper().Equals("PLACEHOLDER");

                                pcxMatID = isPlaceholder ? "100" : "999";
                                pcxSuppMatID = isPlaceholder ? "100" : "999";
                                pdmMatName = isPlaceholder ? "PLACEHOLDER" : "N/A";
                                mdmNotExisting = "Y";
                            }
                            else
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
                                    nikeMSState = dataSource.Rows[0]["NIKE_MS_STATE"].ToString();
                                    //csCode = dataSource.Rows[0]["CS_CD"].ToString();
                                    blackListYN = dataSource.Rows[0]["BLACK_LIST_YN"].ToString();
                                    mdmNotExisting = dataSource.Rows[0]["MDM_NOT_EXST"].ToString();
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
                                        nikeMSState = results[9];
                                        //csCode = results[7];
                                        blackListYN = results[10];
                                        mdmNotExisting = results[11];
                                    }
                                }
                                else
                                {
                                    pcxMatID = "100";
                                    pcxSuppMatID = "100";
                                    pdmMatName = "PLACEHOLDER";
                                    mdmNotExisting = "Y";

                                    Common.ShowMessageBox("Unregistered code.", "E");
                                }
                            }
                        }
                        else
                        {
                            // When this is fake bom.
                            pdmMatName = value;
                        }
                    }

                    #endregion
                }

                List<DataRow> rows = new List<DataRow>();

                foreach (int rowHandle in view.GetSelectedRows())
                    rows.Add(view.GetDataRow(rowHandle));

                foreach (DataRow row in rows)
                {
                    string rowStatus = row["ROW_STATUS"].ToString();
                    string lockYN = row["LOCK_YN"].ToString();

                    if (!rowStatus.Equals("D") && !lockYN.Equals("Y"))
                    {
                        switch (view.FocusedColumn.FieldName)
                        {
                            case "COLOR_CD":
                            case "COLOR_NAME":

                                row["PCX_COLOR_ID"] = pcxColorID;
                                row["COLOR_CD"] = pdmColorCode;
                                row["COLOR_NAME"] = pdmColorName;
                                break;

                            case "PART_NAME":

                                // When the part name is changed from 'Lamination' to general part.
                                if (oldValue.ToString().Contains("LAMINATION") && !partName.Contains("LAMINATION"))
                                {
                                    // An user doesn't need to set nike pattern part name for general part.
                                    row["PTRN_PART_NAME"] = "";
                                    row["PTRN_PART_CD"] = "";
                                }

                                row["PART_NAME"] = partName;
                                row["PART_CD"] = partCode;

                                // Set part type by each case.
                                if (partName.Contains("AIRBAG"))
                                {
                                    // If the part name contains 'AIRBAG', fix BOM section to 'AIRBAG'.
                                    row["PART_TYPE"] = "AIRBAG";
                                    row["MDSL_CHK"] = "Y";
                                    row["OTSL_CHK"] = "N";
                                }
                                else if (partName.Contains("LAMINATION"))
                                {
                                    // If the part name contains 'LAMINATION', fix BOM section to 'UPPER'.
                                    row["PART_TYPE"] = "UPPER";
                                    row["MDSL_CHK"] = "N";
                                    row["OTSL_CHK"] = "N";
                                }
                                else if (partType == "MIDSOLE" || partType == "OUTSOLE")
                                {
                                    // If the part type is 'MIDSOLE' or 'OUTSOLE', fix BOM section to it.
                                    row["PART_TYPE"] = partType;
                                    row["MDSL_CHK"] = (partType == "MIDSOLE") ? "Y" : "N";
                                    row["OTSL_CHK"] = (partType == "OUTSOLE") ? "Y" : "N";
                                }
                                else if (pcxPartId == "3389" || pcxPartId == "3390")
                                {
                                    // Fix BOM section to 'UPPER' for WET CHEMISTRY & WET CHEMISTRY-MULTI parts.
                                    row["PART_TYPE"] = "UPPER";
                                    row["MDSL_CHK"] = "N";
                                    row["OTSL_CHK"] = "N";
                                }
                                else
                                {
                                    // Follow existing BOM section(When a row is created, the BOM section follows the upper row).
                                    row["PART_TYPE"] = partType;
                                    row["MDSL_CHK"] = "N";
                                    row["OTSL_CHK"] = "N";
                                }

                                row["PCX_PART_ID"] = pcxPartId;

                                Common.BindDefaultMaterialByNCFRule(view, row, pcxPartId);

                                break;

                            case "PTRN_PART_NAME":

                                row["PTRN_PART_NAME"] = partName;
                                row["PTRN_PART_CD"] = partCode;
                                break;

                            case "MXSXL_NUMBER":
                            case "MAT_NAME":

                                if (pcxMatID == "87031" || pcxMatID == "78733" || pcxMatID == "79467")
                                    row["STICKER_YN"] = "Y";    // Lamination sticker.
                                else if (pcxMatID == "54638" || pcxMatID == "79341")
                                    row["COMBINE_YN"] = "Y";    // Transfer logo.
                                else
                                    row["STICKER_YN"] = "N";

                                row["MXSXL_NUMBER"] = mxsxlNumber;
                                row["PCX_SUPP_MAT_ID"] = pcxSuppMatID;
                                //row["CS_CD"] = csCode;
                                row["MCS_NUMBER"] = mcsNumber;
                                row["PCX_MAT_ID"] = pcxMatID;
                                row["MAT_CD"] = pdmMatCode;
                                row["MAT_NAME"] = pdmMatName;
                                row["VENDOR_NAME"] = vendorName;
                                row["NIKE_MS_STATE"] = nikeMSState;
                                row["BLACK_LIST_YN"] = blackListYN;
                                row["MDM_NOT_EXST"] = mdmNotExisting;

                                // Automatically tick for 'Code Transfer'.
                                if (mdmNotExisting.Equals("Y"))
                                    row["CDMKR_YN"] = "Y";
                                else
                                    row["CDMKR_YN"] = "N";

                                break;

                            case "MAT_COMMENT":

                                row["MAT_COMMENT"] = value;

                                // When an user manually types data into material comment,
                                // clear the encoded comment to avoid discrepancy.
                                row["ENCODED_CMT"] = "";
                                break;

                            default:

                                row[view.FocusedColumn.FieldName] = value;
                                break;
                        }

                        if (rowStatus != "I")
                            row["ROW_STATUS"] = "U";
                    }
                }

                //view.RefreshData();
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

        private void CustomKeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Delete)
            {
                List<DataRow> rows = new List<DataRow>();
                string[] colNonModifySingle = new string[] { "PART_NIKE_NO", "UPD_USER", "UPD_YMD" };
                string[] colNonModifyMultiple = new string[] { "DEV_COLORWAY_ID", "PART_NIKE_NO", "UPD_USER" };
                string[] colModifyLocked = new string[] { "MAT_COMMENT", "COLOR_COMMENT", "PROCESS", "REMARKS" };
                string fieldName = string.Empty;
                string rowStatus = string.Empty;
                string lockYN = string.Empty;

                foreach (int rowHandle in view.GetSelectedRows())
                    rows.Add(view.GetDataRow(rowHandle));

                try
                {
                    view.CellValueChanged -= new CellValueChangedEventHandler(CustomCellValueChanged);

                    fieldName = view.FocusedColumn.FieldName;

                    // Validate the focus is valid.
                    if (view.Name.Equals("gvwSingleEdit"))
                    {
                        if (colNonModifySingle.Contains(fieldName))
                            return;

                        if (isLocked)
                        {
                            if (!colModifyLocked.Contains(fieldName))
                                return;
                        }
                    }
                    else if (view.Name.Equals("gvwMultipleEdit"))
                    {
                        if (colNonModifyMultiple.Contains(fieldName))
                            return;
                    }

                    foreach (DataRow row in rows)
                    {
                        rowStatus = row["ROW_STATUS"].ToString();
                        lockYN = row["LOCK_YN"].ToString();

                        if (!rowStatus.Equals("D") && !lockYN.ToString().Equals("Y"))
                        {
                            switch (fieldName)
                            {
                                case "PTRN_PART_NAME":

                                    row["PTRN_PART_CD"] = "";
                                    row["PTRN_PART_NAME"] = "";
                                    break;

                                case "CS_PTRN_NAME":

                                    row["CS_PTRN_CD"] = "";
                                    row["CS_PTRN_NAME"] = "";
                                    break;

                                case "MAT_COMMENT":

                                    row["MAT_COMMENT"] = "";
                                    row["ENCODED_CMT"] = "";
                                    break;

                                default:
                                    row[fieldName] = "";
                                    break;
                            }

                            if (!rowStatus.Equals("I"))
                                row["ROW_STATUS"] = "U";
                        }
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
        }

        private void CustomShowingEditor(object sender, CancelEventArgs e)
        {
            GridView view = sender as GridView;

            if (view.GetRowCellValue(view.FocusedRowHandle, "ROW_STATUS").ToString().Equals("D") ||
                view.GetRowCellValue(view.FocusedRowHandle, "LOCK_YN").ToString().Equals("Y"))
                e.Cancel = true;
        }

        private void CustomMouseDown(object sender, MouseEventArgs e)
        {
            try
            {
                // GridHitInfo : Contains information about a specific point within a Grid View.
                GridHitInfo hitInfo = view.CalcHitInfo(e.Location);

                // InRowCell : Gets a value indicating whether the test point is within a cell.
                if (hitInfo.InRowCell)
                {
                    // RealColumnEdit : Gets the repository item that actually represents the column's editor.
                    if (hitInfo.Column.RealColumnEdit is DevExpress.XtraEditors.Repository.RepositoryItemCheckEdit)
                    {
                        view.ClearSelection();
                        view.FocusedColumn = hitInfo.Column;
                        view.FocusedRowHandle = hitInfo.RowHandle;

                        // ShowEditor : Creates an editor for the cell.
                        view.ShowEditor();

                        // ActiveEditor : Gets a View's active editor.
                        CheckEdit edit = view.ActiveEditor as CheckEdit;

                        if (edit == null)
                            return;

                        // 행의 인디케이터에 따라 체크 표기 여부 선택
                        string rowStatus = view.GetRowCellValue(view.FocusedRowHandle, "ROW_STATUS").ToString();

                        if (rowStatus != "D")
                        {
                            edit.Toggle();

                            if (rowStatus != "I" && (view.FocusedColumn.FieldName != "CDMKR_YN"))
                                view.SetRowCellValue(view.FocusedRowHandle, "ROW_STATUS", "U");
                        }

                        DXMouseEventArgs.GetMouseArgs(e).Handled = true;
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }

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

        private void CustomRowCellStyle(object sender, RowCellStyleEventArgs e)
        {
            GridView view = sender as GridView;

            #region BOM이 LOCK된 경우 별도의 스타일 적용

            if (isLocked)
            {
                string[] editableColumns = new string[] { "MAT_COMMENT", "COLOR_COMMENT", "PROCESS", "REMARKS", "CDMKR_YN" };

                if (!editableColumns.Contains(e.Column.FieldName))
                {
                    if (!view.IsCellSelected(e.RowHandle, e.Column))
                        e.Appearance.BackColor = Color.LightSlateGray;
                }

                return;
            }

            #endregion

            #region 삭제된 행의 스타일 적용 - 스타일 적용의 가장 우선 순위

            string rowStatus = view.GetRowCellValue(e.RowHandle, "ROW_STATUS").ToString();

            if (rowStatus == "D")
            {
                if (view.IsCellSelected(e.RowHandle, e.Column) == false)
                {
                    e.Appearance.BackColor = Color.DarkGray;

                    // 멀티 수정 모드의 경우 기준 라인의 글자를 볼드-흰색으로 표시
                    if (view == gvwMultipleEdit)
                    {
                        string colorVersion = view.GetRowCellValue(e.RowHandle, "COLOR_VER").ToString();

                        if (colorVersion == "VER1")
                        {
                            e.Appearance.Font = new Font(e.Appearance.Font, FontStyle.Bold);
                            e.Appearance.ForeColor = Color.White;
                        }
                    }

                    // 삭제의 경우 다른 스타일 무시
                    return;
                }
            }

            #endregion

            #region 멀티 수정 모드의 경우 기준 라인 스타일 적용

            if (view == gvwMultipleEdit)
            {
                if (view.GetRowCellValue(e.RowHandle, "COLOR_VER").ToString().Equals("VER1"))
                {
                    if (view.IsCellSelected(e.RowHandle, e.Column) == false)
                    {
                        e.Appearance.BackColor = Color.Tan;
                        e.Appearance.Font = new Font(e.Appearance.Font, FontStyle.Bold);
                    }
                    return;
                }
            }

            #endregion

            if (e.Column.FieldName == "PCX_PART_ID")
            {
                #region PCX Part ID 공란
                string pcxPartId = view.GetRowCellValue(e.RowHandle, "PCX_PART_ID").ToString();
                if (pcxPartId == "")
                {
                    if (view.IsCellSelected(e.RowHandle, e.Column) == false)
                        e.Appearance.BackColor = Color.DeepSkyBlue;
                }
                #endregion
            }
            else if (e.Column.FieldName == "PART_CD")
            {
                #region 파트 코드 공란
                string partCode = view.GetRowCellValue(e.RowHandle, "PART_CD").ToString();
                if (partCode == "")
                {
                    if (view.IsCellSelected(e.RowHandle, e.Column) == false)
                        e.Appearance.BackColor = Color.Red;
                }
                #endregion
            }
            else if (e.Column.FieldName == "PART_NAME")
            {
                #region PCX 라이브러리 매칭 실패
                string partName = view.GetRowCellValue(e.RowHandle, "PART_NAME").ToString();

                if (partName == "Failed to load part info. from MDM." ||
                    partName == "Failed to load ptrn. part info. from MDM.")
                {
                    if (view.IsCellSelected(e.RowHandle, e.Column) == false)
                    {
                        e.Appearance.ForeColor = Color.Red;
                        e.Appearance.Font = new Font(e.Appearance.Font, FontStyle.Bold);
                    }
                }
                #endregion

                #region 파트 묶음에 다른 파트명이 존재할 경우 스타일 적용
                if (view == gvwMultipleEdit)
                {
                    // 기준 라인은 건너뜀
                    int remainder = e.RowHandle % NumSelectedBOMs;
                    if (remainder != 0)
                    {
                        // 본 파트명
                        string now = view.GetRowCellValue(e.RowHandle, "PART_NAME").ToString();
                        // 기준 라인의 파트명으로 비교
                        string comparer = view.GetRowCellValue(e.RowHandle - remainder, "PART_NAME").ToString();

                        if (now != comparer)
                        {
                            if (view.IsCellSelected(e.RowHandle, e.Column) == false)
                            {
                                e.Appearance.BackColor = Color.Red;
                                e.Appearance.ForeColor = Color.White;
                            }
                        }
                    }
                }
                #endregion

                #region Replace 표기
                if (view == gvwSingleEdit)
                {
                    string matchedStatus = view.GetRowCellValue(e.RowHandle, "STATUS").ToString();

                    if (matchedStatus == "R")
                    {
                        if (view.IsCellSelected(e.RowHandle, e.Column) == false)
                        {
                            e.Appearance.BackColor = Color.LightGreen;
                            //e.Appearance.ForeColor = Color.White;
                        }
                    }
                    else if (matchedStatus == "T")
                    {
                        if (view.IsCellSelected(e.RowHandle, e.Column) == false)
                        {
                            e.Appearance.BackColor = Color.LightSalmon;
                            //e.Appearance.ForeColor = Color.White;
                        }
                    }
                }
                #endregion

                #region Lamination
                if (partName.Contains("LAMINATION") == true)
                {
                    if (view.IsCellSelected(e.RowHandle, e.Column) == false)
                    {
                        e.Appearance.ForeColor = Color.LimeGreen;
                        e.Appearance.Font = new Font(e.Appearance.Font, FontStyle.Bold);
                    }
                }

                #endregion
            }
            else if (e.Column.FieldName == "PTRN_PART_NAME")
            {
                #region 패턴 파트 묶음에 다른 파트명이 존재할 경우 스타일 적용

                if (view == gvwMultipleEdit)
                {
                    int remainder = e.RowHandle % NumSelectedBOMs;

                    if (remainder != 0)
                    {
                        string now = view.GetRowCellValue(e.RowHandle, "PTRN_PART_NAME").ToString();
                        string comparer = view.GetRowCellValue(e.RowHandle - remainder, "PTRN_PART_NAME").ToString();

                        if (now != comparer)
                        {
                            if (view.IsCellSelected(e.RowHandle, e.Column) == false)
                            {
                                e.Appearance.BackColor = Color.Red;
                                e.Appearance.ForeColor = Color.White;
                            }
                        }
                    }
                }

                #endregion
            }
            else if (e.Column.FieldName == "CS_PTRN_NAME")
            {
                #region Fake BOM의 경우 패턴 파트 필수 입력

                string ptrnPartName = view.GetRowCellValue(e.RowHandle, "CS_PTRN_NAME").ToString();

                if (CSBOMStatus == "F")
                {
                    if (ptrnPartName == "")
                    {
                        if (view.IsCellSelected(e.RowHandle, e.Column) == false)
                            e.Appearance.BackColor = Color.Red;
                    }
                }

                #endregion

                #region 패턴 파트 묶음에 다른 파트명이 존재할 경우 스타일 적용

                if (view == gvwMultipleEdit)
                {
                    int remainder = e.RowHandle % NumSelectedBOMs;

                    if (remainder != 0)
                    {
                        string now = view.GetRowCellValue(e.RowHandle, "CS_PTRN_NAME").ToString();
                        string comparer = view.GetRowCellValue(e.RowHandle - remainder, "CS_PTRN_NAME").ToString();

                        if (now != comparer)
                        {
                            if (view.IsCellSelected(e.RowHandle, e.Column) == false)
                            {
                                e.Appearance.BackColor = Color.Red;
                                e.Appearance.ForeColor = Color.White;
                            }
                        }
                    }
                }

                #endregion
            }
            else if (e.Column.FieldName == "PART_TYPE")
            {
                #region 유효한 파트 타입 여부 확인

                if (CSBOMStatus == "F")
                {
                    if (view.GetRowCellValue(e.RowHandle, "PART_TYPE").ToString().Equals(""))
                    {
                        if (view.IsCellSelected(e.RowHandle, e.Column) == false)
                            e.Appearance.BackColor = Color.Red;
                    }
                }
                else
                {
                    string[] validPartTypes = new string[] { "UPPER", "MIDSOLE", "OUTSOLE", "PACKAGING", "AIRBAG" };
                    string partType = view.GetRowCellValue(e.RowHandle, "PART_TYPE").ToString();

                    if (partType == "" || !validPartTypes.Contains(partType))
                    {
                        if (view.IsCellSelected(e.RowHandle, e.Column) == false)
                            e.Appearance.BackColor = Color.DeepSkyBlue;
                    }
                }

                #endregion
            }
            else if (e.Column.FieldName.Equals("PCX_MAT_ID") || e.Column.FieldName.Equals("PCX_SUPP_MAT_ID"))
            {
                #region When a code is 'PLACEHOLDER' or 'NA'.

                switch (view.GetRowCellValue(e.RowHandle, e.Column.FieldName).ToString())
                {
                    case "100":

                        if (view.IsCellSelected(e.RowHandle, e.Column) == false)
                        {
                            e.Appearance.ForeColor = Color.Red;
                            e.Appearance.Font = new Font(e.Appearance.Font, FontStyle.Bold);
                        }

                        break;

                    case "999":

                        if (view.IsCellSelected(e.RowHandle, e.Column) == false)
                        {
                            e.Appearance.ForeColor = Color.Red;
                            e.Appearance.Font = new Font(e.Appearance.Font, FontStyle.Bold);
                        }

                        break;

                    case "":

                        if (view.IsCellSelected(e.RowHandle, e.Column) == false)
                        {
                            e.Appearance.BackColor = Color.DeepSkyBlue;
                            //e.Appearance.Font = new Font(e.Appearance.Font, FontStyle.Bold);
                        }

                        break;
                }

                #endregion
            }
            else if (e.Column.FieldName == "MAT_NAME")
            {
                #region 주요 속성 값 표기

                string materialName = view.GetRowCellValue(e.RowHandle, "MAT_NAME").ToString();
                string pcxMatID = view.GetRowCellValue(e.RowHandle, "PCX_MAT_ID").ToString();

                if (materialName.Equals("PLACEHOLDER") || materialName.Equals("N/A"))
                {
                    if (view.IsCellSelected(e.RowHandle, e.Column) == false)
                    {
                        e.Appearance.ForeColor = Color.Red;
                        e.Appearance.Font = new Font(e.Appearance.Font, FontStyle.Bold);

                        return;
                    }
                }
                else if (pcxMatID == "78730" || pcxMatID == "78728" || pcxMatID == "79466"
                    || pcxMatID == "78732" || pcxMatID == "78734" || pcxMatID == "79482")
                {
                    /* <LAMINATION>
                     * Lamination  HA 700L - DS
                     * Lamination - DM-629-MD - QD, VJ
                     * Lamination - WSM-170 - JJ
                     * Lamination - TECHNOMELT MELTACE 870
                     * Lamination - CMH 1020C
                     * LOCTITE BONDACE 7430WL-2 */

                    if (view.IsCellSelected(e.RowHandle, e.Column) == false)
                    {
                        e.Appearance.ForeColor = Color.LimeGreen;
                        e.Appearance.Font = new Font(e.Appearance.Font, FontStyle.Bold);
                    }
                }
                else if (pcxMatID == "54638" || pcxMatID == "79341")
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
                else if (pcxMatID == "87031" || pcxMatID == "78733" || pcxMatID == "79467")
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

                if (view.GetRowCellValue(e.RowHandle, "NIKE_MS_STATE").ToString().Equals("Retired"))
                {
                    /* When a material is retired. */

                    if (view.IsCellSelected(e.RowHandle, e.Column) == false)
                    {
                        e.Appearance.ForeColor = Color.Orange;
                        e.Appearance.Font = new Font(e.Appearance.Font, FontStyle.Bold);
                        e.Appearance.BackColor = Color.Firebrick;
                    }
                }

                if (view.GetRowCellValue(e.RowHandle, "BLACK_LIST_YN").ToString().Equals("Y"))
                {
                    /* When a material has been enrolled in black list. */

                    if (view.IsCellSelected(e.RowHandle, e.Column) == false)
                    {
                        e.Appearance.ForeColor = Color.White;
                        e.Appearance.Font = new Font(e.Appearance.Font, FontStyle.Bold);
                        e.Appearance.BackColor = Color.Black;
                    }
                }

                if (view.GetRowCellValue(e.RowHandle, "MDM_NOT_EXST").ToString().Equals("Y"))
                {
                    /* When a material has not yet been created as 'CS code' through MDM. */

                    if (view.IsCellSelected(e.RowHandle, e.Column) == false)
                    {
                        e.Appearance.ForeColor = Color.White;
                        e.Appearance.Font = new Font(e.Appearance.Font, FontStyle.Bold);
                        e.Appearance.BackColor = Color.HotPink;
                    }
                }

                #endregion
            }
            else if (e.Column.FieldName == "MAT_COMMENT")
            {
                // When failing to match with pcx lib.
                if (view.GetRowCellValue(e.RowHandle, "MAT_COMMENT").ToString().Equals("Failed to load material info. from MDM."))
                {
                    if (view.IsCellSelected(e.RowHandle, e.Column) == false)
                    {
                        e.Appearance.ForeColor = Color.Red;
                        e.Appearance.Font = new Font(e.Appearance.Font, FontStyle.Bold);
                    }
                }
            }
            else if (e.Column.FieldName == "COLOR_COMMENT")
            {
                // When failing to match with pcx lib.
                if (view.GetRowCellValue(e.RowHandle, "COLOR_COMMENT").ToString().Equals("Failed to load color info from PCX Lib."))
                {
                    if (view.IsCellSelected(e.RowHandle, e.Column) == false)
                    {
                        e.Appearance.ForeColor = Color.Red;
                        e.Appearance.Font = new Font(e.Appearance.Font, FontStyle.Bold);
                    }
                }
            }
            //else if (e.Column.FieldName == "CS_CD")
            //{
            //    #region PCX 라이브러리 매칭 실패
            //    string csCode = view.GetRowCellValue(e.RowHandle, "CS_CD").ToString();
            //    if (csCode == "CS")
            //    {
            //        if (view.IsCellSelected(e.RowHandle, e.Column) == false)
            //        {
            //            e.Appearance.ForeColor = Color.Red;
            //            e.Appearance.Font = new Font(e.Appearance.Font, FontStyle.Bold);
            //        }
            //    }
            //    #endregion
            //}

            #region Custom Color by User

            string colorFormat = view.GetRowCellValue(e.RowHandle, "CELL_COLOR").ToString();
            string[] items = colorFormat.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);

            foreach (string item in items)
            {
                string[] tmp = item.Split('/');
                string columnName = tmp[0];
                string colorName = tmp[1];

                if (e.Column.FieldName == columnName)
                {
                    if (view.IsCellSelected(e.RowHandle, e.Column) == false)
                        e.Appearance.BackColor = Color.FromArgb(Convert.ToInt32(colorName));
                }
            }

            #endregion
        }

        private void CustomDrawColumnHeader(object sender, ColumnHeaderCustomDrawEventArgs e)
        {
            GridView view = sender as GridView;

            if (e.Column == null)
                return;
            else if (e.Column.FieldName == "PCX_PART_ID" || e.Column.FieldName == "PCX_SUPP_MAT_ID"
                || e.Column.FieldName == "PCX_MAT_ID" || e.Column.FieldName == "PART_TYPE"
                || e.Column.FieldName == "PCX_COLOR_ID" || e.Column.FieldName == "PTRN_PART_NAME"
                || e.Column.FieldName == "PART_NIKE_COMMENT" || e.Column.FieldName == "PART_NAME"
                || e.Column.FieldName == "MAT_NAME" || e.Column.FieldName == "COLOR_NAME")
            {
                e.Cache.FillRectangle(Color.DeepSkyBlue, e.Bounds);
                e.Cache.DrawRectangle(e.Cache.GetPen(Color.LightBlue), e.Bounds);
                e.Appearance.DrawString(e.Cache, e.Info.Caption, e.Info.CaptionRect);
                e.Handled = true;
            }
            else
                return;
        }

        #endregion

        #region Button Events

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnSave_Click(object sender, EventArgs e)
        {
            DataTable dataSource;
            GridColumn columnBeforeSave = view.FocusedColumn;  // Current focused column.
            int rowHandleBeforeSave = view.FocusedRowHandle;   // Current focused row.

            try
            {
                SplashScreenManager.ShowForm(this, typeof(MainWaitForm), false, false, false);

                if (Common.IsFilterApplied(view))
                    return;

                if (!CanPassSaveRule())
                    return;

                // In case of only inline BOM.
                // Inform the user that BOM needs to be reconfirmed.
                if (!Common.IsFakeBOM(CSBOMStatus))
                {
                    switch (view.Name)
                    {
                        case "gvwSingleEdit":

                            if (CSBOMStatus.Equals("C") && !isLocked)
                                Common.ShowMessageBox("This is already confirmed. Please click 'Confirm' again.", "W");

                            break;

                        case "gvwMultipleEdit":

                            dataSource = ReturnBOMInfo("ConfirmFlag");

                            if (dataSource != null)
                            {
                                foreach (DataRow row in dataSource.Rows)
                                {
                                    if (row["CS_BOM_CFM"].ToString().Equals("C"))
                                    {
                                        Common.ShowMessageBox("This is already confirmed. Please click 'Confirm' again.", "W");
                                        break;
                                    }
                                }
                            }

                            break;
                    }
                }

                SaveData();

                // Update BOM status to 'N'.
                if (!Common.IsFakeBOM(CSBOMStatus))
                {
                    PKG_INTG_BOM.UPDATE_BOM_CFM_INFO pkgUpdate = new PKG_INTG_BOM.UPDATE_BOM_CFM_INFO();
                    pkgUpdate.ARG_FACTORY = Factory;
                    pkgUpdate.ARG_WS_NO = WSNumber;
                    pkgUpdate.ARG_CS_BOM_CFM = "N";
                    pkgUpdate.ARG_CBD_YN = view.Name.Equals("gvwSingleEdit") ? (btnCBDChk.Checked ? "Y" : "N") : "Pass";
                    pkgUpdate.ARG_UPD_USER = Common.sessionID;

                    ArrayList list = new ArrayList();
                    list.Add(pkgUpdate);

                    if (Common.projectBaseForm.Exe_Modify_PKG(list) == null)
                    {
                        MessageBox.Show("Failed to change BOM status.", "",
                            MessageBoxButtons.OK, MessageBoxIcon.Error, MessageBoxDefaultButton.Button1);
                        return;
                    }
                }

                // Except the BOM locked and fake.
                if (!isLocked && !Common.IsFakeBOM(CSBOMStatus))
                    CSBOMStatus = "N";

                // Re-bind datasource to view.
                if (view.Name.Equals("gvwSingleEdit"))
                    grdSingleEdit.DataSource = BindDataSourceGridView();
                else
                    grdMultipleEdit.DataSource = BindDataSourceGridView();

                FocusColumn(view, rowHandleBeforeSave, columnBeforeSave.FieldName, false);

                SplashScreenManager.CloseForm(false);
                MessageBox.Show("Complete");
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString(), "",
                    MessageBoxButtons.OK, MessageBoxIcon.Error, MessageBoxDefaultButton.Button1);
            }
            finally
            {
                SplashScreenManager.CloseForm(false);
            }
        }

        /// <summary>
        /// Change from 'Confirm' to 'WS confirm' is prohibited.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnWSConfirm_Click(object sender, EventArgs e)
        {
            ArrayList list = new ArrayList();

            if (CSBOMStatus.Equals("C"))
            {
                Common.ShowMessageBox("Already confirmed BOM.", "E");
                return;
            }

            if (Common.IsFilterApplied(view))
                return;

            if (CanPassWSConfirmRule())
            {
                // Update BOM status to 'W'.
                PKG_INTG_BOM.UPDATE_BOM_CFM_INFO pkgUpdate = new PKG_INTG_BOM.UPDATE_BOM_CFM_INFO();
                pkgUpdate.ARG_FACTORY = Factory;
                pkgUpdate.ARG_WS_NO = WSNumber;
                pkgUpdate.ARG_CS_BOM_CFM = "W";
                pkgUpdate.ARG_CBD_YN = btnCBDChk.Checked ? "Y" : "N";
                pkgUpdate.ARG_UPD_USER = Common.sessionID;

                list.Add(pkgUpdate);

                if (Common.projectBaseForm.Exe_Modify_PKG(list) == null)
                {
                    Common.ShowMessageBox("Failed to change BOM Status.", "E");
                    return;
                }

                // BOM status change from 'Save' to 'WS Confirm'.
                if (CSBOMStatus.Equals("N"))
                    CSBOMStatus = "W";

                grdSingleEdit.DataSource = BindDataSourceGridView();
                MessageBox.Show("Complete");
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnConfirm_Click(object sender, EventArgs e)
        {
            DataTable dataSource;
            string DPA = string.Empty;
            string sampleETS = string.Empty;
            int confirmCount = 0;

            if (CSBOMStatus.Equals("C"))
            {
                Common.ShowMessageBox("Already confirmed BOM.", "E");
                return;
            }

            try
            {
                if (Common.IsFilterApplied(view))
                    return;

                PKG_INTG_BOM.SELECT_BOM_INFO_BY_CONFIRM pkgSelect = new PKG_INTG_BOM.SELECT_BOM_INFO_BY_CONFIRM();
                pkgSelect.ARG_WORK_TYPE = "InfoForBOMConfirm";
                pkgSelect.ARG_FACTORY = Factory;
                pkgSelect.ARG_WS_NO = WSNumber;
                pkgSelect.ARG_PART_SEQ = "";
                pkgSelect.OUT_CURSOR = string.Empty;

                dataSource = Common.projectBaseForm.Exe_Select_PKG(pkgSelect).Tables[0];

                DPA = dataSource.Rows[0]["DPA"].ToString();
                sampleETS = dataSource.Rows[0]["SAMPLE_ETS"].ToString();
                confirmCount = Convert.ToInt32(dataSource.Rows[0]["CFM_CNT"]);

                if (AllowConfirm(dataSource))
                {
                    ArrayList list = new ArrayList();

                    #region When an user re-confirm the BOM of which type is prod. cfm.

                    if (Convert.ToInt32(dataSource.Rows[0]["PROD_CFM_CNT"]) > 0)
                    {
                        ConfirmReason form = new ConfirmReason() { Factory = this.Factory, WSNumber = this.WSNumber };

                        if (form.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                        {
                            if (!form.hasNormallySaved)
                                return;
                        }
                        else
                            return;
                    }

                    #endregion

                    SplashScreenManager.ShowForm(this, typeof(MainWaitForm), false, false, false);
                    SplashScreenManager.Default.SendCommand(MainWaitForm.SplashScreenCommand.TransferToNCF, "");

                    #region Transfer NCF data.

                    PKG_INTG_BOM.BOM_IMPORT pkgTransfer = new PKG_INTG_BOM.BOM_IMPORT();
                    pkgTransfer.ARG_WORK_TYPE = "ValidateTransfer";
                    pkgTransfer.ARG_FACTORY = Factory;
                    pkgTransfer.ARG_WS_NO = WSNumber;
                    pkgTransfer.ARG_UPD_USER = Common.sessionID;
                    pkgTransfer.OUT_CURSOR = string.Empty;

                    // Get BOM lineitems from database.
                    dataSource = Common.projectBaseForm.Exe_Select_PKG(pkgTransfer).Tables[0];

                    if (dataSource.Rows.Count > 0)
                    {
                        // Validate Requried fields when importing BOM were entered.
                        for (int rowHandle = 0; rowHandle < dataSource.Rows.Count; rowHandle++)
                        {
                            DataRow row = dataSource.Rows[rowHandle];

                            foreach (DataColumn column in dataSource.Columns)
                            {
                                if (column.ColumnName.Equals("PART_NAME"))
                                    continue;

                                // if the value is null,
                                if (row[column.ColumnName].ToString().Equals(""))
                                {
                                    if (column.ColumnName.Equals("PART_ID"))
                                    {
                                        MessageBox.Show("Please input PCX Part ID", "",
                                            MessageBoxButtons.OK, MessageBoxIcon.Warning, MessageBoxDefaultButton.Button1);
                                    }
                                    else if (column.ColumnName.Equals("CS_PTRN_PART_ID"))
                                    {
                                        MessageBox.Show("Please input CS pattern part of " + row["PART_NAME"].ToString(), "",
                                            MessageBoxButtons.OK, MessageBoxIcon.Warning, MessageBoxDefaultButton.Button1);
                                    }
                                    else if (column.ColumnName.Equals("PCX_MAT_ID") || column.ColumnName.Equals("PCX_SUPP_MAT_ID"))
                                    {
                                        MessageBox.Show("Please input PCX Material ID of " + row["PART_NAME"].ToString(), "",
                                            MessageBoxButtons.OK, MessageBoxIcon.Warning, MessageBoxDefaultButton.Button1);
                                    }
                                    else if (column.ColumnName.Equals("PART_TYPE"))
                                    {
                                        MessageBox.Show("Invalid part type about " + row["PART_NAME"].ToString(), "",
                                            MessageBoxButtons.OK, MessageBoxIcon.Warning, MessageBoxDefaultButton.Button1);
                                    }

                                    Common.FocusCell(gvwSingleEdit, rowHandle, column.ColumnName, false);
                                    return;
                                }
                            }
                        }
                    }
                    else
                    {
                        Common.ShowMessageBox("No line items on the BOM.", "E");
                        return;
                    }

                    // Generate a new PCX key.
                    PKG_INTG_BOM.BOM_IMPORT pkgGetKey = new PKG_INTG_BOM.BOM_IMPORT();
                    pkgGetKey.ARG_WORK_TYPE = "GetPCXKey";
                    pkgGetKey.ARG_FACTORY = Factory;
                    pkgGetKey.ARG_WS_NO = WSNumber;
                    pkgGetKey.ARG_UPD_USER = Common.sessionID;
                    pkgGetKey.OUT_CURSOR = string.Empty;

                    dataSource = Common.projectBaseForm.Exe_Select_PKG(pkgGetKey).Tables[0];

                    if (dataSource.Rows.Count > 0)
                    {
                        // Transfer data to PCX table.
                        PKG_INTG_BOM.TRANSFER_DATA_TO_PCXBOM pkgInsert = new PKG_INTG_BOM.TRANSFER_DATA_TO_PCXBOM();
                        pkgInsert.ARG_PCX_KEY = dataSource.Rows[0][0].ToString();
                        pkgInsert.ARG_FACTORY = Factory;
                        pkgInsert.ARG_WS_NO = WSNumber;
                        pkgInsert.ARG_UPD_USER = Common.sessionID;

                        list.Add(pkgInsert);

                        if (Common.projectBaseForm.Exe_Modify_PKG(list) == null)
                        {
                            Common.ShowMessageBox("Failed to transfer NCF data.", "E");
                            return;
                        }

                        list.Clear();
                    }

                    #endregion

                    SplashScreenManager.Default.SendCommand(MainWaitForm.SplashScreenCommand.ConvertToOCF, "");

                    #region Transfer OCF data.

                    PKG_INTG_BOM.INSERT_CONVERT_TO_OCF pkgInsertConvert = new PKG_INTG_BOM.INSERT_CONVERT_TO_OCF();
                    pkgInsertConvert.ARG_FACTORY = Factory;
                    pkgInsertConvert.ARG_WS_NO = WSNumber;
                    pkgInsertConvert.UPD_USER = Common.sessionID;

                    list.Add(pkgInsertConvert);

                    if (Common.projectBaseForm.Exe_Modify_PKG(list) == null)
                    {
                        Common.ShowMessageBox("Failed to transfer OCF data.", "E");
                        return;
                    }

                    list.Clear();

                    #endregion

                    #region Send Email with changed lineitems.

                    string userPER = Common.GetPIC(DPA, "PER");
                    string userCosting = Common.GetPIC(DPA, "COSTING");

                    // Check the status of PER.
                    PKG_INTG_BOM.SELECT_BOM_INFO_BY_CONFIRM pkgSelect3 = new PKG_INTG_BOM.SELECT_BOM_INFO_BY_CONFIRM();
                    pkgSelect3.ARG_WORK_TYPE = "PER_STATUS";
                    pkgSelect3.ARG_FACTORY = Factory;
                    pkgSelect3.ARG_WS_NO = WSNumber;
                    pkgSelect3.ARG_PART_SEQ = "";
                    pkgSelect3.OUT_CURSOR = string.Empty;

                    dataSource = Common.projectBaseForm.Exe_Select_PKG(pkgSelect3).Tables[0];

                    // PER has been done, more than one confirmed and receiver exists.
                    if (dataSource.Rows.Count > 0 && confirmCount > 0 && (!userPER.Equals("") || !userCosting.Equals("")))
                    {
                        PKG_INTG_BOM.SELECT_BOM_INFO_BY_CONFIRM pkgSelect4 = new PKG_INTG_BOM.SELECT_BOM_INFO_BY_CONFIRM();
                        pkgSelect4.ARG_WORK_TYPE = "ChangedLineItems";
                        pkgSelect4.ARG_FACTORY = Factory;
                        pkgSelect4.ARG_WS_NO = WSNumber;
                        pkgSelect4.ARG_PART_SEQ = "";
                        pkgSelect4.OUT_CURSOR = string.Empty;

                        dataSource = Common.projectBaseForm.Exe_Select_PKG(pkgSelect4).Tables[0];

                        if (dataSource.Rows.Count > 0)
                        {
                            SplashScreenManager.Default.SendCommand(MainWaitForm.SplashScreenCommand.SendEmail, "");

                            try
                            {
                                Common.projectBaseForm.SendEmailBOMChanges(Common.sessionID, new string[] { userPER, userCosting }, Common.LoadBOMCaption(Factory, WSNumber), dataSource);
                            }
                            catch (Exception ex)
                            {
                                MessageBox.Show(ex.ToString());
                            }
                        }
                    }

                    #endregion

                    SplashScreenManager.Default.SendCommand(MainWaitForm.SplashScreenCommand.UpdateStatus, "");

                    #region Update BOM status.

                    PKG_INTG_BOM.UPDATE_BOM_CFM_INFO pkgUpdate = new PKG_INTG_BOM.UPDATE_BOM_CFM_INFO();
                    pkgUpdate.ARG_FACTORY = Factory;
                    pkgUpdate.ARG_WS_NO = WSNumber;
                    pkgUpdate.ARG_CS_BOM_CFM = "C";
                    pkgUpdate.ARG_CBD_YN = btnCBDChk.Checked ? "Y" : "N";
                    pkgUpdate.ARG_UPD_USER = Common.sessionID;

                    list.Add(pkgUpdate);

                    if (Common.projectBaseForm.Exe_Modify_PKG(list) == null)
                    {
                        Common.ShowMessageBox("Failed to change the BOM Status.", "E");
                        return;
                    }

                    #endregion

                    grdSingleEdit.DataSource = BindDataSourceGridView();

                    // In case of auto lock.
                    isLocked = (grdSingleEdit.DataSource as DataTable).Rows[0]["LOCK_YN"].ToString().Equals("Y") ? true : false;

                    if (isLocked)
                        Common.MakeColumnsUneditable(view);

                    CSBOMStatus = "C";

                    #region Get the earliest possible BOM import date.

                    PKG_INTG_BOM.SELECT_BOM_INFO_BY_CONFIRM pkgGetETS = new PKG_INTG_BOM.SELECT_BOM_INFO_BY_CONFIRM();
                    pkgGetETS.ARG_WORK_TYPE = "SampleETS";
                    pkgGetETS.ARG_FACTORY = Factory;
                    pkgGetETS.ARG_WS_NO = sampleETS;
                    pkgGetETS.ARG_PART_SEQ = "";
                    pkgGetETS.OUT_CURSOR = string.Empty;

                    dataSource = Common.projectBaseForm.Exe_Select_PKG(pkgGetETS).Tables[0];

                    #endregion

                    SplashScreenManager.CloseForm(false);

                    Common.ShowMessageBox(string.Format("Complete.\n\n-> Please import the BOM on {0}.",
                        Convert.ToDateTime(dataSource.Rows[0]["WORK_YMD"]).ToString("d")), "I");
                }
            }
            catch (Exception ex)
            {
                Common.ShowMessageBox(ex.ToString(), "E");
                return;
            }
            finally
            {
                SplashScreenManager.CloseForm(false);
            }
        }

        /// <summary>
        /// Replace BOM data with the data of the BOM selected.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnReplace_Click(object sender, EventArgs e)
        {
            // The BOM confirmed can't be replaced.
            if (CSBOMStatus.Equals("W") || CSBOMStatus.Equals("C"))
            {
                Common.ShowMessageBox("The BOM already confirmed can not be replaced.", "E");
                return;
            }
            else
            {
                DataTable dataSource = ReturnBOMInfo("MatchStatus");

                if (dataSource != null)
                {
                    // Validate that the BOM was replaced.
                    if (dataSource.Rows[0]["REQ_BOM_YN"].ToString().Equals("M"))
                    {
                        Common.ShowMessageBox("This BOM had already been replaced.\nTry again after reset.", "E");
                        return;
                    }
                    else
                    {
                        BOMSearch searchForm = new BOMSearch()
                        {
                            Factory = this.Factory,
                            WSNumber = this.WSNumber,
                            Mode = "BOM"
                        };

                        if (searchForm.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                            grdSingleEdit.DataSource = BindDataSourceGridView();
                    }
                }
            }
        }

        /// <summary>
        /// Revert to original BOM data.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnReset_Click(object sender, EventArgs e)
        {
            DataTable dataSource;

            if (CSBOMStatus.Equals("C") || CSBOMStatus.Equals("W"))
            {
                Common.ShowMessageBox("The BOM already confirmed cannot be reset.", "E");
                return;
            }
            else
            {
                dataSource = ReturnBOMInfo("MatchStatus");

                if (dataSource != null)
                {
                    if (dataSource.Rows[0]["REQ_BOM_YN"].ToString() != "M")
                    {
                        Common.ShowMessageBox("It has not yet been matched.", "E");
                        return;
                    }
                    else
                    {
                        if (MessageBox.Show("Do you really want to proceed?", "",
                            MessageBoxButtons.OKCancel, MessageBoxIcon.Question, MessageBoxDefaultButton.Button1) == System.Windows.Forms.DialogResult.OK)
                        {
                            PKG_INTG_BOM.INSERT_RESET_BY_ORG_DATA pkgInsert = new PKG_INTG_BOM.INSERT_RESET_BY_ORG_DATA();
                            pkgInsert.ARG_WORK_TYPE = string.Empty;
                            pkgInsert.ARG_FACTORY = Factory;
                            pkgInsert.ARG_WS_NO = WSNumber;
                            pkgInsert.ARG_UPD_USER = Common.sessionID;

                            ArrayList list = new ArrayList();
                            list.Add(pkgInsert);

                            if (Common.projectBaseForm.Exe_Modify_PKG(list) == null)
                            {
                                Common.ShowMessageBox("Failed to reset.", "E");
                                return;
                            }

                            MessageBox.Show("Complete");
                            grdSingleEdit.DataSource = BindDataSourceGridView();
                        }
                    }
                }
            }
        }

        /// <summary>
        /// JSON 파일로 Export
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnExport_Click(object sender, EventArgs e)
        {
            List<string> validTypes = new List<string> { "UPPER", "MIDSOLE", "OUTSOLE", "PACKAGING", "AIRBAG" };    // Valid section in PCX.
            DataTable dataSource = grdSingleEdit.DataSource as DataTable;

            Func<int, string, bool> IsEmpty = (rowHandle, fieldName) =>
            {
                if (view.GetRowCellValue(rowHandle, fieldName).ToString().Equals(""))
                {
                    Common.ShowMessageBox(string.Format("{0} is requried.", fieldName), "E");
                    Common.FocusCell(view, rowHandle, fieldName, true);

                    return true;
                }

                return false;
            };

            if (Common.IsFilterApplied(gvwSingleEdit))
                return;

            if (dataSource.AsEnumerable().Where(
                x => x["ROW_STATUS"].ToString() != "N").Count() > 0)
            {
                Common.ShowMessageBox("Please save the data first.", "E");
                return;
            }

            if (dataSource.AsEnumerable().Where(
                x => !validTypes.Contains(x["PART_TYPE"].ToString())).Count() > 0)
            {
                Common.ShowMessageBox("There are invalid part types. Please check again", "E");
                return;
            }

            if (dataSource.AsEnumerable().Where(
                x => x["NIKE_MS_STATE"].ToString().Equals("Retired")).Count() > 0)
            {
                if (MessageBox.Show("There are materials retired in the BOM, Do you want to proceed?", "",
                    MessageBoxButtons.OKCancel, MessageBoxIcon.Warning) == System.Windows.Forms.DialogResult.Cancel)
                    return;
            }

            if (dataSource.AsEnumerable().Where(
                x => x["COMBINE_YN"].ToString().Equals("Y") &&
                    !x["PTRN_PART_NAME"].ToString().Equals("")).Count() > 0)
            {
                Common.ShowMessageBox("There are rows checked in 'Combine' with Nike Pattern Part.", "E");
                return;
            }

            // Valid Required fields are entered.
            for (int rowHandle = 0; rowHandle < gvwSingleEdit.RowCount; rowHandle++)
            {
                if (IsEmpty(rowHandle, "PCX_PART_ID"))
                    return;

                if (IsEmpty(rowHandle, "PCX_SUPP_MAT_ID"))
                    return;

                if (IsEmpty(rowHandle, "PCX_MAT_ID"))
                    return;
            }

            BOMImport form = new BOMImport() { Factory = this.Factory, WSNumber = this.WSNumber };
            form.Show();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnCBDChk_CheckedChanged(object sender, EventArgs e)
        {
            CheckButton btn = (DevExpress.XtraEditors.CheckButton)sender;

            if (btn.Checked)
                btn.Text = "Checked";
            else
                btn.Text = "CBD";
        }

        /// <summary>
        /// 
        /// </summary>
        private void SaveData()
        {
            ArrayList list = new ArrayList();

            NumberLineItems();

            for (int i = 0; i < view.RowCount; i++)
            {
                PKG_INTG_BOM.UPDATE_BOM_TAIL pkgUpdate = new PKG_INTG_BOM.UPDATE_BOM_TAIL();
                pkgUpdate.ARG_FACTORY = view.GetRowCellValue(i, "FACTORY").ToString();
                pkgUpdate.ARG_WS_NO = view.GetRowCellValue(i, "WS_NO").ToString();
                pkgUpdate.ARG_PART_SEQ = view.GetRowCellValue(i, "PART_SEQ").ToString();
                pkgUpdate.ARG_PART_NIKE_NO = view.GetRowCellValue(i, "PART_NIKE_NO").ToString();
                pkgUpdate.ARG_PART_NIKE_COMMENT = view.GetRowCellValue(i, "PART_NIKE_COMMENT").ToString();
                pkgUpdate.ARG_PART_CD = view.GetRowCellValue(i, "PART_CD").ToString();
                pkgUpdate.ARG_PART_NAME = view.GetRowCellValue(i, "PART_NAME").ToString().Trim();
                pkgUpdate.ARG_PART_TYPE = view.GetRowCellValue(i, "PART_TYPE").ToString().Trim();
                pkgUpdate.ARG_BTTM = view.GetRowCellValue(i, "BTTM").ToString();
                pkgUpdate.ARG_MXSXL_NUMBER = view.GetRowCellValue(i, "MXSXL_NUMBER").ToString();
                pkgUpdate.ARG_CS_CD = "";
                pkgUpdate.ARG_MAT_CD = view.GetRowCellValue(i, "MAT_CD").ToString();
                pkgUpdate.ARG_MAT_NAME = view.GetRowCellValue(i, "MAT_NAME").ToString();
                pkgUpdate.ARG_MAT_COMMENT = view.GetRowCellValue(i, "MAT_COMMENT").ToString().Trim();
                pkgUpdate.ARG_MCS_NUMBER = view.GetRowCellValue(i, "MCS_NUMBER").ToString();
                pkgUpdate.ARG_COLOR_CD = view.GetRowCellValue(i, "COLOR_CD").ToString();
                pkgUpdate.ARG_COLOR_NAME = view.GetRowCellValue(i, "COLOR_NAME").ToString();
                pkgUpdate.ARG_COLOR_COMMENT = view.GetRowCellValue(i, "COLOR_COMMENT").ToString().Trim();
                pkgUpdate.ARG_SORT_NO = view.GetRowCellValue(i, "SORT_NO").ToString();
                pkgUpdate.ARG_REMARKS = view.GetRowCellValue(i, "REMARKS").ToString().Trim();
                pkgUpdate.ARG_UPD_USER = Common.sessionID;
                pkgUpdate.ARG_PTRN_PART_NAME = view.GetRowCellValue(i, "PTRN_PART_NAME").ToString().Trim();
                pkgUpdate.ARG_PCX_MAT_ID = view.GetRowCellValue(i, "PCX_MAT_ID").ToString();
                pkgUpdate.ARG_PCX_SUPP_MAT_ID = view.GetRowCellValue(i, "PCX_SUPP_MAT_ID").ToString();
                pkgUpdate.ARG_PCX_COLOR_ID = view.GetRowCellValue(i, "PCX_COLOR_ID").ToString();
                pkgUpdate.ARG_ROW_STATUS = view.GetRowCellValue(i, "ROW_STATUS").ToString();                // Update Type 구분(N, I, D)
                pkgUpdate.ARG_PROCESS = view.GetRowCellValue(i, "PROCESS").ToString().ToUpper();
                pkgUpdate.ARG_VENDOR_NAME = view.GetRowCellValue(i, "VENDOR_NAME").ToString().ToUpper();
                pkgUpdate.ARG_COMBINE_YN = view.GetRowCellValue(i, "COMBINE_YN").ToString();
                pkgUpdate.ARG_STICKER_YN = view.GetRowCellValue(i, "STICKER_YN").ToString();
                pkgUpdate.ARG_PTRN_PART_CD = view.GetRowCellValue(i, "PTRN_PART_CD").ToString();
                pkgUpdate.ARG_MDSL_CHK = view.GetRowCellValue(i, "MDSL_CHK").ToString();
                pkgUpdate.ARG_OTSL_CHK = view.GetRowCellValue(i, "OTSL_CHK").ToString();
                pkgUpdate.ARG_CELL_COLOR = view.GetRowCellValue(i, "CELL_COLOR").ToString();
                pkgUpdate.ARG_CS_PTRN_CD = view.GetRowCellValue(i, "CS_PTRN_CD").ToString();
                pkgUpdate.ARG_CS_PTRN_NAME = view.GetRowCellValue(i, "CS_PTRN_NAME").ToString();
                pkgUpdate.ARG_LOGIC_GROUP = "";
                pkgUpdate.ARG_MAT_FORECAST_PRCNT = 0;
                pkgUpdate.ARG_COLOR_FORECAST_PRCNT = 0;
                pkgUpdate.ARG_ENCODED_CMT = view.GetRowCellValue(i, "ENCODED_CMT").ToString();

                list.Add(pkgUpdate);
            }

            if (Common.projectBaseForm.Exe_Modify_PKG(list) == null)
            {
                MessageBox.Show("Failed to save data.", "",
                    MessageBoxButtons.OK, MessageBoxIcon.Error, MessageBoxDefaultButton.Button1);
            }
        }

        /// <summary>
        /// Number the line item sequently.
        /// </summary>
        private void NumberLineItems()
        {
            try
            {
                view.CellValueChanged -= new CellValueChangedEventHandler(CustomCellValueChanged);

                for (int i = 0; i < NumSelectedBOMs; i++)
                {
                    int sortNumber = 1;

                    // Increase by the number of selected BOMS.
                    for (int j = i; j < view.RowCount; j += NumSelectedBOMs)
                    {
                        if (view.GetRowCellValue(j, "ROW_STATUS").ToString() != "D")
                        {
                            view.SetRowCellValue(j, "SORT_NO", sortNumber);
                            sortNumber++;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString(), "",
                    MessageBoxButtons.OK, MessageBoxIcon.Error, MessageBoxDefaultButton.Button1);
            }
            finally
            {
                view.CellValueChanged += new CellValueChangedEventHandler(CustomCellValueChanged);
            }
        }

        /// <summary>
        /// Return BOM information by the workType.
        /// </summary>
        /// <returns></returns>
        private DataTable ReturnBOMInfo(string workType)
        {
            PKG_INTG_BOM.SELECT_BOM_INFO_BY_CONFIRM pkgSelect = new PKG_INTG_BOM.SELECT_BOM_INFO_BY_CONFIRM();
            pkgSelect.ARG_WORK_TYPE = workType;
            pkgSelect.ARG_FACTORY = Factory;
            pkgSelect.ARG_WS_NO = WSNumber;
            pkgSelect.ARG_PART_SEQ = "";
            pkgSelect.OUT_CURSOR = string.Empty;

            DataSet ds = Common.projectBaseForm.Exe_Select_PKG(pkgSelect);

            if (ds.Tables[0].Rows.Count > 0)
                return ds.Tables[0];
            else
                return null;
        }

        /// <summary>
        /// 뷰에서 조건에 해당하는 행의 위치를 구한다.
        /// </summary>
        /// <param name="view"></param>
        /// <param name="targetWsNo"></param>
        /// <param name="targetPartCode"></param>
        /// <returns></returns>
        private int GetRowHandle(GridView view, string targetWsNo, string targetPartCode)
        {
            bool isPassedPrecedeLine = false; // 선행하는 행은 포함될 항목이라 가정

            for (int rowHandle = 0; rowHandle < view.RowCount; rowHandle++)
            {
                string wsNo = view.GetRowCellValue(rowHandle, "WS_NO").ToString();
                string partCode = view.GetRowCellValue(rowHandle, "PART_CD").ToString();

                if ((wsNo == targetWsNo) && (partCode == targetPartCode))
                {
                    if (isPassedPrecedeLine)
                        return rowHandle;
                    else
                        isPassedPrecedeLine = true;
                }
            }

            return 0;
        }

        /// <summary>
        /// Validate that Parts on BOM are duplicate.
        /// </summary>
        /// <returns></returns>
        private bool CanPassSaveRule()
        {
            if (Common.IsFakeBOM(CSBOMStatus))
            {
                // Delegate for checking an attribute is entered.
                Func<int, string, bool> IsValueEmpty = (rowHandle, fieldName) =>
                {
                    if (view.GetRowCellValue(rowHandle, fieldName).ToString() == "")
                    {
                        MessageBox.Show(string.Format("{0} is required. Please input.", fieldName), "",
                           MessageBoxButtons.OK, MessageBoxIcon.Error, MessageBoxDefaultButton.Button1);

                        Common.FocusCell(view, rowHandle, fieldName, true);
                        return true;
                    }
                    else
                        return false;
                };

                /* In case of Fake BOM */

                for (int rowHandle = 0; rowHandle < view.RowCount; rowHandle++)
                {
                    // Skip rows which will be deleted.
                    if (view.GetRowCellValue(rowHandle, "ROW_STATUS").ToString() == "D")
                        continue;

                    if (IsValueEmpty(rowHandle, "CS_PTRN_NAME"))
                        return false;

                    if (IsValueEmpty(rowHandle, "PART_TYPE"))
                        return false;
                }
            }
            else
            {
                /* In case of Inline BOM */

                List<string> partList = new List<string>();
                List<string> lamPartList = new List<string>();
                string partCode = string.Empty;
                string partName = string.Empty;
                string csPtrnCode = string.Empty;
                string csPtrnName = string.Empty;
                string key = string.Empty;

                Func<string, bool> IsPartDuplicate = (_WSNumber) =>
                {
                    if (partName.Contains("LAMINATION"))
                    {
                        key = partCode + csPtrnCode;

                        if (lamPartList.Contains(key))
                        {
                            MessageBox.Show("\"" + partName + " / " + csPtrnName + "\" is duplicate.", "",
                                        MessageBoxButtons.OK, MessageBoxIcon.Error, MessageBoxDefaultButton.Button1);

                            Common.FocusCell(view, GetRowHandle(view, _WSNumber, partCode), "PART_NAME", true);
                            return true;
                        }
                        else
                            lamPartList.Add(key);
                    }
                    else
                    {
                        if (partList.Contains(partCode))
                        {
                            MessageBox.Show("\"" + partName + "\" is duplicate.", "",
                                MessageBoxButtons.OK, MessageBoxIcon.Error, MessageBoxDefaultButton.Button1);

                            Common.FocusCell(view, GetRowHandle(view, _WSNumber, partCode), "PART_NAME", true);
                            return true;
                        }
                        else
                            partList.Add(partCode);
                    }

                    return false;
                };

                if (view.Name.Equals("gvwMultipleEdit"))
                {
                    string[] listWSNumber = WSNumber.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
                    DataTable dataSource = grdMultipleEdit.DataSource as DataTable;
                    DataRow[] rows;

                    foreach (string wsNumber in listWSNumber)
                    {
                        rows = dataSource.Select("WS_NO = '" + wsNumber + "'");

                        foreach (DataRow row in rows)
                        {
                            partCode = row["PART_CD"].ToString();
                            partName = row["PART_NAME"].ToString();
                            csPtrnCode = row["CS_PTRN_CD"].ToString();
                            csPtrnName = row["CS_PTRN_NAME"].ToString();

                            if (partCode == "" || row["ROW_STATUS"].ToString().Equals("D"))
                                continue;

                            if (IsPartDuplicate(wsNumber))
                                return false;
                        }

                        // Clear list for the next BOM.
                        partList.Clear();
                        lamPartList.Clear();
                    }
                }
                else if (view.Name.Equals("gvwSingleEdit"))
                {
                    for (int rowHandle = 0; rowHandle < view.RowCount; rowHandle++)
                    {
                        partCode = view.GetRowCellValue(rowHandle, "PART_CD").ToString();
                        partName = view.GetRowCellValue(rowHandle, "PART_NAME").ToString();
                        csPtrnCode = view.GetRowCellValue(rowHandle, "CS_PTRN_CD").ToString();
                        csPtrnName = view.GetRowCellValue(rowHandle, "CS_PTRN_NAME").ToString();

                        if (partCode == "" || view.GetRowCellValue(rowHandle, "ROW_STATUS").ToString().Equals("D"))
                            continue;

                        if (IsPartDuplicate(WSNumber))
                            return false;
                    }
                }

                DataTable dt = view.Equals(gvwSingleEdit) ? grdSingleEdit.DataSource as DataTable : grdMultipleEdit.DataSource as DataTable;

                if (dt.AsEnumerable().Where(x => (x["PCX_MAT_ID"].ToString().Equals("62499") || x["PCX_MAT_ID"].ToString().Equals("62496"))
                    && x["COLOR_CD"].ToString().Equals("10A")).Count() > 0)
                {
                    Common.ShowMessageBox("Please change the color of PCX material '62499' or '62496'\nto '99J'.", "W");
                    return false;
                }
            }

            return true;
        }

        /// <summary>
        /// <para>1. 변경 사항 저장 여부 확인</para>
        /// <para>2. 파트, 패턴파트 입력 여부 확인</para>
        /// <para>3. DPA, Prod. Factory</para>
        /// <para>4. 라인 아이템 사용 가능 여부</para>
        /// <para>5. 공정 사용 가능 여부</para>        
        /// </summary>
        /// <param name="view"></param>
        /// <returns></returns>
        private bool AllowConfirm(DataTable dataSource)
        {
            string[] productionTypes = new string[] { "GTMC", "RFCC", "PRDC" };
            string prodFactory = dataSource.Rows[0]["PROD_FACTORY"].ToString();
            string sampleType = dataSource.Rows[0]["ST_CD"].ToString();

            if (Common.HasLineitemUnsaved(view))
                return false;

            if (HasNullParts())
                return false;

            if (HasPtrnPartCombined())
                return false;

            if (Has999Items())
                return false;

            if (!CanPassLamRule(prodFactory))
                return false;

            if (!CanPassNCFRule())
                return false;

            // DPA is used to load PIC from PMX to send an e-mail with a file which includes changed line items.
            if (dataSource.Rows[0]["DPA"].ToString().Equals("X"))
            {
                Common.ShowMessageBox("DPA is not entered or has not yet been registered.", "E");
                return false;
            }

            // Validate the prod. factory is valid for specific sample types.
            if (productionTypes.Contains(sampleType))
            {
                if (prodFactory.Equals("X") || prodFactory.Equals("DS"))
                {
                    Common.ShowMessageBox("Not valid prod. factory.", "E");
                    return false;
                }
            }

            // Logic for each sample type.
            switch (sampleType)
            {
                // When the sample type is SPA, an user must input comment data through the specific pop-up form.
                case "GTMC":

                    bool isIncluded = false;
                    DataRow[] rows = ((DataTable)grdSingleEdit.DataSource).AsEnumerable().Where(
                        x => x["MAT_COMMENT"].ToString().Split(new string[] { ":", "/" }, StringSplitOptions.None).Length >= 6).ToArray();

                    foreach (DataRow row in rows)
                    {
                        // Validate that the comment was input through the specific pop-up form.
                        if (row["ENCODED_CMT"].ToString().Equals(""))
                            isIncluded = true;
                    }

                    if (isIncluded)
                    {
                        if (MessageBox.Show("Some comments must be input through pop-up, Do you want to proceed?", "",
                            MessageBoxButtons.OKCancel, MessageBoxIcon.Warning, MessageBoxDefaultButton.Button1) == System.Windows.Forms.DialogResult.Cancel)
                            return false;
                    }

                    break;

                case "CCFM":

                    if (Convert.ToInt32(dataSource.Rows[0]["JSON_HEADER_CNT"]).Equals(0))
                    {
                        Common.ShowMessageBox("Please matching json header information first.", "E");
                        return false;
                    }

                    break;

                default:
                    break;
            }

            // Load operations from database.
            PKG_INTG_BOM.SELECT_BOM_CONFIRM_VLDTN pkgSelect = new PKG_INTG_BOM.SELECT_BOM_CONFIRM_VLDTN();
            pkgSelect.ARG_WORK_TYPE = "OperationMaster";
            pkgSelect.ARG_FACTORY = Factory;
            pkgSelect.ARG_WS_NO = WSNumber;
            pkgSelect.OUT_CURSOR = string.Empty;

            DataTable dt = Common.projectBaseForm.Exe_Select_PKG(pkgSelect).Tables[0];

            if (dt.Rows.Count > 0)
            {
                foreach (DataRow row in dt.Rows)
                {
                    // OP_USE_YN == 'N' : Not usable.
                    if (row["OP_USE_YN"].ToString() == "N")
                    {
                        MessageBox.Show("\"" + row["OP_NAME"].ToString() + "\" is not available operation. Please ask MDM.", "",
                            MessageBoxButtons.OK, MessageBoxIcon.Warning, MessageBoxDefaultButton.Button1);
                        return false;
                    }
                }
            }

            return true;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        private bool CanPassWSConfirmRule()
        {
            if (Common.HasLineitemUnsaved(view))
                return false;

            if (HasNullParts())
                return false;

            // See if BTTM is empty although the check-box of Midsole or Outsole is checked.
            PKG_INTG_BOM.SELECT_BOM_CONFIRM_VLDTN pkgSelect = new PKG_INTG_BOM.SELECT_BOM_CONFIRM_VLDTN();
            pkgSelect.ARG_WORK_TYPE = "BTTMOmission";
            pkgSelect.ARG_FACTORY = Factory;
            pkgSelect.ARG_WS_NO = WSNumber;
            pkgSelect.OUT_CURSOR = string.Empty;

            if (Convert.ToInt32(Common.projectBaseForm.Exe_Select_PKG(pkgSelect).Tables[0].Rows[0]["CNT"]) > 0)
            {
                if (MessageBox.Show("There is a lineitem of which type is midsole or outsole but BTTM is empty. Do you want to proceed?", "",
                    MessageBoxButtons.YesNo, MessageBoxIcon.Warning) == System.Windows.Forms.DialogResult.No)
                    return false;
            }
            return true;
        }

        /// <summary>
        /// <para>Check sourcing factory and lamination materials are correctly matched.</para>
        /// <para>1. Normal Lamination</para>
        /// <para>2. EVA</para>
        /// <para>3. Sticker</para>
        /// <para>4. Molded Sockliner</para>
        /// </summary>
        /// <param name="view"></param>
        /// <returns></returns>
        private bool CanPassLamRule(string prodFactory)
        {
            // Lamination material codes per source factory.
            string[] arrSuppMatQD = new string[] { "139078", "139101", "139110" };
            string[] arrSuppMatVJ = new string[] { "139075", "139113", "139107", "140875" };
            string[] arrSuppMatJJ = new string[] { "140823", "139104", "140826" };

            // Lamination material codes of this BOM.
            string[] arrSuppMatBOM = (grdSingleEdit.DataSource as DataTable).AsEnumerable().Where(
                x => x["PART_NAME"].ToString().Contains("LAMINATION")).Select(r => r["PCX_SUPP_MAT_ID"].ToString()).Distinct().ToArray();

            int numOrg = arrSuppMatBOM.Count();

            Func<string[], bool> IncludeOthers = (arr) =>
            {
                if (arrSuppMatBOM.Except(arr).Count() != numOrg)
                {
                    MessageBox.Show("Please check sourcing and lamination materials are correctly matched.", "",
                        MessageBoxButtons.OK, MessageBoxIcon.Warning, MessageBoxDefaultButton.Button1);
                    return true;
                }
                else
                    return false;
            };

            switch (prodFactory)
            {
                case "QD":

                    if (IncludeOthers(arrSuppMatVJ))
                        return false;

                    if (IncludeOthers(arrSuppMatJJ))
                        return false;

                    break;

                case "VJ":

                    if (IncludeOthers(arrSuppMatQD))
                        return false;

                    if (IncludeOthers(arrSuppMatJJ))
                        return false;

                    break;

                case "JJ":

                    if (IncludeOthers(arrSuppMatQD))
                        return false;

                    if (IncludeOthers(arrSuppMatVJ))
                        return false;

                    break;
            }

            #region backup
            //if (prodFactory == "QD")
            //{
            //    #region Compare VJ or JJ

            //    after = arrSuppMatBOM.Except(arrSuppMatVJ).Count();

            //    if (before != after)
            //    {
            //        MessageBox.Show("Please check sourcing and lamination materials are correctly matched.", "",
            //            MessageBoxButtons.OK, MessageBoxIcon.Warning, MessageBoxDefaultButton.Button1);
            //        return false;
            //    }
            //    else
            //    {
            //        after = arrSuppMatBOM.Except(arrSuppMatJJ).Count();

            //        if (before != after)
            //        {
            //            MessageBox.Show("Please check sourcing and lamination materials are correctly matched.", "",
            //                MessageBoxButtons.OK, MessageBoxIcon.Warning, MessageBoxDefaultButton.Button1);
            //            return false;
            //        }
            //    }

            //    #endregion
            //}
            //else if (prodFactory == "VJ")
            //{
            //    #region Compare QD or JJ

            //    after = arrSuppMatBOM.Except(arrSuppMatQD).Count();

            //    if (before != after)
            //    {
            //        MessageBox.Show("Please check sourcing and lamination materials are correctly matched.", "",
            //            MessageBoxButtons.OK, MessageBoxIcon.Warning, MessageBoxDefaultButton.Button1);
            //        return false;
            //    }
            //    else
            //    {
            //        after = arrSuppMatBOM.Except(arrSuppMatJJ).Count();

            //        if (before != after)
            //        {
            //            MessageBox.Show("Please check sourcing and lamination materials are correctly matched.", "",
            //                MessageBoxButtons.OK, MessageBoxIcon.Warning, MessageBoxDefaultButton.Button1);
            //            return false;
            //        }
            //    }

            //    #endregion
            //}
            //else if (prodFactory == "JJ")
            //{
            //    #region Compare QD or VJ

            //    after = arrSuppMatBOM.Except(arrSuppMatQD).Count();

            //    if (before != after)
            //    {
            //        MessageBox.Show("Please check sourcing and lamination materials are correctly matched.", "",
            //            MessageBoxButtons.OK, MessageBoxIcon.Warning, MessageBoxDefaultButton.Button1);
            //        return false;
            //    }
            //    else
            //    {
            //        after = arrSuppMatBOM.Except(arrSuppMatVJ).Count();

            //        if (before != after)
            //        {
            //            MessageBox.Show("Please check sourcing and lamination materials are correctly matched.", "",
            //                MessageBoxButtons.OK, MessageBoxIcon.Warning, MessageBoxDefaultButton.Button1);
            //            return false;
            //        }
            //    }

            //    #endregion
            //}
            #endregion

            return true;
        }

        /// <summary>
        /// Validate that there is a line item of which code about parts is null.
        /// </summary>
        /// <returns></returns>
        private bool HasNullParts()
        {
            PKG_INTG_BOM.SELECT_BOM_CONFIRM_VLDTN pkgSelect = new PKG_INTG_BOM.SELECT_BOM_CONFIRM_VLDTN();
            pkgSelect.ARG_WORK_TYPE = "NullPartExisting";
            pkgSelect.ARG_FACTORY = Factory;
            pkgSelect.ARG_WS_NO = WSNumber;
            pkgSelect.OUT_CURSOR = string.Empty;

            DataTable dataSource = Common.projectBaseForm.Exe_Select_PKG(pkgSelect).Tables[0];

            if (dataSource.Rows.Count > 0)
            {
                foreach (DataRow row in dataSource.Rows)
                {
                    if (row["PART_CD"].ToString() == "X" || row["CS_PTRN_CD"].ToString() == "X")
                    {
                        MessageBox.Show("Part Name and CS Pattern Name must be input.", "",
                            MessageBoxButtons.OK, MessageBoxIcon.Warning, MessageBoxDefaultButton.Button1);

                        FocusColumn(view, Convert.ToInt32(row["SORT_NO"]) - 1, "PART_NAME", true);
                        return true;
                    }
                }
            }
            return false;
        }

        /// <summary>
        /// 999 라인 기입 여부 확인
        /// </summary>
        /// <param name="view"></param>
        /// <returns></returns>
        private bool Has999Items()
        {
            List<DataRow> list = ((DataTable)grdSingleEdit.DataSource).AsEnumerable().Where(
                x => x["PCX_MAT_ID"].ToString().Equals("999")).ToList();

            if (list.Count > 0)
            {
                MessageBox.Show("N/A(999) line can not be included in the BOM", "",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning, MessageBoxDefaultButton.Button1);

                return true;
            }

            return false;
        }

        /// <summary>
        /// Validate that a combine box of a row with Nike Pattern Part Name was ticked.
        /// </summary>
        /// <returns></returns>
        private bool HasPtrnPartCombined()
        {
            List<DataRow> list = (grdSingleEdit.DataSource as DataTable).AsEnumerable().Where(
                x => x["COMBINE_YN"].ToString() == "Y" && x["PTRN_PART_NAME"].ToString() != "").ToList();

            if (list.Count > 0)
            {
                MessageBox.Show("Please check a combine box of a row with Nike Pattern Part Name was ticked.", "",
                   MessageBoxButtons.OK, MessageBoxIcon.Error, MessageBoxDefaultButton.Button1);

                Common.FocusCell(gvwSingleEdit, Convert.ToInt32(list[0]["SORT_NO"]) - 1, "PTRN_PART_NAME", true);

                return true;
            }

            return false;
        }

        /// <summary>
        /// 매개변수에 지정된 셀 선택
        /// </summary>
        /// <param name="rowHandle"></param>
        /// <param name="columnName"></param>
        private void FocusColumn(GridView view, int rowHandle, string columnName, bool showing)
        {
            view.ClearSelection();
            view.SelectCell(rowHandle, view.Columns[columnName]);
            view.FocusedRowHandle = rowHandle;
            view.FocusedColumn = view.Columns[columnName];

            if (showing == true)
                view.ShowEditor();
        }

        /// <summary>
        /// MIDSOLE 또는 OUTSOLE 체크하였으니 BTTM 입력 값이 없는 경우 메시지 알림
        /// </summary>
        /// <param name="view"></param>
        /// <returns></returns>
        private bool CheckOmissionOfBTTM(GridView view, int rowHandle)
        {
            string midsoleCheck = view.GetRowCellValue(rowHandle, "MDSL_CHK").ToString();
            string outsoleCheck = view.GetRowCellValue(rowHandle, "OTSL_CHK").ToString();
            if (midsoleCheck == "Y" || outsoleCheck == "Y")
            {
                string BTTM = view.GetRowCellValue(rowHandle, "BTTM").ToString();
                if (BTTM == "")
                {
                    return false;
                }
            }
            return true;
        }

        #endregion

        #region User Defined Functions

        /// <summary>
        /// Hide some columns for fake bom.
        /// </summary>
        private void HideColumnsFakeBOM()
        {
            switch (EditType)
            {
                case "Single":

                    gvwSingleEdit.Columns["PART_CD"].Visible = false;
                    gvwSingleEdit.Columns["PTRN_PART_NAME"].Visible = false;
                    gvwSingleEdit.Columns["PART_NIKE_COMMENT"].Visible = false;
                    gvwSingleEdit.Columns["COMBINE_YN"].Visible = false;
                    gvwSingleEdit.Columns["STICKER_YN"].Visible = false;
                    gvwSingleEdit.Columns["MDSL_CHK"].Visible = false;
                    gvwSingleEdit.Columns["OTSL_CHK"].Visible = false;
                    gvwSingleEdit.Columns["MXSXL_NUMBER"].Visible = false;
                    gvwSingleEdit.Columns["PCX_SUPP_MAT_ID"].Visible = false;
                    //gvwSingleEdit.Columns["CS_CD"].Visible = false;
                    gvwSingleEdit.Columns["MCS_NUMBER"].Visible = false;
                    gvwSingleEdit.Columns["PCX_MAT_ID"].Visible = false;
                    gvwSingleEdit.Columns["MAT_CD"].Visible = false;
                    gvwSingleEdit.Columns["VENDOR_NAME"].Visible = false;
                    gvwSingleEdit.Columns["PCX_COLOR_ID"].Visible = false;
                    gvwSingleEdit.Columns["PCX_PART_ID"].Visible = false;

                    btnCBDChk.Enabled = false;
                    btnConfirm.Enabled = false;
                    btnWSConfirm.Enabled = false;

                    break;

                case "Multiple":

                    gvwMultipleEdit.Columns["PART_CD"].Visible = false;
                    gvwMultipleEdit.Columns["PTRN_PART_NAME"].Visible = false;
                    gvwMultipleEdit.Columns["PART_NIKE_COMMENT"].Visible = false;
                    gvwMultipleEdit.Columns["COMBINE_YN"].Visible = false;
                    gvwMultipleEdit.Columns["STICKER_YN"].Visible = false;
                    gvwMultipleEdit.Columns["MDSL_CHK"].Visible = false;
                    gvwMultipleEdit.Columns["OTSL_CHK"].Visible = false;
                    gvwMultipleEdit.Columns["MXSXL_NUMBER"].Visible = false;
                    gvwMultipleEdit.Columns["PCX_SUPP_MAT_ID"].Visible = false;
                    //gvwMultipleEdit.Columns["CS_CD"].Visible = false;
                    gvwMultipleEdit.Columns["MCS_NUMBER"].Visible = false;
                    gvwMultipleEdit.Columns["PCX_MAT_ID"].Visible = false;
                    gvwMultipleEdit.Columns["MAT_CD"].Visible = false;
                    gvwMultipleEdit.Columns["VENDOR_NAME"].Visible = false;
                    gvwMultipleEdit.Columns["PCX_COLOR_ID"].Visible = false;
                    gvwMultipleEdit.Columns["PCX_PART_ID"].Visible = false;

                    break;
            }
        }

        /// <summary>
        /// Load BOM line items.
        /// </summary>
        /// <returns></returns>
        private DataTable BindDataSourceGridView()
        {
            DataSet ds;

            PKG_INTG_BOM.SELECT_BOM_TAIL_DATA pkgSelect = new PKG_INTG_BOM.SELECT_BOM_TAIL_DATA();
            pkgSelect.ARG_FACTORY = Factory;
            pkgSelect.ARG_WS_NO = WSNumber;
            pkgSelect.OUT_CURSOR = string.Empty;

            ds = Common.projectBaseForm.Exe_Select_PKG(pkgSelect);
            ds.Tables[0].Columns["PROCESS"].MaxLength = 500;        // Release the limit of length of the column.
            return ds.Tables[0];                                    // If there is no data in the BOM, just returns schema.
        }

        /// <summary>
        /// Verify that each BOM has the same number of parts.
        /// </summary>
        /// <returns></returns>
        private bool AreNumPartsSame()
        {
            try
            {
                // 선택된 BOM의 라인 아이템이 하나도 없는 경우 실행 불가 - Colorway ID를 가져올 수 없음
                if (gvwMultipleEdit.RowCount == 0)
                {
                    MessageBox.Show("Each BOM must have at least one row.");
                    return false;
                }

                // 각 BOM의 파트 개수를 저장할 배열
                int[] eachPartNumbers = new int[NumSelectedBOMs];

                // 각 BOM의 파트 개수
                int numberOfParts = 0;

                // i : 각 BOM의 첫 행의 RowHandle 번호
                for (int i = 0; i < NumSelectedBOMs; i++)
                {
                    string targetWsNo = gvwMultipleEdit.GetRowCellValue(i, "WS_NO").ToString();

                    // 그리드의 모든 행을 스캔
                    for (int j = 0; j < gvwMultipleEdit.RowCount; j++)
                    {
                        string wsNo = gvwMultipleEdit.GetRowCellValue(j, "WS_NO").ToString();

                        // WS_NO 단위로 개수 카운트
                        if (targetWsNo == wsNo)
                            numberOfParts++;
                    }

                    eachPartNumbers[i] = numberOfParts;

                    // 다음 BOM을 위해 초기화
                    numberOfParts = 0;
                }

                // 비교용 임시 변수
                int comparer = 0;

                // 각 BOM의 파트 개수가 같은지 확인
                foreach (int number in eachPartNumbers)
                {
                    // 첫 번째 BOM의 파트 개수가 기준
                    if (comparer == 0)
                        comparer = number;
                    else if (comparer != number)
                    {
                        MessageBox.Show("The number of parts of BOM's you selected is different.", "",
                            MessageBoxButtons.OK, MessageBoxIcon.Error, MessageBoxDefaultButton.Button1);
                        return false;
                    }
                }

                return true;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
                return false;
            }
        }

        /// <summary>
        /// 멀티 에딧모드에서 파트 묶음 단위로 액션이 가능한지 판별
        /// , 가장 상단의 기준이 되는 셀만 선택하도록 유도
        /// </summary>
        /// <returns></returns>
        private bool AllowMultiFunction()
        {
            if (view.RowCount.Equals(0))
            {
                Common.ShowMessageBox("There are no rows.", "W");
                return false; ;
            }

            if (view.GetSelectedCells().Length > 1 || !view.GetFocusedRowCellValue("COLOR_VER").ToString().Equals("VER1"))
            {
                Common.ShowMessageBox("Please select an one uppermost cell in a bundle of parts.", "W");
                return false;
            }

            string comparer = string.Empty;

            for (int i = 0; i < NumSelectedBOMs; i++)
            {
                if (i.Equals(0))
                    comparer = view.GetFocusedRowCellValue("PART_NAME").ToString();
                else
                {
                    if (!comparer.Equals(view.GetRowCellValue(view.FocusedRowHandle + i, "PART_NAME").ToString()))
                    {
                        Common.ShowMessageBox("There is a different part name in a bundle of parts.", "W");
                        return false;
                    }
                }
            }

            return true;
        }

        #endregion

        #region Form Events

        /// <summary>
        /// Release lock status from owner so that others can lock this BOM.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BOMEditing_FormClosing(object sender, FormClosingEventArgs e)
        {
            // Release lock status from owner so that others can lock this BOM.
            PKG_INTG_BOM.BOM_LOCK pkgSelect = new PKG_INTG_BOM.BOM_LOCK();
            pkgSelect.ARG_WORK_TYPE = "RETURN";
            pkgSelect.ARG_FACTORY = Factory;
            pkgSelect.ARG_WS_NO = WSNumber;
            pkgSelect.OUT_CURSOR = string.Empty;

            DataTable dataSource = Common.projectBaseForm.Exe_Select_PKG(pkgSelect).Tables[0];

            if (!dataSource.Rows[0]["FLAG"].ToString().Equals("COMPLETE"))
            {
                MessageBox.Show("Failed to normally close the form.", "",
                    MessageBoxButtons.OK, MessageBoxIcon.Error, MessageBoxDefaultButton.Button1);
            }

            // Refresh BOM header info.
            if (EditType.Equals("Single"))
                Common.RefreshHeaderInfo(Common.viewPCXBOMManagement, ParentRowhandle);
            else
                Common.projectBaseForm.QueryClick();
        }

        #endregion
    }
}