using CSI.Client.ProjectBaseForm;               // ProjectBaseForm Class
using CSI.PCC.PCX.COM;                          // Common Class
using CSI.PCC.PCX.PACKAGE;                      // Package Class

using DevExpress.XtraTreeList;
using DevExpress.XtraTreeList.Nodes;
using System;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Windows.Forms;
using System.Collections.Generic;
using System.Collections;
using DevExpress.XtraEditors;
using DevExpress.XtraTreeList.ViewInfo;
using DevExpress.Utils;
using DevExpress.XtraSplashScreen;

namespace CSI.PCC.PCX
{
    public partial class NewMultiEdit : DevExpress.XtraEditors.XtraForm
    {
        public ProjectBaseForm projectBaseForm = Common.projectBaseForm;
        public string[] SOURCE_OF_PARENT;
        private Color ROOT_COLOR, FOCUS_COLOR, FONT_COLOR, BACK_COLOR;

        public NewMultiEdit()
        {
            InitializeComponent();
        }

        protected override void OnLoad(System.EventArgs e)
        {
            base.OnLoad(e);

            #region 기본 컬러 설정
            rootColorSelector.EditValue = Color.FromArgb(219, 238, 227);
            focusColorSelector.EditValue = Color.Coral;
            borderColorSelector.EditValue = Color.Black;
            fontColorSelector.EditValue = Color.Black;
            backColorSelector.EditValue = Color.White;
            #endregion

            // 트리 리스트 키 설정
            vwTreeList.KeyFieldName = "KEY_FIELD";
            vwTreeList.ParentFieldName = "PARENT_FIELD";

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

            // 트리 리스트에 데이터 바인딩
            BindDataSourceToTreeList("DataSource");
        }

        #region Control Event

        #region Color Selector
        private void rootColorSelector_EditValueChanged(object sender, EventArgs e)
        {
            ROOT_COLOR = (Color)rootColorSelector.EditValue;
            vwTreeList.Refresh();
        }
        
        private void focusColorSelector_EditValueChanged(object sender, EventArgs e)
        {
            FOCUS_COLOR = (Color)focusColorSelector.EditValue;
            vwTreeList.Appearance.FixedLine.BackColor = FOCUS_COLOR;
            vwTreeList.Refresh();
        }
        
        private void borderColorSelector_EditValueChanged(object sender, EventArgs e)
        {
            vwTreeList.Appearance.HorzLine.BackColor = (Color)borderColorSelector.EditValue;
            vwTreeList.Appearance.VertLine.BackColor = (Color)borderColorSelector.EditValue;
            vwTreeList.Refresh();
        }
        
        private void fontColorSelector_EditValueChanged(object sender, EventArgs e)
        {
            FONT_COLOR = (Color)fontColorSelector.EditValue;
            vwTreeList.Refresh();
        }
        
        private void backColorSelector_EditValueChanged(object sender, EventArgs e)
        {
            BACK_COLOR = (Color)backColorSelector.EditValue;
            vwTreeList.Refresh();
        }
        #endregion

