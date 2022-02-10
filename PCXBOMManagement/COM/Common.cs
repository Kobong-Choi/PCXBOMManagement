using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Data;                  // DataTable 선언
using System.Drawing;
using System.Diagnostics;           // Process

using CSI.Client.ProjectBaseForm;   // ProjectBaseForm 선언 
using CSI.PCC.PCX.PACKAGE;

using DevExpress.XtraEditors;
using DevExpress.XtraGrid.Views.Grid;   // GridView Event
using DevExpress.XtraGrid.Views.Base;
using DevExpress.XtraGrid.Columns;
using DevExpress.XtraGrid;
using System.Net.Mail;
using System.Net;
using System.Net.Mime;

namespace CSI.PCC.PCX.COM
{
    // Don't need to have instance member -> static.
    // Define common method and class to be used frequently.
    static class Common
    {
        // Common Variables.
        public static string sessionID = "";
        public static string sessionFactory = "";
        public static ProjectBaseForm projectBaseForm;
        public static GridView viewPCXBOMManagement;
        public static GridControl controlPCXBOMManagement;
        public static string[] adminUser = new string[] { "kobong.choi", "james.jung", "chulho.shin", "hyeongseung.shin", "cheol.hwang", "kunwoong.kim", "jerry.hwang" };
        public static string[] categorySU = new string[] { "kobong.choi", "jerry.hwang", "james.jung", "jason.noh", "hailey.lee", "richard.park", "hyungsun.yoon", "so.yeo" };
        public static string[] CSFactoryCodes = new string[] { "DS", "VJ", "JJ", "QD", "RJ" };

        /// <summary>
        /// Bind datasource to lookUpEdit.
        /// </summary>
        /// <param name="edit"></param>
        /// <param name="factoryType"></param>
        /// <param name="hasAllRow"></param>
        public static void BindLookUpEdit(string workType, LookUpEdit edit, bool hasAllRow, string useYN)
        {
            DataTable dataSource = new DataTable();

            if (workType == "Factory")
            {
                PKG_INTG_BOM_COMMON.SELECT_FACTORY pkgSelect = new PKG_INTG_BOM_COMMON.SELECT_FACTORY();
                pkgSelect.ARG_FACTORY_TYPE = "PCC";
                pkgSelect.OUT_CURSOR = string.Empty;

                dataSource = projectBaseForm.Exe_Select_PKG(pkgSelect).Tables[0];
            }
            else if (workType == "Season")
            {
                PKG_INTG_BOM_COMMON.SELECT_SEASON pkgSelect = new PKG_INTG_BOM_COMMON.SELECT_SEASON();
                pkgSelect.ARG_GROUP_CODE = "SEASON";
                pkgSelect.OUT_CURSOR = string.Empty;

                dataSource = projectBaseForm.Exe_Select_PKG(pkgSelect).Tables[0];
            }
            else if (workType == "SampleType")
            {
                PKG_INTG_BOM_COMMON.SELECT_SAMPLE_TYPE pkgSelect = new PKG_INTG_BOM_COMMON.SELECT_SAMPLE_TYPE();
                pkgSelect.ARG_FACTORY = sessionFactory;
                pkgSelect.ARG_ST_DIV = "B";
                pkgSelect.ARG_USE_YN = useYN;
                pkgSelect.OUT_CURSOR = string.Empty;

                dataSource = projectBaseForm.Exe_Select_PKG(pkgSelect).Tables[0];
            }
            else if (workType == "SubType")
            {
                PKG_INTG_BOM_COMMON.SELECT_SUB_SAMPLE_TYPE pkgSelect = new PKG_INTG_BOM_COMMON.SELECT_SUB_SAMPLE_TYPE();
                pkgSelect.ARG_ST_DIV = "S";
                pkgSelect.OUT_CURSOR = string.Empty;

                dataSource = projectBaseForm.Exe_Select_PKG(pkgSelect).Tables[0];
            }
            else if (workType == "Developer")
            {
                PKG_INTG_BOM_COMMON.SELECT_DEVELOPER pkgSelect = new PKG_INTG_BOM_COMMON.SELECT_DEVELOPER();
                pkgSelect.ARG_FACTORY = sessionFactory;
                pkgSelect.OUT_CURSOR = string.Empty;

                dataSource = projectBaseForm.Exe_Select_PKG(pkgSelect).Tables[0];
            }
            else if (workType == "Category")
            {
                PKG_INTG_BOM_COMMON.SELECT_CATEGORY pkgSelect = new PKG_INTG_BOM_COMMON.SELECT_CATEGORY();
                pkgSelect.ARG_GROUP_CODE = "BOM_CAT";
                pkgSelect.ARG_CODE_VALUE = "WMT";
                pkgSelect.OUT_CURSOR = string.Empty;

                dataSource = projectBaseForm.Exe_Select_PKG(pkgSelect).Tables[0];
            }
            else if (workType == "TD")
            {
                PKG_INTG_BOM_COMMON.SELECT_TD pkgSelect = new PKG_INTG_BOM_COMMON.SELECT_TD();
                pkgSelect.ARG_GROUP_CODE = "TD";
                pkgSelect.OUT_CURSOR = string.Empty;

                dataSource = projectBaseForm.Exe_Select_PKG(pkgSelect).Tables[0];
            }
            else if (workType == "Gender")
            {
                PKG_INTG_BOM_COMMON.SELECT_GENDER pkgSelect = new PKG_INTG_BOM_COMMON.SELECT_GENDER();
                pkgSelect.ARG_ATTRIBUTE_NAME = "Gender";
                pkgSelect.OUT_CURSOR = string.Empty;

                dataSource = projectBaseForm.Exe_Select_PKG(pkgSelect).Tables[0];
            }

            if (hasAllRow)
                edit.Properties.DataSource = InsertRowForAllSearch(dataSource);
            else
                edit.Properties.DataSource = dataSource;

            // Set default editValue.
            switch (workType)
            {
                case "Factory":
                    {
                        edit.EditValue = sessionFactory;
                        break;
                    }

                case "Developer":
                    {
                        edit.EditValue = sessionID;

                        if (edit.Text == "")
                            edit.ItemIndex = 0;

                        break;
                    }

                default:
                    {
                        edit.EditValue = "";
                        break;
                    }
            }
        }

