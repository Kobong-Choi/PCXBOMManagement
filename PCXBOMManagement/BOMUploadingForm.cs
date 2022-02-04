using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

using CSI.Client.ProjectBaseForm;   // ProjectBaseForm Class
using CSI.PCC.PCX.COM;              // Common Class
using CSI.PCC.PCX.PACKAGE;          // Package Class
using Newtonsoft.Json;              // Related to JSON
using Newtonsoft.Json.Linq;         // JToken, JObject
using System.IO;                    // Path Class
using System.Xml;                   // Related to XML
using System.Collections;           // ArrayList

namespace CSI.PCC.PCX
{
    public partial class BOMUploadingForm : DevExpress.XtraEditors.XtraForm
    {
        #region User Defined Variables
        
        public ProjectBaseForm projectForm = Common.PROJECTBASEFORM.projectForm;
        DataSet cnvtObjects = null;
        public bool isUploaded = false;

        #endregion

        public BOMUploadingForm()
        {
            InitializeComponent();
        }

        /// <summary>
        /// 폼 로딩 시 실행
        /// LookUpEdit에 데이터를 바인딩한다.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BOMUploadingForm_Load(object sender, EventArgs e)
        {
            // PM 수정 불가능
            txtPCCPM.EditValue = Common.USER_INFO.userID;

            #region Bind Data To LookUpEdit

            // Factory
            Common.BindDataToLookUpEdit(cboFactory, "FACTORY", "", "PCC", false);
            cboFactory.EditValue = Common.USER_INFO.userFactory;

            // Season
            Common.BindDataToLookUpEdit(cboSeason, "SEASON", "", "SEASON", false);
            // 기본 설정은 가장 상위의 아이템
            cboSeason.ItemIndex = 0;

            // Category
            Common.BindDataToLookUpEdit(cboCategory, "CATEGORY", "", "BOM_CAT", false);
            // 기본 설정은 가장 상위의 아이템
            cboCategory.ItemIndex = 0;

            // Round
            Common.BindDataToLookUpEdit(cboRound, "ROUND", Common.USER_INFO.userFactory, "B", false);
            // 기본 설정은 가장 상위의 아이템
            cboRound.ItemIndex = 0;

            // TD
            Common.BindDataToLookUpEdit(cboTD, "TD", "", "TD", false);
            // 기본 설정은 가장 상위의 아이템
            cboTD.ItemIndex = 0;

            // Gender
            Common.BindDataToLookUpEdit(cboGender, "GENDER", "", "", false);
            // 기본 설정은 가장 상위의 아이템
            cboGender.ItemIndex = 0;
            
            // Age
            Common.BindDataToLookUpEdit(cboAge, "AGE", "", "", false);
            // 기본 설정은 가장 상위의 아이템
            cboAge.ItemIndex = 0;

            #endregion
        }

        #region Button Click Event Handler