        /// <summary>
        /// 데이터 변경 시 각 컬럼에 맞게 처리
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void CustomCellValueChanged(object sender, CellValueChangedEventArgs e)
        {
            #region 각 속성을 저장할 변수
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
            string matRisk = string.Empty;

            string pcxColorID = string.Empty;
            string pdmColorCode = string.Empty;
            string pdmColorName = string.Empty;
            #endregion

            TreeList vwTreeList = sender as TreeList;

            // 이벤트 순환을 막기위해 일시적으로 끊음
            vwTreeList.CellValueChanged -= new CellValueChangedEventHandler(CustomCellValueChanged);

            // 유저가 입력한 값
            string value = e.Value.ToString();

            // 컬럼별 분기
            if (e.Column.FieldName == "COLOR_CD")
            {
                #region PDM 컬러코드 다이렉트 입력 시, 다른 속성 자동 입력을 위해 코드 정보를 가져옴
                if (value != "")
                {
                    // 패키지 매개변수 입력
                    PKG_INTG_BOM.SELECT_FOR_DIRECT_INPUT pkgSelect = new PKG_INTG_BOM.SELECT_FOR_DIRECT_INPUT();
                    pkgSelect.ARG_TYPE = "Color";
                    pkgSelect.ARG_CODE = value;
                    pkgSelect.ARG_NAME = "";
                    pkgSelect.OUT_CURSOR = string.Empty;
                    // 패키지 호출하여 PCX Color 정보를 가져옴
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
            else if (e.Column.FieldName == "COLOR_NAME")
            {
                #region 컬러명 다이렉트 입력 시, 다른 속성 자동 입력을 위해 코드 정보를 가져옴
                if (value != "")
                {
                    // 패키지 매개변수 입력
                    PKG_INTG_BOM.SELECT_FOR_DIRECT_INPUT pkgSelect = new PKG_INTG_BOM.SELECT_FOR_DIRECT_INPUT();
                    pkgSelect.ARG_TYPE = "Color";
                    pkgSelect.ARG_CODE = "";
                    pkgSelect.ARG_NAME = value;
                    pkgSelect.OUT_CURSOR = string.Empty;
                    // 패키지 호출하여 PCX Color 정보를 가져옴
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
            else if (e.Column.FieldName == "MXSXL_NUMBER")
            {
                #region MxSxL Number 다이렉트 입력 시, 다른 속성 자동 입력을 위해 코드 정보를 가져옴
                if (value != "")
                {
                    DataTable dataSource = null;

                    #region PCC 자재 우선 검색
                    // 패키지 매개변수 입력
                    PKG_INTG_BOM.SELECT_FOR_DIRECT_INPUT pkgSelect = new PKG_INTG_BOM.SELECT_FOR_DIRECT_INPUT();
                    pkgSelect.ARG_TYPE = "PCC_ByCode";
                    pkgSelect.ARG_CODE = value;
                    pkgSelect.ARG_NAME = "";
                    pkgSelect.OUT_CURSOR = string.Empty;
                    // 패키지 호출하여 PCX Color 정보를 가져옴
                    dataSource = projectBaseForm.Exe_Select_PKG(pkgSelect).Tables[0];
                    #endregion

                    // 검색된 자재가 없을 경우
                    if (dataSource.Rows.Count == 0)
                    {
                        #region CS 대표 자재 검색
                        PKG_INTG_BOM.SELECT_FOR_DIRECT_INPUT pkgSelect2 = new PKG_INTG_BOM.SELECT_FOR_DIRECT_INPUT();
                        pkgSelect2.ARG_TYPE = "CS_ByCode";
                        pkgSelect2.ARG_CODE = value;
                        pkgSelect2.ARG_NAME = "";
                        pkgSelect2.OUT_CURSOR = string.Empty;
                        // 패키지 호출하여 PCX Color 정보를 가져옴
                        dataSource = projectBaseForm.Exe_Select_PKG(pkgSelect2).Tables[0];
                        #endregion

                        #region 조회된 자재 개수에 따라
                        if (dataSource.Rows.Count == 0)
                        {
                            mxsxlNumber = "";
                            pcxSuppMatID = "100";
                            csCode = "";
                            mcsNumber = "";
                            pcxMatID = "100";
                            pdmMatCode = "";
                            pdmMatName = "";
                            vendorName = "";
                            matRisk = "";

                            MessageBox.Show("Unregistered Material.");
                        }
                        else if (dataSource.Rows.Count == 1)
                        {
                            mxsxlNumber = dataSource.Rows[0]["MXSXL_NUMBER"].ToString();
                            mcsNumber = dataSource.Rows[0]["MCS_NUMBER"].ToString();
                            csCode = dataSource.Rows[0]["CS_CD"].ToString();
                            pdmMatCode = dataSource.Rows[0]["MAT_CD"].ToString();
                            pdmMatName = dataSource.Rows[0]["MAT_NAME"].ToString();
                            matRisk = dataSource.Rows[0]["MAT_RISK"].ToString();
                            pcxSuppMatID = "100";
                            pcxMatID = "100";
                        }
                        else if (dataSource.Rows.Count > 1)
                        {
                            object[] parameters = new object[] { 4, value, "" };

                            FindCode findForm = new FindCode(parameters);

                            if (findForm.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                            {
                                // 검색 결과
                                string[] result = (string[])findForm.FORM_RESULT;

                                if (result[0] == "PCX_Material")
                                {
                                    pdmMatCode = result[2];
                                    pdmMatName = result[3];
                                    mxsxlNumber = result[4];
                                    mcsNumber = result[5];

                                }
                                else if (result[0] == "PCC_Material")
                                {
                                    pdmMatCode = result[2];
                                    pdmMatName = result[3];
                                    mxsxlNumber = result[4];
                                    mcsNumber = result[5];
                                    csCode = result[7];
                                }
                                else if (result[0] == "CS_Material")
                                {
                                    mxsxlNumber = result[1];
                                    pdmMatCode = result[2];
                                    pdmMatName = result[3];
                                    pcxSuppMatID = "100";
                                    pcxMatID = "100";
                                }
                            }
                        }
                        #endregion
                    }
                    else if (dataSource.Rows.Count == 1)
                    {
                        mxsxlNumber = dataSource.Rows[0]["MXSXL_NUMBER"].ToString();
                        pcxSuppMatID = dataSource.Rows[0]["PCX_SUPP_MTL_NUMBER"].ToString();
                        csCode = dataSource.Rows[0]["CS_CD"].ToString();
                        mcsNumber = dataSource.Rows[0]["MCS_NUMBER"].ToString();
                        pcxMatID = dataSource.Rows[0]["PCX_MTL_NUMBER"].ToString();
                        pdmMatCode = dataSource.Rows[0]["MAT_CD"].ToString();
                        pdmMatName = dataSource.Rows[0]["MAT_NAME"].ToString();
                        vendorName = dataSource.Rows[0]["VENDOR_NAME"].ToString();
                        matRisk = dataSource.Rows[0]["MAT_RISK"].ToString();
                    }
                    else if (dataSource.Rows.Count > 1)
                    {
                        object[] parameters = new object[] { 3, value, "" };

                        FindCode findForm = new FindCode(parameters);

                        if (findForm.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                        {
                            // 검색 결과
                            string[] result = (string[])findForm.FORM_RESULT;

                            if (result[0] == "PCX_Material")
                            {
                                mxsxlNumber = result[4];
                                pcxSuppMatID = result[8];
                                csCode = result[7];
                                mcsNumber = result[5];
                                pcxMatID = result[1];
                                pdmMatCode = result[2];
                                pdmMatName = result[3];
                                vendorName = result[6];

                            }
                            else if (result[0] == "PCC_Material")
                            {
                                mxsxlNumber = result[4];
                                pcxSuppMatID = result[8];
                                csCode = result[7];
                                mcsNumber = result[5];
                                pcxMatID = result[1];
                                pdmMatCode = result[2];
                                pdmMatName = result[3];
                                vendorName = result[6];
                            }
                            else if (result[0] == "CS_Material")
                            {
                                mxsxlNumber = result[1];
                                pdmMatCode = result[2];
                                pdmMatName = result[3];
                                csCode = "CS";
                                pcxSuppMatID = "100";
                                pcxMatID = "100";
                            }
                        }
                    }
                }
                #endregion
            }
            else if (e.Column.FieldName == "MAT_NAME")
            {
                #region 자재명 다이렉트 입력 시, 다른 속성 자동 입력을 위해 코드 정보를 가져옴
                if (value != "")
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
                        #endregion
                    }
                    else
                    {
                        DataTable dataSource = null;

                        #region PCC 자재 우선 검색
                        // 패키지 매개변수 입력
                        PKG_INTG_BOM.SELECT_FOR_DIRECT_INPUT pkgSelect = new PKG_INTG_BOM.SELECT_FOR_DIRECT_INPUT();
                        pkgSelect.ARG_TYPE = "PCC_ByName";
                        pkgSelect.ARG_CODE = "";
                        pkgSelect.ARG_NAME = value;
                        pkgSelect.OUT_CURSOR = string.Empty;
                        // 패키지 호출하여 PCX Color 정보를 가져옴
                        dataSource = projectBaseForm.Exe_Select_PKG(pkgSelect).Tables[0];
                        #endregion

                        // 검색된 자재가 없을 경우
                        if (dataSource.Rows.Count == 0)
                        {
                            #region CS 대표 자재 검색
                            PKG_INTG_BOM.SELECT_FOR_DIRECT_INPUT pkgSelect2 = new PKG_INTG_BOM.SELECT_FOR_DIRECT_INPUT();
                            pkgSelect2.ARG_TYPE = "CS_ByName";
                            pkgSelect2.ARG_CODE = "";
                            pkgSelect2.ARG_NAME = value;
                            pkgSelect2.OUT_CURSOR = string.Empty;
                            // 패키지 호출하여 PCX Color 정보를 가져옴
                            dataSource = projectBaseForm.Exe_Select_PKG(pkgSelect2).Tables[0];
                            #endregion

                            #region 조회된 자재 개수에 따라
                            if (dataSource.Rows.Count == 0)
                            {
                                mxsxlNumber = "";
                                pcxSuppMatID = "100";
                                csCode = "";
                                mcsNumber = "";
                                pcxMatID = "100";
                                pdmMatCode = "";
                                pdmMatName = "";
                                vendorName = "";
                                matRisk = "";

                                MessageBox.Show("Unregistered Material.");
                            }
                            else if (dataSource.Rows.Count == 1)
                            {
                                mxsxlNumber = dataSource.Rows[0]["MXSXL_NUMBER"].ToString();
                                mcsNumber = dataSource.Rows[0]["MCS_NUMBER"].ToString();
                                csCode = dataSource.Rows[0]["CS_CD"].ToString();
                                pdmMatCode = dataSource.Rows[0]["MAT_CD"].ToString();
                                pdmMatName = dataSource.Rows[0]["MAT_NAME"].ToString();
                                matRisk = dataSource.Rows[0]["MAT_RISK"].ToString();
                                pcxSuppMatID = "100";
                                pcxMatID = "100";
                            }
                            else if (dataSource.Rows.Count > 1)
                            {
                                object[] parameters = new object[] { 4, value, "" };

                                FindCode findForm = new FindCode(parameters);

                                if (findForm.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                                {
                                    // 검색 결과
                                    string[] result = (string[])findForm.FORM_RESULT;

                                    if (result[0] == "PCX_Material")
                                    {
                                        pdmMatCode = result[2];
                                        pdmMatName = result[3];
                                        mxsxlNumber = result[4];
                                        mcsNumber = result[5];

                                    }
                                    else if (result[0] == "PCC_Material")
                                    {
                                        pdmMatCode = result[2];
                                        pdmMatName = result[3];
                                        mxsxlNumber = result[4];
                                        mcsNumber = result[5];
                                        csCode = result[7];
                                    }
                                    else if (result[0] == "CS_Material")
                                    {
                                        mxsxlNumber = result[1];
                                        pdmMatCode = result[2];
                                        pdmMatName = result[3];
                                        pcxSuppMatID = "100";
                                        pcxMatID = "100";
                                    }
                                }
                            }
                            #endregion
                        }
                        else if (dataSource.Rows.Count == 1)
                        {
                            mxsxlNumber = dataSource.Rows[0]["MXSXL_NUMBER"].ToString();
                            pcxSuppMatID = dataSource.Rows[0]["PCX_SUPP_MTL_NUMBER"].ToString();
                            csCode = dataSource.Rows[0]["CS_CD"].ToString();
                            mcsNumber = dataSource.Rows[0]["MCS_NUMBER"].ToString();
                            pcxMatID = dataSource.Rows[0]["PCX_MTL_NUMBER"].ToString();
                            pdmMatCode = dataSource.Rows[0]["MAT_CD"].ToString();
                            pdmMatName = dataSource.Rows[0]["MAT_NAME"].ToString();
                            vendorName = dataSource.Rows[0]["VENDOR_NAME"].ToString();
                            matRisk = dataSource.Rows[0]["MAT_RISK"].ToString();
                        }
                        else if (dataSource.Rows.Count > 1)
                        {
                            object[] parameters = new object[] { 3, value, "" };

                            FindCode findForm = new FindCode(parameters);

                            if (findForm.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                            {
                                // 검색 결과
                                string[] result = (string[])findForm.FORM_RESULT;

                                if (result[0] == "PCX_Material")
                                {
                                    mxsxlNumber = result[4];
                                    pcxSuppMatID = result[8];
                                    csCode = result[7];
                                    mcsNumber = result[5];
                                    pcxMatID = result[1];
                                    pdmMatCode = result[2];
                                    pdmMatName = result[3];
                                    vendorName = result[6];
                                }
                                else if (result[0] == "PCC_Material")
                                {
                                    mxsxlNumber = result[4];
                                    pcxSuppMatID = result[8];
                                    csCode = result[7];
                                    mcsNumber = result[5];
                                    pcxMatID = result[1];
                                    pdmMatCode = result[2];
                                    pdmMatName = result[3];
                                    vendorName = result[6];
                                }
                                else if (result[0] == "CS_Material")
                                {
                                    mxsxlNumber = result[1];
                                    pdmMatCode = result[2];
                                    pdmMatName = result[3];
                                    csCode = "CS";
                                    pcxSuppMatID = "100";
                                    pcxMatID = "100";
                                }
                            }
                        }
                    }
                }
                #endregion
            }

            // 선택된 노드의 개수만큼 반복
            foreach (TreeListNode node in vwTreeList.Selection)
            {
                // Root Node는 건너띔
                string parentField = node.GetValue("PARENT_FIELD").ToString();

                if (parentField == "0") continue;
                else
                {
                    string rowStatus = node.GetValue("ROW_STATUS").ToString();

                    if (rowStatus != "D")
                    {
                        if (e.Column.FieldName == "COLOR_CD")
                        {
                            #region COLOR_CD
                            node.SetValue("PCX_COLOR_ID", pcxColorID);
                            node.SetValue("COLOR_CD", pdmColorCode);
                            node.SetValue("COLOR_NAME", pdmColorName);
                            #endregion
                        }
                        if (e.Column.FieldName == "COLOR_NAME")
                        {
                            #region COLOR_NAME
                            node.SetValue("PCX_COLOR_ID", pcxColorID);
                            node.SetValue("COLOR_CD", pdmColorCode);
                            node.SetValue("COLOR_NAME", pdmColorName);
                            #endregion
                        }
                        if (e.Column.FieldName == "MXSXL_NUMBER")
                        {
                            #region MXSXL_NUMBER
                            // 스티커 자동 체크
                            if (mxsxlNumber == "229749.522.3")
                                node.SetValue("STICKER_YN", "Y");

                            node.SetValue("MXSXL_NUMBER", mxsxlNumber);
                            node.SetValue("PCX_SUPP_MAT_ID", (pcxSuppMatID == "") ? "100" : pcxSuppMatID);
                            node.SetValue("CS_CD", csCode);
                            node.SetValue("MCS_NUMBER", mcsNumber);
                            node.SetValue("PCX_MAT_ID", (pcxMatID == "") ? "100" : pcxMatID);
                            node.SetValue("MAT_CD", pdmMatCode);
                            node.SetValue("MAT_NAME", (pdmMatName == "") ? "PLACEHOLDER" : pdmMatName);
                            node.SetValue("VENDOR_NAME", vendorName);
                            #endregion
                        }
                        if (e.Column.FieldName == "MAT_NAME")
                        {
                            #region MAT_NAME
                            // 스티커 자동 체크
                            if (mxsxlNumber == "229749.522.3")
                                node.SetValue("STICKER_YN", "Y");

                            node.SetValue("MXSXL_NUMBER", mxsxlNumber);
                            node.SetValue("PCX_SUPP_MAT_ID", (pcxSuppMatID == "") ? "100" : pcxSuppMatID);
                            node.SetValue("CS_CD", csCode);
                            node.SetValue("MCS_NUMBER", mcsNumber);
                            node.SetValue("PCX_MAT_ID", (pcxMatID == "") ? "100" : pcxMatID);
                            node.SetValue("MAT_CD", pdmMatCode);
                            node.SetValue("MAT_NAME", (pdmMatName == "") ? "PLACEHOLDER" : pdmMatName);
                            node.SetValue("VENDOR_NAME", vendorName);
                            #endregion
                        }
                        else if (e.Column.FieldName == "MAT_COMMENT")
                        {
                            node.SetValue("MAT_COMMENT", value);
                        }
                        else if (e.Column.FieldName == "COLOR_COMMENT")
                        {
                            node.SetValue("COLOR_COMMENT", value);
                        }
                        else if (e.Column.FieldName == "PROCESS")
                        {
                            node.SetValue("PROCESS", value);
                        }
                        else if (e.Column.FieldName == "REMARKS")
                        {
                            node.SetValue("REMARKS", value);
                        }
                        else if (e.Column.FieldName == "PART_TYPE")
                        {
                            node.SetValue("PART_TYPE", value);
                        }
                        else if (e.Column.FieldName == "BTTM")
                        {
                            node.SetValue("BTTM", value);
                        }
                        else if (e.Column.FieldName == "PART_NIKE_COMMENT")
                        {
                            node.SetValue("PART_NIKE_COMMENT", value);
                        }
                    }

                    // 인디케이터를 "U"로 변경, 신규 행과 삭제할 행은 제외
                    if (rowStatus != "I" && rowStatus != "D")
                        node.SetValue("ROW_STATUS", "U");
                }
            }

            vwTreeList.CellValueChanged += new CellValueChangedEventHandler(CustomCellValueChanged);
        }

        /// <summary>
        /// TreeList Cell Style 적용
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void CustomNodeCellStyle(object sender, GetCustomNodeCellStyleEventArgs e)
        {
            string parentField = e.Node.GetValue("PARENT_FIELD").ToString();
            bool isRoot = parentField == "0" ? true : false;
            if (isRoot)
            {
                /* 루트 노드 */
                e.Appearance.Font = new Font(this.Font, FontStyle.Bold);

                if (e.Node.Selected)
                {
                    e.Appearance.BackColor = FOCUS_COLOR;
                    e.Appearance.ForeColor = Color.White;
                }
                else
                {
                    bool isDeleted = true;
                    // 모든 자식 노드의 상태값을 탐색하여 삭제 표시를 할 것인지 확인
                    foreach (TreeListNode childNode in e.Node.Nodes)
                    {
                        string rowStatus = childNode.GetValue("ROW_STATUS").ToString();
                        if (rowStatus != "D")
                            isDeleted = false;
                    }

                    if (isDeleted)
                    {
                        e.Appearance.BackColor = Color.DarkGray;
                        e.Appearance.ForeColor = Color.White;
                    }
                    else
                    {
                        e.Appearance.BackColor = ROOT_COLOR;
                        e.Appearance.ForeColor = FONT_COLOR;
                    }
                }
            }
            else
            {
                /* 자식 노드 */
                if (e.Node.Selected)
                {
                    e.Appearance.BackColor = FOCUS_COLOR;
                    e.Appearance.ForeColor = Color.White;
                }
                else
                {
                    string rowStatus = e.Node.GetValue("ROW_STATUS").ToString();
                    if (rowStatus == "D")
                    {
                        e.Appearance.BackColor = Color.DarkGray;
                        e.Appearance.ForeColor = Color.White;
                    }
                    else
                    {
                        string materialName = e.Node.GetValue("MAT_NAME").ToString();
                        if (materialName.ToUpper() == "LAMINATION - HA-710S")
                        {
                            e.Appearance.ForeColor = Color.DarkOrange;
                            e.Appearance.Font = new Font(e.Appearance.Font, FontStyle.Bold);
                        }
                        else if (materialName.ToUpper() == "GENERIC SCREENED_TRANSFER LOGO")
                        {
                            e.Appearance.ForeColor = Color.DeepSkyBlue;
                            e.Appearance.Font = new Font(e.Appearance.Font, FontStyle.Bold);
                        }
                        else
                            e.Appearance.ForeColor = FONT_COLOR;
                        
                        e.Appearance.BackColor = BACK_COLOR;
                    }
                }
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void CustomShowingEditor(object sender, CancelEventArgs e)
        {
            TreeList vwTreeList = sender as TreeList;

            string fieldName = vwTreeList.FocusedColumn.FieldName;

            if (fieldName == "PART_TYPE" || fieldName == "COMBINE_YN" || fieldName == "STICKER_YN"
                || fieldName == "MDSL_CHK" || fieldName == "OTSL_CHK" || fieldName == "BTTM" || fieldName == "PART_NIKE_COMMENT"
                || fieldName == "MXSXL_NUMBER" || fieldName == "MAT_NAME" || fieldName == "MAT_COMMENT"
                || fieldName == "COLOR_CD" || fieldName == "COLOR_NAME" || fieldName == "COLOR_COMMENT"
                || fieldName == "REMARKS" || fieldName == "PROCESS")
            {
                // 루트 노드의 경우 수정 불가
                string parentField = vwTreeList.FocusedNode.GetValue("PARENT_FIELD").ToString();
                if (parentField == "0")
                    e.Cancel = true;
                else
                {
                    string rowStatus = vwTreeList.FocusedNode.GetValue("ROW_STATUS").ToString();
                    if (rowStatus == "D")
                        e.Cancel = true;
                }
            }
        }

        /// <summary>
        /// 삽입, 수정, 삭제에 대한 인디케이터 표기
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void CustomDrawNodeIndicator(object sender, CustomDrawNodeIndicatorEventArgs e)
        {
            e.Appearance.Font = new Font("SimSun", 11, FontStyle.Bold);
            e.Appearance.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;

            Color borderColor = Color.FromArgb(132, 157, 189);  // Office 2010 Border Color
            Color backColor = Color.FromArgb(232, 242, 251);    // Office 2010 Back Color

            e.Cache.FillRectangle(backColor, e.Bounds);
            // Fill 먼저 수행 후 Draw 해야 Border Color 표기 됨
            Pen pen = new Pen(borderColor);
            e.Cache.DrawRectangle(pen, e.Bounds);

            string rowStatus = e.Node.GetValue("ROW_STATUS").ToString();
            if (rowStatus == "U")
            {
                e.Appearance.ForeColor = Color.Blue;
                e.Appearance.DrawString(e.Cache, "U", e.Bounds);
            }
            else if (rowStatus == "I")
            {
                e.Appearance.ForeColor = Color.Green;
                e.Appearance.DrawString(e.Cache, "I", e.Bounds);
            }
            else if (rowStatus == "D")
            {
                e.Appearance.ForeColor = Color.DimGray;
                e.Appearance.DrawString(e.Cache, "D", e.Bounds);
            }

            e.Handled = true;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void CustomMouseDown(object sender, MouseEventArgs e)
        {
            TreeList vwTreeList = sender as TreeList;

            vwTreeList.CellValueChanged -= new CellValueChangedEventHandler(CustomCellValueChanged);

            TreeListHitInfo hitInfo = vwTreeList.CalcHitInfo(e.Location);

            if (hitInfo.HitInfoType == HitInfoType.Cell)
            {
                if (hitInfo.Column.ColumnEdit is DevExpress.XtraEditors.Repository.RepositoryItemCheckEdit)
                {
                    #region CheckEdit을 클릭한 경우

                    // 루트 노드의 체크 에딧을 클릭한 경우 모든 자식 노드의 체크 에딧 상태값 변경
                    string parentField = hitInfo.Node.GetValue("PARENT_FIELD").ToString();

                    if (parentField == "0")
                    {
                        foreach (TreeListNode node in hitInfo.Node.Nodes)
                        {
                            string value = node.GetValue(hitInfo.Column.FieldName).ToString();
                            if (value == "Y")
                                node.SetValue(hitInfo.Column.FieldName, "N");
                            else
                                node.SetValue(hitInfo.Column.FieldName, "Y");

                            string rowStatus = node.GetValue("ROW_STATUS").ToString();
                            if (rowStatus != "I" && rowStatus != "D")
                                node.SetValue("ROW_STATUS", "U");
                        }
                    }
                    else
                    {
                        vwTreeList.Selection.Clear();
                        vwTreeList.FocusedColumn = hitInfo.Column;
                        vwTreeList.FocusedNode = hitInfo.Node;
                        vwTreeList.ShowEditor();

                        CheckEdit edit = vwTreeList.ActiveEditor as CheckEdit;

                        if (edit == null)
                            return;

                        string rowStatus = vwTreeList.FocusedNode.GetValue("ROW_STATUS").ToString();

                        if (rowStatus != "D")
                        {
                            edit.Toggle();

                            if (rowStatus != "I" && rowStatus != "D")
                                vwTreeList.FocusedNode.SetValue("ROW_STATUS", "U");
                        }
                        DXMouseEventArgs.GetMouseArgs(e).Handled = true;
                    }
                    #endregion
                }
                else if (e.Button != System.Windows.Forms.MouseButtons.Right)
                {
                    if (vwTreeList.Selection.Contains(hitInfo.Node) && vwTreeList.Selection.Count > 1)
                        vwTreeList.Selection.Clear();
                }
            }

            vwTreeList.CellValueChanged += new CellValueChangedEventHandler(CustomCellValueChanged);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void CustomMouseMove(object sender, MouseEventArgs e)
        {
            TreeList vwTreeList = sender as TreeList;

            if (e.Button == System.Windows.Forms.MouseButtons.Left)
            {
                TreeListHitInfo hitInfo = vwTreeList.CalcHitInfo(e.Location);
                if (hitInfo.HitInfoType == HitInfoType.Cell)
                {
                    if (vwTreeList.Selection.Contains(hitInfo.Node) == false)
                        vwTreeList.Selection.Add(hitInfo.Node);
                }
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void CustomMouseUp(object sender, MouseEventArgs e)
        {
            TreeList vwTreeList = sender as TreeList;

            if (vwTreeList.Selection.Count > 1)
            {
                TreeListHitInfo hitInfo = vwTreeList.CalcHitInfo(e.Location);
                if (hitInfo.HitInfoType == HitInfoType.Cell)
                    vwTreeList.ShowEditor();
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void CustomKeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Delete)
            {
                vwTreeList.CellValueChanged -= new CellValueChangedEventHandler(CustomCellValueChanged);

                string fcsdColumnName = vwTreeList.FocusedColumn.FieldName;
                if (fcsdColumnName == "PTRN_PART_NAME" || fcsdColumnName == "BTTM" || fcsdColumnName == "PART_NIKE_COMMENT"
                    || fcsdColumnName == "MAT_COMMENT" || fcsdColumnName == "COLOR_COMMENT" || fcsdColumnName == "PROCESS"
                    || fcsdColumnName == "REMARKS")
                {
                    foreach (TreeListNode node in vwTreeList.Selection)
                    {
                        if (node.HasChildren)
                            continue;

                        node.SetValue(fcsdColumnName, "");
                    }
                }
                vwTreeList.CellValueChanged += new CellValueChangedEventHandler(CustomCellValueChanged);
            }
        }

        /// <summary>
        /// BOM 헤더 변경이 있을 경우 헤더 정보 리프레쉬
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void CustomFormClosing(object sender, FormClosingEventArgs e)
        {
            projectBaseForm.QueryClick();
        }
        #endregion

        #region Context Menu event
        /// <summary>
        /// Context Menu Event 처리
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ContextMenuItem_Click(object sender, EventArgs e)
        {
            System.Windows.Forms.ToolStripMenuItem menuItem = sender as System.Windows.Forms.ToolStripMenuItem;

            switch (menuItem.Name)
            {
                case "addPart":
                    AddNewPart();
                    break;

                case "addLine":
                    AddNewLine();
                    break;

                case "addAll":
                    AddNewLineAtOnce();
                    break;

                case "findCode":
                    FindCodeFromLibrary();
                    break;
                    
                case "findProcess":
                    FindProcessFromMaster();
                    break;

                case "deleteNode":
                    DeleteNode();
                    break;

                case "rowUp":
                    RowUp();
                    break;

                case "rowDown":
                    RowDown();
                    break;

                case "expandAll":
                    vwTreeList.ExpandAll();
                    vwTreeList.MoveNext();
                    break;

                case "collapseAll":
                    vwTreeList.CollapseAll();
                    vwTreeList.MoveNext();
                    break;

                case "setPtrnTop":
                    SetPtrnPartName("Top");
                    break;

                case "setPtrnEach":
                    SetPtrnPartName("Each");
                    break;

                case "matInfo":
                    ShowMaterialInfomation();
                    break;

                case "mdmConnect":
                    ConnectToMdm();
                    break;

                default:
                    break;
            }
        }

        /// <summary>
        /// 필요한 코드 정보를 검색한다.
        /// </summary>
        private void FindCodeFromLibrary()
        {
            foreach (TreeListNode node in vwTreeList.Selection)
            {
                string rowStatus = node.GetValue("ROW_STATUS").ToString();
                
                if (rowStatus == "D")
                {
                    MessageBox.Show("Contains rows to be deleted.");
                    return;
                }
            }
            
            // 포커스하고 있는 컬럼명
            string fieldName = vwTreeList.FocusedColumn.FieldName;
            string keyword = vwTreeList.FocusedNode.GetValue(fieldName).ToString();
            string partDelimiter = string.Empty;

            int initialType = 0;

            if (fieldName == "PART_NAME" || fieldName == "PTRN_PART_NAME" || fieldName == "PART_TYPE" || fieldName == "PART_CD")
            {
                // 파트
                initialType = 0;

                // 패턴 파트 검색의 경우 구분자 입력
                if (fieldName == "PTRN_PART_NAME")
                    partDelimiter = "Pattern";
            }
            else if (fieldName == "MXSXL_NUMBER" || fieldName == "PCX_SUPP_MAT_ID" || fieldName == "MCS_NUMBER"
                || fieldName == "PCX_MAT_ID" || fieldName == "MAT_CD" || fieldName == "MAT_NAME"
                || fieldName == "MAT_COMMENT" || fieldName == "VENDOR_NAME")
            {
                // 자재
                
                string csCode = vwTreeList.FocusedNode.GetValue("CS_CD").ToString();
                if (csCode == "CS")
                    initialType = 4;
                else
                    initialType = 3;
            }
            else if (fieldName == "PCX_COLOR_ID" || fieldName == "COLOR_CD" || fieldName == "COLOR_NAME")
            {
                // 컬러
                initialType = 2;
            }
            else if (fieldName == "CS_CD")
            {
                // CS# 자재 선택 시
                initialType = 3;
            }

            object[] parameters = new object[] { initialType, keyword, partDelimiter };

            FindCode findForm = new FindCode(parameters);

            if (findForm.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                vwTreeList.CellValueChanged -= new CellValueChangedEventHandler(CustomCellValueChanged);
                
                // 검색 결과
                string[] result = (string[])findForm.FORM_RESULT;
                
                // 선택한 행의 개수만큼 반복
                foreach (TreeListNode node in vwTreeList.Selection)
                {
                    // 부모 노드는 업데이트 안 함
                    string parentField = node.GetValue("PARENT_FIELD").ToString();
                    if (parentField == "0")
                        continue;

                    // 각 타입에 맞게 그리드에 적용
                    if (result[0] == "Part")
                    {
                        //node.SetValue("PART_NAME", result[1]);
                        //node.SetValue("PART_TYPE", result[2]);
                        //node.SetValue("PART_CD", result[3]);
                        //node.SetValue("MDSL_CHK", (result[2] == "MIDSOLE" || result[2] == "AIRBAG") ? "Y" : "N");
                        //node.SetValue("OTSL_CHK", (result[2] == "OUTSOLE") ? "Y" : "N");
                        //node.SetValue("PCX_PART_ID", result[4]);

                        MessageBox.Show("You can't change the part name.");
                        return;
                    }
                    else if (result[0] == "PatternPart")
                    {
                        node.SetValue("PTRN_PART_NAME", result[1]);
                        node.SetValue("PTRN_PART_CD", result[3]);
                    }
                    else if (result[0] == "PCX_Material")
                    {
                        // 스티커 자동 체크
                        if (result[4] == "229749.522.3")
                            node.SetValue("STICKER_YN", "Y");

                        node.SetValue("PCX_MAT_ID", result[1]);
                        node.SetValue("MAT_CD", result[2]);
                        node.SetValue("MAT_NAME", result[3]);
                        node.SetValue("MXSXL_NUMBER", result[4]);
                        node.SetValue("MCS_NUMBER", result[5]);
                        node.SetValue("VENDOR_NAME", result[6]);
                        node.SetValue("CS_CD", result[7]);
                        node.SetValue("PCX_SUPP_MAT_ID", result[8]);
                    }
                    else if (result[0] == "Color")
                    {
                        node.SetValue("PCX_COLOR_ID", result[1]);
                        node.SetValue("COLOR_CD", result[2]);
                        node.SetValue("COLOR_NAME", result[3]);
                    }
                    else if (result[0] == "PCC_Material")
                    {
                        // 스티커 자동 체크
                        if (result[4] == "229749.522.3")
                            node.SetValue("STICKER_YN", "Y");

                        node.SetValue("PCX_MAT_ID", result[1]);
                        node.SetValue("MAT_CD", result[2]);
                        node.SetValue("MAT_NAME", result[3]);
                        node.SetValue("MXSXL_NUMBER", result[4]);
                        node.SetValue("MCS_NUMBER", result[5]);
                        node.SetValue("VENDOR_NAME", result[6]);
                        node.SetValue("CS_CD", result[7]);
                        node.SetValue("PCX_SUPP_MAT_ID", result[8]);
                    }
                    else if (result[0] == "CS_Material")
                    {
                        node.SetValue("MXSXL_NUMBER", result[1]);
                        node.SetValue("MAT_CD", result[2]);
                        node.SetValue("MAT_NAME", result[3]);
                        node.SetValue("MCS_NUMBER", "");
                        node.SetValue("CS_CD", "CS");
                        node.SetValue("PCX_SUPP_MAT_ID", "100");
                        node.SetValue("PCX_MAT_ID", "100");
                    }
                    
                    // 선택한 행의 현재 상태 값
                    string rowStatus = node.GetValue("ROW_STATUS").ToString();
                    
                    // 신규 행은 인디케이터 변경 안 함
                    if (rowStatus != "I")
                        node.SetValue("ROW_STATUS", "U");
                }
                vwTreeList.CellValueChanged += new CellValueChangedEventHandler(CustomCellValueChanged);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        private void FindProcessFromMaster()
        {
            #region 삭제할 행이 포함되었는지 확인
            foreach (TreeListNode node in vwTreeList.Selection)
            {
                string rowStatus = node.GetValue("ROW_STATUS").ToString();
                if (rowStatus == "D")
                {
                    MessageBox.Show("Contains rows to be deleted.");
                    return;
                }
            }
            #endregion

            FindProcess findForm = new FindProcess()
            {
                EnteredProcess = vwTreeList.FocusedNode.GetValue("PROCESS").ToString().ToUpper()
            };

            if (findForm.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                // 불필요한 이벤트 반복을 피함
                vwTreeList.CellValueChanged -= new CellValueChangedEventHandler(CustomCellValueChanged);
                // 자식폼에게 전달 받은 프로세스 리스트
                List<string> listOfSelectedProcess = (List<string>)findForm.SelectedProcess;
                // 프로세스1,프로세스2,프로세스3 형태의 프로세스 나열
                string process = string.Join(",", listOfSelectedProcess.ToArray());
                // 선택한 노드 개수만큼 반복
                foreach (TreeListNode node in vwTreeList.Selection)
                {
                    // 루트 노드는 건너띔
                    if (node.HasChildren)
                        continue;

                    // 생성된 프로세스를 셀에 바인딩
                    node.SetValue("PROCESS", process);
                    // 노드의 현재 상태 값
                    string rowStatus = node.GetValue("ROW_STATUS").ToString();
                    if (rowStatus != "I")
                        node.SetValue("ROW_STATUS", "U");
                }
                // 이벤트 다시 연결
                vwTreeList.CellValueChanged += new CellValueChangedEventHandler(CustomCellValueChanged);
            }
        }

        /// <summary>
        /// 새로운 루트 노드를 추가한다.
        /// </summary>
        private void AddNewPart()
        {
            bool isClicked = CheckRootIsClicked();
            if (isClicked == false)
                return;

            #region 추가할 파트가 이미 리스트에 있는지 확인

            // 신규로 삽입할 파트명
            string targetPartName = string.Empty;

            object[] parameters = new object[] { 0, "", "" };

            FindCode form = new FindCode(parameters);

            if (form.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                string[] result = (string[])form.FORM_RESULT;
                targetPartName = result[1];

                foreach (TreeListNode node in vwTreeList.Nodes)
                {
                    // 루트 노드부터 확인
                    string partName = node.GetValue("KEY_FIELD").ToString();
                    if (targetPartName == partName)
                    {
                        MessageBox.Show("There are already same part.");
                        return;
                    }
                    // 자식 노드를 가지고 있으면 자식도 비교
                    if (node.HasChildren)
                    {
                        foreach (TreeListNode childNode in node.Nodes)
                        {
                            partName = childNode.GetValue("PART_NAME").ToString();
                            if (targetPartName == partName)
                            {
                                MessageBox.Show("There are already same part.");
                                return;
                            }
                        }
                    }
                }
            }
            #endregion

            // Find Code를 통해 검색한 파트명이 없는 경우 종료 - 검색 없이 폼을 종료한 경우
            if (targetPartName == "")
                return;

            DataTable dataSource = (DataTable)vwTreeList.DataSource;
            string key = vwTreeList.FocusedNode["KEY_FIELD"].ToString();
            // 현재 데이터 소스에서 포커싱된 부모 노드의 위치를 구함
            int insertionPosition = 0;
            foreach (DataRow row in dataSource.Rows)
            {
                if (row["KEY_FIELD"].ToString() == key)
                    break;
                else
                    insertionPosition++;
            }

            #region 신규 행 필드 값 입력
            DataRow newRow = dataSource.NewRow();
            newRow["KEY_FIELD"] = targetPartName;
            newRow["PARENT_FIELD"] = "0";
            newRow["FACTORY"] = "";
            newRow["DEV_COLORWAY_ID"] = "";
            newRow["PART_SEQ"] = 0;
            newRow["PART_NIKE_COMMENT"] = "";
            newRow["PART_CD"] = "";
            newRow["PCX_PART_ID"] = "";
            newRow["PART_NAME"] = targetPartName;
            newRow["PTRN_PART_CD"] = "";
            newRow["PTRN_PART_NAME"] = "";
            newRow["PART_TYPE"] = "";
            newRow["BTTM"] = "";
            newRow["MXSXL_NUMBER"] = "";
            newRow["CS_CD"] = "";
            newRow["MCS_NUMBER"] = "";
            newRow["PCX_SUPP_MAT_ID"] = "";
            newRow["MAT_CD"] = "";
            newRow["MAT_NAME"] = "";
            newRow["MAT_COMMENT"] = "";
            newRow["PCX_COLOR_ID"] = "";
            newRow["COLOR_CD"] = "";
            newRow["COLOR_NAME"] = "";
            newRow["SORT_NO"] = 99;
            newRow["REMARKS"] = "";
            newRow["UPD_USER"] = "";
            newRow["UPD_YMD"] = DateTime.Now;
            newRow["VENDOR_NAME"] = "";
            newRow["PCX_MAT_ID"] = "";
            newRow["PROCESS"] = "";
            newRow["ROW_STATUS"] = "I";
            newRow["PRVS_STATUS"] = "N";
            newRow["COMBINE_YN"] = "N";
            newRow["STICKER_YN"] = "N";
            newRow["MDSL_CHK"] = "N";
            newRow["OTSL_CHK"] = "N";
            #endregion

            dataSource.Rows.InsertAt(newRow, insertionPosition + 1);
        }

        /// <summary>
        /// 선택된 루트 노드에 자식 노드를 추가한다.
        /// </summary>
        private void AddNewLine()
        {
            bool isClicked = CheckRootIsClicked();
            if (isClicked == false)
                return;

            bool isInserted = CheckNodeCanBeInserted();
            if (isInserted == false)
                return;

            BOMSelection form = new BOMSelection();
            form.CONCAT_WS_NO = SOURCE_OF_PARENT[1];
            // 폼 로드 및 추가할 컬러웨이 선택
            if (form.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                // 폼에서 리턴 받은 결과값
                string[] returnValue = (string[])form.FORM_RESULT;
                // 선택한 컬러웨이의 키값
                string key = returnValue[0] + returnValue[1];
                // 자식 노드가 존재할 경우 선택한 컬러웨이의 라인 아이템이 이미 존재하는지 확인
                if (vwTreeList.FocusedNode.HasChildren)
                {
                    foreach (TreeListNode childNode in vwTreeList.FocusedNode.Nodes)
                    {
                        string childKey = childNode.GetValue("FACTORY").ToString() + childNode.GetValue("WS_NO").ToString();
                        if (key == childKey)
                        {
                            MessageBox.Show("A Line already exists for that colorway.");
                            return;
                        }
                    }
                }

                #region 루트 노드의 파트 관련 코드 정보를 가져옴
                string partCode = string.Empty;
                string partName = string.Empty;
                string partType = string.Empty;
                string pcxPartId = string.Empty;

                PKG_INTG_BOM.SELECT_FOR_DIRECT_INPUT pkgSelect = new PKG_INTG_BOM.SELECT_FOR_DIRECT_INPUT();
                pkgSelect.ARG_TYPE = "Part";
                pkgSelect.ARG_CODE = vwTreeList.FocusedNode["PART_NAME"].ToString();
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
                #endregion

                // 해당 컬러웨이의 다음 차례의 파트 시퀀스를 계산
                int partSeq = GetNextPartSeq(key);
                // factory + ws_no + part_seq
                string keyField = returnValue[0] + returnValue[1] + partSeq.ToString();
                // 새로 삽입할 신규 자식 노드에 값 입력, DB 쿼리 호출 순서와 맞아야함
                object[] newNode = new object[] {keyField, partName, returnValue[0], returnValue[1], returnValue[3], partSeq,
                "", partCode, pcxPartId, partName, "", "", partType, "", "", "", "", "", "", "", "", "", "", "", "", 99, "",
                Common.sessionID, DateTime.Now, "", "", "", "I", "N", "", "", (partType == "MIDSOLE") ? "Y" : "N",
                (partType == "OUTSOLE") ? "Y" : "N"};

                vwTreeList.AppendNode(newNode, vwTreeList.FocusedNode);
                vwTreeList.MoveNext();
            }
        }

        /// <summary>
        /// 한 번에 모든 컬러웨이의 라인 아이템을 추가한다.
        /// </summary>
        private void AddNewLineAtOnce()
        {
            bool isClicked = CheckRootIsClicked();
            if (isClicked == false)
                return;

            // 이미 자식 노드가 존재하면 실행 취소
            if (vwTreeList.FocusedNode.HasChildren)
            {
                MessageBox.Show("At least one item already exists.");
                return;
            }

            #region 루트 노드의 파트 관련 코드 정보를 가져옴
            string partCode = string.Empty;
            string partName = string.Empty;
            string partType = string.Empty;
            string pcxPartId = string.Empty;

            PKG_INTG_BOM.SELECT_FOR_DIRECT_INPUT pkgSelect = new PKG_INTG_BOM.SELECT_FOR_DIRECT_INPUT();
            pkgSelect.ARG_TYPE = "Part";
            pkgSelect.ARG_CODE = vwTreeList.FocusedNode["PART_NAME"].ToString();
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
            #endregion

            char[] separator = new char[] { ',' };
            string[] eachWsNo = SOURCE_OF_PARENT[1].Split(separator, StringSplitOptions.RemoveEmptyEntries);

            // 선택한 BOM의 개수만큼 반복
            for (int i = 0; i < Convert.ToInt32(SOURCE_OF_PARENT[2]); i++)
            {
                string key = SOURCE_OF_PARENT[0] + eachWsNo[i];
                int partSeq = GetNextPartSeq(key);
                string colorwayId = GetColorwayId(key);
                // factory + ws_no + part_seq
                string keyField = SOURCE_OF_PARENT[0] + eachWsNo[i] + partSeq.ToString();
                // 새로 삽입할 신규 자식 노드에 값 입력, DB 쿼리 호출 순서와 맞아야함
                object[] newNode = new object[] {keyField, partName, SOURCE_OF_PARENT[0], eachWsNo[i], colorwayId, partSeq,
                "", partCode, pcxPartId, partName, "", "", partType, "", "", "", "", "", "", "", "", "", "", "", "", 99, "",
                Common.sessionID, DateTime.Now, "", "", "", "I", "N", "", "", (partType == "MIDSOLE") ? "Y" : "N",
                (partType == "OUTSOLE") ? "Y" : "N"};

                vwTreeList.AppendNode(newNode, vwTreeList.FocusedNode);
            }
            vwTreeList.MoveNext();
        }

        /// <summary>
        /// 노드의 상태 값을 삭제로 변경
        /// </summary>
        private void DeleteNode()
        {
            #region 루트노드와 자식노드가 같이 선택되었는지 확인
            // 둘 이상의 노드가 선택된 경우
            if (vwTreeList.Selection.Count > 1)
            {
                foreach (TreeListNode node in vwTreeList.Selection)
                {
                    // 루트 노드가 포함되어 있으면
                    if (node.HasChildren)
                    {
                        MessageBox.Show("Please select only one root if you want to delete all rows.");
                        FocusRootNode();
                        return;
                    }
                }
            }
            #endregion

            // 이벤트 꼬임 방지
            vwTreeList.CellValueChanged -= new CellValueChangedEventHandler(CustomCellValueChanged);

            // Root 선택과 그렇지 않은 케이스로 분류
            if (vwTreeList.FocusedNode.HasChildren)
            {
                string mode = GetDeleteMode(vwTreeList.FocusedNode);
                if (mode == "Delete")
                {
                    foreach (TreeListNode childNode in vwTreeList.FocusedNode.Nodes)
                    {
                        string rowStatus = childNode.GetValue("ROW_STATUS").ToString();
                        // 이미 삭제된 노드의 경우 건너띔
                        if (rowStatus == "D")
                            continue;
                        // 현재 상태 값 저장
                        childNode.SetValue("PRVS_STATUS", rowStatus);
                        // 상태 값을 삭제로 변경
                        childNode.SetValue("ROW_STATUS", "D");
                    }
                }
                else if (mode == "Return")
                {
                    foreach (TreeListNode childNode in vwTreeList.FocusedNode.Nodes)
                    {
                        // 이전 상태 값을 불러와 현재의 상태 값 변경
                        string prvsStatus = childNode.GetValue("PRVS_STATUS").ToString();
                        childNode.SetValue("ROW_STATUS", prvsStatus);
                    }
                }
            }
            else
            {
                // 선택된 자식 노드들만 삭제
                foreach (TreeListNode node in vwTreeList.Selection)
                {
                    string rowStatus = node.GetValue("ROW_STATUS").ToString();
                    if (rowStatus != "D")
                    {
                        node.SetValue("PRVS_STATUS", rowStatus);
                        node.SetValue("ROW_STATUS", "D");
                    }
                    else
                    {
                        string prvsStatus = node.GetValue("PRVS_STATUS").ToString();
                        node.SetValue("ROW_STATUS", prvsStatus);
                    }
                }
            }

            vwTreeList.CellValueChanged += new CellValueChangedEventHandler(CustomCellValueChanged);
            vwTreeList.Refresh();
        }

        /// <summary>
        /// 모든 자식 노드를 삭제할 것인지 이전으로 되돌릴 것인지 확인
        /// Case 1. 현재 모든 자식 노드의 상태 값이 삭제 : 이전으로 되돌림
        /// Others. 모든 자식 노드의 상태 값을 삭제로 변경
        /// </summary>
        /// <param name="node"></param>
        /// <returns></returns>
        private string GetDeleteMode(TreeListNode node)
        {
            // 기본값은 되돌리기
            string mode = "Return";
            foreach (TreeListNode childNode in node.Nodes)
            {
                string rowStatus = childNode.GetValue("ROW_STATUS").ToString();
                if (rowStatus != "D")
                {
                    mode = "Delete";
                    return mode;
                }
            }
            return mode;
        }

        /// <summary>
        /// 
        /// </summary>
        private void RowUp()
        {
            // 루트 노드 선택 여부 확인
            string parentFieldValue = vwTreeList.FocusedNode["PARENT_FIELD"].ToString();
            if (parentFieldValue != "0")
            {
                MessageBox.Show("Please select root node");
                FocusRootNode();
                return;
            }
            else if (parentFieldValue == "0")
            {
                // 상위 루트 노드 존재 여부 확인
                TreeListNode previousNode = vwTreeList.FocusedNode.PrevNode;
                if (previousNode != null)
                {
                    string partName = previousNode["KEY_FIELD"].ToString();
                    // 옮길 위치
                    int insertPosition = ReturnCurrentPosition(partName);
                    partName = vwTreeList.FocusedNode["KEY_FIELD"].ToString();
                    // 선택한 루트 노드의 현재 위치
                    int deletePosition = ReturnCurrentPosition(partName);

                    DataTable dataSource = (DataTable)vwTreeList.DataSource;
                    DataRow rowToBeInserted = dataSource.NewRow();
                    DataRow rowToBeDeleted = dataSource.Rows[deletePosition];
                    // 내용 복사
                    rowToBeInserted.ItemArray = rowToBeDeleted.ItemArray;
                    // 선택한 루트 노드 삭제
                    dataSource.Rows.RemoveAt(deletePosition);
                    // 루트 노드 삭제 시 링크된 자식 노드의 PARENT_FIELD 값이 0으로 변경되므로 다시 세팅
                    foreach (DataRow dr in dataSource.Rows)
                    {
                        if (dr["PART_NAME"].ToString() == partName)
                        {
                            dr["PARENT_FIELD"] = partName;
                        }
                    }
                    // 옮길 위치에 선택한 루트 노드 새로 삽입
                    dataSource.Rows.InsertAt(rowToBeInserted, insertPosition);
                    // 선택한 루트 노드 다시 포커싱
                    vwTreeList.FocusedNode = vwTreeList.FindNodeByKeyID(partName);
                }
            }
        }

        /// <summary>
        /// 
        /// </summary>
        private void RowDown()
        {
            // 루트 노드 선택 여부 확인
            string parentFieldValue = vwTreeList.FocusedNode["PARENT_FIELD"].ToString();
            if (parentFieldValue != "0")
            {
                MessageBox.Show("Please select root node");
                FocusRootNode();
                return;
            }
            else if (parentFieldValue == "0")
            {
                // 하위 루트 노드 존재 여부 확인
                TreeListNode nextNode = vwTreeList.FocusedNode.NextNode;
                if (nextNode != null)
                {
                    string partName = nextNode["KEY_FIELD"].ToString();
                    // 옮길 위치
                    int insertPosition = ReturnCurrentPosition(partName);
                    partName = vwTreeList.FocusedNode["KEY_FIELD"].ToString();
                    // 선택한 루트 노드의 현재 위치
                    int deletePosition = ReturnCurrentPosition(partName);

                    DataTable dataSource = (DataTable)vwTreeList.DataSource;
                    DataRow rowToBeInserted = dataSource.NewRow();
                    DataRow rowToBeDeleted = dataSource.Rows[deletePosition];
                    // 내용 복사
                    rowToBeInserted.ItemArray = rowToBeDeleted.ItemArray;
                    // 선택한 루트 노드 삭제
                    dataSource.Rows.RemoveAt(deletePosition);
                    // 루트 노드 삭제 시 링크된 자식 노드의 PARENT_FIELD 값이 0으로 변경되므로 다시 세팅
                    foreach (DataRow dr in dataSource.Rows)
                    {
                        if (dr["PART_NAME"].ToString() == partName)
                        {
                            dr["PARENT_FIELD"] = partName;
                        }
                    }
                    // 옮길 위치에 선택한 루트 노드 새로 삽입
                    dataSource.Rows.InsertAt(rowToBeInserted, insertPosition);
                    // 선택한 루트 노드 다시 포커싱
                    vwTreeList.FocusedNode = vwTreeList.FindNodeByKeyID(partName);
                }
            }
        }

        /// <summary>
        /// TreeList의 데이터소스에서 매개변수의 파트명과 일치하는 행의 위치를 찾아서 리턴
        /// </summary>
        /// <param name="partName"></param>
        /// <returns></returns>
        private int ReturnCurrentPosition(string partName)
        {
            DataTable dataSource = (DataTable)vwTreeList.DataSource;

            int rowNum = 0;
            foreach (DataRow dr in dataSource.Rows)
            {
                if (dr["KEY_FIELD"].ToString() == partName)
                    break;
                else
                    rowNum++;
            }
            return rowNum;
        }

        /// <summary>
        /// 루트 노드 선택 여부를 확인한다.
        /// </summary>
        /// <returns></returns>
        private bool CheckRootIsClicked()
        {
            // 하나의 행만 선택하였는지 확인
            if (vwTreeList.Selection.Count > 1)
            {
                MessageBox.Show("Please select only one parent row.");
                FocusRootNode();
                return false;
            }
            else
            {
                // 부모 노드를 선택하였는지 확인
                string parentField = vwTreeList.FocusedNode.GetValue("PARENT_FIELD").ToString();
                if (parentField != "0")
                {
                    MessageBox.Show("Please select only one parent row.");
                    FocusRootNode();
                    return false;
                }
            }

            return true;
        }

        /// <summary>
        /// 포커스된 노드의 루트 노드를 포커싱한다.
        /// </summary>
        private void FocusRootNode()
        {
            TreeListNode parentNode = vwTreeList.FocusedNode.ParentNode;

            vwTreeList.Selection.Clear();
            vwTreeList.FocusedNode = parentNode;
            vwTreeList.Selection.Add(vwTreeList.FocusedNode);
        }

        /// <summary>
        /// 선택된 루트 노드에 새로운 자식 노드를 추가할 수 있는지 확인
        /// (컬러웨이별 해당 파트에 하나의 파트만 존재해야함 - 파트 중복 방지)
        /// </summary>
        private bool CheckNodeCanBeInserted()
        {
            int numOfChildNode = vwTreeList.FocusedNode.Nodes.Count;
            int numOfSelectedBOM = Convert.ToInt32(SOURCE_OF_PARENT[2]);

            if (numOfChildNode == numOfSelectedBOM)
            {
                MessageBox.Show("There is a line item of all colorways.");
                return false;
            }

            return true;
        }

        /// <summary>
        /// 신규 노드 삽입 시 BOM의 다음 Seq.를 구함
        /// </summary>
        /// <returns></returns>
        private int GetNextPartSeq(string key)
        {
            int nextPartSeq = 1;
            foreach (TreeListNode rootNode in vwTreeList.Nodes)
            {
                if (rootNode.HasChildren)
                {
                    foreach (TreeListNode childNode in rootNode.Nodes)
                    {
                        string childKey = childNode.GetValue("FACTORY").ToString() + childNode.GetValue("WS_NO").ToString();
                        if (key == childKey)
                        {
                            int partSeq = Convert.ToInt32(childNode.GetValue("PART_SEQ"));
                            if (partSeq > nextPartSeq)
                            {
                                nextPartSeq = partSeq;
                            }
                        }
                    }
                }
            }
            return nextPartSeq + 1;
        }

        /// <summary>
        /// 트리 리스트에서 키 값으로 컬러웨이 아이디를 찾아서 리턴한다.
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        private string GetColorwayId(string key)
        {
            foreach (TreeListNode rootNode in vwTreeList.Nodes)
            {
                if (rootNode.HasChildren)
                {
                    foreach (TreeListNode childNode in rootNode.Nodes)
                    {
                        string childKey = childNode.GetValue("FACTORY").ToString() + childNode.GetValue("WS_NO").ToString();
                        if (key == childKey)
                        {
                            string colorwayId = childNode.GetValue("DEV_COLORWAY_ID").ToString();
                            if (colorwayId != "")
                                return colorwayId;
                        }
                    }
                }
            }
            return "";
        }

        /// <summary>
        /// 
        /// </summary>
        private void SetPtrnPartName(string type)
        {
            vwTreeList.CellValueChanged -= new CellValueChangedEventHandler(CustomCellValueChanged);

            if (type == "Top")
            {
                // 기준 파트명
                string partName = string.Empty;
                // 기준 파트코드
                string partCode = string.Empty;
                // 루트 노드가 포함된 경우 건너띄고 바로 다음 노드를 기준으로 삼음
                foreach (TreeListNode node in vwTreeList.Selection)
                {
                    if (node.HasChildren)
                        continue;

                    partName = node.GetValue("PART_NAME").ToString();
                    partCode = node.GetValue("PART_CD").ToString();
                    break;
                }

                foreach (TreeListNode node in vwTreeList.Selection)
                {
                    // 루트 노드는 건너띔
                    if (node.HasChildren)
                        continue;

                    string rowStatus = node.GetValue("ROW_STATUS").ToString();
                    if (rowStatus != "D")
                    {
                        node.SetValue("PTRN_PART_NAME", partName);
                        node.SetValue("PTRN_PART_CD", partCode);
                    }
                    // 인디케이터를 "U"로 변경, 신규 행과 삭제할 행은 제외
                    if (rowStatus != "I" && rowStatus != "D")
                        node.SetValue("ROW_STATUS", "U");
                }
            }
            else if (type == "Each")
            {
                foreach (TreeListNode node in vwTreeList.Selection)
                {
                    if (node.HasChildren)
                        continue;

                    string partName = node.GetValue("PART_NAME").ToString();
                    string partCode = node.GetValue("PART_CD").ToString();
                    string rowStatus = node.GetValue("ROW_STATUS").ToString();
                    if (rowStatus != "D")
                    {
                        node.SetValue("PTRN_PART_NAME", partName);
                        node.SetValue("PTRN_PART_CD", partCode);
                    }
                    // 인디케이터를 "U"로 변경, 신규 행과 삭제할 행은 제외
                    if (rowStatus != "I" && rowStatus != "D")
                        node.SetValue("ROW_STATUS", "U");
                }
            }

            vwTreeList.CellValueChanged += new CellValueChangedEventHandler(CustomCellValueChanged);
        }

        /// <summary>
        /// 
        /// </summary>
        private void ShowMaterialInfomation()
        {
            string PDMSuppMatNumber = vwTreeList.FocusedNode.GetValue("MXSXL_NUMBER").ToString();
            if (PDMSuppMatNumber == "") return;

            string[] splitPDMSuppMatNum = PDMSuppMatNumber.Split('.');
            if (splitPDMSuppMatNum.Length != 3) return;

            string materialCode = splitPDMSuppMatNum[0];

            Dictionary<string, string> dic = new Dictionary<string, string>();
            dic.Add("MXSXL_NUMBER", PDMSuppMatNumber);
            dic.Add("MAT_CD", materialCode);

            MaterialInformation form = new MaterialInformation();
            form.SOURCE_OF_MAT_INFO = dic;

            form.ShowDialog();
        }

        /// <summary>
        /// 
        /// </summary>
        private void ConnectToMdm()
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
        #endregion

        #region Button Event
        /// <summary>
        /// Save 처리
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnSave_Click(object sender, EventArgs e)
        {
            // 수행시간이 비교적 길어 WaitForm을 띄움
            SplashScreenManager.ShowForm(this, typeof(MainWaitForm), true, true, false);

            #region 선택된 BOM의 컨펌 여부 확인
            DataTable dataSource = ReturnBOMHeaderInfo("ConfirmFlag");
            if (dataSource != null)
            {
                for (int i = 0; i < dataSource.Rows.Count; i++)
                {
                    string confirmFlag = dataSource.Rows[i]["CS_BOM_CFM"].ToString();
                    if (confirmFlag == "C")
                    {
                        // 이미 컨펌된 BOM이 하나라도 있으면 알림
                        MessageBox.Show("There is a already confirmed BOM. Please click 'Confirm' again.");
                        break;
                    }
                }
            }
            #endregion

            char[] separator = new char[] { ',' };

            // 유저가 선택한 각 BOM의 WS_NO 추출
            string[] arrWsNo = SOURCE_OF_PARENT[1].Split(separator, StringSplitOptions.RemoveEmptyEntries);

            // 솔팅을 위한 비교 대상 생성 -> <Key(Factory + WS_NO), Sort No>
            Dictionary<string, int> sortNumberByKey = new Dictionary<string, int>();

            for (int i = 0; i < arrWsNo.Length; i++)
            {
                string key = SOURCE_OF_PARENT[0] + arrWsNo[i]; // factory + ws_no
                sortNumberByKey.Add(key, 1);  // 초기값은 모두 1부터 시작
            }

            string rowStatus = string.Empty;
            ArrayList parameters = new ArrayList();

            // 솔팅 넘버 세팅
            foreach (TreeListNode rootNode in vwTreeList.Nodes)
            {
                if (rootNode.HasChildren)
                {
                    foreach (TreeListNode childNode in rootNode.Nodes)
                    {
                        rowStatus = childNode.GetValue("ROW_STATUS").ToString();
                        if (rowStatus != "D")
                        {
                            #region 솔팅 넘버 세팅
                            // 자식 노드의 키 값
                            string childKey = childNode.GetValue("FACTORY").ToString() + childNode.GetValue("WS_NO").ToString();
                            if (sortNumberByKey.ContainsKey(childKey))
                            {
                                // 현재 솔팅 넘버 값으로 자식 노드의 정렬 순서 지정
                                childNode.SetValue("SORT_NO", sortNumberByKey[childKey]);
                                // 솔팅 넘버를 하나 증가시킴
                                int nextSortNo = sortNumberByKey[childKey] + 1;
                                // 기존 키 삭제 후 증가된 솔팅 넘버로 키 업데이트
                                sortNumberByKey.Remove(childKey);
                                sortNumberByKey.Add(childKey, nextSortNo);
                            }
                            #endregion
                        }

                        #region 패키지 매개변수 입력
                        PKG_INTG_BOM.UPDATE_BOM_TAIL pkgUpdate = new PKG_INTG_BOM.UPDATE_BOM_TAIL();
                        pkgUpdate.ARG_FACTORY = SOURCE_OF_PARENT[0];
                        pkgUpdate.ARG_WS_NO = childNode.GetValue("WS_NO").ToString();
                        pkgUpdate.ARG_PART_SEQ = childNode.GetValue("PART_SEQ").ToString();
                        pkgUpdate.ARG_PART_NIKE_NO = childNode.GetValue("PART_NIKE_NO").ToString();
                        pkgUpdate.ARG_PART_NIKE_COMMENT = childNode.GetValue("PART_NIKE_COMMENT").ToString();
                        pkgUpdate.ARG_PART_CD = childNode.GetValue("PART_CD").ToString();
                        pkgUpdate.ARG_PART_NAME = childNode.GetValue("PART_NAME").ToString().Trim();
                        pkgUpdate.ARG_PART_TYPE = childNode.GetValue("PART_TYPE").ToString().Trim();
                        pkgUpdate.ARG_BTTM = childNode.GetValue("BTTM").ToString();
                        pkgUpdate.ARG_MXSXL_NUMBER = childNode.GetValue("MXSXL_NUMBER").ToString();
                        pkgUpdate.ARG_CS_CD = childNode.GetValue("CS_CD").ToString();
                        pkgUpdate.ARG_MAT_CD = childNode.GetValue("MAT_CD").ToString();
                        pkgUpdate.ARG_MAT_NAME = childNode.GetValue("MAT_NAME").ToString();
                        pkgUpdate.ARG_MAT_COMMENT = childNode.GetValue("MAT_COMMENT").ToString().Trim();
                        pkgUpdate.ARG_MCS_NUMBER = childNode.GetValue("MCS_NUMBER").ToString();
                        pkgUpdate.ARG_COLOR_CD = childNode.GetValue("COLOR_CD").ToString();
                        pkgUpdate.ARG_COLOR_NAME = childNode.GetValue("COLOR_NAME").ToString();
                        pkgUpdate.ARG_COLOR_COMMENT = childNode.GetValue("COLOR_COMMENT").ToString().Trim();
                        pkgUpdate.ARG_SORT_NO = childNode.GetValue("SORT_NO").ToString();
                        pkgUpdate.ARG_REMARKS = childNode.GetValue("REMARKS").ToString().Trim();
                        pkgUpdate.ARG_UPD_USER = Common.sessionID;
                        pkgUpdate.ARG_PTRN_PART_NAME = childNode.GetValue("PTRN_PART_NAME").ToString().Trim();
                        pkgUpdate.ARG_PCX_MAT_ID = childNode.GetValue("PCX_MAT_ID").ToString();
                        pkgUpdate.ARG_PCX_SUPP_MAT_ID = childNode.GetValue("PCX_SUPP_MAT_ID").ToString();
                        pkgUpdate.ARG_PCX_COLOR_ID = childNode.GetValue("PCX_COLOR_ID").ToString();
                        // Update Type 구분(N, I, D)
                        pkgUpdate.ARG_ROW_STATUS = childNode.GetValue("ROW_STATUS").ToString();
                        pkgUpdate.ARG_PROCESS = childNode.GetValue("PROCESS").ToString().ToUpper();
                        pkgUpdate.ARG_VENDOR_NAME = childNode.GetValue("VENDOR_NAME").ToString().ToUpper();
                        pkgUpdate.ARG_COMBINE_YN = childNode.GetValue("COMBINE_YN").ToString();
                        pkgUpdate.ARG_STICKER_YN = childNode.GetValue("STICKER_YN").ToString();
                        pkgUpdate.ARG_PTRN_PART_CD = childNode.GetValue("PTRN_PART_CD").ToString();
                        pkgUpdate.ARG_MDSL_CHK = childNode.GetValue("MDSL_CHK").ToString();
                        pkgUpdate.ARG_OTSL_CHK = childNode.GetValue("OTSL_CHK").ToString();
                        #endregion

                        parameters.Add(pkgUpdate);
                    }
                }
            }

            if (projectBaseForm.Exe_Modify_PKG(parameters) != null)
            {
                #region 컨펌 플래그 업데이트
                // 패키지 매개변수 입력
                PKG_INTG_BOM.UPDATE_BOM_CFM_INFO pkgUpdate = new PKG_INTG_BOM.UPDATE_BOM_CFM_INFO();
                pkgUpdate.ARG_FACTORY = SOURCE_OF_PARENT[0];
                pkgUpdate.ARG_WS_NO = SOURCE_OF_PARENT[1];
                pkgUpdate.ARG_CS_BOM_CFM = "N";
                pkgUpdate.ARG_CBD_YN = "Pass";
                pkgUpdate.ARG_UPD_USER = Common.sessionID;
                // 패키지 호출용 임시변수
                ArrayList arrayList = new ArrayList();
                arrayList.Add(pkgUpdate);
                // 패키지 호출
                if (projectBaseForm.Exe_Modify_PKG(arrayList) == null)
                {
                    // WaitForm 종료
                    SplashScreenManager.CloseForm(false);
                    MessageBox.Show("Failed to change BOM status");
                    return;
                }
                #endregion

                // 저장 전 포커싱된 노드의 키값 저장
                object keyField = string.Empty;

                if (vwTreeList.FocusedNode.HasChildren)
                    keyField = vwTreeList.FocusedNode.GetValue("KEY_FIELD");
                else
                    keyField = vwTreeList.FocusedNode.ParentNode.GetValue("KEY_FIELD");
                
                // 업데이트된 데이터로 소스 리바인딩
                BindDataSourceToTreeList("DataSource");

                // WaitForm 종료
                SplashScreenManager.CloseForm(false);

                // 저장 전 위치 포커싱
                vwTreeList.ExpandAll();
                vwTreeList.Selection.Clear();
                vwTreeList.FocusedNode = vwTreeList.FindNodeByKeyID(keyField);
                vwTreeList.Selection.Add(vwTreeList.FocusedNode);

                MessageBox.Show("Complete");
            }
            else
            {
                // WaitForm 종료
                SplashScreenManager.CloseForm(false);
                MessageBox.Show("Failed to save", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
        }

        /// <summary>
        /// workType에 맞는 BOM 헤더 정보를 가져옴
        /// </summary>
        /// <returns></returns>
        private DataTable ReturnBOMHeaderInfo(string workType)
        {
            try
            {
                // 패키지 매개변수 입력
                PKG_INTG_BOM.SELECT_BOM_INFO_BY_CONFIRM pkgSelect = new PKG_INTG_BOM.SELECT_BOM_INFO_BY_CONFIRM();
                pkgSelect.ARG_WORK_TYPE = workType;
                pkgSelect.ARG_FACTORY = SOURCE_OF_PARENT[0];
                pkgSelect.ARG_WS_NO = SOURCE_OF_PARENT[1];
                pkgSelect.ARG_PART_SEQ = "";
                pkgSelect.OUT_CURSOR = string.Empty;
                // 데이터 소스를 가져옴
                DataTable dataSource = projectBaseForm.Exe_Select_PKG(pkgSelect).Tables[0];
                if (dataSource.Rows.Count > 0)
                    return dataSource;

                return null;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
                return null;
            }
        }
        #endregion

        #region 사용자 정의 함수
        /// <summary>
        /// 
        /// </summary>
        /// <param name="workType"></param>
        private void BindDataSourceToTreeList(string workType)
        {
            PKG_INTG_BOM.SELECT_NEW_MUL_EDIT_SOURCE pkgSelect = new PKG_INTG_BOM.SELECT_NEW_MUL_EDIT_SOURCE();
            pkgSelect.ARG_WORK_TYPE = workType;
            pkgSelect.ARG_FACTORY = SOURCE_OF_PARENT[0];
            pkgSelect.ARG_WS_NO = SOURCE_OF_PARENT[1];
            pkgSelect.OUT_CURSOR = string.Empty;

            DataTable dataSource = projectBaseForm.Exe_Select_PKG(pkgSelect).Tables[0];
            vwTreeList.DataSource = dataSource;
        }
        #endregion
    }
}