        /// <summary>
        /// Bind dataSource to CheckedComboBoxEdit.
        /// </summary>
        /// <param name="workType"></param>
        /// <param name="edit"></param>
        /// <param name="hasAllRow"></param>
        /// <param name="useYN"></param>
        public static void BindChkComboEdit(string workType, CheckedComboBoxEdit edit, bool hasAllRow, string useYN)
        {
            DataTable dataSource = new DataTable();

            if (workType == "Season")
            {
                PKG_INTG_BOM_COMMON.SELECT_SEASON pkgSelect = new PKG_INTG_BOM_COMMON.SELECT_SEASON();
                pkgSelect.ARG_GROUP_CODE = "SEASON";
                pkgSelect.OUT_CURSOR = string.Empty;

                dataSource = projectBaseForm.Exe_Select_PKG(pkgSelect).Tables[0];
            }
            else if (workType == "SampleType")
            {
                PKG_INTG_BOM_COMMON.SELECT_SAMPLE_TYPE pkgSelect = new PKG_INTG_BOM_COMMON.SELECT_SAMPLE_TYPE();
                pkgSelect.ARG_FACTORY = sessionFactory;
                pkgSelect.ARG_ST_DIV = "B";
                pkgSelect.ARG_USE_YN = useYN;
                pkgSelect.OUT_CURSOR = string.Empty;

                dataSource = projectBaseForm.Exe_Select_PKG(pkgSelect).Tables[0];
            }

            if (hasAllRow)
                edit.Properties.DataSource = InsertRowForAllSearch(dataSource);
            else
                edit.Properties.DataSource = dataSource;

            //edit.SetEditValue(dataSource.Rows[0]["CODE"].ToString());
        }

