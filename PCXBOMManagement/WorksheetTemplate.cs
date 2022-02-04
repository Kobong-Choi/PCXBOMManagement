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
    public partial class WorksheetTemplate : DevExpress.XtraEditors.XtraForm
    {
        // JFlatform 기능 호출을 위한 부모 폼 정보
        public ProjectBaseForm projectBaseForm = Common.projectBaseForm;
        // [0] : style number, [1] season
        public string[] KEY_VALUE;
        
        public WorksheetTemplate()
        {
            InitializeComponent();
        }

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);

            DataTable dataSource;

            dataSource = GetTemplateContents("SPEC");
            BindDataToControl(dataSource, "SPEC");

            dataSource = GetTemplateContents("Comments");
            BindDataToControl(dataSource, "Comments");

            dataSource = GetTemplateContents("Operation");
            grdOperation.DataSource = dataSource;

            if (Common.sessionFactory != "DS")
            {
                // 해외 공장의 경우 공정명 영어로 표기
                gvwOperation.Columns["OP_K_NAME"].Visible = false;
                gvwOperation.Columns["OP_NAME"].Visible = true;

                gvwOperation.Columns["OP_NAME"].VisibleIndex = 0;
                gvwOperation.Columns["OP_CHK"].VisibleIndex = 1;

                // 해외 공장의 경우 실 정보 및 코멘트 입력 안 함                
                txtNeedleSize.Properties.ReadOnly = true;
                txtSPI.Properties.ReadOnly = true;
                txtStitchingMargin.Properties.ReadOnly = true;
                txtTwoStitchMargin.Properties.ReadOnly = true;
                txtThreadType.Properties.ReadOnly = true;
                
                txtComment1.Properties.ReadOnly = true;
                txtComment2.Properties.ReadOnly = true;
                txtComment3.Properties.ReadOnly = true;
                txtComment4.Properties.ReadOnly = true;
                txtComment5.Properties.ReadOnly = true;
                txtComment6.Properties.ReadOnly = true;
                txtComment7.Properties.ReadOnly = true;

                btnStitchingInfo.Visible = false;

                this.Height = Convert.ToInt32(this.Height * 0.6);
            }
        }
        
        #region 버튼 이벤트

        /// <summary>
        /// 마스터 재봉 정보를 불러옴
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnStitchingInfo_Click(object sender, EventArgs e)
        {
            Dictionary<string, string> dic = null;

            object returnObj = projectBaseForm.OpenChildForm(@"\POPUP\CSI.PCC.MODEL.P_ChoiceBP.dll", dic,
                JPlatform.Client.Library.interFace.OpenType.Modal);

            if (returnObj != null)
            {
                if (returnObj is Dictionary<string, string>)
                {
                    dic = (Dictionary<string, string>)returnObj;
                    txtNeedleSize.Text = dic["NEDDLE_SIZE"];
                    txtSPI.Text = dic["STITCH_MARGIN"];
                    txtStitchingMargin.Text = dic["STITCH_MARGIN_MM"];
                    txtTwoStitchMargin.Text = dic["TWO_STITCH_MARGIN"];
                    txtThreadType.Text = dic["THREAD_TYPE"];
                }
            }
        }

        /// <summary>
        /// DCS 버튼 상태 변경
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnDCS_CheckedChanged(object sender, EventArgs e)
        {
            CheckButton btn = sender as CheckButton;

            if (btn.Checked)
                btn.Text = "Checked";
            else
                btn.Text = "DCS";
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnSave_Click(object sender, EventArgs e)
        {
            if (SaveSpecAndCommentsInfo())
            {
                if (SaveOperationInfo())
                    MessageBox.Show("Save Complete");
            }
            else
                MessageBox.Show("Failed to save");
        }
        
        /// <summary>
        /// Spec 및 코멘트 정보를 저장
        /// </summary>
        /// <returns></returns>
        private bool SaveSpecAndCommentsInfo()
        {
            #region 패키지 매개변수 입력

            PKG_INTG_BOM_WORKSHEET.UPDATE_TEMPLATE_SPEC pkgInsert = new PKG_INTG_BOM_WORKSHEET.UPDATE_TEMPLATE_SPEC();
            pkgInsert.ARG_FACTORY = Common.sessionFactory;
            pkgInsert.ARG_KEY_VALUE = KEY_VALUE[1] + "-" + KEY_VALUE[0]; // season + styleNumber
            pkgInsert.ARG_KEY_TYPE = "STYLE";
            pkgInsert.ARG_LAST_CD = txtLastId.Text.Trim();
            pkgInsert.ARG_HEEL_HEIGHT = txtHeelHeight.Text.Trim();
            pkgInsert.ARG_MEDIAL_HEIGHT = txtMedialHeight.Text.Trim();
            pkgInsert.ARG_LATERAL_HEIGHT = txtLateralHeight.Text.Trim();
            pkgInsert.ARG_LACE_LENGTH = txtLaceLength.Text.Trim();
            pkgInsert.ARG_MS_HARDNESS = txtMSHardness.Text.Trim();
            pkgInsert.ARG_IDS_LENGTH = txtIDSLength.Text.Trim();
            pkgInsert.ARG_MS_CODE = txtMSCode.Text.Trim();
            pkgInsert.ARG_OS_CODE = txtOSCode.Text.Trim();
            pkgInsert.ARG_UPPER_MATERIAL = txtUpperMaterial.Text.Trim();
            pkgInsert.ARG_MS_MATERIAL = txtMidsoleMaterial.Text.Trim();
            pkgInsert.ARG_OUTSOLE_MATERIAL = txtOutsoleMaterial.Text.Trim();
            pkgInsert.ARG_NEEDLE_SIZE = txtNeedleSize.Text.Trim();
            pkgInsert.ARG_SPI = txtSPI.Text.Trim();
            pkgInsert.ARG_STITCHING_MARGIN = txtStitchingMargin.Text.Trim();
            pkgInsert.ARG_TWOROW_STITCHING_MARGIN = txtTwoStitchMargin.Text.Trim();
            pkgInsert.ARG_THREAD_TYPE = txtThreadType.Text.Trim();
            pkgInsert.ARG_DCS_YN = (btnDCS.Checked == true) ? "Y" : "N";
            pkgInsert.ARG_WS_COMMENT1 = txtComment1.Text.Trim();
            pkgInsert.ARG_WS_COMMENT2 = txtComment2.Text.Trim();
            pkgInsert.ARG_WS_COMMENT3 = txtComment3.Text.Trim();
            pkgInsert.ARG_WS_COMMENT4 = txtComment4.Text.Trim();
            pkgInsert.ARG_WS_COMMENT5 = txtComment5.Text.Trim();
            pkgInsert.ARG_WS_COMMENT6 = txtComment6.Text.Trim();
            pkgInsert.ARG_WS_COMMENT7 = txtComment7.Text.Trim();
            pkgInsert.ARG_UPD_USER = Common.sessionID;
            pkgInsert.ARG_NIKE_DEV = txtNikeDev.Text.Trim();
            pkgInsert.ARG_WHQ_DEV = txtWHQDev.Text.Trim();

            #endregion

            ArrayList arrayList = new ArrayList();
            arrayList.Add(pkgInsert);

            try
            {
                projectBaseForm.Exe_Modify_PKG(arrayList);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
                return false;
            }
            return true;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        private bool SaveOperationInfo()
        {
            // 패키지 호출용 임시변수
            ArrayList arrayList = new ArrayList();

            for (int i = 0; i < gvwOperation.RowCount; i++)
            {
                PKG_INTG_BOM_WORKSHEET.UPDATE_TEMPLATE_OPERATION pkgInsert = new PKG_INTG_BOM_WORKSHEET.UPDATE_TEMPLATE_OPERATION();
                pkgInsert.ARG_FACTORY = Common.sessionFactory;
                pkgInsert.ARG_KEY_VALUE = KEY_VALUE[1] + "-" + KEY_VALUE[0];
                pkgInsert.ARG_KEY_TYPE = "STYLE";
                pkgInsert.ARG_OP_CD = gvwOperation.GetRowCellValue(i, "OP_CD").ToString();
                pkgInsert.ARG_OP_CHK = gvwOperation.GetRowCellValue(i, "OP_CHK").ToString();
                pkgInsert.ARG_UPD_USER = Common.sessionID;

                arrayList.Add(pkgInsert);
            }
            try
            {
                projectBaseForm.Exe_Modify_PKG(arrayList);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
                return false;
            }
            return true;
        }

        #endregion

        #region 사용자 정의 함수

        /// <summary>
        /// workType에 맞는 데이터를 조회
        /// </summary>
        /// <param name="workType"></param>
        /// <returns></returns>
        private DataTable GetTemplateContents(string workType)
        {
            PKG_INTG_BOM_WORKSHEET.CREATE_TEMPLATE_BY_STYLE pkgSelect = new PKG_INTG_BOM_WORKSHEET.CREATE_TEMPLATE_BY_STYLE();
            pkgSelect.ARG_WORK_TYPE = workType;
            pkgSelect.ARG_FACTORY = Common.sessionFactory;
            pkgSelect.ARG_KEY_VALUE = KEY_VALUE[1] + "-" + KEY_VALUE[0];
            pkgSelect.ARG_KEY_TYPE = "STYLE";
            pkgSelect.OUT_CURSOR = string.Empty;

            try
            {
                DataTable dataSource = projectBaseForm.Exe_Select_PKG(pkgSelect).Tables[0];
                return dataSource;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
                return null;
            }
        }

        /// <summary>
        /// 데이터 소스의 데이터를 각 컨트롤에 바인딩
        /// </summary>
        /// <param name="dataSource"></param>
        /// <param name="workType"></param>
        private void BindDataToControl(DataTable dataSource, string workType)
        {
            if (workType == "SPEC")
            {
                if (dataSource.Rows.Count > 0)
                {
                    #region DB에서 가져온 데이터를 각 컨트롤에 바인딩

                    txtLastId.Text = dataSource.Rows[0]["LAST_CD"].ToString().Trim();
                    txtHeelHeight.Text = dataSource.Rows[0]["HEEL_HEIGHT"].ToString().Trim();
                    txtMedialHeight.Text = dataSource.Rows[0]["MEDIAL_HEIGHT"].ToString().Trim();
                    txtLateralHeight.Text = dataSource.Rows[0]["LATERAL_HEIGHT"].ToString().Trim();
                    txtMSHardness.Text = dataSource.Rows[0]["MS_HARDNESS"].ToString().Trim();
                    txtIDSLength.Text = dataSource.Rows[0]["IDS_LENGTH"].ToString().Trim();
                    txtLaceLength.Text = dataSource.Rows[0]["LACE_LENGTH"].ToString().Trim();
                    txtNeedleSize.Text = dataSource.Rows[0]["NEEDLE_SIZE"].ToString().Trim();
                    txtSPI.Text = dataSource.Rows[0]["SPI"].ToString().Trim();
                    txtStitchingMargin.Text = dataSource.Rows[0]["STITCHING_MARGIN"].ToString().Trim();
                    txtTwoStitchMargin.Text = dataSource.Rows[0]["TWOROW_STITCHING_MARGIN"].ToString().Trim();
                    txtThreadType.Text = dataSource.Rows[0]["THREAD_TYPE"].ToString().Trim();

                    string DCS_YN = dataSource.Rows[0]["DCS_YN"].ToString().Trim();

                    if (DCS_YN == "Y")
                        btnDCS.Checked = true;
                    else
                        btnDCS.Checked = false;

                    txtMSCode.Text = dataSource.Rows[0]["MS_CODE"].ToString().Trim();
                    txtOSCode.Text = dataSource.Rows[0]["OS_CODE"].ToString().Trim();
                    txtUpperMaterial.Text = dataSource.Rows[0]["UPPER_MATERIAL"].ToString().Trim();
                    txtMidsoleMaterial.Text = dataSource.Rows[0]["MS_MATERIAL"].ToString().Trim();
                    txtOutsoleMaterial.Text = dataSource.Rows[0]["OUTSOLE_MATERIAL"].ToString().Trim();
                    txtNikeDev.Text = dataSource.Rows[0]["NIKE_DEV"].ToString().Trim();
                    txtWHQDev.Text = dataSource.Rows[0]["WHQ_DEV"].ToString().Trim();

                    #endregion
                }
            }
            else if (workType == "Comments")
            {
                if (dataSource.Rows.Count > 0)
                {
                    #region DB에서 가져온 데이터를 각 컨트롤에 바인딩

                    txtComment1.Text = dataSource.Rows[0]["1"].ToString().Trim();
                    txtComment2.Text = dataSource.Rows[0]["2"].ToString().Trim();
                    txtComment3.Text = dataSource.Rows[0]["3"].ToString().Trim();
                    txtComment4.Text = dataSource.Rows[0]["4"].ToString().Trim();
                    txtComment5.Text = dataSource.Rows[0]["5"].ToString().Trim();
                    txtComment6.Text = dataSource.Rows[0]["6"].ToString().Trim();
                    txtComment7.Text = dataSource.Rows[0]["7"].ToString().Trim();

                    #endregion
                }
            }
        }

        #endregion

        #region 컨텍스트 메뉴 이벤트

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void CustomContextMenu_Click(object sender, EventArgs e)
        {
            System.Windows.Forms.ToolStripMenuItem menuItem = sender as System.Windows.Forms.ToolStripMenuItem;

            switch (menuItem.Name)
            {
                case "MultiCheck":
                    ToggleOperationRow();
                    break;

                default:
                    break;
            }
        }

        /// <summary>
        /// 유저가 선택한 행의 공정 상태 값을 Toggle
        /// </summary>
        private void ToggleOperationRow()
        {
            int[] rowHandles = gvwOperation.GetSelectedRows();

            foreach (int rowHandle in rowHandles)
            {
                string checkStatus = gvwOperation.GetRowCellValue(rowHandle, "OP_CHK").ToString();
                if (checkStatus == "N")
                    gvwOperation.SetRowCellValue(rowHandle, "OP_CHK", "Y");
                else
                    gvwOperation.SetRowCellValue(rowHandle, "OP_CHK", "N");
            }
        }

        #endregion

        #region 그리드 이벤트

        /// <summary>
        /// 체크 박스를 한 번의 클릭으로 Toggle 되도록 수행
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void gvwOperation_MouseDown(object sender, MouseEventArgs e)
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
        /// 그리드 셀 스타일 적용
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void gvwOperation_RowCellStyle(object sender, RowCellStyleEventArgs e)
        {
            GridView view = sender as GridView;

            if (e.Column.FieldName == "OP_CHK")
            {
                if (view.IsCellSelected(e.RowHandle, e.Column) == false)
                    e.Appearance.BackColor = Color.Yellow;
            }
        }

        #endregion
    }
}