        /// <summary>
        /// JSON BOM에서 데이터를 읽어 각 컨트롤에 바인딩한다.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnLoad_Click(object sender, EventArgs e)
        {
            // 하나 이상의 파일 이름을 지정하여 열 수 있는 공용 대화 상자 생성
            OpenFileDialog fileOpen = new OpenFileDialog();
            // 화면에 표시되는 파일 형식 결정
            fileOpen.Filter = "json 파일|*.json";
            // 파일 대화 상자에 표시되는 초기 디렉터리 설정
            fileOpen.InitialDirectory = "";
            // 하나의 파일만 선택할 수 있도록 설정
            fileOpen.Multiselect = false;

            // 모달 대화 상자로 표시
            if (fileOpen.ShowDialog() == DialogResult.OK)
            {
                // 선택한 각 파일의 이름이 하나씩 들어 있는 배열을 가져옴
                string[] fileNames = fileOpen.FileNames;
                // 파일의 확장자를 소문자로 가져옴
                string fileExt = Path.GetExtension(fileNames[0]).ToLower();
                // 파일명을 가져옴
                string fileName = Path.GetFileName(fileNames[0]);

                // 선택한 파일의 확장자 확인('json'이 아닐 경우 리턴)
                if (fileExt != ".json") 
                    return;
                
                using (StreamReader file = File.OpenText(fileNames[0]))
                {
                    /* JsonTextReader : Represents a reader that provides fast, non-cached, forward-only access to JSON text data. */

                    // Initializes a new instance of the JsonTextReader class with the specified TextReader.
                    using (JsonTextReader reader = new JsonTextReader(file))
                    {
                        // Creates a JToken from a JsonReader.
                        JObject jObjects = (JObject)JToken.ReadFrom(reader);

                        cnvtObjects = ConvertJsonIntoXml(jObjects.ToString());

                        //cnvtJObjects.Tables[0] : JSON의 "BOM Header" Object
                        DataTable objBomHeader = cnvtObjects.Tables[0];
                        //cnvtJObjects.Tables[1] : JSON의 "BOM Data" Object
                        DataTable objBomData = cnvtObjects.Tables[1];

                        //"BOM Header" Object의 값들을 Header Info. 컨트롤에 바인딩
                        BindDataToPCXControls(objBomHeader);
                        //"BOM Data" Object의 값들을 Line Item 그리드에 바인딩
                        BindDataToLineItemGrid(objBomData);    
                    }
                }
            }
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            /******************************************************************
             * Save 방법이 두 가지임
             * 1. 매뉴얼 입력 후 생성
             * 2. JSON BOM을 Load하여 생성
            ******************************************************************/

            #region Save전 필수 검증 로직 추가
            #endregion

            string createdKey = CreateBOMKey(); //BOM Key를 새로 만듦

            SaveDataToOrgTable(createdKey); //히스토리 테이블에 데이터 저장

            SaveDataToOprTable(createdKey); //운영 테이블에 데이터 저장
        }
        #endregion

        #region User Defined Functions

        /// <summary>
        /// JSON 형태의 데이터를 XML 형태의 테이블 데이터로 바꿈
        /// </summary>
        /// <param name="_jObject"></param>
        /// <returns></returns>
        private DataSet ConvertJsonIntoXml(String _jObject)
        {
            /****************************************************************************************
             * Valid XML must have one root element
             * the JSON passed to DeserializeXmlNode should have one property in the root JSON object
            ****************************************************************************************/
            _jObject = "{ \"rootNode\": {" + _jObject.Trim().TrimStart('{').TrimEnd('}') + @"} }";

            var cnvtXMLData = new XmlDocument();
            // Deserializes the XmlNode from a JSON string.
            cnvtXMLData = JsonConvert.DeserializeXmlNode(_jObject);
            
            var _cnvtJObjects = new DataSet();
            // XML 스키마와 데이터를 DataSet으로 읽어옴
            _cnvtJObjects.ReadXml(new XmlNodeReader(cnvtXMLData));

            return _cnvtJObjects;
        }

        private void BindDataToPCXControls(DataTable _objBomHeader)
        {
            DataRow headerValues = _objBomHeader.Rows[0];

            txtDevStyleId.Text = headerValues["developmentStyleIdentifier"].ToString();
            txtDevColorwayId.Text = headerValues["developmentColorwayIdentifier"].ToString();
            txtColorwayName.Text = headerValues["colorwayName"].ToString();
            txtSourcingConfigId.Text = headerValues["sourcingConfigurationIdentifier"].ToString();
            txtSourcingConfigName.Text = headerValues["sourcingConfigurationName"].ToString();
            txtBomId.Text = headerValues["bomIdentifier"].ToString();
            txtBomName.Text = headerValues["bomName"].ToString();
            txtBomVersion.Text = headerValues["bomVersionNumber"].ToString();
            txtStyleNumber.Text = headerValues["styleNumber"].ToString();
            txtStyleName.Text = headerValues["styleName"].ToString();
            txtModelId.Text = headerValues["modelIdentifier"].ToString();
            cboGender.EditValue = headerValues["gender"].ToString();
            cboAge.EditValue = headerValues["age"].ToString();
            txtProductId.Text = headerValues["productId"].ToString();
            txtProductCode.Text = headerValues["productCode"].ToString();
        }