        /// <summary>
        /// Add a row for searching 'ALL' at the top.
        /// </summary>
        /// <param name="dataSource"></param>
        /// <returns></returns>
        private static DataTable InsertRowForAllSearch(DataTable dataSource)
        {
            DataRow row = dataSource.NewRow();

            row[0] = "";
            row[1] = "ALL";

            dataSource.Rows.InsertAt(row, 0);

            return dataSource;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="objIn"></param>
        /// <returns></returns>
        public static string NulltoString(object objIn)
        {
            return objIn == null ? "" : objIn.ToString().Trim();
        }

        /// <summary>
        /// BOM의 Caption 정보를 로드한다.
        /// </summary>
        /// <param name="workType"></param>
        /// <param name="factory"></param>
        /// <param name="wsNo"></param>
        /// <returns></returns>
        public static string LoadBOMCaption(string factory, string wsNo)
        {
            string caption = string.Empty;

            PKG_INTG_BOM.SELECT_BOM_INFO_BY_CONFIRM pkgSelect = new PKG_INTG_BOM.SELECT_BOM_INFO_BY_CONFIRM();
            pkgSelect.ARG_WORK_TYPE = "BOMCaption";
            pkgSelect.ARG_FACTORY = factory;
            pkgSelect.ARG_WS_NO = wsNo;
            pkgSelect.ARG_PART_SEQ = "";
            pkgSelect.OUT_CURSOR = string.Empty;

            DataTable dataSource = projectBaseForm.Exe_Select_PKG(pkgSelect).Tables[0];

            if (dataSource != null)
            {
                string category = dataSource.Rows[0]["CATEGORY"].ToString();
                string season = dataSource.Rows[0]["SEASON"].ToString();
                string sampleType = dataSource.Rows[0]["SAMPLE_TYPE"].ToString();
                string sampleTypeCode = dataSource.Rows[0]["ST_CD"].ToString();
                string subType = dataSource.Rows[0]["SUB_SAMPLE_TYPE"].ToString();
                string subTypeCode = dataSource.Rows[0]["SUB_ST_CD"].ToString();
                string devName = dataSource.Rows[0]["DEV_NAME"].ToString();
                string devStyleNumber = dataSource.Rows[0]["DEV_STYLE_NUMBER"].ToString();
                string colorwayId = dataSource.Rows[0]["DEV_COLORWAY_ID"].ToString();

                caption = season + "_" + devName.Replace("/", "") + "_" + devStyleNumber + "_" +
                        sampleType + "_" + colorwayId;
            }

            return caption;
        }

        /// <summary>
        /// 메인 화면의 포커스된 행의 데이터를 초기화 한다.
        /// </summary>
        /// <param name="view"></param>
        /// <param name="rowHandle"></param>
        public static void RefreshHeaderInfo(GridView view, int rowHandle)
        {
            DataTable dataSource = null;
            string value = string.Empty;

            if (view.RowCount <= rowHandle)
                return;

            PKG_INTG_BOM.SELECT_BOM_HEADER_REFRESH pkgSelect = new PKG_INTG_BOM.SELECT_BOM_HEADER_REFRESH();
            pkgSelect.ARG_FACTORY = view.GetRowCellValue(rowHandle, "FACTORY").ToString();
            pkgSelect.ARG_WS_NO = view.GetRowCellValue(rowHandle, "WS_NO").ToString();
            pkgSelect.OUT_CURSOR = string.Empty;

            dataSource = projectBaseForm.Exe_Select_PKG(pkgSelect).Tables[0];

            foreach (GridColumn column in view.Columns)
            {
                value = dataSource.Rows[0][column.FieldName].ToString();

                if (value == "")
                    continue;

                view.SetRowCellValue(rowHandle, column, value);
            }

            ((DataTable)Common.controlPCXBOMManagement.DataSource).AcceptChanges();
        }

        /// <summary>
        /// 수정이 필요한 셀 포커싱 및 에딧모드 진입
        /// </summary>
        /// <param name="view"></param>
        /// <param name="rowHandle"></param>
        /// <param name="fieldName"></param>
        public static void FocusCell(GridView view, int rowHandle, string fieldName, bool isShown)
        {
            try
            {
                view.ClearSelection();
                view.SelectCell(rowHandle, view.Columns[fieldName]);
                view.FocusedRowHandle = rowHandle;
                view.FocusedColumn = view.Columns[fieldName];

                if (isShown == true)
                    view.ShowEditor();
            }
            catch (Exception ex)
            {
                projectBaseForm.MessageBoxW(ex.ToString());
                return;
            }
        }

        /// <summary>
        /// Automatically set properties which should be input by NCF rule.
        /// Version for DataRow
        /// </summary>
        /// <param name="view"></param>
        /// <param name="rowHandle"></param>
        /// <param name="pcxPartId"></param>
        public static void BindDefaultMaterialByNCFRule(GridView view, DataRow row, string pcxPartId)
        {
            if (pcxPartId == "1369")
            {
                /* THREAD */

                //view.SetRowCellValue(rowHandle, "MXSXL_NUMBER", "226640.716.1");
                //view.SetRowCellValue(rowHandle, "CS_CD", "0");
                //view.SetRowCellValue(rowHandle, "MAT_CD", "226640");
                //view.SetRowCellValue(rowHandle, "PCX_SUPP_MAT_ID", "149445");
                //view.SetRowCellValue(rowHandle, "PCX_MAT_ID", "83694");
                //view.SetRowCellValue(rowHandle, "MAT_NAME", "UPPER STITCHING THREAD AVG. PRICE");
                //view.SetRowCellValue(rowHandle, "VENDOR_NAME", "FACTORY IN-HOUSE MANUFACTURING");

                row["MXSXL_NUMBER"] = "226640.716.1";
                row["CS_CD"] = "0";
                row["MAT_CD"] = "226640";
                row["PCX_SUPP_MAT_ID"] = "149445";
                row["PCX_MAT_ID"] = "83694";
                row["MAT_NAME"] = "UPPER STITCHING THREAD AVG. PRICE";
                row["VENDOR_NAME"] = "FACTORY IN-HOUSE MANUFACTURING";
            }
            else if (pcxPartId == "3391")
            {
                /* UPPER CEMENT */

                //view.SetRowCellValue(rowHandle, "MXSXL_NUMBER", "226624.716.1");
                //view.SetRowCellValue(rowHandle, "CS_CD", "0");
                //view.SetRowCellValue(rowHandle, "MAT_CD", "226624");
                //view.SetRowCellValue(rowHandle, "PCX_SUPP_MAT_ID", "138316");
                //view.SetRowCellValue(rowHandle, "PCX_MAT_ID", "78312");
                //view.SetRowCellValue(rowHandle, "MAT_NAME", "UPPER CEMENT");
                //view.SetRowCellValue(rowHandle, "VENDOR_NAME", "FACTORY IN-HOUSE MANUFACTURING");

                row["MXSXL_NUMBER"] = "226624.716.1";
                row["CS_CD"] = "0";
                row["MAT_CD"] = "226624";
                row["PCX_SUPP_MAT_ID"] = "138316";
                row["PCX_MAT_ID"] = "78312";
                row["MAT_NAME"] = "UPPER CEMENT";
                row["VENDOR_NAME"] = "FACTORY IN-HOUSE MANUFACTURING";
            }
            else if (pcxPartId == "1311")
            {
                /* OUTER CARTON */

                //view.SetRowCellValue(rowHandle, "MXSXL_NUMBER", "2136.2056.1");
                //view.SetRowCellValue(rowHandle, "CS_CD", "0");
                //view.SetRowCellValue(rowHandle, "MCS_NUMBER", "BX/ 002");
                //view.SetRowCellValue(rowHandle, "MAT_CD", "2136");
                //view.SetRowCellValue(rowHandle, "PCX_SUPP_MAT_ID", "101347");
                //view.SetRowCellValue(rowHandle, "PCX_MAT_ID", "60268");
                //view.SetRowCellValue(rowHandle, "MAT_NAME", "GENERIC OUTER CARTON");
                //view.SetRowCellValue(rowHandle, "VENDOR_NAME", "OIA PACKAGING");
                //view.SetRowCellValue(rowHandle, "PART_TYPE", "PACKAGING");

                row["MXSXL_NUMBER"] = "2136.2056.1";
                row["CS_CD"] = "0";
                row["MCS_NUMBER"] = "BX/ 002";
                row["MAT_CD"] = "2136";
                row["PCX_SUPP_MAT_ID"] = "101347";
                row["PCX_MAT_ID"] = "60268";
                row["MAT_NAME"] = "GENERIC OUTER CARTON";
                row["VENDOR_NAME"] = "OIA PACKAGING";
                row["PART_TYPE"] = "PACKAGING";
            }
        }

        /// <summary>
        /// Automatically set properties which should be input by NCF rule.
        /// Version for RowHandle
        /// </summary>
        /// <param name="view"></param>
        /// <param name="rowHandle"></param>
        /// <param name="pcxPartId"></param>
        public static void BindDefaultMaterialByNCFRule(GridView view, int rowHandle, string pcxPartId)
        {
            if (pcxPartId == "1369")
            {
                /* THREAD */

                view.SetRowCellValue(rowHandle, "MXSXL_NUMBER", "226640.716.1");
                view.SetRowCellValue(rowHandle, "CS_CD", "0");
                view.SetRowCellValue(rowHandle, "MAT_CD", "226640");
                view.SetRowCellValue(rowHandle, "PCX_SUPP_MAT_ID", "149445");
                view.SetRowCellValue(rowHandle, "PCX_MAT_ID", "83694");
                view.SetRowCellValue(rowHandle, "MAT_NAME", "UPPER STITCHING THREAD AVG. PRICE");
                view.SetRowCellValue(rowHandle, "VENDOR_NAME", "FACTORY IN-HOUSE MANUFACTURING");

                //row["MXSXL_NUMBER"] = "226640.716.1";
                //row["CS_CD"] = "0";
                //row["MAT_CD"] = "226640";
                //row["PCX_SUPP_MAT_ID"] = "149445";
                //row["PCX_MAT_ID"] = "83694";
                //row["MAT_NAME"] = "UPPER STITCHING THREAD AVG. PRICE";
                //row["VENDOR_NAME"] = "FACTORY IN-HOUSE MANUFACTURING";
            }
            else if (pcxPartId == "3391")
            {
                /* UPPER CEMENT */

                view.SetRowCellValue(rowHandle, "MXSXL_NUMBER", "226624.716.1");
                view.SetRowCellValue(rowHandle, "CS_CD", "0");
                view.SetRowCellValue(rowHandle, "MAT_CD", "226624");
                view.SetRowCellValue(rowHandle, "PCX_SUPP_MAT_ID", "138316");
                view.SetRowCellValue(rowHandle, "PCX_MAT_ID", "78312");
                view.SetRowCellValue(rowHandle, "MAT_NAME", "UPPER CEMENT");
                view.SetRowCellValue(rowHandle, "VENDOR_NAME", "FACTORY IN-HOUSE MANUFACTURING");

                //row["MXSXL_NUMBER"] = "226624.716.1";
                //row["CS_CD"] = "0";
                //row["MAT_CD"] = "226624";
                //row["PCX_SUPP_MAT_ID"] = "138316";
                //row["PCX_MAT_ID"] = "78312";
                //row["MAT_NAME"] = "UPPER CEMENT";
                //row["VENDOR_NAME"] = "FACTORY IN-HOUSE MANUFACTURING";
            }
            else if (pcxPartId == "1311")
            {
                /* OUTER CARTON */

                view.SetRowCellValue(rowHandle, "MXSXL_NUMBER", "2136.2056.1");
                view.SetRowCellValue(rowHandle, "CS_CD", "0");
                view.SetRowCellValue(rowHandle, "MCS_NUMBER", "BX/ 002");
                view.SetRowCellValue(rowHandle, "MAT_CD", "2136");
                view.SetRowCellValue(rowHandle, "PCX_SUPP_MAT_ID", "101347");
                view.SetRowCellValue(rowHandle, "PCX_MAT_ID", "60268");
                view.SetRowCellValue(rowHandle, "MAT_NAME", "GENERIC OUTER CARTON");
                view.SetRowCellValue(rowHandle, "VENDOR_NAME", "OIA PACKAGING");
                view.SetRowCellValue(rowHandle, "PART_TYPE", "PACKAGING");

                //row["MXSXL_NUMBER"] = "2136.2056.1";
                //row["CS_CD"] = "0";
                //row["MCS_NUMBER"] = "BX/ 002";
                //row["MAT_CD"] = "2136";
                //row["PCX_SUPP_MAT_ID"] = "101347";
                //row["PCX_MAT_ID"] = "60268";
                //row["MAT_NAME"] = "GENERIC OUTER CARTON";
                //row["VENDOR_NAME"] = "OIA PACKAGING";
                //row["PART_TYPE"] = "PACKAGING";
            }
        }

        /// <summary>
        /// 셀에 유저가 입력한 컬러를 배경색으로 표기한다.
        /// </summary>
        /// <param name="view"></param>
        public static void FillColorInCell(GridView view)
        {
            // 새 컬러 선택
            ColorDialog dialog = new ColorDialog();
            dialog.AllowFullOpen = false;
            dialog.ShowHelp = true;

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

                    colorList.Add(columnName, selectedColor);   // 신규 컬러 등록
                    colorFormat = string.Empty; // 새로운 컬러 포맷을 만들기 위해 초기화

                    foreach (KeyValuePair<string, string> kvp in colorList)
                    {
                        /* 신규 포맷 생성 */

                        string pair = kvp.Key + "/" + kvp.Value;
                        colorFormat += pair + ",";
                    }

                    view.SetRowCellValue(cell.RowHandle, "CELL_COLOR", colorFormat); // 신규 포맷 등록
                }
            }
        }

