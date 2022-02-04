using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Collections;                       // ArrayList

using CSI.Client.ProjectBaseForm;               // ProjectBaseForm Class
using CSI.PCC.PCX.COM;                          // Common Class
using CSI.PCC.PCX.PACKAGE;                      // Package Class

using DevExpress.XtraGrid;                      // GridControl
using DevExpress.XtraGrid.Views.Base;           // GridCell
using DevExpress.XtraGrid.Views.Grid;           // GridView
using DevExpress.XtraGrid.Views.Grid.ViewInfo;  // GridHitInfo
using DevExpress.Utils;                         // DXMouseEventArgs
using DevExpress.XtraEditors;                   // GridLookUpEdit

namespace CSI.PCC.PCX
{
    public partial class FindCode : DevExpress.XtraEditors.XtraForm
    {
        public ProjectBaseForm projectBaseForm = Common.projectBaseForm;
        private InitialValues initialValues;

        // 폼 종료 시 검색된 코드 정보를 담을 변수
        public object FORM_RESULT;

        public FindCode()
        {
            InitializeComponent();
        }

        public FindCode(object[] parameters)
        {
            InitializeComponent();

            initialValues = new InitialValues();

            initialValues.Type = Convert.ToInt32(parameters[0]);
            initialValues.Keyword = parameters[1].ToString();
            initialValues.PartDelimiter = parameters[2].ToString();
        }

        private void FindCode_Load(object sender, EventArgs e)
        {
            rdoSearchType.SelectedIndexChanged -= new EventHandler(rdoSearchType_SelectedIndexChanged);

            rdoSearchType.SelectedIndex = initialValues.Type;
            txtKeyword.Text = initialValues.Keyword;

            SetGridView(initialValues.Type);
            
            rdoSearchType.SelectedIndexChanged += new EventHandler(rdoSearchType_SelectedIndexChanged);
        }

        /// <summary>
        /// PCX 라이브러리에서 특정 데이터를 검색
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnSearch_Click(object sender, EventArgs e)
        {
            // 검색할 문자열을 입력하였는지 확인
            if (txtKeyword.Text == "")
            {
                MessageBox.Show("Please enter a character to search for.");
                return;
            }

            // 검색 타입
            int type = rdoSearchType.SelectedIndex;

            // 타입에 맞게 라이브러리 조회
            if (type == 0)
            {
                SearchCodeFromLibrary("Part");
            }
            else if (type == 1)
            {
                SearchCodeFromLibrary("PCX_Material");
            }
            else if (type == 2)
            {
                SearchCodeFromLibrary("Color");
            }
            else if (type == 3)
            {
                SearchCodeFromLibrary("PCC_Material");
            }
            else if (type == 4)
            {
                SearchCodeFromLibrary("CS_Material");
            }
        }

        /// <summary>
        /// 텍스트 박스에 문자열 입력 후 Enter 키를 누를 경우 동작
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void txtKeyword_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                // 검색할 문자열을 입력하였는지 확인
                if (txtKeyword.Text == "")
                {
                    MessageBox.Show("Please enter a character to search for.");
                    return;
                }

                int selectedIndex = rdoSearchType.SelectedIndex;