        private void BindDataToLineItemGrid(DataTable _objBomData)
        {
            try
            {
                //그리드의 DataSource
                DataTable dataSource = _objBomData;     

                //라이브러리와 매칭된 값을 저장할 컬럼 추가
                dataSource.Columns.Add("PCX_PART_TYPE");
                dataSource.Columns.Add("PCX_PART_NAME");
                dataSource.Columns.Add("CS_PART_NAME");
                dataSource.Columns.Add("CS_PART_CODE");
                dataSource.Columns.Add("PDM_SUPP_MAT_NUM");
                dataSource.Columns.Add("PDM_MAT_NAME");
                dataSource.Columns.Add("PCX_COLOR_NAME");

                //dataSource의 로우 개수만큼 반복
                foreach (DataRow dr in dataSource.Rows)
                {
                    //로우 단위로 PCX Library와 매칭
                    PKG_PCX_BOM_MNG.SELECT_MATCHED_CODE pkgSelectMatchedCode = new PKG_PCX_BOM_MNG.SELECT_MATCHED_CODE();
                    pkgSelectMatchedCode.ARG_BOM_SECTION_ID = dr["billOfMaterialsSectionIdentifier"].ToString().Trim();
                    pkgSelectMatchedCode.ARG_PCX_PART_ID = dr["partNameIdentifier"].ToString().Trim();
                    pkgSelectMatchedCode.ARG_PCX_SUPP_MAT_ID = dr["suppliedMaterialIdentifier"].ToString().Trim();
                    pkgSelectMatchedCode.ARG_PCX_COLOR_ID = dr["colorIdentifier"].ToString().Trim();
                    pkgSelectMatchedCode.OUT_CURSOR = string.Empty;

                    DataTable matchedValues = projectForm.Exe_Select_PKG(pkgSelectMatchedCode).Tables[0];  //매칭된 코드 정보

                    //매칭된 값이 하나도 없으면 에러가 발생하므로 예외처리
                    if (matchedValues.Rows.Count > 0)
                    {
                        //매칭된 코드 값을 현재 로우에 저장
                        dr["PCX_PART_TYPE"] = matchedValues.Rows[0]["PCX_PART_TYPE"].ToString();
                        dr["PCX_PART_NAME"] = matchedValues.Rows[0]["PCX_PART_NAME"].ToString();
                        dr["CS_PART_NAME"] = matchedValues.Rows[0]["CS_PART_NAME"].ToString();
                        dr["CS_PART_CODE"] = matchedValues.Rows[0]["CS_PART_CODE"].ToString();
                        dr["PDM_SUPP_MAT_NUM"] = matchedValues.Rows[0]["PDM_SUPP_MAT_NUM"].ToString();
                        dr["PDM_MAT_NAME"] = matchedValues.Rows[0]["PDM_MAT_NAME"].ToString();
                        dr["PCX_COLOR_NAME"] = matchedValues.Rows[0]["PCX_COLOR_NAME"].ToString();
                    }
                }
                grdLineItem.DataSource = dataSource;    //만들어진 DataSource를 그리드에 바인딩
                gvwLineItem.BestFitColumns();           //그리드 컬럼 최적화
            }
            catch (Exception ex)
            {
                projectForm.MessageBoxW(ex.ToString());
            }
        }