        /// <summary>
        /// <para>그리드에서 사용하는 파트타입 룩업의 소스를 만든다.</para>
        /// <para>(Nike Section ID + CS Master Type)</para>
        /// </summary>
        /// <returns></returns>
        public static DataTable CreatePartTypeLookUpSource()
        {
            DataTable types = new DataTable();
            types.Columns.Add("TYPE");

            DataRow newRow = types.NewRow();
            newRow["TYPE"] = "UPPER";
            types.Rows.Add(newRow);

            newRow = types.NewRow();
            newRow["TYPE"] = "MIDSOLE";
            types.Rows.Add(newRow);

            newRow = types.NewRow();
            newRow["TYPE"] = "OUTSOLE";
            types.Rows.Add(newRow);

            newRow = types.NewRow();
            newRow["TYPE"] = "AIRBAG";
            types.Rows.Add(newRow);

            newRow = types.NewRow();
            newRow["TYPE"] = "PACKAGING";
            types.Rows.Add(newRow);

            newRow = types.NewRow();
            newRow["TYPE"] = "LACE";
            types.Rows.Add(newRow);

            newRow = types.NewRow();
            newRow["TYPE"] = "SOCKLINER";
            types.Rows.Add(newRow);

            newRow = types.NewRow();
            newRow["TYPE"] = "KNIT";
            types.Rows.Add(newRow);

            newRow = types.NewRow();
            newRow["TYPE"] = "PACK";
            types.Rows.Add(newRow);

            newRow = types.NewRow();
            newRow["TYPE"] = "OTHER";
            types.Rows.Add(newRow);

            return types;
        }

