using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO;                                // MemoryStream
using System.Drawing.Imaging;                   // ImageFormat
using System.Collections;                       // ArrayList

using DevExpress.XtraGrid;                      // GridControl
using DevExpress.XtraGrid.Views.Base;           // GridCell
using DevExpress.XtraGrid.Views.Grid;           // GridView
using DevExpress.XtraGrid.Views.Grid.ViewInfo;  // GridHitInfo
using DevExpress.Utils;                         // DXMouseEventArgs
using DevExpress.XtraEditors;                   // GridLookUpEdit

using CSI.Client.ProjectBaseForm;               // ProjectBaseForm Class
using CSI.PCC.PCX.Packages;                     // Package Class

using JPlatform.Client.Library.interFace;       // OpenType.Modal

namespace CSI.PCC.PCX
{
    public partial class WorksheetMaking : DevExpress.XtraEditors.XtraForm
    {
        // JFlatform 기능 호출을 위한 부모 폼 정보
        public ProjectBaseForm projectBaseForm = Common.projectBaseForm;

        /******************************************************************************
         * 부모 폼에서 전달 받은 소스 BOM 정보
         * [0] : Factory
         * [1] : Worksheet Number
         * [2] : Style Nmae
         * [3] : Colorway ID
         * [4] : Worksheet Status => N : Ready, Y : Request, K : Checked, C : Confirm
         * [5] : CS_BOM_CFM
         * [6] : Style Number
         * [7] : Season
        ******************************************************************************/
        public string[] SOURCE_BOM_INFO;
        public int ROWHANDLE;
        private bool isCopied = false;

        PictureEdit[] EDITS_UPPER_PICTURES = null;
        TextEdit[] EDITS_UPPER_TEXT = null;
        IDataObject OBJECT = null;

        public WorksheetMaking()
        {
            InitializeComponent();
        }

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);

            #region 폼 캡션 정보를 가져옴

            PKG_INTG_BOM.SELECT_BOM_INFO_BY_CONFIRM pkgSelect = new PKG_INTG_BOM.SELECT_BOM_INFO_BY_CONFIRM();
            pkgSelect.ARG_WORK_TYPE = "BOMCaption";
            pkgSelect.ARG_FACTORY = SOURCE_BOM_INFO[0];
            pkgSelect.ARG_WS_NO = SOURCE_BOM_INFO[1];
            pkgSelect.ARG_PART_SEQ = "";
            pkgSelect.OUT_CURSOR = string.Empty;

            DataTable captionInfo = projectBaseForm.Exe_Select_PKG(pkgSelect).Tables[0];

            if (captionInfo != null)
            {
                string category = captionInfo.Rows[0]["CATEGORY"].ToString();
                string season = captionInfo.Rows[0]["SEASON"].ToString();

                string sampleType = captionInfo.Rows[0]["SAMPLE_TYPE"].ToString();
                if (sampleType == "PROMO")
                    xtpPromo.PageVisible = true;    // PROMO 라운드의 경우 추가 이미지 삽입 asked by T&F

                string subType = captionInfo.Rows[0]["SUB_SAMPLE_TYPE"].ToString();
                string devName = captionInfo.Rows[0]["DEV_NAME"].ToString();
                string devStyleNumber = captionInfo.Rows[0]["DEV_STYLE_NUMBER"].ToString();
                //string colorVer = _dataSource.Rows[0]["COLOR_VER"].ToString();
                string colorwayId = captionInfo.Rows[0]["DEV_COLORWAY_ID"].ToString();
                //string rowNumber = dataSourceCaption.Rows[0]["RN"].ToString();

                this.Text = category + " / " + season + " / " + devName + " / " + sampleType + " / " + devStyleNumber + " / " + colorwayId;
            }

            #endregion

            #region 룩업에딧 바인딩

            Common.BindLookUpEdit("Season", leSeason, false, "");
            Common.BindLookUpEdit("SampleType", leSampleType, false, "Y");
            Common.BindLookUpEdit("SubType", leSubType, false, "");
            Common.BindLookUpEdit("Category", leDevTeam, false, "");
            Common.BindLookUpEdit("TD", leTD, false, "");
            Common.BindLookUpEdit("Gender", leGender, false, "");

            #endregion

            SetInitialPropertyOfControl();

            // 유저 팩토리에 따른 컨트롤 사용 여부
            if (Common.sessionFactory == "DS")
            {
                // 영어 공정 명칭 Invisible(Cutting/재단의 경우 재단만 표기)
                gvwOperation.Columns["OP_NAME"].Visible = false;

                // 해외 Upper 이미지 입력 페이지 Invisible
                xtpUpperOVS.PageVisible = false;

                // 본사 기준 컨트롤 배열 세팅
                EDITS_UPPER_PICTURES = new PictureEdit[] { peUpperHQ1, peUpperHQ2, peUpperHQ3, peUpperHQ4 };
                EDITS_UPPER_TEXT = new TextEdit[] { teUpperHQ1, teUpperHQ2, teUpperHQ3, teUpperHQ4 };
            }
            else
            {
                // 한글 공정 명칭 Invisible
                gvwOperation.Columns["OP_K_NAME"].Visible = false;

                // 본사 Upper 이미지 입력 페이지 Invisible
                xtpUpperHQ.PageVisible = false;

                // 해외 공장 기준 컨트롤 배열 세팅
                EDITS_UPPER_PICTURES = new PictureEdit[] { peUpperOVS1, peUpperOVS2, peUpperOVS3, peUpperOVS4, peUpperOVS5, peUpperOVS6 };
                EDITS_UPPER_TEXT = new TextEdit[] { teUpperOVS1, teUpperOVS2, teUpperOVS3, teUpperOVS4, teUpperOVS5, teUpperOVS6 };

                #region 해외공장의 경우 Thread 별도 입력

                int widthOfControl1 = groupControl1.Width;
                int widthOfControl7 = groupControl7.Width;

                groupControl1.Width = widthOfControl1 - widthOfControl7;

                groupControl3.Visible = false;
                groupControl7.Visible = false;
                btnThreadForOvs.Visible = true;

                this.Width = Convert.ToInt32(this.Width * 0.7);

                #endregion
            }

            // 작업지시서 헤더 정보 바인딩
            DataTable dataSource = GetWorksheetContents("Tag_Other");
            BindDataToControl(dataSource, "Tag_Other");

            // 작업지시서 SMS 정보 바인딩
            dataSource = GetWorksheetContents("SMS");
            grdSMS.DataSource = dataSource;

            // 작업지시서 코멘트 정보 바인딩
            dataSource = GetWorksheetContents("Comment");
            BindDataToControl(dataSource, "Comment");

            // 작업지시서 공정 정보 바인딩
            dataSource = GetWorksheetContents("Operation");
            grdOperation.DataSource = dataSource;

            // 이미지 바인딩
            dataSource = GetWorksheetContents("Image");
            BindDataToControl(dataSource, "Image");

            // 미드솔 그리드 데이터 바인딩
            grdMidsole.DataSource = GetWorksheetBottomMaterial("M");

            // 아웃솔 그리드 데이터 바인딩
            grdOutsole.DataSource = GetWorksheetBottomMaterial("T");

            // 공정 작업 여부 체크 시 수행할 이벤트 연결
            repositoryItemCheckEdit1.EditValueChanged += new EventHandler(repositoryItemCheckEdit1_EditValueChanged);

            #region Sample ETS 툴팁 설정

            PKG_INTG_BOM_COMMON.SELECT_POSSIBLE_SHIP_DATE pkgSelectDate = new PKG_INTG_BOM_COMMON.SELECT_POSSIBLE_SHIP_DATE();
            pkgSelectDate.ARG_FACTORY = SOURCE_BOM_INFO[0];
            pkgSelectDate.OUT_CURSOR = string.Empty;

            DataTable etsInfo = projectBaseForm.Exe_Select_PKG(pkgSelectDate).Tables[0];

            if (etsInfo.Rows.Count > 0)
            {
                string earliestShipDate = etsInfo.Rows[0]["WORK_YMD"].ToString();
                string cvtdShipDate = DateTime.ParseExact(earliestShipDate, "yyyyMMdd",
                    System.Globalization.CultureInfo.InvariantCulture).ToShortDateString();

                // 툴팁 설정
                deSampleETS.ToolTip = "Possible Earliest Ship Date : " + cvtdShipDate;
            }