        private string CreateBOMKey()
        {
            PKG_PCX_BOM_MNG.SELECT_BOM_KEY pkgSelectBomKey = new PKG_PCX_BOM_MNG.SELECT_BOM_KEY();
            pkgSelectBomKey.ARG_FACTORY = Common.USER_INFO.userFactory;
            pkgSelectBomKey.ARG_UPD_USER = Common.USER_INFO.userID;
            pkgSelectBomKey.OUT_CURSOR = string.Empty;

            DataTable dtResult = projectForm.Exe_Select_PKG(pkgSelectBomKey).Tables[0];

            string _createdKey = dtResult.Rows[0][0].ToString();

            return _createdKey;
        }

        private void SaveDataToOrgTable(string _createdKey)
        {
            try
            {
                ArrayList listBOMHeader = new ArrayList();
                ArrayList listBOMData = new ArrayList();

                if (cnvtObjects == null)
                {
                    /***************************************************************
                     * 매뉴얼 생성인 경우, 유저가 입력한 데이터만 Head 테이블에 저장
                     * PCX_BOM_TAIL_ORG 테이블에 데이터를 저장하는 것은 생략함
                    ***************************************************************/

                    #region BOM_HEAD_ORG
                    PKG_PCX_BOM_MNG.INSERT_BOM_HEAD_ORG pkgInsertBomHeadOrg = new PKG_PCX_BOM_MNG.INSERT_BOM_HEAD_ORG();
                    pkgInsertBomHeadOrg.ARG_BOM_KEY = _createdKey;
                    pkgInsertBomHeadOrg.ARG_OBJ_ID = "";
                    pkgInsertBomHeadOrg.ARG_OBJ_TYPE = "";
                    pkgInsertBomHeadOrg.ARG_DEV_STYLE_ID = txtDevStyleId.Text.Trim();
                    pkgInsertBomHeadOrg.ARG_DEV_COLORWAY_ID = txtDevColorwayId.Text.Trim();
                    pkgInsertBomHeadOrg.ARG_COLORWAY_NAME = txtColorwayName.Text.Trim();
                    pkgInsertBomHeadOrg.ARG_SRC_CONFIG_ID = txtSourcingConfigId.Text.Trim();
                    pkgInsertBomHeadOrg.ARG_SRC_CONFIG_NAME = txtSourcingConfigName.Text.Trim();
                    pkgInsertBomHeadOrg.ARG_BOM_ID = txtBomId.Text.Trim();
                    pkgInsertBomHeadOrg.ARG_BOM_NAME = txtBomName.Text.Trim();
                    pkgInsertBomHeadOrg.ARG_BOM_VERSION = txtBomVersion.Text.Trim();
                    pkgInsertBomHeadOrg.ARG_BOM_DESC = "";
                    pkgInsertBomHeadOrg.ARG_BOM_CMMT = "";
                    pkgInsertBomHeadOrg.ARG_BOM_STATUS_INDC = "";
                    pkgInsertBomHeadOrg.ARG_CREATE_TIME_STMP = "";
                    pkgInsertBomHeadOrg.ARG_CHANGE_TIME_STMP = "";
                    pkgInsertBomHeadOrg.ARG_CREATED_BY = "";
                    pkgInsertBomHeadOrg.ARG_MODIFIED_BY = "";
                    pkgInsertBomHeadOrg.ARG_STYLE_NO = txtStyleNumber.Text.Trim();
                    pkgInsertBomHeadOrg.ARG_STYLE_NAME = txtStyleName.Text.Trim();
                    pkgInsertBomHeadOrg.ARG_MODEL_ID = txtModelId.Text.Trim();
                    pkgInsertBomHeadOrg.ARG_GENDER = cboGender.EditValue.ToString();
                    pkgInsertBomHeadOrg.ARG_AGE = cboAge.EditValue.ToString();
                    pkgInsertBomHeadOrg.ARG_PRODUCT_ID = txtProductId.Text.Trim();
                    pkgInsertBomHeadOrg.ARG_PRODUCT_CODE = txtProductCode.Text.Trim();
                    pkgInsertBomHeadOrg.ARG_UPD_USER = Common.USER_INFO.userID;

                    listBOMHeader.Add(pkgInsertBomHeadOrg);
                    #endregion
                }
                else
                {
                    /**********************************************************
                     * JSON BOM을 Load한 경우 원본 속성 값을 모두 저장
                    **********************************************************/

                    DataRow objBomHeader = cnvtObjects.Tables[0].Rows[0];        //JSON의 "BOM Header" 원본 값들을 가져옴
                    DataTable objBomData = (DataTable)grdLineItem.DataSource;    //JSON의 "BOM Data" 원본 값들을 가져옴

                    #region BOM_HEAD_ORG
                    PKG_PCX_BOM_MNG.INSERT_BOM_HEAD_ORG pkgInsertBomHeadOrg = new PKG_PCX_BOM_MNG.INSERT_BOM_HEAD_ORG();
                    pkgInsertBomHeadOrg.ARG_BOM_KEY = _createdKey;
                    pkgInsertBomHeadOrg.ARG_OBJ_ID = objBomHeader["objectId"].ToString().Trim();
                    pkgInsertBomHeadOrg.ARG_OBJ_TYPE = objBomHeader["objectType"].ToString().Trim();
                    pkgInsertBomHeadOrg.ARG_DEV_STYLE_ID = txtDevStyleId.Text.Trim();
                    pkgInsertBomHeadOrg.ARG_DEV_COLORWAY_ID = txtDevColorwayId.Text.Trim();
                    pkgInsertBomHeadOrg.ARG_COLORWAY_NAME = txtColorwayName.Text.Trim();
                    pkgInsertBomHeadOrg.ARG_SRC_CONFIG_ID = txtSourcingConfigId.Text.Trim();
                    pkgInsertBomHeadOrg.ARG_SRC_CONFIG_NAME = txtSourcingConfigName.Text.Trim();
                    pkgInsertBomHeadOrg.ARG_BOM_ID = txtBomId.Text.Trim();
                    pkgInsertBomHeadOrg.ARG_BOM_NAME = txtBomName.Text.Trim();
                    pkgInsertBomHeadOrg.ARG_BOM_VERSION = txtBomVersion.Text.Trim();
                    pkgInsertBomHeadOrg.ARG_BOM_DESC = objBomHeader["bomDescription"].ToString().Trim();
                    pkgInsertBomHeadOrg.ARG_BOM_CMMT = objBomHeader["bomComments"].ToString().Trim();
                    pkgInsertBomHeadOrg.ARG_BOM_STATUS_INDC = objBomHeader["billOfMaterialStatusIndicator"].ToString().Trim();
                    pkgInsertBomHeadOrg.ARG_CREATE_TIME_STMP = objBomHeader["createTimestamp"].ToString().Trim();
                    pkgInsertBomHeadOrg.ARG_CHANGE_TIME_STMP = objBomHeader["changeTimestamp"].ToString().Trim();
                    pkgInsertBomHeadOrg.ARG_CREATED_BY = objBomHeader["createdBy"].ToString().Trim();
                    pkgInsertBomHeadOrg.ARG_MODIFIED_BY = objBomHeader["modifiedBy"].ToString().Trim();
                    pkgInsertBomHeadOrg.ARG_STYLE_NO = txtStyleNumber.Text.Trim();
                    pkgInsertBomHeadOrg.ARG_STYLE_NAME = txtStyleName.Text.Trim();
                    pkgInsertBomHeadOrg.ARG_MODEL_ID = txtModelId.Text.Trim();
                    pkgInsertBomHeadOrg.ARG_GENDER = cboGender.EditValue.ToString();
                    pkgInsertBomHeadOrg.ARG_AGE = cboAge.EditValue.ToString();
                    pkgInsertBomHeadOrg.ARG_PRODUCT_ID = txtProductId.Text.Trim();
                    pkgInsertBomHeadOrg.ARG_PRODUCT_CODE = txtProductCode.Text.Trim();
                    pkgInsertBomHeadOrg.ARG_UPD_USER = Common.USER_INFO.userID;

                    listBOMHeader.Add(pkgInsertBomHeadOrg);
                    #endregion

                    #region BOM_TAIL_ORG
                    foreach (DataRow dr in objBomData.Rows)
                    {
                        PKG_PCX_BOM_MNG.INSERT_BOM_TAIL_ORG pkgInsertBomDataOrg = new PKG_PCX_BOM_MNG.INSERT_BOM_TAIL_ORG();
                        pkgInsertBomDataOrg.ARG_BOM_KEY = _createdKey;
                        pkgInsertBomDataOrg.ARG_BOM_LINE_SORT_SEQ = dr["bomLineSortSequence"].ToString();
                        pkgInsertBomDataOrg.ARG_BOM_SECTION_ID = dr["billOfMaterialsSectionIdentifier"].ToString();
                        pkgInsertBomDataOrg.ARG_PART_NAME_ID = dr["partNameIdentifier"].ToString();
                        pkgInsertBomDataOrg.ARG_PTTN_PART_ID = dr["patternPartIdentifier"].ToString();
                        pkgInsertBomDataOrg.ARG_SUPP_MTL_ID = dr["suppliedMaterialIdentifier"].ToString();
                        pkgInsertBomDataOrg.ARG_MTL_ITEM_ID = dr["materialItemIdentifier"].ToString();
                        pkgInsertBomDataOrg.ARG_MTL_ITEM_PLCHDR_DESC = dr["materialItemPlaceholderDescription"].ToString();
                        pkgInsertBomDataOrg.ARG_COLOR_ID = dr["colorIdentifier"].ToString();
                        pkgInsertBomDataOrg.ARG_COLOR_PLCHDR_DESC = dr["colorPlaceholderDescription"].ToString();
                        pkgInsertBomDataOrg.ARG_SUPP_MTL_COL_IS_MULTI = dr["suppliedMaterialColorIsMultipleColors"].ToString();
                        pkgInsertBomDataOrg.ARG_SUPP_MTL_COLOR_ID = dr["suppliedMaterialColorIdentifier"].ToString();
                        pkgInsertBomDataOrg.ARG_BOM_LINE_ITEM_CMMT = dr["bomLineItemComments"].ToString();
                        pkgInsertBomDataOrg.ARG_FAC_INHOUSE_INDC = dr["factoryInHouseIndicator"].ToString();
                        pkgInsertBomDataOrg.ARG_COUNTRY_OF_ORG_ID = dr["countyOfOriginIdentifier"].ToString();
                        pkgInsertBomDataOrg.ARG_ACTL_DIMENSION_DESC = dr["actualDimensionDescription"].ToString();
                        pkgInsertBomDataOrg.ARG_ISO_MSRMNT_CD = dr["isoMeasurementCode"].ToString();
                        pkgInsertBomDataOrg.ARG_NET_USAGE_NO = dr["netUsageNumber"].ToString();
                        pkgInsertBomDataOrg.ARG_WASTE_USAGE_NO = dr["wasteUsageNumber"].ToString();
                        pkgInsertBomDataOrg.ARG_PART_YIELD = dr["partYield"].ToString();
                        pkgInsertBomDataOrg.ARG_CONSUMP_CONVER_RATE = dr["consumptionConversionRate"].ToString();
                        pkgInsertBomDataOrg.ARG_LINE_ITEM_DEFEC_PER_NO = dr["lineItemDefectPercentNumber"].ToString();
                        pkgInsertBomDataOrg.ARG_UNIT_PRICE_ISO_MSR_CD = dr["unitPriceISOMeasurementCode"].ToString();
                        pkgInsertBomDataOrg.ARG_CURRENCY_CD = dr["currencyCode"].ToString();
                        pkgInsertBomDataOrg.ARG_FAC_UNIT_PRICE = dr["factoryUnitPrice"].ToString();
                        pkgInsertBomDataOrg.ARG_UNIT_PRICE_UPCH_NO = dr["unitPriceUpchargeNumber"].ToString();
                        pkgInsertBomDataOrg.ARG_UNIT_PRICE_UPCH_DESC = dr["unitPriceUpchargeDescription"].ToString();
                        pkgInsertBomDataOrg.ARG_FREIGHT_TERM_ID = dr["freightTermIdentifier"].ToString();
                        pkgInsertBomDataOrg.ARG_LANDED_COST_PER_NO = dr["landedCostPercentNumber"].ToString();
                        pkgInsertBomDataOrg.ARG_UPD_USER = Common.USER_INFO.userID;

                        listBOMData.Add(pkgInsertBomDataOrg);
                    }
                    #endregion
                }

                DataSet dsResult = projectForm.Exe_Modify_PKG(listBOMHeader);   //패키지 호출

                if (dsResult == null)
                {
                    projectForm.MessageBoxW("Failed to org. save");
                    return;
                }

                dsResult = projectForm.Exe_Modify_PKG(listBOMData); //패키지 호출

                if (dsResult == null)
                {
                    projectForm.MessageBoxW("Failed to org. save");
                    return;
                }
            }
            catch (Exception ex)
            {
                projectForm.MessageBoxW(ex.ToString());
            }
        }