        /// <summary>
        /// BOM Import에 영향을 미치지 않는 일부 컬럼만 수정 가능하도록 한다.
        /// </summary>
        public static void MakeColumnsUneditable(GridView view)
        {
            string[] columnsEditable = new string[] { "MAT_COMMENT", "COLOR_COMMENT", "PROCESS", "REMARKS", "CDMKR_YN" };

            foreach (GridColumn column in view.Columns)
            {
                if (column.Visible)
                {
                    if (!columnsEditable.Contains(column.FieldName))
                        column.OptionsColumn.AllowEdit = false;
                    else
                        column.OptionsColumn.AllowEdit = true;
                }
                else
                    continue;
            }
        }
        
        /// <summary>
        /// Chain values.
        /// </summary>
        /// <param name="view"></param>
        /// <param name="columnName"></param>
        /// <returns></returns>
        public static string ChainValues(GridView view, string columnName)
        {
            string values = string.Empty;

            foreach (int rowHandle in view.GetSelectedRows())
            {
                if (values.Equals(""))
                    values = view.GetRowCellValue(rowHandle, columnName).ToString();
                else
                    values += "," + view.GetRowCellValue(rowHandle, columnName).ToString();
            }

            return values;
        }

        /// <summary>
        /// 직무에 맞는 담당자 정보를 로드한다.
        /// </summary>
        /// <returns></returns>
        public static string GetPIC(string DPA, string job)
        {
            PKG_INTG_BOM.GET_PIC_FROM_PMX pkgSelect = new PKG_INTG_BOM.GET_PIC_FROM_PMX();
            pkgSelect.ARG_WORK_TYPE = "GET";
            pkgSelect.ARG_DPA = DPA;
            pkgSelect.ARG_JOB = job;
            pkgSelect.OUT_CURSOR = string.Empty;

            DataTable dataSource = projectBaseForm.Exe_Select_PKG(pkgSelect).Tables[0];

            if (dataSource.Rows.Count > 0)
            {
                return dataSource.Rows[0]["USERID"].ToString();
            }
            else
                return "";
        }

