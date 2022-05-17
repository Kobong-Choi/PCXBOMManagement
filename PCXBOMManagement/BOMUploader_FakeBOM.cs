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
using CSI.PCC.PCX.Packages;

namespace CSI.PCC.PCX
{
    public partial class BOMUploader_FakeBOM : DevExpress.XtraEditors.XtraForm
    {
        public ProjectBaseForm projectBaseForm = Common.projectBaseForm;
        // true : 저장 후 종료, false : 저장하지 않고 종료
        public bool SAVED = false;

        public BOMUploader_FakeBOM()
        {
            InitializeComponent();
        }

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);

            try
            {
                #region 룩업 데이터 바인딩

                DataTable resultValue = null;
                // Season
                PKG_INTG_BOM_COMMON.SELECT_COMMON_CODE pkgSelectSeason = new PKG_INTG_BOM_COMMON.SELECT_COMMON_CODE();
                pkgSelectSeason.ARG_TYPE = "SEASON";
                pkgSelectSeason.ARG_SORT_TYPE = "DESC";
                pkgSelectSeason.OUT_CURSOR = string.Empty;

                resultValue = projectBaseForm.Exe_Select_PKG(pkgSelectSeason).Tables[0];
                lookUpSeason.DataSource = resultValue;
                // Sample Type
                PKG_INTG_BOM_COMMON.SELECT_SAMPLE_TYPE pkgSelectSampleType = new PKG_INTG_BOM_COMMON.SELECT_SAMPLE_TYPE();
                pkgSelectSampleType.ARG_FACTORY = Common.sessionFactory;
                pkgSelectSampleType.ARG_ST_DIV = "B";
                pkgSelectSampleType.ARG_USE_YN = "Y";
                pkgSelectSampleType.OUT_CURSOR = string.Empty;

                resultValue = projectBaseForm.Exe_Select_PKG(pkgSelectSampleType).Tables[0];
                lookUpSampleType.DataSource = resultValue;
                
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

                resultValue = projectBaseForm.Exe_Select_PKG(pkgSelectCategory).Tables[0];
                lookUpCategory.DataSource = resultValue;
                // GENDER
                PKG_INTG_BOM_COMMON.SELECT_GENDER pkgSelectGender = new PKG_INTG_BOM_COMMON.SELECT_GENDER();
                pkgSelectGender.ARG_ATTRIBUTE_NAME = "Gender";
                pkgSelectGender.OUT_CURSOR = string.Empty;

                resultValue = projectBaseForm.Exe_Select_PKG(pkgSelectGender).Tables[0];
                lookUpGender.DataSource = resultValue;
                // TD
                PKG_INTG_BOM_COMMON.SELECT_COMMON_CODE pksgSelectTd = new PKG_INTG_BOM_COMMON.SELECT_COMMON_CODE();
                pksgSelectTd.ARG_TYPE = "TD";
                pksgSelectTd.ARG_SORT_TYPE = "ASC";
                pksgSelectTd.OUT_CURSOR = string.Empty;

                resultValue = projectBaseForm.Exe_Select_PKG(pksgSelectTd).Tables[0];
                lookUpTd.DataSource = resultValue;
                // PCC PM
                PKG_INTG_BOM_COMMON.SELECT_DEVELOPER pkgSelectDeveloper = new PKG_INTG_BOM_COMMON.SELECT_DEVELOPER();
                pkgSelectDeveloper.ARG_FACTORY = Common.sessionFactory;
                pkgSelectDeveloper.OUT_CURSOR = string.Empty;

                resultValue = projectBaseForm.Exe_Select_PKG(pkgSelectDeveloper).Tables[0];
                lookUpDeveloper.DataSource = resultValue;
                #endregion

                #region Fake BOM 헤더 생성 및 그리드에 바인딩

                PKG_INTG_BOM.SELECT_FAKE_BOM_HEAD pkgSelect = new PKG_INTG_BOM.SELECT_FAKE_BOM_HEAD();
                pkgSelect.ARG_FACTORY = Common.sessionFactory;
                pkgSelect.ARG_UPD_USER = Common.sessionID;
                pkgSelect.OUT_CURSOR = string.Empty;

                DataTable dataSource = projectBaseForm.Exe_Select_PKG(pkgSelect).Tables[0];
                dataSource.Columns["STYLE_CD"].MaxLength = 10;
                grdHeadData.DataSource = dataSource;

                #endregion
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
                return;
            }
        }

        #region 버튼 이벤트

        /// <summary>
        /// Fake BOM 헤더 정보 업데이트
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnSave_Click(object sender, EventArgs e)
        {
            if (CheckSaveValidation())
            {
                try
                {
                    #region 패키지 매개변수 입력

                    PKG_INTG_BOM.UPDATE_BOM_HEAD pkgUpdate = new PKG_INTG_BOM.UPDATE_BOM_HEAD();

                    pkgUpdate.ARG_FACTORY = gvwHeadData.GetRowCellValue(0, "FACTORY").ToString();
                    pkgUpdate.ARG_WS_NO = gvwHeadData.GetRowCellValue(0, "WS_NO").ToString();

                    string DPA = gvwHeadData.GetRowCellValue(0, "DPA").ToString();
                    string _DPA = DPA.Replace("-", "");

                    pkgUpdate.ARG_DPA = _DPA;

                    pkgUpdate.ARG_PCM_BOM_ID = gvwHeadData.GetRowCellValue(0, "BOM_ID").ToString();
                    pkgUpdate.ARG_SAMPLE_TYPE = gvwHeadData.GetRowCellValue(0, "ST_CD").ToString();
                    pkgUpdate.ARG_SEASON_CD = gvwHeadData.GetRowCellValue(0, "SEASON_CD").ToString();
                    //pkgUpdate.ARG_PCC_ORG = gvwHeadData.GetRowCellValue(0, "PCC_ORG").ToString();
                    pkgUpdate.ARG_CATEGORY = gvwHeadData.GetRowCellValue(0, "CATEGORY").ToString();
                    pkgUpdate.ARG_DEV_NAME = gvwHeadData.GetRowCellValue(0, "DEV_NAME").ToString();
                    pkgUpdate.ARG_PRODUCT_CODE = gvwHeadData.GetRowCellValue(0, "STYLE_CD").ToString();

                    string sampleETS = gvwHeadData.GetRowCellValue(0, "SAMPLE_ETS").ToString();

                    if (sampleETS != "")
                    {
                        DateTime _sampleETS = Convert.ToDateTime(sampleETS);
                        string __sampleETS = _sampleETS.ToString("yyyyMMdd");
                        pkgUpdate.ARG_SAMPLE_ETS = __sampleETS;
                    }
                    else
                        pkgUpdate.ARG_SAMPLE_ETS = "";

                    pkgUpdate.ARG_SAMPLE_QTY = gvwHeadData.GetRowCellValue(0, "SAMPLE_QTY").ToString();
                    pkgUpdate.ARG_SAMPLE_SIZE = gvwHeadData.GetRowCellValue(0, "SAMPLE_SIZE").ToString();
                    pkgUpdate.ARG_SUB_TYPE_REMARK = gvwHeadData.GetRowCellValue(0, "SUB_TYPE_REMARK").ToString();
                    pkgUpdate.ARG_GENDER = gvwHeadData.GetRowCellValue(0, "GENDER").ToString();
                    pkgUpdate.ARG_TD = gvwHeadData.GetRowCellValue(0, "TD").ToString();
                    pkgUpdate.ARG_COLOR_VER = gvwHeadData.GetRowCellValue(0, "COLOR_VER").ToString();
                    pkgUpdate.ARG_MODEL_ID = gvwHeadData.GetRowCellValue(0, "MODEL_ID").ToString();
                    pkgUpdate.ARG_PCC_PM = gvwHeadData.GetRowCellValue(0, "PCC_PM").ToString();
                    pkgUpdate.ARG_DEV_COLORWAY_ID = gvwHeadData.GetRowCellValue(0, "DEV_COLORWAY_ID").ToString();
                    pkgUpdate.ARG_PRODUCT_ID = gvwHeadData.GetRowCellValue(0, "PRODUCT_ID").ToString();
                    pkgUpdate.ARG_DEV_SAMPLE_REQ_ID = gvwHeadData.GetRowCellValue(0, "DEV_SAMPLE_REQ_ID").ToString();
                    pkgUpdate.ARG_STYLE_NUMBER = gvwHeadData.GetRowCellValue(0, "DEV_STYLE_NUMBER").ToString();
                    pkgUpdate.ARG_GEL_YN = gvwHeadData.GetRowCellValue(0, "GEL_YN").ToString();

                    #endregion

                    ArrayList arrayList = new ArrayList();
                    
                    arrayList.Add(pkgUpdate);
                    
                    projectBaseForm.Exe_Modify_PKG(arrayList);
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.ToString());
                    return;
                }
                
                SAVED = true;
                
                MessageBox.Show("Save Complete");
                
                DialogResult = System.Windows.Forms.DialogResult.OK;
                
                this.Close();
            }
        }

        /// <summary>
        /// 저장 전에 필수 입력 속성 값 확인
        /// </summary>
        /// <returns></returns>
        private bool CheckSaveValidation()
        {
            try
            {
                #region Season
                string seasonCode = gvwHeadData.GetRowCellValue(0, "SEASON_CD").ToString();
                if (seasonCode == "")
                {
                    MessageBox.Show("Season is a required field.");
                    SetFocusColumn(0, "SEASON_CD");
                    return false;
                }
                #endregion

                #region Style Name
                string styleName = gvwHeadData.GetRowCellValue(0, "DEV_NAME").ToString();
                if (styleName == "")
                {
                    MessageBox.Show("Style Name is a required field.");
                    SetFocusColumn(0, "DEV_NAME");
                    return false;
                }
                #endregion

                #region Dev Colorway ID
                string colorwayID = gvwHeadData.GetRowCellValue(0, "DEV_COLORWAY_ID").ToString();
                if (colorwayID == "")
                {
                    MessageBox.Show("Colorway ID is required field.");
                    SetFocusColumn(0, "DEV_COLORWAY_ID");
                    return false;
                }
                #endregion

                #region Sample Type
                string sampleTypeCode = gvwHeadData.GetRowCellValue(0, "ST_CD").ToString();
                if (sampleTypeCode == "")
                {
                    MessageBox.Show("Sample Type is a required field.");
                    SetFocusColumn(0, "ST_CD");
                    return false;
                }
                #endregion

                #region Sample SIZE
                string sampleSize = gvwHeadData.GetRowCellValue(0, "SAMPLE_SIZE").ToString();
                if (sampleSize == "")
                {
                    MessageBox.Show("Sample Size is a required field.");
                    SetFocusColumn(0, "SAMPLE_SIZE");
                    return false;
                }
                #endregion

                #region Sample QTY
                string sampleQty = gvwHeadData.GetRowCellValue(0, "SAMPLE_QTY").ToString();
                try
                {
                    double cnvtSampleQty = Convert.ToDouble(sampleQty);
                    if (cnvtSampleQty == 0)
                    {
                        MessageBox.Show("Sample Qty should be more than zero.");
                        SetFocusColumn(0, "SAMPLE_QTY");
                        return false;
                    }
                }
                catch
                {
                    MessageBox.Show("Sample Qty should be a digit number.");
                    SetFocusColumn(0, "SAMPLE_QTY");
                    return false;
                }
                #endregion

                return true;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
                return false;
            }
        }

        /// <summary>
        /// 매개변수에 지정된 셀 선택
        /// </summary>
        /// <param name="rowHandle"></param>
        /// <param name="columnName"></param>
        private void SetFocusColumn(int rowHandle, string columnName)
        {
            // 기존에 선택된 셀의 포커싱 취소
            gvwHeadData.ClearSelection();
            // 포커스할 로우핸들 설정
            gvwHeadData.FocusedRowHandle = rowHandle;
            // 포커스할 컬럼 설정
            gvwHeadData.FocusedColumn = gvwHeadData.Columns[columnName];
            // 입력해야할 셀 선택
            gvwHeadData.SelectCell(rowHandle, gvwHeadData.Columns[columnName]);
            // 에디팅 모드 띄움
            gvwHeadData.ShowEditor();
        }

        #endregion

        #region 폼 이벤트

        /// <summary>
        /// 저장하지 않고 종료 시 생성된 데이터 삭제
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void FakeBOMUploader_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (SAVED == false)
            {
                if (DialogResult.Cancel == MessageBox.Show("Do you really want to close this form without saving data?", "Read it in detail",
                                            MessageBoxButtons.OKCancel, MessageBoxIcon.Question, MessageBoxDefaultButton.Button1))
                    e.Cancel = true; // 폼 클로징 취소
                else
                {
                    // 생성된 BOM 헤더/이력 테이블 데이터 삭제
                    try
                    {
                        PKG_INTG_BOM.DELETE_BOM_DATA pkgDeleteData = new PKG_INTG_BOM.DELETE_BOM_DATA();
                        pkgDeleteData.ARG_PCC = Common.sessionFactory;
                        pkgDeleteData.ARG_CONCAT_WS_NO = gvwHeadData.GetRowCellValue(0, "WS_NO").ToString();
                        pkgDeleteData.ARG_DELETE_USER = Common.sessionID;
                        
                        ArrayList arrayList = new ArrayList();
                        arrayList.Add(pkgDeleteData);
                        
                        projectBaseForm.Exe_Modify_PKG(arrayList);
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(ex.ToString());
                        return;
                    }
                    finally
                    {
                        DialogResult = System.Windows.Forms.DialogResult.Ignore;
                    }
                }
            }
        }

        #endregion

        #region 그리드 이벤트

        /// <summary>
        /// 그리드 셀 스타일 설정
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void gvwHeadData_RowCellStyle(object sender, RowCellStyleEventArgs e)
        {
            GridView view = sender as GridView;
            string fieldName = e.Column.FieldName;

            if (fieldName == "FACTORY" || fieldName == "PCC_PM")
            {
                if (view.IsCellSelected(e.RowHandle, e.Column) == false)
                    e.Appearance.BackColor = Color.Coral;
            }
            else if (fieldName == "SEASON_CD" || fieldName == "DEV_NAME" || fieldName == "ST_CD"
                || fieldName == "DEV_COLORWAY_ID" || fieldName == "SAMPLE_SIZE" || fieldName == "SAMPLE_QTY")
            {
                if (view.IsCellSelected(e.RowHandle, e.Column) == false)
                    e.Appearance.BackColor = Color.FromArgb(255, 255, 128);
            }
        }

        /// <summary>
        /// 멀티 셀 선택 후 Delete 키를 누르면 삭제 되도록 구현
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void gvwHeadData_KeyDown(object sender, KeyEventArgs e)
        {
            GridView view = sender as GridView;

            if (e.KeyCode == Keys.Delete)
            {
                GridCell[] cells = view.GetSelectedCells();

                foreach (GridCell cell in cells)
                {
                    // 아래 컬럼은 수정 불가
                    if (cell.Column.FieldName == "FACTORY" || cell.Column.FieldName == "PCC_PM")
                        continue;
                    else
                        view.SetRowCellValue(cell.RowHandle, cell.Column, "");
                }
            }
        }

        #endregion
    }
}