                if (selectedIndex == 0)
                {
                    SearchCodeFromLibrary("Part");
                }
                else if (selectedIndex == 1)
                {
                    SearchCodeFromLibrary("PCX_Material");
                }
                else if (selectedIndex == 2)
                {
                    SearchCodeFromLibrary("Color");
                }
                else if (selectedIndex == 3)
                {
                    SearchCodeFromLibrary("PCC_Material");
                }
                else if (selectedIndex == 4)
                {
                    SearchCodeFromLibrary("CS_Material");
                }
            }
        }

        /// <summary>
        /// 타입에 맞는 그리드 뷰 설정
        /// </summary>
        /// <param name="_type"></param>
        private void SetGridView(int _type)
        {
            if (_type == 0)
            {
                #region Part

                gvwLibrary.Columns["PCX_PART_ID"].Visible = true;
                gvwLibrary.Columns["PCX_PART_ID"].VisibleIndex = 0;

                gvwLibrary.Columns["PART_NAME"].Visible = true;
                gvwLibrary.Columns["PART_NAME"].VisibleIndex = 1;

                gvwLibrary.Columns["PART_TYPE"].Visible = true;
                gvwLibrary.Columns["PART_TYPE"].VisibleIndex = 2;

                gvwLibrary.Columns["PART_CD"].Visible = true;
                gvwLibrary.Columns["PART_CD"].VisibleIndex = 3;

                gvwLibrary.Columns["PART_STATUS"].Visible = true;
                gvwLibrary.Columns["PART_STATUS"].VisibleIndex = 4;

                gvwLibrary.Columns["PART_SHORT_NAME"].Visible = false;

                gvwLibrary.Columns["BLACK_LIST_YN"].Visible = false;
                gvwLibrary.Columns["PCX_MAT_ID"].Visible = false;
                gvwLibrary.Columns["PCX_SUPP_MAT_ID"].Visible = false;
                gvwLibrary.Columns["MAT_CD"].Visible = false;
                gvwLibrary.Columns["MXSXL_NUMBER"].Visible = false;
                gvwLibrary.Columns["CS_CD"].Visible = false;
                gvwLibrary.Columns["MCS_NUMBER"].Visible = false;
                gvwLibrary.Columns["MAT_NAME"].Visible = false;
                gvwLibrary.Columns["MAT_TYPE"].Visible = false;
                gvwLibrary.Columns["VEN_NAME"].Visible = false;
                gvwLibrary.Columns["MAT_STATUS"].Visible = false;

                gvwLibrary.Columns["PCX_COLOR_ID"].Visible = false;
                gvwLibrary.Columns["COLOR_CD"].Visible = false;
                gvwLibrary.Columns["COLOR_NAME"].Visible = false;
                gvwLibrary.Columns["COLOR_STATUS"].Visible = false;
                
                gvwLibrary.Columns["CS_MAT_CD"].Visible = false;
                gvwLibrary.Columns["CS_MAT_NAME"].Visible = false;
                gvwLibrary.Columns["WIDTH"].Visible = false;
                gvwLibrary.Columns["MNG_UNIT"].Visible = false;
                gvwLibrary.Columns["SPEC_DESC"].Visible = false;

                #endregion
            }
            else if (_type == 1)
            {
                #region PCX Material

                gvwLibrary.Columns["BLACK_LIST_YN"].Visible = true;
                gvwLibrary.Columns["BLACK_LIST_YN"].VisibleIndex = 0;

                gvwLibrary.Columns["MAT_STATUS"].Visible = true;
                gvwLibrary.Columns["MAT_STATUS"].VisibleIndex = 1;

                gvwLibrary.Columns["PCX_MAT_ID"].Visible = true;
                gvwLibrary.Columns["PCX_MAT_ID"].VisibleIndex = 2;
                
                gvwLibrary.Columns["PCX_SUPP_MAT_ID"].Visible = true;
                gvwLibrary.Columns["PCX_SUPP_MAT_ID"].VisibleIndex = 3;

                gvwLibrary.Columns["MAT_CD"].Visible = false;

                gvwLibrary.Columns["MXSXL_NUMBER"].Visible = true;
                gvwLibrary.Columns["MXSXL_NUMBER"].VisibleIndex = 4;

                gvwLibrary.Columns["CS_CD"].Visible = false;

                gvwLibrary.Columns["MCS_NUMBER"].Visible = true;
                gvwLibrary.Columns["MCS_NUMBER"].VisibleIndex = 5;

                gvwLibrary.Columns["MAT_NAME"].Visible = true;
                gvwLibrary.Columns["MAT_NAME"].VisibleIndex = 6;

                gvwLibrary.Columns["VEN_NAME"].Visible = true;
                gvwLibrary.Columns["VEN_NAME"].VisibleIndex = 7;                

                gvwLibrary.Columns["MAT_TYPE"].Visible = true;
                gvwLibrary.Columns["MAT_TYPE"].VisibleIndex = 8;
                
                gvwLibrary.Columns["PCX_PART_ID"].Visible = false;
                gvwLibrary.Columns["PART_NAME"].Visible = false;
                gvwLibrary.Columns["PART_SHORT_NAME"].Visible = false;
                gvwLibrary.Columns["PART_TYPE"].Visible = false;
                gvwLibrary.Columns["PART_CD"].Visible = false;
                gvwLibrary.Columns["PART_STATUS"].Visible = false;

                gvwLibrary.Columns["PCX_COLOR_ID"].Visible = false;
                gvwLibrary.Columns["COLOR_CD"].Visible = false;
                gvwLibrary.Columns["COLOR_NAME"].Visible = false;
                gvwLibrary.Columns["COLOR_STATUS"].Visible = false;

                gvwLibrary.Columns["CS_MAT_CD"].Visible = false;
                gvwLibrary.Columns["CS_MAT_NAME"].Visible = false;
                gvwLibrary.Columns["WIDTH"].Visible = false;
                gvwLibrary.Columns["MNG_UNIT"].Visible = false;
                gvwLibrary.Columns["SPEC_DESC"].Visible = false;

                #endregion
            }
            else if (_type == 2)
            {
                #region Color

                gvwLibrary.Columns["PCX_COLOR_ID"].Visible = true;
                gvwLibrary.Columns["PCX_COLOR_ID"].VisibleIndex = 0;

                gvwLibrary.Columns["COLOR_CD"].Visible = true;
                gvwLibrary.Columns["COLOR_CD"].VisibleIndex = 1;

                gvwLibrary.Columns["COLOR_NAME"].Visible = true;
                gvwLibrary.Columns["COLOR_NAME"].VisibleIndex = 2;

                gvwLibrary.Columns["COLOR_STATUS"].Visible = true;
                gvwLibrary.Columns["COLOR_STATUS"].VisibleIndex = 3;

                gvwLibrary.Columns["PCX_PART_ID"].Visible = false;
                gvwLibrary.Columns["PART_NAME"].Visible = false;
                gvwLibrary.Columns["PART_SHORT_NAME"].Visible = false;
                gvwLibrary.Columns["PART_TYPE"].Visible = false;
                gvwLibrary.Columns["PART_CD"].Visible = false;
                gvwLibrary.Columns["PART_STATUS"].Visible = false;

                gvwLibrary.Columns["BLACK_LIST_YN"].Visible = false;
                gvwLibrary.Columns["PCX_MAT_ID"].Visible = false;
                gvwLibrary.Columns["PCX_SUPP_MAT_ID"].Visible = false;
                gvwLibrary.Columns["MAT_CD"].Visible = false;
                gvwLibrary.Columns["MXSXL_NUMBER"].Visible = false;
                gvwLibrary.Columns["CS_CD"].Visible = false;
                gvwLibrary.Columns["MCS_NUMBER"].Visible = false;
                gvwLibrary.Columns["MAT_NAME"].Visible = false;
                gvwLibrary.Columns["MAT_TYPE"].Visible = false;
                gvwLibrary.Columns["MAT_STATUS"].Visible = false;
                gvwLibrary.Columns["VEN_NAME"].Visible = false;

                gvwLibrary.Columns["CS_MAT_CD"].Visible = false;
                gvwLibrary.Columns["CS_MAT_NAME"].Visible = false;
                gvwLibrary.Columns["WIDTH"].Visible = false;
                gvwLibrary.Columns["MNG_UNIT"].Visible = false;
                gvwLibrary.Columns["SPEC_DESC"].Visible = false;

                #endregion
            }
            else if (_type == 3)
            {
                #region PCC Material

                gvwLibrary.Columns["BLACK_LIST_YN"].Visible = true;
                gvwLibrary.Columns["BLACK_LIST_YN"].VisibleIndex = 0;

                gvwLibrary.Columns["MAT_STATUS"].Visible = true;
                gvwLibrary.Columns["MAT_STATUS"].VisibleIndex = 1;

                gvwLibrary.Columns["PCX_MAT_ID"].Visible = true;
                gvwLibrary.Columns["PCX_MAT_ID"].VisibleIndex = 2;

                gvwLibrary.Columns["PCX_SUPP_MAT_ID"].Visible = true;
                gvwLibrary.Columns["PCX_SUPP_MAT_ID"].VisibleIndex = 3;

                gvwLibrary.Columns["MXSXL_NUMBER"].Visible = true;
                gvwLibrary.Columns["MXSXL_NUMBER"].VisibleIndex = 4;
                                
                gvwLibrary.Columns["CS_CD"].Visible = true;
                gvwLibrary.Columns["CS_CD"].VisibleIndex = 5;

                gvwLibrary.Columns["MCS_NUMBER"].Visible = true;
                gvwLibrary.Columns["MCS_NUMBER"].VisibleIndex = 6;

                gvwLibrary.Columns["MAT_NAME"].Visible = true;
                gvwLibrary.Columns["MAT_NAME"].VisibleIndex = 7;

                gvwLibrary.Columns["MAT_TYPE"].Visible = true;
                gvwLibrary.Columns["MAT_TYPE"].VisibleIndex = 8;

                gvwLibrary.Columns["VEN_NAME"].Visible = true;
                gvwLibrary.Columns["VEN_NAME"].VisibleIndex = 9;

                gvwLibrary.Columns["MAT_CD"].Visible = false;
                gvwLibrary.Columns["PCX_PART_ID"].Visible = false;
                gvwLibrary.Columns["PART_NAME"].Visible = false;
                gvwLibrary.Columns["PART_SHORT_NAME"].Visible = false;
                gvwLibrary.Columns["PART_TYPE"].Visible = false;
                gvwLibrary.Columns["PART_CD"].Visible = false;
                gvwLibrary.Columns["PART_STATUS"].Visible = false;

                gvwLibrary.Columns["PCX_COLOR_ID"].Visible = false;
                gvwLibrary.Columns["COLOR_CD"].Visible = false;
                gvwLibrary.Columns["COLOR_NAME"].Visible = false;
                gvwLibrary.Columns["COLOR_STATUS"].Visible = false;

                gvwLibrary.Columns["CS_MAT_CD"].Visible = false;
                gvwLibrary.Columns["CS_MAT_NAME"].Visible = false;
                gvwLibrary.Columns["WIDTH"].Visible = false;
                gvwLibrary.Columns["MNG_UNIT"].Visible = false;
                gvwLibrary.Columns["SPEC_DESC"].Visible = false;

                #endregion
            }
            else if (_type == 4)
            {
                #region CS Material

                gvwLibrary.Columns["CS_MAT_CD"].Visible = true;
                gvwLibrary.Columns["CS_MAT_CD"].VisibleIndex = 0;

                gvwLibrary.Columns["CS_MAT_NAME"].Visible = true;
                gvwLibrary.Columns["CS_MAT_NAME"].VisibleIndex = 1;

                gvwLibrary.Columns["WIDTH"].Visible = true;
                gvwLibrary.Columns["WIDTH"].VisibleIndex = 2;

                gvwLibrary.Columns["MNG_UNIT"].Visible = true;
                gvwLibrary.Columns["MNG_UNIT"].VisibleIndex = 3;

                gvwLibrary.Columns["SPEC_DESC"].Visible = true;
                gvwLibrary.Columns["SPEC_DESC"].VisibleIndex = 4;

                gvwLibrary.Columns["PCX_PART_ID"].Visible = false;
                gvwLibrary.Columns["PART_NAME"].Visible = false;
                gvwLibrary.Columns["PART_SHORT_NAME"].Visible = false;
                gvwLibrary.Columns["PART_TYPE"].Visible = false;
                gvwLibrary.Columns["PART_CD"].Visible = false;
                gvwLibrary.Columns["PART_STATUS"].Visible = false;

                gvwLibrary.Columns["BLACK_LIST_YN"].Visible = false;
                gvwLibrary.Columns["PCX_MAT_ID"].Visible = false;
                gvwLibrary.Columns["PCX_SUPP_MAT_ID"].Visible = false;
                gvwLibrary.Columns["MAT_CD"].Visible = false;
                gvwLibrary.Columns["MXSXL_NUMBER"].Visible = false;
                gvwLibrary.Columns["CS_CD"].Visible = false;
                gvwLibrary.Columns["MCS_NUMBER"].Visible = false;
                gvwLibrary.Columns["MAT_NAME"].Visible = false;
                gvwLibrary.Columns["MAT_TYPE"].Visible = false;
                gvwLibrary.Columns["MAT_STATUS"].Visible = false;
                gvwLibrary.Columns["VEN_NAME"].Visible = false;

                gvwLibrary.Columns["PCX_COLOR_ID"].Visible = false;
                gvwLibrary.Columns["COLOR_CD"].Visible = false;
                gvwLibrary.Columns["COLOR_NAME"].Visible = false;
                gvwLibrary.Columns["COLOR_STATUS"].Visible = false;

                #endregion
            }
        }

        /// <summary>
        /// PCX 라이브러리에서 키워드에 맞는 데이터를 가져와 그리드에 바인딩
        /// </summary>
        /// <param name="_type"></param>
        private void SearchCodeFromLibrary(string _type)
        {
            try
            {
                #region 폼 컬러 검색 시 Validation

                // 폼 컬러 지정 시 창신 코드를 입력할 경우 MC-MCP 코드를 사용하도록 유도
                if (_type == "Color")
                {
                    // 적어도 두 자리는 입력해야 구분 가능
                    if (txtKeyword.Text.Length > 1)
                    {
                        string colorCode = txtKeyword.Text;
                        char[] eachCharacter = colorCode.ToArray();

                        // 첫 글자가 'C'이면서
                        if (eachCharacter[0] == 'C')
                        {
                            // 두 번째 글자가 0 - 9(아스키코드) 에 속할 경우 창신 컬러 코드임
                            if (eachCharacter[1] >= 48 && eachCharacter[1] <= 57)
                            {
                                MessageBox.Show("In case of CS Color, Please use MC-MCP Code and type color in color comment.",
                                    "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
                                return;
                            }
                        }
                    }
                }

                #endregion

                PKG_INTG_BOM_COMMON.SELECT_LIBRARY_CODE pkgSelectLibCode = new PKG_INTG_BOM_COMMON.SELECT_LIBRARY_CODE();
                pkgSelectLibCode.ARG_TYPE = _type;
                pkgSelectLibCode.ARG_KEYWORD = txtKeyword.Text;
                pkgSelectLibCode.OUT_CURSOR = string.Empty;

                DataTable dataSource = projectBaseForm.Exe_Select_PKG(pkgSelectLibCode).Tables[0];
                grdLibrary.DataSource = dataSource;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
                return;
            }
        }

        /// <summary>
        /// 라디오 그룹의 타입이 바뀔 경우 그리드 뷰를 해당 타입에 맞게 새로 설정
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void rdoSearchType_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                RadioGroup rdoType = sender as RadioGroup;

                int selectedType = rdoType.SelectedIndex;

                SetGridView(selectedType);

                if (selectedType == 1)
                    SearchCodeFromLibrary("PCX_Material");
                else if (selectedType == 3)
                    SearchCodeFromLibrary("PCC_Material");
                else if (selectedType == 4)
                {
                    SearchCodeFromLibrary("CS_Material");
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
        private void gvwLibrary_DoubleClick(object sender, EventArgs e)
        {
            GridView view = sender as GridView;

            string[] result = new string[12];

            if (rdoSearchType.SelectedIndex == 0)
            {
                string partName = view.GetFocusedRowCellValue("PART_NAME").ToString();
                string partType = view.GetFocusedRowCellValue("PART_TYPE").ToString();
                string partCode = view.GetFocusedRowCellValue("PART_CD").ToString();
                string pcxPartId = view.GetFocusedRowCellValue("PCX_PART_ID").ToString();

                if (initialValues.PartDelimiter == "Pattern")
                    result[0] = "PatternPart";
                else
                    result[0] = "Part";

                result[1] = partName;
                result[2] = partType;
                result[3] = partCode;
                result[4] = pcxPartId;
            }
            else if (rdoSearchType.SelectedIndex == 1)
            {
                string pcxMaterialID = view.GetFocusedRowCellValue("PCX_MAT_ID").ToString();
                string pcxSuppMatID = view.GetFocusedRowCellValue("PCX_SUPP_MAT_ID").ToString();
                string pdmMatCode = view.GetFocusedRowCellValue("MAT_CD").ToString();
                string pdmSuppMatCode = view.GetFocusedRowCellValue("MXSXL_NUMBER").ToString();
                string pdmMatName = view.GetFocusedRowCellValue("MAT_NAME").ToString();
                string mcsNumber = view.GetFocusedRowCellValue("MCS_NUMBER").ToString();
                string vendorName = view.GetFocusedRowCellValue("VEN_NAME").ToString();
                string nikeMSState = view.GetFocusedRowCellValue("MAT_STATUS").ToString();
                string blackListYN = view.GetFocusedRowCellValue("BLACK_LIST_YN").ToString();
                string mdmNotExisting = view.GetFocusedRowCellValue("MDM_NOT_EXST").ToString();

                result[0] = "PCX_Material";
                result[1] = pcxMaterialID;
                result[2] = (pdmMatCode == "null") ? "" : pdmMatCode;           // 라이브러리에 null로 존재 시 ""로 변환
                result[3] = pdmMatName;
                result[4] = (pdmSuppMatCode == "null") ? "" : pdmSuppMatCode;   // 라이브러리에 null로 존재 시 ""로 변환
                result[5] = mcsNumber;
                result[6] = vendorName;
                result[7] = ""; // CS Code
                result[8] = pcxSuppMatID;
                result[9] = nikeMSState;
                result[10] = blackListYN;
                result[11] = mdmNotExisting;
            }
            else if (rdoSearchType.SelectedIndex == 2)
            {
                string pcxColorId = view.GetFocusedRowCellValue("PCX_COLOR_ID").ToString();
                string colorCode = view.GetFocusedRowCellValue("COLOR_CD").ToString();
                string colorName = view.GetFocusedRowCellValue("COLOR_NAME").ToString();

                result[0] = "Color";
                result[1] = pcxColorId;
                result[2] = colorCode;
                result[3] = colorName;
            }
            else if (rdoSearchType.SelectedIndex == 3)
            {
                string pcxMatID = view.GetFocusedRowCellValue("PCX_MAT_ID").ToString();
                string pcxSuppMatID = view.GetFocusedRowCellValue("PCX_SUPP_MAT_ID").ToString();
                string pdmMatCode = view.GetFocusedRowCellValue("MAT_CD").ToString();
                string pdmSuppMatCode = view.GetFocusedRowCellValue("MXSXL_NUMBER").ToString();
                string mcsNumber = view.GetFocusedRowCellValue("MCS_NUMBER").ToString();
                string csCode = view.GetFocusedRowCellValue("CS_CD").ToString();
                string pdmMatName = view.GetFocusedRowCellValue("MAT_NAME").ToString();
                string vendorName = view.GetFocusedRowCellValue("VEN_NAME").ToString();
                string nikeMSState = view.GetFocusedRowCellValue("MAT_STATUS").ToString();
                string blackListYN = view.GetFocusedRowCellValue("BLACK_LIST_YN").ToString();

                result[0] = "PCC_Material";
                result[1] = (pcxMatID == "") ? "100" : pcxMatID;
                result[2] = (pdmMatCode == "null") ? "" : pdmMatCode;           // 라이브러리에 null로 존재 시 ""로 변환
                result[3] = pdmMatName;
                result[4] = (pdmSuppMatCode == "null") ? "" : pdmSuppMatCode;   // 라이브러리에 null로 존재 시 ""로 변환
                result[5] = (mcsNumber == "NULL") ? "" : mcsNumber;
                result[6] = vendorName;
                result[7] = csCode;
                result[8] = (pcxSuppMatID == "") ? "100" : pcxSuppMatID;
                result[9] = nikeMSState;
                result[10] = blackListYN;
            }
            else if (rdoSearchType.SelectedIndex == 4)
            {
                string csMatCode = view.GetFocusedRowCellValue("CS_MAT_CD").ToString();
                string materialCode = view.GetFocusedRowCellValue("MAT_CD").ToString();
                string csMatName = view.GetFocusedRowCellValue("CS_MAT_NAME").ToString();

                result[0] = "CS_Material";
                result[1] = csMatCode;
                result[2] = materialCode;
                result[3] = csMatName;
            }

            FORM_RESULT = result;

            this.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.Close();
        }

        /// <summary>
        /// Retired 자재 표기
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void gvwLibrary_RowCellStyle(object sender, RowCellStyleEventArgs e)
        {
            GridView view = sender as GridView;
            
            // Nike MS State가 'Retired'인 자재 표기
            string status = view.GetRowCellValue(e.RowHandle, "MAT_STATUS").ToString();

            if (status == "Retired")
            {
                if (view.IsRowSelected(e.RowHandle) == false)
                {
                    //e.Appearance.ForeColor = Color.Black;
                    e.Appearance.Font = new Font(e.Appearance.Font, FontStyle.Bold);
                    e.Appearance.BackColor = Color.DarkGray;
                }
            }
        }
    }

    class InitialValues
    {
        private int type = 0;
        public int Type
        {
            get { return type; }
            set { type = value; }
        }

        private string keyword = string.Empty;
        public string Keyword
        {
            get { return keyword; }
            set { keyword = value; }
        }

        private string partDelimiter = string.Empty;
        public string PartDelimiter
        {
            get { return partDelimiter; }
            set { partDelimiter = value; }
        }
    }
}