        /// <summary>
        /// 현재 작업중인 Excel 프로세스의 PID 값을 가져온다.
        /// </summary>
        public static List<int> GetExcelPIDs()
        {
            List<int> pids = new List<int>();

            foreach (Process process in Process.GetProcessesByName("EXCEL"))
                pids.Add(process.Id);

            return pids;
        }

        /// <summary>
        /// Excel Object를 Release 한다.
        /// </summary>
        /// <param name="obj"></param>
        public static void ReleaseExcelObject(object obj)
        {
            try
            {
                if (obj != null)
                {
                    System.Runtime.InteropServices.Marshal.ReleaseComObject(obj);
                    obj = null;
                }
            }
            catch (Exception ex)
            {
                obj = null;
                throw ex;
            }
            finally
            {
                GC.Collect();
            }
        }

        /// <summary>
        /// Types : E(Error), I(Information), W(Warning).
        /// </summary>
        /// <param name="message"></param>
        /// <param name="type"></param>
        public static void ShowMessageBox(string message, string type)
        {
            switch (type)
            {
                case "E":

                    MessageBox.Show(message, "",
                        MessageBoxButtons.OK, MessageBoxIcon.Error, MessageBoxDefaultButton.Button1);
                    break;

                case "I":

                    MessageBox.Show(message, "",
                        MessageBoxButtons.OK, MessageBoxIcon.Information, MessageBoxDefaultButton.Button1);
                    break;

                case "W":

                    MessageBox.Show(message, "",
                        MessageBoxButtons.OK, MessageBoxIcon.Warning, MessageBoxDefaultButton.Button1);
                    break;
            }
        }

