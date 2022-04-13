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
using DevExpress.XtraSplashScreen;              // XtraSplashScreen
using CSI.Client.ProjectBaseForm;               // ProjectBaseForm Class
using CSI.PCC.PCX.COM;                          // Common Class
using CSI.PCC.PCX.PACKAGE;                      // Package Class

namespace CSI.PCC.PCX
{
    public partial class CompareBOM : DevExpress.XtraEditors.XtraForm
    {
        public ProjectBaseForm projectBaseForm = Common.projectBaseForm;

        public string BASE_FACTORY { get; set; }
        public string BASE_WS_NO { get; set; }
        public string SUB_WS_NO { get; set; }
        public string BOM_STATUS { get; set; }

        //Color CURR_BACK_COLOR = Color.White;
        Dictionary<int, Color> APPLIED_ROWS = new Dictionary<int, Color>();

        public CompareBOM()
        {
            InitializeComponent();
        }

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);

            #region Part Type 룩업 데이터 바인딩

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

            repositoryItemLookUpEdit1.DataSource = types;   // Single GridView

            #endregion

            BindDataSourceGridView();
        }

        #region 컨텍스트 메뉴 이벤트

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ContextMenu_Click(object sender, EventArgs e)
        {
            System.Windows.Forms.ToolStripMenuItem menuItem = sender as System.Windows.Forms.ToolStripMenuItem;

            switch (menuItem.Name)
            {
                case "FindCode":
                    FindCodeFromLibrary();
                    break;

                case "FindProcess":
                    FindProcessFromMaster();
                    break;

                case "SetPtrnByTop":
                    BindPtrnPartName("Top");
                    break;

                case "SetPtrnByEach":
                    BindPtrnPartName("Each");
                    break;

                case "CopyPtrnV1N4":
                    CopyFromCSPatternPart("New");
                    break;

                case "CopyPtrnF1F4":
                    CopyFromCSPatternPart("Old");
                    break;

                case "MulCheckCombine":
                    MultiCheck("COMBINE_YN");
                    break;

                case "MulCheckSticker":
                    MultiCheck("STICKER_YN");
                    break;

                case "MulCheckMidsole":
                    MultiCheck("MDSL_CHK");
                    break;

                case "MulCheckOutsole":
                    MultiCheck("OTSL_CHK");
                    break;

                case "MulCheckCode":
                    MultiCheck("CDMKR_YN");
                    break;

                case "transfer":
                    TransferToCodemaker();
                    break;

                case "transferRecord":
                    ShowRecordOfTransfer();
                    break;

                case "MatInfo":
                    ShowMaterialInfomation();
                    break;

                case "FocusRow":
                    ShowFocusedRow();
                    break;

                case "PartAdd":
                    OpenPartMDM();
                    break;

                case "genComment":
                    GenerateComment();
                    break;

                default:
                    break;
            }
        }

        /// <summary>
        /// 라이브러리에서 코드를 찾아 셀에 입력
        /// </summary>
        /// <param name="_type"></param>
        private void FindCodeFromLibrary()
        {
            if (Common.HasLineitemDeleted(gvwMaster))
                return;

            // Parameters to send to the childForm.
            int initSearchType = 0;
            string keyword = gvwMaster.GetFocusedRowCellValue(gvwMaster.FocusedColumn).ToString();
            string partDelimiter = string.Empty;

            // Get the initial search type from the focused column.
            switch (gvwMaster.FocusedColumn.FieldName)
            {
                case "PART_NAME":
                case "PART_TYPE":
                case "PART_CODE":
                case "PTRN_PART_NAME":

                    initSearchType = 0;

                    if (gvwMaster.FocusedColumn.FieldName == "PTRN_PART_NAME")
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

                    // First of all PCX,
                    initSearchType = 1;
                    break;

                case "PCX_COLOR_ID":
                case "COLOR_CD":
                case "COLOR_NAME":

                    initSearchType = 2;
                    break;

                case "CS_CD":

                    initSearchType = 4;
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
                    gvwMaster.CellValueChanged -= new CellValueChangedEventHandler(gvwMaster_CellValueChanged);

                    foreach (int rowHandle in gvwMaster.GetSelectedRows())
                    {
                        if (gvwMaster.GetRowCellValue(rowHandle, "LOCK_YN").ToString().Equals("N"))
                        {
                            if (results[0] == "Part")
                            {
                                string oldValue = gvwMaster.GetRowCellValue(rowHandle, "PART_NAME").ToString();

                                // When the part name is changed from 'Lamination' to general part.
                                if (oldValue.Contains("LAMINATION") == true && results[1].Contains("LAMINATION") == false)
                                {
                                    // An user doesn't need to set nike pattern part name for general part.
                                    gvwMaster.SetRowCellValue(rowHandle, "PTRN_PART_NAME", "");
                                    gvwMaster.SetRowCellValue(rowHandle, "PTRN_PART_CD", "");
                                }

                                gvwMaster.SetRowCellValue(rowHandle, "PART_NAME", results[1]);
                                gvwMaster.SetRowCellValue(rowHandle, "PART_CD", results[3]);

                                // Set part type by each case.
                                if (results[1].Contains("AIRBAG"))
                                {
                                    // If the part name contains 'AIRBAG', fix BOM section to 'AIRBAG'.
                                    gvwMaster.SetRowCellValue(rowHandle, "PART_TYPE", "AIRBAG");
                                    gvwMaster.SetRowCellValue(rowHandle, "MDSL_CHK", "Y");
                                    gvwMaster.SetRowCellValue(rowHandle, "OTSL_CHK", "N");
                                }
                                else if (results[1].Contains("LAMINATION"))
                                {
                                    // If the part name contains 'AIRBAG', fix BOM section to 'UPPER'.
                                    gvwMaster.SetRowCellValue(rowHandle, "PART_TYPE", "UPPER");
                                    gvwMaster.SetRowCellValue(rowHandle, "MDSL_CHK", "N");
                                    gvwMaster.SetRowCellValue(rowHandle, "OTSL_CHK", "N");
                                }
                                else if (results[2] == "MIDSOLE" || results[2] == "OUTSOLE")
                                {
                                    // If the part type is 'MIDSOLE' or 'OUTSOLE', fix BOM section to it.
                                    gvwMaster.SetRowCellValue(rowHandle, "PART_TYPE", results[2]);
                                    gvwMaster.SetRowCellValue(rowHandle, "MDSL_CHK", (results[2] == "MIDSOLE") ? "Y" : "N");
                                    gvwMaster.SetRowCellValue(rowHandle, "OTSL_CHK", (results[2] == "OUTSOLE") ? "Y" : "N");
                                }
                                else if (results[4] == "3389" || results[4] == "3390")
                                {
                                    // Fix BOM section to 'UPPER' for WET CHEMISTRY & WET CHEMISTRY-MULTI parts.
                                    gvwMaster.SetRowCellValue(rowHandle, "PART_TYPE", "UPPER");
                                    gvwMaster.SetRowCellValue(rowHandle, "MDSL_CHK", "N");
                                    gvwMaster.SetRowCellValue(rowHandle, "OTSL_CHK", "N");
                                }
                                else
                                {
                                    // Follow existing BOM section(When a row is created, the BOM section follows the upper row).
                                    gvwMaster.SetRowCellValue(rowHandle, "PART_TYPE", results[2]);
                                    gvwMaster.SetRowCellValue(rowHandle, "MDSL_CHK", "N");
                                    gvwMaster.SetRowCellValue(rowHandle, "OTSL_CHK", "N");
                                }

                                gvwMaster.SetRowCellValue(rowHandle, "PCX_PART_ID", results[4]);

                                Common.BindDefaultMaterialByNCFRule(gvwMaster, rowHandle, results[4]);
                            }
                            else if (results[0] == "PatternPart")
                            {
                                gvwMaster.SetRowCellValue(rowHandle, "PTRN_PART_NAME", results[1]);
                                gvwMaster.SetRowCellValue(rowHandle, "PTRN_PART_CD", results[3]);
                            }
                            else if (results[0] == "PCX_Material")
                            {
                                // Automatically tick for sticker & combine columns.
                                if (results[1] == "87031" || results[1] == "78733" || results[1] == "79467")
                                    gvwMaster.SetRowCellValue(rowHandle, "STICKER_YN", "Y");
                                else if (results[1] == "54638" || results[1] == "79341")
                                    gvwMaster.SetRowCellValue(rowHandle, "COMBINE_YN", "Y");
                                else
                                    gvwMaster.SetRowCellValue(rowHandle, "STICKER_YN", "N");

                                gvwMaster.SetRowCellValue(rowHandle, "PCX_MAT_ID", results[1]);
                                gvwMaster.SetRowCellValue(rowHandle, "MAT_CD", results[2]);
                                gvwMaster.SetRowCellValue(rowHandle, "MAT_NAME", results[3]);
                                gvwMaster.SetRowCellValue(rowHandle, "MXSXL_NUMBER", results[4]);
                                gvwMaster.SetRowCellValue(rowHandle, "MCS_NUMBER", results[5]);
                                gvwMaster.SetRowCellValue(rowHandle, "VENDOR_NAME", results[6]);
                                gvwMaster.SetRowCellValue(rowHandle, "CS_CD", results[7]);
                                gvwMaster.SetRowCellValue(rowHandle, "PCX_SUPP_MAT_ID", results[8]);
                                //gvwMaster.SetRowCellValue(rowHandle, "NIKE_MS_STATE", results[9]);
                                //gvwMaster.SetRowCellValue(rowHandle, "BLACK_LIST_YN", results[10]);
                                //gvwMaster.SetRowCellValue(rowHandle, "MDM_NOT_EXST", results[11]);

                                // Automatically tick for 'Code Transfer'.
                                if (results[11] == "Y")
                                {
                                    gvwMaster.SetRowCellValue(rowHandle, "CDMKR_YN", "Y");
                                    //gvwMaster.SetRowCellValue(rowHandle, "MDM_NOT_EXST", "Y");
                                }
                                else
                                {
                                    gvwMaster.SetRowCellValue(rowHandle, "CDMKR_YN", "N");
                                    //gvwMaster.SetRowCellValue(rowHandle, "MDM_NOT_EXST", "N");
                                }
                            }
                            else if (results[0] == "Color")
                            {
                                gvwMaster.SetRowCellValue(rowHandle, "PCX_COLOR_ID", results[1]);
                                gvwMaster.SetRowCellValue(rowHandle, "COLOR_CD", results[2]);
                                gvwMaster.SetRowCellValue(rowHandle, "COLOR_NAME", results[3]);
                            }
                            else if (results[0] == "PCC_Material")
                            {
                                // Automatically tick for sticker & combine columns.
                                if (results[1] == "87031" || results[1] == "78733" || results[1] == "79467")
                                    gvwMaster.SetRowCellValue(rowHandle, "STICKER_YN", "Y");
                                else if (results[1] == "54638" || results[1] == "79341")
                                    gvwMaster.SetRowCellValue(rowHandle, "COMBINE_YN", "Y");
                                else
                                    gvwMaster.SetRowCellValue(rowHandle, "STICKER_YN", "N");

                                gvwMaster.SetRowCellValue(rowHandle, "PCX_MAT_ID", results[1]);
                                gvwMaster.SetRowCellValue(rowHandle, "MAT_CD", results[2]);
                                gvwMaster.SetRowCellValue(rowHandle, "MAT_NAME", results[3]);
                                gvwMaster.SetRowCellValue(rowHandle, "MXSXL_NUMBER", results[4]);
                                gvwMaster.SetRowCellValue(rowHandle, "MCS_NUMBER", results[5]);
                                gvwMaster.SetRowCellValue(rowHandle, "VENDOR_NAME", results[6]);
                                gvwMaster.SetRowCellValue(rowHandle, "CS_CD", results[7]);
                                gvwMaster.SetRowCellValue(rowHandle, "PCX_SUPP_MAT_ID", results[8]);
                                //gvwMaster.SetRowCellValue(rowHandle, "NIKE_MS_STATE", results[9]);
                                //gvwMaster.SetRowCellValue(rowHandle, "BLACK_LIST_YN", results[10]);

                                // If an user finds material from 'PCC Material', it means the code of this material was already created.
                                //gvwMaster.SetRowCellValue(rowHandle, "MDM_NOT_EXST", "N");
                                gvwMaster.SetRowCellValue(rowHandle, "CDMKR_YN", "N");
                            }
                            else if (results[0] == "CS_Material")
                            {
                                gvwMaster.SetRowCellValue(rowHandle, "MXSXL_NUMBER", results[1]);
                                gvwMaster.SetRowCellValue(rowHandle, "MAT_CD", results[2]);
                                gvwMaster.SetRowCellValue(rowHandle, "MAT_NAME", results[3]);
                                gvwMaster.SetRowCellValue(rowHandle, "MCS_NUMBER", "");
                                gvwMaster.SetRowCellValue(rowHandle, "CS_CD", "CS");
                                gvwMaster.SetRowCellValue(rowHandle, "PCX_SUPP_MAT_ID", "100");
                                gvwMaster.SetRowCellValue(rowHandle, "PCX_MAT_ID", "100");
                            }

                            if (gvwMaster.GetRowCellValue(rowHandle, "ROW_STATUS").ToString() != "I")
                                gvwMaster.SetRowCellValue(rowHandle, "ROW_STATUS", "U");
                        }
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.ToString());
                }
                finally
                {
                    // Link event again.
                    gvwMaster.CellValueChanged += new CellValueChangedEventHandler(gvwMaster_CellValueChanged);
                }
            }
        }

        /// <summary>
        /// 프로세스 마스터에서 프로세스를 선택하여 저장
        /// </summary>
        private void FindProcessFromMaster()
        {
            int[] rowHandles = gvwMaster.GetSelectedRows();

            foreach (int rowHandle in rowHandles)
            {
                if (gvwMaster.GetRowCellValue(rowHandle, "ROW_STATUS").ToString().Equals("D"))
                {
                    Common.ShowMessageBox("There are rows which have been deleted.", "E");
                    return;
                }
            }

            FindProcess findForm = new FindProcess()
            {
                EnteredProcess = gvwMaster.GetRowCellValue(gvwMaster.FocusedRowHandle, "PROCESS").ToString().ToUpper()
            };

            if (findForm.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                try
                {
                    gvwMaster.CellValueChanged -= new CellValueChangedEventHandler(gvwMaster_CellValueChanged);

                    List<string> listOfSelectedProcess = (List<string>)findForm.SelectedProcess;

                    // Like process1, process2, prcoess3, ...
                    string process = string.Join(",", listOfSelectedProcess.ToArray());

                    foreach (int rowHandle in rowHandles)
                    {

                        gvwMaster.SetRowCellValue(rowHandle, "PROCESS", process);

                        if (gvwMaster.GetRowCellValue(rowHandle, "ROW_STATUS").ToString() != "I")
                            gvwMaster.SetRowCellValue(rowHandle, "ROW_STATUS", "U");

                    }
                }
                catch (Exception ex)
                {
                    Common.ShowMessageBox(ex.ToString(), "E");
                    return;
                }
                finally
                {
                    gvwMaster.CellValueChanged += new CellValueChangedEventHandler(gvwMaster_CellValueChanged);
                }
            }
        }

        /// <summary>
        /// 타입에 맞게 패턴파트 적용
        /// </summary>
        /// <param name="type"></param>
        private void BindPtrnPartName(string type)
        {
            int[] rowHandles = gvwMaster.GetSelectedRows();
            string partName = string.Empty;
            string partCode = string.Empty;
            string rowStatus = string.Empty;
            string lockYN = string.Empty;

            try
            {
                gvwMaster.CellValueChanged -= new CellValueChangedEventHandler(gvwMaster_CellValueChanged);

                foreach (int rowHandle in rowHandles)
                {
                    partName = type.Equals("Top") ? gvwMaster.GetRowCellValue(rowHandles[0], "PART_NAME").ToString()
                        : gvwMaster.GetRowCellValue(rowHandle, "PART_NAME").ToString();

                    partCode = type.Equals("Top") ? gvwMaster.GetRowCellValue(rowHandles[0], "PART_CD").ToString()
                        : gvwMaster.GetRowCellValue(rowHandle, "PART_CD").ToString();

                    rowStatus = gvwMaster.GetRowCellValue(rowHandle, "ROW_STATUS").ToString();
                    lockYN = gvwMaster.GetRowCellValue(rowHandle, "LOCK_YN").ToString();

                    if (!rowStatus.Equals("D") && lockYN.Equals("N"))
                    {
                        gvwMaster.SetRowCellValue(rowHandle, "CS_PTRN_NAME", partName);
                        gvwMaster.SetRowCellValue(rowHandle, "CS_PTRN_CD", partCode);

                        if (!rowStatus.Equals("I"))
                            gvwMaster.SetRowCellValue(rowHandle, "ROW_STATUS", "U");
                    }
                }
            }
            finally
            {
                gvwMaster.RefreshData();
                gvwMaster.CellValueChanged += new CellValueChangedEventHandler(gvwMaster_CellValueChanged);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="fieldName"></param>
        private void MultiCheck(string fieldName)
        {
            string rowStatus = string.Empty;
            string lockYN = string.Empty;

            try
            {
                gvwMaster.CellValueChanged -= new CellValueChangedEventHandler(gvwMaster_CellValueChanged);

                foreach (int rowHandle in gvwMaster.GetSelectedRows())
                {
                    rowStatus = gvwMaster.GetRowCellValue(rowHandle, "ROW_STATUS").ToString();
                    lockYN = gvwMaster.GetRowCellValue(rowHandle, "LOCK_YN").ToString();

                    if (!rowStatus.Equals("D") && lockYN.Equals("N"))
                    {
                        if (gvwMaster.GetRowCellValue(rowHandle, fieldName).ToString().Equals("Y"))
                            gvwMaster.SetRowCellValue(rowHandle, fieldName, "N");
                        else
                            gvwMaster.SetRowCellValue(rowHandle, fieldName, "Y");

                        if (!rowStatus.Equals("I") && !fieldName.Equals("CDMKR_YN"))
                            gvwMaster.SetRowCellValue(rowHandle, "ROW_STATUS", "U");
                    }
                }
            }
            finally
            {
                gvwMaster.RefreshData();
                gvwMaster.CellValueChanged += new CellValueChangedEventHandler(gvwMaster_CellValueChanged);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        private void ShowMaterialInfomation()
        {
            try
            {
                string PDMSuppMatNumber = gvwMaster.GetRowCellValue(gvwMaster.FocusedRowHandle, "MXSXL_NUMBER").ToString();
                if (PDMSuppMatNumber == "") return;

                string[] splitPDMSuppMatNum = PDMSuppMatNumber.Split('.');
                if (splitPDMSuppMatNum.Length != 3) return;

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
        private void ShowFocusedRow()
        {
            // 포커스된 행의 개수
            int[] selectedRowHandles = gvwMaster.GetSelectedRows();

            if (selectedRowHandles.Length == 0)
            {
                // 0개일 경우 포커스된 행 전체 선택
                gvwMaster.SelectRow(gvwMaster.FocusedRowHandle);
            }
            else if (selectedRowHandles.Length == 1)
            {
                // 1개일 경우 선택인지 취소인지 구분
                if (CheckRowIsSelected(gvwMaster, gvwMaster.FocusedRowHandle))
                    gvwMaster.UnselectRow(gvwMaster.FocusedRowHandle);
                else
                    gvwMaster.SelectRow(gvwMaster.FocusedRowHandle);
            }
            else
            {
                // 1개 이상일 경우 선택인지 취소인지 구분
                string type = "Select";

                GridCell[] cells = gvwMaster.GetSelectedCells();
                if (cells.Length > 25)
                    type = "Cancel";
                else
                    type = "Select";

                if (type == "Select")
                {
                    // 선택한 행 모두 전체 선택
                    foreach (int rowHandle in selectedRowHandles)
                    {
                        if (CheckRowIsSelected(gvwMaster, rowHandle))
                            gvwMaster.UnselectRow(rowHandle);
                        else
                            gvwMaster.SelectRow(rowHandle);
                    }
                }
                else if (type == "Cancel")
                {
                    // 포커스된 행만 전체 선택 취소
                    if (CheckRowIsSelected(gvwMaster, gvwMaster.FocusedRowHandle))
                        gvwMaster.UnselectRow(gvwMaster.FocusedRowHandle);
                    else
                        gvwMaster.SelectRow(gvwMaster.FocusedRowHandle);
                }
            }
        }

        /// <summary>
        /// 타겟 행이 현재 전체 선택되었는지 아닌지 반환
        /// </summary>
        /// <param name="view"></param>
        /// <param name="targetRowHandle"></param>
        /// <returns></returns>
        private bool CheckRowIsSelected(GridView view, int targetRowHandle)
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
        /// Transfer data selected by an user to codemaker.
        /// </summary>
        /// <param name="view"></param>
        private void TransferToCodemaker()
        {
            if (Common.IsFilterApplied(gvwMaster))
                return;

            if (Common.HasLineitemUnsaved(gvwMaster))
                return;

            DataTable dataSource = grdMaster.DataSource as DataTable;
            List<DataRow> rows = dataSource.AsEnumerable().Where(r => r["CDMKR_YN"].ToString().Equals("Y")).ToList();

            ArrayList listToSend = new ArrayList();

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

                listToSend.Add(pkgInsert);
            }

            if (projectBaseForm.Exe_Modify_PKG(listToSend) == null)
            {
                MessageBox.Show("Failed to transfer data.");
                return;
            }
            else
                MessageBox.Show("Complete");
        }

        /// <summary>
        /// Show records of data transferred to codemaker.
        /// </summary>
        private void ShowRecordOfTransfer()
        {
            TransferRecord formRecord = new TransferRecord();

            formRecord.INHERITANCE = new string[] { BASE_FACTORY, "," + BASE_WS_NO + SUB_WS_NO };

            if (formRecord.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            { }
        }

        /// <summary>
        /// 입력된 CS 패턴 파트와 동일하게 Nike 패턴 파트에 자동 입력한다.
        /// </summary>
        private void CopyFromCSPatternPart(string type)
        {
            DataTable dataSource = grdMaster.DataSource as DataTable;

            gvwMaster.CellValueChanged -= new CellValueChangedEventHandler(gvwMaster_CellValueChanged);

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

                                if (rowStatus != "D" && lockYN.Equals("N"))
                                {
                                    // Update when Nike Pattern Part Name is different from CS Pattern Part Name.
                                    if (row["PTRN_PART_NAME"].ToString() != row["CS_PTRN_NAME"].ToString())
                                    {
                                        row["PTRN_PART_NAME"] = row["CS_PTRN_NAME"].ToString();
                                        row["PTRN_PART_CD"] = row["CS_PTRN_CD"].ToString();

                                        // Update row indicator.
                                        if (rowStatus != "I" && rowStatus != "D")
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

                        if (rowStatus != "D" && lockYN.Equals("N"))
                        {
                            // Do When Nike Pattern Part and CS Pattern Part are different.
                            if (row["PTRN_PART_NAME"].ToString() != row["CS_PTRN_NAME"].ToString())
                            {
                                row["PTRN_PART_NAME"] = row["CS_PTRN_NAME"].ToString();
                                row["PTRN_PART_CD"] = row["CS_PTRN_CD"].ToString();

                                // Update indicator of row status.
                                if (rowStatus != "I" && rowStatus != "D")
                                    row["ROW_STATUS"] = "U";
                            }
                            else
                                continue;
                        }
                    }

                    break;
            }

            gvwMaster.RefreshData();
            gvwMaster.CellValueChanged += new CellValueChangedEventHandler(gvwMaster_CellValueChanged);
        }

        /// <summary>
        /// 
        /// </summary>
        private void GenerateComment()
        {
            PCXComment form = new PCXComment("EDIT");
            form.BaseForm = projectBaseForm;
            form.EncodedComment = gvwMaster.GetRowCellValue(gvwMaster.FocusedRowHandle, "ENCODED_CMT").ToString();

            if (form.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                gvwMaster.CellValueChanged -= new CellValueChangedEventHandler(gvwMaster_CellValueChanged);

                foreach (int rowHandle in gvwMaster.GetSelectedRows())
                {
                    gvwMaster.SetRowCellValue(rowHandle, "MAT_COMMENT", form.Comment);
                    gvwMaster.SetRowCellValue(rowHandle, "ENCODED_CMT", form.EncodedComment);

                    if (gvwMaster.GetRowCellValue(rowHandle, "ROW_STATUS").ToString() != "I")
                        gvwMaster.SetRowCellValue(rowHandle, "ROW_STATUS", "U");
                }

                gvwMaster.CellValueChanged += new CellValueChangedEventHandler(gvwMaster_CellValueChanged);
            }
        }

        #endregion

        #region 사용자 정의 함수

        /// <summary>
        /// 비교할 BOM 데이터를 가져온다.
        /// </summary>
        private void BindDataSourceGridView()
        {
            PKG_INTG_COMPARE_BOM.SELECT_SOURCE_DATA pkgSelect = new PKG_INTG_COMPARE_BOM.SELECT_SOURCE_DATA();
            pkgSelect.ARG_FACTORY = BASE_FACTORY;
            pkgSelect.ARG_BASE_WS_NO = BASE_WS_NO;
            pkgSelect.ARG_SUB_WS_NO = SUB_WS_NO;
            pkgSelect.OUT_CURSOR = string.Empty;

            DataTable dataSource = projectBaseForm.Exe_Select_PKG(pkgSelect).Tables[0];
            if (dataSource != null)
                grdMaster.DataSource = dataSource;
            else
            {
                Common.ShowMessageBox("Failed to load.", "E");
                return;
            }
        }

        #endregion

        #region 컨트롤 이벤트

        /// <summary>
        /// 변경된 데이터를 저장한다.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnSave_Click(object sender, EventArgs e)
        {
            ArrayList list = new ArrayList();

            try
            {
                SplashScreenManager.ShowForm(this, typeof(MainWaitForm), false, false, false);

                if (CheckSaveValidation(gvwMaster) == false)
                    return;

                if (BOM_STATUS.Equals("C"))
                    Common.ShowMessageBox("There is a BOM already confirmed. Please confirm again.", "I");

                for (int i = 0; i < gvwMaster.RowCount; i++)
                {
                    if (gvwMaster.GetRowCellValue(i, "ROW_STATUS").ToString().Equals("N"))
                        continue;

                    PKG_INTG_COMPARE_BOM.UPDATE_BOM_TAIL_DATA pkgUpdate = new PKG_INTG_COMPARE_BOM.UPDATE_BOM_TAIL_DATA();
                    pkgUpdate.ARG_FACTORY = gvwMaster.GetRowCellValue(i, "FACTORY").ToString();
                    pkgUpdate.ARG_WS_NO = gvwMaster.GetRowCellValue(i, "WS_NO").ToString();
                    pkgUpdate.ARG_PART_SEQ = gvwMaster.GetRowCellValue(i, "PART_SEQ").ToString();
                    pkgUpdate.ARG_NIKE_COMMENT = gvwMaster.GetRowCellValue(i, "PART_NIKE_COMMENT").ToString();
                    pkgUpdate.ARG_PART_CD = gvwMaster.GetRowCellValue(i, "PART_CD").ToString();
                    pkgUpdate.ARG_PART_NAME = gvwMaster.GetRowCellValue(i, "PART_NAME").ToString();
                    pkgUpdate.ARG_PART_TYPE = gvwMaster.GetRowCellValue(i, "PART_TYPE").ToString();
                    pkgUpdate.ARG_BTTM = gvwMaster.GetRowCellValue(i, "BTTM").ToString();
                    pkgUpdate.ARG_MXSXL_NUMBER = gvwMaster.GetRowCellValue(i, "MXSXL_NUMBER").ToString();
                    pkgUpdate.ARG_CS_CD = gvwMaster.GetRowCellValue(i, "CS_CD").ToString();
                    pkgUpdate.ARG_MAT_CD = gvwMaster.GetRowCellValue(i, "MAT_CD").ToString();
                    pkgUpdate.ARG_MAT_NAME = gvwMaster.GetRowCellValue(i, "MAT_NAME").ToString();
                    pkgUpdate.ARG_MAT_COMMENT = gvwMaster.GetRowCellValue(i, "MAT_COMMENT").ToString();
                    pkgUpdate.ARG_MCS_NUMBER = gvwMaster.GetRowCellValue(i, "MCS_NUMBER").ToString();
                    pkgUpdate.ARG_COLOR_CD = gvwMaster.GetRowCellValue(i, "COLOR_CD").ToString();
                    pkgUpdate.ARG_COLOR_NAME = gvwMaster.GetRowCellValue(i, "COLOR_NAME").ToString();
                    pkgUpdate.ARG_COLOR_COMMENT = gvwMaster.GetRowCellValue(i, "COLOR_COMMENT").ToString();
                    pkgUpdate.ARG_REMARKS = gvwMaster.GetRowCellValue(i, "REMARKS").ToString();
                    pkgUpdate.ARG_UPD_USER = Common.sessionID;
                    pkgUpdate.ARG_PTRN_PART_NAME = gvwMaster.GetRowCellValue(i, "PTRN_PART_NAME").ToString();
                    pkgUpdate.ARG_PCX_MAT_ID = gvwMaster.GetRowCellValue(i, "PCX_MAT_ID").ToString();
                    pkgUpdate.ARG_PCX_SUPP_MAT_ID = gvwMaster.GetRowCellValue(i, "PCX_SUPP_MAT_ID").ToString();
                    pkgUpdate.ARG_PCX_COLOR_ID = gvwMaster.GetRowCellValue(i, "PCX_COLOR_ID").ToString();
                    pkgUpdate.ARG_ROW_STATUS = gvwMaster.GetRowCellValue(i, "ROW_STATUS").ToString();
                    pkgUpdate.ARG_PROCESS = gvwMaster.GetRowCellValue(i, "PROCESS").ToString();
                    pkgUpdate.ARG_VENDOR_NAME = gvwMaster.GetRowCellValue(i, "VENDOR_NAME").ToString();
                    pkgUpdate.ARG_COMBINE_YN = gvwMaster.GetRowCellValue(i, "COMBINE_YN").ToString();
                    pkgUpdate.ARG_STICKER_YN = gvwMaster.GetRowCellValue(i, "STICKER_YN").ToString();
                    pkgUpdate.ARG_PTRN_PART_CD = gvwMaster.GetRowCellValue(i, "PTRN_PART_CD").ToString();
                    pkgUpdate.ARG_MDSL_CHK = gvwMaster.GetRowCellValue(i, "MDSL_CHK").ToString();
                    pkgUpdate.ARG_OTSL_CHK = gvwMaster.GetRowCellValue(i, "OTSL_CHK").ToString();
                    pkgUpdate.ARG_CS_PTRN_CD = gvwMaster.GetRowCellValue(i, "CS_PTRN_CD").ToString();
                    pkgUpdate.ARG_CS_PTRN_NAME = gvwMaster.GetRowCellValue(i, "CS_PTRN_NAME").ToString();
                    pkgUpdate.ARG_ENCODED_CMT = gvwMaster.GetRowCellValue(i, "ENCODED_CMT").ToString();

                    list.Add(pkgUpdate);
                }

                if (projectBaseForm.Exe_Modify_PKG(list) == null)
                {
                    Common.ShowMessageBox("Failed to save.", "E");
                    return;
                }

                list.Clear();

                // Change BOM status to 'N'.
                PKG_INTG_COMPARE_BOM.UPDATE_BOM_HEAD_DATA pkgUpdate2 = new PKG_INTG_COMPARE_BOM.UPDATE_BOM_HEAD_DATA();
                pkgUpdate2.ARG_FACTORY = BASE_FACTORY;
                pkgUpdate2.ARG_CHAINED_WS_NO = string.Format("{0},{1}", BASE_WS_NO, SUB_WS_NO);
                pkgUpdate2.ARG_UPD_USER = Common.sessionID;

                list.Add(pkgUpdate2);

                if (projectBaseForm.Exe_Modify_PKG(list) == null)
                {
                    Common.ShowMessageBox("Failed to update header info.", "E");
                    return;
                }

                BindDataSourceGridView();

                SplashScreenManager.CloseForm(false);
                MessageBox.Show("Complete.");
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
        /// Save 전 필수 입력 속성 확인
        /// </summary>
        /// <param name="view"></param>
        /// <returns></returns>
        private bool CheckSaveValidation(GridView view)
        {
            try
            {
                List<string> partList = new List<string>();
                List<string> lamPartList = new List<string>();
                string chainedWsNo = "," + BASE_WS_NO + SUB_WS_NO;

                // BOM 별 파트 중복 체크
                string[] listWsNo = chainedWsNo.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);

                foreach (string wsNo in listWsNo)
                {
                    DataTable dataSource = grdMaster.DataSource as DataTable;
                    DataRow[] rows = dataSource.Select("WS_NO = '" + wsNo + "'");

                    foreach (DataRow row in rows)
                    {
                        string partCode = row["PART_CD"].ToString();
                        string partName = row["PART_NAME"].ToString();
                        string ptrnPartCode = row["CS_PTRN_CD"].ToString();
                        string ptrnPartName = row["CS_PTRN_NAME"].ToString();

                        if (partCode == "")
                            continue;

                        if (partName.Contains("LAMINATION") == true)
                        {
                            string key = partCode + ptrnPartCode;

                            if (lamPartList.Contains(key) == true)
                            {
                                MessageBox.Show("\"" + partName + " / " + ptrnPartName + "\" is duplicate.");
                                SetFocusColumn(view, GetRowHandle(view, wsNo, partCode), "PART_NAME", true);
                                return false;
                            }
                            else
                                lamPartList.Add(key);
                        }
                        else
                        {
                            if (partList.Contains(partCode) == true)
                            {
                                MessageBox.Show("\"" + partName + "\" is duplicate.");
                                SetFocusColumn(view, GetRowHandle(view, wsNo, partCode), "PART_NAME", true);
                                return false;
                            }
                            else
                                partList.Add(partCode);
                        }
                    }

                    // 다음 BOM 진행을 위해 리스트를 클리어
                    partList.Clear();
                    lamPartList.Clear();
                }
                return true;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
                return false; ;
            }
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
            bool passFirst = false; // 선행하는 행은 포함될 항목이라 가정

            for (int rowHandle = 0; rowHandle < view.RowCount; rowHandle++)
            {
                string wsNo = view.GetRowCellValue(rowHandle, "WS_NO").ToString();
                string partCode = view.GetRowCellValue(rowHandle, "PART_CD").ToString();

                if ((wsNo == targetWsNo) && (partCode == targetPartCode))
                {
                    if (passFirst == true)
                        return rowHandle;
                    else
                        passFirst = true;
                }
            }

            return 0;
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

        #endregion

        #region 그리드 이벤트

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void gvwMaster_CellValueChanged(object sender, CellValueChangedEventArgs e)
        {
            try
            {
                #region Variable

                string partCode = string.Empty;
                string partName = string.Empty;
                string partType = string.Empty;
                string pcxPartId = string.Empty;

                string mxsxlNumber = string.Empty;
                string pcxSuppMatID = string.Empty;
                string csCode = string.Empty;
                string mcsNumber = string.Empty;
                string pcxMatID = string.Empty;
                string pdmMatCode = string.Empty;
                string pdmMatName = string.Empty;
                string vendorName = string.Empty;
                //string matRisk = string.Empty;
                string nikeMSState = string.Empty;

                string pcxColorID = string.Empty;
                string pdmColorCode = string.Empty;
                string pdmColorName = string.Empty;

                string mdmNotExisting = "N";

                #endregion

                GridView view = sender as GridView;
                GridCell[] cells = view.GetSelectedCells();

                // if there are no changes, finish event.
                var currValue = view.ActiveEditor.EditValue.ToString();
                var oldValue = view.ActiveEditor.OldEditValue;

                if (currValue.ToString() == oldValue.ToString())
                    return;

                // To avoid infinite loop.
                view.CellValueChanged -= new CellValueChangedEventHandler(gvwMaster_CellValueChanged);

                string value = currValue;

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
                        if (BOM_STATUS != "F")
                        {
                            PKG_INTG_BOM.SELECT_FOR_DIRECT_INPUT pkgSelect = new PKG_INTG_BOM.SELECT_FOR_DIRECT_INPUT();
                            pkgSelect.ARG_TYPE = "Color";
                            pkgSelect.ARG_CODE = "";
                            pkgSelect.ARG_NAME = value;
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
                        if (BOM_STATUS != "F")
                        {
                            // 패키지 매개변수 입력
                            PKG_INTG_BOM.SELECT_FOR_DIRECT_INPUT pkgSelect = new PKG_INTG_BOM.SELECT_FOR_DIRECT_INPUT();
                            pkgSelect.ARG_TYPE = "Part";
                            pkgSelect.ARG_CODE = value;
                            pkgSelect.ARG_NAME = "";
                            pkgSelect.OUT_CURSOR = string.Empty;

                            // 패키지 호출하여 PCX Color 정보를 가져옴
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
                        if (BOM_STATUS != "F")
                        {
                            PKG_INTG_BOM.SELECT_FOR_DIRECT_INPUT pkgSelect = new PKG_INTG_BOM.SELECT_FOR_DIRECT_INPUT();
                            pkgSelect.ARG_TYPE = "Part";
                            pkgSelect.ARG_CODE = value;
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
                        // Get the material info. from PCX library.
                        PKG_INTG_BOM.SELECT_FOR_DIRECT_INPUT pkgSelect = new PKG_INTG_BOM.SELECT_FOR_DIRECT_INPUT();
                        pkgSelect.ARG_TYPE = "PCX_ByCode";
                        pkgSelect.ARG_CODE = value;
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
                            //blackListYN = dataSource.Rows[0]["BLACK_LIST_YN"].ToString();
                            mdmNotExisting = dataSource.Rows[0]["MDM_NOT_EXST"].ToString();
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
                        if (BOM_STATUS != "F")
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
                                //matRisk = "";
                                mdmNotExisting = "Y";

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
                                    //blackListYN = dataSource.Rows[0]["BLACK_LIST_YN"].ToString();
                                    mdmNotExisting = dataSource.Rows[0]["MDM_NOT_EXST"].ToString();
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
                                        //blackListYN = results[10];
                                        mdmNotExisting = results[11];
                                    }
                                }
                                else
                                {
                                    pcxSuppMatID = "100";
                                    pcxMatID = "100";
                                    pdmMatName = "PLACEHOLDER";

                                    MessageBox.Show("Not registered in PCX.");
                                }
                            }
                        }
                        else
                        {
                            pdmMatName = value;
                        }
                    }

                    #endregion
                }

                foreach (GridCell cell in cells)
                {
                    string rowStatus = view.GetRowCellValue(cell.RowHandle, "ROW_STATUS").ToString();
                    string lockYN = view.GetRowCellValue(cell.RowHandle, "LOCK_YN").ToString();

                    if (rowStatus != "D" && lockYN.Equals("N"))
                    {
                        if (view.FocusedColumn.FieldName == "COLOR_CD")
                        {
                            view.SetRowCellValue(cell.RowHandle, "PCX_COLOR_ID", pcxColorID);
                            view.SetRowCellValue(cell.RowHandle, "COLOR_CD", pdmColorCode);
                            view.SetRowCellValue(cell.RowHandle, "COLOR_NAME", pdmColorName);
                        }
                        else if (view.FocusedColumn.FieldName == "COLOR_NAME")
                        {
                            view.SetRowCellValue(cell.RowHandle, "PCX_COLOR_ID", pcxColorID);
                            view.SetRowCellValue(cell.RowHandle, "COLOR_CD", pdmColorCode);
                            view.SetRowCellValue(cell.RowHandle, "COLOR_NAME", pdmColorName);
                        }
                        else if (view.FocusedColumn.FieldName == "PART_NAME")
                        {
                            // When the part name is changed from 'Lamination' to general part.
                            if (oldValue.ToString().Contains("LAMINATION") == true && partName.Contains("LAMINATION") == false)
                            {
                                // An user doesn't need to set nike pattern part name for general part.
                                view.SetRowCellValue(cell.RowHandle, "PTRN_PART_NAME", "");
                                view.SetRowCellValue(cell.RowHandle, "PTRN_PART_CD", "");
                            }

                            view.SetRowCellValue(cell.RowHandle, "PART_NAME", partName);
                            view.SetRowCellValue(cell.RowHandle, "PART_CD", partCode);

                            if (partName.Contains("AIRBAG"))
                            {
                                // If the part name contains 'AIRBAG', fix BOM section to 'AIRBAG'.
                                view.SetRowCellValue(cell.RowHandle, "PART_TYPE", "AIRBAG");
                                view.SetRowCellValue(cell.RowHandle, "MDSL_CHK", "Y");
                                view.SetRowCellValue(cell.RowHandle, "OTSL_CHK", "N");
                            }
                            else if (partName.Contains("LAMINATION"))
                            {
                                // If the part name contains 'LAMINATION', fix BOM section to 'UPPER'.
                                view.SetRowCellValue(cell.RowHandle, "PART_TYPE", "UPPER");
                                view.SetRowCellValue(cell.RowHandle, "MDSL_CHK", "N");
                                view.SetRowCellValue(cell.RowHandle, "OTSL_CHK", "N");
                            }
                            else if (partType == "MIDSOLE" || partType == "OUTSOLE")
                            {
                                // If the part type is 'MIDSOLE' or 'OUTSOLE', fix BOM section to it.
                                view.SetRowCellValue(cell.RowHandle, "PART_TYPE", partType);
                                view.SetRowCellValue(cell.RowHandle, "MDSL_CHK", (partType == "MIDSOLE") ? "Y" : "N");
                                view.SetRowCellValue(cell.RowHandle, "OTSL_CHK", (partType == "OUTSOLE") ? "Y" : "N");
                            }
                            else if (pcxPartId == "3389" || pcxPartId == "3390")
                            {
                                // Fix BOM section to 'UPPER' for WET CHEMISTRY & WET CHEMISTRY-MULTI parts.
                                view.SetRowCellValue(cell.RowHandle, "PART_TYPE", "UPPER");
                                view.SetRowCellValue(cell.RowHandle, "MDSL_CHK", "N");
                                view.SetRowCellValue(cell.RowHandle, "OTSL_CHK", "N");
                            }
                            else
                            {
                                // // Follow existing BOM section(When a row is created, the BOM section follows the upper row).
                                view.SetRowCellValue(cell.RowHandle, "PART_TYPE", partType);
                                view.SetRowCellValue(cell.RowHandle, "MDSL_CHK", "N");
                                view.SetRowCellValue(cell.RowHandle, "OTSL_CHK", "N");
                            }

                            view.SetRowCellValue(cell.RowHandle, "PCX_PART_ID", pcxPartId);

                            Common.BindDefaultMaterialByNCFRule(view, cell.RowHandle, pcxPartId);
                        }
                        else if (view.FocusedColumn.FieldName == "PTRN_PART_NAME")
                        {
                            view.SetRowCellValue(cell.RowHandle, "PTRN_PART_NAME", partName);
                            view.SetRowCellValue(cell.RowHandle, "PTRN_PART_CD", partCode);
                        }
                        else if (view.FocusedColumn.FieldName == "MXSXL_NUMBER")
                        {
                            // LOGO, STICKER 자동 체크
                            if (pcxMatID == "87031" || pcxMatID == "78733" || pcxMatID == "79467")
                                view.SetRowCellValue(cell.RowHandle, "STICKER_YN", "Y");
                            else if (pcxMatID == "54638" || pcxMatID == "79341")
                                view.SetRowCellValue(cell.RowHandle, "COMBINE_YN", "Y");
                            else
                                view.SetRowCellValue(cell.RowHandle, "STICKER_YN", "N");

                            view.SetRowCellValue(cell.RowHandle, "MXSXL_NUMBER", mxsxlNumber);
                            view.SetRowCellValue(cell.RowHandle, "PCX_SUPP_MAT_ID", (pcxSuppMatID == "") ? "100" : pcxSuppMatID);
                            view.SetRowCellValue(cell.RowHandle, "CS_CD", csCode);
                            view.SetRowCellValue(cell.RowHandle, "MCS_NUMBER", mcsNumber);
                            view.SetRowCellValue(cell.RowHandle, "PCX_MAT_ID", (pcxMatID == "") ? "100" : pcxMatID);
                            view.SetRowCellValue(cell.RowHandle, "MAT_CD", pdmMatCode);
                            view.SetRowCellValue(cell.RowHandle, "MAT_NAME", (pdmMatName == "") ? "PLACEHOLDER" : pdmMatName);
                            view.SetRowCellValue(cell.RowHandle, "VENDOR_NAME", vendorName);
                            //view.SetRowCellValue(cell.RowHandle, "NIKE_MS_STATE", nikeMSState);

                            // Automatically tick for 'Code Transfer'.
                            if (mdmNotExisting == "Y")
                            {
                                view.SetRowCellValue(cell.RowHandle, "CDMKR_YN", "Y");
                                //view.SetRowCellValue(cell.RowHandle, "MDM_NOT_EXST", "Y");
                            }
                            else
                            {
                                view.SetRowCellValue(cell.RowHandle, "CDMKR_YN", "N");
                                //view.SetRowCellValue(cell.RowHandle, "MDM_NOT_EXST", "N");
                            }
                        }
                        else if (view.FocusedColumn.FieldName == "MAT_NAME")
                        {
                            // LOGO, STICKER 자동 체크
                            if (pcxMatID == "87031" || pcxMatID == "78733" || pcxMatID == "79467")
                                view.SetRowCellValue(cell.RowHandle, "STICKER_YN", "Y");
                            else if (pcxMatID == "54638" || pcxMatID == "79341")
                                view.SetRowCellValue(cell.RowHandle, "COMBINE_YN", "Y");
                            else
                                view.SetRowCellValue(cell.RowHandle, "STICKER_YN", "N");

                            view.SetRowCellValue(cell.RowHandle, "MXSXL_NUMBER", mxsxlNumber);
                            view.SetRowCellValue(cell.RowHandle, "PCX_SUPP_MAT_ID", (pcxSuppMatID == "") ? "100" : pcxSuppMatID);
                            view.SetRowCellValue(cell.RowHandle, "CS_CD", csCode);
                            view.SetRowCellValue(cell.RowHandle, "MCS_NUMBER", mcsNumber);
                            view.SetRowCellValue(cell.RowHandle, "PCX_MAT_ID", (pcxMatID == "") ? "100" : pcxMatID);
                            view.SetRowCellValue(cell.RowHandle, "MAT_CD", pdmMatCode);
                            view.SetRowCellValue(cell.RowHandle, "MAT_NAME", (pdmMatName == "") ? "PLACEHOLDER" : pdmMatName);
                            view.SetRowCellValue(cell.RowHandle, "VENDOR_NAME", vendorName);
                            //view.SetRowCellValue(cell.RowHandle, "NIKE_MS_STATE", nikeMSState);

                            // Automatically tick for 'Code Transfer'.
                            if (mdmNotExisting == "Y")
                            {
                                view.SetRowCellValue(cell.RowHandle, "CDMKR_YN", "Y");
                                //view.SetRowCellValue(cell.RowHandle, "MDM_NOT_EXST", "Y");
                            }
                            else
                            {
                                view.SetRowCellValue(cell.RowHandle, "CDMKR_YN", "N");
                                //view.SetRowCellValue(cell.RowHandle, "MDM_NOT_EXST", "N");
                            }
                        }
                        else if (view.FocusedColumn.FieldName == "MAT_COMMENT")
                        {
                            view.SetRowCellValue(cell.RowHandle, "MAT_COMMENT", value);

                            // When an user manually types data into material comment,
                            // clear the encoded comment to avoid discrepancy in data.
                            view.SetRowCellValue(cell.RowHandle, "ENCODED_CMT", "");
                        }
                        else if (view.FocusedColumn.FieldName == "COLOR_COMMENT")
                        {
                            view.SetRowCellValue(cell.RowHandle, "COLOR_COMMENT", value);
                        }
                        else if (view.FocusedColumn.FieldName == "PROCESS")
                        {
                            view.SetRowCellValue(cell.RowHandle, "PROCESS", value);
                        }
                        else if (view.FocusedColumn.FieldName == "REMARKS")
                        {
                            view.SetRowCellValue(cell.RowHandle, "REMARKS", value);
                        }
                        else if (view.FocusedColumn.FieldName == "PART_TYPE")
                        {
                            view.SetRowCellValue(cell.RowHandle, "PART_TYPE", value);
                        }
                        else if (view.FocusedColumn.FieldName == "BTTM")
                        {
                            view.SetRowCellValue(cell.RowHandle, "BTTM", value);
                        }
                        else if (view.FocusedColumn.FieldName == "PART_NIKE_COMMENT")
                        {
                            view.SetRowCellValue(cell.RowHandle, "PART_NIKE_COMMENT", value);
                        }

                        // 인디케이터를 "U"로 변경, 신규 행과 삭제할 행은 제외
                        if (rowStatus != "I")
                            view.SetRowCellValue(cell.RowHandle, "ROW_STATUS", "U");
                    }
                }

                // 스타일 새로 적용
                view.RefreshData();

                // 이벤트 다시 연결
                view.CellValueChanged += new CellValueChangedEventHandler(gvwMaster_CellValueChanged);
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
        private void gvwMaster_RowStyle(object sender, RowStyleEventArgs e)
        {
            GridView view = sender as GridView;

            // 아직 컬러가 부여되지 않은 라인
            if (e.RowHandle >= 0 && !APPLIED_ROWS.ContainsKey(e.RowHandle))
            {
                // 첫 라인부터 아래로 순서대로 컬러 부여
                if (e.RowHandle == 0)
                {
                    e.Appearance.BackColor = Color.White;
                    APPLIED_ROWS.Add(e.RowHandle, Color.White);
                }
                else if (APPLIED_ROWS.ContainsKey(e.RowHandle - 1))
                {
                    Color prevRowColor = APPLIED_ROWS[e.RowHandle - 1];
                    string criteria = view.GetRowCellValue(e.RowHandle, "BACK_COLOR").ToString();

                    if (criteria == "BACK")
                    {
                        if (prevRowColor == Color.White)
                        {
                            e.Appearance.BackColor = Color.LightSalmon;
                            APPLIED_ROWS.Add(e.RowHandle, Color.LightSalmon);
                        }
                        else if (prevRowColor == Color.LightSalmon)
                        {
                            e.Appearance.BackColor = Color.White;
                            APPLIED_ROWS.Add(e.RowHandle, Color.White);
                        }
                    }
                    else if (criteria == "FRONT")
                    {
                        e.Appearance.BackColor = prevRowColor;
                        APPLIED_ROWS.Add(e.RowHandle, prevRowColor);
                    }
                }
            }
            else if (APPLIED_ROWS.ContainsKey(e.RowHandle))
            {
                e.Appearance.BackColor = APPLIED_ROWS[e.RowHandle];
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void gvwMaster_CustomDrawRowIndicator(object sender, RowIndicatorCustomDrawEventArgs e)
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
        private void gvwMaster_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Delete)
            {
                GridView view = sender as GridView;
                GridCell[] cells = view.GetSelectedCells();

                try
                {
                    view.CellValueChanged -= new CellValueChangedEventHandler(gvwMaster_CellValueChanged);

                    foreach (GridCell cell in cells)
                    {
                        string fieldName = cell.Column.FieldName;

                        if (fieldName == "PART_NIKE_NO" || fieldName == "UPD_USER" || fieldName == "UPD_YMD")
                            continue;

                        string rowStatus = view.GetRowCellValue(cell.RowHandle, "ROW_STATUS").ToString();
                        string lockYN = view.GetRowCellValue(cell.RowHandle, "LOCK_YN").ToString();

                        if (rowStatus != "D" && lockYN.Equals("N"))
                        {
                            if (fieldName == "PTRN_PART_NAME")
                            {
                                view.SetRowCellValue(cell.RowHandle, "PTRN_PART_CD", "");
                                view.SetRowCellValue(cell.RowHandle, "PTRN_PART_NAME", "");
                            }
                            else if (fieldName == "CS_PTRN_NAME")
                            {
                                view.SetRowCellValue(cell.RowHandle, "CS_PTRN_NAME", "");
                                view.SetRowCellValue(cell.RowHandle, "CS_PTRN_CD", "");
                            }
                            else if (fieldName == "MAT_COMMENT")
                            {
                                view.SetRowCellValue(cell.RowHandle, "MAT_COMMENT", "");
                                view.SetRowCellValue(cell.RowHandle, "ENCODED_CMT", "");
                            }
                            else
                                view.SetRowCellValue(cell.RowHandle, cell.Column, "");

                            if (rowStatus != "I")
                                view.SetRowCellValue(cell.RowHandle, "ROW_STATUS", "U");
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
                    view.CellValueChanged += new CellValueChangedEventHandler(gvwMaster_CellValueChanged);
                };
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void gvwMaster_MouseDown(object sender, MouseEventArgs e)
        {
            try
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
                        // 다른 셀이 선택된 상태로 체크박스 표기 시 잘못된 데이터가 기입됨
                        view.ClearSelection();

                        // 에디터 생성 전 행/열 선택
                        view.FocusedColumn = hitInfo.Column;
                        view.FocusedRowHandle = hitInfo.RowHandle;

                        // ShowEditor : Creates an editor for the cell
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

                            if (rowStatus != "I")
                                view.SetRowCellValue(view.FocusedRowHandle, "ROW_STATUS", "U");
                        }

                        DXMouseEventArgs.GetMouseArgs(e).Handled = true;
                    }
                }
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
        private void gvwMaster_ShowingEditor(object sender, CancelEventArgs e)
        {
            GridView view = sender as GridView;

            if (view.GetRowCellValue(view.FocusedRowHandle, "ROW_STATUS").ToString().Equals("D") ||
                view.GetRowCellValue(view.FocusedRowHandle, "LOCK_YN").ToString().Equals("Y"))
                e.Cancel = true;
        }

        #endregion

        #region 폼 이벤트

        private void CompareBOM_FormClosing(object sender, FormClosingEventArgs e)
        {
            projectBaseForm.QueryClick();
        }

        #endregion
    }
}