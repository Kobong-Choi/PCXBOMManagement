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
    public partial class BOMHeadUpdater : DevExpress.XtraEditors.XtraForm
    {
        public string Factory { get; set; }
        public string WSNumber { get; set; }

        public BOMHeadUpdater()
        {
            InitializeComponent();
        }

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);

            // 해외는 GEL 관리 X
            if (!Common.sessionFactory.Equals("DS"))
                gvwHeadData.Columns["GEL_YN"].Visible = false;

            #region Bind dataSource to LookUpEdit.

            DataTable resultValue = null;

            // Season
            PKG_INTG_BOM_COMMON.SELECT_COMMON_CODE pkgSelectSeason = new PKG_INTG_BOM_COMMON.SELECT_COMMON_CODE();
            pkgSelectSeason.ARG_TYPE = "SEASON";
            pkgSelectSeason.ARG_SORT_TYPE = "DESC";
            pkgSelectSeason.OUT_CURSOR = string.Empty;

            resultValue = Common.projectBaseForm.Exe_Select_PKG(pkgSelectSeason).Tables[0];
            lookUpSeason.DataSource = resultValue;

            // Sample Type
            PKG_INTG_BOM_COMMON.SELECT_SAMPLE_TYPE pkgSelectSampleType = new PKG_INTG_BOM_COMMON.SELECT_SAMPLE_TYPE();
            pkgSelectSampleType.ARG_FACTORY = this.Factory;
            pkgSelectSampleType.ARG_ST_DIV = "B";
            pkgSelectSampleType.ARG_USE_YN = "Y";
            pkgSelectSampleType.OUT_CURSOR = string.Empty;

            resultValue = Common.projectBaseForm.Exe_Select_PKG(pkgSelectSampleType).Tables[0];
            lookUpSampleType.DataSource = resultValue;

            // Sub Type
            PKG_INTG_BOM_COMMON.SELECT_SUB_SAMPLE_TYPE pkgSelectSubType = new PKG_INTG_BOM_COMMON.SELECT_SUB_SAMPLE_TYPE();
            pkgSelectSubType.ARG_ST_DIV = "S";
            pkgSelectSubType.OUT_CURSOR = string.Empty;

            resultValue = Common.projectBaseForm.Exe_Select_PKG(pkgSelectSubType).Tables[0];
            lookUpSubType.DataSource = resultValue;

            //// PCC Org
            //PKG_INTG_BOM_COMMON.SELECT_COMMON_CODE pkgSelectOrg = new PKG_INTG_BOM_COMMON.SELECT_COMMON_CODE();
            //pkgSelectOrg.ARG_TYPE = "PCC_ORG";
            //pkgSelectOrg.ARG_SORT_TYPE = "ASC";
            //pkgSelectOrg.OUT_CURSOR = string.Empty;

            //resultValue = projectBaseForm.Exe_Select_PKG(pkgSelectOrg).Tables[0];
            //lookUpOrg.DataSource = resultValue;

            // PCC Category
            PKG_INTG_BOM_COMMON.SELECT_CATEGORY pkgSelectCategory = new PKG_INTG_BOM_COMMON.SELECT_CATEGORY();
            pkgSelectCategory.ARG_GROUP_CODE = "BOM_CAT";
            pkgSelectCategory.ARG_CODE_VALUE = "WMT";
            pkgSelectCategory.OUT_CURSOR = string.Empty;

            resultValue = Common.projectBaseForm.Exe_Select_PKG(pkgSelectCategory).Tables[0];
            lookUpCategory.DataSource = resultValue;

            // GENDER
            PKG_INTG_BOM_COMMON.SELECT_GENDER pkgSelectGender = new PKG_INTG_BOM_COMMON.SELECT_GENDER();
            pkgSelectGender.ARG_ATTRIBUTE_NAME = "Gender";
            pkgSelectGender.OUT_CURSOR = string.Empty;

            resultValue = Common.projectBaseForm.Exe_Select_PKG(pkgSelectGender).Tables[0];
            lookUpGender.DataSource = resultValue;

            // TD
            PKG_INTG_BOM_COMMON.SELECT_COMMON_CODE pksgSelectTd = new PKG_INTG_BOM_COMMON.SELECT_COMMON_CODE();
            pksgSelectTd.ARG_TYPE = "TD";
            pksgSelectTd.ARG_SORT_TYPE = "ASC";
            pksgSelectTd.OUT_CURSOR = string.Empty;

            resultValue = Common.projectBaseForm.Exe_Select_PKG(pksgSelectTd).Tables[0];
            lookUpTd.DataSource = resultValue;

            // PCC PM
            PKG_INTG_BOM_COMMON.SELECT_DEVELOPER pkgSelectDeveloper = new PKG_INTG_BOM_COMMON.SELECT_DEVELOPER();
            pkgSelectDeveloper.ARG_FACTORY = this.Factory;
            pkgSelectDeveloper.OUT_CURSOR = string.Empty;

            resultValue = Common.projectBaseForm.Exe_Select_PKG(pkgSelectDeveloper).Tables[0];
            lookUpDeveloper.DataSource = resultValue;

            #endregion

            PKG_INTG_BOM.SELECT_BOM_HEAD_DATA pkgSelectHeadData = new PKG_INTG_BOM.SELECT_BOM_HEAD_DATA();
            pkgSelectHeadData.ARG_FACTORY = this.Factory;
            pkgSelectHeadData.ARG_CONCAT_WS_NO = this.WSNumber;
            pkgSelectHeadData.OUT_CURSOR = string.Empty;

            DataTable bomHeadData = Common.projectBaseForm.Exe_Select_PKG(pkgSelectHeadData).Tables[0];
            bomHeadData.Columns["STYLE_CD"].MaxLength = 10;
            bomHeadData.Columns["SUB_TYPE_REMARK"].MaxLength = 200;

            grdHeadData.DataSource = bomHeadData;

        }

        #region 버튼 이벤트

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnSave_Click(object sender, EventArgs e)
        {
            ArrayList arrayList = new ArrayList();

            AutoReplaceCharacter();

            for (int rowHandle = 0; rowHandle < gvwHeadData.RowCount; rowHandle++)
            {
                #region GEL 확인

                bool isGel = gvwHeadData.GetRowCellValue(rowHandle, "GEL_YN").ToString().Equals("Y") ? true : false;
                string sampleType = gvwHeadData.GetRowCellValue(rowHandle, "ST_CD").ToString();
                string sampleQTY = gvwHeadData.GetRowCellValue(rowHandle, "SAMPLE_QTY").ToString();

                // GEL 모델인 경우 
                if (isGel)
                {
                    // 선택 가능한 라운드 입력 여부 확인
                    if (new string[] { "FRST", "SCND", "THRD", "PRDC" }.Contains(sampleType) == false)
                    {
                        if (MessageBox.Show("The Sample Type doesn't match the GEL.\nDo you want to proceed?", "Warning",
                            MessageBoxButtons.OKCancel, MessageBoxIcon.Warning) == System.Windows.Forms.DialogResult.Cancel)
                        {
                            Common.FocusCell(gvwHeadData, rowHandle, "ST_CD", true);
                            return;
                        }
                    }

                    // 적정 수량 입력 여부 확인
                    if (Convert.ToDouble(sampleQTY) < 10)
                    {
                        if (MessageBox.Show("Is the Sample QTY correct? normally more than 10.", "Warning",
                            MessageBoxButtons.OKCancel, MessageBoxIcon.Warning) == System.Windows.Forms.DialogResult.Cancel)
                        {
                            Common.FocusCell(gvwHeadData, rowHandle, "ST_CD", true);
                            return;
                        }
                    }
                }

                // GEL이 아닌데 GEL 라운드를 선택한 경우
                if (!isGel && (new string[] { "FRST", "SCND", "THRD" }.Contains(sampleType)))
                {
                    if (MessageBox.Show("Is The Sample Type you selected correct? it's for GEL.", "Warning",
                        MessageBoxButtons.OKCancel, MessageBoxIcon.Warning) == System.Windows.Forms.DialogResult.Cancel)
                    {
                        Common.FocusCell(gvwHeadData, rowHandle, "ST_CD", true);
                        return;
                    }
                }

                #endregion

                #region BOM Unique 여부 확인

                /********************************************************************************************
                     * BOM Unique 기준 : Factory, Season, Sample Type, Sub Type, Colorway ID, Sub Type Remark
                *********************************************************************************************/

                PKG_INTG_BOM.SELECT_IS_EXISTING_BOM pkgSelect = new PKG_INTG_BOM.SELECT_IS_EXISTING_BOM();
                pkgSelect.ARG_FACTORY = gvwHeadData.GetRowCellValue(rowHandle, "FACTORY").ToString();
                pkgSelect.ARG_WS_NO = gvwHeadData.GetRowCellValue(rowHandle, "WS_NO").ToString();
                pkgSelect.ARG_SEASON_CD = gvwHeadData.GetRowCellValue(rowHandle, "SEASON_CD").ToString();
                pkgSelect.ARG_DEV_COLORWAY_ID = gvwHeadData.GetRowCellValue(rowHandle, "DEV_COLORWAY_ID").ToString();
                pkgSelect.ARG_SAMPLE_TYPE = gvwHeadData.GetRowCellValue(rowHandle, "ST_CD").ToString();
                pkgSelect.ARG_SUB_TYPE = gvwHeadData.GetRowCellValue(rowHandle, "SUB_ST_CD").ToString();
                pkgSelect.ARG_SUB_TYPE_REMARK = gvwHeadData.GetRowCellValue(rowHandle, "SUB_TYPE_REMARK").ToString(); ;
                pkgSelect.OUT_CURSOR = string.Empty;

                DataTable returnValue = Common.projectBaseForm.Exe_Select_PKG(pkgSelect).Tables[0];

                int rowCount = Convert.ToInt32(returnValue.Rows[0]["ROW_COUNT"]);   // 동일한 BOM의 개수
                bool isExisting = (rowCount == 0) ? false : true;                   // 동일한 BOM 존재 여부

                #endregion

                if (!isExisting)
                {
                    PKG_INTG_BOM.INSERT_BOM_RECORD_HEAD pkgInsert = new PKG_INTG_BOM.INSERT_BOM_RECORD_HEAD();
                    pkgInsert.ARG_FACTORY = gvwHeadData.GetRowCellValue(rowHandle, "FACTORY").ToString();
                    pkgInsert.ARG_WS_NO = gvwHeadData.GetRowCellValue(rowHandle, "WS_NO").ToString();
                    pkgInsert.ARG_DPA = gvwHeadData.GetRowCellValue(rowHandle, "DPA").ToString();
                    pkgInsert.ARG_BOM_ID = gvwHeadData.GetRowCellValue(rowHandle, "BOM_ID").ToString();
                    pkgInsert.ARG_ST_CD = gvwHeadData.GetRowCellValue(rowHandle, "ST_CD").ToString();
                    pkgInsert.ARG_SEASON_CD = gvwHeadData.GetRowCellValue(rowHandle, "SEASON_CD").ToString();
                    //pkgInsert.ARG_PCC_ORG = gvwHeadData.GetRowCellValue(rowHandle, "PCC_ORG").ToString();
                    pkgInsert.ARG_CATEGORY = gvwHeadData.GetRowCellValue(rowHandle, "CATEGORY").ToString();
                    pkgInsert.ARG_DEV_NAME = gvwHeadData.GetRowCellValue(rowHandle, "DEV_NAME").ToString();
                    pkgInsert.ARG_STYLE_CD = gvwHeadData.GetRowCellValue(rowHandle, "STYLE_CD").ToString();
                    pkgInsert.ARG_SAMPLE_ETS = gvwHeadData.GetRowCellValue(rowHandle, "SAMPLE_ETS").ToString();
                    pkgInsert.ARG_SAMPLE_QTY = gvwHeadData.GetRowCellValue(rowHandle, "SAMPLE_QTY").ToString();
                    pkgInsert.ARG_SAMPLE_SIZE = gvwHeadData.GetRowCellValue(rowHandle, "SAMPLE_SIZE").ToString();
                    pkgInsert.ARG_SUB_TYPE_REMARK = gvwHeadData.GetRowCellValue(rowHandle, "SUB_TYPE_REMARK").ToString();
                    pkgInsert.ARG_GENDER = gvwHeadData.GetRowCellValue(rowHandle, "GENDER").ToString();
                    pkgInsert.ARG_TD = gvwHeadData.GetRowCellValue(rowHandle, "TD").ToString();
                    pkgInsert.ARG_COLOR_VER = gvwHeadData.GetRowCellValue(rowHandle, "COLOR_VER").ToString();
                    pkgInsert.ARG_MODEL_ID = gvwHeadData.GetRowCellValue(rowHandle, "MODEL_ID").ToString();
                    pkgInsert.ARG_PCC_PM = gvwHeadData.GetRowCellValue(rowHandle, "PCC_PM").ToString();
                    pkgInsert.ARG_PRODUCT_ID = gvwHeadData.GetRowCellValue(rowHandle, "PRODUCT_ID").ToString();
                    pkgInsert.ARG_DEV_COLORWAY_ID = gvwHeadData.GetRowCellValue(rowHandle, "DEV_COLORWAY_ID").ToString();
                    pkgInsert.ARG_DEV_STYLE_NUMBER = gvwHeadData.GetRowCellValue(rowHandle, "DEV_STYLE_NUMBER").ToString();
                    pkgInsert.ARG_DEV_SAMPLE_REQ_ID = gvwHeadData.GetRowCellValue(rowHandle, "DEV_SAMPLE_REQ_ID").ToString();
                    pkgInsert.ARG_UPD_USER = Common.sessionID;
                    pkgInsert.ARG_GEL_YN = gvwHeadData.GetRowCellValue(rowHandle, "GEL_YN").ToString();

                    arrayList.Add(pkgInsert);
                }
                else
                {
                    Common.ShowMessageBox("A BOM with the same information already exists.\nPlease check the criteria of unique", "W");
                    Common.FocusCell(gvwHeadData, rowHandle, "SUB_TYPE_REMARK", true);
                    return;
                }
            }

            if (Common.projectBaseForm.Exe_Modify_PKG(arrayList) == null)
            {
                Common.ShowMessageBox("Failed to save.", "E");
                return;
            }

            this.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.Close();
        }

        /// <summary>
        /// Replace the character '-' to ''.
        /// </summary>
        private void AutoReplaceCharacter()
        {
            string val = string.Empty;

            Action<int, string> action = (i, fieldName) =>
            {
                val = gvwHeadData.GetRowCellValue(i, fieldName).ToString();

                if (val.Contains("-"))
                {
                    val = val.Replace("-", "");
                    gvwHeadData.SetRowCellValue(i, fieldName, val.Trim());
                }
            };

            try
            {

                gvwHeadData.CellValueChanged -= new CellValueChangedEventHandler(gvwHeadData_CellValueChanged);

                for (int rowHandle = 0; rowHandle < gvwHeadData.RowCount; rowHandle++)
                {
                    action(rowHandle, "STYLE_CD");
                    action(rowHandle, "DPA");
                }
            }
            finally
            {
                gvwHeadData.CellValueChanged += new CellValueChangedEventHandler(gvwHeadData_CellValueChanged);
            }
        }

        #endregion

        #region 그리드 이벤트

        /// <summary>
        /// 셀에 데이터가 입력될 때 실행
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void gvwHeadData_CellValueChanged(object sender, CellValueChangedEventArgs e)
        {
            try
            {
                gvwHeadData.CellValueChanged -= new DevExpress.XtraGrid.Views.Base.CellValueChangedEventHandler(gvwHeadData_CellValueChanged);

                GridView view = sender as GridView;
                // 데이터를 변경한 컬럼
                string fcsdColumnName = gvwHeadData.FocusedColumn.FieldName;
                if (fcsdColumnName == "SAMPLE_ETS")
                {
                    // 입력한 값
                    string orgValue = gvwHeadData.GetFocusedRowCellValue("SAMPLE_ETS").ToString();
                    // LookUpEdit에서 직접 선택한 경우 Date Format이므로 타입 변경
                    if (orgValue.Length > 8)
                    {
                        // DateTime 형태로 변환
                        DateTime dateTime = Convert.ToDateTime(orgValue);
                        // 다시 yyyyMMdd 형태로 변환
                        string sampleETS = dateTime.ToString("yyyyMMdd");
                        // 셀에 다시 바인딩
                        gvwHeadData.SetFocusedRowCellValue("SAMPLE_ETS", sampleETS);
                    }
                }
                gvwHeadData.CellValueChanged += new DevExpress.XtraGrid.Views.Base.CellValueChangedEventHandler(gvwHeadData_CellValueChanged);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
                return;
            }
        }

        /// <summary>
        /// 변경 불가 컬럼 색상 표기
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void gvwHeadData_RowCellStyle(object sender, RowCellStyleEventArgs e)
        {
            try
            {
                GridView view = sender as GridView;

                if (e.Column.FieldName == "FACTORY" || e.Column.FieldName == "ST_CD" ||
                    e.Column.FieldName == "SAMPLE_ETS" || e.Column.FieldName == "SUB_ST_CD")
                {
                    if (view.IsCellSelected(e.RowHandle, e.Column) == false)
                        e.Appearance.BackColor = Color.Coral;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
                return;
            }
        }

        /// <summary>
        /// 그리드에서 특정 키를 누를 경우 동작
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void gvwHeadData_KeyDown(object sender, KeyEventArgs e)
        {
            try
            {
                GridView view = sender as GridView;
                string fcsdFieldName = view.FocusedColumn.FieldName;

                if (e.KeyCode == Keys.Delete)
                {
                    GridCell[] cells = view.GetSelectedCells();

                    foreach (GridCell cell in cells)
                    {
                        // 아래 컬럼은 수정 불가
                        if (cell.Column.FieldName == "FACTORY" || cell.Column.FieldName == "ST_CD" ||
                            cell.Column.FieldName == "SAMPLE_ETS" || cell.Column.FieldName == "SUB_ST_CD")
                            continue;
                        else
                            view.SetRowCellValue(cell.RowHandle, cell.Column, "");
                    }
                }
                else if (e.KeyCode == Keys.Enter)
                {
                    // 현재 입력한 값
                    if (view.EditingValue != null)
                    {
                        // 복사할 값
                        string value = string.Empty;
                        // 입력 중인 값의 데이터 타입 -> Sample ETS 예외 처리용
                        Type type = view.EditingValue.GetType();
                        // DateTime일 경우 'yyyMMdd' 형태로 변환
                        if (type.Name == "DateTime")
                        {
                            string orgValue = view.EditingValue.ToString();
                            DateTime time = Convert.ToDateTime(orgValue);
                            value = time.ToString("yyyyMMdd");
                        }
                        else // String 타입은 그냥 복사
                            value = view.EditingValue.ToString();

                        // 선택된 셀들
                        GridCell[] cells = view.GetSelectedCells();
                        // 각 셀의 데이터를 복사할 값으로 업데이트
                        foreach (GridCell cell in cells)
                            view.SetRowCellValue(cell.RowHandle, cell.Column, value);
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
        private void CustomMouseDown(object sender, MouseEventArgs e)
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

                        #region GEL 업데이트 가능 여부 확인

                        PKG_INTG_BOM.SELECT_BOM_INFO_BY_CONFIRM pkgSelect = new PKG_INTG_BOM.SELECT_BOM_INFO_BY_CONFIRM();
                        pkgSelect.ARG_WORK_TYPE = "PurchaseCheck";
                        pkgSelect.ARG_FACTORY = view.GetFocusedRowCellValue("FACTORY").ToString();
                        pkgSelect.ARG_WS_NO = view.GetFocusedRowCellValue("WS_NO").ToString();
                        pkgSelect.ARG_PART_SEQ = "";
                        pkgSelect.OUT_CURSOR = string.Empty;

                        DataTable dtResult = Common.projectBaseForm.Exe_Select_PKG(pkgSelect).Tables[0];

                        if (dtResult.Rows.Count > 0)
                        {
                            if (Convert.ToInt32(dtResult.Rows[0]["CNT"]) > 0)
                            {
                                MessageBox.Show("Cannot be updated for the BOM whose materials were already purchased. Please ask IT team.", "Warning",
                                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                                return;
                            }
                        }

                        #endregion

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
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
                return;
            }
        }

        #endregion
    }
}