        /// <summary>
        /// See if there are line items to be deleted.
        /// </summary>
        /// <param name="view"></param>
        public static bool HasLineitemDeleted(GridView view)
        {
            foreach (int rowHandle in view.GetSelectedRows())
            {
                if (view.GetRowCellValue(rowHandle, "ROW_STATUS").ToString().Equals("D"))
                {
                    ShowMessageBox("There are line items to be deleted.", "W");
                    return true;
                }
            }

            return false;
        }

        /// <summary>
        /// Validate that there are rows unsaved on the gridview.
        /// </summary>
        /// <param name="view"></param>
        /// <returns></returns>
        public static bool HasLineitemUnsaved(GridView view)
        {
            for (int rowHandle = 0; rowHandle < view.RowCount; rowHandle++)
            {
                if (view.GetRowCellValue(rowHandle, "ROW_STATUS").ToString() != "N")
                {
                    MessageBox.Show("Save data first.", "",
                        MessageBoxButtons.OK, MessageBoxIcon.Warning, MessageBoxDefaultButton.Button1);
                    
                    return true;
                }
            }

            return false;
        }

        /// <summary>
        /// Validate a locked BOM was included in the selection.
        /// </summary>
        /// <param name="factory"></param>
        /// <param name="wsNo"></param>
        /// <returns></returns>
        public static bool HasBOMLocked(GridView view)
        {
            DataSet ds = null;

            PKG_INTG_BOM.SELECT_BOM_HEAD_DATA pkgSelect = new PKG_INTG_BOM.SELECT_BOM_HEAD_DATA();
            pkgSelect.ARG_FACTORY = view.GetFocusedRowCellValue("FACTORY").ToString();
            pkgSelect.ARG_CONCAT_WS_NO = ChainValues(view, "WS_NO");
            pkgSelect.OUT_CURSOR = string.Empty;

            ds = projectBaseForm.Exe_Select_PKG(pkgSelect);

            if (ds.Tables[0].Rows.Count > 0)
            {
                if (ds.Tables[0].AsEnumerable().Where(x => x["LOCK_YN"].ToString().Equals("Y")).Count() > 0)
                {
                    MessageBox.Show("Locked BOM is included in the selection.", "",
                        MessageBoxButtons.OK, MessageBoxIcon.Error, MessageBoxDefaultButton.Button1);
                    return true;
                }
                else
                    return false;
            }
            else
                return false;
        }

