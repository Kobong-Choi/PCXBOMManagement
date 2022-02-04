using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Collections;                         // ArrayList
using System.Reflection;                          // Assembly Class
using System.Diagnostics;                         // Process
using System.IO;                                  // Path Class
using System.Xml;                                 // Related to XML
using System.Net.Mail;
using System.Net;
using System.Net.Mime;
using System.Data.OleDb;                          // OleDbConnection

using CSI.Client.ProjectBaseForm;                 // ProjectBaseForm Class
using CSI.PCC.PCX.COM;                            // Common Class
using CSI.PCC.PCX.PACKAGE;                        // Package Class

using JPlatform.Client.Library.interFace;

using Newtonsoft.Json;                            // Related to JSON
using Newtonsoft.Json.Linq;                       // JToken, JObject

using DevExpress.XtraGrid.Views.Grid;             // GridView Event
using DevExpress.XtraSplashScreen;                // XtraSplashScreen
using DevExpress.XtraEditors;                     // CheckedComboBoxEdit

using Excel = Microsoft.Office.Interop.Excel;     // Excel Manage

namespace CSI.PCC.PCX
{
    public partial class PCXBOMManagement : ProjectBaseForm
    {
        private class BOMHeader
        {
            // Automatically from JSON.
            public string objectId { get; set; }
            public string bomPartUUID { get; set; }         // new for Gemini.
            public string objectType { get; set; }
            public string bomContractVersion { get; set; }
            public string developmentStyleIdentifier { get; set; }
            public string developmentColorwayIdentifier { get; set; }
            public string colorwayName { get; set; }
            public string sourcingConfigurationIdentifier { get; set; }
            public string sourcingConfigurationName { get; set; }
            public string seasonIdentifier { get; set; }    // new for Gemini.
            public string bomIdentifier { get; set; }
            public string bomName { get; set; }
            public string bomVersionNumber { get; set; }
            public string bomDescription { get; set; }
            public string bomComments { get; set; }
            public string billOfMaterialStatusIndicator { get; set; }
            public string createTimestamp { get; set; }
            public string changeTimestamp { get; set; }
            public string createdBy { get; set; }
            public string modifiedBy { get; set; }
            public string styleNumber { get; set; }
            public string styleName { get; set; }
            public string modelIdentifier { get; set; }
            public string gender { get; set; }
            public string age { get; set; }
            public string productId { get; set; }
            public string productCode { get; set; }
            public string colorwayCode { get; set; }

            // by manual.
            public string genderName { get; set; }
            public string prodFactory { get; set; }
        }
        private class BOMData
        {
            // Automatically from JSON.
            public string bomLineSortSequence { get; set; }
            public string billOfMaterialsSectionIdentifier { get; set; }
            public string billOfMaterialsLineItemUUID { get; set; }     // new for Gemini.
            public string partNameIdentifier { get; set; }
            public string patternPartIdentifier { get; set; }
            public string suppliedMaterialIdentifier { get; set; }
            public string materialItemIdentifier { get; set; }
            public string materialItemPlaceholderDescription { get; set; }
            public string colorIdentifier { get; set; }
            public string colorPlaceholderDescription { get; set; }
            public string suppliedMaterialColorIsMultipleColors { get; set; }
            public string suppliedMaterialColorIdentifier { get; set; }
            public string bomLineItemComments { get; set; }
            public string factoryInHouseIndicator { get; set; }
            public string countyOfOriginIdentifier { get; set; }
            public string actualDimensionDescription { get; set; }
            public string isoMeasurementCode { get; set; }
            public string netUsageNumber { get; set; }
            public string wasteUsageNumber { get; set; }
            public string partYield { get; set; }
            public string consumptionConversionRate { get; set; }
            public string lineItemDefectPercentNumber { get; set; }
            public string unitPriceISOMeasurementCode { get; set; }
            public string currencyCode { get; set; }
            public string factoryUnitPrice { get; set; }
            public string unitPriceUpchargeNumber { get; set; }
            public string unitPriceUpchargeDescription { get; set; }
            public string freightTermIdentifier { get; set; }
            public string landedCostPercentNumber { get; set; }
            public string pccSortOrder { get; set; }
            public string rollupVariationLevel { get; set; }    // will be deleted.

            // by manual.
            public string partType { get; set; }
            public string partCode { get; set; }
            public string partName { get; set; }
            public string ptrnPartCode { get; set; }
            public string ptrnPartName { get; set; }
            public string csPtrnCode { get; set; }
            public string csPtrnName { get; set; }
            public string pdmMaterialCode { get; set; }
            public string pdmMaterialName { get; set; }
            public string materialComments { get; set; }
            public string mxsxlNumber { get; set; }
            public string mcsNumber { get; set; }
            public string colorCode { get; set; }
            public string colorName { get; set; }
            public string colorComments { get; set; }
            public string vendorName { get; set; }
            public string csCode { get; set; }
        }
        private readonly Font generalFont = new Font("Tahoma", 9, FontStyle.Bold);
        private string[] ownColorColumns = new string[] { "BOM_STATUS", "PUR_STATUS", "WS_STATUS", "LINE_CNT" };
        private GridView grdView;

        public PCXBOMManagement()
        {
            InitializeComponent();
        }

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);

            this.AssemblyVersion = Assembly.GetExecutingAssembly().GetName().Version.ToString();
            this.FormName = this.Name;

            this.NewButton =
            this.SaveButton =
            this.DeleteButton =
            this.DeleteRowButton =
            this.AddButton = false;

            grdView = gvwBomList;

            // Set initial values.
            Common.projectBaseForm = this;
            Common.viewPCXBOMManagement = gvwBomList;
            Common.controlPCXBOMManagement = grdBomList;
            Common.sessionID = SessionInfo.UserID;
            Common.sessionFactory = SessionInfo.Factory;

            Common.BindLookUpEdit("Factory", lePCC, false, "");
            Common.BindLookUpEdit("Developer", leDeveloper, true, "");
            Common.BindChkComboEdit("Season", chkCBSeason, false, "");
            Common.BindChkComboEdit("SampleType", chkCBSampleType, false, "A");

            if (Common.adminUser.Contains(SessionInfo.UserID))
                this.btnShow.Visible = true;