        private void SaveDataToOprTable(string _createdKey)
        {
            try
            {
                ArrayList listBOMHeader = new ArrayList();
                ArrayList listBOMData = new ArrayList();

                #region BOM_HEAD
                PKG_PCX_BOM_MNG.INSERT_BOM_HEAD pkgInsertBomHead = new PKG_PCX_BOM_MNG.INSERT_BOM_HEAD();
                pkgInsertBomHead.ARG_BOM_KEY = _createdKey;
                pkgInsertBomHead.ARG_UPD_USER = Common.USER_INFO.userID;
                pkgInsertBomHead.ARG_SEASON_CD = cboSeason.EditValue.ToString();
                pkgInsertBomHead.ARG_ROUND = cboRound.EditValue.ToString();
                pkgInsertBomHead.ARG_SAMPLE_SIZE = txtSampleSize.Text;
                pkgInsertBomHead.ARG_SAMPLE_QTY = txtSampleQty.Text;
                pkgInsertBomHead.ARG_SAMPLE_ETS = Convert.ToDateTime(dateSampleEts.EditValue).ToString("yyyyMMdd");
                pkgInsertBomHead.ARG_LAST_CD = txtLastCode.Text;
                pkgInsertBomHead.ARG_TD = cboTD.EditValue.ToString();
                pkgInsertBomHead.ARG_CATEGORY = cboCategory.EditValue.ToString();

                listBOMHeader.Add(pkgInsertBomHead);
                #endregion

                if (cnvtObjects == null)
                {
                    #region BOM_TAIL
                    PKG_PCX_BOM_MNG.INSERT_BOM_TAIL pkgInsertBomData = new PKG_PCX_BOM_MNG.INSERT_BOM_TAIL();
                    pkgInsertBomData.ARG_BOM_KEY = _createdKey;
                    pkgInsertBomData.ARG_PART_SEQ = "1";                        //Integer Type
                    pkgInsertBomData.ARG_SORT_NO = "1";                         //Integer Type
                    pkgInsertBomData.ARG_UPD_USER = Common.USER_INFO.userID;
                    pkgInsertBomData.ARG_BOM_LINE_SORT_SEQ = "1";               //Integer Type
                    pkgInsertBomData.ARG_BOM_SECTION_ID = "";
                    pkgInsertBomData.ARG_PART_TYPE = "";
                    pkgInsertBomData.ARG_PCX_PART_ID = "";
                    pkgInsertBomData.ARG_PART_CD = "";
                    pkgInsertBomData.ARG_PART_NAME = "";
                    pkgInsertBomData.ARG_PCX_PTTN_PART_ID = "";
                    pkgInsertBomData.ARG_PTTN_PART_CD = "";
                    pkgInsertBomData.ARG_PTTN_PART_NAME = "";
                    pkgInsertBomData.ARG_PCX_SUPP_MTL_ID = "";
                    pkgInsertBomData.ARG_MXSXL_NUMBER = "";
                    pkgInsertBomData.ARG_MAT_NAME = "";
                    pkgInsertBomData.ARG_MAT_CMMT = "";
                    pkgInsertBomData.ARG_PCX_COLOR_ID = "";

                    listBOMData.Add(pkgInsertBomData);
                    #endregion
                }
                else
                {
                    DataTable objBomData = (DataTable)grdLineItem.DataSource;

                    #region BOM_TAIL
                    int partSeq = 1;
                    
                    foreach (DataRow dr in objBomData.Rows)
                    {
                        PKG_PCX_BOM_MNG.INSERT_BOM_TAIL pkgInsertBomData = new PKG_PCX_BOM_MNG.INSERT_BOM_TAIL();
                        pkgInsertBomData.ARG_BOM_KEY = _createdKey;
                        pkgInsertBomData.ARG_PART_SEQ = partSeq.ToString();
                        pkgInsertBomData.ARG_SORT_NO = partSeq.ToString();
                        pkgInsertBomData.ARG_UPD_USER = Common.USER_INFO.userID;
                        pkgInsertBomData.ARG_BOM_LINE_SORT_SEQ = dr["bomLineSortSequence"].ToString();
                        pkgInsertBomData.ARG_BOM_SECTION_ID = dr["billOfMaterialsSectionIdentifier"].ToString();
                        pkgInsertBomData.ARG_PART_TYPE = dr["PCX_PART_TYPE"].ToString();
                        pkgInsertBomData.ARG_PCX_PART_ID = dr["partNameIdentifier"].ToString();
                        pkgInsertBomData.ARG_PART_CD = dr["CS_PART_CODE"].ToString();
                        pkgInsertBomData.ARG_PART_NAME = dr["CS_PART_NAME"].ToString();
                        pkgInsertBomData.ARG_PCX_PTTN_PART_ID = dr["patternPartIdentifier"].ToString();
                        pkgInsertBomData.ARG_PTTN_PART_CD = dr["CS_PART_CODE"].ToString();
                        pkgInsertBomData.ARG_PTTN_PART_NAME = dr["CS_PART_NAME"].ToString();
                        pkgInsertBomData.ARG_PCX_SUPP_MTL_ID = dr["suppliedMaterialIdentifier"].ToString();
                        pkgInsertBomData.ARG_MXSXL_NUMBER = dr["PDM_SUPP_MAT_NUM"].ToString();
                        pkgInsertBomData.ARG_MAT_NAME = dr["PDM_MAT_NAME"].ToString();
                        pkgInsertBomData.ARG_MAT_CMMT = dr["bomLineItemComments"].ToString();
                        pkgInsertBomData.ARG_PCX_COLOR_ID = dr["colorIdentifier"].ToString();

                        listBOMData.Add(pkgInsertBomData);
                        partSeq++;
                    }
                    #endregion
                }

                DataSet dsResult = projectForm.Exe_Modify_PKG(listBOMHeader);

                if (dsResult == null)
                {
                    projectForm.MessageBoxW("Failed to save for head.");
                    return;
                }

                dsResult = projectForm.Exe_Modify_PKG(listBOMData);

                if (dsResult != null)
                {
                    MessageBox.Show("Save Complete.", "Save", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    isUploaded = true;
                    this.Close();
                }
                else
                {
                    projectForm.MessageBoxW("Failed to save for tail.");
                    isUploaded = false;
                    return;
                }
            }
            catch (Exception ex)
            {
                projectForm.MessageBoxW(ex.ToString());
            }
        }
        #endregion
    }
}