        /// <summary>
        /// <para>If any filter is applied to the view, notify user popup.</para>
        /// <para>1. See if the filter is applied on 'Find Panel'.</para>
        /// <para>2. See if the filter is applied on a each column.</para>
        /// </summary>
        /// <param name="view"></param>
        /// <returns></returns>
        public static bool IsFilterApplied(GridView view)
        {
            List<string> columnList = new List<string>();

            // Check whether a text is being on the find panel.
            if (view.FindFilterText != "")
            {
                Common.ShowMessageBox("Please clear the find panel at the top.", "E");
                return true;
            }

            // Check whether the filter has been applied to each column.
            foreach (GridColumn column in view.VisibleColumns)
            {
                if (column.FilterInfo.FilterString != "")
                    columnList.Add(column.Caption);
            }

            // Show column list whose filter was applied.
            if (columnList.Count > 0)
            {
                string errorMessage = string.Empty;

                for (int i = 0; i < columnList.Count; i++)
                    errorMessage += string.Format("{0}. {1}\n", (i + 1).ToString(), columnList[i]);

                MessageBox.Show("** Please clear filters applied to below columns.\n\n" + errorMessage, "",
                    MessageBoxButtons.OK, MessageBoxIcon.Error, MessageBoxDefaultButton.Button1);

                return true;
            }
            else
                return false;
        }

        /// <summary>
        /// 현재 BOM의 LOCK 상태를 리턴한다.
        /// </summary>
        /// <param name="LOCK_YN"></param>
        /// <returns></returns>
        public static bool IsBOMLocked(bool isLocked)
        {
            if (isLocked)
                return true;
            else
                return false;
        }

        /// <summary>
        /// See if the BOM has been made by fake.
        /// </summary>
        /// <param name="CSBOMStatus"></param>
        /// <returns></returns>
        public static bool IsFakeBOM(string CSBOMStatus)
        {
            if (CSBOMStatus.Equals("F"))
                return true;
            else
                return false;
        }
    }
}