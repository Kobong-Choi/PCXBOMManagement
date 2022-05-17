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
    public partial class BOMUploader : DevExpress.XtraEditors.XtraForm
    {
        private string[] unmodifiedFields = new string[] { "FACTORY", "PROD_FACTORY", "PCC_PM" };
        private string[] requiredFields = new string[] { "SEASON_CD", "DEV_NAME", "DEV_COLORWAY_ID", "ST_CD", "SAMPLE_SIZE", "SAMPLE_QTY", "SAMPLE_ETS", "SUB_TYPE_REMARK" };
        bool hasNormallyDone = false;   // Flag representing that the works has been performed normally.
        
        public string ChainedWSNo { get; set; }

        public BOMUploader()
        {
            InitializeComponent();
        }

        protected override void OnLoad(EventArgs e)
        {
            DataTable dataSource = null;

            base.OnLoad(e);

            // GEL is only managed by HQ.
            if (Common.sessionFactory != "DS")
                gvwHead.Columns["GEL_YN"].Visible = false;

            #region Bind datasource into each repository lookupEdit.

            // Season
            PKG_INTG_BOM_COMMON.SELECT_COMMON_CODE pkgSelectSeason = new PKG_INTG_BOM_COMMON.SELECT_COMMON_CODE();
            pkgSelectSeason.ARG_TYPE = "SEASON";
            pkgSelectSeason.ARG_SORT_TYPE = "DESC";
            pkgSelectSeason.OUT_CURSOR = string.Empty;

            dataSource = Common.projectBaseForm.Exe_Select_PKG(pkgSelectSeason).Tables[0];
            lookUpSeason.DataSource = dataSource;

            // Sample Type
            PKG_INTG_BOM_COMMON.SELECT_SAMPLE_TYPE pkgSelectSampleType = new PKG_INTG_BOM_COMMON.SELECT_SAMPLE_TYPE();
            pkgSelectSampleType.ARG_FACTORY = Common.sessionFactory;
            pkgSelectSampleType.ARG_ST_DIV = "B";
            pkgSelectSampleType.ARG_USE_YN = "Y";
            pkgSelectSampleType.OUT_CURSOR = string.Empty;

            dataSource = Common.projectBaseForm.Exe_Select_PKG(pkgSelectSampleType).Tables[0];
            lookUpSampleType.DataSource = dataSource;

            // PCC Category
            PKG_INTG_BOM_COMMON.SELECT_CATEGORY pkgSelectCategory = new PKG_INTG_BOM_COMMON.SELECT_CATEGORY();
            pkgSelectCategory.ARG_GROUP_CODE = "BOM_CAT";
            pkgSelectCategory.ARG_CODE_VALUE = "WMT";
            pkgSelectCategory.OUT_CURSOR = string.Empty;

            dataSource = Common.projectBaseForm.Exe_Select_PKG(pkgSelectCategory).Tables[0];
            lookUpCategory.DataSource = dataSource;

            // GENDER
            PKG_INTG_BOM_COMMON.SELECT_GENDER pkgSelectGender = new PKG_INTG_BOM_COMMON.SELECT_GENDER();
            pkgSelectGender.ARG_ATTRIBUTE_NAME = "Gender";
            pkgSelectGender.OUT_CURSOR = string.Empty;

            dataSource = Common.projectBaseForm.Exe_Select_PKG(pkgSelectGender).Tables[0];
            lookUpGender.DataSource = dataSource;

            // TD
            PKG_INTG_BOM_COMMON.SELECT_COMMON_CODE pksgSelectTd = new PKG_INTG_BOM_COMMON.SELECT_COMMON_CODE();
            pksgSelectTd.ARG_TYPE = "TD";
            pksgSelectTd.ARG_SORT_TYPE = "ASC";
            pksgSelectTd.OUT_CURSOR = string.Empty;

            dataSource = Common.projectBaseForm.Exe_Select_PKG(pksgSelectTd).Tables[0];
            lookUpTd.DataSource = dataSource;

            // PCC PM
            PKG_INTG_BOM_COMMON.SELECT_DEVELOPER pkgSelectDeveloper = new PKG_INTG_BOM_COMMON.SELECT_DEVELOPER();
            pkgSelectDeveloper.ARG_FACTORY = Common.sessionFactory;
            pkgSelectDeveloper.OUT_CURSOR = string.Empty;

            dataSource = Common.projectBaseForm.Exe_Select_PKG(pkgSelectDeveloper).Tables[0];
            lookUpDeveloper.DataSource = dataSource;

            #endregion

            // Load BOM Header data of all BOM's.
            PKG_INTG_BOM.SELECT_BOM_HEAD_DATA pkgSelect = new PKG_INTG_BOM.SELECT_BOM_HEAD_DATA();
            pkgSelect.ARG_FACTORY = Common.sessionFactory;
            pkgSelect.ARG_CONCAT_WS_NO = ChainedWSNo;
            pkgSelect.OUT_CURSOR = string.Empty;

            grdHead.DataSource = Common.projectBaseForm.Exe_Select_PKG(pkgSelect).Tables[0];
        }

        #region Button Events

        /// <summary>
        /// Inform an user of what is the unique criteria.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnHelp_Click(object sender, EventArgs e)
        {
            Common.ShowMessageBox("** Criteria of Unique\nCombination of Season, Sample Type, Sub Type, Colorway ID, Sub. Remark", "I");
        }

        /// <summary>
        /// Update some data of BOM header.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnSave_Click(object sender, EventArgs e)
        {
            ArrayList list = new ArrayList();
            string[] GELRounds = new string[] { "FRST", "SCND", "THRD", "PRDC" };
            bool isGel = false;
            bool hasContained = false;

            // Replace '-' to ''.
            Action<string, int> action = (column, rowHandle) =>
            {
                string data = gvwHead.GetRowCellValue(rowHandle, column).ToString();

                if (data.Contains("-"))
                    gvwHead.SetRowCellValue(rowHandle, column, data.Replace("-", "").Trim());
            };

            if (CheckRequiredField())
            {
                for (int rowHandle = 0; rowHandle < gvwHead.RowCount; rowHandle++)
                {
                    action("STYLE_CD", rowHandle);
                    action("DPA", rowHandle);

                    hasContained = GELRounds.Contains(gvwHead.GetRowCellValue(rowHandle, "ST_CD").ToString());
                    isGel = gvwHead.GetRowCellValue(rowHandle, "GEL_YN").ToString() == "Y" ? true : false;

                    if (isGel && !hasContained)
                    {
                        if (MessageBox.Show("The Sample Type doesn't match the GEL.\nDo you want to proceed?", "",
                            MessageBoxButtons.OKCancel, MessageBoxIcon.Warning) == DialogResult.Cancel)
                        {
                            Common.FocusCell(gvwHead, rowHandle, "ST_CD", true);
                            return;
                        }
                    }
                    else if (!isGel && hasContained)
                    {
                        if (MessageBox.Show("Is The Sample Type you selected correct? it's for GEL.\nDo you want to proceed?", "",
                            MessageBoxButtons.OKCancel, MessageBoxIcon.Warning) == DialogResult.Cancel)
                        {
                            Common.FocusCell(gvwHead, rowHandle, "ST_CD", true);
                            return;
                        }
                    }

                    /*******************************************************************************************
                    * BOM Unique Criteria : Factory, Season, Sample Type, Sub Type, Colorway ID, Sub Type Remark
                    ********************************************************************************************/

                    PKG_INTG_BOM.SELECT_IS_EXISTING_BOM pkgSelect = new PKG_INTG_BOM.SELECT_IS_EXISTING_BOM();
                    pkgSelect.ARG_FACTORY = gvwHead.GetRowCellValue(rowHandle, "FACTORY").ToString();
                    pkgSelect.ARG_WS_NO = gvwHead.GetRowCellValue(rowHandle, "WS_NO").ToString();
                    pkgSelect.ARG_SEASON_CD = gvwHead.GetRowCellValue(rowHandle, "SEASON_CD").ToString();
                    pkgSelect.ARG_DEV_COLORWAY_ID = gvwHead.GetRowCellValue(rowHandle, "DEV_COLORWAY_ID").ToString();
                    pkgSelect.ARG_SAMPLE_TYPE = gvwHead.GetRowCellValue(rowHandle, "ST_CD").ToString();
                    pkgSelect.ARG_SUB_TYPE = "NA";  // Default.
                    pkgSelect.ARG_SUB_TYPE_REMARK = gvwHead.GetRowCellValue(rowHandle, "SUB_TYPE_REMARK").ToString(); ;
                    pkgSelect.OUT_CURSOR = string.Empty;

                    DataTable returnValue = Common.projectBaseForm.Exe_Select_PKG(pkgSelect).Tables[0];
                    
                    if (Convert.ToInt32(returnValue.Rows[0]["ROW_COUNT"]) == 0)
                    {
                        PKG_INTG_BOM.UPDATE_BOM_HEAD pkgUpdateHead = new PKG_INTG_BOM.UPDATE_BOM_HEAD();
                        pkgUpdateHead.ARG_FACTORY = gvwHead.GetRowCellValue(rowHandle, "FACTORY").ToString();
                        pkgUpdateHead.ARG_WS_NO = gvwHead.GetRowCellValue(rowHandle, "WS_NO").ToString();
                        pkgUpdateHead.ARG_DPA = gvwHead.GetRowCellValue(rowHandle, "DPA").ToString();
                        pkgUpdateHead.ARG_PCM_BOM_ID = gvwHead.GetRowCellValue(rowHandle, "BOM_ID").ToString();
                        pkgUpdateHead.ARG_SAMPLE_TYPE = gvwHead.GetRowCellValue(rowHandle, "ST_CD").ToString();
                        pkgUpdateHead.ARG_SEASON_CD = gvwHead.GetRowCellValue(rowHandle, "SEASON_CD").ToString();
                        pkgUpdateHead.ARG_CATEGORY = gvwHead.GetRowCellValue(rowHandle, "CATEGORY").ToString();
                        pkgUpdateHead.ARG_DEV_NAME = gvwHead.GetRowCellValue(rowHandle, "DEV_NAME").ToString();
                        pkgUpdateHead.ARG_PRODUCT_CODE = gvwHead.GetRowCellValue(rowHandle, "STYLE_CD").ToString();
                        pkgUpdateHead.ARG_SAMPLE_ETS = gvwHead.GetRowCellValue(rowHandle, "SAMPLE_ETS").ToString();
                        pkgUpdateHead.ARG_SAMPLE_QTY = gvwHead.GetRowCellValue(rowHandle, "SAMPLE_QTY").ToString();
                        pkgUpdateHead.ARG_SAMPLE_SIZE = gvwHead.GetRowCellValue(rowHandle, "SAMPLE_SIZE").ToString();
                        pkgUpdateHead.ARG_SUB_TYPE_REMARK = gvwHead.GetRowCellValue(rowHandle, "SUB_TYPE_REMARK").ToString();
                        pkgUpdateHead.ARG_GENDER = gvwHead.GetRowCellValue(rowHandle, "GENDER").ToString();
                        pkgUpdateHead.ARG_TD = gvwHead.GetRowCellValue(rowHandle, "TD").ToString();
                        pkgUpdateHead.ARG_COLOR_VER = gvwHead.GetRowCellValue(rowHandle, "COLOR_VER").ToString();
                        pkgUpdateHead.ARG_MODEL_ID = gvwHead.GetRowCellValue(rowHandle, "MODEL_ID").ToString();
                        pkgUpdateHead.ARG_PCC_PM = gvwHead.GetRowCellValue(rowHandle, "PCC_PM").ToString();
                        pkgUpdateHead.ARG_DEV_COLORWAY_ID = gvwHead.GetRowCellValue(rowHandle, "DEV_COLORWAY_ID").ToString();
                        pkgUpdateHead.ARG_PRODUCT_ID = gvwHead.GetRowCellValue(rowHandle, "PRODUCT_ID").ToString();
                        pkgUpdateHead.ARG_DEV_SAMPLE_REQ_ID = gvwHead.GetRowCellValue(rowHandle, "DEV_SAMPLE_REQ_ID").ToString();
                        pkgUpdateHead.ARG_STYLE_NUMBER = gvwHead.GetRowCellValue(rowHandle, "DEV_STYLE_NUMBER").ToString();
                        pkgUpdateHead.ARG_GEL_YN = gvwHead.GetRowCellValue(rowHandle, "GEL_YN").ToString();

                        list.Add(pkgUpdateHead);
                    }
                    else
                    {
                        Common.ShowMessageBox("A BOM with the same information already exists.\nPlease check the criteria of unique.(refer to help button)", "E");
                        return;
                    }
                }

                // Call procedures.
                if (Common.projectBaseForm.Exe_Modify_PKG(list) == null)
                {
                    Common.ShowMessageBox("Failed to upload BOM.", "E");
                    return;
                }

                hasNormallyDone = true;
                DialogResult = DialogResult.OK;
                MessageBox.Show("Complete.");
                this.Close();
            }
        }

        /// <summary>
        /// Check whether the required fields were input.
        /// </summary>
        /// <returns></returns>
        private bool CheckRequiredField()
        {
            string[] requiredColumns = new string[] { "SEASON_CD", "DEV_NAME", "DEV_COLORWAY_ID", "ST_CD",
                "SAMPLE_ETS", "SUB_TYPE_REMARK", "SAMPLE_SIZE" };

            for (int rowHandle = 0; rowHandle < gvwHead.RowCount; rowHandle++)
            {
                foreach (string column in requiredColumns)
                {
                    if (gvwHead.GetRowCellValue(rowHandle, column).ToString() == "")
                    {
                        MessageBox.Show(string.Format("{0} is a requried field.", column), "",
                            MessageBoxButtons.OK, MessageBoxIcon.Error, MessageBoxDefaultButton.Button1);
                        
                        Common.FocusCell(gvwHead, rowHandle, column, true);
                        return false;
                    }
                }

                try
                {
                    double sampleQty = Convert.ToDouble(gvwHead.GetRowCellValue(rowHandle, "SAMPLE_QTY"));

                    if (sampleQty == 0)
                    {
                        MessageBox.Show("Sample Qty should be more than zero.", "",
                            MessageBoxButtons.OK, MessageBoxIcon.Error, MessageBoxDefaultButton.Button1);

                        Common.FocusCell(gvwHead, rowHandle, "SAMPLE_QTY", true);
                        return false;
                    }
                }
                catch
                {
                    MessageBox.Show("Sample Qty should be a digit number.", "",
                        MessageBoxButtons.OK, MessageBoxIcon.Error, MessageBoxDefaultButton.Button1);
                    
                    Common.FocusCell(gvwHead, rowHandle, "SAMPLE_QTY", true);
                    return false;
                }
            }

            return true;
        }

        #endregion

        #region Grid Events

        /// <summary>
        /// Run when any key is pressed on the keyboard.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void CustomKeyDown(object sender, KeyEventArgs e)
        {
            GridView view = sender as GridView;
            string value = string.Empty;

            try
            {
                view.CellValueChanged -= new CellValueChangedEventHandler(CustomCellValueChanged);

                if (e.KeyCode == Keys.Delete)
                {
                    foreach (GridCell cell in view.GetSelectedCells())
                    {
                        if (unmodifiedFields.Contains(cell.Column.FieldName))
                            continue;
                        else
                            view.SetRowCellValue(cell.RowHandle, cell.Column, "");
                    }
                }
                else if (e.KeyCode == Keys.Enter)
                {
                    if (view.EditingValue != null)
                    {
                        if (view.EditingValue.GetType().Name == "DateTime")
                            value = Convert.ToDateTime(view.EditingValue.ToString()).ToString("yyyyMMdd");
                        else
                            value = view.EditingValue.ToString();

                        foreach (GridCell cell in view.GetSelectedCells())
                            view.SetRowCellValue(cell.RowHandle, cell.Column, value);
                    }
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

        /// <summary>
        /// Highlights some fields.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void CustomRowCellStyle(object sender, RowCellStyleEventArgs e)
        {
            GridView view = sender as GridView;

            if (requiredFields.Contains(e.Column.FieldName))
            {
                if (view.IsCellSelected(e.RowHandle, e.Column) == false)
                    e.Appearance.BackColor = Color.FromArgb(255, 255, 128);
            }
            else if (unmodifiedFields.Contains(e.Column.FieldName))
            {
                if (view.IsCellSelected(e.RowHandle, e.Column) == false)
                    e.Appearance.BackColor = Color.Coral;
            }
        }

        /// <summary>
        /// One-Click toggle checkEdit.
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

        /// <summary>
        /// When an user selects sample ets by only mouse except keyboard.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void CustomCellValueChanged(object sender, CellValueChangedEventArgs e)
        {
            try
            {
                gvwHead.CellValueChanged -= new CellValueChangedEventHandler(CustomCellValueChanged);

                switch (e.Column.FieldName)
                {
                    case "SAMPLE_ETS":

                        gvwHead.SetRowCellValue(e.RowHandle, "SAMPLE_ETS", Convert.ToDateTime(e.Value).ToString("yyyyMMdd"));
                        break;
                }
            }
            catch (Exception ex)
            {
                Common.ShowMessageBox(ex.ToString(), "E");
            }
            finally
            {
                gvwHead.CellValueChanged += new CellValueChangedEventHandler(CustomCellValueChanged);
            }
        }

        #endregion

        #region Form Events

        /// <summary>
        /// When an user withdraws uploading BOM's.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void CustomFormClosing(object sender, FormClosingEventArgs e)
        {
            ArrayList list = new ArrayList();

            // When an user withdraws uploading BOM.
            if (hasNormallyDone == false)
            {
                if (DialogResult.Cancel == MessageBox.Show("Do you really want to close this form without saving data?", "",
                                            MessageBoxButtons.OKCancel, MessageBoxIcon.Question, MessageBoxDefaultButton.Button1))
                    e.Cancel = true;
                else
                {
                    // Delete temporary data which has already been uploaded.
                    PKG_INTG_BOM.DELETE_BOM_DATA pkgDelete = new PKG_INTG_BOM.DELETE_BOM_DATA();
                    pkgDelete.ARG_PCC = Common.sessionFactory;
                    pkgDelete.ARG_CONCAT_WS_NO = ChainedWSNo;
                    pkgDelete.ARG_DELETE_USER = Common.sessionID;

                    list.Add(pkgDelete);

                    if (Common.projectBaseForm.Exe_Modify_PKG(list) == null)
                    {
                        MessageBox.Show("Failed to close BOM uploader. Please ask to IT dept.", "",
                            MessageBoxButtons.OK, MessageBoxIcon.Error, MessageBoxDefaultButton.Button1);
                        
                        e.Cancel = true;
                    }
                    else
                        DialogResult = DialogResult.Ignore;
                }
            }
        }

        #endregion
    }
}