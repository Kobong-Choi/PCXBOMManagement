using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO;
using System.Collections;                       // ArrayList
using System.Diagnostics;                       // Process
using System.Xml;                               // XmlDocument

using DevExpress.XtraGrid;                      // GridControl
using DevExpress.XtraGrid.Columns;              // GridColumn
using DevExpress.XtraGrid.Views.Base;           // GridCell
using DevExpress.XtraGrid.Views.Grid;           // GridView
using DevExpress.XtraGrid.Views.Grid.ViewInfo;  // GridHitInfo
using DevExpress.Utils;                         // DXMouseEventArgs
using DevExpress.XtraEditors;                   // GridLookUpEdit
using DevExpress.XtraSplashScreen;              // XtraSplashScreen

using CSI.Client.ProjectBaseForm;               // ProjectBaseForm Class
using CSI.PCC.PCX.COM;                          // Common Class
using CSI.PCC.PCX.PACKAGE;                      // Package Class

using JPlatform.Client.JBaseForm;

using Excel = Microsoft.Office.Interop.Excel;   // Excel Manage

using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace CSI.PCC.PCX
{
    public partial class LogicBOMEditing : DevExpress.XtraEditors.XtraForm
    {
        public ProjectBaseForm projectBaseForm = Common.projectBaseForm;

        public string Factory { get; set; }
        public string WorksheetNumbers { get; set; }
        public string CSBOMStatus { get; set; }
        public int ParentRowhandle { get; set; }

        public LogicBOMEditing()
        {
            InitializeComponent();
        }

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);

            // Bind data to lookUpEdit for part type.
            repositoryItemLookUpEdit1.DataSource = Common.CreatePartTypeLookUpSource();

            BindDataSourceToGridView("Header");
            BindDataSourceToGridView("Data");
        }

        /// <summary>
        /// Bind BOM data to grid view.
        /// </summary>
        private void BindDataSourceToGridView(string type)
        {
            DataSet ds = null;

            if (type == "Header")
            {
                PKG_INTG_BOM.BOM_IMPORT pkgSelect = new PKG_INTG_BOM.BOM_IMPORT();
                pkgSelect.ARG_WORK_TYPE = "HeadInfo";
                pkgSelect.ARG_FACTORY = Factory;
                pkgSelect.ARG_WS_NO = WorksheetNumbers;
                pkgSelect.ARG_UPD_USER = Common.sessionID;
                pkgSelect.OUT_CURSOR = string.Empty;

                ds = Common.projectBaseForm.Exe_Select_PKG(pkgSelect);

                if (ds.Tables[0].Rows.Count > 0)
                    grdHeader.DataSource = ds.Tables[0];
            }
            else if (type == "Data")
            {
                PKG_INTG_BOM.SELECT_BOM_TAIL_DATA pkgSelect = new PKG_INTG_BOM.SELECT_BOM_TAIL_DATA();
                pkgSelect.ARG_FACTORY = Factory;
                pkgSelect.ARG_WS_NO = WorksheetNumbers;
                pkgSelect.OUT_CURSOR = String.Empty;

                ds = Common.projectBaseForm.Exe_Select_PKG(pkgSelect);

                if (ds.Tables[0].Rows.Count > 0)
                    grdData.DataSource = ds.Tables[0];
            }
        }

        #region Button Event

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnSave_Click(object sender, EventArgs e)
        {
            GridView view = gvwData;
            ArrayList arrayList = new ArrayList();

            // To keep focusing the row which was selected before save.
            GridColumn fcsdColumnBeforeSave = view.FocusedColumn;
            int fcsdRowHandleBeforeSave = view.FocusedRowHandle;

            if (Common.IsFilterApplied(view))
                return;

            if (CSBOMStatus.Equals("C"))
            {
                MessageBox.Show("This is already confirmed. Please click 'Confirm' again.", "Warning",
                            MessageBoxButtons.OK, MessageBoxIcon.Warning, MessageBoxDefaultButton.Button1);
            }

            try
            {
                SplashScreenManager.ShowForm(this, typeof(MainWaitForm), false, false, false);

                ResetSortOrder(view);

                for (int i = 0; i < view.RowCount; i++)
                {
                    #region Set parameters for calling procedure.

                    PKG_INTG_BOM.UPDATE_BOM_TAIL pkgUpdate = new PKG_INTG_BOM.UPDATE_BOM_TAIL();
                    pkgUpdate.ARG_FACTORY = view.GetRowCellValue(i, "FACTORY").ToString();
                    pkgUpdate.ARG_WS_NO = view.GetRowCellValue(i, "WS_NO").ToString();
                    pkgUpdate.ARG_PART_SEQ = view.GetRowCellValue(i, "PART_SEQ").ToString();
                    pkgUpdate.ARG_PART_NIKE_NO = view.GetRowCellValue(i, "PART_NIKE_NO").ToString();
                    pkgUpdate.ARG_PART_NIKE_COMMENT = view.GetRowCellValue(i, "PART_NIKE_COMMENT").ToString();
                    pkgUpdate.ARG_PART_CD = view.GetRowCellValue(i, "PART_CD").ToString();
                    pkgUpdate.ARG_PART_NAME = view.GetRowCellValue(i, "PART_NAME").ToString().Trim();
                    pkgUpdate.ARG_PART_TYPE = view.GetRowCellValue(i, "PART_TYPE").ToString().Trim();
                    pkgUpdate.ARG_BTTM = "";
                    pkgUpdate.ARG_MXSXL_NUMBER = view.GetRowCellValue(i, "MXSXL_NUMBER").ToString();
                    pkgUpdate.ARG_CS_CD = "";
                    pkgUpdate.ARG_MAT_CD = view.GetRowCellValue(i, "MAT_CD").ToString();
                    pkgUpdate.ARG_MAT_NAME = view.GetRowCellValue(i, "MAT_NAME").ToString();
                    pkgUpdate.ARG_MAT_COMMENT = "";
                    pkgUpdate.ARG_MCS_NUMBER = view.GetRowCellValue(i, "MCS_NUMBER").ToString();
                    pkgUpdate.ARG_COLOR_CD = view.GetRowCellValue(i, "COLOR_CD").ToString();
                    pkgUpdate.ARG_COLOR_NAME = view.GetRowCellValue(i, "COLOR_NAME").ToString();
                    pkgUpdate.ARG_COLOR_COMMENT = "";
                    pkgUpdate.ARG_SORT_NO = view.GetRowCellValue(i, "SORT_NO").ToString();
                    pkgUpdate.ARG_REMARKS = view.GetRowCellValue(i, "REMARKS").ToString().Trim();
                    pkgUpdate.ARG_UPD_USER = Common.sessionID;
                    pkgUpdate.ARG_PTRN_PART_NAME = view.GetRowCellValue(i, "PTRN_PART_NAME").ToString().Trim();
                    pkgUpdate.ARG_PCX_MAT_ID = view.GetRowCellValue(i, "PCX_MAT_ID").ToString();
                    pkgUpdate.ARG_PCX_SUPP_MAT_ID = view.GetRowCellValue(i, "PCX_SUPP_MAT_ID").ToString();
                    pkgUpdate.ARG_PCX_COLOR_ID = view.GetRowCellValue(i, "PCX_COLOR_ID").ToString();
                    pkgUpdate.ARG_ROW_STATUS = view.GetRowCellValue(i, "ROW_STATUS").ToString();
                    pkgUpdate.ARG_PROCESS = "";
                    pkgUpdate.ARG_VENDOR_NAME = view.GetRowCellValue(i, "VENDOR_NAME").ToString().ToUpper();
                    pkgUpdate.ARG_COMBINE_YN = "N";
                    pkgUpdate.ARG_STICKER_YN = "N";
                    pkgUpdate.ARG_PTRN_PART_CD = view.GetRowCellValue(i, "PTRN_PART_CD").ToString();
                    pkgUpdate.ARG_MDSL_CHK = "N";
                    pkgUpdate.ARG_OTSL_CHK = "N";
                    pkgUpdate.ARG_CELL_COLOR = view.GetRowCellValue(i, "CELL_COLOR").ToString();
                    pkgUpdate.ARG_CS_PTRN_CD = "";
                    pkgUpdate.ARG_CS_PTRN_NAME = "";
                    pkgUpdate.ARG_LOGIC_GROUP = view.GetRowCellValue(i, "LOGIC_GROUP").ToString();
                    pkgUpdate.ARG_MAT_FORECAST_PRCNT = Convert.ToDouble(view.GetRowCellValue(i, "MAT_FORECAST_PRCNT"));
                    pkgUpdate.ARG_COLOR_FORECAST_PRCNT = Convert.ToDouble(view.GetRowCellValue(i, "COLOR_FORECAST_PRCNT"));
                    pkgUpdate.ARG_ENCODED_CMT = "";

                    arrayList.Add(pkgUpdate);

                    #endregion
                }

                // Call procedure to save line items of BOM.
                if (projectBaseForm.Exe_Modify_PKG(arrayList) == null)
                    MessageBox.Show("Failed to update data.", "Error",
                        MessageBoxButtons.OK, MessageBoxIcon.Error, MessageBoxDefaultButton.Button1);
                else
                {
                    arrayList.Clear();

                    // Update the status of BOM header to 'Save'.
                    PKG_INTG_BOM.UPDATE_BOM_CFM_INFO pkgUpdate = new PKG_INTG_BOM.UPDATE_BOM_CFM_INFO();
                    pkgUpdate.ARG_FACTORY = Factory;
                    pkgUpdate.ARG_WS_NO = WorksheetNumbers;
                    pkgUpdate.ARG_CS_BOM_CFM = "N";
                    pkgUpdate.ARG_CBD_YN = "Y";
                    pkgUpdate.ARG_UPD_USER = Common.sessionID;

                    arrayList.Add(pkgUpdate);

                    if (projectBaseForm.Exe_Modify_PKG(arrayList) == null)
                    {
                        MessageBox.Show("Failed to update BOM status to 'Save'.");
                        return;
                    }
                    else
                    {
                        CSBOMStatus = "N";
                    }

                    BindDataSourceToGridView("Data");

                    SetFocusColumn(view, fcsdRowHandleBeforeSave, fcsdColumnBeforeSave.FieldName, false);

                    SplashScreenManager.CloseForm(false);
                    MessageBox.Show("Complete");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
                return;
            }
            finally
            {
                SplashScreenManager.CloseForm(false);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnConfirm_Click(object sender, EventArgs e)
        {
            // See if the BOM has already been confirmed.
            if (CSBOMStatus.Equals("C"))
            {
                MessageBox.Show("This BOM has been already confirmed.", "",
                    MessageBoxButtons.OK, MessageBoxIcon.Information, MessageBoxDefaultButton.Button1);
                return;
            }

            try
            {
                if (Common.IsFilterApplied(gvwData)) return;

                if (HasUnsavedItems(gvwData) == false) return;

                #region 백업

                //PKG_INTG_BOM.BOM_IMPORT pkgTransfer = new PKG_INTG_BOM.BOM_IMPORT();
                //pkgTransfer.ARG_WORK_TYPE = "ValidateTransfer";
                //pkgTransfer.ARG_FACTORY = bomHeader.Factory;
                //pkgTransfer.ARG_WS_NO = bomHeader.WsNo;
                //pkgTransfer.ARG_UPD_USER = Common.userID;
                //pkgTransfer.OUT_CURSOR = string.Empty;

                //dataSource = projectBaseForm.Exe_Select_PKG(pkgTransfer).Tables[0];

                //if (dataSource.Rows.Count > 0)
                //{
                //    for (int rowHandle = 0; rowHandle < dataSource.Rows.Count; rowHandle++)
                //    {
                //        DataRow row = dataSource.Rows[rowHandle];

                //        foreach (DataColumn column in dataSource.Columns)
                //        {
                //            // Skip some columns which don't need to be validated.
                //            if (column.ColumnName == "PART_NAME" || column.ColumnName == "CS_PTRN_PART_ID")
                //                continue;

                //            if (row[column.ColumnName].ToString() == "")
                //            {
                //                if (column.ColumnName == "PART_ID")
                //                {
                //                    MessageBox.Show("Please input PCX Part ID", "Warining",
                //                        MessageBoxButtons.OK, MessageBoxIcon.Warning, MessageBoxDefaultButton.Button1);
                //                }
                //                else if (column.ColumnName == "PCX_MAT_ID" || column.ColumnName == "PCX_SUPP_MAT_ID")
                //                {
                //                    MessageBox.Show("Please input PCX Material ID of " + row["PART_NAME"].ToString(), "Warining",
                //                        MessageBoxButtons.OK, MessageBoxIcon.Warning, MessageBoxDefaultButton.Button1);
                //                }
                //                else if (column.ColumnName == "PART_TYPE")
                //                {
                //                    MessageBox.Show("Invalid part type about " + row["PART_NAME"].ToString(), "Warining",
                //                        MessageBoxButtons.OK, MessageBoxIcon.Warning, MessageBoxDefaultButton.Button1);
                //                }

                //                Common.FocusCellToModify(gvwData, rowHandle, column.ColumnName, false);
                //                return;
                //            }
                //        }
                //    }
                //}
                //else
                //{
                //    MessageBox.Show("There are no items in BOM.", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Error,
                //        MessageBoxDefaultButton.Button1);
                //    return;
                //}

                #endregion

                if (ValidateBeforeConfirm(GetDataSource("HeadInfo")) == false) return;

                SplashScreenManager.ShowForm(this, typeof(MainWaitForm), false, false, false);
                SplashScreenManager.Default.SendCommand(MainWaitForm.SplashScreenCommand.TransferToNCF, "");

                #region Transfer data

                ArrayList arrayList = new ArrayList();
                string pcxKey = string.Empty;

                // Get PCX Key
                // If the key was already created, this would return that.
                // If not, it returns a new key.
                PKG_INTG_BOM.BOM_IMPORT pkgGetKey = new PKG_INTG_BOM.BOM_IMPORT();
                pkgGetKey.ARG_WORK_TYPE = "GetPCXKey";
                pkgGetKey.ARG_FACTORY = Factory;
                pkgGetKey.ARG_WS_NO = WorksheetNumbers;
                pkgGetKey.ARG_UPD_USER = Common.sessionID;
                pkgGetKey.OUT_CURSOR = string.Empty;

                DataTable dataSource = projectBaseForm.Exe_Select_PKG(pkgGetKey).Tables[0];

                if (dataSource.Rows.Count > 0)
                    pcxKey = dataSource.Rows[0][0].ToString();

                // Transfer data to table's related to NCF process.
                PKG_INTG_BOM.TRANSFER_DATA_TO_PCXBOM pkgInsert = new PKG_INTG_BOM.TRANSFER_DATA_TO_PCXBOM();
                pkgInsert.ARG_PCX_KEY = pcxKey;
                pkgInsert.ARG_FACTORY = Factory;
                pkgInsert.ARG_WS_NO = WorksheetNumbers;
                pkgInsert.ARG_UPD_USER = Common.sessionID;

                arrayList.Add(pkgInsert);

                if (projectBaseForm.Exe_Modify_PKG(arrayList) == null)
                {
                    MessageBox.Show("Failed to transfer data to costing. Please ask IT.", "",
                        MessageBoxButtons.OK, MessageBoxIcon.Warning, MessageBoxDefaultButton.Button1);
                    return;
                }

                #endregion

                arrayList.Clear();

                #region Update BOM Status

                PKG_INTG_BOM.UPDATE_BOM_CFM_INFO pkgUpdate = new PKG_INTG_BOM.UPDATE_BOM_CFM_INFO();
                pkgUpdate.ARG_FACTORY = Factory;
                pkgUpdate.ARG_WS_NO = WorksheetNumbers;
                pkgUpdate.ARG_CS_BOM_CFM = "C";
                pkgUpdate.ARG_CBD_YN = "Y";
                pkgUpdate.ARG_UPD_USER = Common.sessionID;

                arrayList.Add(pkgUpdate);

                if (projectBaseForm.Exe_Modify_PKG(arrayList) == null)
                {
                    MessageBox.Show("Failed to change the BOM status to 'Confirm'.");
                    return;
                }
                else
                {
                    CSBOMStatus = "C";
                }

                #endregion

                BindDataSourceToGridView("Data");

                SplashScreenManager.CloseForm(false);

                MessageBox.Show("Complete");
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
            finally
            {
                SplashScreenManager.CloseForm(false);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnExport_Click(object sender, EventArgs e)
        {
            DataTable srcOfHeader = new DataTable();
            DataTable srcOfData = new DataTable();

            srcOfHeader = GetDataSource("HeadInfo");

            if (srcOfHeader.Rows.Count > 0)
            {
                if (ValidateBeforeConfirm(srcOfHeader))
                {
                    srcOfData = GetDataSource("BOMData");

                    if (srcOfData.Rows.Count > 0)
                    {
                        SaveFileDialog sDialog = new SaveFileDialog();

                        sDialog.Title = "Specify the path where you want to save the file.";
                        sDialog.DefaultExt = "json";
                        sDialog.Filter = "JSON File (*.json)|*.json";

                        DataTable captionInfo = GetDataSource("BOMCaption");

                        if (captionInfo != null)
                        {
                            string season = captionInfo.Rows[0]["SEASON"].ToString();
                            string sampleType = captionInfo.Rows[0]["SAMPLE_TYPE"].ToString();
                            string devName = captionInfo.Rows[0]["DEV_NAME"].ToString();
                            string colorwayId = captionInfo.Rows[0]["DEV_COLORWAY_ID"].ToString();

                            // Set default file name.
                            sDialog.FileName = "NBY_" + season + "_" + sampleType + "_" + devName + "_" + colorwayId;
                        }

                        // Get the path where an user want to save the file.
                        if (sDialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                        {
                            var JSONFile = new JObject();
                            var logicBOMHeader = new JObject();
                            var logicBOMData = new JArray();
                            var cycleYear = new JArray();

                            #region JObject for logic BOM header.

                            if (srcOfHeader.Rows[0]["CYCLE_YEAR"].ToString().Contains(','))
                            {
                                // When Cycle Year has more than one.
                                string[] years = srcOfHeader.Rows[0]["CYCLE_YEAR"].ToString().Split(',');

                                foreach (string year in years)
                                    cycleYear.Add(year);
                            }
                            else
                            {
                                // When Cycle Year has one value.
                                cycleYear.Add(srcOfHeader.Rows[0]["CYCLE_YEAR"].ToString());
                            }

                            logicBOMHeader.Add("objectId", Common.NulltoString(srcOfHeader.Rows[0]["OBJ_ID"]));
                            logicBOMHeader.Add("objectType", Common.NulltoString(srcOfHeader.Rows[0]["OBJ_TYPE"]));
                            logicBOMHeader.Add("bomContractVersion", Common.NulltoString(srcOfHeader.Rows[0]["BOM_CONTRACT_VER"]));
                            logicBOMHeader.Add("developmentStyleIdentifier", Common.NulltoString(srcOfHeader.Rows[0]["DEV_STYLE_ID"]));
                            logicBOMHeader.Add("developmentStyleTypeIdentifier", Common.NulltoString(srcOfHeader.Rows[0]["DEV_STYLE_TYPE_ID"]));
                            logicBOMHeader.Add("developmentColorwayIdentifier", Common.NulltoString(srcOfHeader.Rows[0]["DEV_COLORWAY_ID"]));
                            logicBOMHeader.Add("colorwayName", Common.NulltoString(srcOfHeader.Rows[0]["COLORWAY_NAME"]));
                            logicBOMHeader.Add("sourcingConfigurationIdentifier", Common.NulltoString(srcOfHeader.Rows[0]["SRC_CONFIG_ID"]));
                            logicBOMHeader.Add("sourcingConfigurationName", Common.NulltoString(srcOfHeader.Rows[0]["SRC_CONFIG_NAME"]));
                            logicBOMHeader.Add("logicBomIdentifier", Common.NulltoString(srcOfHeader.Rows[0]["BOM_ID"]));
                            logicBOMHeader.Add("logicBOMName", Common.NulltoString(srcOfHeader.Rows[0]["BOM_NAME"]));
                            logicBOMHeader.Add("logicBOMVersion", Common.NulltoString(srcOfHeader.Rows[0]["BOM_VERSION_NUM"]));
                            logicBOMHeader.Add("logicBomDescription", Common.NulltoString(srcOfHeader.Rows[0]["BOM_DESC"]));
                            logicBOMHeader.Add("logicBomComments", Common.NulltoString(srcOfHeader.Rows[0]["BOM_COMMENTS"]));
                            logicBOMHeader.Add("logicBOMStatus", Common.NulltoString(srcOfHeader.Rows[0]["BOM_STATUS_IND"]));
                            logicBOMHeader.Add("logicBOMStateIdentifier", Common.NulltoString(srcOfHeader.Rows[0]["LOGIC_BOM_STATE_ID"]));
                            logicBOMHeader.Add("logicBOMGateIdentifier", Common.NulltoString(srcOfHeader.Rows[0]["LOGIC_BOM_GATE_ID"]));
                            logicBOMHeader.Add("createTimestamp", Common.NulltoString(srcOfHeader.Rows[0]["CREATE_TIME_STAMP"]));
                            logicBOMHeader.Add("changeTimestamp", Common.NulltoString(srcOfHeader.Rows[0]["CHANGE_TIME_STAMP"]));
                            logicBOMHeader.Add("createdBy", Common.NulltoString(srcOfHeader.Rows[0]["CREATED_BY"]));
                            logicBOMHeader.Add("modifiedBy", Common.NulltoString(srcOfHeader.Rows[0]["MODIFIED_BY"]));
                            logicBOMHeader.Add("styleNumber", Common.NulltoString(srcOfHeader.Rows[0]["STYLE_NUMBER"]));
                            logicBOMHeader.Add("styleName", Common.NulltoString(srcOfHeader.Rows[0]["STYLE_NAME"]));
                            logicBOMHeader.Add("modelIdentifier", Common.NulltoString(srcOfHeader.Rows[0]["MODEL_ID"]));
                            logicBOMHeader.Add("cycleYear", cycleYear);
                            logicBOMHeader.Add("productId", Common.NulltoString(srcOfHeader.Rows[0]["PRODUCT_ID"]));
                            logicBOMHeader.Add("productCode", Common.NulltoString(srcOfHeader.Rows[0]["PRODUCT_CODE"]));
                            logicBOMHeader.Add("colorwayCode", Common.NulltoString(srcOfHeader.Rows[0]["COLORWAY_CODE"]));

                            #endregion

                            #region JArray for logic BOM data.

                            foreach (DataRow row in srcOfData.Rows)
                            {
                                var lineItem = new JObject();

                                lineItem.Add("logicBOMLineItemNumber", row["PCC_SORT_NO"].ToString());
                                lineItem.Add("logicSectionIdentifier",
                                    Common.NulltoString(ConvertPartTypeToID(row["PART_TYPE"].ToString())));
                                lineItem.Add("partNameIdentifier", row["PART_ID"].ToString());
                                lineItem.Add("patternPartIdentifier", row["PTRN_PART_ID"].ToString());
                                lineItem.Add("suppliedMaterialIdentifier", row["SUPP_MAT_ID"].ToString());
                                lineItem.Add("materialItemIdentifier", row["MAT_ID"].ToString());
                                lineItem.Add("materialItemPlaceholderDescription", "");
                                lineItem.Add("colorIdentifier", row["PCX_COLOR_ID"].ToString());
                                lineItem.Add("colorPlaceholderDescription", "");

                                string colorName = row["COLOR_NAME"].ToString();
                                bool isMultipleColor = false;

                                if (colorName != "")
                                {
                                    if (colorName.Contains("MC-"))
                                        isMultipleColor = true;
                                }

                                lineItem.Add("suppliedMaterialColorIsMultipleColors", (isMultipleColor == true) ? "true" : "false");
                                lineItem.Add("suppliedMaterialColorIdentifier", "");
                                lineItem.Add("logicBOMLineItemComments", row["PCX_COMMENT"].ToString());
                                lineItem.Add("factoryInHouseIndicator", "");
                                lineItem.Add("countyOfOriginIdentifier", "");
                                lineItem.Add("actualDimensionDescription", "");
                                lineItem.Add("isoMeasurementCode", "");
                                lineItem.Add("netUsageNumber", "0");
                                lineItem.Add("wasteUsageNumber", "0");
                                lineItem.Add("partYield", "0");
                                lineItem.Add("consumptionConversionRate", "0");
                                lineItem.Add("lineItemDefectPercentNumber", "0");
                                lineItem.Add("unitPriceISOMeasurementCode", "");
                                lineItem.Add("currencyCode", "");
                                lineItem.Add("factoryUnitPrice", "0");
                                lineItem.Add("unitPriceUpchargeNumber", "0");
                                lineItem.Add("unitPriceUpchargeDescription", "");
                                lineItem.Add("freightTermIdentifier", "");
                                lineItem.Add("landedCostPercentNumber", "0");
                                lineItem.Add("pccSortOrder", row["PCC_SORT_NO"].ToString());
                                lineItem.Add("logicGroup", row["LOGIC_GROUP"].ToString());
                                lineItem.Add("materialForecastPercent", row["MAT_FORECAST_PRCNT"].ToString());
                                lineItem.Add("colorForecastPercent", row["COLOR_FORECAST_PRCNT"].ToString());

                                logicBOMData.Add(lineItem);
                            }

                            #endregion

                            JSONFile.Add("Logic BOM Header", logicBOMHeader);
                            JSONFile.Add("Logic BOM Data", logicBOMData);

                            // Export to JSON file.
                            System.IO.File.WriteAllText(sDialog.FileName, JSONFile.ToString());

                            MessageBox.Show("Complete", "", MessageBoxButtons.OK, MessageBoxIcon.Information, MessageBoxDefaultButton.Button1);
                        }
                    }
                    else
                    {
                        MessageBox.Show("Failed to get BOM data.", "",
                        MessageBoxButtons.OK, MessageBoxIcon.Error, MessageBoxDefaultButton.Button1);
                    }
                }
                else
                    return;
            }
            else
            {
                MessageBox.Show("Failed to get BOM header.", "",
                    MessageBoxButtons.OK, MessageBoxIcon.Error, MessageBoxDefaultButton.Button1);
                return;
            }
        }

        /// <summary>
        /// Reset sort order of the logic BOM.
        /// </summary>
        /// <param name="view"></param>
        private void ResetSortOrder(GridView view)
        {
            try
            {
                view.CellValueChanged -= new CellValueChangedEventHandler(CustomCellValueChanged);

                int sortNo = 1;
                for (int rowHandle = 0; rowHandle < view.RowCount; rowHandle++)
                {
                    // Skip deleted row.
                    if (view.GetRowCellValue(rowHandle, "ROW_STATUS").ToString() != "D")
                    {
                        view.SetRowCellValue(rowHandle, "SORT_NO", sortNo);
                        sortNo++;
                    }
                }

                view.CellValueChanged += new CellValueChangedEventHandler(CustomCellValueChanged);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
                return;
            }
        }

        /// <summary>
        /// 매개변수에 지정된 셀 선택
        /// </summary>
        /// <param name="rowHandle"></param>
        /// <param name="columnName"></param>
        private void SetFocusColumn(GridView view, int rowHandle, string columnName, bool showing)
        {
            // 기존에 선택된 셀의 포커싱 취소
            view.ClearSelection();
            // 입력해야할 셀 선택
            view.SelectCell(rowHandle, view.Columns[columnName]);
            // 포커스할 로우핸들 설정
            view.FocusedRowHandle = rowHandle;
            // 포커스할 컬럼 설정
            view.FocusedColumn = view.Columns[columnName];
            // 에디팅 모드 띄움
            if (showing == true)
                view.ShowEditor();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="partType"></param>
        /// <returns></returns>
        private string ConvertPartTypeToID(string partType)
        {
            string sectionId = string.Empty;

            switch (partType)
            {
                case "UPPER":
                    sectionId = "1";
                    break;

                case "MIDSOLE":
                    sectionId = "2";
                    break;

                case "OUTSOLE":
                    sectionId = "3";
                    break;

                case "AIRBAG":
                    sectionId = "4";
                    break;

                case "PACKAGING":
                    sectionId = "5";
                    break;
            }

            return sectionId;
        }

        /// <summary>
        /// 데이터베이스로부터 소스 데이터를 가져옴
        /// </summary>
        /// <param name="workType"></param>
        /// <returns></returns>
        private DataTable GetDataSource(string workType)
        {
            PKG_INTG_BOM.BOM_IMPORT pkgSelect = new PKG_INTG_BOM.BOM_IMPORT();
            pkgSelect.ARG_WORK_TYPE = workType;
            pkgSelect.ARG_FACTORY = Factory;
            pkgSelect.ARG_WS_NO = WorksheetNumbers;
            pkgSelect.ARG_UPD_USER = Common.sessionID;
            pkgSelect.OUT_CURSOR = string.Empty;

            DataTable dataSource = projectBaseForm.Exe_Select_PKG(pkgSelect).Tables[0];

            return dataSource;
        }

        /// <summary>
        /// Follow NBY validation.
        /// </summary>
        /// <returns></returns>
        private bool ValidateBeforeConfirm(DataTable dataSource)
        {
            // Required Attributes for NBY Logic BOM Import.
            string objectID = dataSource.Rows[0]["OBJ_ID"].ToString();
            string devStyleID = dataSource.Rows[0]["DEV_STYLE_ID"].ToString();
            string devColorwayID = dataSource.Rows[0]["DEV_COLORWAY_ID"].ToString();
            string sourceConfigID = dataSource.Rows[0]["SRC_CONFIG_ID"].ToString();
            string logicBOMID = dataSource.Rows[0]["BOM_ID"].ToString();
            string logicBOMStateID = dataSource.Rows[0]["LOGIC_BOM_STATE_ID"].ToString();
            string logicBOMGateID = dataSource.Rows[0]["LOGIC_BOM_GATE_ID"].ToString();

            if (objectID == "")
            {
                // When this is copied logic BOM.
                MessageBox.Show("Please upload logic BOM again by using 'Matching' function.");
                return false;
            }
            else if (devStyleID == "")
            {
                MessageBox.Show("Please input Dev. Style ID.");
                return false;
            }
            else if (devColorwayID == "")
            {
                MessageBox.Show("Please input Dev. Colorway ID.");
                return false;
            }
            else if (sourceConfigID == "")
            {
                MessageBox.Show("Please input Sourcing Configuration ID.");
                return false;
            }
            else if (logicBOMID == "")
            {
                MessageBox.Show("Please input Logic BOM ID.");
                return false;
            }
            else if (logicBOMStateID == "")
            {
                MessageBox.Show("Please input Logic BOM State ID.");
                return false;
            }
            else if (logicBOMGateID == "")
            {
                MessageBox.Show("Please input Logic BOM Gate ID.");
                return false;
            }

            return true;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnMatch_Click(object sender, EventArgs e)
        {
            OpenFileDialog dialog = new OpenFileDialog();
            dialog.Title = "Upload";
            dialog.Multiselect = false;
            dialog.RestoreDirectory = true;
            dialog.Filter = "JSON 파일|*.json";

            if (dialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                string fileName = dialog.FileName;

                using (StreamReader reader = File.OpenText(fileName))
                {
                    using (JsonTextReader jsonReader = new JsonTextReader(reader))
                    {
                        JObject jObjects = (JObject)JToken.ReadFrom(jsonReader);

                        DataSet cnvtObjects = ConvertJson2Xml(jObjects.ToString());

                        DataTable dtLogicBOMHeader = cnvtObjects.Tables["Logic BOM Header"];
                        DataTable dtLogicBOMCycleYear = new DataTable();
                        DataTable dtLogicBOMData = new DataTable();

                        string cycleYear = string.Empty;

                        if (cnvtObjects.Tables.Count > 2)
                        {
                            // When cycle year has values more than one.
                            dtLogicBOMData = cnvtObjects.Tables["Logic BOM Data"];
                            dtLogicBOMCycleYear = cnvtObjects.Tables["cycleYear"];

                            foreach (DataRow row in dtLogicBOMCycleYear.Rows)
                            {
                                if (cycleYear.Equals(""))
                                    cycleYear = row[0].ToString();
                                else
                                    cycleYear += "," + row[0].ToString();
                            }
                        }
                        else
                        {
                            // When cycle year has only one value.
                            dtLogicBOMData = cnvtObjects.Tables["Logic BOM Data"];
                            cycleYear = dtLogicBOMHeader.Rows[0]["cycleYear"].ToString();
                        }

                        DataRow headerValues = dtLogicBOMHeader.Rows[0];
                        ArrayList arrayList = new ArrayList();

                        #region 원본 헤더 저장

                        PKG_INTG_BOM.INSERT_REPLACE_JSON_ORG_HEADER pkgInsertOrgHead = new PKG_INTG_BOM.INSERT_REPLACE_JSON_ORG_HEADER();
                        pkgInsertOrgHead.ARG_FACTORY = Factory;
                        pkgInsertOrgHead.ARG_WS_NO = WorksheetNumbers;
                        pkgInsertOrgHead.ARG_OBJ_ID = headerValues["objectId"].ToString();
                        pkgInsertOrgHead.ARG_BOM_PART_UUID = "";
                        pkgInsertOrgHead.ARG_OBJ_TYPE = headerValues["objectType"].ToString();
                        pkgInsertOrgHead.ARG_BOM_CONTRACT_VER = headerValues["bomContractVersion"].ToString();
                        pkgInsertOrgHead.ARG_DEV_STYLE_ID = headerValues["developmentStyleIdentifier"].ToString();
                        pkgInsertOrgHead.ARG_DEV_COLORWAY_ID = headerValues["developmentColorwayIdentifier"].ToString();
                        pkgInsertOrgHead.ARG_COLORWAY_NAME = headerValues["colorwayName"].ToString();
                        pkgInsertOrgHead.ARG_SRC_CONFIG_ID = headerValues["sourcingConfigurationIdentifier"].ToString();
                        pkgInsertOrgHead.ARG_SRC_CONFIG_NAME = headerValues["sourcingConfigurationName"].ToString();
                        pkgInsertOrgHead.ARG_SEASON_ID = "";
                        pkgInsertOrgHead.ARG_BOM_ID = headerValues["logicBomIdentifier"].ToString();
                        pkgInsertOrgHead.ARG_BOM_NAME = headerValues["logicBOMName"].ToString();
                        pkgInsertOrgHead.ARG_BOM_VERSION_NUM = headerValues["logicBOMVersion"].ToString();
                        pkgInsertOrgHead.ARG_BOM_DESC = headerValues["logicBomDescription"].ToString();
                        pkgInsertOrgHead.ARG_BOM_COMMENTS = headerValues["logicBomComments"].ToString();
                        pkgInsertOrgHead.ARG_BOM_STATUS_IND = headerValues["logicBOMStatus"].ToString();
                        pkgInsertOrgHead.ARG_CREATE_TIME_STAMP = headerValues["createTimestamp"].ToString();
                        pkgInsertOrgHead.ARG_CHANGE_TIME_STAMP = headerValues["changeTimestamp"].ToString();
                        pkgInsertOrgHead.ARG_CREATED_BY = headerValues["createdBy"].ToString();
                        pkgInsertOrgHead.ARG_MODIFIED_BY = headerValues["modifiedBy"].ToString();
                        pkgInsertOrgHead.ARG_STYLE_NUMBER = headerValues["styleNumber"].ToString();
                        pkgInsertOrgHead.ARG_STYLE_NAME = headerValues["styleName"].ToString();
                        pkgInsertOrgHead.ARG_MODEL_ID = headerValues["modelIdentifier"].ToString();
                        pkgInsertOrgHead.ARG_GENDER = "";
                        pkgInsertOrgHead.ARG_AGE = "";
                        pkgInsertOrgHead.ARG_PRODUCT_ID = headerValues["productId"].ToString();
                        pkgInsertOrgHead.ARG_PRODUCT_CODE = headerValues["productCode"].ToString();
                        pkgInsertOrgHead.ARG_COLORWAY_CODE = headerValues["colorwayCode"].ToString();
                        pkgInsertOrgHead.ARG_DEV_STYLE_TYPE_ID = headerValues["developmentStyleTypeIdentifier"].ToString();
                        pkgInsertOrgHead.ARG_LOGIC_BOM_STATE_ID = headerValues["logicBOMStateIdentifier"].ToString();
                        pkgInsertOrgHead.ARG_LOGIC_BOM_GATE_ID = headerValues["logicBOMGateIdentifier"].ToString();
                        pkgInsertOrgHead.ARG_CYCLE_YEAR = cycleYear;

                        arrayList.Add(pkgInsertOrgHead);

                        if (projectBaseForm.Exe_Modify_PKG(arrayList) == null)
                        {
                            MessageBox.Show("Failed to replace JSON Org. header.");
                            return;
                        }

                        #endregion

                        #region 원본 라인 아이템 저장

                        PKG_INTG_BOM.BOM_IMPORT pkgSelect = new PKG_INTG_BOM.BOM_IMPORT();
                        pkgSelect.ARG_WORK_TYPE = "isExisting";
                        pkgSelect.ARG_FACTORY = Factory;
                        pkgSelect.ARG_WS_NO = WorksheetNumbers;
                        pkgSelect.ARG_UPD_USER = Common.sessionID;
                        pkgSelect.OUT_CURSOR = string.Empty;

                        string flag = string.Empty;
                        DataTable dtResult = projectBaseForm.Exe_Select_PKG(pkgSelect).Tables[0];

                        if (dtResult.Rows.Count > 0)
                            flag = dtResult.Rows[0][0].ToString();

                        if (flag == "PASS")
                        {
                            ArrayList list = new ArrayList();
                            int sequence = 1;

                            foreach (DataRow dr in dtLogicBOMData.Rows)
                            {
                                PKG_INTG_BOM.INSERT_BOM_TAIN_JSON_ORG pkgInsertOrgData = new PKG_INTG_BOM.INSERT_BOM_TAIN_JSON_ORG();
                                pkgInsertOrgData.ARG_FACTORY = Factory;
                                pkgInsertOrgData.ARG_WS_NO = WorksheetNumbers;
                                pkgInsertOrgData.ARG_BOM_LINE_SORT_SEQ = dr["logicBOMLineItemNumber"].ToString();
                                pkgInsertOrgData.ARG_BOM_SECTION_ID = dr["logicSectionIdentifier"].ToString();
                                pkgInsertOrgData.ARG_PART_NAME_ID = dr["partNameIdentifier"].ToString();
                                pkgInsertOrgData.ARG_PTRN_PART_ID = dr["patternPartIdentifier"].ToString();
                                pkgInsertOrgData.ARG_SUPP_MAT_ID = dr["suppliedMaterialIdentifier"].ToString();
                                pkgInsertOrgData.ARG_MAT_ITEM_ID = dr["materialItemIdentifier"].ToString();
                                pkgInsertOrgData.ARG_MAT_ITEM_PLHDR_DESC = dr["materialItemPlaceholderDescription"].ToString();
                                pkgInsertOrgData.ARG_COLOR_ID = dr["colorIdentifier"].ToString();
                                pkgInsertOrgData.ARG_COLOR_PLHDR_DESC = dr["colorPlaceholderDescription"].ToString();
                                pkgInsertOrgData.ARG_SUPP_MAT_COLOR_IS_MUL = dr["suppliedMaterialColorIsMultipleColors"].ToString();
                                pkgInsertOrgData.ARG_SUPP_MAT_COLOR_ID = dr["suppliedMaterialColorIdentifier"].ToString();
                                pkgInsertOrgData.ARG_BOM_LINEITEM_COMMENTS = dr["logicBOMLineItemComments"].ToString();
                                pkgInsertOrgData.ARG_FAC_IN_HOUSE_IND = dr["factoryInHouseIndicator"].ToString();
                                pkgInsertOrgData.ARG_COUNTY_ORG_ID = dr["countyOfOriginIdentifier"].ToString();
                                pkgInsertOrgData.ARG_ACTUAL_DIMENSION_DESC = dr["actualDimensionDescription"].ToString();
                                pkgInsertOrgData.ARG_ISO_MEASUREMENT_CODE = dr["isoMeasurementCode"].ToString();
                                pkgInsertOrgData.ARG_NET_USAGE_NUMBER = dr["netUsageNumber"].ToString();
                                pkgInsertOrgData.ARG_WASTE_USAGE_NUMBER = dr["wasteUsageNumber"].ToString();
                                pkgInsertOrgData.ARG_PART_YIELD = dr["partYield"].ToString();
                                pkgInsertOrgData.ARG_CONSUM_CONVER_RATE = dr["consumptionConversionRate"].ToString();
                                pkgInsertOrgData.ARG_LINEITEM_DEFECT_PER_NUM = dr["lineItemDefectPercentNumber"].ToString();
                                pkgInsertOrgData.ARG_UNIT_PRICE_ISO_MSR_CODE = dr["unitPriceISOMeasurementCode"].ToString();
                                pkgInsertOrgData.ARG_CURRENCY_CODE = dr["currencyCode"].ToString();
                                pkgInsertOrgData.ARG_FACTORY_UNIT_PRICE = dr["factoryUnitPrice"].ToString();
                                pkgInsertOrgData.ARG_UNIT_PRICE_UPCHARGE_NUM = dr["unitPriceUpchargeNumber"].ToString();
                                pkgInsertOrgData.ARG_UNIT_PRICE_UPCHARGE_DESC = dr["unitPriceUpchargeDescription"].ToString();
                                pkgInsertOrgData.ARG_FREIGHT_TERM_ID = dr["freightTermIdentifier"].ToString();
                                pkgInsertOrgData.ARG_LANDED_COST_PER_NUM = dr["landedCostPercentNumber"].ToString();
                                pkgInsertOrgData.ARG_PCC_SORT_ORDER = dr["pccSortOrder"].ToString();
                                pkgInsertOrgData.ARG_ROLLUP_VARIATION_LV = "";
                                pkgInsertOrgData.ARG_PART_SEQ = Convert.ToString(sequence);
                                pkgInsertOrgData.ARG_LOGIC_GROUP = dr["logicGroup"].ToString();
                                pkgInsertOrgData.ARG_MAT_FORECAST_PRCNT = Convert.ToDouble(dr["materialForecastPercent"].ToString());
                                pkgInsertOrgData.ARG_COLOR_FORECAST_PRCNT = Convert.ToDouble(dr["colorForecastPercent"].ToString());

                                list.Add(pkgInsertOrgData);
                                sequence++;
                            }

                            if (projectBaseForm.Exe_Modify_PKG(list) == null)
                            {
                                MessageBox.Show("Failed to replace JSON Org. Data");
                                return;
                            }
                        }

                        #endregion
                    }
                }

                BindDataSourceToGridView("Header");
                MessageBox.Show("Complete");
            }
        }

        /// <summary>
        /// JSON 형태의 BOM을 XML 형태로 컨버팅
        /// </summary>
        /// <param name="_jObject"></param>
        /// <returns></returns>
        private DataSet ConvertJson2Xml(String _jObject)
        {
            var xmlDocument = new XmlDocument();
            var dataSet = new DataSet();
            // Valid XML must have one root element(the JSON passed to DeserializeXmlNode should have one property in the root JSON object)
            _jObject = "{ \"rootNode\": {" + _jObject.Trim().TrimStart('{').TrimEnd('}') + @"} }";
            // Deserializes the XmlNode from a JSON string. (Deserialize : 일련의 바이트로부터 데이터 구조를 추출하는 일은 역직렬화)
            xmlDocument = JsonConvert.DeserializeXmlNode(_jObject);
            // Reads XML schema and data into the DataSet using the specified XmlReader.
            dataSet.ReadXml(new XmlNodeReader(xmlDocument));

            return dataSet;
        }

        /// <summary>
        /// See if there are line items not saved yet.
        /// </summary>
        /// <param name="view"></param>
        /// <returns></returns>
        private bool HasUnsavedItems(GridView view)
        {
            for (int rowHandle = 0; rowHandle < view.RowCount; rowHandle++)
            {
                if (!view.GetRowCellValue(rowHandle, "ROW_STATUS").ToString().Equals("N"))
                {
                    MessageBox.Show("Save First.", "",
                        MessageBoxButtons.OK, MessageBoxIcon.Warning, MessageBoxDefaultButton.Button1);
                    return false;
                }
            }

            return true;
        }

        #endregion

        #region Context Menu Event

        /// <summary>
        /// For Context Menus.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ctxMenuStrip_Click(object sender, EventArgs e)
        {
            System.Windows.Forms.ToolStripMenuItem menuItem = sender as System.Windows.Forms.ToolStripMenuItem;

            switch (menuItem.Name)
            {
                case "RowUp":

                    NewRowUp();
                    break;

                case "RowDown":

                    NewRowDown();
                    break;

                case "AddRow":

                    if (Common.IsFilterApplied(gvwData))
                        break;

                    AddNewRow();
                    break;

                case "DeleteRow":

                    if (Common.IsFilterApplied(gvwData))
                        break;

                    DeleteRows();
                    break;

                case "FindCode":

                    FindCodeFromLibrary(gvwData);
                    break;

                case "FillColor":

                    FillColorInCell(gvwData);
                    break;

                case "MaterialInfo":
                    ShowMaterialInfomation(gvwData);
                    break;

                case "showFcsdRow":
                    ShowFocusedRow(gvwData);
                    break;

                case "ExportToExcel":
                    ExportBOMToExcel();
                    break;

                default:
                    break;
            }
        }

        /// <summary>
        /// 선택한 행을 올림
        /// </summary>
        private void NewRowUp()
        {
            try
            {
                int focusedRowHandle = gvwData.FocusedRowHandle;

                if (focusedRowHandle == 0)
                {
                    MessageBox.Show("It is first row", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                // 유저가 선택한 행들의 로우 핸들 넘버 배열
                int[] rowHandles = gvwData.GetSelectedRows();

                // 선택한 여러 행 중 가장 상위의 행을 포커스 로우로 지정
                focusedRowHandle = rowHandles[0];

                // 그리드의 데이터 소스
                DataTable dataSource = (DataTable)grdData.DataSource;

                // 여러 행을 올릴 경우 반복해서 실행
                foreach (int rowHandle in rowHandles)
                {
                    // 삭제될 행
                    DataRow rowToBeDeleted = gvwData.GetDataRow(rowHandle);

                    // 추가될 행
                    DataRow rowToBeInserted = dataSource.NewRow();

                    // 행 내용을 그대로 복사
                    rowToBeInserted.ItemArray = rowToBeDeleted.ItemArray;

                    // 기존 행 삭제
                    dataSource.Rows.Remove(rowToBeDeleted);

                    // 기존 위치 보다 한 칸 위에 행 삽입
                    dataSource.Rows.InsertAt(rowToBeInserted, rowHandle - 1);
                }

                // 최초 선택 했던 셀만큼 재선택
                gvwData.SelectCells(focusedRowHandle - 1, gvwData.FocusedColumn, focusedRowHandle + rowHandles.Length - 2, gvwData.FocusedColumn);

                // 포커스 로우 설정
                gvwData.FocusedRowHandle = focusedRowHandle - 1;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
                return;
            }
        }

        /// <summary>
        /// 선택한 행을 내림
        /// </summary>
        private void NewRowDown()
        {
            try
            {
                int[] rowHandles = gvwData.GetSelectedRows();

                // 선택한 여러 행 중 가장 상위의 행을 포커스 로우로 지정
                int focusedRowHandle = rowHandles[0];

                // 그리드의 데이터 소스
                DataTable dataSource = (DataTable)grdData.DataSource;

                // 유저가 선택한 BOM의 개수만큼 반복
                for (int i = 0; i < rowHandles.Length; i++)
                {
                    // 삭제될 행
                    DataRow rowToBeDeleted = gvwData.GetDataRow(focusedRowHandle);

                    // 추가될 행
                    DataRow rowToBeInserted = dataSource.NewRow();

                    // 행 내용을 그대로 복사
                    rowToBeInserted.ItemArray = rowToBeDeleted.ItemArray;

                    // 현재 포커싱된 행 삭제
                    dataSource.Rows.Remove(rowToBeDeleted);

                    // 다시 삽입
                    dataSource.Rows.InsertAt(rowToBeInserted, focusedRowHandle + rowHandles.Length);
                }

                // 셀 선택
                gvwData.SelectCells(focusedRowHandle + 1, gvwData.FocusedColumn, focusedRowHandle + rowHandles.Length, gvwData.FocusedColumn);

                // 포커스 로우 설정
                gvwData.FocusedRowHandle = focusedRowHandle + 1;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
                return;
            }
        }

        /// <summary>
        /// 신규 행 추가
        /// </summary>
        /// <param name="_type"></param>
        private void AddNewRow()
        {
            try
            {
                // See if a user selected only one row to avoid ambiguity of focus.
                GridCell[] cells = gvwData.GetSelectedCells();

                if (cells.Length > 1)
                {
                    MessageBox.Show("Please select only one cell", "Warning",
                        MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }

                DataTable dataSource = (DataTable)grdData.DataSource;
                int focusedRowHandle = gvwData.FocusedRowHandle;

                // Create a new row.
                DataRow newRow = dataSource.NewRow();

                // Input key value.
                newRow["FACTORY"] = Factory;
                newRow["WS_NO"] = WorksheetNumbers;

                // When there are no rows on the view.
                if (focusedRowHandle < 0)
                {
                    newRow["PART_SEQ"] = "1";
                    newRow["PART_TYPE"] = "UPPER";
                    newRow["ROW_STATUS"] = "I";
                    newRow["PRVS_STATUS"] = "I";
                    newRow["COMBINE_YN"] = "N";
                    newRow["STICKER_YN"] = "N";
                    newRow["MDSL_CHK"] = "N";
                    newRow["OTSL_CHK"] = "N";
                    newRow["MAT_FORECAST_PRCNT"] = 0.0;
                    newRow["COLOR_FORECAST_PRCNT"] = 0.0;

                    // Insert at first row.
                    dataSource.Rows.InsertAt(newRow, 0);

                    // Move focus to the created row.
                    gvwData.SelectCell(0, gvwData.FocusedColumn);
                    gvwData.FocusedRowHandle = 0;
                }
                else
                {
                    // Follow the part type of the upper row.
                    string prevRowPartType = gvwData.GetRowCellValue(focusedRowHandle, "PART_TYPE").ToString();

                    // Create a new part sequence.
                    string partSequence = CalcNextPartSequence();

                    // lose focus of the row selected previously.
                    gvwData.UnselectCell(focusedRowHandle, gvwData.FocusedColumn);

                    newRow["PART_SEQ"] = partSequence;
                    newRow["PART_TYPE"] = prevRowPartType;
                    newRow["ROW_STATUS"] = "I";
                    newRow["PRVS_STATUS"] = "I";
                    newRow["COMBINE_YN"] = "N";
                    newRow["STICKER_YN"] = "N";
                    newRow["MDSL_CHK"] = "N";
                    newRow["OTSL_CHK"] = "N";
                    newRow["MAT_FORECAST_PRCNT"] = 0.0;
                    newRow["COLOR_FORECAST_PRCNT"] = 0.0;

                    // Insert at the bottom of the focused row.
                    dataSource.Rows.InsertAt(newRow, focusedRowHandle + 1);

                    // Move focus to the created row.
                    gvwData.SelectCell(focusedRowHandle + 1, gvwData.FocusedColumn);
                    gvwData.FocusedRowHandle = focusedRowHandle + 1;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
                return;
            }
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
            try
            {
                // 이벤트 꼬임을 방지하기 위해 CellValueChanged 이벤트 끊음
                gvwData.CellValueChanged -= new CellValueChangedEventHandler(CustomCellValueChanged);

                // 삭제할 행이 하나도 없는 경우 리턴
                if (gvwData.RowCount == 0) return;

                // 선택된 행의 개수만큼 반복
                foreach (int rowHandle in gvwData.GetSelectedRows())
                {
                    // 선택된 행의 상태값
                    string rowStatus = gvwData.GetRowCellValue(rowHandle, "ROW_STATUS").ToString();

                    if (rowStatus != "D")
                    {
                        // 상태값이 삭제가 아니라면, 현재의 상태값을 기록해둠
                        gvwData.SetRowCellValue(rowHandle, "PRVS_STATUS", rowStatus);

                        // 현재 상태값을 삭제로 변경
                        gvwData.SetRowCellValue(rowHandle, "ROW_STATUS", "D");
                    }
                    else
                    {
                        // 현재 상태값이 삭제인 경우 이전 상태값을 가져옴
                        string previousRowStatus = gvwData.GetRowCellValue(rowHandle, "PRVS_STATUS").ToString();

                        // 이전 상태값으로 변경
                        gvwData.SetRowCellValue(rowHandle, "ROW_STATUS", previousRowStatus);
                    }
                }

                // 셀 스타일 새로 적용
                gvwData.RefreshData();

                // 이벤트 다시 연결
                gvwData.CellValueChanged += new CellValueChangedEventHandler(CustomCellValueChanged);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
                return;
            }
        }

        /// <summary>
        /// 라이브러리에서 코드를 찾아 셀에 입력
        /// </summary>
        /// <param name="_type"></param>
        private void FindCodeFromLibrary(GridView view)
        {
            if (Common.HasLineitemDeleted(view) == true) return;

            // Parameters to send to the childForm.
            int initSearchType = 0;
            string partDelimiter = string.Empty;
            string keyword = view.GetFocusedRowCellValue(view.FocusedColumn).ToString();

            // Get the initial search type from the focused column.
            switch (view.FocusedColumn.FieldName)
            {
                case "PCX_PART_ID":
                case "PART_NAME":
                case "PTRN_PART_NAME":
                case "PART_TYPE":

                    initSearchType = 0;

                    if (view.FocusedColumn.FieldName == "PTRN_PART_NAME")
                        partDelimiter = "Pattern";
                    break;

                case "MXSXL_NUMBER":
                case "PCX_SUPP_MAT_ID":
                case "MAT_NAME":
                case "VENDOR_NAME":

                    initSearchType = 1;
                    break;

                case "PCX_COLOR_ID":
                case "COLOR_CD":
                case "COLOR_NAME":

                    initSearchType = 2;
                    break;

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
                        if (results[0] == "Part")
                        {
                            //string oldValue = view.GetRowCellValue(rowHandle, "PART_NAME").ToString();
                            string oldValue = row["PART_NAME"].ToString();

                            // When the user changed part type from lamination to normal.
                            if (oldValue.Contains("LAMINATION") == true && results[1].Contains("LAMINATION") == false)
                            {
                                //view.SetRowCellValue(rowHandle, "PTRN_PART_NAME", "");
                                //view.SetRowCellValue(rowHandle, "PTRN_PART_CD", "");

                                // Automatically reset attributes of the pattern part to avoid mistake.
                                row["PTRN_PART_NAME"] = "";
                                row["PTRN_PART_CD"] = "";
                            }

                            if (results[2] == "MIDSOLE" || results[2] == "OUTSOLE")
                            {
                                //view.SetRowCellValue(rowHandle, "PART_TYPE", results[2]);
                                row["PART_TYPE"] = results[2];
                            }
                            else if (results[4] == "3389" || results[4] == "3390")
                            {
                                //view.SetRowCellValue(rowHandle, "PART_TYPE", "UPPER");

                                // Fix BOM section to 'UPPER' only for WET CHEMISTRY & WET CHEMISTRY-MULTI.
                                row["PART_TYPE"] = "UPPER";
                            }

                            //view.SetRowCellValue(rowHandle, "PART_NAME", results[1]);
                            //view.SetRowCellValue(rowHandle, "PART_CD", results[3]);
                            //view.SetRowCellValue(rowHandle, "PCX_PART_ID", results[4]);

                            row["PART_NAME"] = results[1];
                            row["PART_CD"] = results[3];
                            row["PCX_PART_ID"] = results[4];

                            Common.BindDefaultMaterialByNCFRule(view, row, results[4]);
                        }
                        else if (results[0] == "PatternPart")
                        {
                            //view.SetRowCellValue(rowHandle, "PTRN_PART_NAME", results[1]);
                            //view.SetRowCellValue(rowHandle, "PTRN_PART_CD", results[3]);

                            row["PTRN_PART_NAME"] = results[1];
                            row["PTRN_PART_CD"] = results[3];
                        }
                        else if (results[0] == "PCX_Material")
                        {
                            //view.SetRowCellValue(rowHandle, "PCX_MAT_ID", results[1]);
                            //view.SetRowCellValue(rowHandle, "MAT_CD", results[2]);
                            //view.SetRowCellValue(rowHandle, "MAT_NAME", results[3]);
                            //view.SetRowCellValue(rowHandle, "MXSXL_NUMBER", results[4]);
                            //view.SetRowCellValue(rowHandle, "MCS_NUMBER", results[5]);
                            //view.SetRowCellValue(rowHandle, "VENDOR_NAME", results[6]);
                            //view.SetRowCellValue(rowHandle, "CS_CD", results[7]);
                            //view.SetRowCellValue(rowHandle, "PCX_SUPP_MAT_ID", results[8]);
                            //view.SetRowCellValue(rowHandle, "NIKE_MS_STATE", results[9]);
                            //view.SetRowCellValue(rowHandle, "BLACK_LIST_YN", results[10]);

                            row["PCX_MAT_ID"] = results[1];
                            row["MAT_CD"] = results[2];
                            row["MAT_NAME"] = results[3];
                            row["MXSXL_NUMBER"] = results[4];
                            row["MCS_NUMBER"] = results[5];
                            row["VENDOR_NAME"] = results[6];
                            row["CS_CD"] = results[7];
                            row["PCX_SUPP_MAT_ID"] = results[8];
                            row["NIKE_MS_STATE"] = results[9];
                            row["BLACK_LIST_YN"] = results[10];
                        }
                        else if (results[0] == "Color")
                        {
                            //view.SetRowCellValue(rowHandle, "PCX_COLOR_ID", results[1]);
                            //view.SetRowCellValue(rowHandle, "COLOR_CD", results[2]);
                            //view.SetRowCellValue(rowHandle, "COLOR_NAME", results[3]);

                            row["PCX_COLOR_ID"] = results[1];
                            row["COLOR_CD"] = results[2];
                            row["COLOR_NAME"] = results[3];
                        }
                        else if (results[0] == "PCC_Material")
                        {
                            //view.SetRowCellValue(rowHandle, "PCX_MAT_ID", results[1]);
                            //view.SetRowCellValue(rowHandle, "MAT_CD", results[2]);
                            //view.SetRowCellValue(rowHandle, "MAT_NAME", results[3]);
                            //view.SetRowCellValue(rowHandle, "MXSXL_NUMBER", results[4]);
                            //view.SetRowCellValue(rowHandle, "MCS_NUMBER", results[5]);
                            //view.SetRowCellValue(rowHandle, "VENDOR_NAME", results[6]);
                            //view.SetRowCellValue(rowHandle, "CS_CD", results[7]);
                            //view.SetRowCellValue(rowHandle, "PCX_SUPP_MAT_ID", results[8]);
                            //view.SetRowCellValue(rowHandle, "NIKE_MS_STATE", results[9]);
                            //view.SetRowCellValue(rowHandle, "BLACK_LIST_YN", results[10]);

                            row["PCX_MAT_ID"] = results[1];
                            row["MAT_CD"] = results[2];
                            row["MAT_NAME"] = results[3];
                            row["MXSXL_NUMBER"] = results[4];
                            row["MCS_NUMBER"] = results[5];
                            row["VENDOR_NAME"] = results[6];
                            row["CS_CD"] = results[7];
                            row["PCX_SUPP_MAT_ID"] = results[8];
                            row["NIKE_MS_STATE"] = results[9];
                            row["BLACK_LIST_YN"] = results[10];
                        }
                        else if (results[0] == "CS_Material")
                        {
                            //view.SetRowCellValue(rowHandle, "PCX_MAT_ID", "100");
                            //view.SetRowCellValue(rowHandle, "MAT_CD", results[2]);
                            //view.SetRowCellValue(rowHandle, "MAT_NAME", results[3]);
                            //view.SetRowCellValue(rowHandle, "MXSXL_NUMBER", results[1]);
                            //view.SetRowCellValue(rowHandle, "MCS_NUMBER", "");
                            //view.SetRowCellValue(rowHandle, "VENDOR_NAME", "");
                            //view.SetRowCellValue(rowHandle, "CS_CD", "CS");
                            //view.SetRowCellValue(rowHandle, "PCX_SUPP_MAT_ID", "100");

                            row["PCX_MAT_ID"] = "100";
                            row["MAT_CD"] = results[2];
                            row["MAT_NAME"] = results[3];
                            row["MXSXL_NUMBER"] = results[1];
                            row["MCS_NUMBER"] = "";
                            row["VENDOR_NAME"] = "";
                            row["CS_CD"] = "CS";
                            row["PCX_SUPP_MAT_ID"] = "100";
                        }

                        //if (view.GetRowCellValue(rowHandle, "ROW_STATUS").ToString() != "I")
                        //    view.SetRowCellValue(rowHandle, "ROW_STATUS", "U");

                        if (row["ROW_STATUS"].ToString().Equals("I") == false)
                            row["ROW_STATUS"] = "U";
                    }

                    #region Method - RowHandle(Past Version)

                    //foreach (int rowHandle in view.GetSelectedRows())
                    //{
                    //    if (results[0] == "Part")
                    //    {
                    //        string oldValue = view.GetRowCellValue(rowHandle, "PART_NAME").ToString();

                    //        // When the user changed part type from lamination to normal.
                    //        if (oldValue.Contains("LAMINATION") == true && results[1].Contains("LAMINATION") == false)
                    //        {
                    //            // Automatically reset attributes of the pattern part to avoid mistake.
                    //            view.SetRowCellValue(rowHandle, "PTRN_PART_NAME", "");
                    //            view.SetRowCellValue(rowHandle, "PTRN_PART_CD", "");
                    //        }

                    //        view.SetRowCellValue(rowHandle, "PART_NAME", results[1]);

                    //        if (results[2] == "MIDSOLE" || results[2] == "OUTSOLE")
                    //        {
                    //            view.SetRowCellValue(rowHandle, "PART_TYPE", results[2]);
                    //        }
                    //        else if (results[4] == "3389" || results[4] == "3390")
                    //        {
                    //            // Fix BOM section to 'UPPER' only for WET CHEMISTRY & WET CHEMISTRY-MULTI.
                    //            view.SetRowCellValue(rowHandle, "PART_TYPE", "UPPER");
                    //        }

                    //        view.SetRowCellValue(rowHandle, "PART_CD", results[3]);
                    //        view.SetRowCellValue(rowHandle, "PCX_PART_ID", results[4]);

                    //        Common.BindDefaultMaterialByNCFRule(view, rowHandle, results[4]);
                    //    }
                    //    else if (results[0] == "PatternPart")
                    //    {
                    //        view.SetRowCellValue(rowHandle, "PTRN_PART_NAME", results[1]);
                    //        view.SetRowCellValue(rowHandle, "PTRN_PART_CD", results[3]);
                    //    }
                    //    else if (results[0] == "PCX_Material")
                    //    {
                    //        view.SetRowCellValue(rowHandle, "PCX_MAT_ID", results[1]);
                    //        view.SetRowCellValue(rowHandle, "MAT_CD", results[2]);
                    //        view.SetRowCellValue(rowHandle, "MAT_NAME", results[3]);
                    //        view.SetRowCellValue(rowHandle, "MXSXL_NUMBER", results[4]);
                    //        view.SetRowCellValue(rowHandle, "MCS_NUMBER", results[5]);
                    //        view.SetRowCellValue(rowHandle, "VENDOR_NAME", results[6]);
                    //        view.SetRowCellValue(rowHandle, "CS_CD", results[7]);
                    //        view.SetRowCellValue(rowHandle, "PCX_SUPP_MAT_ID", results[8]);
                    //        view.SetRowCellValue(rowHandle, "NIKE_MS_STATE", results[9]);
                    //        view.SetRowCellValue(rowHandle, "BLACK_LIST_YN", results[10]);
                    //    }
                    //    else if (results[0] == "Color")
                    //    {
                    //        view.SetRowCellValue(rowHandle, "PCX_COLOR_ID", results[1]);
                    //        view.SetRowCellValue(rowHandle, "COLOR_CD", results[2]);
                    //        view.SetRowCellValue(rowHandle, "COLOR_NAME", results[3]);
                    //    }
                    //    else if (results[0] == "PCC_Material")
                    //    {
                    //        view.SetRowCellValue(rowHandle, "PCX_MAT_ID", results[1]);
                    //        view.SetRowCellValue(rowHandle, "MAT_CD", results[2]);
                    //        view.SetRowCellValue(rowHandle, "MAT_NAME", results[3]);
                    //        view.SetRowCellValue(rowHandle, "MXSXL_NUMBER", results[4]);
                    //        view.SetRowCellValue(rowHandle, "MCS_NUMBER", results[5]);
                    //        view.SetRowCellValue(rowHandle, "VENDOR_NAME", results[6]);
                    //        view.SetRowCellValue(rowHandle, "CS_CD", results[7]);
                    //        view.SetRowCellValue(rowHandle, "PCX_SUPP_MAT_ID", results[8]);
                    //        view.SetRowCellValue(rowHandle, "NIKE_MS_STATE", results[9]);
                    //        view.SetRowCellValue(rowHandle, "BLACK_LIST_YN", results[10]);
                    //    }
                    //    else if (results[0] == "CS_Material")
                    //    {
                    //        view.SetRowCellValue(rowHandle, "PCX_MAT_ID", "100");
                    //        view.SetRowCellValue(rowHandle, "MAT_CD", results[2]);
                    //        view.SetRowCellValue(rowHandle, "MAT_NAME", results[3]);
                    //        view.SetRowCellValue(rowHandle, "MXSXL_NUMBER", results[1]);
                    //        view.SetRowCellValue(rowHandle, "MCS_NUMBER", "");
                    //        view.SetRowCellValue(rowHandle, "VENDOR_NAME", "");
                    //        view.SetRowCellValue(rowHandle, "CS_CD", "CS");
                    //        view.SetRowCellValue(rowHandle, "PCX_SUPP_MAT_ID", "100");
                    //    }

                    //    if (view.GetRowCellValue(rowHandle, "ROW_STATUS").ToString() != "I")
                    //        view.SetRowCellValue(rowHandle, "ROW_STATUS", "U");
                    //}

                    #endregion
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.ToString());
                }
                finally
                {
                    // Link event again.
                    view.CellValueChanged += new CellValueChangedEventHandler(CustomCellValueChanged);
                }
            }
        }

        /// <summary>
        /// 셀에 유저가 입력한 컬러를 배경색으로 표기한다.
        /// </summary>
        /// <param name="view"></param>
        private void FillColorInCell(GridView view)
        {
            // 새 컬러 선택
            ColorDialog dialog = new ColorDialog();
            dialog.AllowFullOpen = false;
            dialog.FullOpen = false;
            dialog.ShowHelp = false;
            dialog.Color = Color.White;

            if (dialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                GridCell[] cells = view.GetSelectedCells();

                foreach (GridCell cell in cells)
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

                        if (colorList.ContainsKey(columnName) == true)  // 현 컬럼에 이미 등록된 컬러가 있는 경우 기존 것 삭제
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
        /// 신규 행 추가 시 입력할 파트 시퀀스를 구함
        /// 1-n. 순차
        /// </summary>
        /// <param name="_view"></param>
        /// <returns></returns>
        private string CalcNextPartSequence()
        {
            // 뷰에 행이 하나도 없는면 1(첫 번째 행)
            if (gvwData.RowCount == 0) return "1";

            // 뷰에서 현재 가장 큰 파트 시퀀스
            int curMaxPartSeq = 0;

            for (int i = 0; i < gvwData.RowCount; i++)
            {
                // 각 행의 파트 시퀀스
                string partSequence = gvwData.GetRowCellValue(i, "PART_SEQ").ToString();

                // 정수형으로 변환
                int numPartSequence = Convert.ToInt32(partSequence);

                // 뷰에서 가장 큰 Part Sequence를 구함
                if (numPartSequence > curMaxPartSeq)
                    curMaxPartSeq = numPartSequence;
            }
            // 뷰에서 가장 큰 파트 시퀀스 + 1의 값이 다음 파트 시퀀스
            return (curMaxPartSeq + 1).ToString();
        }

        /// <summary>
        /// 
        /// </summary>
        private void ShowMaterialInfomation(GridView view)
        {
            try
            {
                string PDMSuppMatNumber = view.GetRowCellValue(view.FocusedRowHandle, "MXSXL_NUMBER").ToString();

                if (PDMSuppMatNumber == "")
                    return;

                string[] splitPDMSuppMatNum = PDMSuppMatNumber.Split('.');

                if (splitPDMSuppMatNum.Length != 3)
                    return;

                string materialCode = splitPDMSuppMatNum[0];

                Dictionary<string, string> dic = new Dictionary<string, string>();
                dic.Add("MXSXL_NUMBER", PDMSuppMatNumber);
                dic.Add("MAT_CD", materialCode);

                MaterialInformation form = new MaterialInformation() { MaterialInfo = dic };
                form.ShowDialog();
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
        private void ShowFocusedRow(GridView view)
        {
            int[] selectedRowHandles = view.GetSelectedRows();

            if (selectedRowHandles.Length == 0)
            {
                view.SelectRow(view.FocusedRowHandle);
            }
            else if (selectedRowHandles.Length == 1)
            {
                if (CheckRowIsSelected(view, view.FocusedRowHandle))
                {
                    // When the row is selected.
                    view.UnselectRow(view.FocusedRowHandle);
                }
                else
                {
                    // When the row is not selected.
                    view.SelectRow(view.FocusedRowHandle);
                }
            }
            else
            {
                // When an user selected more than one row.
                string type = string.Empty;
                GridCell[] cells = view.GetSelectedCells();

                // 17 : The number of columns which are visible on a row.
                if (cells.Length > 17)
                    type = "Cancel";
                else
                    type = "Select";

                if (type == "Select")
                {
                    // 선택한 행 모두 전체 선택
                    foreach (int rowHandle in selectedRowHandles)
                    {
                        if (CheckRowIsSelected(view, rowHandle))
                            view.UnselectRow(rowHandle);
                        else
                            view.SelectRow(rowHandle);
                    }
                }
                else if (type == "Cancel")
                {
                    // 포커스된 행만 전체 선택 취소
                    if (CheckRowIsSelected(view, view.FocusedRowHandle))
                        view.UnselectRow(view.FocusedRowHandle);
                    else
                        view.SelectRow(view.FocusedRowHandle);
                }
            }
        }

        /// <summary>
        /// See if the row is selected.
        /// </summary>
        /// <param name="view"></param>
        /// <param name="focusedRowHandle"></param>
        /// <returns></returns>
        private bool CheckRowIsSelected(GridView view, int focusedRowHandle)
        {
            int count = 0;
            GridCell[] cells = view.GetSelectedCells();

            foreach (GridCell cell in cells)
            {
                if (cell.RowHandle == focusedRowHandle)
                    count++;
            }

            // If the count of selected cell is more than 5, we consider that this row is selected.
            if (count > 5)
                return true;
            else
                return false;
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
                    grdData.ExportToXls(myStream);
                    myStream.Close();
                }
            }
        }

        #endregion

        #region Grid Event

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void CustomCellValueChanged(object sender, CellValueChangedEventArgs e)
        {
            try
            {
                #region Variables

                // for Part.
                string partCode = string.Empty;
                string partName = string.Empty;
                string partType = string.Empty;
                string pcxPartId = string.Empty;

                // for Materials.
                string mxsxlNumber = string.Empty;
                string pcxSuppMatID = string.Empty;
                string csCode = string.Empty;
                string mcsNumber = string.Empty;
                string pcxMatID = string.Empty;
                string pdmMatCode = string.Empty;
                string pdmMatName = string.Empty;
                string vendorName = string.Empty;
                string matRisk = string.Empty;
                string nikeMSState = string.Empty;
                string blackListYN = string.Empty;

                // for Color.
                string pcxColorID = string.Empty;
                string pdmColorCode = string.Empty;
                string pdmColorName = string.Empty;

                #endregion

                GridView view = sender as GridView;
                GridCell[] cells = view.GetSelectedCells();

                var currValue = view.ActiveEditor.EditValue.ToString();
                var oldValue = view.ActiveEditor.OldEditValue;

                // Continue when the value has been changed.
                if (currValue.ToString() == oldValue.ToString())
                    return;

                view.CellValueChanged -= new CellValueChangedEventHandler(CustomCellValueChanged);

                string value = currValue;

                if (view.FocusedColumn.FieldName == "COLOR_CD")
                {
                    #region PDM 컬러코드 다이렉트 입력 시, 다른 속성 자동 입력을 위해 코드 정보를 가져옴

                    if (value != "")
                    {
                        PKG_INTG_BOM.SELECT_FOR_DIRECT_INPUT pkgSelect = new PKG_INTG_BOM.SELECT_FOR_DIRECT_INPUT();
                        pkgSelect.ARG_TYPE = "Color";
                        pkgSelect.ARG_CODE = value.ToUpper();
                        pkgSelect.ARG_NAME = "";
                        pkgSelect.OUT_CURSOR = string.Empty;

                        DataTable dataSource = projectBaseForm.Exe_Select_PKG(pkgSelect).Tables[0];
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
                            pkgSelect.ARG_NAME = value.ToUpper();
                            pkgSelect.OUT_CURSOR = string.Empty;

                            DataTable dataSource = projectBaseForm.Exe_Select_PKG(pkgSelect).Tables[0];
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
                            pkgSelect.ARG_CODE = value.ToUpper();
                            pkgSelect.ARG_NAME = "";
                            pkgSelect.OUT_CURSOR = string.Empty;

                            DataTable dataSource = projectBaseForm.Exe_Select_PKG(pkgSelect).Tables[0];

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
                else if (view.FocusedColumn.FieldName == "PTRN_PART_NAME")
                {
                    #region Part Name 다이렉트 입력 시, 다른 속성 자동 입력을 위해 코드 정보를 가져옴

                    if (value != "")
                    {
                        if (CSBOMStatus != "F")
                        {
                            PKG_INTG_BOM.SELECT_FOR_DIRECT_INPUT pkgSelect = new PKG_INTG_BOM.SELECT_FOR_DIRECT_INPUT();
                            pkgSelect.ARG_TYPE = "Part";
                            pkgSelect.ARG_CODE = value.ToUpper();
                            pkgSelect.ARG_NAME = "";
                            pkgSelect.OUT_CURSOR = string.Empty;

                            DataTable dataSource = projectBaseForm.Exe_Select_PKG(pkgSelect).Tables[0];

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
                    #region MxSxL Number 다이렉트 입력 시, 다른 속성 자동 입력을 위해 코드 정보를 가져옴

                    if (value != "")
                    {
                        // PCC 자재 우선 검색
                        PKG_INTG_BOM.SELECT_FOR_DIRECT_INPUT pkgSelect = new PKG_INTG_BOM.SELECT_FOR_DIRECT_INPUT();
                        pkgSelect.ARG_TYPE = "PCX_ByCode";
                        pkgSelect.ARG_CODE = value.ToUpper();
                        pkgSelect.ARG_NAME = "";
                        pkgSelect.OUT_CURSOR = string.Empty;

                        DataTable dataSource = projectBaseForm.Exe_Select_PKG(pkgSelect).Tables[0];

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
                            csCode = dataSource.Rows[0]["CS_CD"].ToString();
                            blackListYN = dataSource.Rows[0]["BLACK_LIST_YN"].ToString();
                            //mdmNotExisting = dataSource.Rows[0]["MDM_NOT_EXST"].ToString();
                            //matRisk = dataSource.Rows[0]["MAT_RISK"].ToString();
                        }
                        else
                        {
                            pcxMatID = "100";
                            pcxSuppMatID = "100";
                            pdmMatName = "PLACEHOLDER";

                            MessageBox.Show("Not registered in PCX.");
                        }
                    }

                    #endregion
                }
                else if (view.FocusedColumn.FieldName == "MAT_NAME")
                {
                    #region 자재명 다이렉트 입력 시, 다른 속성 자동 입력을 위해 코드 정보를 가져옴

                    if (value != "")
                    {
                        if (CSBOMStatus != "F")
                        {
                            if (value.ToUpper() == "PLACEHOLDER" || value.ToUpper() == "N/A")
                            {
                                #region 자재명에 PLACEHOLDER 또는 N/A 입력 시 코드 자동 기입

                                mxsxlNumber = "";
                                pcxSuppMatID = (value.ToUpper() == "PLACEHOLDER") ? "100" : "999";
                                csCode = "";
                                mcsNumber = "";
                                pcxMatID = (value.ToUpper() == "PLACEHOLDER") ? "100" : "999";
                                pdmMatCode = "";
                                pdmMatName = (value.ToUpper() == "PLACEHOLDER") ? "PLACEHOLDER" : "N/A";
                                vendorName = "";
                                matRisk = "";
                                nikeMSState = "";
                                blackListYN = "N";

                                #endregion
                            }
                            else
                            {
                                // PCC 자재 우선 검색
                                PKG_INTG_BOM.SELECT_FOR_DIRECT_INPUT pkgSelect = new PKG_INTG_BOM.SELECT_FOR_DIRECT_INPUT();
                                pkgSelect.ARG_TYPE = "PCX_ByName";
                                pkgSelect.ARG_CODE = "";
                                pkgSelect.ARG_NAME = value.ToUpper();
                                pkgSelect.OUT_CURSOR = string.Empty;

                                DataTable dataSource = projectBaseForm.Exe_Select_PKG(pkgSelect).Tables[0];

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
                                    csCode = dataSource.Rows[0]["CS_CD"].ToString();
                                    blackListYN = dataSource.Rows[0]["BLACK_LIST_YN"].ToString();
                                    //mdmNotExisting = dataSource.Rows[0]["MDM_NOT_EXST"].ToString();
                                    //matRisk = dataSource.Rows[0]["MAT_RISK"].ToString();
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
                                        csCode = results[7];
                                        blackListYN = results[10];
                                        //mdmNotExisting = results[11];
                                    }
                                }
                                else
                                {
                                    pcxSuppMatID = "100";
                                    pcxMatID = "100";
                                    pdmMatName = "PLACEHOLDER";
                                    //mdmNotExisting = "Y";

                                    MessageBox.Show("Not registered in PCX.");
                                }
                            }
                        }
                        else
                        {
                            // Fake BOM의 경우 유저 입력 값을 바로 입력
                            pdmMatName = value;
                        }
                    }
                    #endregion
                }

                List<DataRow> selectedRows = new List<DataRow>();

                foreach (int rowHandle in view.GetSelectedRows())
                    selectedRows.Add(view.GetDataRow(rowHandle));

                foreach (DataRow row in selectedRows)
                {
                    string rowStatus = row["ROW_STATUS"].ToString();

                    if (rowStatus != "D")
                    {
                        if (view.FocusedColumn.FieldName == "COLOR_CD")
                        {
                            //view.SetRowCellValue(cell.RowHandle, "PCX_COLOR_ID", pcxColorID);
                            //view.SetRowCellValue(cell.RowHandle, "COLOR_CD", pdmColorCode);
                            //view.SetRowCellValue(cell.RowHandle, "COLOR_NAME", pdmColorName);

                            row["PCX_COLOR_ID"] = pcxColorID;
                            row["COLOR_CD"] = pdmColorCode;
                            row["COLOR_NAME"] = pdmColorName;
                        }
                        else if (view.FocusedColumn.FieldName == "COLOR_NAME")
                        {
                            //view.SetRowCellValue(cell.RowHandle, "PCX_COLOR_ID", pcxColorID);
                            //view.SetRowCellValue(cell.RowHandle, "COLOR_CD", pdmColorCode);
                            //view.SetRowCellValue(cell.RowHandle, "COLOR_NAME", pdmColorName);

                            row["PCX_COLOR_ID"] = pcxColorID;
                            row["COLOR_CD"] = pdmColorCode;
                            row["COLOR_NAME"] = pdmColorName;
                        }
                        else if (view.FocusedColumn.FieldName == "PART_NAME")
                        {
                            // To avoid user mistake.
                            // When an user changed part type from lamination to normal part, clear the pattern part.
                            if (oldValue.ToString().Contains("LAMINATION") == true && partName.Contains("LAMINATION") == false)
                            {
                                //view.SetRowCellValue(cell.RowHandle, "PTRN_PART_NAME", "");
                                //view.SetRowCellValue(cell.RowHandle, "PTRN_PART_CD", "");

                                row["PTRN_PART_NAME"] = "";
                                row["PTRN_PART_CD"] = "";
                            }

                            //view.SetRowCellValue(cell.RowHandle, "PART_NAME", partName);
                            //view.SetRowCellValue(cell.RowHandle, "PART_CD", partCode);

                            row["PART_NAME"] = partName;
                            row["PART_CD"] = partCode;

                            if (partType == "MIDSOLE" || partType == "OUTSOLE")
                            {
                                //view.SetRowCellValue(cell.RowHandle, "PART_TYPE", partType);
                                row["PART_TYPE"] = partType;
                            }

                            //view.SetRowCellValue(cell.RowHandle, "PCX_PART_ID", pcxPartId);
                            row["PCX_PART_ID"] = pcxPartId;

                            Common.BindDefaultMaterialByNCFRule(view, row, pcxPartId);
                        }
                        else if (view.FocusedColumn.FieldName == "PTRN_PART_NAME")
                        {
                            //view.SetRowCellValue(cell.RowHandle, "PTRN_PART_NAME", partName);
                            //view.SetRowCellValue(cell.RowHandle, "PTRN_PART_CD", partCode);

                            row["PTRN_PART_NAME"] = partName;
                            row["PTRN_PART_CD"] = partCode;
                        }
                        else if (view.FocusedColumn.FieldName == "MXSXL_NUMBER")
                        {
                            //view.SetRowCellValue(cell.RowHandle, "MXSXL_NUMBER", mxsxlNumber);
                            //view.SetRowCellValue(cell.RowHandle, "PCX_SUPP_MAT_ID", (pcxSuppMatID == "") ? "100" : pcxSuppMatID);
                            //view.SetRowCellValue(cell.RowHandle, "CS_CD", csCode);
                            //view.SetRowCellValue(cell.RowHandle, "MCS_NUMBER", mcsNumber);
                            //view.SetRowCellValue(cell.RowHandle, "PCX_MAT_ID", (pcxMatID == "") ? "100" : pcxMatID);
                            //view.SetRowCellValue(cell.RowHandle, "MAT_CD", pdmMatCode);
                            //view.SetRowCellValue(cell.RowHandle, "MAT_NAME", (pdmMatName == "") ? "PLACEHOLDER" : pdmMatName);
                            //view.SetRowCellValue(cell.RowHandle, "VENDOR_NAME", vendorName);
                            //view.SetRowCellValue(cell.RowHandle, "NIKE_MS_STATE", nikeMSState);
                            //view.SetRowCellValue(cell.RowHandle, "BLACK_LIST_YN", blackListYN);

                            row["MXSXL_NUMBER"] = mxsxlNumber;
                            row["PCX_SUPP_MAT_ID"] = (pcxSuppMatID == "") ? "100" : pcxSuppMatID;
                            row["CS_CD"] = csCode;
                            row["MCS_NUMBER"] = mcsNumber;
                            row["PCX_MAT_ID"] = (pcxMatID == "") ? "100" : pcxMatID;
                            row["MAT_CD"] = pdmMatCode;
                            row["MAT_NAME"] = (pdmMatName == "") ? "PLACEHOLDER" : pdmMatName;
                            row["VENDOR_NAME"] = vendorName;
                            row["NIKE_MS_STATE"] = nikeMSState;
                            row["BLACK_LIST_YN"] = blackListYN;
                        }
                        else if (view.FocusedColumn.FieldName == "MAT_NAME")
                        {
                            //view.SetRowCellValue(cell.RowHandle, "MXSXL_NUMBER", mxsxlNumber);
                            //view.SetRowCellValue(cell.RowHandle, "PCX_SUPP_MAT_ID", (pcxSuppMatID == "") ? "100" : pcxSuppMatID);
                            //view.SetRowCellValue(cell.RowHandle, "CS_CD", csCode);
                            //view.SetRowCellValue(cell.RowHandle, "MCS_NUMBER", mcsNumber);
                            //view.SetRowCellValue(cell.RowHandle, "PCX_MAT_ID", (pcxMatID == "") ? "100" : pcxMatID);
                            //view.SetRowCellValue(cell.RowHandle, "MAT_CD", pdmMatCode);
                            //view.SetRowCellValue(cell.RowHandle, "MAT_NAME", (pdmMatName == "") ? "PLACEHOLDER" : pdmMatName);
                            //view.SetRowCellValue(cell.RowHandle, "VENDOR_NAME", vendorName);
                            //view.SetRowCellValue(cell.RowHandle, "NIKE_MS_STATE", nikeMSState);
                            //view.SetRowCellValue(cell.RowHandle, "BLACK_LIST_YN", blackListYN);

                            row["MXSXL_NUMBER"] = mxsxlNumber;
                            row["PCX_SUPP_MAT_ID"] = (pcxSuppMatID == "") ? "100" : pcxSuppMatID;
                            row["CS_CD"] = csCode;
                            row["MCS_NUMBER"] = mcsNumber;
                            row["PCX_MAT_ID"] = (pcxMatID == "") ? "100" : pcxMatID;
                            row["MAT_CD"] = pdmMatCode;
                            row["MAT_NAME"] = (pdmMatName == "") ? "PLACEHOLDER" : pdmMatName;
                            row["VENDOR_NAME"] = vendorName;
                            row["NIKE_MS_STATE"] = nikeMSState;
                            row["BLACK_LIST_YN"] = blackListYN;
                        }
                        else if (view.FocusedColumn.FieldName == "MAT_COMMENT")
                        {
                            //view.SetRowCellValue(cell.RowHandle, "MAT_COMMENT", value);
                            row["MAT_COMMENT"] = value;
                        }
                        else if (view.FocusedColumn.FieldName == "COLOR_COMMENT")
                        {
                            //view.SetRowCellValue(cell.RowHandle, "COLOR_COMMENT", value);
                            row["COLOR_COMMENT"] = value;
                        }
                        else if (view.FocusedColumn.FieldName == "PROCESS")
                        {
                            //view.SetRowCellValue(cell.RowHandle, "PROCESS", value);
                            row["PROCESS"] = value;
                        }
                        else if (view.FocusedColumn.FieldName == "REMARKS")
                        {
                            //view.SetRowCellValue(cell.RowHandle, "REMARKS", value);
                            row["REMARKS"] = value;
                        }
                        else if (view.FocusedColumn.FieldName == "PART_TYPE")
                        {
                            //view.SetRowCellValue(cell.RowHandle, "PART_TYPE", value);
                            row["PART_TYPE"] = value;
                        }
                        else if (view.FocusedColumn.FieldName == "BTTM")
                        {
                            //view.SetRowCellValue(cell.RowHandle, "BTTM", value);
                            row["BTTM"] = value;
                        }
                        else if (view.FocusedColumn.FieldName == "PART_NIKE_COMMENT")
                        {
                            //view.SetRowCellValue(cell.RowHandle, "PART_NIKE_COMMENT", value);
                            row["PART_NIKE_COMMENT"] = value;
                        }
                        else if (view.FocusedColumn.FieldName == "LOGIC_GROUP")
                        {
                            //view.SetRowCellValue(cell.RowHandle, "LOGIC_GROUP", value);
                            row["LOGIC_GROUP"] = value;
                        }
                        else if (view.FocusedColumn.FieldName == "MAT_FORECAST_PRCNT")
                        {
                            //view.SetRowCellValue(cell.RowHandle, "MAT_FORECAST_PRCNT", value);
                            row["MAT_FORECAST_PRCNT"] = value;
                        }
                        else if (view.FocusedColumn.FieldName == "COLOR_FORECAST_PRCNT")
                        {
                            //view.SetRowCellValue(cell.RowHandle, "COLOR_FORECAST_PRCNT", value);
                            row["COLOR_FORECAST_PRCNT"] = value;
                        }
                    }

                    //if (rowStatus != "I" && rowStatus != "D")
                    //    view.SetRowCellValue(cell.RowHandle, "ROW_STATUS", "U");

                    if (row["ROW_STATUS"].ToString().Equals("I") == false)
                        row["ROW_STATUS"] = "U";
                }

                #region Method - RowHandle(Past Version)

                //foreach (GridCell cell in cells)
                //{
                //    // 삭제된 행 제외하고 값 업데이트
                //    string rowStatus = view.GetRowCellValue(cell.RowHandle, "ROW_STATUS").ToString();

                //    if (rowStatus != "D")
                //    {
                //        if (view.FocusedColumn.FieldName == "COLOR_CD")
                //        {
                //            view.SetRowCellValue(cell.RowHandle, "PCX_COLOR_ID", pcxColorID);
                //            view.SetRowCellValue(cell.RowHandle, "COLOR_CD", pdmColorCode);
                //            view.SetRowCellValue(cell.RowHandle, "COLOR_NAME", pdmColorName);
                //        }
                //        else if (view.FocusedColumn.FieldName == "COLOR_NAME")
                //        {
                //            view.SetRowCellValue(cell.RowHandle, "PCX_COLOR_ID", pcxColorID);
                //            view.SetRowCellValue(cell.RowHandle, "COLOR_CD", pdmColorCode);
                //            view.SetRowCellValue(cell.RowHandle, "COLOR_NAME", pdmColorName);
                //        }
                //        else if (view.FocusedColumn.FieldName == "PART_NAME")
                //        {
                //            // To avoid user mistake.
                //            // When an user changed part type from lamination to normal part, clear the pattern part.
                //            if (oldValue.ToString().Contains("LAMINATION") == true && partName.Contains("LAMINATION") == false)
                //            {
                //                view.SetRowCellValue(cell.RowHandle, "PTRN_PART_NAME", "");
                //                view.SetRowCellValue(cell.RowHandle, "PTRN_PART_CD", "");
                //            }

                //            view.SetRowCellValue(cell.RowHandle, "PART_NAME", partName);
                //            view.SetRowCellValue(cell.RowHandle, "PART_CD", partCode);

                //            if (partType == "MIDSOLE" || partType == "OUTSOLE")
                //                view.SetRowCellValue(cell.RowHandle, "PART_TYPE", partType);

                //            view.SetRowCellValue(cell.RowHandle, "PCX_PART_ID", pcxPartId);

                //            Common.BindDefaultMaterialByNCFRule(view, cell.RowHandle, pcxPartId);
                //        }
                //        else if (view.FocusedColumn.FieldName == "PTRN_PART_NAME")
                //        {
                //            view.SetRowCellValue(cell.RowHandle, "PTRN_PART_NAME", partName);
                //            view.SetRowCellValue(cell.RowHandle, "PTRN_PART_CD", partCode);
                //        }
                //        else if (view.FocusedColumn.FieldName == "MXSXL_NUMBER")
                //        {
                //            view.SetRowCellValue(cell.RowHandle, "MXSXL_NUMBER", mxsxlNumber);
                //            view.SetRowCellValue(cell.RowHandle, "PCX_SUPP_MAT_ID", (pcxSuppMatID == "") ? "100" : pcxSuppMatID);
                //            view.SetRowCellValue(cell.RowHandle, "CS_CD", csCode);
                //            view.SetRowCellValue(cell.RowHandle, "MCS_NUMBER", mcsNumber);
                //            view.SetRowCellValue(cell.RowHandle, "PCX_MAT_ID", (pcxMatID == "") ? "100" : pcxMatID);
                //            view.SetRowCellValue(cell.RowHandle, "MAT_CD", pdmMatCode);
                //            view.SetRowCellValue(cell.RowHandle, "MAT_NAME", (pdmMatName == "") ? "PLACEHOLDER" : pdmMatName);
                //            view.SetRowCellValue(cell.RowHandle, "VENDOR_NAME", vendorName);
                //            view.SetRowCellValue(cell.RowHandle, "NIKE_MS_STATE", nikeMSState);
                //            view.SetRowCellValue(cell.RowHandle, "BLACK_LIST_YN", blackListYN);
                //        }
                //        else if (view.FocusedColumn.FieldName == "MAT_NAME")
                //        {
                //            view.SetRowCellValue(cell.RowHandle, "MXSXL_NUMBER", mxsxlNumber);
                //            view.SetRowCellValue(cell.RowHandle, "PCX_SUPP_MAT_ID", (pcxSuppMatID == "") ? "100" : pcxSuppMatID);
                //            view.SetRowCellValue(cell.RowHandle, "CS_CD", csCode);
                //            view.SetRowCellValue(cell.RowHandle, "MCS_NUMBER", mcsNumber);
                //            view.SetRowCellValue(cell.RowHandle, "PCX_MAT_ID", (pcxMatID == "") ? "100" : pcxMatID);
                //            view.SetRowCellValue(cell.RowHandle, "MAT_CD", pdmMatCode);
                //            view.SetRowCellValue(cell.RowHandle, "MAT_NAME", (pdmMatName == "") ? "PLACEHOLDER" : pdmMatName);
                //            view.SetRowCellValue(cell.RowHandle, "VENDOR_NAME", vendorName);
                //            view.SetRowCellValue(cell.RowHandle, "NIKE_MS_STATE", nikeMSState);
                //            view.SetRowCellValue(cell.RowHandle, "BLACK_LIST_YN", blackListYN);
                //        }
                //        else if (view.FocusedColumn.FieldName == "MAT_COMMENT")
                //        {
                //            view.SetRowCellValue(cell.RowHandle, "MAT_COMMENT", value);
                //        }
                //        else if (view.FocusedColumn.FieldName == "COLOR_COMMENT")
                //        {
                //            view.SetRowCellValue(cell.RowHandle, "COLOR_COMMENT", value);
                //        }
                //        else if (view.FocusedColumn.FieldName == "PROCESS")
                //        {
                //            view.SetRowCellValue(cell.RowHandle, "PROCESS", value);
                //        }
                //        else if (view.FocusedColumn.FieldName == "REMARKS")
                //        {
                //            view.SetRowCellValue(cell.RowHandle, "REMARKS", value);
                //        }
                //        else if (view.FocusedColumn.FieldName == "PART_TYPE")
                //        {
                //            view.SetRowCellValue(cell.RowHandle, "PART_TYPE", value);
                //        }
                //        else if (view.FocusedColumn.FieldName == "BTTM")
                //        {
                //            view.SetRowCellValue(cell.RowHandle, "BTTM", value);
                //        }
                //        else if (view.FocusedColumn.FieldName == "PART_NIKE_COMMENT")
                //        {
                //            view.SetRowCellValue(cell.RowHandle, "PART_NIKE_COMMENT", value);
                //        }
                //        else if (view.FocusedColumn.FieldName == "LOGIC_GROUP")
                //        {
                //            view.SetRowCellValue(cell.RowHandle, "LOGIC_GROUP", value);
                //        }
                //        else if (view.FocusedColumn.FieldName == "MAT_FORECAST_PRCNT")
                //        {
                //            view.SetRowCellValue(cell.RowHandle, "MAT_FORECAST_PRCNT", value);
                //        }
                //        else if (view.FocusedColumn.FieldName == "COLOR_FORECAST_PRCNT")
                //        {
                //            view.SetRowCellValue(cell.RowHandle, "COLOR_FORECAST_PRCNT", value);
                //        }
                //    }

                //    // Change indicator of the row to 'Update', except new and deletion row.
                //    if (rowStatus != "I" && rowStatus != "D")
                //        view.SetRowCellValue(cell.RowHandle, "ROW_STATUS", "U");
                //}

                #endregion

                view.RefreshData();
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

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void CustomRowCellStyle(object sender, RowCellStyleEventArgs e)
        {
            GridView view = sender as GridView;

            #region Apply color to the row deleted. - Top Priority.

            string rowStatus = view.GetRowCellValue(e.RowHandle, "ROW_STATUS").ToString();

            if (rowStatus == "D")
            {
                if (view.IsCellSelected(e.RowHandle, e.Column) == false)
                {
                    e.Appearance.BackColor = Color.DarkGray;

                    // Priority 1.
                    return;
                }
            }

            #endregion

            if (e.Column.FieldName == "PCX_PART_ID")
            {
                #region Empty

                string pcxPartId = view.GetRowCellValue(e.RowHandle, "PCX_PART_ID").ToString();

                if (pcxPartId == "")
                {
                    if (view.IsCellSelected(e.RowHandle, e.Column) == false)
                        e.Appearance.BackColor = Color.DeepSkyBlue;
                }

                #endregion
            }
            else if (e.Column.FieldName == "PART_NAME")
            {
                #region No Library

                string partName = view.GetRowCellValue(e.RowHandle, "PART_NAME").ToString();

                if (partName == "Failed to match a part with pcx library." ||
                    partName == "Failed to match a pattern part with pcx library.")
                {
                    if (view.IsCellSelected(e.RowHandle, e.Column) == false)
                    {
                        e.Appearance.ForeColor = Color.Red;
                        e.Appearance.Font = new Font(e.Appearance.Font, FontStyle.Bold);
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
            else if (e.Column.FieldName == "PART_TYPE")
            {
                #region Valid part type

                string[] validPartTypes = new string[] { "UPPER", "MIDSOLE", "OUTSOLE", "PACKAGING", "AIRBAG" };
                string partType = view.GetRowCellValue(e.RowHandle, "PART_TYPE").ToString();

                if (partType == "" || validPartTypes.Contains(partType) == false)
                {
                    if (view.IsCellSelected(e.RowHandle, e.Column) == false)
                        e.Appearance.BackColor = Color.DeepSkyBlue;
                }

                #endregion
            }
            else if (e.Column.FieldName == "PCX_SUPP_MAT_ID")
            {
                #region PLACEHOLDER & N/A

                string pcxSuppMatID = view.GetRowCellValue(e.RowHandle, "PCX_SUPP_MAT_ID").ToString();

                if (pcxSuppMatID == "100")
                {
                    if (view.IsCellSelected(e.RowHandle, e.Column) == false)
                    {
                        e.Appearance.ForeColor = Color.Red;
                        e.Appearance.Font = new Font(e.Appearance.Font, FontStyle.Bold);
                    }
                }
                else if (pcxSuppMatID == "999")
                {
                    if (view.IsCellSelected(e.RowHandle, e.Column) == false)
                    {
                        e.Appearance.ForeColor = Color.Red;
                        e.Appearance.Font = new Font(e.Appearance.Font, FontStyle.Bold);
                    }
                }
                else if (pcxSuppMatID == "")
                {
                    if (view.IsCellSelected(e.RowHandle, e.Column) == false)
                    {
                        e.Appearance.BackColor = Color.DeepSkyBlue;
                        //e.Appearance.Font = new Font(e.Appearance.Font, FontStyle.Bold);
                    }
                }

                #endregion
            }
            else if (e.Column.FieldName == "MAT_NAME")
            {
                #region Special Materials

                string materialName = view.GetRowCellValue(e.RowHandle, "MAT_NAME").ToString();
                string pcxMaterialID = view.GetRowCellValue(e.RowHandle, "PCX_MAT_ID").ToString();
                string nikeMsState = view.GetRowCellValue(e.RowHandle, "NIKE_MS_STATE").ToString();
                string blackListYN = view.GetRowCellValue(e.RowHandle, "BLACK_LIST_YN").ToString();

                if (materialName == "PLACEHOLDER" || materialName == "N/A")
                {
                    if (view.IsCellSelected(e.RowHandle, e.Column) == false)
                    {
                        e.Appearance.ForeColor = Color.Red;
                        e.Appearance.Font = new Font(e.Appearance.Font, FontStyle.Bold);
                    }
                }
                else if (pcxMaterialID == "78730" || pcxMaterialID == "78728" || pcxMaterialID == "79466"
                    || pcxMaterialID == "78732" || pcxMaterialID == "78734" || pcxMaterialID == "79482")
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

                if (nikeMsState == "Retired")
                {
                    /* Retired Material */

                    if (view.IsCellSelected(e.RowHandle, e.Column) == false)
                    {
                        e.Appearance.ForeColor = Color.Orange;
                        e.Appearance.Font = new Font(e.Appearance.Font, FontStyle.Bold);
                        e.Appearance.BackColor = Color.Firebrick;
                    }
                }

                if (blackListYN == "Y")
                {
                    /* BlackList Material */

                    if (view.IsCellSelected(e.RowHandle, e.Column) == false)
                    {
                        e.Appearance.ForeColor = Color.White;
                        e.Appearance.Font = new Font(e.Appearance.Font, FontStyle.Bold);
                        e.Appearance.BackColor = Color.Black;
                    }
                }

                #endregion
            }

            #region Apply user custom color to the view.

            string colorList = view.GetRowCellValue(e.RowHandle, "CELL_COLOR").ToString();
            string[] items = colorList.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);

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

        /// <summary>
        /// <para>1. Event for deletion.</para>
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void CustomKeyDown(object sender, KeyEventArgs e)
        {
            try
            {
                if (e.KeyCode == Keys.Delete)
                {
                    GridView view = sender as GridView;

                    // To avoid event loop.
                    view.CellValueChanged -= new CellValueChangedEventHandler(CustomCellValueChanged);

                    List<DataRow> selectedRows = new List<DataRow>();

                    foreach (int rowHandle in view.GetSelectedRows())
                        selectedRows.Add(view.GetDataRow(rowHandle));

                    foreach (DataRow row in selectedRows)
                    {
                        string fieldName = view.FocusedColumn.FieldName;

                        // Below columns can not be changed.
                        if (fieldName == "PART_NIKE_NO" || fieldName == "UPD_USER" || fieldName == "UPD_YMD")
                            continue;

                        string rowStatus = row["ROW_STATUS"].ToString();

                        // Delete data.
                        if (rowStatus != "D")
                        {
                            if (fieldName == "PCX_PART_ID" || fieldName == "PART_CD" || fieldName == "PART_NAME")
                            {
                                //view.SetRowCellValue(cell.RowHandle, "PCX_PART_ID", "");
                                //view.SetRowCellValue(cell.RowHandle, "PART_CD", "");
                                //view.SetRowCellValue(cell.RowHandle, "PART_NAME", "");

                                row["PCX_PART_ID"] = "";
                                row["PART_CD"] = "";
                                row["PART_NAME"] = "";
                            }
                            else if (fieldName == "PTRN_PART_NAME")
                            {
                                //view.SetRowCellValue(cell.RowHandle, "PTRN_PART_CD", "");
                                //view.SetRowCellValue(cell.RowHandle, "PTRN_PART_NAME", "");

                                row["PTRN_PART_CD"] = "";
                                row["PTRN_PART_NAME"] = "";
                            }
                            else if (fieldName == "MXSXL_NUMBER" || fieldName == "PCX_SUPP_MAT_ID"
                                || fieldName == "MAT_NAME" || fieldName == "VENDOR_NAME")
                            {
                                //view.SetRowCellValue(cell.RowHandle, "MXSXL_NUMBER", "");
                                //view.SetRowCellValue(cell.RowHandle, "PCX_SUPP_MAT_ID", "");
                                //view.SetRowCellValue(cell.RowHandle, "PCX_MAT_ID", "");
                                //view.SetRowCellValue(cell.RowHandle, "MAT_NAME", "");
                                //view.SetRowCellValue(cell.RowHandle, "MAT_CD", "");
                                //view.SetRowCellValue(cell.RowHandle, "CS_CD", "");
                                //view.SetRowCellValue(cell.RowHandle, "MCS_NUMBER", "");
                                //view.SetRowCellValue(cell.RowHandle, "VENDOR_NAME", "");

                                row["MXSXL_NUMBER"] = "";
                                row["PCX_SUPP_MAT_ID"] = "";
                                row["PCX_MAT_ID"] = "";
                                row["MAT_NAME"] = "";
                                row["MAT_CD"] = "";
                                row["CS_CD"] = "";
                                row["MCS_NUMBER"] = "";
                                row["VENDOR_NAME"] = "";
                            }
                            else if (fieldName == "PCX_COLOR_ID" || fieldName == "COLOR_CD"
                                || fieldName == "COLOR_NAME")
                            {
                                //view.SetRowCellValue(cell.RowHandle, "PCX_COLOR_ID", "");
                                //view.SetRowCellValue(cell.RowHandle, "COLOR_CD", "");
                                //view.SetRowCellValue(cell.RowHandle, "COLOR_NAME", "");

                                row["PCX_COLOR_ID"] = "";
                                row["COLOR_CD"] = "";
                                row["COLOR_NAME"] = "";
                            }
                            else if (fieldName == "MAT_FORECAST_PRCNT" || fieldName == "COLOR_FORECAST_PRCNT")
                            {
                                //view.SetRowCellValue(cell.RowHandle, cell.Column, 0);
                                row[fieldName] = 0;
                            }
                            else
                            {
                                //view.SetRowCellValue(cell.RowHandle, cell.Column, "");
                                row[fieldName] = "";
                            }

                            //if (rowStatus != "I")
                            //    view.SetRowCellValue(cell.RowHandle, "ROW_STATUS", "U");

                            // Change indicator of view to 'Update'.
                            if (rowStatus.Equals("I") == false)
                                row["ROW_STATUS"] = "U";
                        }
                    }

                    #region Method - RowHandle(Past Version)

                    //GridCell[] cells = view.GetSelectedCells();
                    //foreach (GridCell cell in cells)
                    //{
                    //    string fieldName = cell.Column.FieldName;

                    //    // Columns which can not be deleted.
                    //    if (view == gvwData)
                    //    {
                    //        if (fieldName == "PART_NIKE_NO" || fieldName == "UPD_USER" || fieldName == "UPD_YMD")
                    //            continue;
                    //    }

                    //    string rowStatus = view.GetRowCellValue(cell.RowHandle, "ROW_STATUS").ToString();

                    //    // Delete data.
                    //    if (rowStatus != "D")
                    //    {
                    //        if (fieldName == "PCX_PART_ID" || fieldName == "PART_CD" || fieldName == "PART_NAME")
                    //        {
                    //            view.SetRowCellValue(cell.RowHandle, "PCX_PART_ID", "");
                    //            view.SetRowCellValue(cell.RowHandle, "PART_CD", "");
                    //            view.SetRowCellValue(cell.RowHandle, "PART_NAME", "");
                    //        }
                    //        else if (fieldName == "PTRN_PART_NAME")
                    //        {
                    //            view.SetRowCellValue(cell.RowHandle, "PTRN_PART_CD", "");
                    //            view.SetRowCellValue(cell.RowHandle, "PTRN_PART_NAME", "");
                    //        }
                    //        else if (fieldName == "MXSXL_NUMBER" || fieldName == "PCX_SUPP_MAT_ID"
                    //            || fieldName == "MAT_NAME" || fieldName == "VENDOR_NAME")
                    //        {
                    //            view.SetRowCellValue(cell.RowHandle, "MXSXL_NUMBER", "");
                    //            view.SetRowCellValue(cell.RowHandle, "PCX_SUPP_MAT_ID", "");
                    //            view.SetRowCellValue(cell.RowHandle, "PCX_MAT_ID", "");
                    //            view.SetRowCellValue(cell.RowHandle, "MAT_NAME", "");
                    //            view.SetRowCellValue(cell.RowHandle, "MAT_CD", "");
                    //            view.SetRowCellValue(cell.RowHandle, "CS_CD", "");
                    //            view.SetRowCellValue(cell.RowHandle, "MCS_NUMBER", "");
                    //            view.SetRowCellValue(cell.RowHandle, "VENDOR_NAME", "");
                    //        }
                    //        else if (fieldName == "PCX_COLOR_ID" || fieldName == "COLOR_CD"
                    //            || fieldName == "COLOR_NAME")
                    //        {
                    //            view.SetRowCellValue(cell.RowHandle, "PCX_COLOR_ID", "");
                    //            view.SetRowCellValue(cell.RowHandle, "COLOR_CD", "");
                    //            view.SetRowCellValue(cell.RowHandle, "COLOR_NAME", "");
                    //        }
                    //        else if (fieldName == "MAT_FORECAST_PRCNT" || fieldName == "COLOR_FORECAST_PRCNT")
                    //        {
                    //            view.SetRowCellValue(cell.RowHandle, cell.Column, 0);
                    //        }
                    //        else
                    //            view.SetRowCellValue(cell.RowHandle, cell.Column, "");

                    //        // Change indicator of view to 'Update'.
                    //        if (rowStatus != "I")
                    //            view.SetRowCellValue(cell.RowHandle, "ROW_STATUS", "U");
                    //    }
                    //}

                    #endregion

                    view.RefreshData();
                    view.CellValueChanged += new CellValueChangedEventHandler(CustomCellValueChanged);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
                return;
            }
        }

        #endregion

        #region Form Event

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void LogicBOMEditing_FormClosing(object sender, FormClosingEventArgs e)
        {
            Common.RefreshHeaderInfo(Common.viewPCXBOMManagement, ParentRowhandle);
        }

        #endregion
    }
}