            #endregion
        }

        #region 그리드 이벤트

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void gvwSMS_RowCellStyle(object sender, RowCellStyleEventArgs e)
        {
            GridView view = sender as GridView;

            switch (e.Column.FieldName)
            {
                case "USER_JOB":
                case "USER_NAME":
                    string DPA = view.GetRowCellValue(e.RowHandle, "DPA").ToString();
                    // PMX에 담당자로 설정된 유저는 노란색 표기
                    if (DPA != "")
                    {
                        if (view.IsCellSelected(e.RowHandle, e.Column) == false)
                        {
                            e.Appearance.BackColor = Color.Yellow;
                        }
                    }
                    break;
            }
        }

        /// <summary>
        /// 
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
        /// 공정 수량의 멀티 입력 또는 삭제
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void gvwOperation_KeyDown(object sender, KeyEventArgs e)
        {
            GridView view = sender as GridView;

            if (e.KeyCode == Keys.Delete)
            {
                GridCell[] cells = view.GetSelectedCells();
                // 선택된 셀의 공정 수량을 0으로 업데이트
                foreach (GridCell cell in cells)
                    view.SetRowCellValue(cell.RowHandle, "OP_QTY", "0");
            }
            else if (e.KeyCode == Keys.Enter)
            {
                // 최초 Enter 키 입력 시 개체 참조 오류를 피하기 위함
                if (gvwOperation.EditingValue != null)
                {
                    // 입력한 공정 수량
                    string opQty = gvwOperation.EditingValue.ToString();

                    GridCell[] cells = view.GetSelectedCells();
                    // 선택된 셀의 공정 수량 업데이트
                    foreach (GridCell cell in cells)
                        view.SetRowCellValue(cell.RowHandle, "OP_QTY", opQty);
                }
            }
        }

        #endregion

        #region 사용자 정의 함수

        /// <summary>
        /// 폼 로드 시 컨트롤 초기 속성 설정
        /// </summary>
        private void SetInitialPropertyOfControl()
        {
            try
            {
                string worksheetStatus = SOURCE_BOM_INFO[4];

                if (worksheetStatus == "C" || worksheetStatus == "Y" || worksheetStatus == "K")
                {
                    // 이미 작업을 요청한 경우
                    btnSetByModel.Enabled = false;
                    btnCopy.Enabled = false;
                    btnPromoTag.Enabled = false;

                    meRework.Enabled = false;
                    //grdSMS.Enabled = false;
                    grdOperation.Enabled = false;

                    chkVJ.Properties.ReadOnly = true;
                    chkJJ.Properties.ReadOnly = true;
                    chkQD.Properties.ReadOnly = true;
                    chkDS.Properties.ReadOnly = true;
                    chkRJ.Properties.ReadOnly = true;

                    deSampleETS.Properties.ReadOnly = true;
                    txtUpperMtl.Properties.ReadOnly = true;
                    txtMidsoleMtl.Properties.ReadOnly = true;
                    txtOutsoleMtl.Properties.ReadOnly = true;

                    txtLastCd.Properties.ReadOnly = true;
                    txtPatternId.Properties.ReadOnly = true;
                    txtStlFile.Properties.ReadOnly = true;
                    txtSampleWeight.Properties.ReadOnly = true;
                    txtHeelHeight.Properties.ReadOnly = true;
                    txtMedialHeight.Properties.ReadOnly = true;
                    txtLateralHeight.Properties.ReadOnly = true;
                    txtLaceLength.Properties.ReadOnly = true;
                    txtMSHardness.Properties.ReadOnly = true;
                    txtIDSLength.Properties.ReadOnly = true;
                    txtLengthToe.Properties.ReadOnly = true;
                    txtMSCode.Properties.ReadOnly = true;
                    txtOSCode.Properties.ReadOnly = true;
                    txtWSQty.Properties.ReadOnly = true;
                    txtNikeSendQty.Properties.ReadOnly = true;
                    txtDPA.Properties.ReadOnly = true;
                    txtPMO.Properties.ReadOnly = true;
                    txtWHQDev.Properties.ReadOnly = true;
                    txtDevSampleReqId.Properties.ReadOnly = true;
                    txtIPW.Properties.ReadOnly = true;
                    txtBOMID.Properties.ReadOnly = true;
                    txtSubRemark.Properties.ReadOnly = true;

                    txtNeedleSize.Properties.ReadOnly = true;
                    txtSPI.Properties.ReadOnly = true;
                    txtStitchMargin.Properties.ReadOnly = true;
                    txtTwoRowStitchMargin.Properties.ReadOnly = true;
                    txtThreadType.Properties.ReadOnly = true;

                    txtComment1.Properties.ReadOnly = true;
                    txtComment2.Properties.ReadOnly = true;
                    txtComment3.Properties.ReadOnly = true;
                    txtComment4.Properties.ReadOnly = true;
                    txtComment5.Properties.ReadOnly = true;
                    txtComment6.Properties.ReadOnly = true;
                    txtComment7.Properties.ReadOnly = true;

                    chkProtoType.Properties.ReadOnly = true;
                    chkWaterJet.Properties.ReadOnly = true;
                    chkDCS.Properties.ReadOnly = true;
                    chkRework.Properties.ReadOnly = true;
                    chkInnovation.Properties.ReadOnly = true;
                    //chkCBD.Properties.ReadOnly = true;
                }

                // Fake BOM의 경우 SetByModel 버튼 비활성화
                if (SOURCE_BOM_INFO[5] == "F")
                {
                    btnCreate.Enabled = false;
                    btnSetByModel.Enabled = false;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
                return;
            }
        }

        /// <summary>
        /// 작업 유형에 맞은 데이터를 가져옴
        /// </summary>
        /// <returns></returns>
        private DataTable GetWorksheetContents(string _workType)
        {
            try
            {
                PKG_INTG_BOM_WORKSHEET.LOAD_WORKSHEET_CONTENTS pkgSelect = new PKG_INTG_BOM_WORKSHEET.LOAD_WORKSHEET_CONTENTS();
                pkgSelect.ARG_WORK_TYPE = _workType;
                pkgSelect.ARG_FACTORY = SOURCE_BOM_INFO[0];
                pkgSelect.ARG_WS_NO = SOURCE_BOM_INFO[1];
                pkgSelect.OUT_CURSOR = string.Empty;

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
        /// 
        /// </summary>
        /// <param name="partType"></param>
        /// <returns></returns>
        private DataTable GetWorksheetBottomMaterial(string partType)
        {
            try
            {
                string CS_BOM_CFM = SOURCE_BOM_INFO[5];

                PKG_INTG_BOM_WORKSHEET.LOAD_BOTTOM_MATERIALS pkgSelect = new PKG_INTG_BOM_WORKSHEET.LOAD_BOTTOM_MATERIALS();
                pkgSelect.ARG_WORK_TYPE = (CS_BOM_CFM == "F") ? "Fake" : "Inline";
                pkgSelect.ARG_FACTORY = SOURCE_BOM_INFO[0];
                pkgSelect.ARG_WS_NO = SOURCE_BOM_INFO[1];
                pkgSelect.ARG_PART_TYPE = (CS_BOM_CFM == "F") ? ((partType == "M") ? "MIDSOLE" : "OUTSOLE") : partType;
                pkgSelect.OUT_CURSOR = string.Empty;

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
        /// 전달 받은 데이터소스를 참조하여 컨트롤에 데이터를 바인딩 한다.
        /// </summary>
        /// <param name="dataSource"></param>
        private void BindDataToControl(DataTable dataSource, string workType)
        {
            if (workType == "Tag_Other")
            {
                #region Tag + Others

                if (dataSource.Rows.Count > 0)
                {
                    // Tag Information
                    leDevTeam.EditValue = dataSource.Rows[0]["CATEGORY"].ToString();
                    txtDevStyleName.Text = dataSource.Rows[0]["DEV_STYLE_NAME_1"].ToString();
                    txtStyleNumber.Text = dataSource.Rows[0]["DEV_STYLE_NUMBER"].ToString();
                    leSeason.EditValue = dataSource.Rows[0]["SEASON_CD"].ToString();
                    leTD.EditValue = dataSource.Rows[0]["TD"].ToString();
                    txtDevSampleReqId.Text = dataSource.Rows[0]["DEV_SAMPLE_REQ_ID"].ToString();
                    txtColorwayName.Text = dataSource.Rows[0]["COLORWAY_NAME_1"].ToString();
                    txtSize.Text = dataSource.Rows[0]["SAMPLE_SIZE"].ToString();
                    leGender.EditValue = dataSource.Rows[0]["GENDER"].ToString();
                    deSampleETS.EditValue = dataSource.Rows[0]["SAMPLE_ETS_1"];
                    txtWHQDev.Text = dataSource.Rows[0]["WHQ_DEV"].ToString();
                    txtUpperMtl.Text = dataSource.Rows[0]["UPPER_MATERIAL"].ToString();
                    txtMidsoleMtl.Text = dataSource.Rows[0]["MS_MATERIAL"].ToString();
                    txtOutsoleMtl.Text = dataSource.Rows[0]["OUTSOLE_MATERIAL"].ToString();
                    txtLastCd.Text = dataSource.Rows[0]["LAST_CD"].ToString();
                    txtPatternId.Text = dataSource.Rows[0]["PATTERN_ID"].ToString();
                    txtStlFile.Text = dataSource.Rows[0]["STL_FILE"].ToString();

                    leSampleType.EditValue = dataSource.Rows[0]["ST_CD"].ToString();

                    // 프로모 라운드의 경우 별도의 태그 버튼 활성화
                    if (leSampleType.EditValue.ToString() == "PRM" || leSampleType.EditValue.ToString() == "PRMC")
                        btnPromoTag.Visible = true;

                    leSubType.EditValue = dataSource.Rows[0]["SUB_ST_CD"].ToString();
                    if (leSubType.EditValue.ToString() == "NA")
                        txtSubRemark.Enabled = false;

                    txtSampleWeight.Text = dataSource.Rows[0]["SAMPLE_WEIGHT"].ToString();
                    //txtCollarHeight.Text = _dataSource.Rows[0]["COLLAR_HEIGHT"].ToString();
                    txtHeelHeight.Text = dataSource.Rows[0]["HEEL_HEIGHT"].ToString();
                    txtMedialHeight.Text = dataSource.Rows[0]["MEDIAL_HEIGHT"].ToString();
                    txtLateralHeight.Text = dataSource.Rows[0]["LATERAL_HEIGHT"].ToString();
                    txtLaceLength.Text = dataSource.Rows[0]["LACE_LENGTH"].ToString();
                    txtMSHardness.Text = dataSource.Rows[0]["MS_HARDNESS"].ToString();
                    txtIDSLength.Text = dataSource.Rows[0]["IDS_LENGTH"].ToString();
                    txtLengthToe.Text = dataSource.Rows[0]["LENGTH_TOE_SPRING"].ToString();
                    txtMSCode.Text = dataSource.Rows[0]["MS_CODE"].ToString();
                    txtOSCode.Text = dataSource.Rows[0]["OS_CODE"].ToString();
                    txtProductId.Text = dataSource.Rows[0]["PRODUCT_ID"].ToString();
                    deReqdate.EditValue = dataSource.Rows[0]["REQ_DATE_1"];
                    txtCDCDev.Text = dataSource.Rows[0]["PCC_PM"].ToString();
                    txtPMO.Text = dataSource.Rows[0]["NIKE_DEV"].ToString();
                    txtWSQty.Text = dataSource.Rows[0]["WS_QTY"].ToString();
                    txtNikeSendQty.Text = dataSource.Rows[0]["NIKE_SEND_QTY"].ToString();
                    txtIPW.Text = dataSource.Rows[0]["IPW"].ToString();
                    // Others Information
                    txtDPA.Text = dataSource.Rows[0]["DPA"].ToString();
                    txtBOMID.Text = dataSource.Rows[0]["BOM_ID"].ToString();
                    txtDevFactory.Text = SOURCE_BOM_INFO[0];

                    #region Prod. Factory
                    string prodFactory = dataSource.Rows[0]["PROD_FACTORY"].ToString();
                    string[] factories = prodFactory.Split('/');
                    // 선택된 생산 공장 체크 표기
                    if (factories.Length > 0)
                    {
                        foreach (string factory in factories)
                        {
                            if (factory == "VJ")
                                chkVJ.EditValue = "Y";
                            else if (factory == "JJ")
                                chkJJ.EditValue = "Y";
                            else if (factory == "QD")
                                chkQD.EditValue = "Y";
                            else if (factory == "RJ")
                                chkRJ.EditValue = "Y";
                            else if (factory == "DS")
                                chkDS.EditValue = "Y";
                            else if (factory == "OT")
                                chkOther.EditValue = "Y";
                        }
                    }
                    #endregion

                    txtColorwayId.Text = dataSource.Rows[0]["DEV_COLORWAY_ID"].ToString();
                    txtProductCode.Text = dataSource.Rows[0]["STYLE_CD"].ToString();
                    deBOMIssueDate.EditValue = dataSource.Rows[0]["BOM_ISSUE_DATE"].ToString();
                    txtSubRemark.Text = dataSource.Rows[0]["SUB_TYPE_REMARK"].ToString();
                    meRework.Text = dataSource.Rows[0]["REWORK_COMMENT"].ToString();

                    txtNeedleSize.Text = dataSource.Rows[0]["NEEDLE_SIZE"].ToString();
                    txtSPI.Text = dataSource.Rows[0]["SPI"].ToString();
                    txtStitchMargin.Text = dataSource.Rows[0]["STITCHING_MARGIN"].ToString();
                    txtTwoRowStitchMargin.Text = dataSource.Rows[0]["TWOROW_STITCHING_MARGIN"].ToString();
                    txtThreadType.Text = dataSource.Rows[0]["THREAD_TYPE"].ToString();

                    #region 체크 항목 표기 여부
                    string protoYN = dataSource.Rows[0]["PROTO_YN"].ToString();
                    if (protoYN == "Y")
                        chkProtoType.Checked = true;
                    else
                        chkProtoType.Checked = false;

                    string waterJetYN = dataSource.Rows[0]["WATER_JET_YN"].ToString();
                    if (waterJetYN == "Y")
                        chkWaterJet.Checked = true;
                    else
                        chkWaterJet.Checked = false;

                    string dcs = dataSource.Rows[0]["DCS_YN"].ToString();
                    if (dcs == "Y")
                        chkDCS.Checked = true;
                    else
                        chkDCS.Checked = false;

                    string reworkYN = dataSource.Rows[0]["REWORK_YN"].ToString();
                    if (reworkYN == "Y")
                        chkRework.Checked = true;
                    else
                        chkRework.Checked = false;

                    string innoYN = dataSource.Rows[0]["INNOVATION_YN"].ToString();
                    if (innoYN == "Y")
                        chkInnovation.Checked = true;
                    else
                        chkInnovation.Checked = false;

                    string cbdYN = dataSource.Rows[0]["CBD_YN"].ToString();
                    if (cbdYN == "Y")
                        chkCBD.Checked = true;
                    else
                        chkCBD.Checked = false;
                    #endregion
                }

                #endregion
            }
            else if (workType == "Comment")
            {
                #region Comment
                if (dataSource.Rows.Count > 0)
                {

                    // Oracle Pivot 사용하여 Seq 기준으로 코멘트를 열로 나열
                    txtComment1.Text = dataSource.Rows[0]["1"].ToString();
                    txtComment2.Text = dataSource.Rows[0]["2"].ToString();
                    txtComment3.Text = dataSource.Rows[0]["3"].ToString();
                    txtComment4.Text = dataSource.Rows[0]["4"].ToString();
                    txtComment5.Text = dataSource.Rows[0]["5"].ToString();
                    txtComment6.Text = dataSource.Rows[0]["6"].ToString();
                    txtComment7.Text = dataSource.Rows[0]["7"].ToString();

                }
                else
                {
                    txtComment1.Text = "";
                    txtComment2.Text = "";
                    txtComment3.Text = "";
                    txtComment4.Text = "";
                    txtComment5.Text = "";
                    txtComment6.Text = "";
                    txtComment7.Text = "";
                }
                #endregion
            }
            else if (workType == "Image")
            {
                #region Image
                if (dataSource.Rows.Count > 0)
                {
                    byte[] data = null;

                    foreach (DataRow dr in dataSource.Rows)
                    {
                        if (dr["IMG_TYPE"].ToString() == "UPPER")
                        {
                            if (Common.sessionFactory == "DS")
                            {
                                if (Convert.ToInt32(dr["IMG_SEQ"]) >= 4)
                                    continue;
                            }

                            data = (byte[])dr["RAW_FILE"]; //Image

                            Image image = ConvertByteArrayToImage(data);

                            EDITS_UPPER_PICTURES[Convert.ToInt32(dr["IMG_SEQ"])].Image = image;

                            if (dr["IMG_TITLE"].ToString().Trim().Replace(" ", "").ToUpper().Equals(""))
                            {
                                EDITS_UPPER_TEXT[Convert.ToInt32(dr["IMG_SEQ"])].Text = "Image " + (Convert.ToInt32(dr["IMG_SEQ"])).ToString();
                            }
                            else
                            {
                                EDITS_UPPER_TEXT[Convert.ToInt32(dr["IMG_SEQ"])].Text = dr["IMG_TITLE"].ToString();
                            }
                        }
                        else if (dr["IMG_TYPE"].ToString() == "MIDSOLE")
                        {
                            data = (byte[])dr["RAW_FILE"]; //Image

                            Image img = ConvertByteArrayToImage(data);
                            peMidsole.Image = img;
                        }
                        else if (dr["IMG_TYPE"].ToString() == "OUTSOLE")
                        {
                            data = (byte[])dr["RAW_FILE"]; //Image

                            Image img = ConvertByteArrayToImage(data);
                            peOutsole.Image = img;
                        }
                        else if (dr["IMG_TYPE"].ToString() == "PROMO")
                        {
                            data = (byte[])dr["RAW_FILE"]; //Image

                            Image img = ConvertByteArrayToImage(data);
                            pePromoCover.Image = img;
                        }
                    }
                }
                else
                {
                    peUpperHQ1.Image = null;
                    peUpperHQ2.Image = null;
                    peUpperHQ3.Image = null;
                    peUpperHQ4.Image = null;
                    peMidsole.Image = null;
                    peOutsole.Image = null;
                    pePromoCover.Image = null;

                    teUpperHQ1.Text = "";
                    teUpperHQ2.Text = "";
                    teUpperHQ3.Text = "";
                    teUpperHQ4.Text = "";
                }
                #endregion
            }
        }

        /// <summary>
        /// 체크되어 있는 생산 공장에 따라 코드 채번 ex) VJ/JJ, JJ/QD, ...
        /// </summary>
        /// <returns></returns>
        private string CreateProdFactoryCode()
        {
            // 각 생산 공장을 저장할 배열
            string[] prodFactoryList = new string[6];

            // 각 공장의 체크 여부 확인 후 생산 공장명 저장
            if (chkVJ.Checked)
                prodFactoryList[0] = "VJ";

            if (chkJJ.Checked)
                prodFactoryList[1] = "JJ";

            if (chkQD.Checked)
                prodFactoryList[2] = "QD";

            if (chkRJ.Checked)
                prodFactoryList[3] = "RJ";

            if (chkDS.Checked)
                prodFactoryList[4] = "DS";

            if (chkOther.Checked)
                prodFactoryList[5] = "OT";

            // 선행하는 배열요소에 데이터 존재 여부
            bool existingInFront = false;

            // 최종적으로 사용할 생산 공장 코드
            string prodFactoryCode = string.Empty;

            // 선택한 생산 공장으로 신규 코드를 만듦 ex) VJ/JJ, JJ/QD...
            foreach (string factory in prodFactoryList)
            {
                if (factory != null)
                {
                    if (existingInFront == true)
                        prodFactoryCode += "/";

                    prodFactoryCode += factory;
                    existingInFront = true;
                }
            }
            return prodFactoryCode;
        }

        /// <summary>
        /// DateTime 형식을 String 형식으로 컨버팅
        /// </summary>
        /// <returns></returns>
        private string ConvertDateEditValue()
        {
            // 반환할 Sample ETS
            string sampleETS = string.Empty;

            // 바인딩되어 있는 값
            object obj = deSampleETS.EditValue;

            // 바인딩되어 있는 값의 타입
            Type type = obj.GetType();

            if (type.Name == "DateTime")
            {
                string orgValue = obj.ToString();
                DateTime time = Convert.ToDateTime(orgValue);

                // yyyyMMdd 형식으로 변경
                sampleETS = time.ToString("yyyyMMdd");
            }
            return sampleETS;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="image"></param>
        private byte[] ConvertImageToByteArray(Image image)
        {
            try
            {
                MemoryStream stream = new MemoryStream();
                // Saves this image to the specified stream in the specified format.
                image.Save(stream, ImageFormat.Gif);
                // Writes the stream contents to a byte array, regardless of the Position property.
                return stream.ToArray();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
                return null;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="byteArray"></param>
        /// <returns></returns>
        private Image ConvertByteArrayToImage(byte[] byteArray)
        {
            try
            {
                // Initializes a new non-resizable instance of the MemoryStream class based on the specified byte array.
                MemoryStream stream = new MemoryStream(byteArray);
                // Creates an Image from the specified data stream.
                Image image = Image.FromStream(stream);
                return image;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
                return null;
            }
        }

        /// <summary>
        /// string -> decimal 변환
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        decimal ConvertDecimal(string str)
        {
            try
            {
                if (String.IsNullOrEmpty(str))
                {
                    return 0;
                }
                else
                {
                    return Convert.ToDecimal(txtWSQty.Text);
                }
            }
            catch
            {
                MessageBox.Show("Only numbers can be entered.");
                return 0;
            }
        }

        #endregion

        #region 컨텍스트 메뉴 이벤트

        /// <summary>
        /// 컨텍스트 메뉴 이벤트 처리
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ContextMenu_Click(object sender, EventArgs e)
        {
            // 선택된 컨텍스트 메뉴 아이템
            ToolStripMenuItem menuItem = sender as ToolStripMenuItem;
            // 이벤트를 발생시킨 컨트롤
            ContextMenuStrip ctxMenuStrip = menuItem.Owner as ContextMenuStrip;

            if (ctxMenuStrip.Name == "ctxMenuImage")
            {
                Control control = FindControlAtPoint(this, this.PointToClient(Cursor.Position));

                if (control is PictureEdit)
                {
                    PictureEdit pictureEidt = control as PictureEdit;

                    switch (menuItem.Name)
                    {
                        case "Open":
                            OpenImageFile(pictureEidt);
                            break;

                        case "Copy":
                            CopyImageFile(pictureEidt);
                            break;

                        case "Paste":
                            PasteImageFile(pictureEidt);
                            break;

                        case "Delete":
                            DeleteImageFile(pictureEidt);
                            break;

                        default:
                            break;
                    }
                }
                else
                    return;
            }
            else
            {
                switch (menuItem.Name)
                {
                    case "Enroll":
                        EnrollSMSUser();
                        break;

                    case "Exclude":
                        ExcludeSelectedUser();
                        break;

                    case "MultiCheck":
                        OperationMultiCheck();
                        break;
                }
            }
        }

        /// <summary>
        /// 유저가 선택한 이미지 파일을 업로드
        /// </summary>
        /// <param name="_pictureEidt"></param>
        private void OpenImageFile(PictureEdit _pictureEidt)
        {
            OpenFileDialog dialog = new OpenFileDialog();

            dialog.DefaultExt = "jpg, gif, png";
            dialog.Filter = "Image File (*.jpg)|*.jpg|Image File(*.gif)|*.gif|Image File(*.png)|*.png";
            dialog.RestoreDirectory = false;

            if (dialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                // 파일의 절대 경로
                string filePath = dialog.FileName;

                Image image = Image.FromFile(filePath);
                // 이미지 폭, 높이의 최대값
                double maxWidth = 400D;
                double maxHeight = 300D;
                // 업로드한 이미지의 폭, 높이
                double width = Convert.ToDouble(image.Width);
                double height = Convert.ToDouble(image.Height);
                // 최대값 보다 클 경우 이미지 리사이징
                if (width > maxWidth || height > maxHeight)
                {
                    if (width > height)
                    {
                        double factor = maxWidth / width;

                        height *= factor;
                        width = maxWidth;
                    }
                    else
                    {
                        double factor = maxHeight / width;

                        width *= factor;
                        height = maxHeight;
                    }
                    Bitmap bitmap = new Bitmap(image, Convert.ToInt32(width), Convert.ToInt32(height));
                    image = (Image)bitmap;
                }
                _pictureEidt.Image = image;
                //FILE_NAME = dialog.FileName.Substring(dialog.FileName.LastIndexOf("\\") + 1);
            }
        }

        /// <summary>
        /// 커서 위치의 이미지를 클립보드에 복사
        /// </summary>
        /// <param name="_pictureEdit"></param>
        private void CopyImageFile(PictureEdit _pictureEdit)
        {
            if (_pictureEdit.Image == null) return;

            Clipboard.SetImage(_pictureEdit.Image);
        }

        /// <summary>
        /// 클립보드에 저장된 이미지를 커서 위치의 컨트롤에 바인딩
        /// </summary>
        /// <param name="_pictureEdit"></param>
        private void PasteImageFile(PictureEdit _pictureEdit)
        {
            try
            {
                if (Clipboard.GetDataObject() == null)
                    return;
                else
                {
                    OBJECT = Clipboard.GetDataObject();

                    Image image;

                    string[] strPicDir;

                    int imgFwidth;
                    int imgFheight;

                    System.Drawing.Bitmap update_image;

                    int iWidth = _pictureEdit.Size.Width;
                    int iHeight = _pictureEdit.Size.Height;

                    if (OBJECT.GetDataPresent(DataFormats.Bitmap))
                    {
                        image = (Image)OBJECT.GetData(DataFormats.Bitmap);
                        imgFwidth = image.Width;
                        imgFheight = image.Height;
                        // 이미지 크기가 기본 영역보다 클 경우는 줄여서 load
                        imgFwidth = (imgFwidth < iWidth) ? imgFwidth : iWidth;
                        imgFheight = (imgFheight < iHeight) ? imgFheight : iHeight;

                        update_image = new System.Drawing.Bitmap(image, imgFwidth, imgFheight);
                        image = (Image)update_image;

                        _pictureEdit.Image = image;
                    }
                    else if (OBJECT.GetDataPresent(DataFormats.FileDrop))
                    {
                        strPicDir = (string[])Clipboard.GetDataObject().GetData(DataFormats.FileDrop);
                        image = System.Drawing.Image.FromFile(strPicDir[0]);
                        imgFwidth = image.Width;
                        imgFheight = image.Height;
                        // 이미지 크기가 기본 영역보다 클 경우는 줄여서 load
                        imgFwidth = (imgFwidth < iWidth) ? imgFwidth : iWidth;
                        imgFheight = (imgFheight < iHeight) ? imgFheight : iHeight;

                        update_image = new System.Drawing.Bitmap(image, imgFwidth, imgFheight);
                        image = (Image)update_image;

                        _pictureEdit.Image = image;
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
        /// <param name="_pictureEidt"></param>
        private void DeleteImageFile(PictureEdit _pictureEidt)
        {
            try
            {
                if (_pictureEidt == null) return;

                _pictureEidt.Image.Dispose();
                _pictureEidt.Image = null;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
                return;
            }
        }

        /// <summary>
        /// SMS를 전송 받을 유저를 찾은 후 등록
        /// </summary>
        private void EnrollSMSUser()
        {
            try
            {
                // 유저 검색
                object returnObj = projectBaseForm.OpenChildForm(@"\POPUP\CSI.PCC.SYS.P_FindUser.dll", null,
                    JPlatform.Client.Library.interFace.OpenType.Modal);
                // 입력 받은 유저 정보가 있을 경우
                if (returnObj != null)
                {
                    Dictionary<string, string> dic = (Dictionary<string, string>)returnObj;

                    PKG_INTG_BOM.INSERT_SMS_USER pkgInsert = new PKG_INTG_BOM.INSERT_SMS_USER();
                    pkgInsert.ARG_FACTORY = Common.sessionFactory;
                    pkgInsert.ARG_WS_NO = SOURCE_BOM_INFO[1];
                    pkgInsert.ARG_USER_ID = dic["USER_ID"];
                    pkgInsert.ARG_UPD_USER = Common.sessionID;
                    
                    ArrayList arrayList = new ArrayList();
                    arrayList.Add(pkgInsert);
                    
                    projectBaseForm.Exe_Modify_PKG(arrayList);
                    
                    grdSMS.DataSource = GetWorksheetContents("SMS");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
                return;
            }
        }

        /// <summary>
        /// 선택한 유저 SMS 등록 리스트에서 제외
        /// </summary>
        private void ExcludeSelectedUser()
        {
            try
            {
                // 선택된 행의 로우핸들 배열
                int[] rowHandles = gvwSMS.GetSelectedRows();

                foreach (int rowHandle in rowHandles)
                {
                    string dpa = gvwSMS.GetRowCellValue(rowHandle, "DPA").ToString();
                    bool isExisting = (dpa == "") ? false : true;

                    if (!isExisting)
                    {
                        // 등록된 사용자의 ID
                        string userID = gvwSMS.GetRowCellValue(rowHandle, "USER_ID").ToString();

                        PKG_INTG_BOM.DELETE_SMS_USER pkgDelete = new PKG_INTG_BOM.DELETE_SMS_USER();
                        pkgDelete.ARG_FACTORY = Common.sessionFactory;
                        pkgDelete.ARG_WS_NO = SOURCE_BOM_INFO[1];
                        pkgDelete.ARG_USER_ID = userID;
                        
                        ArrayList arrayList = new ArrayList();
                        arrayList.Add(pkgDelete);
                        
                        projectBaseForm.Exe_Modify_PKG(arrayList);
                    }
                    else
                    {
                        // PMX에 담당자로 등록된 유저는 제외할 수 없음
                        MessageBox.Show("User enrolled automatically by system can not be excluded.", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        continue;
                    }
                }
                // SMS 그리드 데이터 소스 리바인딩
                grdSMS.DataSource = GetWorksheetContents("SMS");
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
                return;
            }
        }

        /// <summary>
        /// 현재 커서 위치의 컨트롤을 가져옴
        /// </summary>
        /// <param name="container"></param>
        /// <param name="pos"></param>
        /// <returns></returns>
        private static Control FindControlAtPoint(Control container, Point pos)
        {
            Control child;

            foreach (Control c in container.Controls)
            {
                if (c.Visible && c.Bounds.Contains(pos))
                {
                    child = FindControlAtPoint(c, new Point(pos.X - c.Left, pos.Y - c.Top));
                    if (child == null) return c;
                    else return child;
                }
            }
            return null;
        }

        /// <summary>
        /// 
        /// </summary>
        private void OperationMultiCheck()
        {
            // 선택된 행의 로우핸들 배열
            int[] rowHandles = gvwOperation.GetSelectedRows();
            // 선택된 행의 개수만큼 반복
            foreach (int rowHandle in rowHandles)
            {
                // 공정 체크 여부
                string isChecked = gvwOperation.GetRowCellValue(rowHandle, "OP_CHK").ToString();
                // 체크될 공정은 수량 및 일정도 같이 업데이트
                if (isChecked == "N")
                {
                    gvwOperation.SetRowCellValue(rowHandle, "OP_CHK", "Y");
                    gvwOperation.SetRowCellValue(rowHandle, "OP_QTY", txtWSQty.Text);
                    gvwOperation.SetRowCellValue(rowHandle, "OP_YMD", ConvertDateEditValue());
                }
                else
                {
                    // 체크 해제될 공정은 수량을 0으로 초기화
                    gvwOperation.SetRowCellValue(rowHandle, "OP_CHK", "N");
                    gvwOperation.SetRowCellValue(rowHandle, "OP_QTY", "0");
                }
            }
        }

        #endregion

        #region 버튼 이벤트

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnSave_Click(object sender, EventArgs e)
        {
            try
            {
                if (CheckRequiredField())
                {
                    if (SaveTagAndOtherInforatmion())
                    {
                        if (SaveCommentInformation())
                        {
                            CalcOperationLeadTime();

                            if (SaveOperationInformation())
                            {

                                #region 기존 이미지 삭제
                                // 패키지 호출용 임시 변수
                                ArrayList arrayList4 = new ArrayList();

                                PKG_INTG_BOM.DELETE_BOM_IMAGE pkgDelete = new PKG_INTG_BOM.DELETE_BOM_IMAGE();
                                pkgDelete.ARG_FACTORY = Common.sessionFactory;
                                pkgDelete.ARG_WS_NO = SOURCE_BOM_INFO[1];

                                arrayList4.Add(pkgDelete);
                                // 패키지 호출 - 기존 이미지 삭제
                                projectBaseForm.Exe_Modify_PKG(arrayList4);
                                #endregion

                                #region 신규 이미지 저장

                                string imageType = string.Empty;
                                string imageTitle = string.Empty;
                                int imageSeq = 0;
                                byte[] image = null;

                                ArrayList arrayList5 = new ArrayList();

                                // EDITS_UPPER_PICTURES : 세션 Factory에 따라 멤버 개수 다름
                                // +3의 경우 Midsole, Outsole, Cover Image를 저장하기 위함(반복 횟수 강제 제어)
                                for (int i = 0; i < EDITS_UPPER_PICTURES.Length + 3; i++)
                                {
                                    if (i < EDITS_UPPER_PICTURES.Length)
                                    {
                                        /************* Upper 작업 *************/

                                        // 이미지가 없는 경우 건너띔
                                        if (EDITS_UPPER_PICTURES[i].Image == null) continue;

                                        imageSeq = i;
                                        imageType = "UPPER";
                                        imageTitle = EDITS_UPPER_TEXT[Convert.ToInt32(imageSeq)].Text;
                                        image = ConvertImageToByteArray(EDITS_UPPER_PICTURES[Convert.ToInt32(imageSeq)].Image);
                                    }
                                    else if (i == EDITS_UPPER_PICTURES.Length)
                                    {
                                        /************* Midsole 작업 *************/

                                        // 이미지가 없는 경우 건너띔
                                        if (peMidsole.Image == null) continue;

                                        imageSeq = 0;
                                        imageType = "MIDSOLE";
                                        imageTitle = "";
                                        image = ConvertImageToByteArray(peMidsole.Image);
                                    }
                                    else if (i == EDITS_UPPER_PICTURES.Length + 1)
                                    {
                                        /************* Outsole 작업 *************/

                                        // 이미지가 없는 경우 건너띔
                                        if (peOutsole.Image == null) continue;

                                        imageSeq = 0;
                                        imageType = "OUTSOLE";
                                        imageTitle = "";
                                        image = ConvertImageToByteArray(peOutsole.Image);
                                    }
                                    else if (i == EDITS_UPPER_PICTURES.Length + 2)
                                    {
                                        /************* Promo Cover 작업 *************/

                                        if (pePromoCover.Image == null) continue;

                                        imageSeq = 0;
                                        imageType = "PROMO";
                                        imageTitle = "";
                                        image = ConvertImageToByteArray(pePromoCover.Image);
                                    }
                                    else
                                        continue;

                                    PKG_INTG_BOM.INSERT_BOM_IMAGE pkgInsert = new PKG_INTG_BOM.INSERT_BOM_IMAGE();
                                    pkgInsert.ARG_FACTORY = Common.sessionFactory;
                                    pkgInsert.ARG_WS_NO = SOURCE_BOM_INFO[1];
                                    pkgInsert.ARG_IMAGE_TYPE = imageType;
                                    pkgInsert.ARG_IMAGE_SEQ = imageSeq.ToString();
                                    pkgInsert.ARG_IMAGE_TITLE = imageTitle;
                                    pkgInsert.ARG_IMAGE_FILE = image;
                                    pkgInsert.ARG_UPD_USER = Common.sessionID;

                                    arrayList5.Add(pkgInsert);
                                }

                                if (arrayList5.Count > 0)
                                {
                                    if (projectBaseForm.Exe_Modify_PKG(arrayList5) == null)
                                    {
                                        MessageBox.Show("Failed to save image.");
                                        return;
                                    }
                                }

                                #endregion
                                
                                MessageBox.Show("Complete.");

                                isCopied = false;
                                this.Close();
                            }
                        }
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
        /// 특정 BOM을 검색하여 Worksheet Data를 복사
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnCopy_Click(object sender, EventArgs e)
        {
            BOMSearch form = new BOMSearch()
            {
                Factory = SOURCE_BOM_INFO[0],
                WSNumber = SOURCE_BOM_INFO[1],
                StyleName = SOURCE_BOM_INFO[2],
                Mode = "Worksheet"
            };

            if (form.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                DataTable dataSource = GetWorksheetContents("Tag_Other");
                BindDataToControl(dataSource, "Tag_Other");

                // 작업지시서 SMS 정보 바인딩
                dataSource = GetWorksheetContents("SMS");
                grdSMS.DataSource = dataSource;

                // 작업지시서 코멘트 정보 바인딩
                dataSource = GetWorksheetContents("Comment");
                BindDataToControl(dataSource, "Comment");

                // 작업지시서 공정 정보 바인딩
                dataSource = GetWorksheetContents("Operation");
                grdOperation.DataSource = dataSource;

                // 이미지 바인딩
                dataSource = GetWorksheetContents("Image");
                BindDataToControl(dataSource, "Image");

                isCopied = true;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnCreate_Click(object sender, EventArgs e)
        {
            // [6] : styleNumber
            // [7] : season
            string[] keyValue = new string[] { SOURCE_BOM_INFO[6], SOURCE_BOM_INFO[7] };

            if (SOURCE_BOM_INFO[6] == "" || SOURCE_BOM_INFO[7] == "")
            {
                MessageBox.Show("Season Code or Style Number is not input. Please check it.");
                return;
            }

            WorksheetTemplate form = new WorksheetTemplate();
            form.KEY_VALUE = keyValue;

            form.Show();
        }

        /// <summary>
        /// DPA 또는 모델 단위로 유저가 설정한 기본 값을 가져옴
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnSetByModel_Click(object sender, EventArgs e)
        {
            string selectedType = string.Empty;      // Load할 템플릿 타입
            string keyValue = string.Empty;

            // 타입 선택
            TypeSelection_WSTmplt typeForm = new TypeSelection_WSTmplt();

            if (typeForm.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                selectedType = typeForm.SELECTED_TYPE;
            else
                return;

            if (selectedType == "DPA")
            {
                if (txtDPA.Text.Trim().Length == 0)
                {
                    // DPA 필수 입력
                    MessageBox.Show("Please type DPA.");
                    return;
                }

                keyValue = txtDPA.Text.Replace("-", "").Trim();
            }
            else if (selectedType == "STYLE")
            {
                keyValue = SOURCE_BOM_INFO[7] + "-" + SOURCE_BOM_INFO[6];
            }

            if (BindSpecInfo(keyValue, selectedType) == false)
                return;

            if (SOURCE_BOM_INFO[0] == "DS")
            {
                if (BindComments(keyValue, selectedType) == false)
                    return;
            }

            if (BindOperation(keyValue, selectedType) == false)
                return;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnPromoTag_Click(object sender, EventArgs e)
        {
            Dictionary<string, string> dictionary = new Dictionary<string, string>();

            dictionary.Add("FACTORY", SOURCE_BOM_INFO[0]);
            dictionary.Add("WS_NO", SOURCE_BOM_INFO[1]);

            object returnObj = projectBaseForm.OpenChildForm(@"\POPUP\CSI.PCC.BOM.P_TFPromoTag.dll", dictionary, OpenType.Modal);
        }

        /// <summary>
        /// 해외공장 전용 Thread 입력 폼을 로드한다.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnThreadForOvs_Click(object sender, EventArgs e)
        {
            Dictionary<string, string> dictionary = new Dictionary<string, string>();

            dictionary.Add("FACTORY", SOURCE_BOM_INFO[0]);
            dictionary.Add("WS_NO", SOURCE_BOM_INFO[1]);

            projectBaseForm.OpenChildForm(@"\POPUP\CSI.PCC.BOM.P_ChoiceThread.dll", dictionary, OpenType.Modal);
        }

        /// <summary>
        /// Spec 및 Tag 정보 저장
        /// </summary>
        /// <returns></returns>
        private bool SaveTagAndOtherInforatmion()
        {
            try
            {
                PKG_INTG_BOM_WORKSHEET.UPDATE_TAG_NEEDLE pkgUpdate = new PKG_INTG_BOM_WORKSHEET.UPDATE_TAG_NEEDLE();
                pkgUpdate.ARG_FACTORY = SOURCE_BOM_INFO[0];
                pkgUpdate.ARG_WS_NO = SOURCE_BOM_INFO[1];
                pkgUpdate.ARG_PROD_FACTORY = CreateProdFactoryCode();
                pkgUpdate.ARG_SAMPLE_ETS = ConvertDateEditValue();
                pkgUpdate.ARG_UPPER_MATERIAL = txtUpperMtl.Text;
                pkgUpdate.ARG_MS_MATERIAL = txtMidsoleMtl.Text;
                pkgUpdate.ARG_OUTSOLE_MATERIAL = txtOutsoleMtl.Text;
                pkgUpdate.ARG_LAST_CD = txtLastCd.Text;
                pkgUpdate.ARG_PATTERN_ID = txtPatternId.Text;
                pkgUpdate.ARG_STL_FILE = txtStlFile.Text;
                pkgUpdate.ARG_SAMPLE_WEIGHT = txtSampleWeight.Text;
                pkgUpdate.ARG_HEEL_HEIGHT = txtHeelHeight.Text;
                pkgUpdate.ARG_MEDIAL_HEIGHT = txtMedialHeight.Text;
                pkgUpdate.ARG_LATERAL_HEIGHT = txtLateralHeight.Text;
                pkgUpdate.ARG_LACE_LENGTH = txtLaceLength.Text;
                pkgUpdate.ARG_MS_HARDNESS = txtMSHardness.Text;
                pkgUpdate.ARG_IDS_LENGTH = txtIDSLength.Text;
                pkgUpdate.ARG_LENGTH_TOE_SPRING = txtLengthToe.Text;
                pkgUpdate.ARG_MS_CODE = txtMSCode.Text;
                pkgUpdate.ARG_OS_CODE = txtOSCode.Text;
                pkgUpdate.ARG_WS_QTY = txtWSQty.Text;
                pkgUpdate.ARG_NIKE_SEND_QTY = txtNikeSendQty.Text;

                // '-' 있을 경우 제거하여 등록
                pkgUpdate.ARG_DPA = (txtDPA.Text.Trim().Contains("-")) ? txtDPA.Text.Trim().Replace("-", "") : txtDPA.Text.Trim();

                pkgUpdate.ARG_NIKE_DEV = txtPMO.Text.Trim();
                pkgUpdate.ARG_WHQ_DEV = txtWHQDev.Text.Trim();
                pkgUpdate.ARG_DEV_SAMPLE_REQ_ID = txtDevSampleReqId.Text.Trim();
                pkgUpdate.ARG_IPW = txtIPW.Text.Trim();
                pkgUpdate.ARG_BOM_ID = txtBOMID.Text.Trim();
                pkgUpdate.ARG_NEEDLE_SIZE = txtNeedleSize.Text;
                pkgUpdate.ARG_SPI = txtSPI.Text;
                pkgUpdate.ARG_STITCHING_MARGIN = txtStitchMargin.Text;
                pkgUpdate.ARG_TWOROW_STITCHING_MARGIN = txtTwoRowStitchMargin.Text;
                pkgUpdate.ARG_THREAD_TYPE = txtThreadType.Text;
                pkgUpdate.ARG_PROTO_YN = chkProtoType.Checked ? chkProtoType.Properties.ValueChecked.ToString() : chkProtoType.Properties.ValueUnchecked.ToString();
                pkgUpdate.ARG_WATER_JET_YN = chkWaterJet.Checked ? chkWaterJet.Properties.ValueChecked.ToString() : chkWaterJet.Properties.ValueUnchecked.ToString();
                pkgUpdate.ARG_DCS = chkDCS.Checked ? chkDCS.Properties.ValueChecked.ToString() : chkDCS.Properties.ValueUnchecked.ToString();
                pkgUpdate.ARG_REWORK_YN = chkRework.Checked ? chkRework.Properties.ValueChecked.ToString() : chkRework.Properties.ValueUnchecked.ToString();
                pkgUpdate.ARG_INNOVATION_YN = chkInnovation.Checked ? chkInnovation.Properties.ValueChecked.ToString() : chkInnovation.Properties.ValueUnchecked.ToString();
                pkgUpdate.ARG_CBD_YN = chkCBD.Checked ? chkCBD.Properties.ValueChecked.ToString() : chkCBD.Properties.ValueUnchecked.ToString();
                pkgUpdate.ARG_UPD_USER = Common.sessionID;
                pkgUpdate.ARG_SUB_TYPE_REMARK = txtSubRemark.Text;
                pkgUpdate.ARG_REWORK_COMMENT = meRework.Text;

                ArrayList arrayList = new ArrayList();

                arrayList.Add(pkgUpdate);

                if (projectBaseForm.Exe_Modify_PKG(arrayList) == null)
                {
                    MessageBox.Show("Failed to save Tag info.", "Error",
                        MessageBoxButtons.OK, MessageBoxIcon.Error, MessageBoxDefaultButton.Button1);
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
        /// 코멘트 정보 저장
        /// </summary>
        /// <returns></returns>
        private bool SaveCommentInformation()
        {
            try
            {
                // Comment의 경우 본사만 사용
                if (SOURCE_BOM_INFO[0] == "DS")
                {
                    // 코멘트 컨트롤 배열
                    TextEdit[] edits = new TextEdit[] {txtComment1, txtComment2, txtComment3,
                    txtComment4, txtComment5, txtComment6, txtComment7};

                    // PCC_WS_COMMENT 테이블의 SEQ 컬럼용 임시 변수, 시작은 1부터
                    int sequence = 1;

                    ArrayList arrayList = new ArrayList();

                    foreach (TextEdit edit in edits)
                    {
                        PKG_INTG_BOM_WORKSHEET.UPDATE_COMMENTS pkgUpdateComment = new PKG_INTG_BOM_WORKSHEET.UPDATE_COMMENTS();
                        pkgUpdateComment.ARG_FACTORY = Common.sessionFactory;
                        pkgUpdateComment.ARG_WS_NO = SOURCE_BOM_INFO[1];
                        pkgUpdateComment.ARG_SEQ = sequence.ToString();
                        pkgUpdateComment.ARG_COMMENT = edit.Text;
                        pkgUpdateComment.ARG_UPD_USER = Common.sessionID;

                        arrayList.Add(pkgUpdateComment);
                        sequence++;
                    }

                    if (projectBaseForm.Exe_Modify_PKG(arrayList) == null)
                    {
                        MessageBox.Show("Failed to save W/S Comments.", "Error",
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
        /// 공정 정보 저장
        /// </summary>
        private bool SaveOperationInformation()
        {
            try
            {
                ArrayList arrayList = new ArrayList();

                for (int i = 0; i < gvwOperation.RowCount; i++)
                {
                    PKG_INTG_BOM.UPDATE_WS_OPCD pkgUpdateOperation = new PKG_INTG_BOM.UPDATE_WS_OPCD();
                    pkgUpdateOperation.ARG_FACTORY = Common.sessionFactory;
                    pkgUpdateOperation.ARG_WS_NO = SOURCE_BOM_INFO[1];
                    pkgUpdateOperation.ARG_OP_CD = gvwOperation.GetRowCellValue(i, "OP_CD").ToString();
                    pkgUpdateOperation.ARG_OP_CHK = gvwOperation.GetRowCellValue(i, "OP_CHK").ToString();
                    pkgUpdateOperation.ARG_OP_QTY = gvwOperation.GetRowCellValue(i, "OP_QTY").ToString();
                    pkgUpdateOperation.ARG_OP_YMD = gvwOperation.GetRowCellValue(i, "OP_YMD").ToString();
                    pkgUpdateOperation.ARG_UPD_USER = Common.sessionID;

                    arrayList.Add(pkgUpdateOperation);
                }

                if (projectBaseForm.Exe_Modify_PKG(arrayList) == null)
                {
                    MessageBox.Show("Failed to save operations.", "Error",
                        MessageBoxButtons.OK, MessageBoxIcon.Error, MessageBoxDefaultButton.Button1);
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
        /// 스펙 정보를 템플릿 마스터에서 가져와서 컨트롤에 바인딩 한다.
        /// </summary>
        private bool BindSpecInfo(string keyValue, string keyType)
        {
            try
            {
                DataTable dataSource = GetTemplateDataSource("Spec", keyValue, keyType);

                if (dataSource.Rows.Count > 0)
                {
                    txtLastCd.Text = dataSource.Rows[0]["LAST_CD"].ToString().Trim();
                    txtHeelHeight.Text = dataSource.Rows[0]["HEEL_HEIGHT"].ToString().Trim();
                    txtMedialHeight.Text = dataSource.Rows[0]["MEDIAL_HEIGHT"].ToString().Trim();
                    txtLateralHeight.Text = dataSource.Rows[0]["LATERAL_HEIGHT"].ToString().Trim();
                    txtLaceLength.Text = dataSource.Rows[0]["LACE_LENGTH"].ToString().Trim();
                    txtMSHardness.Text = dataSource.Rows[0]["MS_HARDNESS"].ToString().Trim();
                    txtIDSLength.Text = dataSource.Rows[0]["IDS_LENGTH"].ToString().Trim();
                    txtNeedleSize.Text = dataSource.Rows[0]["NEEDLE_SIZE"].ToString().Trim();
                    txtSPI.Text = dataSource.Rows[0]["SPI"].ToString().Trim();
                    txtStitchMargin.Text = dataSource.Rows[0]["STITCHING_MARGIN"].ToString().Trim();
                    txtTwoRowStitchMargin.Text = dataSource.Rows[0]["TWOROW_STITCHING_MARGIN"].ToString().Trim();
                    txtThreadType.Text = dataSource.Rows[0]["THREAD_TYPE"].ToString().Trim();
                    txtUpperMtl.Text = dataSource.Rows[0]["UPPER_MATERIAL"].ToString().Trim();
                    txtMidsoleMtl.Text = dataSource.Rows[0]["MS_MATERIAL"].ToString().Trim();
                    txtOutsoleMtl.Text = dataSource.Rows[0]["OUTSOLE_MATERIAL"].ToString().Trim();
                    txtMSCode.Text = dataSource.Rows[0]["MS_CODE"].ToString().Trim();
                    txtOSCode.Text = dataSource.Rows[0]["OS_CODE"].ToString().Trim();
                    txtPMO.Text = dataSource.Rows[0]["NIKE_DEV"].ToString().Trim();
                    txtWHQDev.Text = dataSource.Rows[0]["WHQ_DEV"].ToString().Trim();

                    string dcs_yn = dataSource.Rows[0]["DCS_YN"].ToString().Trim();

                    if (dcs_yn == "Y")
                        chkDCS.Checked = true;

                    // 해외공장의 경우 스레드 별도 입력
                    if (SOURCE_BOM_INFO[0] == "DS")
                    {
                        if (txtNeedleSize.Text.Trim() == "" && txtSPI.Text.Trim() == "" && txtStitchMargin.Text.Trim() == ""
                            && txtTwoRowStitchMargin.Text.Trim() == "" && txtThreadType.Text.Trim() == "")
                        {
                            object returnObj = projectBaseForm.OpenChildForm(@"\POPUP\CSI.PCC.MODEL.P_ChoiceBP.dll", null,
                                JPlatform.Client.Library.interFace.OpenType.Modal);

                            if (returnObj != null)
                            {
                                if (returnObj is Dictionary<string, string>)
                                {
                                    Dictionary<string, string> dic = new Dictionary<string, string>();

                                    dic = (Dictionary<string, string>)returnObj;
                                    txtNeedleSize.Text = dic["NEDDLE_SIZE"];
                                    txtSPI.Text = dic["STITCH_MARGIN"];
                                    txtStitchMargin.Text = dic["STITCH_MARGIN_MM"];
                                    txtTwoRowStitchMargin.Text = dic["TWO_STITCH_MARGIN"];
                                    txtThreadType.Text = dic["THREAD_TYPE"];
                                }
                            }
                        }
                    }
                }
                else
                {
                    MessageBox.Show("There is no spec data. Please check worksheet Template.");
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
        /// 코멘트 정보를 템플릿 마스터에서 가져와서 컨트롤에 바인딩 한다.
        /// </summary>
        /// <returns></returns>
        private bool BindComments(string keyValue, string keyType)
        {
            try
            {
                DataTable dataSource = GetTemplateDataSource("Comments", keyValue, keyType);

                if (dataSource.Rows.Count > 0)
                {
                    txtComment1.Text = dataSource.Rows[0]["WS_COMMENT"].ToString().Trim();
                    txtComment2.Text = dataSource.Rows[1]["WS_COMMENT"].ToString().Trim();
                    txtComment3.Text = dataSource.Rows[2]["WS_COMMENT"].ToString().Trim();
                    txtComment4.Text = dataSource.Rows[3]["WS_COMMENT"].ToString().Trim();
                    txtComment5.Text = dataSource.Rows[4]["WS_COMMENT"].ToString().Trim();
                    txtComment6.Text = dataSource.Rows[5]["WS_COMMENT"].ToString().Trim();
                    txtComment7.Text = dataSource.Rows[6]["WS_COMMENT"].ToString().Trim();

                    return true;
                }
                else
                {
                    MessageBox.Show("There is no comments data. Please check worksheet Template.");
                    return false;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
                return false;
            }
        }

        /// <summary>
        /// 공정 정보를 템플릿 마스터에서 가져와서 그리드에 바인딩 한다.
        /// </summary>
        /// <returns></returns>
        private bool BindOperation(string keyValue, string keyType)
        {
            try
            {
                DataTable dataSource = GetTemplateDataSource("Operation", keyValue, keyType);

                if (dataSource.Rows.Count > 0)
                {
                    DataTable originalDataSource = (DataTable)grdOperation.DataSource;
                    DataTable copiedDataSource = originalDataSource.Clone();

                    grdOperation.DataSource = null;

                    foreach (DataRow row in dataSource.Rows)
                    {
                        DataRow dr = copiedDataSource.NewRow();

                        // 삽입할 신규 행의 컬럼의 데이터를 모두 입력
                        foreach (DataColumn column in originalDataSource.Columns)
                            dr[column.ColumnName] = row[column.ColumnName].ToString().Trim();

                        // 공정 체크 여부 확인
                        string op_cd = String.IsNullOrEmpty(Convert.ToString(dr["OP_CHK"])) ? "N" : Convert.ToString(dr["OP_CHK"]).Trim().ToUpper();

                        if (op_cd.Equals("Y"))
                            dr["OP_QTY"] = txtWSQty.Text.ToString();    // 체크된 공정은 샘플 작업 수량 만큼 공정 수량 입력
                        else
                            dr["OP_QTY"] = "0";

                        // 데이터 소스에 신규 행 삽입
                        copiedDataSource.Rows.Add(dr);
                    }

                    copiedDataSource.AcceptChanges();
                    grdOperation.DataSource = copiedDataSource;

                    return true;
                }
                else
                {
                    MessageBox.Show("There is no comments data. Please check worksheet Template.");
                    return false;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
                return false;
            }
        }

        /// <summary>
        /// 워크시트 템플릿 정보를 데이터베이스에서 가져온다.
        /// </summary>
        /// <param name="workType"></param>
        /// <returns></returns>
        private DataTable GetTemplateDataSource(string workType, string keyValue, string keyType)
        {
            try
            {
                PKG_INTG_BOM_WORKSHEET.LOAD_TEMPALTE_BY_TYPE pkgSelect = new PKG_INTG_BOM_WORKSHEET.LOAD_TEMPALTE_BY_TYPE();
                pkgSelect.ARG_WORK_TYPE = workType;
                pkgSelect.ARG_KEY_VALUE = keyValue;
                pkgSelect.ARG_KEY_TYPE = keyType;
                pkgSelect.ARG_FACTORY = Common.sessionFactory;
                //pkgSelect.ARG_WS_NO = SOURCE_BOM_INFO[1];
                pkgSelect.OUT_CURSOR = string.Empty;

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
        /// 필수 입력 값 확인
        /// </summary>
        /// <returns></returns>
        private bool CheckRequiredField()
        {
            try
            {
                if (txtDevStyleName.Text.Trim() == "")
                {
                    MessageBox.Show("Please input Dev Style Nmae.");
                    txtDevStyleName.Focus();
                    return false;
                }

                if (leDevTeam.EditValue.ToString() == "")
                {
                    MessageBox.Show("Please select Dev Team.");
                    leDevTeam.Focus();
                    return false;
                }

                if (leSeason.EditValue.ToString() == "")
                {
                    MessageBox.Show("Please select Season.");
                    leSeason.Focus();
                    return false;
                }

                if (txtSize.Text.Trim() == "")
                {
                    MessageBox.Show("Please input Sample Size");
                    txtSize.Focus();
                    return false;
                }

                if (leGender.EditValue.ToString() == "")
                {
                    MessageBox.Show("Please select Gender.");
                    leGender.Focus();
                    return false;
                }

                // At least one prod. factory must be selected.
                if (!chkVJ.Checked && !chkJJ.Checked && !chkQD.Checked && !chkRJ.Checked && !chkDS.Checked && !chkOther.Checked)
                {
                    MessageBox.Show("Please check at least one production factory.");
                    return false;
                }

                // Colorway ID is required.
                if (txtColorwayId.Text.Trim() == "")
                {
                    MessageBox.Show("Please input Colorway ID.");
                    txtColorwayId.Focus();
                    return false;
                }

                if (SOURCE_BOM_INFO[5] != "F")
                {
                    // OCF에선 아직 BOM ID는 유관부서에서 필요
                    if (txtBOMID.Text.Trim() == "")
                    {
                        MessageBox.Show("Please input BOM ID.");
                        txtBOMID.Focus();
                        return false;
                    }
                }

                if (SOURCE_BOM_INFO[5] != "F")
                {
                    // OCF에선 아직 DPA는 유관부서에서 필요
                    if (txtDPA.Text.Trim() == "")
                    {
                        MessageBox.Show("Please input DPA.");
                        txtDPA.Focus();
                        return false;
                    }
                }

                if (leSampleType.EditValue.ToString() == "")
                {
                    MessageBox.Show("Please select Sample Type.");
                    leSampleType.Focus();
                    return false;
                }

                if (leSubType.EditValue.ToString() == "")
                {
                    MessageBox.Show("Please select Sub Type.");
                    leSampleType.Focus();
                    return false;
                }

                if (txtWSQty.Text.Trim() == "")
                {
                    MessageBox.Show("Please input Production Qty.");
                    txtWSQty.Focus();
                    return false;
                }

                if (txtNikeSendQty.Text.Trim() == "")
                {
                    MessageBox.Show("Please input Nike Send Qty.");
                    txtNikeSendQty.Focus();
                    return false;
                }

                // Validate that the user has chosen at least one process.
                if ((grdOperation.DataSource as DataTable).AsEnumerable().Where(
                    x => x["OP_CHK"].ToString().Equals("Y")).Count() == 0)
                {
                    MessageBox.Show("At least one process must be checked.");
                    return false;
                }

                // 최초 Request 시 Sample ETS 확인
                if (SOURCE_BOM_INFO[4] == "N")
                {
                    if (deSampleETS.DateTime < DateTime.Now)
                    {
                        MessageBox.Show("Sample ETS can not be a past date.");
                        deSampleETS.Focus();
                        return false;
                    }
                }

                #region Sample ETS 검증

                // 최초 Request 시 Sample ETS 확인
                if (SOURCE_BOM_INFO[4] == "N")
                {
                    PKG_INTG_BOM_COMMON.SELECT_POSSIBLE_SHIP_DATE pkgSelect = new PKG_INTG_BOM_COMMON.SELECT_POSSIBLE_SHIP_DATE();
                    pkgSelect.ARG_FACTORY = SOURCE_BOM_INFO[0];
                    pkgSelect.OUT_CURSOR = string.Empty;

                    DataTable dataSource = projectBaseForm.Exe_Select_PKG(pkgSelect).Tables[0];

                    if (dataSource.Rows.Count > 0)
                    {
                        // The earliest possible date to ship.
                        string earliestDate = dataSource.Rows[0]["WORK_YMD"].ToString();

                        // Sample ETS
                        string sampleETS = ConvertDateEditValue();

                        if (Convert.ToInt32(earliestDate) > Convert.ToInt32(sampleETS))
                        {
                            MessageBox.Show("You must select a Sample ETS that is at least 8 working days from today.", "",
                                MessageBoxButtons.OK, MessageBoxIcon.Warning);

                            deSampleETS.Focus();

                            return false;
                        }
                    }

                    PKG_INTG_BOM_COMMON.SELECT_IS_HOLIDAY pkgSelectHoliday = new PKG_INTG_BOM_COMMON.SELECT_IS_HOLIDAY();
                    pkgSelectHoliday.ARG_FACTORY = SOURCE_BOM_INFO[0];
                    pkgSelectHoliday.ARG_WORK_YMD = ConvertDateEditValue();
                    pkgSelectHoliday.OUT_CURSOR = string.Empty;

                    dataSource = projectBaseForm.Exe_Select_PKG(pkgSelectHoliday).Tables[0];

                    if (dataSource.Rows.Count > 0)
                    {
                        if (dataSource.Rows[0]["HOLIDAY_YN"].ToString() == "Y")
                        {
                            MessageBox.Show("Sample ETS can not be a holiday.");

                            deSampleETS.Focus();

                            return false;
                        }
                    }
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
        /// 각 공정의 리드타임을 구함
        /// </summary>
        private void CalcOperationLeadTime()
        {
            try
            {
                for (int rowHandle = 0; rowHandle < gvwOperation.RowCount; rowHandle++)
                {
                    int leadTime = Convert.ToInt32(gvwOperation.GetRowCellValue(rowHandle, "OP_LEADTIME").ToString());
                    DateTime workingDate = deSampleETS.DateTime;

                    for (int i = 0; i < leadTime; i++)
                    {
                        workingDate = workingDate.AddDays(-1);

                        PKG_INTG_BOM_COMMON.SELECT_IS_HOLIDAY pkgSelect = new PKG_INTG_BOM_COMMON.SELECT_IS_HOLIDAY();
                        pkgSelect.ARG_FACTORY = SOURCE_BOM_INFO[0];
                        pkgSelect.ARG_WORK_YMD = workingDate.ToString("yyyyMMdd");
                        pkgSelect.OUT_CURSOR = string.Empty;

                        DataTable dataSource = projectBaseForm.Exe_Select_PKG(pkgSelect).Tables[0];

                        if (dataSource.Rows.Count > 0)
                        {
                            // 공휴일이면 하루 앞당김
                            if (dataSource.Rows[0]["HOLIDAY_YN"].ToString() == "Y")
                                i--;
                        }
                    }

                    gvwOperation.SetRowCellValue(rowHandle, "OP_YMD", workingDate.ToString("yyyyMMdd"));
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
                return;
            }
        }

        #endregion

        #region 컨트롤 이벤트

        /// <summary>
        /// 공정 체크 시, 공정별 수량 및 일정도 같이 업데이트
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void repositoryItemCheckEdit1_EditValueChanged(object sender, EventArgs e)
        {
            try
            {
                // 공정 체크 여부
                string isChecked = Convert.ToString(((CheckEdit)sender).EditValue);
                if (isChecked == "Y")
                {
                    gvwOperation.SetRowCellValue(gvwOperation.FocusedRowHandle, "OP_QTY", txtWSQty.Text);
                    gvwOperation.SetRowCellValue(gvwOperation.FocusedRowHandle, "OP_YMD", ConvertDateEditValue());
                }
                else
                    gvwOperation.SetRowCellValue(gvwOperation.FocusedRowHandle, "OP_QTY", "0");
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
                return;
            }
        }

        /// <summary>
        /// 체크된 공정의 작업 날짜를 유저가 새로 입력한 날짜로 변경
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void deSampleETS_EditValueChanged(object sender, EventArgs e)
        {
            try
            {
                for (int i = 0; i < gvwOperation.RowCount; i++)
                {
                    // 체크된 것만 새로 변경
                    if (Convert.ToString(gvwOperation.GetRowCellValue(i, "OP_CHK")) == "Y")
                        gvwOperation.SetRowCellValue(i, "OP_YMD", ConvertDateEditValue());
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
        private void txtWSQty_EditValueChanged(object sender, EventArgs e)
        {
            try
            {
                decimal sampleQty = ConvertDecimal(txtWSQty.Text);

                for (int i = 0; i < gvwOperation.RowCount; i++)
                {
                    if (Convert.ToString(gvwOperation.GetRowCellValue(i, "OP_CHK")) == "Y")
                    {
                        gvwOperation.SetRowCellValue(i, "OP_QTY", sampleQty);
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

        #region 폼 이벤트

        /// <summary>
        /// 폼 종료 시 통합 BOM 조회 화면의 BOM 헤더 정보 리프레쉬
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void WorksheetMaking_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (isCopied)
            {
                MessageBox.Show("Please save the data first.", "",
                    MessageBoxButtons.OK, MessageBoxIcon.Error, MessageBoxDefaultButton.Button1);

                e.Cancel = true;
            }

            Common.RefreshHeaderInfo(Common.viewPCXBOMManagement, ROWHANDLE);
        }

        #endregion
    }
}