            // HQ only manages 'GEL' models.
            if (SessionInfo.Factory != "DS")
                gvwBomList.Columns["GEL_YN"].Visible = false;
        }

        #region ContextMenu Events.

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ContextMenu_Click(object sender, EventArgs e)
        {
            ToolStripMenuItem menuItem = sender as ToolStripMenuItem;

            Func<bool> AreRowsSelected = () =>
            {
                if (gvwBomList.GetSelectedRows().Length < 2)
                {
                    MessageBox.Show("Please select more than one row.", "",
                        MessageBoxButtons.OK, MessageBoxIcon.Information, MessageBoxDefaultButton.Button1);
                    return false;
                }
                else
                    return true;
            };

            Func<bool> hasLogicBOM = () =>
            {
                foreach (int rowHandle in gvwBomList.GetSelectedRows())
                {
                    if (gvwBomList.GetRowCellValue(rowHandle, "LOGIC_BOM_YN").ToString() == "Y")
                    {
                        MessageBox.Show("You can't use this function for logic BOM.", "",
                            MessageBoxButtons.OK, MessageBoxIcon.Error, MessageBoxDefaultButton.Button1);

                        return true;
                    }
                }

                return false;
            };

            switch (menuItem.Name)
            {
                case "multipleEditing":

                    if (!AreRowsSelected())
                        return;

                    if (Common.HasBOMLocked(gvwBomList))
                        return;

                    if (hasLogicBOM())
                        return;

                    ShowMultiEditForm();
                    break;

                case "multiplePurchase":

                    if (!AreRowsSelected())
                        return;

                    ShowMultiPurchaseForm();
                    break;

                case "deleteBom":

                    DeleteSelectedBOM();
                    break;

                case "copyBom":

                    CopyBOM();
                    break;

                case "fakeBOM":

                    FakeBOM();
                    break;

                case "wsConfirm":

                    if (hasLogicBOM())
                        return;

                    MakeStatusReadyOnSite();
                    break;

                case "request":

                    if (hasLogicBOM())
                        return;

                    RequestWorkOrder();
                    break;

                case "headerUpdate":

                    ShowHeaderUpdater();
                    break;

                case "designateAsRep":

                    DesignateRepBOM();
                    break;

                case "release":

                    if (hasLogicBOM())
                        return;

                    Release();
                    break;

                case "compareBOM":

                    if (!AreRowsSelected())
                        return;

                    if (Common.HasBOMLocked(gvwBomList))
                        return;

                    if (hasLogicBOM())
                        return;

                    CompareBOM();
                    break;

                case "cancelBOM":

                    CancelBOM();
                    break;

                case "extractPtrnPart":

                    ExtractPtrnPart();
                    break;

                case "newMultiEdit":

                    OpenNewMultiEdit();
                    break;

                case "reqUnlock":

                    if (hasLogicBOM())
                        return;

                    RequestUnlock();
                    break;

                default:
                    break;
            }
        }

        /// <summary>
        /// BOM 멀티 수정 팝업창을 띄운다.
        /// </summary>
        private void ShowMultiEditForm()
        {
            List<string> styleNumbers = new List<string>();
            List<string> bomTypes = new List<string>() { "Inline", "Fake" };
            string[] inlineCodes = new string[] { "N", "M", "W", "C" };

            foreach (int rowHandle in gvwBomList.GetSelectedRows())
            {
                if (gvwBomList.GetRowCellValue(rowHandle, "DEV_COLORWAY_ID").ToString() == "")
                {
                    MessageBox.Show("Colorway ID is required for multi-edit.", "",
                        MessageBoxButtons.OK, MessageBoxIcon.Error, MessageBoxDefaultButton.Button1);

                    return;
                }

                if (gvwBomList.GetRowCellValue(rowHandle, "LOGIC_BOM_YN").ToString() == "Y")
                {
                    MessageBox.Show("Logic BOM can not be available for multi function.", "",
                            MessageBoxButtons.OK, MessageBoxIcon.Error, MessageBoxDefaultButton.Button1);

                    return;
                }

                if (inlineCodes.Contains(gvwBomList.GetRowCellValue(rowHandle, "CS_BOM_CFM").ToString()))
                {
                    if (bomTypes.Contains("Inline"))
                        bomTypes.Remove("Inline");
                }
                else
                {
                    if (bomTypes.Contains("Fake"))
                        bomTypes.Remove("Fake");
                }

                styleNumbers.Add(gvwBomList.GetRowCellValue(rowHandle, "DEV_STYLE_NUMBER").ToString());
            }

            if (styleNumbers.AsEnumerable().GroupBy(x => x).Count() > 1)
            {
                if (MessageBox.Show("You selected more than one style. Do you want to proceed?", "",
                    MessageBoxButtons.OKCancel, MessageBoxIcon.Question, MessageBoxDefaultButton.Button1) == System.Windows.Forms.DialogResult.Cancel)

                    return;
            }

            if (bomTypes.Count == 0)
            {
                MessageBox.Show("Inline BOM cannot be compared to Fake BOM.", "",
                    MessageBoxButtons.OK, MessageBoxIcon.Error, MessageBoxDefaultButton.Button1);

                return;
            }

            BOMEditing editForm = new BOMEditing();
            editForm.Factory = gvwBomList.GetFocusedRowCellValue("FACTORY").ToString();
            editForm.WSNumber = Common.ChainValues(gvwBomList, "WS_NO");
            editForm.EditType = "Multiple";
            editForm.NumSelectedBOMs = gvwBomList.GetSelectedRows().Length;
            editForm.CSBOMStatus = (gvwBomList.GetRowCellValue(gvwBomList.FocusedRowHandle, "CS_BOM_CFM").ToString() == "F") ? "F" : "Inline";

            editForm.Show();
        }

        /// <summary>
        /// 멀티 발주창을 띄운다.
        /// </summary>
        private void ShowMultiPurchaseForm()
        {
            List<string> styleNumbers = new List<string>();

            foreach (int rowHandle in gvwBomList.GetSelectedRows())
            {
                if (gvwBomList.GetRowCellValue(rowHandle, "LOGIC_BOM_YN").ToString() == "Y")
                {
                    MessageBox.Show("Logic BOM can not be available for multi function.", "",
                            MessageBoxButtons.OK, MessageBoxIcon.Error, MessageBoxDefaultButton.Button1);

                    return;
                }

                styleNumbers.Add(gvwBomList.GetRowCellValue(rowHandle, "DEV_STYLE_NUMBER").ToString());
            }

            if (styleNumbers.AsEnumerable().GroupBy(x => x).Count() > 1)
            {
                if (MessageBox.Show("You selected more than one style. Do you want to proceed?", "",
                    MessageBoxButtons.OKCancel, MessageBoxIcon.Question, MessageBoxDefaultButton.Button1) == System.Windows.Forms.DialogResult.Cancel)

                    return;
            }

            //InitializeSequence("REQ_NO");

            Purchase form = new Purchase
            {
                Factory = gvwBomList.GetFocusedRowCellValue("FACTORY").ToString(),
                WorksheetNumbers = Common.ChainValues(gvwBomList, "WS_NO"),
                EditType = "Multiple",
                HasLockedBOM = Common.HasBOMLocked(gvwBomList)
            };

            if (form.ShowDialog() == DialogResult.OK)
                BindDataSourceGridView();
        }

        /// <summary>
        /// Delete BOM's selected.
        /// </summary>
        /// <returns></returns>
        private void DeleteSelectedBOM()
        {
            DataSet ds = null;
            ArrayList list = new ArrayList();
            string worksheetNumbers = string.Empty;
            bool hasPurchased = false;

            if (MessageBox.Show("Do you really want to delete BOM's you selected?", "",
                MessageBoxButtons.OKCancel, MessageBoxIcon.Question, MessageBoxDefaultButton.Button1) == DialogResult.Cancel)
                return;

            foreach (int rowHandle in gvwBomList.GetSelectedRows())
            {
                // BOM made by other person can not be deleted.
                if (gvwBomList.GetRowCellValue(rowHandle, "PCC_PM").ToString() != Common.sessionID)
                    continue;

                // BOM being worked on site can not be deleted.
                switch (gvwBomList.GetRowCellValue(rowHandle, "WS_STATUS_CD").ToString())
                {
                    case "C":
                    case "K":
                    case "Y":
                        continue;
                }

                // BOM whose material orders have been requested can not be deleted.
                PKG_INTG_BOM.SELECT_BOM_INFO_BY_CONFIRM pkgSelect = new PKG_INTG_BOM.SELECT_BOM_INFO_BY_CONFIRM();
                pkgSelect.ARG_WORK_TYPE = "PurchaseCheck";
                pkgSelect.ARG_FACTORY = gvwBomList.GetRowCellValue(rowHandle, "FACTORY").ToString();
                pkgSelect.ARG_WS_NO = gvwBomList.GetRowCellValue(rowHandle, "WS_NO").ToString();
                pkgSelect.ARG_PART_SEQ = "";
                pkgSelect.OUT_CURSOR = string.Empty;

                ds = Exe_Select_PKG(pkgSelect);

                if (ds.Tables[0].Rows.Count > 0)
                {
                    if (Convert.ToInt32(ds.Tables[0].Rows[0]["CNT"]) > 0)
                    {
                        hasPurchased = true;
                        continue;
                    }
                }

                if (worksheetNumbers.Equals(""))
                    worksheetNumbers = gvwBomList.GetRowCellValue(rowHandle, "WS_NO").ToString();
                else
                    worksheetNumbers += "," + gvwBomList.GetRowCellValue(rowHandle, "WS_NO").ToString();
            }

            if (hasPurchased)
            {
                MessageBox.Show("BOM whose material orders have been requested can not be deleted.", "",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning, MessageBoxDefaultButton.Button1);
            }

            if (!worksheetNumbers.Equals(""))
            {
                PKG_INTG_BOM.DELETE_BOM_DATA pkgDelete = new PKG_INTG_BOM.DELETE_BOM_DATA();
                pkgDelete.ARG_PCC = gvwBomList.GetFocusedRowCellValue("FACTORY").ToString();
                pkgDelete.ARG_CONCAT_WS_NO = worksheetNumbers;
                pkgDelete.ARG_DELETE_USER = Common.sessionID;

                list.Add(pkgDelete);

                if (Exe_Modify_PKG(list) == null)
                {
                    MessageBox.Show("Failed to delete BOM's.", "",
                        MessageBoxButtons.OK, MessageBoxIcon.Error, MessageBoxDefaultButton.Button1);
                }

                BindDataSourceGridView();
            }
        }

        /// <summary>
        /// Copy BOM from target.
        /// </summary>
        private void CopyBOM()
        {
            if (gvwBomList.GetSelectedRows().Length > 1)
            {
                // When an user selected more than one BOM.
                MessageBox.Show("Please select only one BOM to copy.", "Warning",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);

                return;
            }
            else
            {
                CopyBOM copyForm = new CopyBOM()
                {
                    Factory = gvwBomList.GetFocusedRowCellValue("FACTORY").ToString(),
                    WSNo = gvwBomList.GetFocusedRowCellValue("WS_NO").ToString(),
                    SeasonCode = gvwBomList.GetFocusedRowCellValue("SEASON_CD").ToString(),
                    SampleTypeCode = gvwBomList.GetFocusedRowCellValue("ST_CD").ToString(),
                    SubTypeCode = gvwBomList.GetFocusedRowCellValue("SUB_ST_CD").ToString(),
                    ColorwayID = gvwBomList.GetFocusedRowCellValue("DEV_COLORWAY_ID").ToString()
                };

                if (copyForm.ShowDialog() == DialogResult.OK)
                    BindDataSourceGridView();
            }
        }

        /// <summary>
        /// Change BOM status to 'WS Confirm'.
        /// </summary>
        private void MakeStatusReadyOnSite()
        {
            DataSet ds = null;
            ArrayList list = new ArrayList();
            string worksheetNumbers = Common.ChainValues(gvwBomList, "WS_NO");

            // Validate part codes and CS pattern part codes for all lineitems are entered.
            PKG_INTG_BOM.SELECT_BOM_CONFIRM_VLDTN pkgValidate = new PKG_INTG_BOM.SELECT_BOM_CONFIRM_VLDTN();
            pkgValidate.ARG_WORK_TYPE = "NullPartExisting";
            pkgValidate.ARG_FACTORY = gvwBomList.GetFocusedRowCellValue("FACTORY").ToString();
            pkgValidate.ARG_WS_NO = worksheetNumbers;
            pkgValidate.OUT_CURSOR = string.Empty;

            ds = Exe_Select_PKG(pkgValidate);

            if (ds.Tables[0].Rows.Count > 0)
            {
                MessageBox.Show("Please make sure all of parts & CS pattern's are input.", "",
                    MessageBoxButtons.OK, MessageBoxIcon.Error, MessageBoxDefaultButton.Button1);
                return;
            }

            // Optional Validation : If Midsole or Outsole has been checked, Data input to BTTM is recommended.
            PKG_INTG_BOM.SELECT_BOM_CONFIRM_VLDTN pkgValidate2 = new PKG_INTG_BOM.SELECT_BOM_CONFIRM_VLDTN();
            pkgValidate2.ARG_WORK_TYPE = "BTTMOmission";
            pkgValidate2.ARG_FACTORY = gvwBomList.GetFocusedRowCellValue("FACTORY").ToString();
            pkgValidate2.ARG_WS_NO = worksheetNumbers;
            pkgValidate2.OUT_CURSOR = string.Empty;

            ds = Exe_Select_PKG(pkgValidate2);

            if (Convert.ToInt32(ds.Tables[0].Rows[0]["CNT"]) > 0)
            {
                if (MessageBox.Show("Midsole or Outsole has been marked but BTTM is empty.\nDo you want to proceed?", "",
                    MessageBoxButtons.YesNo, MessageBoxIcon.Warning, MessageBoxDefaultButton.Button1) == DialogResult.No)
                    return;
            }

            // Update BOM status.
            PKG_INTG_BOM.UPDATE_BOM_CFM_INFO pkgUpdate = new PKG_INTG_BOM.UPDATE_BOM_CFM_INFO();
            pkgUpdate.ARG_FACTORY = gvwBomList.GetFocusedRowCellValue("FACTORY").ToString();
            pkgUpdate.ARG_WS_NO = worksheetNumbers;
            pkgUpdate.ARG_CS_BOM_CFM = "W";
            pkgUpdate.ARG_CBD_YN = "Pass";
            pkgUpdate.ARG_UPD_USER = Common.sessionID;

            list.Add(pkgUpdate);

            if (Exe_Modify_PKG(list) == null)
            {
                MessageBox.Show("Failed to change BOM status.", "",
                    MessageBoxButtons.OK, MessageBoxIcon.Error, MessageBoxDefaultButton.Button1);
                return;
            }

            foreach (int rowHandle in gvwBomList.GetSelectedRows())
            {
                gvwBomList.SetRowCellValue(rowHandle, "BOM_STATUS", "WS Confirm");
                gvwBomList.SetRowCellValue(rowHandle, "CS_BOM_CFM", "W");
            }

            MessageBox.Show("Complete");
        }

        /// <summary>
        /// Request work order to workshop.
        /// </summary>
        private void RequestWorkOrder()
        {
            DataSet ds = null;
            ArrayList list = new ArrayList();
            string sampleETS = string.Empty;
            string earliestShipDate = string.Empty;
            string factory = string.Empty;
            string worksheetNumber = string.Empty;
            string statusCode = string.Empty;
            string[] possibleBOMStatus = new string[] { "C", "W", "F" };
            bool isHoliday = false;

            foreach (int rowHandle in gvwBomList.GetSelectedRows())
            {
                factory = gvwBomList.GetRowCellValue(rowHandle, "FACTORY").ToString();
                worksheetNumber = gvwBomList.GetRowCellValue(rowHandle, "WS_NO").ToString();

                #region Confirm the Sample ETS is valid.

                sampleETS = gvwBomList.GetRowCellValue(rowHandle, "SAMPLE_ETS").ToString();

                if (sampleETS == "")
                {
                    MessageBox.Show("Please make sure you had input sample ETS.", "",
                        MessageBoxButtons.OK, MessageBoxIcon.Error, MessageBoxDefaultButton.Button1);
                    return;
                }

                if (sampleETS.Length == 8)
                {
                    DateTime dateTime = new DateTime(
                        Convert.ToInt32(sampleETS.Substring(0, 4)),
                        Convert.ToInt32(sampleETS.Substring(4, 2)),
                        Convert.ToInt32(sampleETS.Substring(6, 2))
                        );

                    // Compare the sample ETS with Today.
                    if (dateTime < DateTime.Now)
                    {
                        MessageBox.Show("Sample ETS can not be earlier than today.", "",
                            MessageBoxButtons.OK, MessageBoxIcon.Error, MessageBoxDefaultButton.Button1);
                        return;
                    }

                    // Get the earliest possible ship date.
                    PKG_INTG_BOM_COMMON.SELECT_POSSIBLE_SHIP_DATE pkgSelect = new PKG_INTG_BOM_COMMON.SELECT_POSSIBLE_SHIP_DATE();
                    pkgSelect.ARG_FACTORY = factory;
                    pkgSelect.OUT_CURSOR = string.Empty;

                    ds = Exe_Select_PKG(pkgSelect);

                    if (ds.Tables[0].Rows.Count > 0)
                    {
                        earliestShipDate = ds.Tables[0].Rows[0]["WORK_YMD"].ToString();

                        if (Convert.ToInt32(earliestShipDate) > Convert.ToInt32(sampleETS))
                        {
                            MessageBox.Show("Sample ETS must be at least 5 working days from today.", "",
                                MessageBoxButtons.OK, MessageBoxIcon.Error, MessageBoxDefaultButton.Button1);
                            return;
                        }
                    }

                    // Confirm the sample ETS is holiday.
                    PKG_INTG_BOM_COMMON.SELECT_IS_HOLIDAY pkgSelect2 = new PKG_INTG_BOM_COMMON.SELECT_IS_HOLIDAY();
                    pkgSelect2.ARG_FACTORY = factory;
                    pkgSelect2.ARG_WORK_YMD = sampleETS;
                    pkgSelect2.OUT_CURSOR = string.Empty;

                    ds = Exe_Select_PKG(pkgSelect2);

                    if (ds.Tables[0].Rows.Count > 0)
                    {
                        isHoliday = ds.Tables[0].Rows[0]["HOLIDAY_YN"].ToString().Equals("Y") ? true : false;

                        if (isHoliday)
                        {
                            MessageBox.Show("Sample ETS can not be a holiday.", "",
                                MessageBoxButtons.OK, MessageBoxIcon.Error, MessageBoxDefaultButton.Button1);
                            return;
                        }
                    }
                }
                else
                {
                    MessageBox.Show("Not a valid format.", "",
                        MessageBoxButtons.OK, MessageBoxIcon.Error, MessageBoxDefaultButton.Button1);
                    return;
                }

                #endregion

                #region Confirm the worksheet status is ready to request.

                // Load the worksheet status and operations.
                PKG_INTG_BOM.SELECT_WS_STATUS_OP_CNT pkgSelect3 = new PKG_INTG_BOM.SELECT_WS_STATUS_OP_CNT();
                pkgSelect3.ARG_FACTORY = factory;
                pkgSelect3.ARG_WS_NO = worksheetNumber;
                pkgSelect3.OUT_CURSOR = string.Empty;

                ds = Exe_Select_PKG(pkgSelect3);

                if (ds.Tables[0].Rows.Count > 0)
                {
                    statusCode = ds.Tables[0].Rows[0]["WS_STATUS"].ToString();
                }
                else
                {
                    MessageBox.Show("At least one operation should be included.", "",
                        MessageBoxButtons.OK, MessageBoxIcon.Error, MessageBoxDefaultButton.Button1);

                    Common.FocusCell(gvwBomList, rowHandle, "WS_STATUS", false);
                    return;
                }

                if (!statusCode.Equals("N"))
                {
                    MessageBox.Show("Not a valid status to request.", "",
                        MessageBoxButtons.OK, MessageBoxIcon.Error, MessageBoxDefaultButton.Button1);

                    if (statusCode == "C")
                        gvwBomList.SetRowCellValue(rowHandle, "WS_STATUS", "Confirm");
                    else if (statusCode == "Y")
                        gvwBomList.SetRowCellValue(rowHandle, "WS_STATUS", "Request");
                    else if (statusCode == "K")
                        gvwBomList.SetRowCellValue(rowHandle, "WS_STATUS", "Checked");

                    Common.FocusCell(gvwBomList, rowHandle, "WS_STATUS", false);
                    return;
                }

                #endregion

                #region Confirm the bom status is ready to request.

                if (!possibleBOMStatus.Contains(gvwBomList.GetRowCellValue(rowHandle, "CS_BOM_CFM").ToString()))
                {
                    MessageBox.Show("Invalid BOM status to request.", "",
                        MessageBoxButtons.OK, MessageBoxIcon.Error, MessageBoxDefaultButton.Button1);

                    Common.FocusCell(gvwBomList, rowHandle, "BOM_STATUS", false);
                    return;
                }

                #endregion

                #region Confirm required fields have been entered.
                
                PKG_INTG_BOM_WORKSHEET.LOAD_WORKSHEET_CONTENTS pkgSelect4 = new PKG_INTG_BOM_WORKSHEET.LOAD_WORKSHEET_CONTENTS();
                pkgSelect4.ARG_WORK_TYPE = "Validation";
                pkgSelect4.ARG_FACTORY = factory;
                pkgSelect4.ARG_WS_NO = worksheetNumber;
                pkgSelect4.OUT_CURSOR = string.Empty;

                ds = Exe_Select_PKG(pkgSelect4);

                if (ds.Tables[0].Rows.Count > 0)
                {
                    foreach (DataColumn column in ds.Tables[0].Columns)
                    {
                        if (ds.Tables[0].Rows[0].IsNull(column))
                        {
                            if (gvwBomList.GetRowCellValue(rowHandle, "BOM_STATUS").ToString().Equals("Fake")
                                && (column.ColumnName.Equals("BOM_ID") || column.ColumnName.Equals("DPA")))
                                continue;
                            else
                                return;
                        }
                    }
                }

                #endregion

                #region Confirm the promo cover page have been made.
                
                if (gvwBomList.GetRowCellValue(rowHandle, "SAMPLE_TYPE").ToString() == "PROMO")
                {
                    PKG_INTG_BOM.SELECT_REQ_ON_SITE pkgSelect5 = new PKG_INTG_BOM.SELECT_REQ_ON_SITE();
                    pkgSelect5.ARG_WORK_TYPE = "IS_EXISTING";
                    pkgSelect5.ARG_FACTORY = factory;
                    pkgSelect5.ARG_WS_NO = worksheetNumber;
                    pkgSelect5.OUT_CURSOR = string.Empty;

                    ds = Exe_Select_PKG(pkgSelect5);

                    if (!ds.Tables[0].Rows[0]["FLAG"].ToString().Equals("EXISTING"))
                    {
                        MessageBox.Show("Please input 'Promo Cover Page'.", "",
                            MessageBoxButtons.OK, MessageBoxIcon.Error, MessageBoxDefaultButton.Button1);
                        return;
                    }
                }

                #endregion

                if (MessageBox.Show("Would you like to drop the worksheet on-site?", "",
                    MessageBoxButtons.OKCancel, MessageBoxIcon.Question) == DialogResult.Cancel)
                    return;

                // Transfer data.
                PKG_INTG_BOM.INSERT_PCC_PLAN_OPCD pkgInsert = new PKG_INTG_BOM.INSERT_PCC_PLAN_OPCD();
                pkgInsert.ARG_FACTORY = factory;
                pkgInsert.ARG_WS_NO = worksheetNumber;
                pkgInsert.UPD_USER = Common.sessionID;

                list.Add(pkgInsert);

                if (Exe_Modify_PKG(list) == null)
                {
                    MessageBox.Show("Failed to request work order.", "",
                        MessageBoxButtons.OK, MessageBoxIcon.Error, MessageBoxDefaultButton.Button1);
                    return;
                }

                Common.RefreshHeaderInfo(gvwBomList, rowHandle);

                #region backup

                //if (CheckSampleETSIsValid(rowHandle))
                //{
                //    string factory = gvwBomList.GetRowCellValue(rowHandle, "FACTORY").ToString();
                //    string wsNo = gvwBomList.GetRowCellValue(rowHandle, "WS_NO").ToString();

                //    // Check the current W/S status and Verify that at least one operation is selected.
                //    PKG_INTG_BOM.SELECT_WS_STATUS_OP_CNT pkgSelect = new PKG_INTG_BOM.SELECT_WS_STATUS_OP_CNT();
                //    pkgSelect.ARG_FACTORY = factory;
                //    pkgSelect.ARG_WS_NO = wsNo;
                //    pkgSelect.OUT_CURSOR = string.Empty;

                //    DataTable dataSource = Exe_Select_PKG(pkgSelect).Tables[0];

                //    if (dataSource.Rows.Count > 0)
                //    {
                //        switch (dataSource.Rows[0]["WS_STATUS"].ToString())
                //        {
                //            case "C":
                //                MessageBox.Show("Already Confirmed");
                //                continue;

                //            case "Y":
                //                MessageBox.Show("Already Requested");
                //                continue;

                //            case "K":
                //                MessageBox.Show("Already Checked");
                //                continue;

                //            default:
                //                break;
                //        }
                //    }
                //    else
                //    {
                //        MessageBox.Show("Please select at least one operation.");
                //        FocusCellInWork(rowHandle, "WS_STATUS");

                //        return;
                //    }

                //    // BOM status must be 'Confirm' or 'W/S Confirm'.
                //    if (gvwBomList.GetRowCellValue(rowHandle, "CS_BOM_CFM").ToString() == "N")
                //    {
                //        MessageBox.Show("BOM Status must be W/S Confirm or Confirm");
                //        FocusCellInWork(rowHandle, "BOM_STATUS");

                //        return;
                //    }

                //    if (CheckWSRequiredFieldIsInput(rowHandle))
                //    {
                //        // In case of 'PROMO' round.
                //        if (gvwBomList.GetRowCellValue(rowHandle, "SAMPLE_TYPE").ToString() == "PROMO")
                //        {
                //            // Validate that the promo cover page are made.
                //            PKG_INTG_BOM.SELECT_REQ_ON_SITE pkgSelectFlag = new PKG_INTG_BOM.SELECT_REQ_ON_SITE();
                //            pkgSelectFlag.ARG_WORK_TYPE = "IS_EXISTING";
                //            pkgSelectFlag.ARG_FACTORY = factory;
                //            pkgSelectFlag.ARG_WS_NO = wsNo;
                //            pkgSelectFlag.OUT_CURSOR = string.Empty;

                //            DataTable returnData = Exe_Select_PKG(pkgSelectFlag).Tables[0];

                //            string flag = returnData.Rows[0]["FLAG"].ToString();

                //            if (flag == "EXISTING")
                //            {
                //                // Skip.
                //            }
                //            else if (flag == "NOT_EXISTING")
                //            {
                //                MessageBox.Show("Please input 'Promo Cover Page'.");
                //                return;
                //            }
                //        }

                //        if (MessageBox.Show("Would you like to drop the worksheet on-site?", "",
                //            MessageBoxButtons.OKCancel, MessageBoxIcon.Question) == System.Windows.Forms.DialogResult.Cancel)
                //            return;

                //        // Data Transfer.
                //        PKG_INTG_BOM.INSERT_PCC_PLAN_OPCD pkgInsert = new PKG_INTG_BOM.INSERT_PCC_PLAN_OPCD();
                //        pkgInsert.ARG_FACTORY = factory;
                //        pkgInsert.ARG_WS_NO = wsNo;
                //        pkgInsert.UPD_USER = Common.sessionID;

                //        ArrayList arrayList = new ArrayList();
                //        arrayList.Add(pkgInsert);

                //        if (Exe_Modify_PKG(arrayList) == null)
                //        {
                //            MessageBox.Show("Failed to request");
                //            return;
                //        }

                //        Common.RefreshHeaderInfo(gvwBomList, rowHandle);
                //    }
                //    else
                //    {
                //        MessageBox.Show("Please save the worksheet first, there are required data have not been input.");
                //        FocusCellInWork(rowHandle, "WS_STATUS");
                //    }
                //}
                //else
                //{
                //    FocusCellInWork(rowHandle, "SAMPLE_ETS");
                //    return;
                //}
                
                #endregion
            }
        }

        /// <summary>
        /// Change current BOM status to initial status.
        /// </summary>
        /// <returns></returns>
        private void Release()
        {
            DataSet ds = null;
            ArrayList list = new ArrayList();
            string factory = string.Empty;
            string worksheetNumber = string.Empty;
            string statusCode = string.Empty;
            string placesInWorkshop = string.Empty;

            foreach (int rowHandle in gvwBomList.GetSelectedRows())
            {
                factory = gvwBomList.GetRowCellValue(rowHandle, "FACTORY").ToString();
                worksheetNumber = gvwBomList.GetRowCellValue(rowHandle, "WS_NO").ToString();

                // Load worksheet status and operations.
                PKG_INTG_BOM.SELECT_WS_STATUS_OP_CNT pkgSelect = new PKG_INTG_BOM.SELECT_WS_STATUS_OP_CNT();
                pkgSelect.ARG_FACTORY = factory;
                pkgSelect.ARG_WS_NO = worksheetNumber;
                pkgSelect.OUT_CURSOR = string.Empty;

                ds = Exe_Select_PKG(pkgSelect);

                if (ds.Tables[0].Rows.Count > 0)
                {
                    statusCode = ds.Tables[0].Rows[0]["WS_STATUS"].ToString();
                }
                else
                {
                    MessageBox.Show("At least one operation should be included.", "",
                        MessageBoxButtons.OK, MessageBoxIcon.Error, MessageBoxDefaultButton.Button1);

                    Common.FocusCell(gvwBomList, rowHandle, "WS_STATUS", false);
                    return;
                }

                switch (statusCode)
                {
                    case "N":
                    case "C":

                        MessageBox.Show("Invalid status to release.", "",
                            MessageBoxButtons.OK, MessageBoxIcon.Error, MessageBoxDefaultButton.Button1);

                        if (statusCode == "C")
                            gvwBomList.SetRowCellValue(rowHandle, "WS_STATUS", "Confirm");

                        Common.FocusCell(gvwBomList, rowHandle, "WS_STATUS", false);
                        return;

                    case "K":

                        // Confirm some sites have checked this worksheet.
                        PKG_INTG_BOM.SELECT_BOM_INFO_BY_CONFIRM pkgSelect2 = new PKG_INTG_BOM.SELECT_BOM_INFO_BY_CONFIRM();
                        pkgSelect2.ARG_WORK_TYPE = "WorkInProgress";
                        pkgSelect2.ARG_FACTORY = factory;
                        pkgSelect2.ARG_WS_NO = worksheetNumber;
                        pkgSelect2.ARG_PART_SEQ = "";
                        pkgSelect2.OUT_CURSOR = string.Empty;

                        ds = Exe_Select_PKG(pkgSelect2);

                        if (ds.Tables[0].Rows.Count > 0)
                        {
                            MessageBox.Show(string.Format("the WorkSheet already checked from {0} can not be released.",
                               ds.Tables[0].Rows[0][0].ToString().Substring(0, ds.Tables[0].Rows[0][0].ToString().Length - 1)),
                               "", MessageBoxButtons.OK, MessageBoxIcon.Error, MessageBoxDefaultButton.Button1);

                            gvwBomList.SetRowCellValue(rowHandle, "WS_STATUS", "Checked");
                            Common.FocusCell(gvwBomList, rowHandle, "WS_STATUS", false);
                            return;
                        }

                        break;

                    case "Y":
                        // Normal.
                        break;
                }

                // Confirm the some operations have been completed on site.
                PKG_INTG_BOM.SELECT_BOM_INFO_BY_CONFIRM pkgSelect3 = new PKG_INTG_BOM.SELECT_BOM_INFO_BY_CONFIRM();
                pkgSelect3.ARG_WORK_TYPE = "CheckProdScan";
                pkgSelect3.ARG_FACTORY = factory;
                pkgSelect3.ARG_WS_NO = worksheetNumber;
                pkgSelect3.ARG_PART_SEQ = "";
                pkgSelect3.OUT_CURSOR = string.Empty;

                ds = Exe_Select_PKG(pkgSelect3);

                if (ds.Tables[0].Rows.Count > 0)
                {
                    if (Convert.ToInt32(ds.Tables[0].Rows[0][0]) > 0)
                    {
                        MessageBox.Show("Some operations have been completed on site.", "",
                            MessageBoxButtons.OK, MessageBoxIcon.Error, MessageBoxDefaultButton.Button1);

                        Common.FocusCell(gvwBomList, rowHandle, "WS_STATUS", false);
                        return;
                    }
                }

                // Release W/S status.
                PKG_INTG_BOM.UPDATE_WS_RELEASE_BOM pkgUpdate = new PKG_INTG_BOM.UPDATE_WS_RELEASE_BOM();
                pkgUpdate.ARG_FACTORY = factory;
                pkgUpdate.ARG_WS_NO = worksheetNumber;
                pkgUpdate.ARG_UPD_USER = Common.sessionID;

                list.Add(pkgUpdate);

                if (Exe_Modify_PKG(list) == null)
                {
                    MessageBox.Show("Failed to release.", "",
                        MessageBoxButtons.OK, MessageBoxIcon.Error, MessageBoxDefaultButton.Button1);
                    return;
                }

                Common.RefreshHeaderInfo(gvwBomList, rowHandle);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        private void ConfirmBOM()
        {
            try
            {
                if (MessageBox.Show("Do you really want to confirm?", "Confirm BOM", MessageBoxButtons.OKCancel, MessageBoxIcon.Question) == System.Windows.Forms.DialogResult.OK)
                { }
                else
                    return;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
                return;
            }
        }

        /// <summary>
        /// Show form of Header Updater.
        /// </summary>
        private void ShowHeaderUpdater()
        {
            BOMHeadUpdater updateForm = new BOMHeadUpdater()
            {
                Factory = gvwBomList.GetFocusedRowCellValue("FACTORY").ToString(),
                WSNumber = ConcatenateWorksheetNumber()
            };

            if (updateForm.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                BindDataSourceGridView();
                MessageBox.Show("Complete.");
            }
        }

        /// <summary>
        /// Validate that the sample ETS is valid.
        /// </summary>
        /// <param name="rowHandle"></param>
        /// <returns></returns>
        private bool IsSampleETSValid(int rowHandle)
        {
            try
            {
                string sampleETS = gvwBomList.GetRowCellValue(rowHandle, "SAMPLE_ETS").ToString();

                if (sampleETS == "")
                {
                    MessageBox.Show("Please make sure you had input sample ets date.");
                    return false;
                }

                // Validate the format of sample ETS.
                if (sampleETS.Length == 8)
                {
                    int year = Convert.ToInt32(sampleETS.Substring(0, 4));
                    int month = Convert.ToInt32(sampleETS.Substring(4, 2));
                    int day = Convert.ToInt32(sampleETS.Substring(6, 2));

                    DateTime date = new DateTime(year, month, day);

                    if (date < DateTime.Now)
                    {
                        MessageBox.Show("Sample ETS can not be a past date.");
                        return false;
                    }

                    // Validate that the date is valid.
                    PKG_INTG_BOM_COMMON.SELECT_POSSIBLE_SHIP_DATE pkgSelect = new PKG_INTG_BOM_COMMON.SELECT_POSSIBLE_SHIP_DATE();
                    pkgSelect.ARG_FACTORY = gvwBomList.GetRowCellValue(rowHandle, "FACTORY").ToString();
                    pkgSelect.OUT_CURSOR = string.Empty;

                    DataTable dataSource = Exe_Select_PKG(pkgSelect).Tables[0];

                    if (dataSource.Rows.Count > 0)
                    {
                        string earliestShipDate = dataSource.Rows[0]["WORK_YMD"].ToString();

                        if (Convert.ToInt32(earliestShipDate) > Convert.ToInt32(sampleETS))
                        {
                            MessageBox.Show("Sample ETS must be at least 5 working days from today.");
                            return false;
                        }
                    }

                    // Validate that the date is holiday.
                    PKG_INTG_BOM_COMMON.SELECT_IS_HOLIDAY pkgSelectHoliday = new PKG_INTG_BOM_COMMON.SELECT_IS_HOLIDAY();
                    pkgSelectHoliday.ARG_FACTORY = gvwBomList.GetRowCellValue(rowHandle, "FACTORY").ToString();
                    pkgSelectHoliday.ARG_WORK_YMD = sampleETS;
                    pkgSelectHoliday.OUT_CURSOR = string.Empty;

                    DataTable _dataSource = Exe_Select_PKG(pkgSelectHoliday).Tables[0];

                    if (_dataSource.Rows.Count > 0)
                    {
                        string isHoliday = _dataSource.Rows[0]["HOLIDAY_YN"].ToString();

                        if (isHoliday == "Y")
                        {
                            MessageBox.Show("Sample ETS can not be a holiday.");
                            return false;
                        }
                    }
                }
                else
                {
                    MessageBox.Show("Not a valid sample ets format.");
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

        /// <summary>
        /// Show Fake BOM Uploader.
        /// </summary>
        private void FakeBOM()
        {
            BOMUploader_FakeBOM uploader = new BOMUploader_FakeBOM();

            if (uploader.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                BindDataSourceGridView();
        }

        /// <summary>
        /// 선택된 BOM의 Style Number가 다른 경우 계속 진행할지 물어봄
        /// Man/Woman으로 나뉠 경우 스타일 넘버가 달라도 파트는 동일함
        /// </summary>
        /// <param name="rowHandles"></param>
        /// <returns></returns>
        private bool AreStyleNumbersSame()
        {
            List<string> styleNumbers = new List<string>();

            foreach (int rowHandle in gvwBomList.GetSelectedRows())
                styleNumbers.Add(gvwBomList.GetRowCellValue(rowHandle, "DEV_STYLE_NUMBER").ToString());

            if (styleNumbers.AsEnumerable().GroupBy(x => x).Count() > 1)
            {
                if (MessageBox.Show("You selected more than one style. Do you want to proceed?", "",
                    MessageBoxButtons.OKCancel, MessageBoxIcon.Question, MessageBoxDefaultButton.Button1) == System.Windows.Forms.DialogResult.OK)
                    return true;
                else
                    return false;
            }
            else
                return true;
        }

        /// <summary>
        /// 멀티 편집 시 컬러웨이 ID로 구분하므로, BOM 헤더 정보에 컬러웨이 ID가 입력되었는지 확인한다.
        /// </summary>
        /// <param name="rowHandles"></param>
        /// <returns></returns>
        private bool IsColorwayIDExisting(int[] rowHandles)
        {
            foreach (int rowHandle in rowHandles)
            {
                if (gvwBomList.GetRowCellValue(rowHandle, "DEV_COLORWAY_ID").ToString() == "")
                {
                    MessageBox.Show("Colorway ID is required for multi-editing.", "",
                        MessageBoxButtons.OK, MessageBoxIcon.Error, MessageBoxDefaultButton.Button1);

                    return false;
                }
            }

            return true;
        }

        /// <summary>
        /// Fake BOM과 Inline BOM은 멀티 편집 불가
        /// 동일 타입끼리 선택되었는지 확인
        /// </summary>
        /// <param name="rowHandles"></param>
        /// <returns></returns>
        private bool AreBOMTypesSame(int[] rowHandles)
        {
            string[] inlineType = new string[] { "N", "M", "W", "C" };
            List<string> typeList = new List<string>();

            bool allOfAreSame = true;

            foreach (int rowHandle in rowHandles)
            {
                string bomType = gvwBomList.GetRowCellValue(rowHandle, "CS_BOM_CFM").ToString();

                if (inlineType.Contains(bomType))
                    typeList.Add("Inline");
                else
                    typeList.Add("Fake");
            }

            string type = "";
            for (int i = 0; i < typeList.Count; i++)
            {
                if (type == "")
                    type = typeList[i];

                if (type == typeList[i])
                    continue;
                else
                {
                    allOfAreSame = false;
                    MessageBox.Show("Inline BOM cannot be compared to Fake BOM.");
                    return allOfAreSame;
                }
            }
            return allOfAreSame;
        }

        /// <summary>
        /// Designate the selected BOM as representative BOM.
        /// </summary>
        /// <returns></returns>
        private void DesignateRepBOM()
        {
            ArrayList list = new ArrayList();
            string season = gvwBomList.GetFocusedRowCellValue("SEASON").ToString();
            string styleNumber = gvwBomList.GetFocusedRowCellValue("DEV_STYLE_NUMBER").ToString();

            if (gvwBomList.GetSelectedRows().Length > 1)
            {
                MessageBox.Show("Please select only one BOM.", "",
                    MessageBoxButtons.OK, MessageBoxIcon.Error, MessageBoxDefaultButton.Button1);
                return;
            }

            if (season == "" || styleNumber == "")
            {
                MessageBox.Show("Season and Style Number are required. Please input these.", "",
                    MessageBoxButtons.OK, MessageBoxIcon.Error, MessageBoxDefaultButton.Button1);
                return;
            }

            // Delete : when already enrolled.
            // Merge : New or Update.
            PKG_INTG_BOM.INSERT_REP_BOM pkgInsert = new PKG_INTG_BOM.INSERT_REP_BOM();
            pkgInsert.ARG_WORK_TYPE = gvwBomList.GetFocusedRowCellValue("REP_YN").ToString().Equals("Y") ? "Delete" : "Merge";
            pkgInsert.ARG_KEY_VALUE = season + "-" + styleNumber;
            pkgInsert.ARG_FACTORY = gvwBomList.GetFocusedRowCellValue("FACTORY").ToString();
            pkgInsert.ARG_WS_NO = gvwBomList.GetFocusedRowCellValue("WS_NO").ToString();
            
            list.Add(pkgInsert);

            if (Exe_Modify_PKG(list) == null)
            {
                MessageBox.Show("Failed to designate BOM", "",
                    MessageBoxButtons.OK, MessageBoxIcon.Error, MessageBoxDefaultButton.Button1);
                return;
            }

            BindDataSourceGridView();
        }

        /// <summary>
        /// Compare BOM's.
        /// </summary>
        private void CompareBOM()
        {
            DataSet ds = null;
            List<string> list = new List<string>();
            string factory = gvwBomList.GetFocusedRowCellValue("FACTORY").ToString();
            string chainedWSNo = string.Empty;
            string colorways = string.Empty;
            string largestWSNo = string.Empty;
            bool hasConfirmedBOM = false;

            foreach (int rowHandle in gvwBomList.GetSelectedRows())
            {
                switch (gvwBomList.GetRowCellValue(rowHandle, "CS_BOM_CFM").ToString())
                {
                    case "F":

                        Common.ShowMessageBox("Fake BOM can not be compared.", "E");
                        Common.FocusCell(gvwBomList, rowHandle, "DEV_NAME", false);
                        return;

                    case "C":

                        if (!hasConfirmedBOM)
                            hasConfirmedBOM = true;

                        break;

                    default:
                        break;
                }
                
                list.Add(gvwBomList.GetRowCellValue(rowHandle, "WS_NO").ToString());
            }

            // Confirm there is a BOM which includes lineitems of which part code is null.
            PKG_INTG_COMPARE_BOM.SELECT_COMPARE_VLDTN pkgSelect = new PKG_INTG_COMPARE_BOM.SELECT_COMPARE_VLDTN();
            pkgSelect.ARG_WORK_TYPE = "EmptyPart";
            pkgSelect.ARG_FACTORY = factory;
            pkgSelect.ARG_CHAINED_WS_NO = Common.ChainValues(gvwBomList, "WS_NO");
            pkgSelect.OUT_CURSOR = string.Empty;

            ds = Exe_Select_PKG(pkgSelect);

            if (ds.Tables[0].Rows.Count > 0)
            {
                foreach (DataRow row in ds.Tables[0].Rows)
                {
                    if (colorways == "")
                        colorways = row["DEV_COLORWAY_ID"].ToString();
                    else
                        colorways += ", " + row["DEV_COLORWAY_ID"].ToString();
                }

                if (MessageBox.Show(string.Format("There is a lineitem of which part code is empty in colorway {0}. \nThis line is not shown. Do you want to proceed?", colorways), "",
                    MessageBoxButtons.OKCancel, MessageBoxIcon.Warning) == DialogResult.Cancel)
                    return;
            }

            // Select the BOM with the largest number of line items.
            PKG_INTG_COMPARE_BOM.SELECT_LONGEST_BOM pkgSelect2 = new PKG_INTG_COMPARE_BOM.SELECT_LONGEST_BOM();
            pkgSelect2.ARG_FACTORY = factory;
            pkgSelect2.ARG_CHAINED_WS_NO = Common.ChainValues(gvwBomList, "WS_NO");
            pkgSelect2.OUT_CURSOR = string.Empty;

            ds = Exe_Select_PKG(pkgSelect2);

            if (ds.Tables[0].Rows[0]["FLAG"].ToString().Equals("Duplicate"))
            {
                Common.ShowMessageBox("The BOM with the largest number of line items can not have duplicate part.", "E");
                return;
            }

            largestWSNo = ds.Tables[0].Rows[0]["WS_NO"].ToString();
            
            // Except the BOM which has the largest number of line items.
            list.Remove(largestWSNo);

            foreach (string wsNumber in list)
            {
                if (chainedWSNo.Equals(""))
                    chainedWSNo = wsNumber;
                else
                    chainedWSNo += "," + wsNumber;
            }

            CompareBOM form = new CompareBOM()
            {
                BASE_FACTORY = factory,
                BASE_WS_NO = largestWSNo,
                SUB_WS_NO = chainedWSNo,
                BOM_STATUS = hasConfirmedBOM ? "C" : "N",
            };

            form.Show();
        }

        /// <summary>
        /// Cancel BOM's.
        /// </summary>
        private void CancelBOM()
        {
            DataSet ds = null;
            ArrayList list = new ArrayList();
            string factory = string.Empty;
            string wsNo = string.Empty;

            if (MessageBox.Show("Do you really want to proceed?", "",
                MessageBoxButtons.OKCancel, MessageBoxIcon.Question, MessageBoxDefaultButton.Button1) == DialogResult.Cancel)
                return;

            foreach (int rowHandle in gvwBomList.GetSelectedRows())
            {
                factory = gvwBomList.GetRowCellValue(rowHandle, "FACTORY").ToString();
                wsNo = gvwBomList.GetRowCellValue(rowHandle, "WS_NO").ToString();

                if (gvwBomList.GetRowCellValue(rowHandle, "BOM_STATUS").ToString().Equals("Cancel"))
                {
                    /* Release */

                    // Confirm there is a BOM with same unique key before releasing.
                    PKG_INTG_BOM.CANCEL_RELEASE_BOM pkgSelect = new PKG_INTG_BOM.CANCEL_RELEASE_BOM();
                    pkgSelect.ARG_WORK_TYPE = "UniqueCheck";
                    pkgSelect.ARG_FACTORY = factory;
                    pkgSelect.ARG_WS_NO = wsNo;
                    pkgSelect.ARG_UPD_USER = Common.sessionID;
                    pkgSelect.OUT_CURSOR = string.Empty;

                    ds = Exe_Select_PKG(pkgSelect);

                    if (ds.Tables[0].Rows.Count > 0)
                    {
                        if (Convert.ToInt32(ds.Tables[0].Rows[0]["CNT"]) == 0)
                        {
                            // Possible to release.
                            PKG_INTG_BOM.CANCEL_RELEASE_BOM pkgUpdate = new PKG_INTG_BOM.CANCEL_RELEASE_BOM();
                            pkgUpdate.ARG_WORK_TYPE = "Release";
                            pkgUpdate.ARG_FACTORY = factory;
                            pkgUpdate.ARG_WS_NO = wsNo;
                            pkgUpdate.ARG_UPD_USER = Common.sessionID;
                            pkgUpdate.OUT_CURSOR = string.Empty;

                            list.Add(pkgUpdate);
                        }
                        else
                        {
                            // Can't possible to release.
                            MessageBox.Show(string.Format("There is a BOM with same unique key. You can't release this BOM({0}).",
                                gvwBomList.GetRowCellValue(rowHandle, "DEV_COLORWAY_ID").ToString()), "",
                                MessageBoxButtons.OK, MessageBoxIcon.Error, MessageBoxDefaultButton.Button1);

                            Common.FocusCell(gvwBomList, rowHandle, "BOM_STATUS", false);
                            return;
                        }
                    }
                }
                else
                {
                    /* Cancel */

                    PKG_INTG_BOM.CANCEL_RELEASE_BOM pkgUpdate = new PKG_INTG_BOM.CANCEL_RELEASE_BOM();
                    pkgUpdate.ARG_WORK_TYPE = "Cancel";
                    pkgUpdate.ARG_FACTORY = factory;
                    pkgUpdate.ARG_WS_NO = wsNo;
                    pkgUpdate.ARG_UPD_USER = Common.sessionID;
                    pkgUpdate.OUT_CURSOR = string.Empty;

                    list.Add(pkgUpdate);
                }
            }

            if (list.Count > 0)
            {
                if (Exe_Modify_PKG(list) == null)
                {
                    MessageBox.Show("Failed to cancel or release BOM", "",
                        MessageBoxButtons.OK, MessageBoxIcon.Error, MessageBoxDefaultButton.Button1);
                    return;
                }
            }

            BindDataSourceGridView();
        }

        /// <summary>
        /// Extract a list for CS Pattern Parts for PE.
        /// </summary>
        private void ExtractPtrnPart()
        {
            int numSelectedRows = gvwBomList.GetSelectedRows().Length;

            if (numSelectedRows > 1)
            {
                MessageBox.Show("Please select only one BOM.", "",
                    MessageBoxButtons.OK, MessageBoxIcon.Stop, MessageBoxDefaultButton.Button1);
                return;
            }
            else if (numSelectedRows == 1)
            {
                PartListForPE form = new PartListForPE() {
                    Factory = gvwBomList.GetFocusedRowCellValue("FACTORY").ToString(),
                    WorksheetNumber = gvwBomList.GetFocusedRowCellValue("WS_NO").ToString()
                };

                form.Show();
            }
        }

        /// <summary>
        /// 
        /// </summary>
        private void OpenNewMultiEdit()
        {
            NewMultiEdit form = new NewMultiEdit();

            string concatWSNo = string.Empty;
            int[] rowHandles = gvwBomList.GetSelectedRows();

            foreach (int rowHandle in rowHandles)
            {
                string wsNo = gvwBomList.GetRowCellValue(rowHandle, "WS_NO").ToString();
                concatWSNo = concatWSNo + "," + wsNo;
            }

            string[] sourceOfParent = new string[] { SessionInfo.Factory, concatWSNo, rowHandles.Length.ToString() };
            form.SOURCE_OF_PARENT = sourceOfParent;

            form.Show();
        }

        /// <summary>
        /// Validate that required fields are input to prevent requests immediately after copying.
        /// </summary>
        /// <returns></returns>
        private bool CheckWSRequiredFieldIsInput(int rowHandle)
        {
            PKG_INTG_BOM_WORKSHEET.LOAD_WORKSHEET_CONTENTS pkgSelect = new PKG_INTG_BOM_WORKSHEET.LOAD_WORKSHEET_CONTENTS();
            pkgSelect.ARG_WORK_TYPE = "Validation";
            pkgSelect.ARG_FACTORY = gvwBomList.GetRowCellValue(rowHandle, "FACTORY").ToString();
            pkgSelect.ARG_WS_NO = gvwBomList.GetRowCellValue(rowHandle, "WS_NO").ToString();
            pkgSelect.OUT_CURSOR = string.Empty;

            DataTable dataSource = Exe_Select_PKG(pkgSelect).Tables[0];

            for (int i = 0; i < dataSource.Columns.Count; i++)
            {
                if (dataSource.Rows[0][i].ToString() == "")
                {
                    // BOM ID and DPA are not requried fields for Fake BOM.
                    if (i == 3 || i == 4)
                    {
                        if (gvwBomList.GetRowCellValue(rowHandle, "BOM_STATUS").ToString() == "Fake")
                            continue;
                    }

                    return false;
                }
            }

            return true;
        }

        /// <summary>
        /// 코스팅 담당 PIC에게 모델 정보를 포함한 엑셀 파일을 첨부하여 UNLOCK 요청 메일을 발송한다.
        /// </summary>
        private void RequestUnlock()
        {
            DataTable dataSource = new DataTable();
            DataRow newRow = null;
            string DPA = string.Empty;
            string lockYN = string.Empty;

            // Confirm requried fields are entered.
            foreach (int rowHandle in gvwBomList.GetSelectedRows())
            {
                DPA = gvwBomList.GetRowCellValue(rowHandle, "DPA").ToString();
                lockYN = gvwBomList.GetRowCellValue(rowHandle, "LOCK_YN").ToString();

                // DPA must be entered and the status must be locked.
                if (DPA.Equals("") || lockYN.Equals("N"))
                {
                    MessageBox.Show("Please check DPA is entered or BOM is locked.", "",
                        MessageBoxButtons.OK, MessageBoxIcon.Warning, MessageBoxDefaultButton.Button1);

                    Common.FocusCell(gvwBomList, rowHandle, "DPA", false);
                    return;
                }
            }

            // Columns for dataSource.
            dataSource.Columns.Add("CATEGORY");
            dataSource.Columns.Add("SEASON");
            dataSource.Columns.Add("STYLE_NAME");
            dataSource.Columns.Add("STYLE_NUMBER");
            dataSource.Columns.Add("COLORWAY_DESC");
            dataSource.Columns.Add("COLORWAY_ID");
            dataSource.Columns.Add("SAMPLE_TYPE");
            dataSource.Columns.Add("SUB_REMARK");
            dataSource.Columns.Add("PRODUCT_CODE");
            dataSource.Columns.Add("DEVELOPER");
            dataSource.Columns.Add("COSTING_PIC");

            // Create dataSource.
            foreach (int rowHandle in gvwBomList.GetSelectedRows())
            {
                newRow = dataSource.NewRow();

                newRow["CATEGORY"] = gvwBomList.GetRowCellValue(rowHandle, "CATEGORY").ToString();
                newRow["SEASON"] = gvwBomList.GetRowCellValue(rowHandle, "SEASON").ToString();
                newRow["STYLE_NAME"] = gvwBomList.GetRowCellValue(rowHandle, "DEV_NAME").ToString();
                newRow["STYLE_NUMBER"] = gvwBomList.GetRowCellValue(rowHandle, "DEV_STYLE_NUMBER").ToString();
                newRow["COLORWAY_DESC"] = gvwBomList.GetRowCellValue(rowHandle, "COLOR_VER").ToString();
                newRow["COLORWAY_ID"] = gvwBomList.GetRowCellValue(rowHandle, "DEV_COLORWAY_ID").ToString();
                newRow["SAMPLE_TYPE"] = gvwBomList.GetRowCellValue(rowHandle, "SAMPLE_TYPE").ToString();
                newRow["SUB_REMARK"] = gvwBomList.GetRowCellValue(rowHandle, "SUB_TYPE_REMARK").ToString();
                newRow["PRODUCT_CODE"] = gvwBomList.GetRowCellValue(rowHandle, "STYLE_CD").ToString();
                newRow["DEVELOPER"] = gvwBomList.GetRowCellValue(rowHandle, "PCC_PM").ToString();
                newRow["COSTING_PIC"] = Common.GetPIC(gvwBomList.GetRowCellValue(rowHandle, "DPA").ToString(), "COSTING");

                dataSource.Rows.Add(newRow);
            }

            List<DataTable> dtList = dataSource.AsEnumerable().Where(x => x["COSTING_PIC"].ToString() != "").GroupBy(
                r => new { PIC = r.Field<string>("COSTING_PIC") }).Select(g => g.CopyToDataTable()).ToList();

            if (dtList.Count > 0)
            {
                try
                {
                    SplashScreenManager.ShowForm(this, typeof(MainWaitForm), false, false, false);
                    SplashScreenManager.Default.SendCommand(MainWaitForm.SplashScreenCommand.SendEmail, "");

                    foreach (DataTable dt in dtList)
                    {
                        // To avoid runtime error of System.InvalidOperationException.
                        dt.TableName = "Name";  

                        Common.projectBaseForm.SendEmailUnlock(Common.sessionID, dt.Rows[0]["COSTING_PIC"].ToString(), "TD", dt);
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
            else
            {
                MessageBox.Show("Costing PIC is not enrolled on PMX. Can't send an e-mail.", "Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Error, MessageBoxDefaultButton.Button1);
                return;
            }

            MessageBox.Show("Complete");
        }

        #endregion

        #region Button Events.

        /// <summary>
        /// Load data.
        /// </summary>
        public override void QueryClick()
        {
            BindDataSourceGridView();
        }

        /// <summary>
        /// Preview the worksheet.
        /// </summary>
        public override void PrintClick()
        {
            string today = DateTime.Now.ToString("yyyyMMdd");
            int rowCount = gvwBomList.SelectedRowsCount;

            // Create a new folder with the name of today. ex) 20211217
            DirectoryInfo di = new DirectoryInfo(@"C:\PCC_Worksheet\" + today);

            try
            {
                if (!di.Exists)
                    di.Create();

                FileSystemInfo[] infos = di.Parent.GetFileSystemInfos();

                if (infos != null)
                {
                    foreach (FileSystemInfo i in infos)
                    {
                        if (i.Name != today)
                        {
                            // Delete all files in the directory.
                            if (i is DirectoryInfo)
                                (i as DirectoryInfo).Delete(true);
                            else
                                i.Delete();
                        }
                    }

                    if (Common.sessionFactory == "DS")
                    {
                        if (rowCount <= 0 || rowCount > 20)
                        {
                            MessageBox.Show("Please select rows less than 20 rows.", "",
                                MessageBoxButtons.OK, MessageBoxIcon.Error, MessageBoxDefaultButton.Button1);
                            return;
                        }

                        List<string[]> list = new List<string[]>();

                        foreach (int rowHandle in gvwBomList.GetSelectedRows())
                        {
                            list.Add(new string[] {
                                gvwBomList.GetRowCellValue(rowHandle, "FACTORY").ToString(),
                                gvwBomList.GetRowCellValue(rowHandle, "WS_NO").ToString()
                            });
                        }

                        OpenChildForm(@"\POPUP\CSI.PCC.BOM.P_CSBOMWorksheetPrint.dll", list, OpenType.Modal);
                    }
                    else
                    {
                        List<string> list = new List<string>();

                        list.Add(gvwBomList.GetFocusedRowCellValue("FACTORY").ToString());
                        list.Add(gvwBomList.GetFocusedRowCellValue("WS_NO").ToString());

                        OpenChildForm(@"\POPUP\CSI.PCC.PCX.PCX_WORKSHEET_OVERSEAS.dll", list, OpenType.Modal);
                    }
                }
            }
            catch (Exception e)
            {
                MessageBox.Show(e.ToString());
                return;
            }

            #region Backup
            //try
            //{
            //    if (Common.sessionFactory == "DS")
            //    {
            //        int[] rowHandles = gvwBomList.GetSelectedRows();

            //        // 20개 이상 선택 불가
            //        if (rowHandles.Length > 20)
            //        {
            //            MessageBox.Show("You can't choose more than 20 sheets.\n" + "The number of rows you chose : " + rowHandles.Length + ".");
            //            return;
            //        }

            //        string today = DateTime.Today.ToString("yyyyMMdd");

            //        // 금일 기준으로 새로운 디렉토리 생성
            //        string newDirectoryPath = CLIENT_WORKSHEET_PATH + today;

            //        // Initializes a new instance of the DirectoryInfo class on the specified path.
            //        DirectoryInfo dirInfo = new DirectoryInfo(newDirectoryPath);

            //        // 디렉토리가 있는지 확인 후 없으면 새로 생성
            //        if (!dirInfo.Exists)
            //            dirInfo.Create();

            //        // Parent : Gets the parent directory of a specified subdirectory.
            //        // GetFileSystemInfos : Returns an array of strongly typed FileSystemInfo entries representing all the files and subdirectories in a directory.
            //        FileSystemInfo[] fileSysInfos = dirInfo.Parent.GetFileSystemInfos();

            //        DirectoryInfo dirInfo2 = null;

            //        for (int i = 0; i < fileSysInfos.Length; i++)
            //        {
            //            // 이전에 생성된 디렉토리가 존재할 경우 모든 파일 및 디렉토리 삭제
            //            if (fileSysInfos[i].Name != today)
            //            {
            //                dirInfo2 = new DirectoryInfo(fileSysInfos[i].FullName);
            //                FileSystemInfo[] excelFiles = dirInfo2.GetFileSystemInfos();

            //                for (int j = 0; j < excelFiles.Length; j++)
            //                    excelFiles[j].Delete();

            //                fileSysInfos[i].Delete();
            //            }
            //        }

            //        List<string[]> list = new List<string[]>();

            //        foreach (int rowHandle in rowHandles)
            //        {
            //            string factory = gvwBomList.GetRowCellValue(rowHandle, "FACTORY").ToString();
            //            string wsNo = gvwBomList.GetRowCellValue(rowHandle, "WS_NO").ToString();

            //            list.Add(new string[] { factory, wsNo });
            //        }

            //        object returnObj = OpenChildForm(@"\POPUP\CSI.PCC.BOM.P_CSBOMWorksheetPrint.dll", list,
            //            JPlatform.Client.Library.interFace.OpenType.Modal);
            //    }
            //    else
            //    {
            //        /* In case of overseas factory. */

            //        int[] iRows = gvwBomList.GetSelectedRows();

            //        string today = DateTime.Today.ToString("yyyyMMdd");
            //        // 금일 기준으로 새로운 디렉토리 생성
            //        string newDirectoryPath = CLIENT_WORKSHEET_PATH + today;

            //        #region 신규 디렉토리 생성 및 이전 디렉토리 삭제

            //        // Initializes a new instance of the DirectoryInfo class on the specified path.
            //        DirectoryInfo dirInfo = new DirectoryInfo(newDirectoryPath);

            //        // 디렉토리가 있는지 확인 후 없으면 새로 생성
            //        if (!dirInfo.Exists)
            //            dirInfo.Create();

            //        // Parent : Gets the parent directory of a specified subdirectory.
            //        // GetFileSystemInfos : Returns an array of strongly typed FileSystemInfo entries representing all the files and subdirectories in a directory.
            //        FileSystemInfo[] fileSysInfos = dirInfo.Parent.GetFileSystemInfos();

            //        DirectoryInfo dirInfo2 = null;

            //        for (int i = 0; i < fileSysInfos.Length; i++)
            //        {
            //            // 이전에 생성된 디렉토리가 존재할 경우 모든 파일 및 디렉토리 삭제
            //            if (fileSysInfos[i].Name != today)
            //            {
            //                dirInfo2 = new DirectoryInfo(fileSysInfos[i].FullName);
            //                FileSystemInfo[] excelFiles = dirInfo2.GetFileSystemInfos();

            //                for (int j = 0; j < excelFiles.Length; j++)
            //                    excelFiles[j].Delete();

            //                fileSysInfos[i].Delete();
            //            }
            //        }
            //        #endregion

            //        DirectoryInfo di = new DirectoryInfo(newDirectoryPath);       // 생성을 원하는 디렉토리 명시

            //        if (!di.Exists) // 디렉토리가 존재하는지 확인
            //        {
            //            di.Create();
            //        }

            //        List<string> list = new List<string>();

            //        list.Add(Convert.ToString(gvwBomList.GetRowCellValue(iRows[0], "FACTORY")));
            //        list.Add(Convert.ToString(gvwBomList.GetRowCellValue(iRows[0], "WS_NO")));
            //        list.Add(Convert.ToString(gvwBomList.GetRowCellValue(iRows[0], "DPA")));
            //        list.Add(String.IsNullOrEmpty(Convert.ToString(gvwBomList.GetRowCellValue(iRows[0], "WS_STATUS"))) ? "N" : Convert.ToString(gvwBomList.GetRowCellValue(iRows[0], "WS_STATUS")).Trim());
            //        list.Add(Convert.ToString(gvwBomList.GetRowCellValue(iRows[0], "")));
            //        list.Add(String.IsNullOrEmpty(Convert.ToString(gvwBomList.GetRowCellValue(iRows[0], "CS_BOM_CFM"))) ? "A" : Convert.ToString(gvwBomList.GetRowCellValue(iRows[0], "CS_BOM_CFM")).Trim());

            //        OpenChildForm(@"\POPUP\CSI.PCC.PCX.PCX_WORKSHEET_OVERSEAS.dll", list, JPlatform.Client.Library.interFace.OpenType.Modal);

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
        /// Upload a range of types of BOM's.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnUpload_Click(object sender, EventArgs e)
        {
            TypeSelection_BOM typeSelectionForm = new TypeSelection_BOM();

            if (typeSelectionForm.ShowDialog() == DialogResult.OK)
                OpenFiles(typeSelectionForm.SelectedType);
        }

        /// <summary>
        /// BOM Head 데이터 변경을 쉽게하기 위해 Factory, WS NO를 컬럼을 보이게 함, 관리자 전용
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnShow_Click(object sender, EventArgs e)
        {
            Action<string> action = (colName) =>
            {
                if (gvwBomList.Columns[colName].Visible)
                    gvwBomList.Columns[colName].Visible = false;
                else
                    gvwBomList.Columns[colName].Visible = true;
            };

            action("FACTORY");
            action("WS_NO");
        }

        #endregion

        #region Control Events.

        /// <summary>
        /// Search BOM's.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void TextEditKeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
                BindDataSourceGridView();
        }

        /// <summary>
        /// Clear all selection.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void CheckedComboBoxMouseClick(object sender, MouseEventArgs e)
        {
            if (e.Button == System.Windows.Forms.MouseButtons.Right)
                (sender as CheckedComboBoxEdit).SetEditValue("");
        }

        #endregion

        #region Grid Events.

        /// <summary>
        /// 셀에 배경색이 있는 컬럼 중, 선택된 셀은 배경색을 없앰
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void gvwBomList_RowCellStyle(object sender, RowCellStyleEventArgs e)
        {
            GridView view = sender as GridView;

            if (ownColorColumns.Contains(e.Column.FieldName))
            {
                switch (e.Column.FieldName)
                {
                    case "BOM_STATUS":

                        switch (e.CellValue.ToString())
                        {
                            case "Save":

                                if (!view.IsCellSelected(e.RowHandle, e.Column))
                                    e.Appearance.BackColor = Color.LightPink;
                                break;

                            case "Confirm":

                                if (!view.IsCellSelected(e.RowHandle, e.Column))
                                    e.Appearance.BackColor = Color.Cyan;
                                break;

                            case "WS Confirm":

                                if (!view.IsCellSelected(e.RowHandle, e.Column))
                                {
                                    e.Appearance.BackColor = Color.Green;
                                    e.Appearance.ForeColor = Color.White;
                                }
                                break;

                            case "Match":

                                if (!view.IsCellSelected(e.RowHandle, e.Column))
                                    e.Appearance.BackColor = Color.Yellow;
                                break;

                            case "Fake":

                                if (!view.IsCellSelected(e.RowHandle, e.Column))
                                    e.Appearance.BackColor = Color.YellowGreen;
                                break;

                            case "Cancel":

                                if (!view.IsCellSelected(e.RowHandle, e.Column))
                                    e.Appearance.BackColor = Color.DarkGray;
                                break;
                        }

                        break;

                    case "PUR_STATUS":

                        switch (e.CellValue.ToString())
                        {
                            case "Not Yet":

                                if (!view.IsCellSelected(e.RowHandle, e.Column))
                                    e.Appearance.BackColor = Color.LightPink;
                                break;

                            case "Purchase":

                                if (!view.IsCellSelected(e.RowHandle, e.Column))
                                    e.Appearance.BackColor = Color.Yellow;
                                break;
                        }

                        break;

                    case "WS_STATUS":

                        switch (e.CellValue.ToString())
                        {
                            case "Ready":

                                if (!view.IsCellSelected(e.RowHandle, e.Column))
                                    e.Appearance.BackColor = Color.LightPink;
                                break;

                            case "Request":

                                if (!view.IsCellSelected(e.RowHandle, e.Column))
                                    e.Appearance.BackColor = Color.Yellow;
                                break;

                            case "Checked":

                                if (!view.IsCellSelected(e.RowHandle, e.Column))
                                    e.Appearance.BackColor = Color.LimeGreen;
                                break;

                            case "Confirm":

                                if (!view.IsCellSelected(e.RowHandle, e.Column))
                                    e.Appearance.BackColor = Color.Aqua;
                                break;
                        }

                        break;

                    case "LINE_CNT":

                        if (!view.IsCellSelected(e.RowHandle, e.Column))
                            e.Appearance.BackColor = Color.LightYellow;

                        break;
                }
            }
            else
            {
                if (view.GetRowCellValue(e.RowHandle, "REP_YN").ToString() == "Y")
                {
                    if (!view.IsCellSelected(e.RowHandle, e.Column))
                    {
                        e.Appearance.BackColor = Color.SteelBlue;
                        e.Appearance.Font = generalFont;
                        e.Appearance.ForeColor = Color.White;
                    }
                }
            }
        }

        /// <summary>
        /// BOM Editing, Purchase, Worksheet 팝업 창을 띄움
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void gvwBomList_DoubleClick(object sender, EventArgs e)
        {
            GridView view = sender as GridView;
            bool isLogicBOM = view.GetFocusedRowCellValue("LOGIC_BOM_YN").ToString().Equals("Y") ? true : false;

            if (view.FocusedRowHandle < 0)
            {
                MessageBox.Show("Please search a BOM.", "",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning, MessageBoxDefaultButton.Button1);
                return;
            }

            switch (view.FocusedColumn.FieldName)
            {
                case "BOM_STATUS":

                    if (isLogicBOM)
                    {
                        LogicBOMEditing form = new LogicBOMEditing()
                        {
                            Factory = view.GetFocusedRowCellValue("FACTORY").ToString(),
                            WorksheetNumbers = view.GetFocusedRowCellValue("WS_NO").ToString(),
                            CSBOMStatus = view.GetFocusedRowCellValue("CS_BOM_CFM").ToString(),
                            ParentRowhandle = view.FocusedRowHandle
                        };

                        form.Show();
                    }
                    else
                    {
                        BOMEditing form = new BOMEditing()
                        {
                            Factory = view.GetFocusedRowCellValue("FACTORY").ToString(),
                            WSNumber = view.GetFocusedRowCellValue("WS_NO").ToString(),
                            CSBOMStatus = view.GetFocusedRowCellValue("CS_BOM_CFM").ToString(),
                            ParentRowhandle = view.FocusedRowHandle,
                            EditType = "Single",
                            NumSelectedBOMs = 1
                        };

                        form.Show();
                    }

                    break;

                case "PUR_STATUS":

                    if (isLogicBOM)
                    {
                        MessageBox.Show("You can't request ordering materials from logic BOM.", "",
                            MessageBoxButtons.OK, MessageBoxIcon.Error, MessageBoxDefaultButton.Button1);
                        return;
                    }
                    else
                    {
                        Purchase form = new Purchase
                        {
                            Factory = view.GetFocusedRowCellValue("FACTORY").ToString(),
                            WorksheetNumbers = view.GetFocusedRowCellValue("WS_NO").ToString(),
                            CSBOMStatus = view.GetFocusedRowCellValue("CS_BOM_CFM").ToString(),
                            ParentRowhandle = view.FocusedRowHandle,
                            EditType = "Single",
                            HasLockedBOM = Common.HasBOMLocked(view)
                        };

                        form.Show();
                    }

                    break;

                case "WS_STATUS":

                    if (isLogicBOM)
                    {
                        MessageBox.Show("You can't request making sample shoes from logic BOM.", "",
                            MessageBoxButtons.OK, MessageBoxIcon.Error, MessageBoxDefaultButton.Button1);
                        return;

                    }
                    else
                    {
                        string factory = view.GetRowCellValue(grdView.FocusedRowHandle, "FACTORY").ToString().Trim();
                        string wsNo = view.GetRowCellValue(grdView.FocusedRowHandle, "WS_NO").ToString().Trim();
                        string csBOMStatus = view.GetRowCellValue(grdView.FocusedRowHandle, "CS_BOM_CFM").ToString().Trim();
                        string styleName = view.GetRowCellValue(grdView.FocusedRowHandle, "DEV_NAME").ToString().Trim();
                        string colorwayID = view.GetRowCellValue(grdView.FocusedRowHandle, "DEV_COLORWAY_ID").ToString().Trim();
                        string wsStatus = view.GetRowCellValue(grdView.FocusedRowHandle, "WS_STATUS_CD").ToString().Trim();
                        string styleNumber = view.GetRowCellValue(grdView.FocusedRowHandle, "DEV_STYLE_NUMBER").ToString().Trim();
                        string season = view.GetRowCellValue(grdView.FocusedRowHandle, "SEASON").ToString().Trim();

                        string[] sourceBOMInfo = new string[] { factory, wsNo, styleName, colorwayID
                            , wsStatus, csBOMStatus, styleNumber, season};

                        WorksheetMaking makingForm = new WorksheetMaking();
                        makingForm.SOURCE_BOM_INFO = sourceBOMInfo;
                        makingForm.ROWHANDLE = grdView.FocusedRowHandle;

                        makingForm.Show();
                    }

                    break;
            }

            #region backup

            //if (grdView.FocusedColumn.FieldName == "BOM_STATUS")
            //{
            //    if (isLogicBOM == false)
            //    {
            //        // For inline & Fake BOM.
            //        BOMEditing editForm = new BOMEditing();
            //        editForm.Factory = factory;
            //        editForm.WorksheetNumbers = wsNo;
            //        editForm.EditType = "Single";
            //        editForm.NumOfSelectedBOMs = 1;
            //        editForm.ParentRowhandle = grdView.FocusedRowHandle;
            //        editForm.CSBOMStatus = csBOMStatus;

            //        editForm.Show();
            //    }
            //    else
            //    {
            //        // For only logic BOM.
            //        LogicBOMEditing editForm = new LogicBOMEditing(factory, wsNo, csBOMStatus, gvwBomList.FocusedRowHandle);

            //        editForm.Show();
            //    }
            //}
            //else if (grdView.FocusedColumn.FieldName == "PUR_STATUS")
            //{
            //    // Logic BOM vs Inline BOM
            //    if (isLogicBOM == false)
            //    {
            //        //InitializeSequence("REQ_NO");

            //        Purchase form = new Purchase
            //        {
            //            Factory = gvwBomList.GetRowCellValue(gvwBomList.FocusedRowHandle, "FACTORY").ToString(),
            //            WorksheetNumbers = gvwBomList.GetRowCellValue(gvwBomList.FocusedRowHandle, "WS_NO").ToString(),
            //            CSBOMStatus = gvwBomList.GetRowCellValue(gvwBomList.FocusedRowHandle, "CS_BOM_CFM").ToString(),
            //            ParentRowhandle = grdView.FocusedRowHandle,
            //            EditType = "Single",
            //            HasLockedBOM = Common.IsBOMLocked(grdView)
            //        };

            //        form.Show();
            //    }
            //    else
            //    {
            //        // Logic BOM don't order materials.
            //        MessageBox.Show("You can't request ordering materials from logic BOM.", "",
            //            MessageBoxButtons.OK, MessageBoxIcon.Error, MessageBoxDefaultButton.Button1);

            //        return;
            //    }
            //}
            //else if (grdView.FocusedColumn.FieldName == "WS_STATUS")
            //{
            //    if (isLogicBOM == false)
            //    {
            //        string styleName = grdView.GetRowCellValue(grdView.FocusedRowHandle, "DEV_NAME").ToString().Trim();
            //        string colorwayID = grdView.GetRowCellValue(grdView.FocusedRowHandle, "DEV_COLORWAY_ID").ToString().Trim();
            //        string wsStatus = grdView.GetRowCellValue(grdView.FocusedRowHandle, "WS_STATUS_CD").ToString().Trim();
            //        string styleNumber = grdView.GetRowCellValue(grdView.FocusedRowHandle, "DEV_STYLE_NUMBER").ToString().Trim();
            //        string season = grdView.GetRowCellValue(grdView.FocusedRowHandle, "SEASON").ToString().Trim();

            //        string[] sourceBOMInfo = new string[] { factory, wsNo, styleName, colorwayID
            //        ,wsStatus, csBOMStatus, styleNumber, season};

            //        WorksheetMaking makingForm = new WorksheetMaking();
            //        makingForm.SOURCE_BOM_INFO = sourceBOMInfo;
            //        makingForm.ROWHANDLE = grdView.FocusedRowHandle;

            //        makingForm.Show();
            //    }
            //    else
            //    {
            //        // Logic BOM don't need to make sample shoes.
            //        MessageBox.Show("You can't request making sample shoes from logic BOM.", "",
            //            MessageBoxButtons.OK, MessageBoxIcon.Error, MessageBoxDefaultButton.Button1);

            //        return;
            //    }
            //}

            #endregion
        }

        #endregion

        #region User Defiend Functions.

        ///// <summary>
        ///// Initialize component of the form.
        ///// </summary>
        //private void SetInitialValue()
        //{
        //    Common.projectBaseForm = this;
        //    Common.viewOfPCXBOMManagement = gvwBomList;
        //    Common.controlOfPCXBOMManagement = grdBomList;
        //    Common.sessionID = SessionInfo.UserID;
        //    Common.sessionFactory = SessionInfo.Factory;

        //    Common.BindToLookUpEdit("Factory", lePCC, false, "");
        //    Common.BindToLookUpEdit("Developer", leDeveloper, true, "");
        //    Common.BindToChkComboEdit("Season", chkCBSeason, false, "");
        //    Common.BindToChkComboEdit("SampleType", chkCBSampleType, false, "A");

        //    if (Common.adminUser.Contains(SessionInfo.UserID))
        //        this.btnShow.Visible = true;

        //    // HQ only manages 'GEL' models.
        //    if (SessionInfo.Factory != "DS")
        //        gvwBomList.Columns["GEL_YN"].Visible = false;
        //}

        ///// <summary>
        ///// 월이 바뀌면 각 키의 생성에 필요한 시퀀스를 초기화한다.
        ///// </summary>
        ///// <returns></returns>
        //private void InitializeSequence(string workType)
        //{
        //    ArrayList arrayList = new ArrayList();

        //    if (workType == "WS_NO")
        //    {
        //        #region Key of BOM

        //        PKG_INTG_BOM.UPDATE_INIT_SEQ_WS_NO pkgUpdate = new PKG_INTG_BOM.UPDATE_INIT_SEQ_WS_NO();
        //        pkgUpdate.ARG_WORK_TYPE = "SEQ_PCC_WS_NO";
        //        pkgUpdate.ARG_FACTORY = Common.sessionFactory;
        //        pkgUpdate.ARG_UPD_USER = Common.sessionID;

        //        arrayList.Add(pkgUpdate);

        //        if (Exe_Modify_PKG(arrayList) == null)
        //            MessageBox.Show("Failed to initialize sequence.(WS_NO)");

        //        #endregion
        //    }
        //    else if (workType == "REQ_NO")
        //    {
        //        #region Key of Purchase Request

        //        PKG_INTG_BOM.UPDATE_INIT_SEQ_WS_NO pkgUpdate = new PKG_INTG_BOM.UPDATE_INIT_SEQ_WS_NO();
        //        pkgUpdate.ARG_WORK_TYPE = "SEQ_PCC_PUR_REQ_NO";
        //        pkgUpdate.ARG_FACTORY = Common.sessionFactory;
        //        pkgUpdate.ARG_UPD_USER = Common.sessionID;

        //        arrayList.Add(pkgUpdate);

        //        if (Exe_Modify_PKG(arrayList) == null)
        //            MessageBox.Show("Failed to initialize sequence.(REQ_NO)");

        //        #endregion
        //    }
        //    else if (workType == "PCX_KEY")
        //    {
        //        #region Key of PCX BOM

        //        PKG_INTG_BOM.UPDATE_INIT_SEQ_WS_NO pkgUpdate = new PKG_INTG_BOM.UPDATE_INIT_SEQ_WS_NO();
        //        pkgUpdate.ARG_WORK_TYPE = "SEQ_PCX_KEY";
        //        pkgUpdate.ARG_FACTORY = Common.sessionFactory;
        //        pkgUpdate.ARG_UPD_USER = Common.sessionID;

        //        arrayList.Add(pkgUpdate);

        //        if (Exe_Modify_PKG(arrayList) == null)
        //            MessageBox.Show("Failed to initialize sequence.(PCX_KEY)");

        //        #endregion
        //    }
        //    else if (workType == "CDMKR_REQ")
        //    {
        //        #region Key of Codemaker Request

        //        PKG_INTG_BOM.UPDATE_INIT_SEQ_WS_NO pkgUpdate = new PKG_INTG_BOM.UPDATE_INIT_SEQ_WS_NO();
        //        pkgUpdate.ARG_WORK_TYPE = "SEQ_PCC_CDMKR_REQ";
        //        pkgUpdate.ARG_FACTORY = Common.sessionFactory;
        //        pkgUpdate.ARG_UPD_USER = Common.sessionID;

        //        arrayList.Add(pkgUpdate);

        //        if (Exe_Modify_PKG(arrayList) == null)
        //            MessageBox.Show("Failed to initialize sequence.(CDMKR_REQ)");

        //        #endregion
        //    }
        //}

        /// <summary>
        /// 업로드할 파일명을 가져옴
        /// </summary>
        /// <param name="fileType"></param>
        private void OpenFiles(string fileType)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();

            switch (fileType)
            {
                case "XML":
                    openFileDialog.Filter = "XML 파일|*.xml";
                    break;

                case "EXCEL":
                    openFileDialog.Filter = "Excel 파일|*.xls";
                    break;

                case "JSON":
                    openFileDialog.Filter = "JSON 파일|*.json";
                    break;

                case "LOGIC":
                    openFileDialog.Filter = "JSON 파일|*.json";
                    break;

                default:
                    break;
            }

            openFileDialog.Multiselect = true;
            openFileDialog.Title = "Open File";
            openFileDialog.RestoreDirectory = true;

            if (openFileDialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                string[] fileNames = openFileDialog.FileNames;

                switch (fileType)
                {
                    case "XML":
                        XmlBomUploader(fileNames);
                        break;

                    case "EXCEL":
                        ExcelBomUploader(fileNames);
                        break;

                    case "JSON":
                        UploadJSON(fileNames);
                        break;

                    case "LOGIC":   // need to upate.
                        LogicBomUploader(fileNames);
                        break;

                    default:
                        break;
                }
            }
        }

        /// <summary>
        ///  XML 형태의 BOM을 데이터베이스에 업로드 함
        /// </summary>
        /// <param name="fileNames"></param>
        private void XmlBomUploader(string[] fileNames)
        {
            try
            {
                string chainedWsNo = string.Empty;

                foreach (string fileName in fileNames)
                {
                    string fileType = string.Empty;

                    #region 헤더 속성

                    string DPA = string.Empty;
                    string pcmBOMID = string.Empty;
                    string orgSeason = string.Empty;
                    string devName = string.Empty;
                    string productCode = string.Empty;
                    string prodFactory = string.Empty;
                    string td = string.Empty;
                    string colorVer = string.Empty;
                    string modelID = string.Empty;
                    string lastCode = string.Empty;
                    string PM = string.Empty;
                    string whqDev = string.Empty;
                    string IPW = string.Empty;
                    string productID = string.Empty;

                    #endregion

                    DataSet dataSource = ExtractDataFromXMLFile(fileName);

                    if (dataSource != null)
                    {
                        #region 신규 WS_NO 채번

                        PKG_INTG_BOM.SELECT_NEW_WS_NO pkgSelectWsNo = new PKG_INTG_BOM.SELECT_NEW_WS_NO();
                        pkgSelectWsNo.ARG_FACTORY = SessionInfo.Factory;
                        pkgSelectWsNo.OUT_CURSOR = string.Empty;

                        DataTable returnValue = Exe_Select_PKG(pkgSelectWsNo).Tables[0];
                        string createdWsNo = returnValue.Rows[0]["WS_NO"].ToString();

                        #endregion

                        if (dataSource.Tables[0].TableName == "nike_samplerequest")
                        {
                            fileType = "Old";

                            DataTable sampleRequest = dataSource.Tables["nike_samplerequest"];
                            DataRow srInfo = sampleRequest.Rows[0];

                            DataTable nikeBOM = dataSource.Tables["nike_bom"];
                            DataRow bomInfo = nikeBOM.Rows[0];

                            #region 헤더 정보 저장
                            DPA = srInfo["nike_devproj_alias"].ToString().Replace("-", "");
                            pcmBOMID = bomInfo["nike_bom_id"].ToString();
                            orgSeason = srInfo["nike_season"].ToString();
                            devName = srInfo["nike_modelname"].ToString();
                            productCode = bomInfo["nike_bom_productcode"].ToString().Replace("-", "");
                            prodFactory = srInfo["nike_SSfcty"].ToString();
                            td = srInfo["nike_tdcode"].ToString();
                            colorVer = bomInfo["nike_bom_colorversion"].ToString();
                            modelID = srInfo["nike_modelid"].ToString();
                            lastCode = bomInfo["nike_bom_lastcode"].ToString();
                            whqDev = srInfo["nike_whqdeveloper"].ToString();
                            IPW = bomInfo["nike_bom_currentipw"].ToString();
                            productID = bomInfo["nike_productid"].ToString();
                            #endregion
                        }
                        else if (dataSource.Tables[0].TableName == "nike_bom")
                        {
                            fileType = "New";

                            DataTable headerTable = dataSource.Tables["nike_bom"];
                            DataRow headerValues = headerTable.Rows[0];

                            #region 헤더 정보 저장
                            DPA = headerValues["nike_devproj_alias"].ToString().Replace("-", "");
                            pcmBOMID = headerValues["nike_bom_id"].ToString();
                            orgSeason = headerValues["nike_season"].ToString();
                            devName = headerValues["nike_modelname"].ToString();
                            productCode = headerValues["nike_bom_productcode"].ToString().Replace("-", "");
                            prodFactory = headerValues["nike_SSfcty"].ToString().Replace("-", "");
                            td = headerValues["nike_tdcode"].ToString();
                            colorVer = headerValues["nike_bom_colorversion"].ToString();
                            modelID = headerValues["nike_modelid"].ToString();
                            lastCode = headerValues["nike_bom_lastcode"].ToString();
                            whqDev = headerValues["nike_whqdeveloper"].ToString();
                            IPW = headerValues["nike_bom_currentipw"].ToString();
                            productID = headerValues["nike_productid"].ToString();
                            #endregion
                        }

                        #region BOM Header 업로드
                        // 패키지 매개변수 입력
                        PKG_INTG_BOM.INSERT_BOM_HEAD_XML pkgInsert = new PKG_INTG_BOM.INSERT_BOM_HEAD_XML();
                        pkgInsert.ARG_FACTORY = Common.sessionFactory;
                        pkgInsert.ARG_WS_NO = createdWsNo;
                        pkgInsert.ARG_DPA = DPA;
                        pkgInsert.ARG_BOM_ID = pcmBOMID;

                        #region Season Code Converting
                        string year = orgSeason.Substring(2);
                        string seasonCode = orgSeason.Substring(0, 2);
                        string seasonNumber = string.Empty;

                        if (seasonCode == "SP")
                            seasonNumber = "01";
                        else if (seasonCode == "SU")
                            seasonNumber = "02";
                        else if (seasonCode == "FA")
                            seasonNumber = "03";
                        else if (seasonCode == "HO")
                            seasonNumber = "04";
                        // PCC_BOM_HEAD 테이블의 SEASON_CD 컬럼의 도메인에 맞는 Season Code
                        string cvtdSeasonCode = "20" + year + seasonNumber;
                        #endregion

                        pkgInsert.ARG_SEASON_CD = cvtdSeasonCode;
                        pkgInsert.ARG_DEV_NAME = devName;
                        pkgInsert.ARG_PRODUCT_CODE = productCode;
                        pkgInsert.ARG_PROD_FACTORY = prodFactory;
                        pkgInsert.ARG_TD = td;
                        pkgInsert.ARG_COLOR_VER = colorVer;
                        pkgInsert.ARG_MODEL_ID = modelID;
                        pkgInsert.ARG_LAST_CD = lastCode;
                        pkgInsert.ARG_PCC_PM = Common.sessionID;
                        pkgInsert.ARG_WHQ_DEV = whqDev;
                        pkgInsert.ARG_IPW = IPW;
                        pkgInsert.ARG_PRODUCT_ID = productID;

                        // 패키지 호출용 임시변수
                        ArrayList arrayList = new ArrayList();
                        arrayList.Add(pkgInsert);

                        // 패키지 호출 - BOM Header 데이터를 데이터베이스에 저장
                        if (Exe_Modify_PKG(arrayList) == null)
                        {
                            MessageBox.Show("Failed to uplaod BOM");
                            return;
                        }
                        #endregion

                        #region BOM Data 업로드

                        DataTable bomData = dataSource.Tables["nike_bom_lineitem"];

                        #region 매칭된 정보를 저장할 컬럼 추가
                        // Internal Part Code, Name 사용
                        bomData.Columns.Add("PART_CD");
                        bomData.Columns.Add("PART_NAME");
                        bomData.Columns.Add("PCX_MAT_ID");
                        bomData.Columns.Add("PCX_SUPP_MAT_ID");
                        bomData.Columns.Add("PCX_COLOR_ID");
                        bomData.Columns.Add("CS_CD");
                        bomData.Columns.Add("MDSL_CHK");
                        bomData.Columns.Add("OTSL_CHK");
                        #endregion

                        #region Part, Material, Color Mapping Table을 가져옴
                        DataTable listOfPartName = bomData.DefaultView.ToTable(true, "nike_bom_partname");
                        DataTable listOfMSNumber = bomData.DefaultView.ToTable(true, "nike_MxS_number");
                        DataTable listOfColorCode = bomData.DefaultView.ToTable(true, "nike_bom_colorid");

                        string chainedPartName = string.Empty;
                        string chainedMSNumber = string.Empty;
                        string chainedColorCode = string.Empty;

                        foreach (DataRow dr in listOfPartName.Rows)
                        {
                            string partName = dr["nike_bom_partname"].ToString();

                            if (partName.Length > 0)
                                chainedPartName += "," + partName;
                        }

                        foreach (DataRow dr in listOfMSNumber.Rows)
                        {
                            string msNumber = dr["nike_MxS_number"].ToString();

                            if (msNumber.Length > 0 && msNumber != "null")
                                chainedMSNumber += "," + msNumber;
                        }

                        foreach (DataRow dr in listOfColorCode.Rows)
                        {
                            string colorCode = dr["nike_bom_colorid"].ToString();

                            if (colorCode.Length > 0)
                                chainedColorCode += "," + colorCode;
                        }
                        // 패키지 매개변수 입력
                        PKG_INTG_BOM.SELECT_CODE_MPP_TB_EXCEL pkgSelectMppTb = new PKG_INTG_BOM.SELECT_CODE_MPP_TB_EXCEL();
                        pkgSelectMppTb.ARG_CHAINED_PART_NAME = chainedPartName;
                        pkgSelectMppTb.ARG_CHAINED_MSL = chainedMSNumber;
                        pkgSelectMppTb.ARG_CHAINED_COLOR_CD = chainedColorCode;
                        pkgSelectMppTb.OUT_CURSOR = string.Empty;
                        // 패키지를 호출하여 매핑 테이블을 가져옴
                        DataTable mppdTableForBom = Exe_Select_PKG(pkgSelectMppTb).Tables[0];
                        #endregion

                        int partSeq = 1;

                        // 패키지 호출용 임시변수
                        ArrayList arrayList2 = new ArrayList();

                        foreach (DataRow dr in bomData.Rows)
                        {
                            #region 파트 매칭
                            string partName = dr["nike_bom_partname"].ToString().Trim();

                            if (partName.Length > 0)
                            {
                                DataRow[] matchedCode = mppdTableForBom.Select("TYPE = 'PART' AND PCX_PART_NAME = '" + partName.Replace("'", "-") + "'");
                                // 매칭에 성공한 경우
                                if (matchedCode.Length > 0)
                                {
                                    dr["PART_CD"] = matchedCode[0]["PART_CD"];
                                    dr["PART_NAME"] = matchedCode[0]["PCC_PART_NAME"];
                                }
                                else
                                {
                                    if (fileType == "Old")
                                        dr["PART_NAME"] = partName;
                                    else if (fileType == "New")
                                    {
                                        dr["PART_NAME"] = "Failed to match a part with pcx library.";
                                    }
                                }

                            }
                            #endregion

                            #region 자재 매칭
                            string msNumber = dr["nike_MxS_number"].ToString().Trim();

                            if (msNumber.Length > 0)
                            {
                                DataRow[] matchedCode = mppdTableForBom.Select("TYPE = 'MATERIAL' AND MXSXL_NUMBER = '" + msNumber + "'");
                                // 매칭에 성공한 경우
                                if (matchedCode.Length > 0)
                                {
                                    dr["PCX_MAT_ID"] = matchedCode[0]["PCX_MAT_ID"];
                                    dr["PCX_SUPP_MAT_ID"] = matchedCode[0]["NIKE_SUPP_MTL_CODE"];
                                    dr["CS_CD"] = matchedCode[0]["CS_CD"];
                                }
                                else
                                    dr["nike_bom_materialcoment"] = "Failed to match a material with pcx library.";
                            }
                            #endregion

                            #region 컬러 매칭
                            string colorCode = dr["nike_bom_colorid"].ToString().Trim();

                            if (colorCode.Length > 0)
                            {
                                DataRow[] matchedCode = mppdTableForBom.Select("TYPE = 'COLOR' AND PDM_COLOR_CODE = '" + colorCode + "'");
                                // 매칭에 성공한 경우
                                if (matchedCode.Length > 0)
                                    dr["PCX_COLOR_ID"] = matchedCode[0]["PCX_COLOR_ID"];
                                else
                                {
                                    dr["nike_bom_colorid"] = "";
                                    dr["nike_bom_colorname"] = "";
                                    dr["nike_bom_colorcomment"] = "Failed to match a color with pcx library.";
                                }
                            }
                            #endregion

                            PKG_INTG_BOM.INSERT_BOM_TAIN_JSON pkgInsertData = new PKG_INTG_BOM.INSERT_BOM_TAIN_JSON();
                            pkgInsertData.ARG_FACTORY = Common.sessionFactory;
                            pkgInsertData.ARG_WS_NO = createdWsNo;
                            pkgInsertData.ARG_PART_SEQ = partSeq.ToString();
                            pkgInsertData.ARG_PART_NO = dr["nike_bom_partnumber"].ToString().Trim();
                            pkgInsertData.ARG_PART_CD = dr["PART_CD"].ToString();
                            pkgInsertData.ARG_PART_NAME = dr["PART_NAME"].ToString();
                            pkgInsertData.ARG_PART_TYPE = dr["nike_bom_parttype"].ToString().Trim();
                            pkgInsertData.MXSXL_NUMBER = (dr["nike_MxS_number"].ToString().ToLower() == "null") ? "" : dr["nike_MxS_number"].ToString();
                            pkgInsertData.ARG_MAT_CD = dr["nike_material_number"].ToString().Trim();
                            pkgInsertData.ARG_MAT_NAME = dr["nike_material_name"].ToString().Trim();
                            pkgInsertData.ARG_MAT_COMMENTS = dr["nike_bom_materialcoment"].ToString().Trim();
                            pkgInsertData.ARG_MCS_NUMBER = (dr["nike_mcs_number"].ToString().ToLower() == "null") ? "" : dr["nike_mcs_number"].ToString();

                            string materialComments = dr["nike_bom_materialcoment"].ToString().Trim();
                            string colorComments = dr["nike_bom_colorcomment"].ToString().Trim();
                            string nikeComments = string.Empty;

                            if (materialComments.Length > 0 && colorComments.Length == 0)
                                nikeComments = materialComments;
                            else if (materialComments.Length == 0 && colorComments.Length > 0)
                                nikeComments = colorComments;
                            else if (materialComments.Length > 0 && colorComments.Length > 0)
                                nikeComments = materialComments + ", " + colorComments;

                            pkgInsertData.ARG_NIKE_COMMENT = nikeComments;
                            pkgInsertData.ARG_COLOR_CD = dr["nike_bom_colorid"].ToString().Trim();
                            pkgInsertData.ARG_COLOR_NAME = dr["nike_bom_colorname"].ToString().Trim().ToUpper();
                            pkgInsertData.ARG_COLOR_COMMENTS = dr["nike_bom_colorcomment"].ToString().Trim();
                            pkgInsertData.ARG_SORT_NO = partSeq.ToString();
                            pkgInsertData.ARG_UPD_USER = Common.sessionID;
                            pkgInsertData.ARG_PTRN_PART_NAME = "";
                            pkgInsertData.ARG_PCX_SUPP_MAT_ID = dr["PCX_SUPP_MAT_ID"].ToString();
                            pkgInsertData.ARG_PCX_COLOR_ID = dr["PCX_COLOR_ID"].ToString();
                            pkgInsertData.ARG_VENDOR_NAME = dr["nike_supplier_name"].ToString().Trim();
                            pkgInsertData.ARG_PCX_MAT_ID = dr["PCX_MAT_ID"].ToString();
                            pkgInsertData.ARG_PTRN_PART_CD = "";
                            pkgInsertData.ARG_CS_CD = dr["CS_CD"].ToString();
                            pkgInsertData.ARG_MDSL_CHK = (dr["nike_bom_parttype"].ToString().Trim() == "MIDSOLE" || dr["nike_bom_parttype"].ToString().Trim() == "AIRBAG") ? "Y" : "N";
                            pkgInsertData.ARG_OTSL_CHK = (dr["nike_bom_parttype"].ToString().Trim() == "OUTSOLE") ? "Y" : "N";
                            pkgInsertData.ARG_CS_PTRN_NAME = dr["PART_NAME"].ToString();
                            pkgInsertData.ARG_CS_PTRN_CD = dr["PART_CD"].ToString();
                            pkgInsertData.ARG_LOGIC_GROUP = "";
                            pkgInsertData.ARG_MAT_FORECAST_PRCNT = 0;
                            pkgInsertData.ARG_COLOR_FORECAST_PRCNT = 0;

                            arrayList2.Add(pkgInsertData);
                            partSeq++;
                        }

                        if (Exe_Modify_PKG(arrayList2) == null)
                            MessageBox.Show("Failed to upload(XML", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error, MessageBoxDefaultButton.Button1);

                        #endregion

                        chainedWsNo += "," + createdWsNo;
                    }
                }

                BOMUploader headUpdater = new BOMUploader();
                headUpdater.ChainedWSNo = chainedWsNo;

                if (headUpdater.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                    BindDataSourceGridView();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
                return;
            }
        }

        /// <summary>
        ///  Excel 형태의 BOM을 데이터베이스에 업로드 함
        /// </summary>
        /// <param name="_fileNames"></param>
        private void ExcelBomUploader(string[] _fileNames)
        {
            try
            {
                // BOM을 업로드할 때 신규로 채번된 WS_NO를 (,WS_NO1,WS_NO2,WS_NO3) 형태로 연결한 것
                string chainedWsNo = string.Empty;
                // 업로드할 파일 개수만큼 반복
                foreach (string fileName in _fileNames)
                {
                    DataTable dataSource = ExtractDataFromExcelFile(fileName, "YES");

                    if (dataSource != null)
                    {
                        #region 신규 WS_NO 채번
                        PKG_INTG_BOM.SELECT_NEW_WS_NO pkgSelectWsNo = new PKG_INTG_BOM.SELECT_NEW_WS_NO();
                        pkgSelectWsNo.ARG_FACTORY = SessionInfo.Factory;
                        pkgSelectWsNo.OUT_CURSOR = string.Empty;

                        DataTable returnValue = Exe_Select_PKG(pkgSelectWsNo).Tables[0];
                        string createdWsNo = returnValue.Rows[0]["WS_NO"].ToString();
                        #endregion

                        #region BOM Header 업로드

                        PKG_INTG_BOM.INSERT_BOM_HEAD_EXCEL pkgInsertHead = new PKG_INTG_BOM.INSERT_BOM_HEAD_EXCEL();
                        pkgInsertHead.ARG_FACTORY = Common.sessionFactory;
                        pkgInsertHead.ARG_WS_NO = createdWsNo;
                        pkgInsertHead.ARG_PCC_PM = Common.sessionID;

                        ArrayList arrayList = new ArrayList();
                        arrayList.Add(pkgInsertHead);

                        Exe_Modify_PKG(arrayList);

                        #endregion

                        #region BOM Data 업로드

                        #region 매칭된 정보를 저장할 컬럼 추가
                        dataSource.Columns.Add("PART_CD");
                        dataSource.Columns.Add("PART_NAME");
                        dataSource.Columns.Add("PART_TYPE");
                        dataSource.Columns.Add("PDM_MAT_CD");
                        dataSource.Columns.Add("MAT_COMMENTS");
                        dataSource.Columns.Add("MCS_NUMBER");
                        dataSource.Columns.Add("PCX_SUPP_MAT_ID");
                        dataSource.Columns.Add("PCX_COLOR_ID");
                        dataSource.Columns.Add("COLOR_CD");
                        dataSource.Columns.Add("COLOR_NAME");
                        dataSource.Columns.Add("COLOR_COMMENTS");
                        dataSource.Columns.Add("CS_CD");
                        dataSource.Columns.Add("MDSL_CHK");
                        dataSource.Columns.Add("OTSL_CHK");
                        #endregion

                        #region Part, Material, Color Mapping Table을 가져옴
                        DataTable listOfPartName = dataSource.DefaultView.ToTable(true, "Part Name");
                        DataTable listOfMxSxL = dataSource.DefaultView.ToTable(true, "PDM Material By Supplier Number");
                        DataTable listOfColorCode = dataSource.DefaultView.ToTable(true, "Color");

                        string chainedPartName = string.Empty;
                        string chainedMxSxL = string.Empty;
                        string chainedColorCode = string.Empty;

                        foreach (DataRow dr in listOfPartName.Rows)
                        {
                            string partName = dr["Part Name"].ToString().Trim();

                            if (partName.Length > 0)
                            {
                                chainedPartName += "," + partName;
                            }
                        }

                        foreach (DataRow dr in listOfMxSxL.Rows)
                        {
                            string mxsxl = dr["PDM Material By Supplier Number"].ToString().Trim();

                            if (mxsxl.Length > 0)
                                chainedMxSxL += "," + mxsxl;
                        }

                        foreach (DataRow dr in listOfColorCode.Rows)
                        {
                            string pcxColorCode = dr["Color"].ToString().Trim();

                            if (pcxColorCode.Length > 0)
                            {
                                string[] particular = pcxColorCode.Split('-');
                                // PCX 컬러 코드에서 PDM 컬러 코드를 추출
                                string pdmColorCode = particular[0];
                                chainedColorCode += "," + pdmColorCode;
                            }
                        }
                        // 패키지 매개변수 입력
                        PKG_INTG_BOM.SELECT_CODE_MPP_TB_EXCEL pkgSelectMppTb = new PKG_INTG_BOM.SELECT_CODE_MPP_TB_EXCEL();
                        pkgSelectMppTb.ARG_CHAINED_PART_NAME = chainedPartName;
                        pkgSelectMppTb.ARG_CHAINED_MSL = chainedMxSxL;
                        pkgSelectMppTb.ARG_CHAINED_COLOR_CD = chainedColorCode;
                        pkgSelectMppTb.OUT_CURSOR = string.Empty;
                        // 패키지 호출하여 Mapping Table을 가져옴
                        DataTable mppdTableForBom = Exe_Select_PKG(pkgSelectMppTb).Tables[0];
                        #endregion

                        string partType = string.Empty;
                        int partSeq = 1;

                        // 패키지 호출용 임시변수
                        ArrayList arrayList2 = new ArrayList();

                        foreach (DataRow dr in dataSource.Rows)
                        {
                            #region 파트 매칭
                            string concatPartName = dr["Part Name (Concatenated)"].ToString().Trim();
                            string partName = dr["Part Name"].ToString().Trim();

                            // 파트타입 구분 행은 건너띔
                            if (concatPartName != "" && partName == "")
                            {
                                partType = concatPartName.ToUpper();
                                continue;
                            }
                            else if (concatPartName == "" && partName == "")
                                continue;

                            // 미드솔/아웃솔 툴링은 NIKE에서 직접 관리하므로 BOM 업로드시 제외
                            if (partType == "MIDSOLE TOOLING" || partType == "OUTSOLE TOOLING")
                                continue;

                            dr["PART_TYPE"] = partType;

                            if (partName.Length > 0)
                            {
                                DataRow[] matchedCode = mppdTableForBom.Select("TYPE = 'PART' AND PCX_PART_NAME = '" + partName.Replace("'", "-") + "'");
                                // 매칭에 성공한 경우
                                if (matchedCode.Length > 0)
                                {
                                    dr["PART_CD"] = matchedCode[0]["PART_CD"];
                                    dr["PART_NAME"] = matchedCode[0]["PCC_PART_NAME"];
                                }
                                else
                                    dr["PART_NAME"] = "Failed to match a part with pcx library.";
                            }
                            #endregion

                            #region 자재 매칭
                            string mxsxlNumber = dr["PDM Material By Supplier Number"].ToString().Trim();

                            if (mxsxlNumber.Length > 0)
                            {
                                DataRow[] matchedCode = mppdTableForBom.Select("TYPE = 'MATERIAL' AND MXSXL_NUMBER = '" + mxsxlNumber + "'");
                                // 매칭에 성공한 경우
                                if (matchedCode.Length > 0)
                                {
                                    // null은 입력 안 함
                                    if (matchedCode[0]["PDM_MAT_CD"].ToString() == "null")
                                        dr["PDM_MAT_CD"] = "";
                                    else
                                        dr["PDM_MAT_CD"] = matchedCode[0]["PDM_MAT_CD"];

                                    dr["MCS_NUMBER"] = matchedCode[0]["MCS_NUMBER"];
                                    dr["PCX_SUPP_MAT_ID"] = matchedCode[0]["NIKE_SUPP_MTL_CODE"];
                                    dr["CS_CD"] = matchedCode[0]["CS_CD"];
                                }
                                else
                                    dr["MAT_COMMENTS"] = "Failed to match a material with pcx library.";
                            }
                            #endregion

                            #region 컬러 매칭
                            string pcxColorCode = dr["Color"].ToString().Trim();

                            if (pcxColorCode.Length > 0)
                            {
                                string[] particular = pcxColorCode.Split('-');
                                string pdmColorCode = particular[0];

                                if (pdmColorCode == "MC")
                                    dr["COLOR_NAME"] = pcxColorCode;
                                else
                                {
                                    // 코드 매칭
                                    DataRow[] matchedCode = mppdTableForBom.Select("TYPE = 'COLOR' AND PDM_COLOR_CODE = '" + pdmColorCode + "'");
                                    // 매칭에 성공한 경우
                                    if (matchedCode.Length > 0)
                                    {
                                        dr["COLOR_CD"] = particular[0];
                                        dr["COLOR_NAME"] = particular[1];
                                        dr["PCX_COLOR_ID"] = matchedCode[0]["PCX_COLOR_ID"];
                                    }
                                    else
                                        dr["COLOR_COMMENTS"] = "Failed to match a color with pcx library.";
                                }
                            }
                            else
                            {
                                // 모두 공란
                            }
                            #endregion

                            PKG_INTG_BOM.INSERT_BOM_TAIN_JSON pkgInsert = new PKG_INTG_BOM.INSERT_BOM_TAIN_JSON();
                            pkgInsert.ARG_FACTORY = Common.sessionFactory;
                            pkgInsert.ARG_WS_NO = createdWsNo;
                            pkgInsert.ARG_PART_SEQ = partSeq.ToString();
                            pkgInsert.ARG_PART_NO = partSeq.ToString();
                            pkgInsert.ARG_PART_CD = dr["PART_CD"].ToString();
                            pkgInsert.ARG_PART_NAME = dr["PART_NAME"].ToString();
                            pkgInsert.ARG_PART_TYPE = dr["PART_TYPE"].ToString();
                            pkgInsert.MXSXL_NUMBER = dr["PDM Material By Supplier Number"].ToString();
                            pkgInsert.ARG_MAT_CD = dr["PDM_MAT_CD"].ToString();
                            pkgInsert.ARG_MAT_NAME = dr["PDM Material Name"].ToString();
                            pkgInsert.ARG_MAT_COMMENTS = dr["MAT_COMMENTS"].ToString();
                            pkgInsert.ARG_MCS_NUMBER = dr["MCS_NUMBER"].ToString();
                            pkgInsert.ARG_NIKE_COMMENT = dr["Comments"].ToString();
                            pkgInsert.ARG_COLOR_CD = dr["COLOR_CD"].ToString();
                            pkgInsert.ARG_COLOR_NAME = dr["COLOR_NAME"].ToString();
                            pkgInsert.ARG_COLOR_COMMENTS = dr["COLOR_COMMENTS"].ToString();
                            pkgInsert.ARG_SORT_NO = partSeq.ToString();
                            pkgInsert.ARG_UPD_USER = Common.sessionID;
                            pkgInsert.ARG_PTRN_PART_NAME = "";
                            pkgInsert.ARG_PCX_SUPP_MAT_ID = (dr["PDM Material Name"].ToString() == "PLACEHOLDER") ? "100" : dr["PCX_SUPP_MAT_ID"].ToString();
                            pkgInsert.ARG_PCX_COLOR_ID = dr["PCX_COLOR_ID"].ToString();
                            pkgInsert.ARG_VENDOR_NAME = dr["Supplier"].ToString().Trim();
                            pkgInsert.ARG_PCX_MAT_ID = dr["Material"].ToString().Trim();
                            pkgInsert.ARG_PTRN_PART_CD = "";
                            pkgInsert.ARG_CS_CD = dr["CS_CD"].ToString();
                            pkgInsert.ARG_MDSL_CHK = (dr["PART_TYPE"].ToString() == "MIDSOLE" || dr["PART_TYPE"].ToString() == "AIRBAG") ? "Y" : "N";
                            pkgInsert.ARG_OTSL_CHK = (dr["PART_TYPE"].ToString() == "OUTSOLE") ? "Y" : "N";
                            pkgInsert.ARG_CS_PTRN_NAME = dr["PART_NAME"].ToString();
                            pkgInsert.ARG_CS_PTRN_CD = dr["PART_CD"].ToString();

                            arrayList2.Add(pkgInsert);
                            partSeq++;
                        }
                        // 패키지 호출 - BOM Data 데이터베이스에 저장
                        Exe_Modify_PKG(arrayList2);
                        #endregion

                        chainedWsNo += "," + createdWsNo;
                    }
                }

                BOMUploader headUpdater = new BOMUploader();
                headUpdater.ChainedWSNo = chainedWsNo;

                if (headUpdater.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                    BindDataSourceGridView();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
                return;
            }
        }

        /// <summary>
        ///  Upload normal JSON BOM
        /// </summary>
        /// <param name="fileNames"></param>
        private void UploadJSON(string[] fileNames)
        {
            BOMHeader bomHeader = null;
            List<BOMData> bomData = null;
            DataSet ds = null;
            DataTable mtbOfSection = null;
            DataTable mtbOfBOMData = null;
            ArrayList list = new ArrayList();

            string chainedWSNo = string.Empty;

            foreach (string fileName in fileNames)
            {
                int partSeq = 1;
                string worksheetNumber = string.Empty;

                // Load data from JSON file.
                using (StreamReader file = File.OpenText(fileName))
                using (JsonTextReader reader = new JsonTextReader(file))
                {
                    JObject objs = JToken.ReadFrom(reader) as JObject;

                    if (objs.GetValue("BOM Header") == null || objs.GetValue("BOM Data") == null)
                    {
                        MessageBox.Show("Invalid JSON file.", "",
                            MessageBoxButtons.OK, MessageBoxIcon.Error, MessageBoxDefaultButton.Button1);
                        return;
                    }

                    foreach (var obj in objs)
                    {
                        if (obj.Key.Equals("BOM Header"))
                        {
                            bomHeader = JsonConvert.DeserializeObject<BOMHeader>(obj.Value.ToString());
                        }
                        else if (obj.Key.Equals("BOM Data"))
                        {
                            bomData = JsonConvert.DeserializeObject<List<BOMData>>(obj.Value.ToString());
                        }
                    }
                }

                // Generate a new worksheet number.
                PKG_INTG_BOM.SELECT_NEW_WS_NO pkgSelect = new PKG_INTG_BOM.SELECT_NEW_WS_NO();
                pkgSelect.ARG_FACTORY = Common.sessionFactory;
                pkgSelect.OUT_CURSOR = string.Empty;

                ds = Exe_Select_PKG(pkgSelect);

                if (ds.Tables[0].Rows.Count > 0)
                    worksheetNumber = ds.Tables[0].Rows[0]["WS_NO"].ToString();
                else
                {
                    MessageBox.Show("Failed to generate a new key.", "",
                        MessageBoxButtons.OK, MessageBoxIcon.Error, MessageBoxDefaultButton.Button1);
                    return;
                }

                #region Param. PCX JSON ORG. Header

                PKG_INTG_BOM.INSERT_BOM_HEAD_JSON_ORG pkgInsert = new PKG_INTG_BOM.INSERT_BOM_HEAD_JSON_ORG();
                pkgInsert.ARG_FACTORY = Common.sessionFactory;
                pkgInsert.ARG_WS_NO = worksheetNumber;
                pkgInsert.ARG_OBJ_ID = bomHeader.objectId;
                pkgInsert.ARG_OBJ_TYPE = bomHeader.objectType;
                pkgInsert.ARG_BOM_CONTRACT_VER = bomHeader.bomContractVersion;
                pkgInsert.ARG_DEV_STYLE_ID = bomHeader.developmentStyleIdentifier;
                pkgInsert.ARG_DEV_COLORWAY_ID = bomHeader.developmentColorwayIdentifier;
                pkgInsert.ARG_COLORWAY_NAME = bomHeader.colorwayName;
                pkgInsert.ARG_SRC_CONFIG_ID = bomHeader.sourcingConfigurationIdentifier;
                pkgInsert.ARG_SRC_CONFIG_NAME = bomHeader.sourcingConfigurationName;
                pkgInsert.ARG_BOM_ID = bomHeader.bomIdentifier;
                pkgInsert.ARG_BOM_NAME = bomHeader.bomName;
                pkgInsert.ARG_BOM_VERSION_NUM = bomHeader.bomVersionNumber;
                pkgInsert.ARG_BOM_DESC = bomHeader.bomDescription;
                pkgInsert.ARG_BOM_COMMENTS = bomHeader.bomComments;
                pkgInsert.ARG_BOM_STATUS_IND = bomHeader.billOfMaterialStatusIndicator;
                pkgInsert.ARG_CREATE_TIME_STAMP = bomHeader.createTimestamp;
                pkgInsert.ARG_CHANGE_TIME_STAMP = bomHeader.changeTimestamp;
                pkgInsert.ARG_CREATED_BY = bomHeader.createdBy;
                pkgInsert.ARG_MODIFIED_BY = bomHeader.modifiedBy;
                pkgInsert.ARG_STYLE_NUMBER = bomHeader.styleNumber;
                pkgInsert.ARG_STYLE_NAME = bomHeader.styleName;
                pkgInsert.ARG_MODEL_ID = bomHeader.modelIdentifier;
                pkgInsert.ARG_GENDER = bomHeader.gender;
                pkgInsert.ARG_AGE = bomHeader.age;
                pkgInsert.ARG_PRODUCT_ID = bomHeader.productId;
                pkgInsert.ARG_PRODUCT_CODE = bomHeader.productCode;
                pkgInsert.ARG_COLORWAY_CODE = bomHeader.colorwayCode;
                pkgInsert.ARG_DEV_STYLE_TYPE_ID = "";       // logic BOM.
                pkgInsert.ARG_LOGIC_BOM_STATE_ID = "";      // logic BOM.
                pkgInsert.ARG_LOGIC_BOM_GATE_ID = "";       // logic BOM.
                pkgInsert.ARG_CYCLE_YEAR = "";              // logic BOM.
                pkgInsert.ARG_BOM_PART_UUID = bomHeader.bomPartUUID == null ? "" : bomHeader.bomPartUUID;       // Gemini.
                pkgInsert.ARG_SEASON_ID = bomHeader.seasonIdentifier == null ? "" : bomHeader.seasonIdentifier; // Gemini.

                list.Add(pkgInsert);

                #endregion

                // Convert from code to name.
                switch (bomHeader.gender)
                {
                    case "01":
                        bomHeader.genderName = "Male";
                        break;

                    case "02":
                        bomHeader.genderName = "Female";
                        break;

                    case "03":
                        bomHeader.genderName = "Unisex";
                        break;

                    case "04":
                        bomHeader.genderName = "Not Applicable";
                        break;

                    default:
                        bomHeader.genderName = "";
                        break;
                }

                // Detach prod. factory only. (IndexOf is zero base.)
                bomHeader.prodFactory = bomHeader.sourcingConfigurationName.Substring(0, bomHeader.sourcingConfigurationName.IndexOf('-'));

                if (!Common.CSFactoryCodes.Contains(bomHeader.prodFactory))
                    bomHeader.prodFactory = "";

                #region Param. PCC BOM Header.

                PKG_INTG_BOM.INSERT_BOM_HEAD_JSON pkgInsert2 = new PKG_INTG_BOM.INSERT_BOM_HEAD_JSON();
                pkgInsert2.ARG_FACTORY = Common.sessionFactory;
                pkgInsert2.ARG_WS_NO = worksheetNumber;
                pkgInsert2.ARG_DEV_NAME = bomHeader.styleName.Split('-').First().Trim();
                pkgInsert2.ARG_PRODUCT_CODE = bomHeader.productCode != null ? bomHeader.productCode.Replace("-", "") : "";
                pkgInsert2.ARG_PROD_FACTORY = bomHeader.prodFactory;
                pkgInsert2.ARG_GENDER = bomHeader.genderName;
                pkgInsert2.ARG_COLOR_VER = bomHeader.colorwayName.Substring(bomHeader.colorwayName.IndexOf('-') + 1, bomHeader.colorwayName.LastIndexOf("-") - bomHeader.colorwayName.IndexOf("-") - 1);
                pkgInsert2.ARG_MODEL_ID = bomHeader.modelIdentifier;
                pkgInsert2.ARG_PCC_PM = Common.sessionID;
                pkgInsert2.ARG_DEV_STYLE_ID = bomHeader.developmentStyleIdentifier;
                pkgInsert2.ARG_DEV_COLORWAY_ID = bomHeader.developmentColorwayIdentifier;
                pkgInsert2.ARG_PRODUCT_ID = bomHeader.productId;
                pkgInsert2.ARG_STYLE_NUMBER = bomHeader.styleNumber;
                pkgInsert2.ARG_SOURCING_CONFIG_ID = bomHeader.sourcingConfigurationIdentifier;
                pkgInsert2.ARG_PCX_BOM_ID = bomHeader.bomIdentifier;
                pkgInsert2.ARG_DEV_STYLE_TYPE_ID = "";
                pkgInsert2.ARG_LOGIC_BOM_STATE_ID = "";
                pkgInsert2.ARG_LOGIC_BOM_GATE_ID = "";
                pkgInsert2.ARG_CYCLE_YEAR = "";
                pkgInsert2.ARG_LOGIC_BOM_YN = "N";
                pkgInsert2.ARG_BOM_PART_UUID = bomHeader.bomPartUUID == null ? "" : bomHeader.bomPartUUID;  // Gemini.
                pkgInsert2.ARG_SEASON_ID = ConvertSeason(bomHeader.seasonIdentifier);

                list.Add(pkgInsert2);

                #endregion

                #region Param. PCX JSON Org. Data

                foreach (BOMData data in bomData)
                {
                    PKG_INTG_BOM.INSERT_BOM_TAIN_JSON_ORG pkgInsert3 = new PKG_INTG_BOM.INSERT_BOM_TAIN_JSON_ORG();
                    pkgInsert3.ARG_FACTORY = Common.sessionFactory;
                    pkgInsert3.ARG_WS_NO = worksheetNumber;
                    pkgInsert3.ARG_BOM_LINE_SORT_SEQ = data.bomLineSortSequence;
                    pkgInsert3.ARG_BOM_SECTION_ID = data.billOfMaterialsSectionIdentifier;
                    pkgInsert3.ARG_PART_NAME_ID = data.partNameIdentifier;
                    pkgInsert3.ARG_PTRN_PART_ID = data.patternPartIdentifier;
                    pkgInsert3.ARG_SUPP_MAT_ID = data.suppliedMaterialIdentifier;
                    pkgInsert3.ARG_MAT_ITEM_ID = data.materialItemIdentifier;
                    pkgInsert3.ARG_MAT_ITEM_PLHDR_DESC = data.materialItemPlaceholderDescription;
                    pkgInsert3.ARG_COLOR_ID = data.colorIdentifier;
                    pkgInsert3.ARG_COLOR_PLHDR_DESC = data.colorPlaceholderDescription;
                    pkgInsert3.ARG_SUPP_MAT_COLOR_IS_MUL = data.suppliedMaterialColorIsMultipleColors;
                    pkgInsert3.ARG_SUPP_MAT_COLOR_ID = data.suppliedMaterialColorIdentifier;
                    pkgInsert3.ARG_BOM_LINEITEM_COMMENTS = data.bomLineItemComments;
                    pkgInsert3.ARG_FAC_IN_HOUSE_IND = data.factoryInHouseIndicator;
                    pkgInsert3.ARG_COUNTY_ORG_ID = data.countyOfOriginIdentifier;
                    pkgInsert3.ARG_ACTUAL_DIMENSION_DESC = data.actualDimensionDescription;
                    pkgInsert3.ARG_ISO_MEASUREMENT_CODE = data.isoMeasurementCode;
                    pkgInsert3.ARG_NET_USAGE_NUMBER = data.netUsageNumber;
                    pkgInsert3.ARG_WASTE_USAGE_NUMBER = data.wasteUsageNumber;
                    pkgInsert3.ARG_PART_YIELD = data.partYield;
                    pkgInsert3.ARG_CONSUM_CONVER_RATE = data.consumptionConversionRate;
                    pkgInsert3.ARG_LINEITEM_DEFECT_PER_NUM = data.lineItemDefectPercentNumber;
                    pkgInsert3.ARG_UNIT_PRICE_ISO_MSR_CODE = data.unitPriceISOMeasurementCode;
                    pkgInsert3.ARG_CURRENCY_CODE = data.currencyCode;
                    pkgInsert3.ARG_FACTORY_UNIT_PRICE = data.factoryUnitPrice;
                    pkgInsert3.ARG_UNIT_PRICE_UPCHARGE_NUM = data.unitPriceUpchargeNumber;
                    pkgInsert3.ARG_UNIT_PRICE_UPCHARGE_DESC = data.unitPriceUpchargeDescription;
                    pkgInsert3.ARG_FREIGHT_TERM_ID = data.freightTermIdentifier;
                    pkgInsert3.ARG_LANDED_COST_PER_NUM = data.landedCostPercentNumber;
                    pkgInsert3.ARG_PCC_SORT_ORDER = data.pccSortOrder;
                    pkgInsert3.ARG_ROLLUP_VARIATION_LV = data.rollupVariationLevel == null ? "" : data.rollupVariationLevel;    // will be deleted after Gemini go live.
                    pkgInsert3.ARG_PART_SEQ = partSeq.ToString();
                    pkgInsert3.ARG_LOGIC_GROUP = "";
                    pkgInsert3.ARG_MAT_FORECAST_PRCNT = 0;
                    pkgInsert3.ARG_COLOR_FORECAST_PRCNT = 0;

                    list.Add(pkgInsert3);
                    partSeq++;
                }

                #endregion

                // Delegate which chains each value lie value1,value2,value3.
                Func<List<BOMData>, string, string> func = (source, type) =>
                {
                    IOrderedEnumerable<IGrouping<string, BOMData>> sequence;
                    // Avoid closure.
                    string chainedValues = string.Empty;

                    switch (type)
                    {
                        case "partID":  // do together at the same time.
                            sequence = source.GroupBy(x => x.partNameIdentifier).Concat(source.GroupBy(y => y.patternPartIdentifier)).OrderBy(z => z.Key);
                            break;

                        case "suppMatID":
                            sequence = source.GroupBy(x => x.suppliedMaterialIdentifier).OrderBy(x => x.Key);
                            break;

                        case "colorID":
                            sequence = source.GroupBy(x => x.colorIdentifier).OrderBy(x => x.Key);
                            break;

                        default:
                            sequence = null;
                            break;
                    }

                    if (sequence != null)
                    {
                        foreach (var o in sequence)
                        {
                            if (chainedValues == "")
                                chainedValues = o.Key.ToString();
                            else
                                chainedValues += string.Format(",{0}", o.Key.ToString());
                        }
                    }

                    return chainedValues;
                };

                // Get map table for section id.
                PKG_INTG_BOM.SELECT_COMMON_CODE_MPP_TB pkgSelect2 = new PKG_INTG_BOM.SELECT_COMMON_CODE_MPP_TB();
                pkgSelect2.ARG_COMMON_ID = "PCX001";
                pkgSelect2.OUT_CURSOR = string.Empty;

                ds = Exe_Select_PKG(pkgSelect2);

                if (ds.Tables[0].Rows.Count > 0)
                    mtbOfSection = ds.Tables[0];
                else
                {
                    Common.ShowMessageBox("Failed to load common map table.", "E");
                    return;
                }

                // Get map table for BOM data.
                PKG_INTG_BOM.SELECT_BOM_CODE_MPP_TB pkgSelect3 = new PKG_INTG_BOM.SELECT_BOM_CODE_MPP_TB();
                pkgSelect3.ARG_CONCAT_PART_ID = func(bomData, "partID");
                pkgSelect3.ARG_CONCAT_SUPP_MAT_ID = func(bomData, "suppMatID");
                pkgSelect3.ARG_CONCAT_COLOR_ID = func(bomData, "colorID");
                pkgSelect3.OUT_CURSOR = string.Empty;

                ds = Exe_Select_PKG(pkgSelect3);

                if (ds.Tables[0].Rows.Count > 0)
                    mtbOfBOMData = ds.Tables[0];
                else
                {
                    Common.ShowMessageBox("Failed to load BOM map table.", "E");
                    return;
                }

                #region Param. PCC CS BOM TAIL.

                // Reset the sequence.
                partSeq = 1;

                foreach (BOMData lineItem in bomData)
                {
                    DataRow row = null;

                    #region Match section.

                    mtbOfSection.PrimaryKey = new DataColumn[] { mtbOfSection.Columns["NIKE_ID"] };

                    row = mtbOfSection.Rows.Find(lineItem.billOfMaterialsSectionIdentifier);

                    if (row != null)
                        lineItem.partType = row["LIST_VALUES"].ToString().ToUpper();
                    else
                        lineItem.partType = "";

                    if (lineItem.partType == "MIDSOLE TOOLING" || lineItem.partType == "OUTSOLE TOOLING")
                        continue;

                    #endregion

                    #region Match Part & Pattern Part

                    try
                    {
                        row = mtbOfBOMData.Select(string.Format("TYPE = 'PART' AND PCX_PART_ID = '{0}'", lineItem.partNameIdentifier)).First();
                        lineItem.partName = row["PART_NAME"].ToString();
                        lineItem.partCode = row["PART_CD"].ToString();
                    }
                    catch
                    {
                        row = null;
                        lineItem.partName = "Failed to load part info. from MDM.";
                        lineItem.partCode = "";
                    }

                    if (lineItem.patternPartIdentifier == "")
                    {
                        if (row != null)
                        {
                            // If Pattern Part is null then CS Part follows the Nike Part.
                            lineItem.csPtrnName = lineItem.partName;
                            lineItem.csPtrnCode = lineItem.partCode;
                        }
                        else
                        {
                            lineItem.csPtrnName = "";
                            lineItem.csPtrnCode = "";
                        }

                        lineItem.ptrnPartName = "";
                        lineItem.ptrnPartCode = "";
                    }
                    else
                    {
                        try
                        {
                            row = mtbOfBOMData.Select(string.Format("TYPE = 'PART' AND PCX_PART_ID = '{0}'", lineItem.patternPartIdentifier)).First();

                            lineItem.ptrnPartName = row["PART_NAME"].ToString();
                            lineItem.ptrnPartCode = row["PART_CD"].ToString();

                            lineItem.csPtrnName = lineItem.ptrnPartName;
                            lineItem.csPtrnCode = lineItem.ptrnPartCode;
                        }
                        catch
                        {
                            lineItem.ptrnPartName = "Failed to load ptrn. part info. from MDM.";
                            lineItem.ptrnPartCode = "";
                            lineItem.csPtrnName = "";
                            lineItem.csPtrnCode = "";
                        }
                    }

                    #endregion

                    #region Match material.

                    try
                    {
                        row = mtbOfBOMData.Select(string.Format("TYPE = 'MATERIAL' AND NIKE_SUPP_MTL_CODE = '{0}'", lineItem.suppliedMaterialIdentifier)).First();

                        // Data of some attributes in json is 'null', convert it to null manually.
                        lineItem.pdmMaterialCode = row["PDM_MAT_CD"].ToString() == "null" ? "" : row["PDM_MAT_CD"].ToString();
                        lineItem.mxsxlNumber = row["MXSXL_NUMBER"].ToString() == "null" ? "" : row["MXSXL_NUMBER"].ToString();
                        lineItem.pdmMaterialName = row["PDM_MAT_NAME"].ToString();
                        lineItem.mcsNumber = row["MCS_NUMBER"].ToString();
                        lineItem.vendorName = row["VENDOR_NAME"].ToString();
                        lineItem.csCode = "";
                        lineItem.materialComments = "";
                    }
                    catch
                    {
                        lineItem.pdmMaterialCode = "";
                        lineItem.mxsxlNumber = "";
                        lineItem.pdmMaterialName = "";
                        lineItem.mcsNumber = "";
                        lineItem.vendorName = "";
                        lineItem.csCode = "";
                        lineItem.materialComments = lineItem.suppliedMaterialIdentifier == "0" ? "" : "Failed to load material info. from MDM.";
                    }

                    #endregion

                    #region Match color.

                    try
                    {
                        row = mtbOfBOMData.Select(string.Format("TYPE = 'COLOR' AND PCX_COLOR_ID = '{0}'", lineItem.colorIdentifier)).First();

                        string[] colorValues = row["PCX_COLOR_NAME"].ToString().Split('-');

                        if (colorValues[0] == "MC")
                        {
                            lineItem.colorName = row["PCX_COLOR_NAME"].ToString();
                            lineItem.colorCode = row["LGCY_COLOR_CD"].ToString();
                        }
                        else
                        {
                            lineItem.colorName = colorValues[1];
                            lineItem.colorCode = colorValues[0];
                        }

                        lineItem.colorComments = "";
                    }
                    catch
                    {
                        lineItem.colorName = "";
                        lineItem.colorCode = "";
                        lineItem.colorComments = lineItem.colorIdentifier == "" ? "" : "Failed to load color info from PCX Lib.";
                    }

                    #endregion

                    PKG_INTG_BOM.INSERT_BOM_TAIN_JSON pkgInsert4 = new PKG_INTG_BOM.INSERT_BOM_TAIN_JSON();
                    pkgInsert4.ARG_FACTORY = Common.sessionFactory;
                    pkgInsert4.ARG_WS_NO = worksheetNumber;
                    pkgInsert4.ARG_PART_SEQ = partSeq.ToString();
                    pkgInsert4.ARG_PART_NO = lineItem.bomLineSortSequence;
                    pkgInsert4.ARG_PART_CD = lineItem.partCode;
                    pkgInsert4.ARG_PART_NAME = lineItem.partName;
                    pkgInsert4.ARG_PART_TYPE = lineItem.partType;
                    pkgInsert4.MXSXL_NUMBER = lineItem.mxsxlNumber;
                    pkgInsert4.ARG_MAT_CD = lineItem.pdmMaterialCode;
                    pkgInsert4.ARG_MAT_NAME = lineItem.suppliedMaterialIdentifier == "100" ? "PLACEHOLDER" : lineItem.pdmMaterialName;
                    pkgInsert4.ARG_MAT_COMMENTS = lineItem.bomLineItemComments == "" ? lineItem.materialComments : lineItem.bomLineItemComments;
                    pkgInsert4.ARG_MCS_NUMBER = lineItem.mcsNumber;
                    pkgInsert4.ARG_NIKE_COMMENT = lineItem.bomLineItemComments;
                    pkgInsert4.ARG_COLOR_CD = lineItem.colorCode;
                    pkgInsert4.ARG_COLOR_NAME = lineItem.colorName;
                    pkgInsert4.ARG_COLOR_COMMENTS = lineItem.colorComments;
                    pkgInsert4.ARG_SORT_NO = partSeq.ToString();
                    pkgInsert4.ARG_UPD_USER = Common.sessionID;
                    pkgInsert4.ARG_PTRN_PART_NAME = lineItem.ptrnPartName;
                    pkgInsert4.ARG_PCX_SUPP_MAT_ID = lineItem.suppliedMaterialIdentifier;
                    pkgInsert4.ARG_PCX_COLOR_ID = lineItem.colorIdentifier;
                    pkgInsert4.ARG_VENDOR_NAME = lineItem.vendorName;
                    pkgInsert4.ARG_PCX_MAT_ID = lineItem.materialItemIdentifier;
                    pkgInsert4.ARG_PTRN_PART_CD = lineItem.ptrnPartCode;
                    pkgInsert4.ARG_CS_CD = lineItem.csCode;
                    pkgInsert4.ARG_MDSL_CHK = (lineItem.partType == "MIDSOLE" || lineItem.partType == "AIRBAG") ? "Y" : "N";
                    pkgInsert4.ARG_OTSL_CHK = lineItem.partType == "OUTSOLE" ? "Y" : "N";
                    pkgInsert4.ARG_CS_PTRN_NAME = lineItem.csPtrnName;
                    pkgInsert4.ARG_CS_PTRN_CD = lineItem.csPtrnCode;
                    pkgInsert4.ARG_LOGIC_GROUP = "";
                    pkgInsert4.ARG_MAT_FORECAST_PRCNT = 0;
                    pkgInsert4.ARG_COLOR_FORECAST_PRCNT = 0;

                    list.Add(pkgInsert4);
                    partSeq++;
                }

                #endregion

                if (chainedWSNo == "")
                    chainedWSNo = worksheetNumber;
                else
                    chainedWSNo += "," + worksheetNumber;
            }

            // Call procedures.
            if (Exe_Modify_PKG(list) == null)
            {
                Common.ShowMessageBox("Failed to upload json BOM.", "E");
                return;
            }

            BOMUploader uploader = new BOMUploader();
            uploader.ChainedWSNo = chainedWSNo;

            if (uploader.ShowDialog() == DialogResult.OK)
                BindDataSourceGridView();

            #region backup

            //try
            //{
            //    // Chained Key List like ,WS_NO_1,WS_NO_2,WS_NO_3,...
            //    string chainedWsNo = string.Empty;

            //    foreach (string fileName in fileNames)
            //    {
            //        using (StreamReader fileStream = File.OpenText(fileName))
            //        {
            //            using (JsonTextReader jsonReader = new JsonTextReader(fileStream))
            //            {
            //                JObject jObjects = (JObject)JToken.ReadFrom(jsonReader);

            //                DataSet cnvrtObjects = ConvertJson2Xml(jObjects.ToString());
            //                DataTable BOMHeader = cnvrtObjects.Tables[0];
            //                DataTable BOMData = cnvrtObjects.Tables[1];

            //                // Create a new Worksheet Number.
            //                PKG_INTG_BOM.SELECT_NEW_WS_NO pkgSelectWsNo = new PKG_INTG_BOM.SELECT_NEW_WS_NO();
            //                pkgSelectWsNo.ARG_FACTORY = SessionInfo.Factory;
            //                pkgSelectWsNo.OUT_CURSOR = string.Empty;

            //                DataTable dataSource = Exe_Select_PKG(pkgSelectWsNo).Tables[0];
            //                if (dataSource.Rows.Count == 0)
            //                {
            //                    MessageBox.Show("Failed to create a new WS_NO");
            //                    return;
            //                }

            //                string newWsNo = dataSource.Rows[0]["WS_NO"].ToString();

            //                DataRow headerValues = BOMHeader.Rows[0];

            //                #region Upload BOM header for backup.

            //                PKG_INTG_BOM.INSERT_BOM_HEAD_JSON_ORG pkgInsertOrgHead = new PKG_INTG_BOM.INSERT_BOM_HEAD_JSON_ORG();
            //                pkgInsertOrgHead.ARG_FACTORY = SessionInfo.Factory;
            //                pkgInsertOrgHead.ARG_WS_NO = newWsNo;
            //                pkgInsertOrgHead.ARG_OBJ_ID = headerValues["objectId"].ToString();
            //                pkgInsertOrgHead.ARG_OBJ_TYPE = headerValues["objectType"].ToString();
            //                pkgInsertOrgHead.ARG_BOM_CONTRACT_VER = headerValues["bomContractVersion"].ToString();
            //                pkgInsertOrgHead.ARG_DEV_STYLE_ID = headerValues["developmentStyleIdentifier"].ToString();
            //                pkgInsertOrgHead.ARG_DEV_COLORWAY_ID = headerValues["developmentColorwayIdentifier"].ToString();
            //                pkgInsertOrgHead.ARG_COLORWAY_NAME = headerValues["colorwayName"].ToString();
            //                pkgInsertOrgHead.ARG_SRC_CONFIG_ID = headerValues["sourcingConfigurationIdentifier"].ToString();
            //                pkgInsertOrgHead.ARG_SRC_CONFIG_NAME = headerValues["sourcingConfigurationName"].ToString();
            //                pkgInsertOrgHead.ARG_BOM_ID = headerValues["bomIdentifier"].ToString();
            //                pkgInsertOrgHead.ARG_BOM_NAME = headerValues["bomName"].ToString();
            //                pkgInsertOrgHead.ARG_BOM_VERSION_NUM = headerValues["bomVersionNumber"].ToString();
            //                pkgInsertOrgHead.ARG_BOM_DESC = headerValues["bomDescription"].ToString();
            //                pkgInsertOrgHead.ARG_BOM_COMMENTS = headerValues["bomComments"].ToString();
            //                pkgInsertOrgHead.ARG_BOM_STATUS_IND = headerValues["billOfMaterialStatusIndicator"].ToString();
            //                pkgInsertOrgHead.ARG_CREATE_TIME_STAMP = headerValues["createTimestamp"].ToString();
            //                pkgInsertOrgHead.ARG_CHANGE_TIME_STAMP = headerValues["changeTimestamp"].ToString();
            //                pkgInsertOrgHead.ARG_CREATED_BY = headerValues["createdBy"].ToString();
            //                pkgInsertOrgHead.ARG_MODIFIED_BY = headerValues["modifiedBy"].ToString();
            //                pkgInsertOrgHead.ARG_STYLE_NUMBER = headerValues["styleNumber"].ToString();
            //                pkgInsertOrgHead.ARG_STYLE_NAME = headerValues["styleName"].ToString();
            //                pkgInsertOrgHead.ARG_MODEL_ID = headerValues["modelIdentifier"].ToString();
            //                pkgInsertOrgHead.ARG_GENDER = headerValues["gender"].ToString();
            //                pkgInsertOrgHead.ARG_AGE = headerValues["age"].ToString();
            //                pkgInsertOrgHead.ARG_PRODUCT_ID = headerValues["productId"].ToString();
            //                pkgInsertOrgHead.ARG_PRODUCT_CODE = headerValues["productCode"].ToString();
            //                pkgInsertOrgHead.ARG_COLORWAY_CODE = headerValues["colorwayCode"].ToString();
            //                pkgInsertOrgHead.ARG_DEV_STYLE_TYPE_ID = "";
            //                pkgInsertOrgHead.ARG_LOGIC_BOM_STATE_ID = "";
            //                pkgInsertOrgHead.ARG_LOGIC_BOM_GATE_ID = "";
            //                pkgInsertOrgHead.ARG_CYCLE_YEAR = "";

            //                ArrayList listHeader = new ArrayList();

            //                listHeader.Add(pkgInsertOrgHead);

            //                if (Exe_Modify_PKG(listHeader) == null)
            //                {
            //                    MessageBox.Show("Failed to save JSON Org. Header Info.");
            //                    return;
            //                }

            //                #endregion

            //                #region Upload BOM header for live.

            //                #region Style Name

            //                /* Dev Style Name : Style Name + '-' + Style Number */

            //                string[] devStyleName = new string[2] { "", "" };

            //                if (headerValues["styleName"].ToString().Contains('-'))
            //                    devStyleName = headerValues["styleName"].ToString().Split('-');

            //                string styleName = devStyleName[0].Trim();

            //                #endregion

            //                #region Color Version

            //                /* Colorway Name : Dev Colorway ID + '-' + Dev Colorway Description + '--' + Initial Season */

            //                // Zero-based
            //                int idxDelimiter1 = headerValues["colorwayName"].ToString().IndexOf('-');
            //                int idxDelimiter2 = headerValues["colorwayName"].ToString().LastIndexOf('-');

            //                // Colorway Desc. 길이
            //                int length = idxDelimiter2 - idxDelimiter1 - 1;

            //                // Colorway Desc.
            //                string devColorDescription = string.Empty;

            //                if (length < 3)
            //                    devColorDescription = "";
            //                else
            //                    devColorDescription = headerValues["colorwayName"].ToString().Substring(idxDelimiter1 + 1, length);

            //                #endregion

            //                #region Gender

            //                string gender = "";

            //                if (headerValues["gender"].ToString() == "01")
            //                    gender = "Male";
            //                else if (headerValues["gender"].ToString() == "02")
            //                    gender = "Female";
            //                else if (headerValues["gender"].ToString() == "03")
            //                    gender = "Unisex";
            //                else if (headerValues["gender"].ToString() == "04")
            //                    gender = "Not Applicable";

            //                #endregion

            //                #region Product Code

            //                string productCode = "";

            //                if (headerValues["productCode"].ToString() != "")
            //                    productCode = headerValues["styleNumber"].ToString() + headerValues["colorwayCode"].ToString();

            //                #endregion

            //                #region Sourcing Factory

            //                string[] prodFactoryOfCS = new string[4] { "VJ", "JJ", "QD", "RJ" };
            //                string prodFactory = "";

            //                if (headerValues["sourcingConfigurationName"].ToString() != "")
            //                {
            //                    prodFactory = headerValues["sourcingConfigurationName"].ToString().Substring(0, 2);

            //                    if (prodFactoryOfCS.Contains(prodFactory) == false)
            //                        prodFactory = "";
            //                }

            //                #endregion

            //                PKG_INTG_BOM.INSERT_BOM_HEAD_JSON pkgInsertHead = new PKG_INTG_BOM.INSERT_BOM_HEAD_JSON();
            //                pkgInsertHead.ARG_FACTORY = SessionInfo.Factory;
            //                pkgInsertHead.ARG_WS_NO = newWsNo;
            //                pkgInsertHead.ARG_DEV_NAME = styleName;
            //                pkgInsertHead.ARG_PRODUCT_CODE = productCode;
            //                pkgInsertHead.ARG_PROD_FACTORY = prodFactory;
            //                pkgInsertHead.ARG_GENDER = (gender == "") ? "" : gender;
            //                pkgInsertHead.ARG_COLOR_VER = devColorDescription;
            //                pkgInsertHead.ARG_MODEL_ID = headerValues["modelIdentifier"].ToString();
            //                pkgInsertHead.ARG_PCC_PM = SessionInfo.UserID;
            //                pkgInsertHead.ARG_DEV_STYLE_ID = headerValues["developmentStyleIdentifier"].ToString();
            //                pkgInsertHead.ARG_DEV_COLORWAY_ID = headerValues["developmentColorwayIdentifier"].ToString();
            //                pkgInsertHead.ARG_PRODUCT_ID = headerValues["productId"].ToString();
            //                pkgInsertHead.ARG_STYLE_NUMBER = headerValues["styleNumber"].ToString();
            //                pkgInsertHead.ARG_SOURCING_CONFIG_ID = headerValues["sourcingConfigurationIdentifier"].ToString();
            //                pkgInsertHead.ARG_PCX_BOM_ID = headerValues["bomIdentifier"].ToString();
            //                pkgInsertHead.ARG_DEV_STYLE_TYPE_ID = "";
            //                pkgInsertHead.ARG_LOGIC_BOM_STATE_ID = "";
            //                pkgInsertHead.ARG_LOGIC_BOM_GATE_ID = "";
            //                pkgInsertHead.ARG_CYCLE_YEAR = "";
            //                pkgInsertHead.ARG_LOGIC_BOM_YN = "N";

            //                ArrayList arrayList = new ArrayList();

            //                arrayList.Add(pkgInsertHead);

            //                if (Exe_Modify_PKG(arrayList) == null)
            //                {
            //                    MessageBox.Show("Failed to save BOM Header");
            //                    return;
            //                }

            //                #endregion

            //                #region Upload BOM data for backup.

            //                ArrayList listBOMData = new ArrayList();
            //                int partSeqOrg = 1;

            //                foreach (DataRow lineItem in BOMData.Rows)
            //                {
            //                    PKG_INTG_BOM.INSERT_BOM_TAIN_JSON_ORG pkgInsertOrgData = new PKG_INTG_BOM.INSERT_BOM_TAIN_JSON_ORG();
            //                    pkgInsertOrgData.ARG_FACTORY = SessionInfo.Factory;
            //                    pkgInsertOrgData.ARG_WS_NO = newWsNo;
            //                    pkgInsertOrgData.ARG_BOM_LINE_SORT_SEQ = lineItem["bomLineSortSequence"].ToString();
            //                    pkgInsertOrgData.ARG_BOM_SECTION_ID = lineItem["billOfMaterialsSectionIdentifier"].ToString();
            //                    pkgInsertOrgData.ARG_PART_NAME_ID = lineItem["partNameIdentifier"].ToString();
            //                    pkgInsertOrgData.ARG_PTRN_PART_ID = lineItem["patternPartIdentifier"].ToString();
            //                    pkgInsertOrgData.ARG_SUPP_MAT_ID = lineItem["suppliedMaterialIdentifier"].ToString();
            //                    pkgInsertOrgData.ARG_MAT_ITEM_ID = lineItem["materialItemIdentifier"].ToString();
            //                    pkgInsertOrgData.ARG_MAT_ITEM_PLHDR_DESC = lineItem["materialItemPlaceholderDescription"].ToString();
            //                    pkgInsertOrgData.ARG_COLOR_ID = lineItem["colorIdentifier"].ToString();
            //                    pkgInsertOrgData.ARG_COLOR_PLHDR_DESC = lineItem["colorPlaceholderDescription"].ToString();
            //                    pkgInsertOrgData.ARG_SUPP_MAT_COLOR_IS_MUL = lineItem["suppliedMaterialColorIsMultipleColors"].ToString();
            //                    pkgInsertOrgData.ARG_SUPP_MAT_COLOR_ID = lineItem["suppliedMaterialColorIdentifier"].ToString();
            //                    pkgInsertOrgData.ARG_BOM_LINEITEM_COMMENTS = lineItem["bomLineItemComments"].ToString();
            //                    pkgInsertOrgData.ARG_FAC_IN_HOUSE_IND = lineItem["factoryInHouseIndicator"].ToString();
            //                    pkgInsertOrgData.ARG_COUNTY_ORG_ID = lineItem["countyOfOriginIdentifier"].ToString();
            //                    pkgInsertOrgData.ARG_ACTUAL_DIMENSION_DESC = lineItem["actualDimensionDescription"].ToString();
            //                    pkgInsertOrgData.ARG_ISO_MEASUREMENT_CODE = lineItem["isoMeasurementCode"].ToString();
            //                    pkgInsertOrgData.ARG_NET_USAGE_NUMBER = lineItem["netUsageNumber"].ToString();
            //                    pkgInsertOrgData.ARG_WASTE_USAGE_NUMBER = lineItem["wasteUsageNumber"].ToString();
            //                    pkgInsertOrgData.ARG_PART_YIELD = lineItem["partYield"].ToString();
            //                    pkgInsertOrgData.ARG_CONSUM_CONVER_RATE = lineItem["consumptionConversionRate"].ToString();
            //                    pkgInsertOrgData.ARG_LINEITEM_DEFECT_PER_NUM = lineItem["lineItemDefectPercentNumber"].ToString();
            //                    pkgInsertOrgData.ARG_UNIT_PRICE_ISO_MSR_CODE = lineItem["unitPriceISOMeasurementCode"].ToString();
            //                    pkgInsertOrgData.ARG_CURRENCY_CODE = lineItem["currencyCode"].ToString();
            //                    pkgInsertOrgData.ARG_FACTORY_UNIT_PRICE = lineItem["factoryUnitPrice"].ToString();
            //                    pkgInsertOrgData.ARG_UNIT_PRICE_UPCHARGE_NUM = lineItem["unitPriceUpchargeNumber"].ToString();
            //                    pkgInsertOrgData.ARG_UNIT_PRICE_UPCHARGE_DESC = lineItem["unitPriceUpchargeDescription"].ToString();
            //                    pkgInsertOrgData.ARG_FREIGHT_TERM_ID = lineItem["freightTermIdentifier"].ToString();
            //                    pkgInsertOrgData.ARG_LANDED_COST_PER_NUM = lineItem["landedCostPercentNumber"].ToString();
            //                    pkgInsertOrgData.ARG_PCC_SORT_ORDER = lineItem["pccSortOrder"].ToString();
            //                    pkgInsertOrgData.ARG_ROLLUP_VARIATION_LV = lineItem["rollupVariationLevel"].ToString();
            //                    pkgInsertOrgData.ARG_PART_SEQ = Convert.ToString(partSeqOrg);
            //                    pkgInsertOrgData.ARG_LOGIC_GROUP = "";
            //                    pkgInsertOrgData.ARG_MAT_FORECAST_PRCNT = 0;
            //                    pkgInsertOrgData.ARG_COLOR_FORECAST_PRCNT = 0;

            //                    listBOMData.Add(pkgInsertOrgData);
            //                    partSeqOrg++;
            //                }

            //                if (Exe_Modify_PKG(listBOMData) == null)
            //                {
            //                    MessageBox.Show("Failed to save JSON Org. Data");
            //                    return;
            //                }

            //                #endregion

            //                #region Upload BOM data for live.

            //                // 매칭된 코드를 저장할 신규 테이블 생성
            //                DataTable mapTable = new DataTable();

            //                // JSON BOM 원본의 스키마를 그대로 복사
            //                mapTable = BOMData.Clone();

            //                #region 매칭된 코드를 저장할 신규 컬럼 추가

            //                mapTable.Columns.Add("TYPE");
            //                mapTable.Columns.Add("PART_CD");
            //                mapTable.Columns.Add("PART_NAME");
            //                mapTable.Columns.Add("PTRN_PART_CD");
            //                mapTable.Columns.Add("PTRN_PART_NAME");
            //                mapTable.Columns.Add("CS_PTRN_CD");
            //                mapTable.Columns.Add("CS_PTRN_NAME");

            //                mapTable.Columns.Add("PDM_MAT_CD");
            //                mapTable.Columns.Add("PDM_MAT_NAME");
            //                mapTable.Columns.Add("MAT_COMMENTS");
            //                mapTable.Columns.Add("MXSXL_NUMBER");
            //                mapTable.Columns.Add("MCS_NUMBER");

            //                mapTable.Columns.Add("COLOR_CD");
            //                mapTable.Columns.Add("COLOR_NAME");
            //                mapTable.Columns.Add("COLOR_COMMENTS");

            //                mapTable.Columns.Add("VENDOR_NAME");
            //                mapTable.Columns.Add("CS_CD");
            //                mapTable.Columns.Add("MDSL_CHK");
            //                mapTable.Columns.Add("OTSL_CHK");

            //                #endregion

            //                #region Type 매칭 및 라인 아이템 원본 복사

            //                #region Nike Material Section ID Mapping Table을 가져옴

            //                PKG_INTG_BOM.SELECT_COMMON_CODE_MPP_TB pkgSelectCommon = new PKG_INTG_BOM.SELECT_COMMON_CODE_MPP_TB();
            //                pkgSelectCommon.ARG_COMMON_ID = "PCX001";
            //                pkgSelectCommon.OUT_CURSOR = string.Empty;

            //                DataTable mapTableForColumn = Exe_Select_PKG(pkgSelectCommon).Tables[0];

            //                #endregion

            //                foreach (DataRow dr in BOMData.Rows)
            //                {
            //                    // 원본 행을 하나씩 복사
            //                    mapTable.ImportRow(dr);

            //                    // 매칭할 섹션 아이디
            //                    string sectionId = dr["billOfMaterialsSectionIdentifier"].ToString();

            //                    // 매칭된 코드명
            //                    DataRow[] matchedCode = mapTableForColumn.Select("NIKE_ID = '" + sectionId + "'");

            //                    // 테이블의 행 번호는 0부터 시작
            //                    int rIdx = mapTable.Rows.Count - 1;

            //                    // TYPE 컬럼에 매칭된 코드를 대문자로 변환하여 입력
            //                    mapTable.Rows[rIdx]["TYPE"] = matchedCode[0]["LIST_VALUES"].ToString().ToUpper();
            //                }

            //                #endregion

            //                #region Part, Material, Color Mapping Table을 가져옴

            //                // BOM의 파트,자재,컬러ID 리스트
            //                DataTable listOfPartID = mapTable.DefaultView.ToTable(true, "partNameIdentifier");
            //                DataTable listOfPtrnPartID = mapTable.DefaultView.ToTable(true, "patternPartIdentifier");
            //                DataTable listOfSuppMatID = mapTable.DefaultView.ToTable(true, "suppliedMaterialIdentifier");
            //                DataTable listOfColorID = mapTable.DefaultView.ToTable(true, "colorIdentifier");

            //                // (코드,코드,코드) 포맷으로 문자열 연결
            //                string chainedPartID = string.Empty;
            //                string chainedSuppMatID = string.Empty;
            //                string chainedColorID = string.Empty;

            //                foreach (DataRow dr in listOfPartID.Rows)
            //                {
            //                    chainedPartID += "," + dr["partNameIdentifier"].ToString();
            //                }

            //                // 패턴 파트는 파트 라이브러리와 동일하게 사용
            //                foreach (DataRow dr in listOfPtrnPartID.Rows)
            //                {
            //                    chainedPartID += "," + dr["patternPartIdentifier"].ToString();
            //                }

            //                foreach (DataRow dr in listOfSuppMatID.Rows)
            //                {
            //                    chainedSuppMatID += "," + dr["suppliedMaterialIdentifier"].ToString();
            //                }

            //                foreach (DataRow dr in listOfColorID.Rows)
            //                {
            //                    chainedColorID += "," + dr["colorIdentifier"].ToString();
            //                }

            //                PKG_INTG_BOM.SELECT_BOM_CODE_MPP_TB pkgSelectInternal = new PKG_INTG_BOM.SELECT_BOM_CODE_MPP_TB();
            //                pkgSelectInternal.ARG_CONCAT_PART_ID = chainedPartID;
            //                pkgSelectInternal.ARG_CONCAT_SUPP_MAT_ID = chainedSuppMatID;
            //                pkgSelectInternal.ARG_CONCAT_COLOR_ID = chainedColorID;
            //                pkgSelectInternal.OUT_CURSOR = string.Empty;

            //                DataTable mapTableForBOM = Exe_Select_PKG(pkgSelectInternal).Tables[0];

            //                #endregion

            //                // [0] : Part Library 없음, [1] : Material Library 없음, [2] : Color Library 없음
            //                bool[] messageCode = new bool[3] { false, false, false };

            //                // PART_SEQ, SORT_NO 채번용 변수
            //                int partSeq = 1;

            //                ArrayList arrayList2 = new ArrayList();

            //                foreach (DataRow dr in mapTable.Rows)
            //                {
            //                    string partType = dr["TYPE"].ToString();

            //                    // 미드솔/아웃솔 툴링은 WHQ에서 직접 관리, BOM 업로드 시 제외
            //                    if (partType == "MIDSOLE TOOLING" || partType == "OUTSOLE TOOLING")
            //                        continue;

            //                    #region 파트 매칭

            //                    string partNameID = dr["partNameIdentifier"].ToString();

            //                    if (partNameID.Length > 0)
            //                    {
            //                        DataRow[] matchedCode = mapTableForBOM.Select("TYPE = 'PART' AND PCX_PART_ID = '" + partNameID + "'");

            //                        if (matchedCode.Length > 0)
            //                        {
            //                            /* 매칭 성공 */
            //                            dr["PART_NAME"] = matchedCode[0]["PART_NAME"];
            //                            dr["PART_CD"] = matchedCode[0]["PART_CD"];
            //                        }
            //                        else
            //                        {
            //                            /* 매칭 실패 */
            //                            dr["PART_NAME"] = "Failed to match a part with pcx library.";
            //                        }

            //                        // 나이키 패턴 파트가 없는 경우 CS 패턴 파트는 나이키 파트로 설정
            //                        if (dr["patternPartIdentifier"].ToString() == "")
            //                        {
            //                            dr["CS_PTRN_NAME"] = matchedCode[0]["PART_NAME"];
            //                            dr["CS_PTRN_CD"] = matchedCode[0]["PART_CD"];
            //                        }
            //                    }

            //                    #endregion

            //                    #region 패턴 파트 매칭

            //                    string ptrnPartNameID = dr["patternPartIdentifier"].ToString();

            //                    if (ptrnPartNameID.Length > 0)
            //                    {
            //                        DataRow[] matchedCode = mapTableForBOM.Select("TYPE = 'PART' AND PCX_PART_ID = '" + ptrnPartNameID + "'");

            //                        if (matchedCode.Length > 0)
            //                        {
            //                            dr["PTRN_PART_NAME"] = matchedCode[0]["PART_NAME"];
            //                            dr["PTRN_PART_CD"] = matchedCode[0]["PART_CD"];

            //                            // 나이키 패턴 파트가 있는 경우 CS 패턴 파트는 나이키 패턴 파트로 설정
            //                            dr["CS_PTRN_NAME"] = matchedCode[0]["PART_NAME"];
            //                            dr["CS_PTRN_CD"] = matchedCode[0]["PART_CD"];
            //                        }
            //                        else
            //                            dr["PART_NAME"] = "Failed to match a pattern part with pcx library.";
            //                    }

            //                    #endregion

            //                    #region 자재 매칭

            //                    string suppMatID = dr["suppliedMaterialIdentifier"].ToString();

            //                    // No material means "0" in JSON file.
            //                    if (suppMatID.Length > 1)
            //                    {
            //                        DataRow[] matchedCode = mapTableForBOM.Select("TYPE = 'MATERIAL' AND NIKE_SUPP_MTL_CODE = '" + suppMatID + "'");

            //                        if (matchedCode.Length > 0)
            //                        {
            //                            // null은 입력 안 함
            //                            if (matchedCode[0]["PDM_MAT_CD"].ToString() == "null")
            //                                dr["PDM_MAT_CD"] = "";
            //                            else
            //                                dr["PDM_MAT_CD"] = matchedCode[0]["PDM_MAT_CD"];

            //                            dr["PDM_MAT_NAME"] = matchedCode[0]["PDM_MAT_NAME"];

            //                            // null은 입력 안 함
            //                            if (matchedCode[0]["MXSXL_NUMBER"].ToString() == "null")
            //                                dr["MXSXL_NUMBER"] = "";
            //                            else
            //                                dr["MXSXL_NUMBER"] = matchedCode[0]["MXSXL_NUMBER"];

            //                            dr["MCS_NUMBER"] = matchedCode[0]["MCS_NUMBER"];
            //                            dr["VENDOR_NAME"] = matchedCode[0]["VENDOR_NAME"];
            //                            dr["CS_CD"] = matchedCode[0]["CS_CD"];

            //                            //if (matchedCode[0]["NIKE_MS_STATE"].ToString() == "Retired")
            //                            //    dr["MAT_COMMENTS"] = "The nike status of this material is retired.";
            //                        }
            //                        else
            //                        {
            //                            /* Matching 실패한 경우 */
            //                            if (suppMatID != "100")
            //                                dr["MAT_COMMENTS"] = "Failed to match a material with pcx library.";
            //                        }
            //                    }

            //                    #endregion

            //                    #region 칼라 매칭

            //                    string colorID = dr["colorIdentifier"].ToString();

            //                    if (colorID.Length > 0)
            //                    {
            //                        DataRow[] matchedCode = mapTableForBOM.Select("TYPE = 'COLOR' AND PCX_COLOR_ID = '" + colorID + "'");

            //                        // 매칭에 성공한 경우
            //                        if (matchedCode.Length > 0)
            //                        {
            //                            /* PCX_COLOR_NAME
            //                                 * Type 1 : 9NM-LARGENT STEVE-93380
            //                                 * Type 2 : MC-416 */

            //                            string pcxColorName = matchedCode[0]["PCX_COLOR_NAME"].ToString();
            //                            string[] pdmCodeNames = pcxColorName.Split('-');

            //                            // 멀티 컬러의 경우 코드는 공백, 네임에만 표기
            //                            if (pdmCodeNames[0] == "MC")
            //                            {
            //                                dr["COLOR_CD"] = matchedCode[0]["LGCY_COLOR_CD"].ToString();
            //                                dr["COLOR_NAME"] = pcxColorName;
            //                            }
            //                            else
            //                            {
            //                                dr["COLOR_CD"] = pdmCodeNames[0];
            //                                dr["COLOR_NAME"] = pdmCodeNames[1];
            //                            }
            //                        }
            //                        else
            //                            dr["COLOR_COMMENTS"] = "Failed to match a color with pcx library.";
            //                    }

            //                    #endregion

            //                    PKG_INTG_BOM.INSERT_BOM_TAIN_JSON pkgInsertData = new PKG_INTG_BOM.INSERT_BOM_TAIN_JSON();
            //                    pkgInsertData.ARG_FACTORY = SessionInfo.Factory;
            //                    pkgInsertData.ARG_WS_NO = newWsNo;
            //                    pkgInsertData.ARG_PART_SEQ = partSeq.ToString();
            //                    pkgInsertData.ARG_PART_NO = dr["bomLineSortSequence"].ToString();
            //                    pkgInsertData.ARG_PART_CD = dr["PART_CD"].ToString();
            //                    pkgInsertData.ARG_PART_NAME = dr["PART_NAME"].ToString();
            //                    pkgInsertData.ARG_PART_TYPE = dr["TYPE"].ToString();
            //                    pkgInsertData.MXSXL_NUMBER = dr["MXSXL_NUMBER"].ToString();
            //                    pkgInsertData.ARG_MAT_CD = dr["PDM_MAT_CD"].ToString();
            //                    pkgInsertData.ARG_MAT_NAME = (dr["suppliedMaterialIdentifier"].ToString() == "100") ? "PLACEHOLDER" : dr["PDM_MAT_NAME"].ToString();
            //                    pkgInsertData.ARG_MAT_COMMENTS = dr["bomLineItemComments"].ToString() == "" ? dr["MAT_COMMENTS"].ToString() : dr["bomLineItemComments"].ToString();
            //                    pkgInsertData.ARG_MCS_NUMBER = dr["MCS_NUMBER"].ToString();
            //                    pkgInsertData.ARG_NIKE_COMMENT = dr["bomLineItemComments"].ToString();
            //                    pkgInsertData.ARG_COLOR_CD = dr["COLOR_CD"].ToString();
            //                    pkgInsertData.ARG_COLOR_NAME = dr["COLOR_NAME"].ToString();
            //                    pkgInsertData.ARG_COLOR_COMMENTS = dr["COLOR_COMMENTS"].ToString();
            //                    pkgInsertData.ARG_SORT_NO = partSeq.ToString();
            //                    pkgInsertData.ARG_UPD_USER = SessionInfo.UserID;
            //                    pkgInsertData.ARG_PTRN_PART_NAME = dr["PTRN_PART_NAME"].ToString();
            //                    pkgInsertData.ARG_PCX_SUPP_MAT_ID = dr["suppliedMaterialIdentifier"].ToString();
            //                    pkgInsertData.ARG_PCX_COLOR_ID = dr["colorIdentifier"].ToString();
            //                    pkgInsertData.ARG_VENDOR_NAME = dr["VENDOR_NAME"].ToString();
            //                    pkgInsertData.ARG_PCX_MAT_ID = dr["materialItemIdentifier"].ToString();
            //                    pkgInsertData.ARG_PTRN_PART_CD = dr["PTRN_PART_CD"].ToString();
            //                    pkgInsertData.ARG_CS_CD = dr["CS_CD"].ToString();
            //                    pkgInsertData.ARG_MDSL_CHK = (dr["TYPE"].ToString() == "MIDSOLE" || dr["TYPE"].ToString() == "AIRBAG") ? "Y" : "N";
            //                    pkgInsertData.ARG_OTSL_CHK = (dr["TYPE"].ToString() == "OUTSOLE") ? "Y" : "N";
            //                    pkgInsertData.ARG_CS_PTRN_NAME = dr["CS_PTRN_NAME"].ToString();
            //                    pkgInsertData.ARG_CS_PTRN_CD = dr["CS_PTRN_CD"].ToString();
            //                    pkgInsertData.ARG_LOGIC_GROUP = "";
            //                    pkgInsertData.ARG_MAT_FORECAST_PRCNT = 0;
            //                    pkgInsertData.ARG_COLOR_FORECAST_PRCNT = 0;

            //                    // STICKER는 DB에서 처리

            //                    arrayList2.Add(pkgInsertData);
            //                    partSeq++;
            //                }

            //                if (Exe_Modify_PKG(arrayList2) == null)
            //                {
            //                    MessageBox.Show("Failed to save BOM Data.");
            //                    return;
            //                }

            //                #endregion

            //                chainedWsNo += "," + newWsNo;
            //            }
            //        }
            //    }

            //    BOMUploader headUpdater = new BOMUploader();
            //    headUpdater.CONCAT_WS_NO = chainedWsNo;

            //    if (headUpdater.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            //        BindDataSourceToGridView();
            //}
            //catch (Exception ex)
            //{
            //    MessageBox.Show(ex.ToString());
            //    return;
            //}

            #endregion
        }

        /// <summary>
        /// Upload Logic BOM
        /// </summary>
        /// <param name="fileNames"></param>
        private void LogicBomUploader(string[] fileNames)
        {
            try
            {
                // Chained Key List like ,WS_NO_1,WS_NO_2,WS_NO_3,...
                string chainedWsNo = string.Empty;

                foreach (string fileName in fileNames)
                {
                    using (StreamReader fileStream = File.OpenText(fileName))
                    {
                        using (JsonTextReader reader = new JsonTextReader(fileStream))
                        {
                            JObject jObjects = (JObject)JToken.ReadFrom(reader);

                            DataSet cnvrtdObjects = ConvertJson2Xml(jObjects.ToString());

                            DataTable dtLogicBOMHeader = cnvrtdObjects.Tables["Logic BOM Header"];
                            DataTable dtLogicBOMCycleYear = new DataTable();
                            DataTable dtLogicBOMData = new DataTable();

                            string cycleYear = string.Empty;

                            if (cnvrtdObjects.Tables.Count > 2)
                            {
                                // When cycle year has values more than one.
                                dtLogicBOMData = cnvrtdObjects.Tables["Logic BOM Data"];
                                dtLogicBOMCycleYear = cnvrtdObjects.Tables["cycleYear"];

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
                                dtLogicBOMData = cnvrtdObjects.Tables["Logic BOM Data"];
                                cycleYear = dtLogicBOMHeader.Rows[0]["cycleYear"].ToString();
                            }

                            #region 신규 WS_NO를 채번하여 가져옴

                            PKG_INTG_BOM.SELECT_NEW_WS_NO pkgSelectWsNo = new PKG_INTG_BOM.SELECT_NEW_WS_NO();
                            pkgSelectWsNo.ARG_FACTORY = SessionInfo.Factory;
                            pkgSelectWsNo.OUT_CURSOR = string.Empty;

                            DataTable dataSource = Exe_Select_PKG(pkgSelectWsNo).Tables[0];

                            if (dataSource.Rows.Count == 0)
                            {
                                MessageBox.Show("Failed to create a new WS_NO");
                                return;
                            }

                            #endregion

                            string newWsNo = dataSource.Rows[0]["WS_NO"].ToString();

                            DataRow headerValues = dtLogicBOMHeader.Rows[0];

                            #region Upload Logic BOM header for backup.

                            PKG_INTG_BOM.INSERT_BOM_HEAD_JSON_ORG pkgInsertOrgHead = new PKG_INTG_BOM.INSERT_BOM_HEAD_JSON_ORG();
                            pkgInsertOrgHead.ARG_FACTORY = SessionInfo.Factory;
                            pkgInsertOrgHead.ARG_WS_NO = newWsNo;
                            pkgInsertOrgHead.ARG_OBJ_ID = headerValues["objectId"].ToString();
                            pkgInsertOrgHead.ARG_OBJ_TYPE = headerValues["objectType"].ToString();
                            pkgInsertOrgHead.ARG_BOM_CONTRACT_VER = headerValues["bomContractVersion"].ToString();
                            pkgInsertOrgHead.ARG_DEV_STYLE_ID = headerValues["developmentStyleIdentifier"].ToString();
                            pkgInsertOrgHead.ARG_DEV_COLORWAY_ID = headerValues["developmentColorwayIdentifier"].ToString();
                            pkgInsertOrgHead.ARG_COLORWAY_NAME = headerValues["colorwayName"].ToString();
                            pkgInsertOrgHead.ARG_SRC_CONFIG_ID = headerValues["sourcingConfigurationIdentifier"].ToString();
                            pkgInsertOrgHead.ARG_SRC_CONFIG_NAME = headerValues["sourcingConfigurationName"].ToString();
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

                            ArrayList listHeader = new ArrayList();

                            listHeader.Add(pkgInsertOrgHead);

                            if (Exe_Modify_PKG(listHeader) == null)
                            {
                                MessageBox.Show("Failed to save JSON Org. Header Info.");
                                return;
                            }

                            #endregion

                            #region Upload Logic BOM header for live.

                            #region Style Name

                            /* Dev Style Name : Style Name + '-' + Style Number */

                            string[] devStyleName = new string[2] { "", "" };

                            if (headerValues["styleName"].ToString().Contains('-'))
                                devStyleName = headerValues["styleName"].ToString().Split('-');

                            string styleName = devStyleName[0].Trim();

                            #endregion

                            #region Color Version

                            /* Colorway Name : Dev Colorway ID + '-' + Dev Colorway Description + '--' + Initial Season */

                            // Zero-based
                            int idxDelimiter1 = headerValues["colorwayName"].ToString().IndexOf('-');
                            int idxDelimiter2 = headerValues["colorwayName"].ToString().LastIndexOf('-');

                            // Colorway Desc. 길이
                            int length = idxDelimiter2 - idxDelimiter1 - 1;

                            // Colorway Desc.
                            string devColorDescription = string.Empty;

                            if (length < 3)
                                devColorDescription = "";
                            else
                                devColorDescription = headerValues["colorwayName"].ToString().Substring(idxDelimiter1 + 1, length);

                            #endregion

                            #region Product Code

                            string productCode = "";

                            if (headerValues["productCode"].ToString() != "")
                                productCode = headerValues["styleNumber"].ToString() + headerValues["colorwayCode"].ToString();

                            #endregion

                            #region Sourcing Factory

                            string[] factoriesInCS = new string[4] { "VJ", "JJ", "QD", "RJ" };
                            string prodFactory = "";

                            if (headerValues["sourcingConfigurationName"].ToString() != "")
                            {
                                prodFactory = headerValues["sourcingConfigurationName"].ToString().Substring(0, 2);

                                if (factoriesInCS.Contains(prodFactory) == false)
                                    prodFactory = "";
                            }

                            #endregion

                            PKG_INTG_BOM.INSERT_BOM_HEAD_JSON pkgInsertHead = new PKG_INTG_BOM.INSERT_BOM_HEAD_JSON();
                            pkgInsertHead.ARG_FACTORY = SessionInfo.Factory;
                            pkgInsertHead.ARG_WS_NO = newWsNo;
                            pkgInsertHead.ARG_DEV_NAME = styleName;
                            pkgInsertHead.ARG_PRODUCT_CODE = productCode;
                            pkgInsertHead.ARG_PROD_FACTORY = prodFactory;
                            pkgInsertHead.ARG_GENDER = "";
                            pkgInsertHead.ARG_COLOR_VER = devColorDescription;
                            pkgInsertHead.ARG_MODEL_ID = headerValues["modelIdentifier"].ToString();
                            pkgInsertHead.ARG_PCC_PM = SessionInfo.UserID;
                            pkgInsertHead.ARG_DEV_STYLE_ID = headerValues["developmentStyleIdentifier"].ToString();
                            pkgInsertHead.ARG_DEV_COLORWAY_ID = headerValues["developmentColorwayIdentifier"].ToString();
                            pkgInsertHead.ARG_PRODUCT_ID = headerValues["productId"].ToString();
                            pkgInsertHead.ARG_STYLE_NUMBER = headerValues["styleNumber"].ToString();
                            pkgInsertHead.ARG_SOURCING_CONFIG_ID = headerValues["sourcingConfigurationIdentifier"].ToString();
                            pkgInsertHead.ARG_PCX_BOM_ID = headerValues["logicBomIdentifier"].ToString();
                            pkgInsertHead.ARG_DEV_STYLE_TYPE_ID = headerValues["developmentStyleTypeIdentifier"].ToString();
                            pkgInsertHead.ARG_LOGIC_BOM_STATE_ID = headerValues["logicBOMStateIdentifier"].ToString();
                            pkgInsertHead.ARG_LOGIC_BOM_GATE_ID = headerValues["logicBOMGateIdentifier"].ToString();
                            pkgInsertHead.ARG_CYCLE_YEAR = cycleYear;
                            pkgInsertHead.ARG_LOGIC_BOM_YN = "Y";

                            ArrayList arrayList = new ArrayList();

                            arrayList.Add(pkgInsertHead);

                            if (Exe_Modify_PKG(arrayList) == null)
                            {
                                MessageBox.Show("Failed to save BOM Header");
                                return;
                            }

                            #endregion

                            #region Upload Logic BOM data for backup.

                            ArrayList listBOMData = new ArrayList();
                            int partSeqOrg = 1;

                            foreach (DataRow lineItem in dtLogicBOMData.Rows)
                            {
                                PKG_INTG_BOM.INSERT_BOM_TAIN_JSON_ORG pkgInsertOrgData = new PKG_INTG_BOM.INSERT_BOM_TAIN_JSON_ORG();
                                pkgInsertOrgData.ARG_FACTORY = SessionInfo.Factory;
                                pkgInsertOrgData.ARG_WS_NO = newWsNo;
                                pkgInsertOrgData.ARG_BOM_LINE_SORT_SEQ = lineItem["logicBOMLineItemNumber"].ToString();
                                pkgInsertOrgData.ARG_BOM_SECTION_ID = lineItem["logicSectionIdentifier"].ToString();
                                pkgInsertOrgData.ARG_PART_NAME_ID = lineItem["partNameIdentifier"].ToString();
                                pkgInsertOrgData.ARG_PTRN_PART_ID = lineItem["patternPartIdentifier"].ToString();
                                pkgInsertOrgData.ARG_SUPP_MAT_ID = lineItem["suppliedMaterialIdentifier"].ToString();
                                pkgInsertOrgData.ARG_MAT_ITEM_ID = lineItem["materialItemIdentifier"].ToString();
                                pkgInsertOrgData.ARG_MAT_ITEM_PLHDR_DESC = lineItem["materialItemPlaceholderDescription"].ToString();
                                pkgInsertOrgData.ARG_COLOR_ID = lineItem["colorIdentifier"].ToString();
                                pkgInsertOrgData.ARG_COLOR_PLHDR_DESC = lineItem["colorPlaceholderDescription"].ToString();
                                pkgInsertOrgData.ARG_SUPP_MAT_COLOR_IS_MUL = lineItem["suppliedMaterialColorIsMultipleColors"].ToString();
                                pkgInsertOrgData.ARG_SUPP_MAT_COLOR_ID = lineItem["suppliedMaterialColorIdentifier"].ToString();
                                pkgInsertOrgData.ARG_BOM_LINEITEM_COMMENTS = lineItem["logicBOMLineItemComments"].ToString();
                                pkgInsertOrgData.ARG_FAC_IN_HOUSE_IND = lineItem["factoryInHouseIndicator"].ToString();
                                pkgInsertOrgData.ARG_COUNTY_ORG_ID = lineItem["countyOfOriginIdentifier"].ToString();
                                pkgInsertOrgData.ARG_ACTUAL_DIMENSION_DESC = lineItem["actualDimensionDescription"].ToString();
                                pkgInsertOrgData.ARG_ISO_MEASUREMENT_CODE = lineItem["isoMeasurementCode"].ToString();
                                pkgInsertOrgData.ARG_NET_USAGE_NUMBER = lineItem["netUsageNumber"].ToString();
                                pkgInsertOrgData.ARG_WASTE_USAGE_NUMBER = lineItem["wasteUsageNumber"].ToString();
                                pkgInsertOrgData.ARG_PART_YIELD = lineItem["partYield"].ToString();
                                pkgInsertOrgData.ARG_CONSUM_CONVER_RATE = lineItem["consumptionConversionRate"].ToString();
                                pkgInsertOrgData.ARG_LINEITEM_DEFECT_PER_NUM = lineItem["lineItemDefectPercentNumber"].ToString();
                                pkgInsertOrgData.ARG_UNIT_PRICE_ISO_MSR_CODE = lineItem["unitPriceISOMeasurementCode"].ToString();
                                pkgInsertOrgData.ARG_CURRENCY_CODE = lineItem["currencyCode"].ToString();
                                pkgInsertOrgData.ARG_FACTORY_UNIT_PRICE = lineItem["factoryUnitPrice"].ToString();
                                pkgInsertOrgData.ARG_UNIT_PRICE_UPCHARGE_NUM = lineItem["unitPriceUpchargeNumber"].ToString();
                                pkgInsertOrgData.ARG_UNIT_PRICE_UPCHARGE_DESC = lineItem["unitPriceUpchargeDescription"].ToString();
                                pkgInsertOrgData.ARG_FREIGHT_TERM_ID = lineItem["freightTermIdentifier"].ToString();
                                pkgInsertOrgData.ARG_LANDED_COST_PER_NUM = lineItem["landedCostPercentNumber"].ToString();
                                pkgInsertOrgData.ARG_PCC_SORT_ORDER = lineItem["pccSortOrder"].ToString();
                                pkgInsertOrgData.ARG_ROLLUP_VARIATION_LV = "";
                                pkgInsertOrgData.ARG_PART_SEQ = Convert.ToString(partSeqOrg);
                                pkgInsertOrgData.ARG_LOGIC_GROUP = lineItem["logicGroup"].ToString();
                                pkgInsertOrgData.ARG_MAT_FORECAST_PRCNT = Convert.ToDouble(lineItem["materialForecastPercent"].ToString());
                                pkgInsertOrgData.ARG_COLOR_FORECAST_PRCNT = Convert.ToDouble(lineItem["colorForecastPercent"].ToString());

                                listBOMData.Add(pkgInsertOrgData);
                                partSeqOrg++;
                            }

                            if (Exe_Modify_PKG(listBOMData) == null)
                            {
                                MessageBox.Show("Failed to save JSON Org. Data");
                                return;
                            }

                            #endregion

                            #region Upload Logic BOM data for live.

                            // 매칭된 코드를 저장할 신규 테이블 생성
                            DataTable mapTable = new DataTable();

                            // JSON BOM 원본의 스키마를 그대로 복사
                            mapTable = dtLogicBOMData.Clone();

                            #region 매칭된 코드를 저장할 신규 컬럼 추가

                            mapTable.Columns.Add("TYPE");
                            mapTable.Columns.Add("PART_CD");
                            mapTable.Columns.Add("PART_NAME");
                            mapTable.Columns.Add("PTRN_PART_CD");
                            mapTable.Columns.Add("PTRN_PART_NAME");
                            mapTable.Columns.Add("CS_PTRN_CD");
                            mapTable.Columns.Add("CS_PTRN_NAME");

                            mapTable.Columns.Add("PDM_MAT_CD");
                            mapTable.Columns.Add("PDM_MAT_NAME");
                            mapTable.Columns.Add("MAT_COMMENTS");
                            mapTable.Columns.Add("MXSXL_NUMBER");
                            mapTable.Columns.Add("MCS_NUMBER");

                            mapTable.Columns.Add("COLOR_CD");
                            mapTable.Columns.Add("COLOR_NAME");
                            mapTable.Columns.Add("COLOR_COMMENTS");

                            mapTable.Columns.Add("VENDOR_NAME");
                            mapTable.Columns.Add("CS_CD");
                            mapTable.Columns.Add("MDSL_CHK");
                            mapTable.Columns.Add("OTSL_CHK");

                            #endregion

                            #region Type 매칭 및 라인 아이템 원본 복사

                            #region Nike Material Section ID Mapping Table을 가져옴

                            PKG_INTG_BOM.SELECT_COMMON_CODE_MPP_TB pkgSelectCommon = new PKG_INTG_BOM.SELECT_COMMON_CODE_MPP_TB();
                            pkgSelectCommon.ARG_COMMON_ID = "PCX001";
                            pkgSelectCommon.OUT_CURSOR = string.Empty;

                            DataTable mapTableForColumn = Exe_Select_PKG(pkgSelectCommon).Tables[0];

                            #endregion

                            foreach (DataRow dr in dtLogicBOMData.Rows)
                            {
                                // 원본 행을 하나씩 복사
                                mapTable.ImportRow(dr);

                                // 매칭할 섹션 아이디
                                string sectionId = dr["logicSectionIdentifier"].ToString();

                                // 매칭된 코드명
                                DataRow[] matchedCode = mapTableForColumn.Select("NIKE_ID = '" + sectionId + "'");

                                // 테이블의 행 번호는 0부터 시작
                                int rowIdx = mapTable.Rows.Count - 1;

                                // TYPE 컬럼에 매칭된 코드를 대문자로 변환하여 입력
                                mapTable.Rows[rowIdx]["TYPE"] = matchedCode[0]["LIST_VALUES"].ToString().ToUpper();
                            }

                            #endregion

                            #region Part, Material, Color Mapping Table을 가져옴

                            // BOM의 파트,자재,컬러ID 리스트
                            DataTable listOfPartID = mapTable.DefaultView.ToTable(true, "partNameIdentifier");
                            DataTable listOfPtrnPartID = mapTable.DefaultView.ToTable(true, "patternPartIdentifier");
                            DataTable listOfSuppMatID = mapTable.DefaultView.ToTable(true, "suppliedMaterialIdentifier");
                            DataTable listOfColorID = mapTable.DefaultView.ToTable(true, "colorIdentifier");

                            // (코드,코드,코드) 포맷으로 문자열 연결
                            string chainedPartID = string.Empty;
                            string chainedSuppMatID = string.Empty;
                            string chainedColorID = string.Empty;

                            foreach (DataRow dr in listOfPartID.Rows)
                            {
                                chainedPartID += "," + dr["partNameIdentifier"].ToString();
                            }

                            // 패턴 파트는 파트 라이브러리와 동일하게 사용
                            foreach (DataRow dr in listOfPtrnPartID.Rows)
                            {
                                chainedPartID += "," + dr["patternPartIdentifier"].ToString();
                            }

                            foreach (DataRow dr in listOfSuppMatID.Rows)
                            {
                                chainedSuppMatID += "," + dr["suppliedMaterialIdentifier"].ToString();
                            }

                            foreach (DataRow dr in listOfColorID.Rows)
                            {
                                chainedColorID += "," + dr["colorIdentifier"].ToString();
                            }

                            PKG_INTG_BOM.SELECT_BOM_CODE_MPP_TB pkgSelectInternal = new PKG_INTG_BOM.SELECT_BOM_CODE_MPP_TB();
                            pkgSelectInternal.ARG_CONCAT_PART_ID = chainedPartID;
                            pkgSelectInternal.ARG_CONCAT_SUPP_MAT_ID = chainedSuppMatID;
                            pkgSelectInternal.ARG_CONCAT_COLOR_ID = chainedColorID;
                            pkgSelectInternal.OUT_CURSOR = string.Empty;

                            DataTable mapTableForBOM = Exe_Select_PKG(pkgSelectInternal).Tables[0];

                            #endregion

                            // [0] : Part Library 없음, [1] : Material Library 없음, [2] : Color Library 없음
                            bool[] messageCode = new bool[3] { false, false, false };

                            // PART_SEQ, SORT_NO 채번용 변수
                            int partSeq = 1;

                            ArrayList arrayList2 = new ArrayList();

                            foreach (DataRow dr in mapTable.Rows)
                            {
                                string partType = dr["TYPE"].ToString();

                                // 미드솔/아웃솔 툴링은 WHQ에서 직접 관리, BOM 업로드 시 제외
                                if (partType == "MIDSOLE TOOLING" || partType == "OUTSOLE TOOLING")
                                    continue;

                                #region 파트 매칭

                                string partNameID = dr["partNameIdentifier"].ToString();

                                if (partNameID.Length > 0)
                                {
                                    DataRow[] matchedCode = mapTableForBOM.Select("TYPE = 'PART' AND PCX_PART_ID = '" + partNameID + "'");

                                    if (matchedCode.Length > 0)
                                    {
                                        /* 매칭 성공 */
                                        dr["PART_NAME"] = matchedCode[0]["PART_NAME"];
                                        dr["PART_CD"] = matchedCode[0]["PART_CD"];
                                    }
                                    else
                                    {
                                        /* 매칭 실패 */
                                        dr["PART_NAME"] = "Failed to match a part with pcx library.";
                                    }

                                    //// 나이키 패턴 파트가 없는 경우 CS 패턴 파트는 나이키 파트로 설정
                                    //if (dr["patternPartIdentifier"].ToString() == "")
                                    //{
                                    //    dr["CS_PTRN_NAME"] = matchedCode[0]["PART_NAME"];
                                    //    dr["CS_PTRN_CD"] = matchedCode[0]["PART_CD"];
                                    //}
                                }

                                #endregion

                                #region 패턴 파트 매칭

                                string ptrnPartNameID = dr["patternPartIdentifier"].ToString();

                                if (ptrnPartNameID.Length > 0)
                                {
                                    DataRow[] matchedCode = mapTableForBOM.Select("TYPE = 'PART' AND PCX_PART_ID = '" + ptrnPartNameID + "'");

                                    if (matchedCode.Length > 0)
                                    {
                                        dr["PTRN_PART_NAME"] = matchedCode[0]["PART_NAME"];
                                        dr["PTRN_PART_CD"] = matchedCode[0]["PART_CD"];

                                        //// 나이키 패턴 파트가 있는 경우 CS 패턴 파트는 나이키 패턴 파트로 설정
                                        //dr["CS_PTRN_NAME"] = matchedCode[0]["PART_NAME"];
                                        //dr["CS_PTRN_CD"] = matchedCode[0]["PART_CD"];
                                    }
                                    else
                                        dr["PART_NAME"] = "Failed to match a pattern part with pcx library.";
                                }

                                #endregion

                                #region 자재 매칭

                                string suppMatID = dr["suppliedMaterialIdentifier"].ToString();

                                // No material means "0" in JSON file.
                                if (suppMatID.Length > 1)
                                {
                                    DataRow[] matchedCode = mapTableForBOM.Select("TYPE = 'MATERIAL' AND NIKE_SUPP_MTL_CODE = '" + suppMatID + "'");

                                    if (matchedCode.Length > 0)
                                    {
                                        // null은 입력 안 함
                                        if (matchedCode[0]["PDM_MAT_CD"].ToString() == "null")
                                            dr["PDM_MAT_CD"] = "";
                                        else
                                            dr["PDM_MAT_CD"] = matchedCode[0]["PDM_MAT_CD"];

                                        dr["PDM_MAT_NAME"] = matchedCode[0]["PDM_MAT_NAME"];

                                        // null은 입력 안 함
                                        if (matchedCode[0]["MXSXL_NUMBER"].ToString() == "null")
                                            dr["MXSXL_NUMBER"] = "";
                                        else
                                            dr["MXSXL_NUMBER"] = matchedCode[0]["MXSXL_NUMBER"];

                                        dr["MCS_NUMBER"] = matchedCode[0]["MCS_NUMBER"];
                                        dr["VENDOR_NAME"] = matchedCode[0]["VENDOR_NAME"];
                                        dr["CS_CD"] = matchedCode[0]["CS_CD"];

                                        //if (matchedCode[0]["NIKE_MS_STATE"].ToString() == "Retired")
                                        //    dr["MAT_COMMENTS"] = "The nike status of this material is retired.";
                                    }
                                    else
                                    {
                                        /* Matching 실패한 경우 */
                                        if (suppMatID != "100")
                                            dr["MAT_COMMENTS"] = "Failed to match a material with pcx library.";
                                    }
                                }

                                #endregion

                                #region 칼라 매칭
                                string colorID = dr["colorIdentifier"].ToString();
                                if (colorID.Length > 0)
                                {
                                    DataRow[] matchedCode = mapTableForBOM.Select("TYPE = 'COLOR' AND PCX_COLOR_ID = '" + colorID + "'");
                                    // 매칭에 성공한 경우
                                    if (matchedCode.Length > 0)
                                    {
                                        /* PCX_COLOR_NAME
                                             * Type 1 : 9NM-LARGENT STEVE-93380
                                             * Type 2 : MC-416 */

                                        string pcxColorName = matchedCode[0]["PCX_COLOR_NAME"].ToString();
                                        string[] pdmCodeNames = pcxColorName.Split('-');
                                        // 멀티 컬러의 경우 코드는 공백, 네임에만 표기
                                        if (pdmCodeNames[0] == "MC")
                                            dr["COLOR_NAME"] = pcxColorName;
                                        else
                                        {
                                            dr["COLOR_CD"] = pdmCodeNames[0];
                                            dr["COLOR_NAME"] = pdmCodeNames[1];
                                        }
                                    }
                                    else
                                        dr["COLOR_COMMENTS"] = "Failed to match a color with pcx library.";
                                }
                                #endregion

                                PKG_INTG_BOM.INSERT_BOM_TAIN_JSON pkgInsertData = new PKG_INTG_BOM.INSERT_BOM_TAIN_JSON();
                                pkgInsertData.ARG_FACTORY = SessionInfo.Factory;
                                pkgInsertData.ARG_WS_NO = newWsNo;
                                pkgInsertData.ARG_PART_SEQ = partSeq.ToString();
                                pkgInsertData.ARG_PART_NO = dr["logicBOMLineItemNumber"].ToString();
                                pkgInsertData.ARG_PART_CD = dr["PART_CD"].ToString();
                                pkgInsertData.ARG_PART_NAME = dr["PART_NAME"].ToString();
                                pkgInsertData.ARG_PART_TYPE = dr["TYPE"].ToString();
                                pkgInsertData.MXSXL_NUMBER = dr["MXSXL_NUMBER"].ToString();
                                pkgInsertData.ARG_MAT_CD = dr["PDM_MAT_CD"].ToString();
                                pkgInsertData.ARG_MAT_NAME = (dr["suppliedMaterialIdentifier"].ToString() == "100") ? "PLACEHOLDER" : dr["PDM_MAT_NAME"].ToString();
                                pkgInsertData.ARG_MAT_COMMENTS = dr["MAT_COMMENTS"].ToString();
                                pkgInsertData.ARG_MCS_NUMBER = dr["MCS_NUMBER"].ToString();
                                pkgInsertData.ARG_NIKE_COMMENT = dr["logicBOMLineItemComments"].ToString();
                                pkgInsertData.ARG_COLOR_CD = dr["COLOR_CD"].ToString();
                                pkgInsertData.ARG_COLOR_NAME = dr["COLOR_NAME"].ToString();
                                pkgInsertData.ARG_COLOR_COMMENTS = dr["COLOR_COMMENTS"].ToString();
                                pkgInsertData.ARG_SORT_NO = partSeq.ToString();
                                pkgInsertData.ARG_UPD_USER = SessionInfo.UserID;
                                pkgInsertData.ARG_PTRN_PART_NAME = dr["PTRN_PART_NAME"].ToString();
                                pkgInsertData.ARG_PCX_SUPP_MAT_ID = dr["suppliedMaterialIdentifier"].ToString();
                                pkgInsertData.ARG_PCX_COLOR_ID = dr["colorIdentifier"].ToString();
                                pkgInsertData.ARG_VENDOR_NAME = dr["VENDOR_NAME"].ToString();
                                pkgInsertData.ARG_PCX_MAT_ID = dr["materialItemIdentifier"].ToString();
                                pkgInsertData.ARG_PTRN_PART_CD = dr["PTRN_PART_CD"].ToString();
                                pkgInsertData.ARG_CS_CD = dr["CS_CD"].ToString();
                                pkgInsertData.ARG_MDSL_CHK = (dr["TYPE"].ToString() == "MIDSOLE" || dr["TYPE"].ToString() == "AIRBAG") ? "Y" : "N";
                                pkgInsertData.ARG_OTSL_CHK = (dr["TYPE"].ToString() == "OUTSOLE") ? "Y" : "N";
                                pkgInsertData.ARG_CS_PTRN_NAME = dr["CS_PTRN_NAME"].ToString();
                                pkgInsertData.ARG_CS_PTRN_CD = dr["CS_PTRN_CD"].ToString();
                                pkgInsertData.ARG_LOGIC_GROUP = dr["logicGroup"].ToString();
                                pkgInsertData.ARG_MAT_FORECAST_PRCNT = Convert.ToDouble(dr["materialForecastPercent"].ToString());
                                pkgInsertData.ARG_COLOR_FORECAST_PRCNT = Convert.ToDouble(dr["colorForecastPercent"].ToString());
                                // STICKER는 DB에서 처리

                                arrayList2.Add(pkgInsertData);
                                partSeq++;
                            }

                            if (Exe_Modify_PKG(arrayList2) == null)
                            {
                                MessageBox.Show("Failed to save BOM Data.");
                                return;
                            }

                            #endregion

                            chainedWsNo += "," + newWsNo;
                        }
                    }
                }

                BOMUploader headUpdater = new BOMUploader();
                headUpdater.ChainedWSNo = chainedWsNo;

                if (headUpdater.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                    BindDataSourceGridView();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
                return;
            }
        }

        /// <summary>
        /// XML 형태의 BOM의 데이터를 추출하여 가져옴
        /// </summary>
        /// <returns></returns>
        private DataSet ExtractDataFromXMLFile(string fileName)
        {
            try
            {
                //Create new FileStream to read schema with.
                using (FileStream stream = new FileStream(fileName, System.IO.FileMode.Open))
                {
                    XmlTextReader reader = new XmlTextReader(stream);
                    DataSet dataSet = new DataSet();

                    dataSet.ReadXml(reader);
                    reader.Close();

                    return dataSet;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
                return null;
            }
        }

        /// <summary>
        /// Excel 형태의 BOM의 데이터를 추출하여 가져옴
        /// </summary>
        /// <param name="fileName"></param>
        /// <param name="hasHeaders"></param>
        /// <returns></returns>
        private DataTable ExtractDataFromExcelFile(string fileName, string hasHeaders)
        {
            try
            {
                /* hasHeaders : Excel의 첫번째 줄의 데이터를 Field 명으로 인식 할 것인지 여부(YES or NO) */

                // The connection used to open the database.
                string connectionString;
                // 파일 확장자 확인
                if (fileName.Substring(fileName.LastIndexOf('.')).ToLower() == ".xlsx")
                {
                    /*************************************************************************************************************************
                     * 엑셀 버전(xls, xlsx)에 따라 Provider가 다름
                     * You will need to ensure ACE is installed on the computer: http://www.microsoft.com/en-us/download/details.aspx?id=13255
                    *************************************************************************************************************************/
                    connectionString = "Provider=Microsoft.ACE.OLEDB.12.0;Data Source=" + fileName + ";Extended Properties=\"Excel 12.0 Xml;HDR=" + hasHeaders + ";IMEX=0\"";
                }
                else
                    connectionString = "Provider=Microsoft.Jet.OLEDB.4.0;Data Source=" + fileName + ";Extended Properties=\"Excel 8.0;HDR=" + hasHeaders + ";IMEX=0\"";

                DataSet dataSource = new DataSet();

                using (OleDbConnection connection = new OleDbConnection(connectionString))
                {
                    // 데이터베이스 연결
                    connection.Open();
                    // 스키마 정보를 가져옴
                    DataTable schemaTable = connection.GetOleDbSchemaTable(OleDbSchemaGuid.Tables, new object[] { null, null, null, "TABLE" });

                    foreach (DataRow schemaRow in schemaTable.Rows)
                    {
                        // Sheet 명
                        string sheet = schemaRow["TABLE_NAME"].ToString();
                        // Sheet 명 Validation
                        if (!sheet.EndsWith("_"))
                        {
                            try
                            {
                                OleDbCommand command = new OleDbCommand("SELECT * FROM [" + sheet + "]", connection);
                                command.CommandType = CommandType.Text;
                                // Sheet명으로 데이터 테이블 생성
                                DataTable excelSource = new DataTable(sheet);
                                // 반환할 DataSet에 위 테이블 저장
                                dataSource.Tables.Add(excelSource);
                                // Excel 파일을 데이터 테이블 형식으로 추출하여 미리 생성된 테이블에 저장
                                new OleDbDataAdapter(command).Fill(excelSource);
                            }
                            catch (Exception ex)
                            {
                                throw new Exception(ex.Message + string.Format("Sheet:{0}.File:F{1}", sheet, fileName), ex);
                            }
                        }
                    }
                }
                return dataSource.Tables[0];
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
                return null;
            }
        }

        /// <summary>
        /// Convert JSON to XML.
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
        /// 조회 조건에 맞는 데이터를 불러와 grdBomList 그리드에 바인딩한다.
        /// </summary>
        /// <param name="_workType"></param>
        private void BindDataSourceGridView()
        {
            PKG_INTG_BOM.SELECT_BOM_LIST pkgSelect = new PKG_INTG_BOM.SELECT_BOM_LIST();
            pkgSelect.ARG_FACTORY = lePCC.EditValue.ToString();
            pkgSelect.ARG_DEVELOPER = leDeveloper.EditValue.ToString();
            pkgSelect.ARG_SEASON = chkCBSeason.EditValue.ToString().Replace(" ", "");
            pkgSelect.ARG_DEV_COLORWAY_ID = txtDevColorwayId.Text.Replace(" ", "");
            pkgSelect.ARG_PCM_BOM_ID = txtPcmBomId.Text;
            pkgSelect.ARG_SAMPLE_TYPE = chkCBSampleType.EditValue.ToString().Replace(" ", "");
            pkgSelect.ARG_STYLE_NAME = txtStyleName.Text;
            pkgSelect.ARG_REP_YN = (btnRepChk.Checked) ? "Y" : "N";
            pkgSelect.ARG_DEV_STYLE_NUMBER = txtStyleNumber.Text;
            pkgSelect.ARG_PRODUCT_CODE = txtProductCode.Text;
            pkgSelect.OUT_CURSOR = string.Empty;

            DataSet ds = Exe_Select_PKG(pkgSelect);

            if (ds.Tables[0].Rows.Count > 0)
                grdBomList.DataSource = ds.Tables[0];
            else
                grdBomList.DataSource = null;
        }

        /// <summary>
        /// 유저가 선택한 BOM의 WS_NO를 ,WS_NO1,WS_NO2,WS_NO3,.. 형태로 연쇄시킴
        /// </summary>
        /// <returns></returns>
        private string ConcatenateWorksheetNumber()
        {
            // Concatenate each worksheet number.
            string concatWsNo = string.Empty;

            foreach (int rowHandle in gvwBomList.GetSelectedRows())
            {
                if (concatWsNo.Equals(""))
                    concatWsNo = gvwBomList.GetRowCellValue(rowHandle, "WS_NO").ToString();
                else
                    concatWsNo += "," + gvwBomList.GetRowCellValue(rowHandle, "WS_NO").ToString();
            }

            return concatWsNo;
        }

        /// <summary>
        /// Convert Nike season id to the format of internal code.
        /// </summary>
        /// <param name="seasonID"></param>
        /// <returns></returns>
        private string ConvertSeason(string seasonID)
        {
            if (seasonID != null)
            {
                switch (seasonID.Substring(0, 2))
                {
                    case "SP":

                        return string.Format("20{0}01", seasonID.Substring(2, 2));

                    case "SU":

                        return string.Format("20{0}02", seasonID.Substring(2, 2));

                    case "FA":

                        return string.Format("20{0}03", seasonID.Substring(2, 2));

                    case "HO":

                        return string.Format("20{0}04", seasonID.Substring(2, 2));

                    default:
                        return "";
                }
            }
            else
                return "";
        }

        #endregion
    }
}