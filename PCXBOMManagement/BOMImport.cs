using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO;                                // Path Class
using System.Collections;                       // ArrayList
using System.Xml;                               // XmlDocument

using DevExpress.XtraGrid;                      // GridControl
using DevExpress.XtraGrid.Columns;              // GridColumn
using DevExpress.XtraGrid.Views.Base;           // GridCell
using DevExpress.XtraGrid.Views.Grid;           // GridView
using DevExpress.XtraGrid.Views.Grid.ViewInfo;  // GridHitInfo
using DevExpress.Utils;                         // DXMouseEventArgs
using DevExpress.XtraEditors;                   // GridLookUpEdit

using CSI.PCC.PCX.COM;                          // Common Class
using CSI.PCC.PCX.PACKAGE;                      // Package Class

using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace CSI.PCC.PCX
{
    public partial class BOMImport : DevExpress.XtraEditors.XtraForm
    {
        private static string[] requiredFields = new string[] { "BOM_PART_UUID", "DEV_STYLE_ID", "DEV_COLORWAY_ID", "SRC_CONFIG_ID", "BOM_ID" };
        private class BOMHeader
        {
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
        }
        private class BOMData
        {
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
        }
        
        public string Factory { get; set; }
        public string WSNumber { get; set; }

        public BOMImport()
        {
            InitializeComponent();
        }

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);

            repositoryItemLookUpEdit1.DataSource = GetDataSource("LookUp_Gender");
            repositoryItemLookUpEdit2.DataSource = GetDataSource("LookUp_Age"); ;

            // Bind datasource for roll up level.
            DataTable sourceOfRollUp = new DataTable();
            sourceOfRollUp.Columns.Add("CODE");

            DataRow dr = sourceOfRollUp.NewRow();
            dr["CODE"] = "";
            sourceOfRollUp.Rows.Add(dr);

            dr = sourceOfRollUp.NewRow();
            dr["CODE"] = "1";
            sourceOfRollUp.Rows.Add(dr);

            dr = sourceOfRollUp.NewRow();
            dr["CODE"] = "2";
            sourceOfRollUp.Rows.Add(dr);

            repositoryItemLookUpEdit4.DataSource = sourceOfRollUp;

            // Write caption on the top of form.
            DataTable dataSource = GetDataSource("BOMCaption");

            if (dataSource.Rows.Count > 0)
            {
                string caption = string.Format("{0} / {1} / {2} / {3} / {4}",
                    dataSource.Rows[0]["SEASON"].ToString(),
                    dataSource.Rows[0]["SAMPLE_TYPE"].ToString(),
                    dataSource.Rows[0]["DEV_NAME"].ToString(),
                    dataSource.Rows[0]["DEV_STYLE_NUMBER"].ToString(),
                    dataSource.Rows[0]["DEV_COLORWAY_ID"].ToString());

                this.Text = caption;
            }

            if (Common.categorySU.Contains(Common.sessionID))
            {
                btnExportSRT.Visible = true;
                gvwHeader.Columns["BOM_PART_UUID"].Visible = true;
            }

            BindDataSourceGridView("Header");
            BindDataSourceGridView("Data");
        }

        #region Grid Events.

        /// <summary>
        /// Set indicator of the grid.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void gvwHeader_DrawRowIndicator(object sender, RowIndicatorCustomDrawEventArgs e)
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
        /// Highlight the column required when importing BOM.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void gvwHeader_ColumnHeader(object sender, ColumnHeaderCustomDrawEventArgs e)
        {
            if (e.Column == null)
                return;
            else if (requiredFields.Contains(e.Column.FieldName))
            {
                e.Cache.FillRectangle(Color.Coral, e.Bounds);
                e.Appearance.DrawString(e.Cache, e.Info.Caption, e.Info.CaptionRect);
                e.Handled = true;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void gvwData_RowCellStyle(object sender, RowCellStyleEventArgs e)
        {
            switch (e.Column.FieldName)
            {
                case "NIKE_MS_STATE":

                    if (gvwData.GetRowCellValue(e.RowHandle, "NIKE_MS_STATE").ToString().Equals("Retired"))
                    {
                        e.Appearance.ForeColor = Color.Orange;
                        e.Appearance.Font = new Font(e.Appearance.Font, FontStyle.Bold);
                        e.Appearance.BackColor = Color.Firebrick;
                    }

                    break;

                case "SUPP_MAT_ID":

                    if (gvwData.GetRowCellValue(e.RowHandle, "SUPP_MAT_ID").ToString().Equals("100"))
                    {
                        if (!gvwData.IsCellSelected(e.RowHandle, e.Column))
                        {
                            e.Appearance.ForeColor = Color.Crimson;
                            e.Appearance.Font = new Font(e.Appearance.Font, FontStyle.Bold);
                        }
                    }

                    break;

                case "MAT_ID":

                    if (gvwData.GetRowCellValue(e.RowHandle, "MAT_ID").ToString().Equals("100"))
                    {
                        if (!gvwData.IsCellSelected(e.RowHandle, e.Column))
                        {
                            e.Appearance.ForeColor = Color.Crimson;
                            e.Appearance.Font = new Font(e.Appearance.Font, FontStyle.Bold);
                        }
                    }

                    break;

                case "MAT_NAME":

                    if (gvwData.GetRowCellValue(e.RowHandle, "MAT_NAME").ToString().Equals("PLACEHOLDER"))
                    {
                        if (!gvwData.IsCellSelected(e.RowHandle, e.Column))
                        {
                            e.Appearance.ForeColor = Color.Crimson;
                            e.Appearance.Font = new Font(e.Appearance.Font, FontStyle.Bold);
                        }
                    }

                    break;
            }
        }

        /// <summary>
        /// Delete the value of selected cells.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void gvwData_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Delete)
            {
                try
                {
                    gvwData.CellValueChanged -= new CellValueChangedEventHandler(gvwData_CellValueChanged);

                    foreach (GridCell cell in gvwData.GetSelectedCells())
                    {
                        if (!cell.Column.FieldName.Equals("ROLL_UP_LV"))
                            continue;

                        gvwData.SetRowCellValue(cell.RowHandle, cell.Column, "");
                    }
                }
                finally
                {
                    gvwData.CellValueChanged += new CellValueChangedEventHandler(gvwData_CellValueChanged);
                }
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void gvwData_CellValueChanged(object sender, CellValueChangedEventArgs e)
        {
            try
            {
                gvwData.CellValueChanged -= new CellValueChangedEventHandler(gvwData_CellValueChanged);

                if (e.Column.FieldName.Equals("ROLL_UP_LV"))
                {
                    foreach (int rowHandle in gvwData.GetSelectedRows())
                        gvwData.SetRowCellValue(rowHandle, "ROLL_UP_LV", gvwData.GetFocusedRowCellValue("ROLL_UP_LV").ToString());
                }
            }
            finally
            {
                gvwData.CellValueChanged += new CellValueChangedEventHandler(gvwData_CellValueChanged);
            }
        }

        #endregion

        #region Button Events.

        /// <summary>
        /// Replace existing BOM header with new one.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnMatching_Click(object sender, EventArgs e)
        {
            ArrayList list = new ArrayList();
            BOMHeader bomHeader = null;
            List<BOMData> bomData = null;
            OpenFileDialog dialog = new OpenFileDialog();

            dialog.Title = "Upload";
            dialog.Multiselect = false;
            dialog.RestoreDirectory = true;
            dialog.Filter = "JSON 파일|*.json";

            if (dialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                using (StreamReader file = File.OpenText(dialog.FileName))
                using (JsonTextReader reader = new JsonTextReader(file))
                {
                    JObject objs = JToken.ReadFrom(reader) as JObject;

                    if (objs.GetValue("BOM Header") == null || objs.GetValue("BOM Data") == null)
                    {
                        Common.ShowMessageBox("Invalid JSON format.", "E");
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

                    #region BOM Header.

                    PKG_INTG_BOM.INSERT_REPLACE_JSON_ORG_HEADER pkgInsert = new PKG_INTG_BOM.INSERT_REPLACE_JSON_ORG_HEADER();
                    pkgInsert.ARG_FACTORY = Factory;
                    pkgInsert.ARG_WS_NO = WSNumber;
                    pkgInsert.ARG_OBJ_ID = bomHeader.objectId;
                    pkgInsert.ARG_BOM_PART_UUID = bomHeader.bomPartUUID == null ? "" : bomHeader.bomPartUUID;
                    pkgInsert.ARG_OBJ_TYPE = bomHeader.objectType;
                    pkgInsert.ARG_BOM_CONTRACT_VER = bomHeader.bomContractVersion;
                    pkgInsert.ARG_DEV_STYLE_ID = bomHeader.developmentStyleIdentifier;
                    pkgInsert.ARG_DEV_COLORWAY_ID = bomHeader.developmentColorwayIdentifier;
                    pkgInsert.ARG_COLORWAY_NAME = bomHeader.colorwayName;
                    pkgInsert.ARG_SRC_CONFIG_ID = bomHeader.sourcingConfigurationIdentifier;
                    pkgInsert.ARG_SRC_CONFIG_NAME = bomHeader.sourcingConfigurationName;
                    pkgInsert.ARG_SEASON_ID = bomHeader.seasonIdentifier == null ? "" : bomHeader.seasonIdentifier;
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
                    pkgInsert.ARG_DEV_STYLE_TYPE_ID = "";
                    pkgInsert.ARG_LOGIC_BOM_STATE_ID = "";
                    pkgInsert.ARG_LOGIC_BOM_GATE_ID = "";
                    pkgInsert.ARG_CYCLE_YEAR = "";

                    list.Add(pkgInsert);

                    #endregion

                    #region BOM Data.

                    PKG_INTG_BOM.BOM_IMPORT pkgSelect = new PKG_INTG_BOM.BOM_IMPORT();
                    pkgSelect.ARG_WORK_TYPE = "isExisting";
                    pkgSelect.ARG_FACTORY = Factory;
                    pkgSelect.ARG_WS_NO = WSNumber;
                    pkgSelect.ARG_UPD_USER = Common.sessionID;
                    pkgSelect.OUT_CURSOR = string.Empty;

                    DataTable dt = Common.projectBaseForm.Exe_Select_PKG(pkgSelect).Tables[0];

                    if (dt.Rows[0][0].ToString().Equals("PASS"))
                    {
                        int partSeq = 1;

                        foreach (BOMData lineitem in bomData)
                        {
                            PKG_INTG_BOM.INSERT_BOM_TAIN_JSON_ORG pkgInsert2 = new PKG_INTG_BOM.INSERT_BOM_TAIN_JSON_ORG();
                            pkgInsert2.ARG_FACTORY = Factory;
                            pkgInsert2.ARG_WS_NO = WSNumber;
                            pkgInsert2.ARG_BOM_LINE_SORT_SEQ = lineitem.bomLineSortSequence;
                            pkgInsert2.ARG_BOM_SECTION_ID = lineitem.billOfMaterialsSectionIdentifier;
                            pkgInsert2.ARG_PART_NAME_ID = lineitem.partNameIdentifier;
                            pkgInsert2.ARG_PTRN_PART_ID = lineitem.patternPartIdentifier;
                            pkgInsert2.ARG_SUPP_MAT_ID = lineitem.suppliedMaterialIdentifier;
                            pkgInsert2.ARG_MAT_ITEM_ID = lineitem.materialItemIdentifier;
                            pkgInsert2.ARG_MAT_ITEM_PLHDR_DESC = lineitem.materialItemPlaceholderDescription;
                            pkgInsert2.ARG_COLOR_ID = lineitem.colorIdentifier;
                            pkgInsert2.ARG_COLOR_PLHDR_DESC = lineitem.colorPlaceholderDescription;
                            pkgInsert2.ARG_SUPP_MAT_COLOR_IS_MUL = lineitem.suppliedMaterialColorIsMultipleColors;
                            pkgInsert2.ARG_SUPP_MAT_COLOR_ID = lineitem.suppliedMaterialColorIdentifier;
                            pkgInsert2.ARG_BOM_LINEITEM_COMMENTS = lineitem.bomLineItemComments;
                            pkgInsert2.ARG_FAC_IN_HOUSE_IND = lineitem.factoryInHouseIndicator;
                            pkgInsert2.ARG_COUNTY_ORG_ID = lineitem.countyOfOriginIdentifier;
                            pkgInsert2.ARG_ACTUAL_DIMENSION_DESC = lineitem.actualDimensionDescription;
                            pkgInsert2.ARG_ISO_MEASUREMENT_CODE = lineitem.isoMeasurementCode;
                            pkgInsert2.ARG_NET_USAGE_NUMBER = lineitem.netUsageNumber;
                            pkgInsert2.ARG_WASTE_USAGE_NUMBER = lineitem.wasteUsageNumber;
                            pkgInsert2.ARG_PART_YIELD = lineitem.partYield;
                            pkgInsert2.ARG_CONSUM_CONVER_RATE = lineitem.consumptionConversionRate;
                            pkgInsert2.ARG_LINEITEM_DEFECT_PER_NUM = lineitem.lineItemDefectPercentNumber;
                            pkgInsert2.ARG_UNIT_PRICE_ISO_MSR_CODE = lineitem.unitPriceISOMeasurementCode;
                            pkgInsert2.ARG_CURRENCY_CODE = lineitem.currencyCode;
                            pkgInsert2.ARG_FACTORY_UNIT_PRICE = lineitem.factoryUnitPrice;
                            pkgInsert2.ARG_UNIT_PRICE_UPCHARGE_NUM = lineitem.unitPriceUpchargeNumber;
                            pkgInsert2.ARG_UNIT_PRICE_UPCHARGE_DESC = lineitem.unitPriceUpchargeDescription;
                            pkgInsert2.ARG_FREIGHT_TERM_ID = lineitem.freightTermIdentifier;
                            pkgInsert2.ARG_LANDED_COST_PER_NUM = lineitem.landedCostPercentNumber;
                            pkgInsert2.ARG_PCC_SORT_ORDER = lineitem.pccSortOrder;
                            pkgInsert2.ARG_ROLLUP_VARIATION_LV = lineitem.rollupVariationLevel == null ? "" : lineitem.rollupVariationLevel;
                            pkgInsert2.ARG_PART_SEQ = Convert.ToString(partSeq);
                            pkgInsert2.ARG_LOGIC_GROUP = "";
                            pkgInsert2.ARG_MAT_FORECAST_PRCNT = 0;
                            pkgInsert2.ARG_COLOR_FORECAST_PRCNT = 0;

                            list.Add(pkgInsert2);
                            partSeq++;
                        }
                    }

                    #endregion

                    if (Common.projectBaseForm.Exe_Modify_PKG(list) == null)
                    {
                        Common.ShowMessageBox("Failed to match the BOM header with new one.", "E");
                        return;
                    }
                }

                #region Backup

                //using (StreamReader reader = File.OpenText(fileName))
                //{
                //    using (JsonTextReader jsonReader = new JsonTextReader(reader))
                //    {
                //        JObject jObjects = (JObject)JToken.ReadFrom(jsonReader);

                //        DataSet cnvtObjects = ConvertJson2Xml(jObjects.ToString());

                //        DataTable bomHeader = cnvtObjects.Tables[0];
                //        DataTable bomData = null;

                //        // In case of no line items in the JSON BOM for TCO.
                //        if (cnvtObjects.Tables.Count > 1)
                //            bomData = cnvtObjects.Tables[1];

                //        DataRow headerValues = bomHeader.Rows[0];

                //        #region 원본 헤더 저장

                //        PKG_INTG_BOM.INSERT_REPLACE_JSON_ORG_HEADER pkgInsertOrgHead = new PKG_INTG_BOM.INSERT_REPLACE_JSON_ORG_HEADER();
                //        pkgInsertOrgHead.ARG_FACTORY = Factory;
                //        pkgInsertOrgHead.ARG_WS_NO = WSNumber;
                //        pkgInsertOrgHead.ARG_OBJ_ID = headerValues["objectId"].ToString();
                //        pkgInsertOrgHead.ARG_OBJ_TYPE = headerValues["objectType"].ToString();
                //        pkgInsertOrgHead.ARG_BOM_CONTRACT_VER = headerValues["bomContractVersion"].ToString();
                //        pkgInsertOrgHead.ARG_DEV_STYLE_ID = headerValues["developmentStyleIdentifier"].ToString();
                //        pkgInsertOrgHead.ARG_DEV_COLORWAY_ID = headerValues["developmentColorwayIdentifier"].ToString();
                //        pkgInsertOrgHead.ARG_COLORWAY_NAME = headerValues["colorwayName"].ToString();
                //        pkgInsertOrgHead.ARG_SRC_CONFIG_ID = headerValues["sourcingConfigurationIdentifier"].ToString();
                //        pkgInsertOrgHead.ARG_SRC_CONFIG_NAME = headerValues["sourcingConfigurationName"].ToString();
                //        pkgInsertOrgHead.ARG_BOM_ID = headerValues["bomIdentifier"].ToString();
                //        pkgInsertOrgHead.ARG_BOM_NAME = headerValues["bomName"].ToString();
                //        pkgInsertOrgHead.ARG_BOM_VERSION_NUM = headerValues["bomVersionNumber"].ToString();
                //        pkgInsertOrgHead.ARG_BOM_DESC = headerValues["bomDescription"].ToString();
                //        pkgInsertOrgHead.ARG_BOM_COMMENTS = headerValues["bomComments"].ToString();
                //        pkgInsertOrgHead.ARG_BOM_STATUS_IND = headerValues["billOfMaterialStatusIndicator"].ToString();
                //        pkgInsertOrgHead.ARG_CREATE_TIME_STAMP = headerValues["createTimestamp"].ToString();
                //        pkgInsertOrgHead.ARG_CHANGE_TIME_STAMP = headerValues["changeTimestamp"].ToString();
                //        pkgInsertOrgHead.ARG_CREATED_BY = headerValues["createdBy"].ToString();
                //        pkgInsertOrgHead.ARG_MODIFIED_BY = headerValues["modifiedBy"].ToString();
                //        pkgInsertOrgHead.ARG_STYLE_NUMBER = headerValues["styleNumber"].ToString();
                //        pkgInsertOrgHead.ARG_STYLE_NAME = headerValues["styleName"].ToString();
                //        pkgInsertOrgHead.ARG_MODEL_ID = headerValues["modelIdentifier"].ToString();
                //        pkgInsertOrgHead.ARG_GENDER = headerValues["gender"].ToString();
                //        pkgInsertOrgHead.ARG_AGE = headerValues["age"].ToString();
                //        pkgInsertOrgHead.ARG_PRODUCT_ID = headerValues["productId"].ToString();
                //        pkgInsertOrgHead.ARG_PRODUCT_CODE = headerValues["productCode"].ToString();
                //        pkgInsertOrgHead.ARG_COLORWAY_CODE = headerValues["colorwayCode"].ToString();
                //        pkgInsertOrgHead.ARG_DEV_STYLE_TYPE_ID = "";
                //        pkgInsertOrgHead.ARG_LOGIC_BOM_STATE_ID = "";
                //        pkgInsertOrgHead.ARG_LOGIC_BOM_GATE_ID = "";
                //        pkgInsertOrgHead.ARG_CYCLE_YEAR = "";

                //        ArrayList listHeader = new ArrayList();
                //        listHeader.Add(pkgInsertOrgHead);

                //        if (Common.projectBaseForm.Exe_Modify_PKG(listHeader) == null)
                //        {
                //            MessageBox.Show("Failed to save JSON Org. header.");
                //            return;
                //        }

                //        #endregion

                //        if (bomData != null)
                //        {
                //            #region 원본 라인 아이템 저장

                //            PKG_INTG_BOM.BOM_IMPORT pkgSelect = new PKG_INTG_BOM.BOM_IMPORT();
                //            pkgSelect.ARG_WORK_TYPE = "isExisting";
                //            pkgSelect.ARG_FACTORY = Factory;
                //            pkgSelect.ARG_WS_NO = WSNumber;
                //            pkgSelect.ARG_UPD_USER = Common.sessionID;
                //            pkgSelect.OUT_CURSOR = string.Empty;

                //            string flag = string.Empty;
                //            DataTable dtResult = Common.projectBaseForm.Exe_Select_PKG(pkgSelect).Tables[0];

                //            if (dtResult.Rows.Count > 0)
                //                flag = dtResult.Rows[0][0].ToString();

                //            if (flag == "PASS")
                //            {
                //                ArrayList list = new ArrayList();
                //                int partSeq = 1;

                //                foreach (DataRow dr in bomData.Rows)
                //                {
                //                    PKG_INTG_BOM.INSERT_BOM_TAIN_JSON_ORG pkgInsertOrgData = new PKG_INTG_BOM.INSERT_BOM_TAIN_JSON_ORG();
                //                    pkgInsertOrgData.ARG_FACTORY = Factory;
                //                    pkgInsertOrgData.ARG_WS_NO = WSNumber;
                //                    pkgInsertOrgData.ARG_BOM_LINE_SORT_SEQ = dr["bomLineSortSequence"].ToString();
                //                    pkgInsertOrgData.ARG_BOM_SECTION_ID = dr["billOfMaterialsSectionIdentifier"].ToString();
                //                    pkgInsertOrgData.ARG_PART_NAME_ID = dr["partNameIdentifier"].ToString();
                //                    pkgInsertOrgData.ARG_PTRN_PART_ID = dr["patternPartIdentifier"].ToString();
                //                    pkgInsertOrgData.ARG_SUPP_MAT_ID = dr["suppliedMaterialIdentifier"].ToString();
                //                    pkgInsertOrgData.ARG_MAT_ITEM_ID = dr["materialItemIdentifier"].ToString();
                //                    pkgInsertOrgData.ARG_MAT_ITEM_PLHDR_DESC = dr["materialItemPlaceholderDescription"].ToString();
                //                    pkgInsertOrgData.ARG_COLOR_ID = dr["colorIdentifier"].ToString();
                //                    pkgInsertOrgData.ARG_COLOR_PLHDR_DESC = dr["colorPlaceholderDescription"].ToString();
                //                    pkgInsertOrgData.ARG_SUPP_MAT_COLOR_IS_MUL = dr["suppliedMaterialColorIsMultipleColors"].ToString();
                //                    pkgInsertOrgData.ARG_SUPP_MAT_COLOR_ID = dr["suppliedMaterialColorIdentifier"].ToString();
                //                    pkgInsertOrgData.ARG_BOM_LINEITEM_COMMENTS = dr["bomLineItemComments"].ToString();
                //                    pkgInsertOrgData.ARG_FAC_IN_HOUSE_IND = dr["factoryInHouseIndicator"].ToString();
                //                    pkgInsertOrgData.ARG_COUNTY_ORG_ID = dr["countyOfOriginIdentifier"].ToString();
                //                    pkgInsertOrgData.ARG_ACTUAL_DIMENSION_DESC = dr["actualDimensionDescription"].ToString();
                //                    pkgInsertOrgData.ARG_ISO_MEASUREMENT_CODE = dr["isoMeasurementCode"].ToString();
                //                    pkgInsertOrgData.ARG_NET_USAGE_NUMBER = dr["netUsageNumber"].ToString();
                //                    pkgInsertOrgData.ARG_WASTE_USAGE_NUMBER = dr["wasteUsageNumber"].ToString();
                //                    pkgInsertOrgData.ARG_PART_YIELD = dr["partYield"].ToString();
                //                    pkgInsertOrgData.ARG_CONSUM_CONVER_RATE = dr["consumptionConversionRate"].ToString();
                //                    pkgInsertOrgData.ARG_LINEITEM_DEFECT_PER_NUM = dr["lineItemDefectPercentNumber"].ToString();
                //                    pkgInsertOrgData.ARG_UNIT_PRICE_ISO_MSR_CODE = dr["unitPriceISOMeasurementCode"].ToString();
                //                    pkgInsertOrgData.ARG_CURRENCY_CODE = dr["currencyCode"].ToString();
                //                    pkgInsertOrgData.ARG_FACTORY_UNIT_PRICE = dr["factoryUnitPrice"].ToString();
                //                    pkgInsertOrgData.ARG_UNIT_PRICE_UPCHARGE_NUM = dr["unitPriceUpchargeNumber"].ToString();
                //                    pkgInsertOrgData.ARG_UNIT_PRICE_UPCHARGE_DESC = dr["unitPriceUpchargeDescription"].ToString();
                //                    pkgInsertOrgData.ARG_FREIGHT_TERM_ID = dr["freightTermIdentifier"].ToString();
                //                    pkgInsertOrgData.ARG_LANDED_COST_PER_NUM = dr["landedCostPercentNumber"].ToString();
                //                    pkgInsertOrgData.ARG_PCC_SORT_ORDER = dr["pccSortOrder"].ToString();
                //                    pkgInsertOrgData.ARG_ROLLUP_VARIATION_LV = dr["rollupVariationLevel"].ToString();
                //                    pkgInsertOrgData.ARG_PART_SEQ = Convert.ToString(partSeq);
                //                    pkgInsertOrgData.ARG_LOGIC_GROUP = "";
                //                    pkgInsertOrgData.ARG_MAT_FORECAST_PRCNT = 0;
                //                    pkgInsertOrgData.ARG_COLOR_FORECAST_PRCNT = 0;

                //                    list.Add(pkgInsertOrgData);
                //                    partSeq++;
                //                }

                //                if (Common.projectBaseForm.Exe_Modify_PKG(list) == null)
                //                {
                //                    MessageBox.Show("Failed to save JSON Org. Data");
                //                    return;
                //                }
                //            }

                //            #endregion
                //        }
                //    }
                //}

                #endregion

                BindDataSourceGridView("Header");
                MessageBox.Show("Complete");
            }
        }

        /// <summary>
        /// Export 기능 수행
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnExport_Click(object sender, EventArgs e)
        {
            string[] requiredFields = new string[] { "DEV_STYLE_ID", "DEV_COLORWAY_ID", "SRC_CONFIG_ID", "BOM_ID" };
            SaveFileDialog sDialog = new SaveFileDialog();
            DataTable dtCaption = GetDataSource("BOMCaption");
            DataRow rowHeader = gvwHeader.GetDataRow(0);

            if (dtCaption != null)
            {
                sDialog.FileName = string.Format("{0}_{1}_{2}_{3}",
                    dtCaption.Rows[0]["SEASON"].ToString(),
                    dtCaption.Rows[0]["SAMPLE_TYPE"].ToString(),
                    dtCaption.Rows[0]["DEV_NAME"].ToString(),
                    dtCaption.Rows[0]["DEV_COLORWAY_ID"].ToString()
                    );
            }

            sDialog.Title = "파일 저장 경로 지정";
            sDialog.DefaultExt = "json";
            sDialog.Filter = "JSON 파일(*.json)|*.json";

            Func<string, bool> IsEmpty = (fieldName) =>
            {
                if (rowHeader[fieldName].ToString().Equals(""))
                    return true;
                else
                    return false;
            };

            // Validate Required fields were entered.
            foreach (string fieldName in requiredFields)
            {
                if (IsEmpty(fieldName))
                {
                    Common.ShowMessageBox("Required data is empty. ", "E");
                    return;
                }
            }

            if (sDialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                var jObject = new JObject();
                var header = new JObject();

                header.Add("objectId", rowHeader["OBJ_ID"].ToString());
                header.Add("objectType", rowHeader["OBJ_TYPE"].ToString());
                header.Add("bomContractVersion", rowHeader["BOM_CONTRACT_VER"].ToString());
                header.Add("developmentStyleIdentifier", rowHeader["DEV_STYLE_ID"].ToString());
                header.Add("developmentColorwayIdentifier", rowHeader["DEV_COLORWAY_ID"].ToString());
                header.Add("colorwayName", rowHeader["COLORWAY_NAME"].ToString());
                header.Add("sourcingConfigurationIdentifier", rowHeader["SRC_CONFIG_ID"].ToString());
                header.Add("sourcingConfigurationName", rowHeader["SRC_CONFIG_NAME"].ToString());
                header.Add("bomIdentifier", rowHeader["BOM_ID"].ToString());
                header.Add("bomName", rowHeader["BOM_NAME"].ToString());
                header.Add("bomVersionNumber", rowHeader["BOM_VERSION_NUM"].ToString());
                header.Add("bomDescription", rowHeader["BOM_DESC"].ToString());
                header.Add("bomComments", rowHeader["BOM_COMMENTS"].ToString());
                header.Add("billOfMaterialStatusIndicator", rowHeader["BOM_STATUS_IND"].ToString());
                header.Add("createTimestamp", rowHeader["CREATE_TIME_STAMP"].ToString());
                header.Add("changeTimestamp", rowHeader["CHANGE_TIME_STAMP"].ToString());
                header.Add("createdBy", rowHeader["CREATED_BY"].ToString());
                header.Add("modifiedBy", rowHeader["MODIFIED_BY"].ToString());
                header.Add("styleNumber", rowHeader["STYLE_NUMBER"].ToString());
                header.Add("styleName", rowHeader["STYLE_NAME"].ToString());
                header.Add("modelIdentifier", rowHeader["MODEL_ID"].ToString());
                header.Add("gender", rowHeader["GENDER"].ToString());
                header.Add("age", rowHeader["AGE"].ToString());
                header.Add("productId", rowHeader["PRODUCT_ID"].ToString());
                header.Add("productCode", rowHeader["PRODUCT_CODE"].ToString());
                header.Add("colorwayCode", rowHeader["COLORWAY_CODE"].ToString());

                var array = new JArray();
                string colorName = string.Empty;
                bool isMultiColor = false;
                DataRow row;

                for (int rowHandle = 0; rowHandle < gvwData.RowCount; rowHandle++)
                {
                    var lineItem = new JObject();
                    row = gvwData.GetDataRow(rowHandle);

                    lineItem.Add("bomLineSortSequence", row["PCC_SORT_NO"].ToString());
                    lineItem.Add("billOfMaterialsSectionIdentifier", ConvertTypeToID(row["PART_TYPE"].ToString()));
                    lineItem.Add("partNameIdentifier", row["PART_ID"].ToString());
                    lineItem.Add("patternPartIdentifier", row["PTRN_PART_ID"].ToString());
                    lineItem.Add("suppliedMaterialIdentifier", row["SUPP_MAT_ID"].ToString());
                    lineItem.Add("materialItemIdentifier", row["MAT_ID"].ToString());
                    lineItem.Add("materialItemPlaceholderDescription", "");
                    lineItem.Add("colorIdentifier", row["PCX_COLOR_ID"].ToString());
                    lineItem.Add("colorPlaceholderDescription", "");

                    colorName = row["COLOR_NAME"].ToString();

                    if (colorName != "")
                    {
                        if (colorName.Contains("MC-"))
                            isMultiColor = true;
                    }

                    lineItem.Add("suppliedMaterialColorIsMultipleColors", isMultiColor ? "true" : "false");
                    lineItem.Add("suppliedMaterialColorIdentifier", "");
                    lineItem.Add("bomLineItemComments", row["PCX_COMMENT"].ToString());
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
                    lineItem.Add("rollupVariationLevel", row["ROLL_UP_LV"].ToString());

                    array.Add(lineItem);
                }

                jObject.Add("BOM Header", header);
                jObject.Add("BOM Data", array);

                System.IO.File.WriteAllText(sDialog.FileName, jObject.ToString());
                MessageBox.Show("Complete.");
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnExportSRT_Click(object sender, EventArgs e)
        {
            string[] requiredFields = new string[] { "BOM_PART_UUID", "DEV_STYLE_ID", "DEV_COLORWAY_ID", "SRC_CONFIG_ID", "BOM_ID" };
            SaveFileDialog sDialog = new SaveFileDialog();
            DataTable dtCaption = GetDataSource("BOMCaption");
            DataRow rowHeader = gvwHeader.GetDataRow(0);

            if (dtCaption != null)
            {
                sDialog.FileName = string.Format("{0}_{1}_{2}_{3}",
                    dtCaption.Rows[0]["SEASON"].ToString(),
                    dtCaption.Rows[0]["SAMPLE_TYPE"].ToString(),
                    dtCaption.Rows[0]["DEV_NAME"].ToString(),
                    dtCaption.Rows[0]["DEV_COLORWAY_ID"].ToString()
                    );
            }

            sDialog.Title = "파일 저장 경로 지정";
            sDialog.DefaultExt = "json";
            sDialog.Filter = "JSON 파일(*.json)|*.json";

            Func<string, bool> IsEmpty = (fieldName) =>
            {
                if (rowHeader[fieldName].ToString().Equals(""))
                    return true;
                else
                    return false;
            };

            // Validate Required fields were entered.
            foreach (string fieldName in requiredFields)
            {
                if (IsEmpty(fieldName))
                {
                    Common.ShowMessageBox("Required data is empty. ", "E");
                    return;
                }
            }

            if (sDialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                var jObject = new JObject();
                var header = new JObject();

                header.Add("objectId", rowHeader["OBJ_ID"].ToString());
                header.Add("bomPartUUID", rowHeader["BOM_PART_UUID"].ToString());   // Gemini.
                header.Add("objectType", rowHeader["OBJ_TYPE"].ToString());
                header.Add("bomContractVersion", rowHeader["BOM_CONTRACT_VER"].ToString());
                header.Add("developmentStyleIdentifier", rowHeader["DEV_STYLE_ID"].ToString());
                header.Add("developmentColorwayIdentifier", rowHeader["DEV_COLORWAY_ID"].ToString());
                header.Add("colorwayName", rowHeader["COLORWAY_NAME"].ToString());
                header.Add("sourcingConfigurationIdentifier", rowHeader["SRC_CONFIG_ID"].ToString());
                header.Add("sourcingConfigurationName", rowHeader["SRC_CONFIG_NAME"].ToString());
                header.Add("seasonIdentifier", rowHeader["SEASON_ID"].ToString());  // Gemini.
                header.Add("bomIdentifier", rowHeader["BOM_ID"].ToString());
                header.Add("bomName", rowHeader["BOM_NAME"].ToString());
                header.Add("bomVersionNumber", rowHeader["BOM_VERSION_NUM"].ToString());
                header.Add("bomDescription", rowHeader["BOM_DESC"].ToString());
                header.Add("bomComments", rowHeader["BOM_COMMENTS"].ToString());
                header.Add("billOfMaterialStatusIndicator", rowHeader["BOM_STATUS_IND"].ToString());
                header.Add("createTimestamp", rowHeader["CREATE_TIME_STAMP"].ToString());
                header.Add("changeTimestamp", rowHeader["CHANGE_TIME_STAMP"].ToString());
                header.Add("createdBy", rowHeader["CREATED_BY"].ToString());
                header.Add("modifiedBy", rowHeader["MODIFIED_BY"].ToString());
                header.Add("styleNumber", rowHeader["STYLE_NUMBER"].ToString());
                header.Add("styleName", rowHeader["STYLE_NAME"].ToString());
                header.Add("modelIdentifier", rowHeader["MODEL_ID"].ToString());
                header.Add("gender", rowHeader["GENDER"].ToString());
                header.Add("age", rowHeader["AGE"].ToString());
                header.Add("productId", rowHeader["PRODUCT_ID"].ToString());
                header.Add("productCode", rowHeader["PRODUCT_CODE"].ToString());
                header.Add("colorwayCode", rowHeader["COLORWAY_CODE"].ToString());

                var array = new JArray();
                string colorName = string.Empty;
                bool isMultiColor = false;
                DataRow row;

                for (int rowHandle = 0; rowHandle < gvwData.RowCount; rowHandle++)
                {
                    var lineItem = new JObject();
                    row = gvwData.GetDataRow(rowHandle);

                    lineItem.Add("bomLineSortSequence", row["PCC_SORT_NO"].ToString());
                    lineItem.Add("billOfMaterialsSectionIdentifier", ConvertTypeToID(row["PART_TYPE"].ToString()));
                    lineItem.Add("billOfMaterialsLineItemUUID", "");
                    lineItem.Add("partNameIdentifier", row["PART_ID"].ToString());
                    lineItem.Add("patternPartIdentifier", row["PTRN_PART_ID"].ToString());
                    lineItem.Add("suppliedMaterialIdentifier", row["SUPP_MAT_ID"].ToString());
                    lineItem.Add("materialItemIdentifier", row["MAT_ID"].ToString());
                    lineItem.Add("materialItemPlaceholderDescription", "");
                    lineItem.Add("colorIdentifier", row["PCX_COLOR_ID"].ToString());
                    lineItem.Add("colorPlaceholderDescription", "");

                    colorName = row["COLOR_NAME"].ToString();

                    if (colorName != "")
                    {
                        if (colorName.Contains("MC-"))
                            isMultiColor = true;
                    }

                    lineItem.Add("suppliedMaterialColorIsMultipleColors", isMultiColor ? "true" : "false");
                    lineItem.Add("suppliedMaterialColorIdentifier", "");
                    lineItem.Add("bomLineItemComments", row["PCX_COMMENT"].ToString());
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

                    array.Add(lineItem);
                }

                jObject.Add("BOM Header", header);
                jObject.Add("BOM Data", array);

                System.IO.File.WriteAllText(sDialog.FileName, jObject.ToString());
                MessageBox.Show("Complete.");
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="partType"></param>
        /// <returns></returns>
        private string ConvertTypeToID(string partType)
        {
            switch (partType)
            {
                case "UPPER":
                    return "1";

                case "MIDSOLE":
                    return "2";

                case "OUTSOLE":
                    return "3";

                case "AIRBAG":
                    return "4";

                case "PACKAGING":
                    return "5";

                default:
                    return "";
            }
        }

        #endregion

        #region User Defined Functions.

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
            pkgSelect.ARG_WS_NO = WSNumber;
            pkgSelect.ARG_UPD_USER = Common.sessionID;
            pkgSelect.OUT_CURSOR = string.Empty;

            DataTable dataSource = Common.projectBaseForm.Exe_Select_PKG(pkgSelect).Tables[0];

            if (dataSource.Rows.Count > 0)
                return dataSource;
            else
                return null;
        }

        /// <summary>
        /// 소스 데이터를 타입에 맞는 그리드에 바인딩
        /// </summary>
        /// <param name="type"></param>
        private void BindDataSourceGridView(string type)
        {
            DataTable dataSource;

            if (type.Equals("Header"))
            {
                dataSource = GetDataSource("HeadInfo");

                if (dataSource.Rows.Count > 0)
                {
                    dataSource.Columns["OBJ_ID"].MaxLength = 30;
                    dataSource.Columns["OBJ_TYPE"].MaxLength = 30;
                    dataSource.Columns["BOM_CONTRACT_VER"].MaxLength = 20;
                    dataSource.Columns["SRC_CONFIG_NAME"].MaxLength = 100;
                    dataSource.Columns["BOM_NAME"].MaxLength = 50;
                    dataSource.Columns["BOM_VERSION_NUM"].MaxLength = 50;
                    dataSource.Columns["BOM_DESC"].MaxLength = 2000;
                    dataSource.Columns["BOM_COMMENTS"].MaxLength = 2000;
                    dataSource.Columns["BOM_STATUS_IND"].MaxLength = 10;
                    dataSource.Columns["CREATE_TIME_STAMP"].MaxLength = 100;
                    dataSource.Columns["CHANGE_TIME_STAMP"].MaxLength = 100;
                    dataSource.Columns["CREATED_BY"].MaxLength = 100;
                    dataSource.Columns["MODIFIED_BY"].MaxLength = 100;
                    dataSource.Columns["GENDER"].MaxLength = 5;
                    dataSource.Columns["AGE"].MaxLength = 5;
                    dataSource.Columns["COLORWAY_CODE"].MaxLength = 10;

                    grdHeader.DataSource = dataSource;
                }
                else
                {
                    Common.ShowMessageBox("No data.", "E");
                    this.Close();
                }
            }
            else if (type.Equals("Data"))
                grdData.DataSource = GetDataSource("BOMData");
        }

        #endregion
    }
}