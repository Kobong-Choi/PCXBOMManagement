using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CSI.PCC.PCX.PACKAGE
{
    // Custom Attribute
    public class DataType : Attribute
    {
        public string Value
        {
            get;
            set;
        }
    }

    public class PKG_INTG_BOM_COMMON
    {
        public class SELECT_FACTORY
        {
            private string _ARG_FACTORY_TYPE;

            [DataType(Value = "VARCHAR2")]
            public string ARG_FACTORY_TYPE
            {
                get { return _ARG_FACTORY_TYPE; }
                set { _ARG_FACTORY_TYPE = value; }
            }

            private string _OUT_CURSOR;

            [DataType(Value = "REF CURSOR")]
            public string OUT_CURSOR
            {
                get { return _OUT_CURSOR; }
                set { _OUT_CURSOR = value; }
            }
        }

        public class SELECT_SEASON
        {
            private string _ARG_GROUP_CODE;
            [DataType(Value = "VARCHAR2")]
            public string ARG_GROUP_CODE
            {
                get { return _ARG_GROUP_CODE; }
                set { _ARG_GROUP_CODE = value; }
            }

            private string _OUT_CURSOR;
            [DataType(Value = "REF CURSOR")]
            public string OUT_CURSOR
            {
                get { return _OUT_CURSOR; }
                set { _OUT_CURSOR = value; }
            }
        }

        public class SELECT_GENDER
        {
            private string _ARG_ATTRIBUTE_NAME;
            [DataType(Value = "VARCHAR2")]
            public string ARG_ATTRIBUTE_NAME
            {
                get { return _ARG_ATTRIBUTE_NAME; }
                set { _ARG_ATTRIBUTE_NAME = value; }
            }

            private string _OUT_CURSOR;
            [DataType(Value = "REF CURSOR")]
            public string OUT_CURSOR
            {
                get { return _OUT_CURSOR; }
                set { _OUT_CURSOR = value; }
            }
        }

        public class SELECT_COMMON_CODE
        {
            private string _ARG_TYPE;
            [DataType(Value = "VARCHAR2")]
            public string ARG_TYPE
            {
                get { return _ARG_TYPE; }
                set { _ARG_TYPE = value; }
            }

            private string _ARG_SORT_TYPE;
            [DataType(Value = "VARCHAR2")]
            public string ARG_SORT_TYPE
            {
                get { return _ARG_SORT_TYPE; }
                set { _ARG_SORT_TYPE = value; }
            }

            private string _OUT_CURSOR;
            [DataType(Value = "REF CURSOR")]
            public string OUT_CURSOR
            {
                get { return _OUT_CURSOR; }
                set { _OUT_CURSOR = value; }
            }
        }

        public class SELECT_SAMPLE_TYPE
        {
            private string _ARG_FACTORY;
            [DataType(Value = "VARCHAR2")]
            public string ARG_FACTORY
            {
                get { return _ARG_FACTORY; }
                set { _ARG_FACTORY = value; }
            }

            private string _ARG_ST_DIV;
            [DataType(Value = "VARCHAR2")]
            public string ARG_ST_DIV
            {
                get { return _ARG_ST_DIV; }
                set { _ARG_ST_DIV = value; }
            }

            private string _ARG_USE_YN;
            [DataType(Value = "VARCHAR2")]
            public string ARG_USE_YN
            {
                get { return _ARG_USE_YN; }
                set { _ARG_USE_YN = value; }
            }

            private string _OUT_CURSOR;
            [DataType(Value = "REF CURSOR")]
            public string OUT_CURSOR
            {
                get { return _OUT_CURSOR; }
                set { _OUT_CURSOR = value; }
            }
        }

        public class SELECT_SUB_SAMPLE_TYPE
        {
            private string _ARG_ST_DIV;
            [DataType(Value = "VARCHAR2")]
            public string ARG_ST_DIV
            {
                get { return _ARG_ST_DIV; }
                set { _ARG_ST_DIV = value; }
            }

            private string _OUT_CURSOR;
            [DataType(Value = "REF CURSOR")]
            public string OUT_CURSOR
            {
                get { return _OUT_CURSOR; }
                set { _OUT_CURSOR = value; }
            }
        }

        public class SELECT_DEVELOPER
        {
            private string _ARG_FACTORY;
            [DataType(Value = "VARCHAR2")]
            public string ARG_FACTORY
            {
                get { return _ARG_FACTORY; }
                set { _ARG_FACTORY = value; }
            }

            private string _OUT_CURSOR;
            [DataType(Value = "REF CURSOR")]
            public string OUT_CURSOR
            {
                get { return _OUT_CURSOR; }
                set { _OUT_CURSOR = value; }
            }
        }

        public class SELECT_CATEGORY
        {
            private string _ARG_GROUP_CODE;
            [DataType(Value = "VARCHAR2")]
            public string ARG_GROUP_CODE
            {
                get { return _ARG_GROUP_CODE; }
                set { _ARG_GROUP_CODE = value; }
            }

            private string _ARG_CODE_VALUE;
            [DataType(Value = "VARCHAR2")]
            public string ARG_CODE_VALUE
            {
                get { return _ARG_CODE_VALUE; }
                set { _ARG_CODE_VALUE = value; }
            }

            private string _OUT_CURSOR;
            [DataType(Value = "REF CURSOR")]
            public string OUT_CURSOR
            {
                get { return _OUT_CURSOR; }
                set { _OUT_CURSOR = value; }
            }
        }

        public class SELECT_TD
        {
            private string _ARG_GROUP_CODE;
            [DataType(Value = "VARCHAR2")]
            public string ARG_GROUP_CODE
            {
                get { return _ARG_GROUP_CODE; }
                set { _ARG_GROUP_CODE = value; }
            }

            private string _OUT_CURSOR;
            [DataType(Value = "REF CURSOR")]
            public string OUT_CURSOR
            {
                get { return _OUT_CURSOR; }
                set { _OUT_CURSOR = value; }
            }
        }

        public class SELECT_LIBRARY_CODE
        {
            private string _ARG_TYPE;
            [DataType(Value = "VARCHAR2")]
            public string ARG_TYPE
            {
                get { return _ARG_TYPE; }
                set { _ARG_TYPE = value; }
            }

            private string _ARG_KEYWORD;
            [DataType(Value = "VARCHAR2")]
            public string ARG_KEYWORD
            {
                get { return _ARG_KEYWORD; }
                set { _ARG_KEYWORD = value; }
            }

            private string _OUT_CURSOR;
            [DataType(Value = "REF CURSOR")]
            public string OUT_CURSOR
            {
                get { return _OUT_CURSOR; }
                set { _OUT_CURSOR = value; }
            }
        }

        public class SELECT_POSSIBLE_SHIP_DATE
        {
            private string _ARG_FACTORY;
            [DataType(Value = "VARCHAR2")]
            public string ARG_FACTORY
            {
                get { return _ARG_FACTORY; }
                set { _ARG_FACTORY = value; }
            }

            private string _OUT_CURSOR;
            [DataType(Value = "REF CURSOR")]
            public string OUT_CURSOR
            {
                get { return _OUT_CURSOR; }
                set { _OUT_CURSOR = value; }
            }
        }

        public class SELECT_IS_HOLIDAY
        {
            private string _ARG_FACTORY;
            [DataType(Value = "VARCHAR2")]
            public string ARG_FACTORY
            {
                get { return _ARG_FACTORY; }
                set { _ARG_FACTORY = value; }
            }

            private string _ARG_WORK_YMD;
            [DataType(Value = "VARCHAR2")]
            public string ARG_WORK_YMD
            {
                get { return _ARG_WORK_YMD; }
                set { _ARG_WORK_YMD = value; }
            }

            private string _OUT_CURSOR;
            [DataType(Value = "REF CURSOR")]
            public string OUT_CURSOR
            {
                get { return _OUT_CURSOR; }
                set { _OUT_CURSOR = value; }
            }
        }

        public class LOAD_BOM_HEADER
        {
            private string _ARG_FACTORY;
            [DataType(Value = "VARCHAR2")]
            public string ARG_FACTORY
            {
                get { return _ARG_FACTORY; }
                set { _ARG_FACTORY = value; }
            }

            private string _ARG_CONCAT_WS_NO;
            [DataType(Value = "VARCHAR2")]
            public string ARG_CONCAT_WS_NO
            {
                get { return _ARG_CONCAT_WS_NO; }
                set { _ARG_CONCAT_WS_NO = value; }
            }

            private string _OUT_CURSOR;
            [DataType(Value = "REF CURSOR")]
            public string OUT_CURSOR
            {
                get { return _OUT_CURSOR; }
                set { _OUT_CURSOR = value; }
            }
        }
    }

    public class PKG_INTG_BOM
    {
        public class SELECT_BOM_LIST
        {
            private string _ARG_FACTORY;
            [DataType(Value = "VARCHAR2")]
            public string ARG_FACTORY
            {
                get { return _ARG_FACTORY; }
                set { _ARG_FACTORY = value; }
            }

            private string _ARG_DEVELOPER;
            [DataType(Value = "VARCHAR2")]
            public string ARG_DEVELOPER
            {
                get { return _ARG_DEVELOPER; }
                set { _ARG_DEVELOPER = value; }
            }

            private string _ARG_SEASON;
            [DataType(Value = "VARCHAR2")]
            public string ARG_SEASON
            {
                get { return _ARG_SEASON; }
                set { _ARG_SEASON = value; }
            }

            private string _ARG_DEV_COLORWAY_ID;
            [DataType(Value = "VARCHAR2")]
            public string ARG_DEV_COLORWAY_ID
            {
                get { return _ARG_DEV_COLORWAY_ID; }
                set { _ARG_DEV_COLORWAY_ID = value; }
            }

            private string _ARG_PCM_BOM_ID;
            [DataType(Value = "VARCHAR2")]
            public string ARG_PCM_BOM_ID
            {
                get { return _ARG_PCM_BOM_ID; }
                set { _ARG_PCM_BOM_ID = value; }
            }

            private string _ARG_SAMPLE_TYPE;
            [DataType(Value = "VARCHAR2")]
            public string ARG_SAMPLE_TYPE
            {
                get { return _ARG_SAMPLE_TYPE; }
                set { _ARG_SAMPLE_TYPE = value; }
            }

            private string _ARG_STYLE_NAME;
            [DataType(Value = "VARCHAR2")]
            public string ARG_STYLE_NAME
            {
                get { return _ARG_STYLE_NAME; }
                set { _ARG_STYLE_NAME = value; }
            }

            private string _ARG_REP_YN;
            [DataType(Value = "VARCHAR2")]
            public string ARG_REP_YN
            {
                get { return _ARG_REP_YN; }
                set { _ARG_REP_YN = value; }
            }

            private string _ARG_DEV_STYLE_NUMBER;
            [DataType(Value = "VARCHAR2")]
            public string ARG_DEV_STYLE_NUMBER
            {
                get { return _ARG_DEV_STYLE_NUMBER; }
                set { _ARG_DEV_STYLE_NUMBER = value; }
            }

            private string _ARG_PRODUCT_CODE;
            [DataType(Value = "VARCHAR2")]
            public string ARG_PRODUCT_CODE
            {
                get { return _ARG_PRODUCT_CODE; }
                set { _ARG_PRODUCT_CODE = value; }
            }

            private string _OUT_CURSOR;
            [DataType(Value = "REF CURSOR")]
            public string OUT_CURSOR
            {
                get { return _OUT_CURSOR; }
                set { _OUT_CURSOR = value; }
            }
        }

        public class SELECT_NEW_WS_NO
        {
            private string _ARG_FACTORY;
            [DataType(Value = "VARCHAR2")]
            public string ARG_FACTORY
            {
                get { return _ARG_FACTORY; }
                set { _ARG_FACTORY = value; }
            }

            private string _OUT_CURSOR;
            [DataType(Value = "REF CURSOR")]
            public string OUT_CURSOR
            {
                get { return _OUT_CURSOR; }
                set { _OUT_CURSOR = value; }
            }
        }

        public class SELECT_COMMON_CODE_MPP_TB
        {
            private string _ARG_COMMON_ID;
            [DataType(Value = "VARCHAR2")]
            public string ARG_COMMON_ID
            {
                get { return _ARG_COMMON_ID; }
                set { _ARG_COMMON_ID = value; }
            }

            private string _OUT_CURSOR;
            [DataType(Value = "REF CURSOR")]
            public string OUT_CURSOR
            {
                get { return _OUT_CURSOR; }
                set { _OUT_CURSOR = value; }
            }
        }

        public class SELECT_BOM_CODE_MPP_TB
        {
            private string _ARG_CONCAT_PART_ID;
            [DataType(Value = "VARCHAR2")]
            public string ARG_CONCAT_PART_ID
            {
                get { return _ARG_CONCAT_PART_ID; }
                set { _ARG_CONCAT_PART_ID = value; }
            }

            private string _ARG_CONCAT_SUPP_MAT_ID;
            [DataType(Value = "VARCHAR2")]
            public string ARG_CONCAT_SUPP_MAT_ID
            {
                get { return _ARG_CONCAT_SUPP_MAT_ID; }
                set { _ARG_CONCAT_SUPP_MAT_ID = value; }
            }

            private string _ARG_CONCAT_COLOR_ID;
            [DataType(Value = "VARCHAR2")]
            public string ARG_CONCAT_COLOR_ID
            {
                get { return _ARG_CONCAT_COLOR_ID; }
                set { _ARG_CONCAT_COLOR_ID = value; }
            }

            private string _OUT_CURSOR;
            [DataType(Value = "REF CURSOR")]
            public string OUT_CURSOR
            {
                get { return _OUT_CURSOR; }
                set { _OUT_CURSOR = value; }
            }
        }

        public class SELECT_CODE_MPP_TB_EXCEL
        {
            private string _ARG_CHAINED_PART_NAME;
            [DataType(Value = "VARCHAR2")]
            public string ARG_CHAINED_PART_NAME
            {
                get { return _ARG_CHAINED_PART_NAME; }
                set { _ARG_CHAINED_PART_NAME = value; }
            }

            private string _ARG_CHAINED_MSL;
            [DataType(Value = "VARCHAR2")]
            public string ARG_CHAINED_MSL
            {
                get { return _ARG_CHAINED_MSL; }
                set { _ARG_CHAINED_MSL = value; }
            }

            private string _ARG_CHAINED_COLOR_CD;
            [DataType(Value = "VARCHAR2")]
            public string ARG_CHAINED_COLOR_CD
            {
                get { return _ARG_CHAINED_COLOR_CD; }
                set { _ARG_CHAINED_COLOR_CD = value; }
            }

            private string _OUT_CURSOR;
            [DataType(Value = "REF CURSOR")]
            public string OUT_CURSOR
            {
                get { return _OUT_CURSOR; }
                set { _OUT_CURSOR = value; }
            }
        }

        public class SELECT_BOM_HEAD_DATA
        {
            private string _ARG_FACTORY;
            [DataType(Value = "VARCHAR2")]
            public string ARG_FACTORY
            {
                get { return _ARG_FACTORY; }
                set { _ARG_FACTORY = value; }
            }

            private string _ARG_CONCAT_WS_NO;
            [DataType(Value = "VARCHAR2")]
            public string ARG_CONCAT_WS_NO
            {
                get { return _ARG_CONCAT_WS_NO; }
                set { _ARG_CONCAT_WS_NO = value; }
            }

            private string _OUT_CURSOR;
            [DataType(Value = "REF CURSOR")]
            public string OUT_CURSOR
            {
                get { return _OUT_CURSOR; }
                set { _OUT_CURSOR = value; }
            }
        }

        public class SELECT_BOM_TAIL_DATA
        {
            private string _ARG_FACTORY;
            [DataType(Value = "VARCHAR2")]
            public string ARG_FACTORY
            {
                get { return _ARG_FACTORY; }
                set { _ARG_FACTORY = value; }
            }

            private string _ARG_WS_NO;
            [DataType(Value = "VARCHAR2")]
            public string ARG_WS_NO
            {
                get { return _ARG_WS_NO; }
                set { _ARG_WS_NO = value; }
            }

            private string _OUT_CURSOR;
            [DataType(Value = "REF CURSOR")]
            public string OUT_CURSOR
            {
                get { return _OUT_CURSOR; }
                set { _OUT_CURSOR = value; }
            }
        }

        public class SELECT_IS_EXISTING_BOM
        {
            private string _ARG_FACTORY;
            [DataType(Value = "VARCHAR2")]
            public string ARG_FACTORY
            {
                get { return _ARG_FACTORY; }
                set { _ARG_FACTORY = value; }
            }

            private string _ARG_WS_NO;
            [DataType(Value = "VARCHAR2")]
            public string ARG_WS_NO
            {
                get { return _ARG_WS_NO; }
                set { _ARG_WS_NO = value; }
            }

            private string _ARG_SEASON_CD;
            [DataType(Value = "VARCHAR2")]
            public string ARG_SEASON_CD
            {
                get { return _ARG_SEASON_CD; }
                set { _ARG_SEASON_CD = value; }
            }

            private string _ARG_SAMPLE_TYPE;
            [DataType(Value = "VARCHAR2")]
            public string ARG_SAMPLE_TYPE
            {
                get { return _ARG_SAMPLE_TYPE; }
                set { _ARG_SAMPLE_TYPE = value; }
            }

            private string _ARG_SUB_TYPE;
            [DataType(Value = "VARCHAR2")]
            public string ARG_SUB_TYPE
            {
                get { return _ARG_SUB_TYPE; }
                set { _ARG_SUB_TYPE = value; }
            }

            private string _ARG_DEV_COLORWAY_ID;
            [DataType(Value = "VARCHAR2")]
            public string ARG_DEV_COLORWAY_ID
            {
                get { return _ARG_DEV_COLORWAY_ID; }
                set { _ARG_DEV_COLORWAY_ID = value; }
            }

            private string _ARG_SUB_TYPE_REMARK;
            [DataType(Value = "VARCHAR2")]
            public string ARG_SUB_TYPE_REMARK
            {
                get { return _ARG_SUB_TYPE_REMARK; }
                set { _ARG_SUB_TYPE_REMARK = value; }
            }

            //private string _ARG_SAMPLE_SIZE;
            //[DataType(Value = "VARCHAR2")]
            //public string ARG_SAMPLE_SIZE
            //{
            //    get { return _ARG_SAMPLE_SIZE; }
            //    set { _ARG_SAMPLE_SIZE = value; }
            //}

            //private string _ARG_DEV_SR_ID;
            //[DataType(Value = "VARCHAR2")]
            //public string ARG_DEV_SR_ID
            //{
            //    get { return _ARG_DEV_SR_ID; }
            //    set { _ARG_DEV_SR_ID = value; }
            //}

            private string _OUT_CURSOR;
            [DataType(Value = "REF CURSOR")]
            public string OUT_CURSOR
            {
                get { return _OUT_CURSOR; }
                set { _OUT_CURSOR = value; }
            }
        }

        public class SELECT_FOR_DIRECT_INPUT
        {
            private string _ARG_TYPE;
            [DataType(Value = "VARCHAR2")]
            public string ARG_TYPE
            {
                get { return _ARG_TYPE; }
                set { _ARG_TYPE = value; }
            }

            private string _ARG_CODE;
            [DataType(Value = "VARCHAR2")]
            public string ARG_CODE
            {
                get { return _ARG_CODE; }
                set { _ARG_CODE = value; }
            }

            private string _ARG_NAME;
            [DataType(Value = "VARCHAR2")]
            public string ARG_NAME
            {
                get { return _ARG_NAME; }
                set { _ARG_NAME = value; }
            }

            private string _OUT_CURSOR;
            [DataType(Value = "REF CURSOR")]
            public string OUT_CURSOR
            {
                get { return _OUT_CURSOR; }
                set { _OUT_CURSOR = value; }
            }
        }

        public class SELECT_MATERIALS_OF_BOM
        {
            private string _ARG_FACTORY;
            [DataType(Value = "VARCHAR2")]
            public string ARG_FACTORY
            {
                get { return _ARG_FACTORY; }
                set { _ARG_FACTORY = value; }
            }

            private string _ARG_CONCAT_WS_NO;
            [DataType(Value = "VARCHAR2")]
            public string ARG_CONCAT_WS_NO
            {
                get { return _ARG_CONCAT_WS_NO; }
                set { _ARG_CONCAT_WS_NO = value; }
            }

            private string _OUT_CURSOR;
            [DataType(Value = "REF CURSOR")]
            public string OUT_CURSOR
            {
                get { return _OUT_CURSOR; }
                set { _OUT_CURSOR = value; }
            }
        }

        public class SELECT_MATERIALS_TO_PUR
        {
            private string _ARG_FACTORY;
            [DataType(Value = "VARCHAR2")]
            public string ARG_FACTORY
            {
                get { return _ARG_FACTORY; }
                set { _ARG_FACTORY = value; }
            }

            private string _ARG_CONCAT_WS_NO;
            [DataType(Value = "VARCHAR2")]
            public string ARG_CONCAT_WS_NO
            {
                get { return _ARG_CONCAT_WS_NO; }
                set { _ARG_CONCAT_WS_NO = value; }
            }

            private string _ARG_LOCATION;
            [DataType(Value = "VARCHAR2")]
            public string ARG_LOCATION
            {
                get { return _ARG_LOCATION; }
                set { _ARG_LOCATION = value; }
            }

            private string _OUT_CURSOR;
            [DataType(Value = "REF CURSOR")]
            public string OUT_CURSOR
            {
                get { return _OUT_CURSOR; }
                set { _OUT_CURSOR = value; }
            }
        }

        public class SELECT_MAX_PART_SEQ
        {
            private string _ARG_FACTORY;
            [DataType(Value = "VARCHAR2")]
            public string ARG_FACTORY
            {
                get { return _ARG_FACTORY; }
                set { _ARG_FACTORY = value; }
            }

            private string _ARG_WS_NO;
            [DataType(Value = "VARCHAR2")]
            public string ARG_WS_NO
            {
                get { return _ARG_WS_NO; }
                set { _ARG_WS_NO = value; }
            }

            private string _OUT_CURSOR;
            [DataType(Value = "REF CURSOR")]
            public string OUT_CURSOR
            {
                get { return _OUT_CURSOR; }
                set { _OUT_CURSOR = value; }
            }
        }

        public class SELECT_MATERIAL_HISTORY
        {
            private string _ARG_FACTORY;
            [DataType(Value = "VARCHAR2")]
            public string ARG_FACTORY
            {
                get { return _ARG_FACTORY; }
                set { _ARG_FACTORY = value; }
            }

            private string _ARG_WS_NO;
            [DataType(Value = "VARCHAR2")]
            public string ARG_WS_NO
            {
                get { return _ARG_WS_NO; }
                set { _ARG_WS_NO = value; }
            }

            private string _ARG_PART_SEQ;
            [DataType(Value = "VARCHAR2")]
            public string ARG_PART_SEQ
            {
                get { return _ARG_PART_SEQ; }
                set { _ARG_PART_SEQ = value; }
            }

            private string _ARG_DELIMITER;
            [DataType(Value = "VARCHAR2")]
            public string ARG_DELIMITER
            {
                get { return _ARG_DELIMITER; }
                set { _ARG_DELIMITER = value; }
            }

            private string _OUT_CURSOR;
            [DataType(Value = "REF CURSOR")]
            public string OUT_CURSOR
            {
                get { return _OUT_CURSOR; }
                set { _OUT_CURSOR = value; }
            }
        }

        public class SELECT_WS_CONTENTS
        {
            private string _ARG_WORK_TYPE;
            [DataType(Value = "VARCHAR2")]
            public string ARG_WORK_TYPE
            {
                get { return _ARG_WORK_TYPE; }
                set { _ARG_WORK_TYPE = value; }
            }

            private string _ARG_FACTORY;
            [DataType(Value = "VARCHAR2")]
            public string ARG_FACTORY
            {
                get { return _ARG_FACTORY; }
                set { _ARG_FACTORY = value; }
            }

            private string _ARG_WS_NO;
            [DataType(Value = "VARCHAR2")]
            public string ARG_WS_NO
            {
                get { return _ARG_WS_NO; }
                set { _ARG_WS_NO = value; }
            }

            private string _OUT_CURSOR;
            [DataType(Value = "REF CURSOR")]
            public string OUT_CURSOR
            {
                get { return _OUT_CURSOR; }
                set { _OUT_CURSOR = value; }
            }
        }

        public class SELECT_MST_PROCESS
        {
            private string _ARG_DIV;
            [DataType(Value = "VARCHAR2")]
            public string ARG_DIV
            {
                get { return _ARG_DIV; }
                set { _ARG_DIV = value; }
            }

            private string _OUT_CURSOR;
            [DataType(Value = "REF CURSOR")]
            public string OUT_CURSOR
            {
                get { return _OUT_CURSOR; }
                set { _OUT_CURSOR = value; }
            }
        }

        public class SELECT_WS_BOTTOM_MTL
        {
            private string _ARG_WORK_TYPE;
            [DataType(Value = "VARCHAR2")]
            public string ARG_WORK_TYPE
            {
                get { return _ARG_WORK_TYPE; }
                set { _ARG_WORK_TYPE = value; }
            }

            private string _ARG_FACTORY;
            [DataType(Value = "VARCHAR2")]
            public string ARG_FACTORY
            {
                get { return _ARG_FACTORY; }
                set { _ARG_FACTORY = value; }
            }

            private string _ARG_WS_NO;
            [DataType(Value = "VARCHAR2")]
            public string ARG_WS_NO
            {
                get { return _ARG_WS_NO; }
                set { _ARG_WS_NO = value; }
            }

            private string _ARG_PART_TYPE;
            [DataType(Value = "VARCHAR2")]
            public string ARG_PART_TYPE
            {
                get { return _ARG_PART_TYPE; }
                set { _ARG_PART_TYPE = value; }
            }

            private string _OUT_CURSOR;
            [DataType(Value = "REF CURSOR")]
            public string OUT_CURSOR
            {
                get { return _OUT_CURSOR; }
                set { _OUT_CURSOR = value; }
            }
        }

        public class SELECT_WS_TMPLT_CONTENTS
        {
            private string _ARG_WORK_TYPE;
            [DataType(Value = "VARCHAR2")]
            public string ARG_WORK_TYPE
            {
                get { return _ARG_WORK_TYPE; }
                set { _ARG_WORK_TYPE = value; }
            }

            private string _ARG_KEY_VALUE;
            [DataType(Value = "VARCHAR2")]
            public string ARG_KEY_VALUE
            {
                get { return _ARG_KEY_VALUE; }
                set { _ARG_KEY_VALUE = value; }
            }

            private string _ARG_KEY_TYPE;
            [DataType(Value = "VARCHAR2")]
            public string ARG_KEY_TYPE
            {
                get { return _ARG_KEY_TYPE; }
                set { _ARG_KEY_TYPE = value; }
            }

            private string _ARG_FACTORY;
            [DataType(Value = "VARCHAR2")]
            public string ARG_FACTORY
            {
                get { return _ARG_FACTORY; }
                set { _ARG_FACTORY = value; }
            }

            private string _OUT_CURSOR;
            [DataType(Value = "REF CURSOR")]
            public string OUT_CURSOR
            {
                get { return _OUT_CURSOR; }
                set { _OUT_CURSOR = value; }
            }
        }

        public class SELECT_BOM_LIST_COMMON
        {
            private string _ARG_FACTORY;
            [DataType(Value = "VARCHAR2")]
            public string ARG_FACTORY
            {
                get { return _ARG_FACTORY; }
                set { _ARG_FACTORY = value; }
            }

            private string _ARG_WS_NO;
            [DataType(Value = "VARCHAR2")]
            public string ARG_WS_NO
            {
                get { return _ARG_WS_NO; }
                set { _ARG_WS_NO = value; }
            }

            private string _ARG_SEASON_CD;
            [DataType(Value = "VARCHAR2")]
            public string ARG_SEASON_CD
            {
                get { return _ARG_SEASON_CD; }
                set { _ARG_SEASON_CD = value; }
            }

            private string _ARG_ST_CD;
            [DataType(Value = "VARCHAR2")]
            public string ARG_ST_CD
            {
                get { return _ARG_ST_CD; }
                set { _ARG_ST_CD = value; }
            }

            private string _ARG_SUB_ST_CD;
            [DataType(Value = "VARCHAR2")]
            public string ARG_SUB_ST_CD
            {
                get { return _ARG_SUB_ST_CD; }
                set { _ARG_SUB_ST_CD = value; }
            }

            private string _ARG_PCC_PM;
            [DataType(Value = "VARCHAR2")]
            public string ARG_PCC_PM
            {
                get { return _ARG_PCC_PM; }
                set { _ARG_PCC_PM = value; }
            }

            private string _ARG_DEV_COLORWAY_ID;
            [DataType(Value = "VARCHAR2")]
            public string ARG_DEV_COLORWAY_ID
            {
                get { return _ARG_DEV_COLORWAY_ID; }
                set { _ARG_DEV_COLORWAY_ID = value; }
            }

            private string _ARG_STYLE_NAME;
            [DataType(Value = "VARCHAR2")]
            public string ARG_STYLE_NAME
            {
                get { return _ARG_STYLE_NAME; }
                set { _ARG_STYLE_NAME = value; }
            }

            private string _OUT_CURSOR;
            [DataType(Value = "REF CURSOR")]
            public string OUT_CURSOR
            {
                get { return _OUT_CURSOR; }
                set { _OUT_CURSOR = value; }
            }
        }

        public class SELECT_BOM_INFO_BY_CONFIRM
        {
            private string _ARG_WORK_TYPE;
            [DataType(Value = "VARCHAR2")]
            public string ARG_WORK_TYPE
            {
                get { return _ARG_WORK_TYPE; }
                set { _ARG_WORK_TYPE = value; }
            }

            private string _ARG_FACTORY;
            [DataType(Value = "VARCHAR2")]
            public string ARG_FACTORY
            {
                get { return _ARG_FACTORY; }
                set { _ARG_FACTORY = value; }
            }

            private string _ARG_WS_NO;
            [DataType(Value = "VARCHAR2")]
            public string ARG_WS_NO
            {
                get { return _ARG_WS_NO; }
                set { _ARG_WS_NO = value; }
            }

            private string _ARG_PART_SEQ;
            [DataType(Value = "VARCHAR2")]
            public string ARG_PART_SEQ
            {
                get { return _ARG_PART_SEQ; }
                set { _ARG_PART_SEQ = value; }
            }

            private string _OUT_CURSOR;
            [DataType(Value = "REF CURSOR")]
            public string OUT_CURSOR
            {
                get { return _OUT_CURSOR; }
                set { _OUT_CURSOR = value; }
            }
        }

        public class SELECT_PCC_MST_CODE
        {
            private string _ARG_WORK_TYPE;
            [DataType(Value = "VARCHAR2")]
            public string ARG_WORK_TYPE
            {
                get { return _ARG_WORK_TYPE; }
                set { _ARG_WORK_TYPE = value; }
            }

            private string _ARG_CODE;
            [DataType(Value = "VARCHAR2")]
            public string ARG_CODE
            {
                get { return _ARG_CODE; }
                set { _ARG_CODE = value; }
            }

            private string _ARG_CS_CODE;
            [DataType(Value = "VARCHAR2")]
            public string ARG_CS_CODE
            {
                get { return _ARG_CS_CODE; }
                set { _ARG_CS_CODE = value; }
            }

            private string _OUT_CURSOR;
            [DataType(Value = "REF CURSOR")]
            public string OUT_CURSOR
            {
                get { return _OUT_CURSOR; }
                set { _OUT_CURSOR = value; }
            }
        }

        public class SELECT_WS_STATUS_OP_CNT
        {
            private string _ARG_FACTORY;
            [DataType(Value = "VARCHAR2")]
            public string ARG_FACTORY
            {
                get { return _ARG_FACTORY; }
                set { _ARG_FACTORY = value; }
            }

            private string _ARG_WS_NO;
            [DataType(Value = "VARCHAR2")]
            public string ARG_WS_NO
            {
                get { return _ARG_WS_NO; }
                set { _ARG_WS_NO = value; }
            }

            private string _OUT_CURSOR;
            [DataType(Value = "REF CURSOR")]
            public string OUT_CURSOR
            {
                get { return _OUT_CURSOR; }
                set { _OUT_CURSOR = value; }
            }
        }

        public class SELECT_MATERIAL_INFO
        {
            private string _ARG_WORK_TYPE;
            [DataType(Value = "VARCHAR2")]
            public string ARG_WORK_TYPE
            {
                get { return _ARG_WORK_TYPE; }
                set { _ARG_WORK_TYPE = value; }
            }

            private string _ARG_MXSXL_NUMBER;
            [DataType(Value = "VARCHAR2")]
            public string ARG_MXSXL_NUMBER
            {
                get { return _ARG_MXSXL_NUMBER; }
                set { _ARG_MXSXL_NUMBER = value; }
            }

            private string _ARG_MAT_CD;
            [DataType(Value = "VARCHAR2")]
            public string ARG_MAT_CD
            {
                get { return _ARG_MAT_CD; }
                set { _ARG_MAT_CD = value; }
            }

            private string _ARG_CS_CD;
            [DataType(Value = "VARCHAR2")]
            public string ARG_CS_CD
            {
                get { return _ARG_CS_CD; }
                set { _ARG_CS_CD = value; }
            }

            private string _OUT_CURSOR;
            [DataType(Value = "REF CURSOR")]
            public string OUT_CURSOR
            {
                get { return _OUT_CURSOR; }
                set { _OUT_CURSOR = value; }
            }
        }

        public class SELECT_FAKE_BOM_HEAD
        {
            private string _ARG_FACTORY;
            [DataType(Value = "VARCHAR2")]
            public string ARG_FACTORY
            {
                get { return _ARG_FACTORY; }
                set { _ARG_FACTORY = value; }
            }

            private string _ARG_UPD_USER;
            [DataType(Value = "VARCHAR2")]
            public string ARG_UPD_USER
            {
                get { return _ARG_UPD_USER; }
                set { _ARG_UPD_USER = value; }
            }

            private string _OUT_CURSOR;
            [DataType(Value = "REF CURSOR")]
            public string OUT_CURSOR
            {
                get { return _OUT_CURSOR; }
                set { _OUT_CURSOR = value; }
            }
        }

        public class SELECT_FAKE_BOM_DATA
        {
            private string _ARG_WORK_TYPE;
            [DataType(Value = "VARCHAR2")]
            public string ARG_WORK_TYPE
            {
                get { return _ARG_WORK_TYPE; }
                set { _ARG_WORK_TYPE = value; }
            }

            private string _ARG_FACTORY;
            [DataType(Value = "VARCHAR2")]
            public string ARG_FACTORY
            {
                get { return _ARG_FACTORY; }
                set { _ARG_FACTORY = value; }
            }

            private string _ARG_WS_NO;
            [DataType(Value = "VARCHAR2")]
            public string ARG_WS_NO
            {
                get { return _ARG_WS_NO; }
                set { _ARG_WS_NO = value; }
            }

            private string _ARG_PART_TYPE;
            [DataType(Value = "VARCHAR2")]
            public string ARG_PART_TYPE
            {
                get { return _ARG_PART_TYPE; }
                set { _ARG_PART_TYPE = value; }
            }

            private string _OUT_CURSOR;
            [DataType(Value = "REF CURSOR")]
            public string OUT_CURSOR
            {
                get { return _OUT_CURSOR; }
                set { _OUT_CURSOR = value; }
            }
        }

        public class SELECT_WS_TEMPLATE_STYLE
        {
            private string _ARG_WORK_TYPE;
            [DataType(Value = "VARCHAR2")]
            public string ARG_WORK_TYPE
            {
                get { return _ARG_WORK_TYPE; }
                set { _ARG_WORK_TYPE = value; }
            }

            private string _ARG_FACTORY;
            [DataType(Value = "VARCHAR2")]
            public string ARG_FACTORY
            {
                get { return _ARG_FACTORY; }
                set { _ARG_FACTORY = value; }
            }

            private string _ARG_KEY_VALUE;
            [DataType(Value = "VARCHAR2")]
            public string ARG_KEY_VALUE
            {
                get { return _ARG_KEY_VALUE; }
                set { _ARG_KEY_VALUE = value; }
            }

            private string _ARG_KEY_TYPE;
            [DataType(Value = "VARCHAR2")]
            public string ARG_KEY_TYPE
            {
                get { return _ARG_KEY_TYPE; }
                set { _ARG_KEY_TYPE = value; }
            }

            private string _OUT_CURSOR;
            [DataType(Value = "REF CURSOR")]
            public string OUT_CURSOR
            {
                get { return _OUT_CURSOR; }
                set { _OUT_CURSOR = value; }
            }
        }

        public class SELECT_WORKSHOP_INFO
        {
            private string _ARG_WORK_TYPE;
            [DataType(Value = "VARCHAR2")]
            public string ARG_WORK_TYPE
            {
                get { return _ARG_WORK_TYPE; }
                set { _ARG_WORK_TYPE = value; }
            }

            private string _ARG_FACTORY;
            [DataType(Value = "VARCHAR2")]
            public string ARG_FACTORY
            {
                get { return _ARG_FACTORY; }
                set { _ARG_FACTORY = value; }
            }

            private string _ARG_WS_NO;
            [DataType(Value = "VARCHAR2")]
            public string ARG_WS_NO
            {
                get { return _ARG_WS_NO; }
                set { _ARG_WS_NO = value; }
            }

            private string _OUT_CURSOR;
            [DataType(Value = "REF CURSOR")]
            public string OUT_CURSOR
            {
                get { return _OUT_CURSOR; }
                set { _OUT_CURSOR = value; }
            }
        }

        public class SELECT_BOM_HEADER_REFRESH
        {
            private string _ARG_FACTORY;
            [DataType(Value = "VARCHAR2")]
            public string ARG_FACTORY
            {
                get { return _ARG_FACTORY; }
                set { _ARG_FACTORY = value; }
            }

            private string _ARG_WS_NO;
            [DataType(Value = "VARCHAR2")]
            public string ARG_WS_NO
            {
                get { return _ARG_WS_NO; }
                set { _ARG_WS_NO = value; }
            }

            private string _OUT_CURSOR;
            [DataType(Value = "REF CURSOR")]
            public string OUT_CURSOR
            {
                get { return _OUT_CURSOR; }
                set { _OUT_CURSOR = value; }
            }
        }

        public class SELECT_PART_DUPLICATE
        {
            private string _ARG_WORK_TYPE;
            [DataType(Value = "VARCHAR2")]
            public string ARG_WORK_TYPE
            {
                get { return _ARG_WORK_TYPE; }
                set { _ARG_WORK_TYPE = value; }
            }

            private string _ARG_FACTORY;
            [DataType(Value = "VARCHAR2")]
            public string ARG_FACTORY
            {
                get { return _ARG_FACTORY; }
                set { _ARG_FACTORY = value; }
            }

            private string _ARG_WS_NO;
            [DataType(Value = "VARCHAR2")]
            public string ARG_WS_NO
            {
                get { return _ARG_WS_NO; }
                set { _ARG_WS_NO = value; }
            }

            private string _OUT_CURSOR;
            [DataType(Value = "REF CURSOR")]
            public string OUT_CURSOR
            {
                get { return _OUT_CURSOR; }
                set { _OUT_CURSOR = value; }
            }
        }

        public class SELECT_NEW_MUL_EDIT_SOURCE
        {
            private string _ARG_WORK_TYPE;
            [DataType(Value = "VARCHAR2")]
            public string ARG_WORK_TYPE
            {
                get { return _ARG_WORK_TYPE; }
                set { _ARG_WORK_TYPE = value; }
            }

            private string _ARG_FACTORY;
            [DataType(Value = "VARCHAR2")]
            public string ARG_FACTORY
            {
                get { return _ARG_FACTORY; }
                set { _ARG_FACTORY = value; }
            }

            private string _ARG_WS_NO;
            [DataType(Value = "VARCHAR2")]
            public string ARG_WS_NO
            {
                get { return _ARG_WS_NO; }
                set { _ARG_WS_NO = value; }
            }

            private string _OUT_CURSOR;
            [DataType(Value = "REF CURSOR")]
            public string OUT_CURSOR
            {
                get { return _OUT_CURSOR; }
                set { _OUT_CURSOR = value; }
            }
        }

        public class SELECT_REQ_ON_SITE
        {
            private string _ARG_WORK_TYPE;
            [DataType(Value = "VARCHAR2")]
            public string ARG_WORK_TYPE
            {
                get { return _ARG_WORK_TYPE; }
                set { _ARG_WORK_TYPE = value; }
            }

            private string _ARG_FACTORY;
            [DataType(Value = "VARCHAR2")]
            public string ARG_FACTORY
            {
                get { return _ARG_FACTORY; }
                set { _ARG_FACTORY = value; }
            }

            private string _ARG_WS_NO;
            [DataType(Value = "VARCHAR2")]
            public string ARG_WS_NO
            {
                get { return _ARG_WS_NO; }
                set { _ARG_WS_NO = value; }
            }

            private string _OUT_CURSOR;
            [DataType(Value = "REF CURSOR")]
            public string OUT_CURSOR
            {
                get { return _OUT_CURSOR; }
                set { _OUT_CURSOR = value; }
            }
        }

        public class SELECT_BOM_CONFIRM_VLDTN
        {
            private string _ARG_WORK_TYPE;
            [DataType(Value = "VARCHAR2")]
            public string ARG_WORK_TYPE
            {
                get { return _ARG_WORK_TYPE; }
                set { _ARG_WORK_TYPE = value; }
            }

            private string _ARG_FACTORY;
            [DataType(Value = "VARCHAR2")]
            public string ARG_FACTORY
            {
                get { return _ARG_FACTORY; }
                set { _ARG_FACTORY = value; }
            }

            private string _ARG_WS_NO;
            [DataType(Value = "VARCHAR2")]
            public string ARG_WS_NO
            {
                get { return _ARG_WS_NO; }
                set { _ARG_WS_NO = value; }
            }

            private string _OUT_CURSOR;
            [DataType(Value = "REF CURSOR")]
            public string OUT_CURSOR
            {
                get { return _OUT_CURSOR; }
                set { _OUT_CURSOR = value; }
            }
        }

        public class SELECT_NCF_RULE_PASS
        {
            private string _ARG_WORK_TYPE;
            [DataType(Value = "VARCHAR2")]
            public string ARG_WORK_TYPE
            {
                get { return _ARG_WORK_TYPE; }
                set { _ARG_WORK_TYPE = value; }
            }

            private string _ARG_FACTORY;
            [DataType(Value = "VARCHAR2")]
            public string ARG_FACTORY
            {
                get { return _ARG_FACTORY; }
                set { _ARG_FACTORY = value; }
            }

            private string _ARG_WS_NO;
            [DataType(Value = "VARCHAR2")]
            public string ARG_WS_NO
            {
                get { return _ARG_WS_NO; }
                set { _ARG_WS_NO = value; }
            }

            private string _OUT_CURSOR;
            [DataType(Value = "REF CURSOR")]
            public string OUT_CURSOR
            {
                get { return _OUT_CURSOR; }
                set { _OUT_CURSOR = value; }
            }
        }

        public class SELECT_TRNS_RECORD
        {
            private string _ARG_WORK_TYPE;
            [DataType(Value = "VARCHAR2")]
            public string ARG_WORK_TYPE
            {
                get { return _ARG_WORK_TYPE; }
                set { _ARG_WORK_TYPE = value; }
            }

            private string _ARG_FACTORY;
            [DataType(Value = "VARCHAR2")]
            public string ARG_FACTORY
            {
                get { return _ARG_FACTORY; }
                set { _ARG_FACTORY = value; }
            }

            private string _ARG_WS_NO;
            [DataType(Value = "VARCHAR2")]
            public string ARG_WS_NO
            {
                get { return _ARG_WS_NO; }
                set { _ARG_WS_NO = value; }
            }

            private string _OUT_CURSOR;
            [DataType(Value = "REF CURSOR")]
            public string OUT_CURSOR
            {
                get { return _OUT_CURSOR; }
                set { _OUT_CURSOR = value; }
            }
        }

        public class INSERT_BOM_HEAD_JSON
        {
            private string _ARG_FACTORY;
            [DataType(Value = "VARCHAR2")]
            public string ARG_FACTORY
            {
                get { return _ARG_FACTORY; }
                set { _ARG_FACTORY = value; }
            }

            private string _ARG_WS_NO;
            [DataType(Value = "VARCHAR2")]
            public string ARG_WS_NO
            {
                get { return _ARG_WS_NO; }
                set { _ARG_WS_NO = value; }
            }

            private string _ARG_DEV_NAME;
            [DataType(Value = "VARCHAR2")]
            public string ARG_DEV_NAME
            {
                get { return _ARG_DEV_NAME; }
                set { _ARG_DEV_NAME = value; }
            }

            private string _ARG_PRODUCT_CODE;
            [DataType(Value = "VARCHAR2")]
            public string ARG_PRODUCT_CODE
            {
                get { return _ARG_PRODUCT_CODE; }
                set { _ARG_PRODUCT_CODE = value; }
            }

            private string _ARG_PROD_FACTORY;
            [DataType(Value = "VARCHAR2")]
            public string ARG_PROD_FACTORY
            {
                get { return _ARG_PROD_FACTORY; }
                set { _ARG_PROD_FACTORY = value; }
            }

            private string _ARG_GENDER;
            [DataType(Value = "VARCHAR2")]
            public string ARG_GENDER
            {
                get { return _ARG_GENDER; }
                set { _ARG_GENDER = value; }
            }

            private string _ARG_COLOR_VER;
            [DataType(Value = "VARCHAR2")]
            public string ARG_COLOR_VER
            {
                get { return _ARG_COLOR_VER; }
                set { _ARG_COLOR_VER = value; }
            }

            private string _ARG_MODEL_ID;
            [DataType(Value = "VARCHAR2")]
            public string ARG_MODEL_ID
            {
                get { return _ARG_MODEL_ID; }
                set { _ARG_MODEL_ID = value; }
            }

            private string _ARG_PCC_PM;
            [DataType(Value = "VARCHAR2")]
            public string ARG_PCC_PM
            {
                get { return _ARG_PCC_PM; }
                set { _ARG_PCC_PM = value; }
            }

            private string _ARG_DEV_STYLE_ID;
            [DataType(Value = "VARCHAR2")]
            public string ARG_DEV_STYLE_ID
            {
                get { return _ARG_DEV_STYLE_ID; }
                set { _ARG_DEV_STYLE_ID = value; }
            }

            private string _ARG_DEV_COLORWAY_ID;
            [DataType(Value = "VARCHAR2")]
            public string ARG_DEV_COLORWAY_ID
            {
                get { return _ARG_DEV_COLORWAY_ID; }
                set { _ARG_DEV_COLORWAY_ID = value; }
            }

            private string _ARG_PRODUCT_ID;
            [DataType(Value = "VARCHAR2")]
            public string ARG_PRODUCT_ID
            {
                get { return _ARG_PRODUCT_ID; }
                set { _ARG_PRODUCT_ID = value; }
            }

            private string _ARG_STYLE_NUMBER;
            [DataType(Value = "VARCHAR2")]
            public string ARG_STYLE_NUMBER
            {
                get { return _ARG_STYLE_NUMBER; }
                set { _ARG_STYLE_NUMBER = value; }
            }

            private string _ARG_SOURCING_CONFIG_ID;
            [DataType(Value = "VARCHAR2")]
            public string ARG_SOURCING_CONFIG_ID
            {
                get { return _ARG_SOURCING_CONFIG_ID; }
                set { _ARG_SOURCING_CONFIG_ID = value; }
            }

            private string _ARG_PCX_BOM_ID;
            [DataType(Value = "VARCHAR2")]
            public string ARG_PCX_BOM_ID
            {
                get { return _ARG_PCX_BOM_ID; }
                set { _ARG_PCX_BOM_ID = value; }
            }

            private string _ARG_DEV_STYLE_TYPE_ID;
            [DataType(Value = "VARCHAR2")]
            public string ARG_DEV_STYLE_TYPE_ID
            {
                get { return _ARG_DEV_STYLE_TYPE_ID; }
                set { _ARG_DEV_STYLE_TYPE_ID = value; }
            }

            private string _ARG_LOGIC_BOM_STATE_ID;
            [DataType(Value = "VARCHAR2")]
            public string ARG_LOGIC_BOM_STATE_ID
            {
                get { return _ARG_LOGIC_BOM_STATE_ID; }
                set { _ARG_LOGIC_BOM_STATE_ID = value; }
            }

            private string _ARG_LOGIC_BOM_GATE_ID;
            [DataType(Value = "VARCHAR2")]
            public string ARG_LOGIC_BOM_GATE_ID
            {
                get { return _ARG_LOGIC_BOM_GATE_ID; }
                set { _ARG_LOGIC_BOM_GATE_ID = value; }
            }

            private string _ARG_CYCLE_YEAR;
            [DataType(Value = "VARCHAR2")]
            public string ARG_CYCLE_YEAR
            {
                get { return _ARG_CYCLE_YEAR; }
                set { _ARG_CYCLE_YEAR = value; }
            }

            private string _ARG_LOGIC_BOM_YN;
            [DataType(Value = "VARCHAR2")]
            public string ARG_LOGIC_BOM_YN
            {
                get { return _ARG_LOGIC_BOM_YN; }
                set { _ARG_LOGIC_BOM_YN = value; }
            }

            private string _ARG_BOM_PART_UUID;
            [DataType(Value = "VARCHAR2")]
            public string ARG_BOM_PART_UUID
            {
                get { return _ARG_BOM_PART_UUID; }
                set { _ARG_BOM_PART_UUID = value; }
            }

            private string _ARG_SEASON_ID;
            [DataType(Value = "VARCHAR2")]
            public string ARG_SEASON_ID
            {
                get { return _ARG_SEASON_ID; }
                set { _ARG_SEASON_ID = value; }
            }
        }

        public class INSERT_BOM_HEAD_JSON_ORG
        {
            private string _ARG_FACTORY;
            [DataType(Value = "VARCHAR2")]
            public string ARG_FACTORY
            {
                get { return _ARG_FACTORY; }
                set { _ARG_FACTORY = value; }
            }

            private string _ARG_WS_NO;
            [DataType(Value = "VARCHAR2")]
            public string ARG_WS_NO
            {
                get { return _ARG_WS_NO; }
                set { _ARG_WS_NO = value; }
            }

            private string _ARG_OBJ_ID;
            [DataType(Value = "VARCHAR2")]
            public string ARG_OBJ_ID
            {
                get { return _ARG_OBJ_ID; }
                set { _ARG_OBJ_ID = value; }
            }

            private string _ARG_OBJ_TYPE;
            [DataType(Value = "VARCHAR2")]
            public string ARG_OBJ_TYPE
            {
                get { return _ARG_OBJ_TYPE; }
                set { _ARG_OBJ_TYPE = value; }
            }

            private string _ARG_BOM_CONTRACT_VER;
            [DataType(Value = "VARCHAR2")]
            public string ARG_BOM_CONTRACT_VER
            {
                get { return _ARG_BOM_CONTRACT_VER; }
                set { _ARG_BOM_CONTRACT_VER = value; }
            }

            private string _ARG_DEV_STYLE_ID;
            [DataType(Value = "VARCHAR2")]
            public string ARG_DEV_STYLE_ID
            {
                get { return _ARG_DEV_STYLE_ID; }
                set { _ARG_DEV_STYLE_ID = value; }
            }

            private string _ARG_DEV_COLORWAY_ID;
            [DataType(Value = "VARCHAR2")]
            public string ARG_DEV_COLORWAY_ID
            {
                get { return _ARG_DEV_COLORWAY_ID; }
                set { _ARG_DEV_COLORWAY_ID = value; }
            }

            private string _ARG_COLORWAY_NAME;
            [DataType(Value = "VARCHAR2")]
            public string ARG_COLORWAY_NAME
            {
                get { return _ARG_COLORWAY_NAME; }
                set { _ARG_COLORWAY_NAME = value; }
            }

            private string _ARG_SRC_CONFIG_ID;
            [DataType(Value = "VARCHAR2")]
            public string ARG_SRC_CONFIG_ID
            {
                get { return _ARG_SRC_CONFIG_ID; }
                set { _ARG_SRC_CONFIG_ID = value; }
            }

            private string _ARG_SRC_CONFIG_NAME;
            [DataType(Value = "VARCHAR2")]
            public string ARG_SRC_CONFIG_NAME
            {
                get { return _ARG_SRC_CONFIG_NAME; }
                set { _ARG_SRC_CONFIG_NAME = value; }
            }

            private string _ARG_BOM_ID;
            [DataType(Value = "VARCHAR2")]
            public string ARG_BOM_ID
            {
                get { return _ARG_BOM_ID; }
                set { _ARG_BOM_ID = value; }
            }

            private string _ARG_BOM_NAME;
            [DataType(Value = "VARCHAR2")]
            public string ARG_BOM_NAME
            {
                get { return _ARG_BOM_NAME; }
                set { _ARG_BOM_NAME = value; }
            }

            private string _ARG_BOM_VERSION_NUM;
            [DataType(Value = "VARCHAR2")]
            public string ARG_BOM_VERSION_NUM
            {
                get { return _ARG_BOM_VERSION_NUM; }
                set { _ARG_BOM_VERSION_NUM = value; }
            }

            private string _ARG_BOM_DESC;
            [DataType(Value = "VARCHAR2")]
            public string ARG_BOM_DESC
            {
                get { return _ARG_BOM_DESC; }
                set { _ARG_BOM_DESC = value; }
            }

            private string _ARG_BOM_COMMENTS;
            [DataType(Value = "VARCHAR2")]
            public string ARG_BOM_COMMENTS
            {
                get { return _ARG_BOM_COMMENTS; }
                set { _ARG_BOM_COMMENTS = value; }
            }

            private string _ARG_BOM_STATUS_IND;
            [DataType(Value = "VARCHAR2")]
            public string ARG_BOM_STATUS_IND
            {
                get { return _ARG_BOM_STATUS_IND; }
                set { _ARG_BOM_STATUS_IND = value; }
            }

            private string _ARG_CREATE_TIME_STAMP;
            [DataType(Value = "VARCHAR2")]
            public string ARG_CREATE_TIME_STAMP
            {
                get { return _ARG_CREATE_TIME_STAMP; }
                set { _ARG_CREATE_TIME_STAMP = value; }
            }

            private string _ARG_CHANGE_TIME_STAMP;
            [DataType(Value = "VARCHAR2")]
            public string ARG_CHANGE_TIME_STAMP
            {
                get { return _ARG_CHANGE_TIME_STAMP; }
                set { _ARG_CHANGE_TIME_STAMP = value; }
            }

            private string _ARG_CREATED_BY;
            [DataType(Value = "VARCHAR2")]
            public string ARG_CREATED_BY
            {
                get { return _ARG_CREATED_BY; }
                set { _ARG_CREATED_BY = value; }
            }

            private string _ARG_MODIFIED_BY;
            [DataType(Value = "VARCHAR2")]
            public string ARG_MODIFIED_BY
            {
                get { return _ARG_MODIFIED_BY; }
                set { _ARG_MODIFIED_BY = value; }
            }

            private string _ARG_STYLE_NUMBER;
            [DataType(Value = "VARCHAR2")]
            public string ARG_STYLE_NUMBER
            {
                get { return _ARG_STYLE_NUMBER; }
                set { _ARG_STYLE_NUMBER = value; }
            }

            private string _ARG_STYLE_NAME;
            [DataType(Value = "VARCHAR2")]
            public string ARG_STYLE_NAME
            {
                get { return _ARG_STYLE_NAME; }
                set { _ARG_STYLE_NAME = value; }
            }

            private string _ARG_MODEL_ID;
            [DataType(Value = "VARCHAR2")]
            public string ARG_MODEL_ID
            {
                get { return _ARG_MODEL_ID; }
                set { _ARG_MODEL_ID = value; }
            }

            private string _ARG_GENDER;
            [DataType(Value = "VARCHAR2")]
            public string ARG_GENDER
            {
                get { return _ARG_GENDER; }
                set { _ARG_GENDER = value; }
            }

            private string _ARG_AGE;
            [DataType(Value = "VARCHAR2")]
            public string ARG_AGE
            {
                get { return _ARG_AGE; }
                set { _ARG_AGE = value; }
            }

            private string _ARG_PRODUCT_ID;
            [DataType(Value = "VARCHAR2")]
            public string ARG_PRODUCT_ID
            {
                get { return _ARG_PRODUCT_ID; }
                set { _ARG_PRODUCT_ID = value; }
            }

            private string _ARG_PRODUCT_CODE;
            [DataType(Value = "VARCHAR2")]
            public string ARG_PRODUCT_CODE
            {
                get { return _ARG_PRODUCT_CODE; }
                set { _ARG_PRODUCT_CODE = value; }
            }

            private string _ARG_COLORWAY_CODE;
            [DataType(Value = "VARCHAR2")]
            public string ARG_COLORWAY_CODE
            {
                get { return _ARG_COLORWAY_CODE; }
                set { _ARG_COLORWAY_CODE = value; }
            }

            private string _ARG_DEV_STYLE_TYPE_ID;
            [DataType(Value = "VARCHAR2")]
            public string ARG_DEV_STYLE_TYPE_ID
            {
                get { return _ARG_DEV_STYLE_TYPE_ID; }
                set { _ARG_DEV_STYLE_TYPE_ID = value; }
            }

            private string _ARG_LOGIC_BOM_STATE_ID;
            [DataType(Value = "VARCHAR2")]
            public string ARG_LOGIC_BOM_STATE_ID
            {
                get { return _ARG_LOGIC_BOM_STATE_ID; }
                set { _ARG_LOGIC_BOM_STATE_ID = value; }
            }

            private string _ARG_LOGIC_BOM_GATE_ID;
            [DataType(Value = "VARCHAR2")]
            public string ARG_LOGIC_BOM_GATE_ID
            {
                get { return _ARG_LOGIC_BOM_GATE_ID; }
                set { _ARG_LOGIC_BOM_GATE_ID = value; }
            }

            private string _ARG_CYCLE_YEAR;
            [DataType(Value = "VARCHAR2")]
            public string ARG_CYCLE_YEAR
            {
                get { return _ARG_CYCLE_YEAR; }
                set { _ARG_CYCLE_YEAR = value; }
            }

            private string _ARG_BOM_PART_UUID;
            [DataType(Value = "VARCHAR2")]
            public string ARG_BOM_PART_UUID
            {
                get { return _ARG_BOM_PART_UUID; }
                set { _ARG_BOM_PART_UUID = value; }
            }

            private string _ARG_SEASON_ID;
            [DataType(Value = "VARCHAR2")]
            public string ARG_SEASON_ID
            {
                get { return _ARG_SEASON_ID; }
                set { _ARG_SEASON_ID = value; }
            }
        }

        public class INSERT_BOM_HEAD_EXCEL
        {
            private string _ARG_FACTORY;
            [DataType(Value = "VARCHAR2")]
            public string ARG_FACTORY
            {
                get { return _ARG_FACTORY; }
                set { _ARG_FACTORY = value; }
            }

            private string _ARG_WS_NO;
            [DataType(Value = "VARCHAR2")]
            public string ARG_WS_NO
            {
                get { return _ARG_WS_NO; }
                set { _ARG_WS_NO = value; }
            }

            private string _ARG_PCC_PM;
            [DataType(Value = "VARCHAR2")]
            public string ARG_PCC_PM
            {
                get { return _ARG_PCC_PM; }
                set { _ARG_PCC_PM = value; }
            }
        }

        public class INSERT_BOM_HEAD_XML
        {
            private string _ARG_FACTORY;
            [DataType(Value = "VARCHAR2")]
            public string ARG_FACTORY
            {
                get { return _ARG_FACTORY; }
                set { _ARG_FACTORY = value; }
            }

            private string _ARG_WS_NO;
            [DataType(Value = "VARCHAR2")]
            public string ARG_WS_NO
            {
                get { return _ARG_WS_NO; }
                set { _ARG_WS_NO = value; }
            }

            private string _ARG_DPA;
            [DataType(Value = "VARCHAR2")]
            public string ARG_DPA
            {
                get { return _ARG_DPA; }
                set { _ARG_DPA = value; }
            }

            private string _ARG_BOM_ID;
            [DataType(Value = "VARCHAR2")]
            public string ARG_BOM_ID
            {
                get { return _ARG_BOM_ID; }
                set { _ARG_BOM_ID = value; }
            }

            private string _ARG_SEASON_CD;
            [DataType(Value = "VARCHAR2")]
            public string ARG_SEASON_CD
            {
                get { return _ARG_SEASON_CD; }
                set { _ARG_SEASON_CD = value; }
            }

            private string _ARG_DEV_NAME;
            [DataType(Value = "VARCHAR2")]
            public string ARG_DEV_NAME
            {
                get { return _ARG_DEV_NAME; }
                set { _ARG_DEV_NAME = value; }
            }

            private string _ARG_PRODUCT_CODE;
            [DataType(Value = "VARCHAR2")]
            public string ARG_PRODUCT_CODE
            {
                get { return _ARG_PRODUCT_CODE; }
                set { _ARG_PRODUCT_CODE = value; }
            }

            private string _ARG_PROD_FACTORY;
            [DataType(Value = "VARCHAR2")]
            public string ARG_PROD_FACTORY
            {
                get { return _ARG_PROD_FACTORY; }
                set { _ARG_PROD_FACTORY = value; }
            }

            private string _ARG_TD;
            [DataType(Value = "VARCHAR2")]
            public string ARG_TD
            {
                get { return _ARG_TD; }
                set { _ARG_TD = value; }
            }

            private string _ARG_COLOR_VER;
            [DataType(Value = "VARCHAR2")]
            public string ARG_COLOR_VER
            {
                get { return _ARG_COLOR_VER; }
                set { _ARG_COLOR_VER = value; }
            }

            private string _ARG_MODEL_ID;
            [DataType(Value = "VARCHAR2")]
            public string ARG_MODEL_ID
            {
                get { return _ARG_MODEL_ID; }
                set { _ARG_MODEL_ID = value; }
            }

            private string _ARG_LAST_CD;
            [DataType(Value = "VARCHAR2")]
            public string ARG_LAST_CD
            {
                get { return _ARG_LAST_CD; }
                set { _ARG_LAST_CD = value; }
            }

            private string _ARG_PCC_PM;
            [DataType(Value = "VARCHAR2")]
            public string ARG_PCC_PM
            {
                get { return _ARG_PCC_PM; }
                set { _ARG_PCC_PM = value; }
            }

            private string _ARG_WHQ_DEV;
            [DataType(Value = "VARCHAR2")]
            public string ARG_WHQ_DEV
            {
                get { return _ARG_WHQ_DEV; }
                set { _ARG_WHQ_DEV = value; }
            }

            private string _ARG_IPW;
            [DataType(Value = "VARCHAR2")]
            public string ARG_IPW
            {
                get { return _ARG_IPW; }
                set { _ARG_IPW = value; }
            }

            private string _ARG_PRODUCT_ID;
            [DataType(Value = "VARCHAR2")]
            public string ARG_PRODUCT_ID
            {
                get { return _ARG_PRODUCT_ID; }
                set { _ARG_PRODUCT_ID = value; }
            }
        }

        public class INSERT_BOM_TAIN_JSON
        {
            private string _ARG_FACTORY;
            [DataType(Value = "VARCHAR2")]
            public string ARG_FACTORY
            {
                get { return _ARG_FACTORY; }
                set { _ARG_FACTORY = value; }
            }

            private string _ARG_WS_NO;
            [DataType(Value = "VARCHAR2")]
            public string ARG_WS_NO
            {
                get { return _ARG_WS_NO; }
                set { _ARG_WS_NO = value; }
            }

            private string _ARG_PART_SEQ;
            [DataType(Value = "VARCHAR2")]
            public string ARG_PART_SEQ
            {
                get { return _ARG_PART_SEQ; }
                set { _ARG_PART_SEQ = value; }
            }

            private string _ARG_PART_NO;
            [DataType(Value = "VARCHAR2")]
            public string ARG_PART_NO
            {
                get { return _ARG_PART_NO; }
                set { _ARG_PART_NO = value; }
            }

            private string _ARG_PART_CD;
            [DataType(Value = "VARCHAR2")]
            public string ARG_PART_CD
            {
                get { return _ARG_PART_CD; }
                set { _ARG_PART_CD = value; }
            }

            private string _ARG_PART_NAME;
            [DataType(Value = "VARCHAR2")]
            public string ARG_PART_NAME
            {
                get { return _ARG_PART_NAME; }
                set { _ARG_PART_NAME = value; }
            }

            private string _ARG_PART_TYPE;
            [DataType(Value = "VARCHAR2")]
            public string ARG_PART_TYPE
            {
                get { return _ARG_PART_TYPE; }
                set { _ARG_PART_TYPE = value; }
            }

            private string _MXSXL_NUMBER;
            [DataType(Value = "VARCHAR2")]
            public string MXSXL_NUMBER
            {
                get { return _MXSXL_NUMBER; }
                set { _MXSXL_NUMBER = value; }
            }

            private string _ARG_MAT_CD;
            [DataType(Value = "VARCHAR2")]
            public string ARG_MAT_CD
            {
                get { return _ARG_MAT_CD; }
                set { _ARG_MAT_CD = value; }
            }

            private string _ARG_MAT_NAME;
            [DataType(Value = "VARCHAR2")]
            public string ARG_MAT_NAME
            {
                get { return _ARG_MAT_NAME; }
                set { _ARG_MAT_NAME = value; }
            }

            private string _ARG_MAT_COMMENTS;
            [DataType(Value = "VARCHAR2")]
            public string ARG_MAT_COMMENTS
            {
                get { return _ARG_MAT_COMMENTS; }
                set { _ARG_MAT_COMMENTS = value; }
            }

            private string _ARG_MCS_NUMBER;
            [DataType(Value = "VARCHAR2")]
            public string ARG_MCS_NUMBER
            {
                get { return _ARG_MCS_NUMBER; }
                set { _ARG_MCS_NUMBER = value; }
            }

            private string _ARG_NIKE_COMMENT;
            [DataType(Value = "VARCHAR2")]
            public string ARG_NIKE_COMMENT
            {
                get { return _ARG_NIKE_COMMENT; }
                set { _ARG_NIKE_COMMENT = value; }
            }

            private string _ARG_COLOR_CD;
            [DataType(Value = "VARCHAR2")]
            public string ARG_COLOR_CD
            {
                get { return _ARG_COLOR_CD; }
                set { _ARG_COLOR_CD = value; }
            }

            private string _ARG_COLOR_NAME;
            [DataType(Value = "VARCHAR2")]
            public string ARG_COLOR_NAME
            {
                get { return _ARG_COLOR_NAME; }
                set { _ARG_COLOR_NAME = value; }
            }

            private string _ARG_COLOR_COMMENTS;
            [DataType(Value = "VARCHAR2")]
            public string ARG_COLOR_COMMENTS
            {
                get { return _ARG_COLOR_COMMENTS; }
                set { _ARG_COLOR_COMMENTS = value; }
            }

            private string _ARG_SORT_NO;
            [DataType(Value = "VARCHAR2")]
            public string ARG_SORT_NO
            {
                get { return _ARG_SORT_NO; }
                set { _ARG_SORT_NO = value; }
            }

            private string _ARG_UPD_USER;
            [DataType(Value = "VARCHAR2")]
            public string ARG_UPD_USER
            {
                get { return _ARG_UPD_USER; }
                set { _ARG_UPD_USER = value; }
            }

            private string _ARG_PTRN_PART_NAME;
            [DataType(Value = "VARCHAR2")]
            public string ARG_PTRN_PART_NAME
            {
                get { return _ARG_PTRN_PART_NAME; }
                set { _ARG_PTRN_PART_NAME = value; }
            }

            private string _ARG_PCX_SUPP_MAT_ID;
            [DataType(Value = "VARCHAR2")]
            public string ARG_PCX_SUPP_MAT_ID
            {
                get { return _ARG_PCX_SUPP_MAT_ID; }
                set { _ARG_PCX_SUPP_MAT_ID = value; }
            }

            private string _ARG_PCX_COLOR_ID;
            [DataType(Value = "VARCHAR2")]
            public string ARG_PCX_COLOR_ID
            {
                get { return _ARG_PCX_COLOR_ID; }
                set { _ARG_PCX_COLOR_ID = value; }
            }

            private string _ARG_VENDOR_NAME;
            [DataType(Value = "VARCHAR2")]
            public string ARG_VENDOR_NAME
            {
                get { return _ARG_VENDOR_NAME; }
                set { _ARG_VENDOR_NAME = value; }
            }

            private string _ARG_PCX_MAT_ID;
            [DataType(Value = "VARCHAR2")]
            public string ARG_PCX_MAT_ID
            {
                get { return _ARG_PCX_MAT_ID; }
                set { _ARG_PCX_MAT_ID = value; }
            }

            private string _ARG_PTRN_PART_CD;
            [DataType(Value = "VARCHAR2")]
            public string ARG_PTRN_PART_CD
            {
                get { return _ARG_PTRN_PART_CD; }
                set { _ARG_PTRN_PART_CD = value; }
            }

            private string _ARG_CS_CD;
            [DataType(Value = "VARCHAR2")]
            public string ARG_CS_CD
            {
                get { return _ARG_CS_CD; }
                set { _ARG_CS_CD = value; }
            }

            private string _ARG_MDSL_CHK;
            [DataType(Value = "VARCHAR2")]
            public string ARG_MDSL_CHK
            {
                get { return _ARG_MDSL_CHK; }
                set { _ARG_MDSL_CHK = value; }
            }

            private string _ARG_OTSL_CHK;
            [DataType(Value = "VARCHAR2")]
            public string ARG_OTSL_CHK
            {
                get { return _ARG_OTSL_CHK; }
                set { _ARG_OTSL_CHK = value; }
            }

            private string _ARG_CS_PTRN_NAME;
            [DataType(Value = "VARCHAR2")]
            public string ARG_CS_PTRN_NAME
            {
                get { return _ARG_CS_PTRN_NAME; }
                set { _ARG_CS_PTRN_NAME = value; }
            }

            private string _ARG_CS_PTRN_CD;
            [DataType(Value = "VARCHAR2")]
            public string ARG_CS_PTRN_CD
            {
                get { return _ARG_CS_PTRN_CD; }
                set { _ARG_CS_PTRN_CD = value; }
            }

            private string _ARG_LOGIC_GROUP;
            [DataType(Value = "VARCHAR2")]
            public string ARG_LOGIC_GROUP
            {
                get { return _ARG_LOGIC_GROUP; }
                set { _ARG_LOGIC_GROUP = value; }
            }

            private double _ARG_MAT_FORECAST_PRCNT;
            [DataType(Value = "NUMBER")]
            public double ARG_MAT_FORECAST_PRCNT
            {
                get { return _ARG_MAT_FORECAST_PRCNT; }
                set { _ARG_MAT_FORECAST_PRCNT = value; }
            }

            private double _ARG_COLOR_FORECAST_PRCNT;
            [DataType(Value = "NUMBER")]
            public double ARG_COLOR_FORECAST_PRCNT
            {
                get { return _ARG_COLOR_FORECAST_PRCNT; }
                set { _ARG_COLOR_FORECAST_PRCNT = value; }
            }
        }

        public class INSERT_BOM_TAIN_JSON_ORG
        {
            private string _ARG_FACTORY;
            [DataType(Value = "VARCHAR2")]
            public string ARG_FACTORY
            {
                get { return _ARG_FACTORY; }
                set { _ARG_FACTORY = value; }
            }

            private string _ARG_WS_NO;
            [DataType(Value = "VARCHAR2")]
            public string ARG_WS_NO
            {
                get { return _ARG_WS_NO; }
                set { _ARG_WS_NO = value; }
            }

            private string _ARG_BOM_LINE_SORT_SEQ;
            [DataType(Value = "VARCHAR2")]
            public string ARG_BOM_LINE_SORT_SEQ
            {
                get { return _ARG_BOM_LINE_SORT_SEQ; }
                set { _ARG_BOM_LINE_SORT_SEQ = value; }
            }

            private string _ARG_BOM_SECTION_ID;
            [DataType(Value = "VARCHAR2")]
            public string ARG_BOM_SECTION_ID
            {
                get { return _ARG_BOM_SECTION_ID; }
                set { _ARG_BOM_SECTION_ID = value; }
            }

            private string _ARG_PART_NAME_ID;
            [DataType(Value = "VARCHAR2")]
            public string ARG_PART_NAME_ID
            {
                get { return _ARG_PART_NAME_ID; }
                set { _ARG_PART_NAME_ID = value; }
            }

            private string _ARG_PTRN_PART_ID;
            [DataType(Value = "VARCHAR2")]
            public string ARG_PTRN_PART_ID
            {
                get { return _ARG_PTRN_PART_ID; }
                set { _ARG_PTRN_PART_ID = value; }
            }

            private string _ARG_SUPP_MAT_ID;
            [DataType(Value = "VARCHAR2")]
            public string ARG_SUPP_MAT_ID
            {
                get { return _ARG_SUPP_MAT_ID; }
                set { _ARG_SUPP_MAT_ID = value; }
            }

            private string _ARG_MAT_ITEM_ID;
            [DataType(Value = "VARCHAR2")]
            public string ARG_MAT_ITEM_ID
            {
                get { return _ARG_MAT_ITEM_ID; }
                set { _ARG_MAT_ITEM_ID = value; }
            }

            private string _ARG_MAT_ITEM_PLHDR_DESC;
            [DataType(Value = "VARCHAR2")]
            public string ARG_MAT_ITEM_PLHDR_DESC
            {
                get { return _ARG_MAT_ITEM_PLHDR_DESC; }
                set { _ARG_MAT_ITEM_PLHDR_DESC = value; }
            }

            private string _ARG_COLOR_ID;
            [DataType(Value = "VARCHAR2")]
            public string ARG_COLOR_ID
            {
                get { return _ARG_COLOR_ID; }
                set { _ARG_COLOR_ID = value; }
            }

            private string _ARG_COLOR_PLHDR_DESC;
            [DataType(Value = "VARCHAR2")]
            public string ARG_COLOR_PLHDR_DESC
            {
                get { return _ARG_COLOR_PLHDR_DESC; }
                set { _ARG_COLOR_PLHDR_DESC = value; }
            }

            private string _ARG_SUPP_MAT_COLOR_IS_MUL;
            [DataType(Value = "VARCHAR2")]
            public string ARG_SUPP_MAT_COLOR_IS_MUL
            {
                get { return _ARG_SUPP_MAT_COLOR_IS_MUL; }
                set { _ARG_SUPP_MAT_COLOR_IS_MUL = value; }
            }

            private string _ARG_SUPP_MAT_COLOR_ID;
            [DataType(Value = "VARCHAR2")]
            public string ARG_SUPP_MAT_COLOR_ID
            {
                get { return _ARG_SUPP_MAT_COLOR_ID; }
                set { _ARG_SUPP_MAT_COLOR_ID = value; }
            }

            private string _ARG_BOM_LINEITEM_COMMENTS;
            [DataType(Value = "VARCHAR2")]
            public string ARG_BOM_LINEITEM_COMMENTS
            {
                get { return _ARG_BOM_LINEITEM_COMMENTS; }
                set { _ARG_BOM_LINEITEM_COMMENTS = value; }
            }

            private string _ARG_FAC_IN_HOUSE_IND;
            [DataType(Value = "VARCHAR2")]
            public string ARG_FAC_IN_HOUSE_IND
            {
                get { return _ARG_FAC_IN_HOUSE_IND; }
                set { _ARG_FAC_IN_HOUSE_IND = value; }
            }

            private string _ARG_COUNTY_ORG_ID;
            [DataType(Value = "VARCHAR2")]
            public string ARG_COUNTY_ORG_ID
            {
                get { return _ARG_COUNTY_ORG_ID; }
                set { _ARG_COUNTY_ORG_ID = value; }
            }

            private string _ARG_ACTUAL_DIMENSION_DESC;
            [DataType(Value = "VARCHAR2")]
            public string ARG_ACTUAL_DIMENSION_DESC
            {
                get { return _ARG_ACTUAL_DIMENSION_DESC; }
                set { _ARG_ACTUAL_DIMENSION_DESC = value; }
            }

            private string _ARG_ISO_MEASUREMENT_CODE;
            [DataType(Value = "VARCHAR2")]
            public string ARG_ISO_MEASUREMENT_CODE
            {
                get { return _ARG_ISO_MEASUREMENT_CODE; }
                set { _ARG_ISO_MEASUREMENT_CODE = value; }
            }

            private string _ARG_NET_USAGE_NUMBER;
            [DataType(Value = "VARCHAR2")]
            public string ARG_NET_USAGE_NUMBER
            {
                get { return _ARG_NET_USAGE_NUMBER; }
                set { _ARG_NET_USAGE_NUMBER = value; }
            }

            private string _ARG_WASTE_USAGE_NUMBER;
            [DataType(Value = "VARCHAR2")]
            public string ARG_WASTE_USAGE_NUMBER
            {
                get { return _ARG_WASTE_USAGE_NUMBER; }
                set { _ARG_WASTE_USAGE_NUMBER = value; }
            }

            private string _ARG_PART_YIELD;
            [DataType(Value = "VARCHAR2")]
            public string ARG_PART_YIELD
            {
                get { return _ARG_PART_YIELD; }
                set { _ARG_PART_YIELD = value; }
            }

            private string _ARG_CONSUM_CONVER_RATE;
            [DataType(Value = "VARCHAR2")]
            public string ARG_CONSUM_CONVER_RATE
            {
                get { return _ARG_CONSUM_CONVER_RATE; }
                set { _ARG_CONSUM_CONVER_RATE = value; }
            }

            private string _ARG_LINEITEM_DEFECT_PER_NUM;
            [DataType(Value = "VARCHAR2")]
            public string ARG_LINEITEM_DEFECT_PER_NUM
            {
                get { return _ARG_LINEITEM_DEFECT_PER_NUM; }
                set { _ARG_LINEITEM_DEFECT_PER_NUM = value; }
            }

            private string _ARG_UNIT_PRICE_ISO_MSR_CODE;
            [DataType(Value = "VARCHAR2")]
            public string ARG_UNIT_PRICE_ISO_MSR_CODE
            {
                get { return _ARG_UNIT_PRICE_ISO_MSR_CODE; }
                set { _ARG_UNIT_PRICE_ISO_MSR_CODE = value; }
            }

            private string _ARG_CURRENCY_CODE;
            [DataType(Value = "VARCHAR2")]
            public string ARG_CURRENCY_CODE
            {
                get { return _ARG_CURRENCY_CODE; }
                set { _ARG_CURRENCY_CODE = value; }
            }

            private string _ARG_FACTORY_UNIT_PRICE;
            [DataType(Value = "VARCHAR2")]
            public string ARG_FACTORY_UNIT_PRICE
            {
                get { return _ARG_FACTORY_UNIT_PRICE; }
                set { _ARG_FACTORY_UNIT_PRICE = value; }
            }

            private string _ARG_UNIT_PRICE_UPCHARGE_NUM;
            [DataType(Value = "VARCHAR2")]
            public string ARG_UNIT_PRICE_UPCHARGE_NUM
            {
                get { return _ARG_UNIT_PRICE_UPCHARGE_NUM; }
                set { _ARG_UNIT_PRICE_UPCHARGE_NUM = value; }
            }

            private string _ARG_UNIT_PRICE_UPCHARGE_DESC;
            [DataType(Value = "VARCHAR2")]
            public string ARG_UNIT_PRICE_UPCHARGE_DESC
            {
                get { return _ARG_UNIT_PRICE_UPCHARGE_DESC; }
                set { _ARG_UNIT_PRICE_UPCHARGE_DESC = value; }
            }

            private string _ARG_FREIGHT_TERM_ID;
            [DataType(Value = "VARCHAR2")]
            public string ARG_FREIGHT_TERM_ID
            {
                get { return _ARG_FREIGHT_TERM_ID; }
                set { _ARG_FREIGHT_TERM_ID = value; }
            }

            private string _ARG_LANDED_COST_PER_NUM;
            [DataType(Value = "VARCHAR2")]
            public string ARG_LANDED_COST_PER_NUM
            {
                get { return _ARG_LANDED_COST_PER_NUM; }
                set { _ARG_LANDED_COST_PER_NUM = value; }
            }

            private string _ARG_PCC_SORT_ORDER;
            [DataType(Value = "VARCHAR2")]
            public string ARG_PCC_SORT_ORDER
            {
                get { return _ARG_PCC_SORT_ORDER; }
                set { _ARG_PCC_SORT_ORDER = value; }
            }

            private string _ARG_ROLLUP_VARIATION_LV;
            [DataType(Value = "VARCHAR2")]
            public string ARG_ROLLUP_VARIATION_LV
            {
                get { return _ARG_ROLLUP_VARIATION_LV; }
                set { _ARG_ROLLUP_VARIATION_LV = value; }
            }

            private string _ARG_PART_SEQ;
            [DataType(Value = "VARCHAR2")]
            public string ARG_PART_SEQ
            {
                get { return _ARG_PART_SEQ; }
                set { _ARG_PART_SEQ = value; }
            }

            private string _ARG_LOGIC_GROUP;
            [DataType(Value = "VARCHAR2")]
            public string ARG_LOGIC_GROUP
            {
                get { return _ARG_LOGIC_GROUP; }
                set { _ARG_LOGIC_GROUP = value; }
            }

            private double _ARG_MAT_FORECAST_PRCNT;
            [DataType(Value = "NUMBER")]
            public double ARG_MAT_FORECAST_PRCNT
            {
                get { return _ARG_MAT_FORECAST_PRCNT; }
                set { _ARG_MAT_FORECAST_PRCNT = value; }
            }

            private double _ARG_COLOR_FORECAST_PRCNT;
            [DataType(Value = "NUMBER")]
            public double ARG_COLOR_FORECAST_PRCNT
            {
                get { return _ARG_COLOR_FORECAST_PRCNT; }
                set { _ARG_COLOR_FORECAST_PRCNT = value; }
            }
        }

        public class INSERT_BOM_RECORD_HEAD
        {
            private string _ARG_FACTORY;
            [DataType(Value = "VARCHAR2")]
            public string ARG_FACTORY
            {
                get { return _ARG_FACTORY; }
                set { _ARG_FACTORY = value; }
            }

            private string _ARG_WS_NO;
            [DataType(Value = "VARCHAR2")]
            public string ARG_WS_NO
            {
                get { return _ARG_WS_NO; }
                set { _ARG_WS_NO = value; }
            }

            private string _ARG_DPA;
            [DataType(Value = "VARCHAR2")]
            public string ARG_DPA
            {
                get { return _ARG_DPA; }
                set { _ARG_DPA = value; }
            }

            private string _ARG_BOM_ID;
            [DataType(Value = "VARCHAR2")]
            public string ARG_BOM_ID
            {
                get { return _ARG_BOM_ID; }
                set { _ARG_BOM_ID = value; }
            }

            private string _ARG_ST_CD;
            [DataType(Value = "VARCHAR2")]
            public string ARG_ST_CD
            {
                get { return _ARG_ST_CD; }
                set { _ARG_ST_CD = value; }
            }

            private string _ARG_SEASON_CD;
            [DataType(Value = "VARCHAR2")]
            public string ARG_SEASON_CD
            {
                get { return _ARG_SEASON_CD; }
                set { _ARG_SEASON_CD = value; }
            }

            //private string _ARG_PCC_ORG;
            //[DataType(Value = "VARCHAR2")]
            //public string ARG_PCC_ORG
            //{
            //    get { return _ARG_PCC_ORG; }
            //    set { _ARG_PCC_ORG = value; }
            //}

            private string _ARG_CATEGORY;
            [DataType(Value = "VARCHAR2")]
            public string ARG_CATEGORY
            {
                get { return _ARG_CATEGORY; }
                set { _ARG_CATEGORY = value; }
            }

            private string _ARG_DEV_NAME;
            [DataType(Value = "VARCHAR2")]
            public string ARG_DEV_NAME
            {
                get { return _ARG_DEV_NAME; }
                set { _ARG_DEV_NAME = value; }
            }

            private string _ARG_STYLE_CD;
            [DataType(Value = "VARCHAR2")]
            public string ARG_STYLE_CD
            {
                get { return _ARG_STYLE_CD; }
                set { _ARG_STYLE_CD = value; }
            }

            private string _ARG_SAMPLE_ETS;
            [DataType(Value = "VARCHAR2")]
            public string ARG_SAMPLE_ETS
            {
                get { return _ARG_SAMPLE_ETS; }
                set { _ARG_SAMPLE_ETS = value; }
            }

            private string _ARG_SAMPLE_QTY;
            [DataType(Value = "VARCHAR2")]
            public string ARG_SAMPLE_QTY
            {
                get { return _ARG_SAMPLE_QTY; }
                set { _ARG_SAMPLE_QTY = value; }
            }

            private string _ARG_SAMPLE_SIZE;
            [DataType(Value = "VARCHAR2")]
            public string ARG_SAMPLE_SIZE
            {
                get { return _ARG_SAMPLE_SIZE; }
                set { _ARG_SAMPLE_SIZE = value; }
            }

            private string _ARG_SUB_TYPE_REMARK;
            [DataType(Value = "VARCHAR2")]
            public string ARG_SUB_TYPE_REMARK
            {
                get { return _ARG_SUB_TYPE_REMARK; }
                set { _ARG_SUB_TYPE_REMARK = value; }
            }

            private string _ARG_GENDER;
            [DataType(Value = "VARCHAR2")]
            public string ARG_GENDER
            {
                get { return _ARG_GENDER; }
                set { _ARG_GENDER = value; }
            }

            private string _ARG_TD;
            [DataType(Value = "VARCHAR2")]
            public string ARG_TD
            {
                get { return _ARG_TD; }
                set { _ARG_TD = value; }
            }

            private string _ARG_COLOR_VER;
            [DataType(Value = "VARCHAR2")]
            public string ARG_COLOR_VER
            {
                get { return _ARG_COLOR_VER; }
                set { _ARG_COLOR_VER = value; }
            }

            private string _ARG_MODEL_ID;
            [DataType(Value = "VARCHAR2")]
            public string ARG_MODEL_ID
            {
                get { return _ARG_MODEL_ID; }
                set { _ARG_MODEL_ID = value; }
            }

            private string _ARG_PCC_PM;
            [DataType(Value = "VARCHAR2")]
            public string ARG_PCC_PM
            {
                get { return _ARG_PCC_PM; }
                set { _ARG_PCC_PM = value; }
            }

            private string _ARG_PRODUCT_ID;
            [DataType(Value = "VARCHAR2")]
            public string ARG_PRODUCT_ID
            {
                get { return _ARG_PRODUCT_ID; }
                set { _ARG_PRODUCT_ID = value; }
            }

            private string _ARG_DEV_COLORWAY_ID;
            [DataType(Value = "VARCHAR2")]
            public string ARG_DEV_COLORWAY_ID
            {
                get { return _ARG_DEV_COLORWAY_ID; }
                set { _ARG_DEV_COLORWAY_ID = value; }
            }

            private string _ARG_DEV_STYLE_NUMBER;
            [DataType(Value = "VARCHAR2")]
            public string ARG_DEV_STYLE_NUMBER
            {
                get { return _ARG_DEV_STYLE_NUMBER; }
                set { _ARG_DEV_STYLE_NUMBER = value; }
            }

            private string _ARG_DEV_SAMPLE_REQ_ID;
            [DataType(Value = "VARCHAR2")]
            public string ARG_DEV_SAMPLE_REQ_ID
            {
                get { return _ARG_DEV_SAMPLE_REQ_ID; }
                set { _ARG_DEV_SAMPLE_REQ_ID = value; }
            }

            private string _ARG_UPD_USER;
            [DataType(Value = "VARCHAR2")]
            public string ARG_UPD_USER
            {
                get { return _ARG_UPD_USER; }
                set { _ARG_UPD_USER = value; }
            }

            private string _ARG_GEL_YN;
            [DataType(Value = "VARCHAR2")]
            public string ARG_GEL_YN
            {
                get { return _ARG_GEL_YN; }
                set { _ARG_GEL_YN = value; }
            }
        }

        public class INSERT_PUR_REQ_TMPR
        {
            private string _ARG_FACTORY;
            [DataType(Value = "VARCHAR2")]
            public string ARG_FACTORY
            {
                get { return _ARG_FACTORY; }
                set { _ARG_FACTORY = value; }
            }

            private string _ARG_CONCAT_WS_NO;
            [DataType(Value = "VARCHAR2")]
            public string ARG_CONCAT_WS_NO
            {
                get { return _ARG_CONCAT_WS_NO; }
                set { _ARG_CONCAT_WS_NO = value; }
            }

            private string _ARG_DEVELOPER;
            [DataType(Value = "VARCHAR2")]
            public string ARG_DEVELOPER
            {
                get { return _ARG_DEVELOPER; }
                set { _ARG_DEVELOPER = value; }
            }
        }

        public class INSERT_MANUAL_ADDED_MTL
        {
            private string _ARG_FACTORY;
            [DataType(Value = "VARCHAR2")]
            public string ARG_FACTORY
            {
                get { return _ARG_FACTORY; }
                set { _ARG_FACTORY = value; }
            }

            private string _ARG_WS_NO;
            [DataType(Value = "VARCHAR2")]
            public string ARG_WS_NO
            {
                get { return _ARG_WS_NO; }
                set { _ARG_WS_NO = value; }
            }

            private string _ARG_PART_SEQ;
            [DataType(Value = "VARCHAR2")]
            public string ARG_PART_SEQ
            {
                get { return _ARG_PART_SEQ; }
                set { _ARG_PART_SEQ = value; }
            }

            private string _ARG_PART_NAME;
            [DataType(Value = "VARCHAR2")]
            public string ARG_PART_NAME
            {
                get { return _ARG_PART_NAME; }
                set { _ARG_PART_NAME = value; }
            }

            private string _ARG_PART_TYPE;
            [DataType(Value = "VARCHAR2")]
            public string ARG_PART_TYPE
            {
                get { return _ARG_PART_TYPE; }
                set { _ARG_PART_TYPE = value; }
            }

            private string _ARG_MXSXL_NUMBER;
            [DataType(Value = "VARCHAR2")]
            public string ARG_MXSXL_NUMBER
            {
                get { return _ARG_MXSXL_NUMBER; }
                set { _ARG_MXSXL_NUMBER = value; }
            }

            private string _ARG_PCX_MAT_ID;
            [DataType(Value = "VARCHAR2")]
            public string ARG_PCX_MAT_ID
            {
                get { return _ARG_PCX_MAT_ID; }
                set { _ARG_PCX_MAT_ID = value; }
            }

            private string _ARG_MAT_CD;
            [DataType(Value = "VARCHAR2")]
            public string ARG_MAT_CD
            {
                get { return _ARG_MAT_CD; }
                set { _ARG_MAT_CD = value; }
            }

            private string _ARG_MAT_NAME;
            [DataType(Value = "VARCHAR2")]
            public string ARG_MAT_NAME
            {
                get { return _ARG_MAT_NAME; }
                set { _ARG_MAT_NAME = value; }
            }

            private string _ARG_MAT_COMMENT;
            [DataType(Value = "VARCHAR2")]
            public string ARG_MAT_COMMENT
            {
                get { return _ARG_MAT_COMMENT; }
                set { _ARG_MAT_COMMENT = value; }
            }

            private string _ARG_PCX_COLOR_ID;
            [DataType(Value = "VARCHAR2")]
            public string ARG_PCX_COLOR_ID
            {
                get { return _ARG_PCX_COLOR_ID; }
                set { _ARG_PCX_COLOR_ID = value; }
            }

            private string _ARG_COLOR_CD;
            [DataType(Value = "VARCHAR2")]
            public string ARG_COLOR_CD
            {
                get { return _ARG_COLOR_CD; }
                set { _ARG_COLOR_CD = value; }
            }

            private string _ARG_COLOR_NAME;
            [DataType(Value = "VARCHAR2")]
            public string ARG_COLOR_NAME
            {
                get { return _ARG_COLOR_NAME; }
                set { _ARG_COLOR_NAME = value; }
            }

            private string _ARG_OWNER;
            [DataType(Value = "VARCHAR2")]
            public string ARG_OWNER
            {
                get { return _ARG_OWNER; }
                set { _ARG_OWNER = value; }
            }

            private string _ARG_LOCATION;
            [DataType(Value = "VARCHAR2")]
            public string ARG_LOCATION
            {
                get { return _ARG_LOCATION; }
                set { _ARG_LOCATION = value; }
            }

            private string _ARG_COLOR_COMMENT;
            [DataType(Value = "VARCHAR2")]
            public string ARG_COLOR_COMMENT
            {
                get { return _ARG_COLOR_COMMENT; }
                set { _ARG_COLOR_COMMENT = value; }
            }
        }

        public class INSERT_MATERIALS_TO_REQ
        {
            private string _ARG_FACTORY;
            [DataType(Value = "VARCHAR2")]
            public string ARG_FACTORY
            {
                get { return _ARG_FACTORY; }
                set { _ARG_FACTORY = value; }
            }

            private string _ARG_CONCAT_WS_NO;
            [DataType(Value = "VARCHAR2")]
            public string ARG_CONCAT_WS_NO
            {
                get { return _ARG_CONCAT_WS_NO; }
                set { _ARG_CONCAT_WS_NO = value; }
            }

            private string _ARG_PUR_USER;
            [DataType(Value = "VARCHAR2")]
            public string ARG_PUR_USER
            {
                get { return _ARG_PUR_USER; }
                set { _ARG_PUR_USER = value; }
            }
        }

        public class INSERT_COPY_BOM
        {
            private string _ARG_FACTORY;
            [DataType(Value = "VARCHAR2")]
            public string ARG_FACTORY
            {
                get { return _ARG_FACTORY; }
                set { _ARG_FACTORY = value; }
            }

            private string _ARG_WS_NO;
            [DataType(Value = "VARCHAR2")]
            public string ARG_WS_NO
            {
                get { return _ARG_WS_NO; }
                set { _ARG_WS_NO = value; }
            }

            private string _ARG_SEASON_CD;
            [DataType(Value = "VARCHAR2")]
            public string ARG_SEASON_CD
            {
                get { return _ARG_SEASON_CD; }
                set { _ARG_SEASON_CD = value; }

            }

            private string _ARG_ST_CD;
            [DataType(Value = "VARCHAR2")]
            public string ARG_ST_CD
            {
                get { return _ARG_ST_CD; }
                set { _ARG_ST_CD = value; }
            }

            private string _ARG_SUB_ST_CD;
            [DataType(Value = "VARCHAR2")]
            public string ARG_SUB_ST_CD
            {
                get { return _ARG_SUB_ST_CD; }
                set { _ARG_SUB_ST_CD = value; }
            }

            private string _ARG_SUB_TYPE_REMARK;
            [DataType(Value = "VARCHAR2")]
            public string ARG_SUB_TYPE_REMARK
            {
                get { return _ARG_SUB_TYPE_REMARK; }
                set { _ARG_SUB_TYPE_REMARK = value; }
            }

            private string _ARG_UPD_USER;
            [DataType(Value = "VARCHAR2")]
            public string ARG_UPD_USER
            {
                get { return _ARG_UPD_USER; }
                set { _ARG_UPD_USER = value; }
            }

            private string _ARG_CS_BOM_CFM;
            [DataType(Value = "VARCHAR2")]
            public string ARG_CS_BOM_CFM
            {
                get { return _ARG_CS_BOM_CFM; }
                set { _ARG_CS_BOM_CFM = value; }
            }

            private string _ARG_IS_RJ_COPY;
            [DataType(Value = "VARCHAR2")]
            public string ARG_IS_RJ_COPY
            {
                get { return _ARG_IS_RJ_COPY; }
                set { _ARG_IS_RJ_COPY = value; }
            }
        }

        public class INSERT_COPY_WORKSHEET
        {
            private string _ARG_S_FACTORY;
            [DataType(Value = "VARCHAR2")]
            public string ARG_S_FACTORY
            {
                get { return _ARG_S_FACTORY; }
                set { _ARG_S_FACTORY = value; }
            }

            private string _ARG_S_WS_NO;
            [DataType(Value = "VARCHAR2")]
            public string ARG_S_WS_NO
            {
                get { return _ARG_S_WS_NO; }
                set { _ARG_S_WS_NO = value; }
            }

            private string _ARG_T_FACTORY;
            [DataType(Value = "VARCHAR2")]
            public string ARG_T_FACTORY
            {
                get { return _ARG_T_FACTORY; }
                set { _ARG_T_FACTORY = value; }
            }

            private string _ARG_T_WS_NO;
            [DataType(Value = "VARCHAR2")]
            public string ARG_T_WS_NO
            {
                get { return _ARG_T_WS_NO; }
                set { _ARG_T_WS_NO = value; }
            }

            private string _ARG_UPD_USER;
            [DataType(Value = "VARCHAR2")]
            public string ARG_UPD_USER
            {
                get { return _ARG_UPD_USER; }
                set { _ARG_UPD_USER = value; }
            }
        }

        public class REPLACE_SAME_AS_TARGET
        {
            private string _ARG_S_FACTORY;
            [DataType(Value = "VARCHAR2")]
            public string ARG_S_FACTORY
            {
                get { return _ARG_S_FACTORY; }
                set { _ARG_S_FACTORY = value; }
            }

            private string _ARG_S_WS_NO;
            [DataType(Value = "VARCHAR2")]
            public string ARG_S_WS_NO
            {
                get { return _ARG_S_WS_NO; }
                set { _ARG_S_WS_NO = value; }
            }

            private string _ARG_T_FACTORY;
            [DataType(Value = "VARCHAR2")]
            public string ARG_T_FACTORY
            {
                get { return _ARG_T_FACTORY; }
                set { _ARG_T_FACTORY = value; }
            }

            private string _ARG_T_WS_NO;
            [DataType(Value = "VARCHAR2")]
            public string ARG_T_WS_NO
            {
                get { return _ARG_T_WS_NO; }
                set { _ARG_T_WS_NO = value; }
            }

            private string _ARG_UPD_USER;
            [DataType(Value = "VARCHAR2")]
            public string ARG_UPD_USER
            {
                get { return _ARG_UPD_USER; }
                set { _ARG_UPD_USER = value; }
            }
        }

        public class REPLACE_BY_OPTION
        {
            private string _ARG_S_FACTORY;
            [DataType(Value = "VARCHAR2")]
            public string ARG_S_FACTORY
            {
                get { return _ARG_S_FACTORY; }
                set { _ARG_S_FACTORY = value; }
            }

            private string _ARG_S_WS_NO;
            [DataType(Value = "VARCHAR2")]
            public string ARG_S_WS_NO
            {
                get { return _ARG_S_WS_NO; }
                set { _ARG_S_WS_NO = value; }
            }

            private string _ARG_T_FACTORY;
            [DataType(Value = "VARCHAR2")]
            public string ARG_T_FACTORY
            {
                get { return _ARG_T_FACTORY; }
                set { _ARG_T_FACTORY = value; }
            }

            private string _ARG_T_WS_NO;
            [DataType(Value = "VARCHAR2")]
            public string ARG_T_WS_NO
            {
                get { return _ARG_T_WS_NO; }
                set { _ARG_T_WS_NO = value; }
            }

            private string _ARG_UPD_USER;
            [DataType(Value = "VARCHAR2")]
            public string ARG_UPD_USER
            {
                get { return _ARG_UPD_USER; }
                set { _ARG_UPD_USER = value; }
            }

            private string _ARG_COLOR_OPT;
            [DataType(Value = "VARCHAR2")]
            public string ARG_COLOR_OPT
            {
                get { return _ARG_COLOR_OPT; }
                set { _ARG_COLOR_OPT = value; }
            }

            private string _ARG_MAT_OPT;
            [DataType(Value = "VARCHAR2")]
            public string ARG_MAT_OPT
            {
                get { return _ARG_MAT_OPT; }
                set { _ARG_MAT_OPT = value; }
            }
        }

        public class INSERT_BOM_IMAGE
        {
            private string _ARG_FACTORY;
            [DataType(Value = "VARCHAR2")]
            public string ARG_FACTORY
            {
                get { return _ARG_FACTORY; }
                set { _ARG_FACTORY = value; }
            }

            private string _ARG_WS_NO;
            [DataType(Value = "VARCHAR2")]
            public string ARG_WS_NO
            {
                get { return _ARG_WS_NO; }
                set { _ARG_WS_NO = value; }
            }

            private string _ARG_IMAGE_TYPE;
            [DataType(Value = "VARCHAR2")]
            public string ARG_IMAGE_TYPE
            {
                get { return _ARG_IMAGE_TYPE; }
                set { _ARG_IMAGE_TYPE = value; }
            }

            private string _ARG_IMAGE_SEQ;
            [DataType(Value = "VARCHAR2")]
            public string ARG_IMAGE_SEQ
            {
                get { return _ARG_IMAGE_SEQ; }
                set { _ARG_IMAGE_SEQ = value; }
            }

            private string _ARG_IMAGE_TITLE;
            [DataType(Value = "VARCHAR2")]
            public string ARG_IMAGE_TITLE
            {
                get { return _ARG_IMAGE_TITLE; }
                set { _ARG_IMAGE_TITLE = value; }
            }

            private Byte[] _ARG_IMAGE_FILE;
            [DataType(Value = "BLOB")]
            public Byte[] ARG_IMAGE_FILE
            {
                get { return _ARG_IMAGE_FILE; }
                set { _ARG_IMAGE_FILE = value; }
            }

            private string _ARG_UPD_USER;
            [DataType(Value = "VARCHAR2")]
            public string ARG_UPD_USER
            {
                get { return _ARG_UPD_USER; }
                set { _ARG_UPD_USER = value; }
            }
        }

        public class INSERT_SMS_USER
        {
            private string _ARG_FACTORY;
            [DataType(Value = "VARCHAR2")]
            public string ARG_FACTORY
            {
                get { return _ARG_FACTORY; }
                set { _ARG_FACTORY = value; }
            }

            private string _ARG_WS_NO;
            [DataType(Value = "VARCHAR2")]
            public string ARG_WS_NO
            {
                get { return _ARG_WS_NO; }
                set { _ARG_WS_NO = value; }
            }

            private string _ARG_USER_ID;
            [DataType(Value = "VARCHAR2")]
            public string ARG_USER_ID
            {
                get { return _ARG_USER_ID; }
                set { _ARG_USER_ID = value; }
            }

            private string _ARG_UPD_USER;
            [DataType(Value = "VARCHAR2")]
            public string ARG_UPD_USER
            {
                get { return _ARG_UPD_USER; }
                set { _ARG_UPD_USER = value; }
            }
        }

        public class INSERT_CS_BOM_CFM_HISTORY
        {
            private string _ARG_WORK_TYPE;
            [DataType(Value = "VARCHAR2")]
            public string ARG_WORK_TYPE
            {
                get { return _ARG_WORK_TYPE; }
                set { _ARG_WORK_TYPE = value; }
            }

            private string _ARG_FACTORY;
            [DataType(Value = "VARCHAR2")]
            public string ARG_FACTORY
            {
                get { return _ARG_FACTORY; }
                set { _ARG_FACTORY = value; }
            }

            private string _ARG_WS_NO;
            [DataType(Value = "VARCHAR2")]
            public string ARG_WS_NO
            {
                get { return _ARG_WS_NO; }
                set { _ARG_WS_NO = value; }
            }

            private string _ARG_CFM_REASON;
            [DataType(Value = "VARCHAR2")]
            public string ARG_CFM_REASON
            {
                get { return _ARG_CFM_REASON; }
                set { _ARG_CFM_REASON = value; }
            }

            private string _ARG_CFM_DETAIL;
            [DataType(Value = "VARCHAR2")]
            public string ARG_CFM_DETAIL
            {
                get { return _ARG_CFM_DETAIL; }
                set { _ARG_CFM_DETAIL = value; }
            }

            private string _ARG_UPD_USER;
            [DataType(Value = "VARCHAR2")]
            public string ARG_UPD_USER
            {
                get { return _ARG_UPD_USER; }
                set { _ARG_UPD_USER = value; }
            }
        }

        public class INSERT_CONVERT_TO_OCF
        {
            private string _ARG_FACTORY;
            [DataType(Value = "VARCHAR2")]
            public string ARG_FACTORY
            {
                get { return _ARG_FACTORY; }
                set { _ARG_FACTORY = value; }
            }

            private string _ARG_WS_NO;
            [DataType(Value = "VARCHAR2")]
            public string ARG_WS_NO
            {
                get { return _ARG_WS_NO; }
                set { _ARG_WS_NO = value; }
            }

            private string _UPD_USER;
            [DataType(Value = "VARCHAR2")]
            public string UPD_USER
            {
                get { return _UPD_USER; }
                set { _UPD_USER = value; }
            }
        }

        public class INSERT_PCC_PLAN_OPCD
        {
            private string _ARG_FACTORY;
            [DataType(Value = "VARCHAR2")]
            public string ARG_FACTORY
            {
                get { return _ARG_FACTORY; }
                set { _ARG_FACTORY = value; }
            }

            private string _ARG_WS_NO;
            [DataType(Value = "VARCHAR2")]
            public string ARG_WS_NO
            {
                get { return _ARG_WS_NO; }
                set { _ARG_WS_NO = value; }
            }

            private string _UPD_USER;
            [DataType(Value = "VARCHAR2")]
            public string UPD_USER
            {
                get { return _UPD_USER; }
                set { _UPD_USER = value; }
            }
        }

        public class INSERT_WS_TMPLT_SPEC
        {
            private string _ARG_FACTORY;
            [DataType(Value = "VARCHAR2")]
            public string ARG_FACTORY
            {
                get { return _ARG_FACTORY; }
                set { _ARG_FACTORY = value; }
            }

            private string _ARG_KEY_VALUE;
            [DataType(Value = "VARCHAR2")]
            public string ARG_KEY_VALUE
            {
                get { return _ARG_KEY_VALUE; }
                set { _ARG_KEY_VALUE = value; }
            }

            private string _ARG_KEY_TYPE;
            [DataType(Value = "VARCHAR2")]
            public string ARG_KEY_TYPE
            {
                get { return _ARG_KEY_TYPE; }
                set { _ARG_KEY_TYPE = value; }
            }

            private string _ARG_LAST_CD;
            [DataType(Value = "VARCHAR2")]
            public string ARG_LAST_CD
            {
                get { return _ARG_LAST_CD; }
                set { _ARG_LAST_CD = value; }
            }

            private string _ARG_HEEL_HEIGHT;
            [DataType(Value = "VARCHAR2")]
            public string ARG_HEEL_HEIGHT
            {
                get { return _ARG_HEEL_HEIGHT; }
                set { _ARG_HEEL_HEIGHT = value; }
            }

            private string _ARG_MEDIAL_HEIGHT;
            [DataType(Value = "VARCHAR2")]
            public string ARG_MEDIAL_HEIGHT
            {
                get { return _ARG_MEDIAL_HEIGHT; }
                set { _ARG_MEDIAL_HEIGHT = value; }
            }

            private string _ARG_LATERAL_HEIGHT;
            [DataType(Value = "VARCHAR2")]
            public string ARG_LATERAL_HEIGHT
            {
                get { return _ARG_LATERAL_HEIGHT; }
                set { _ARG_LATERAL_HEIGHT = value; }
            }

            private string _ARG_LACE_LENGTH;
            [DataType(Value = "VARCHAR2")]
            public string ARG_LACE_LENGTH
            {
                get { return _ARG_LACE_LENGTH; }
                set { _ARG_LACE_LENGTH = value; }
            }

            private string _ARG_MS_HARDNESS;
            [DataType(Value = "VARCHAR2")]
            public string ARG_MS_HARDNESS
            {
                get { return _ARG_MS_HARDNESS; }
                set { _ARG_MS_HARDNESS = value; }
            }

            private string _ARG_IDS_LENGTH;
            [DataType(Value = "VARCHAR2")]
            public string ARG_IDS_LENGTH
            {
                get { return _ARG_IDS_LENGTH; }
                set { _ARG_IDS_LENGTH = value; }
            }

            private string _ARG_MS_CODE;
            [DataType(Value = "VARCHAR2")]
            public string ARG_MS_CODE
            {
                get { return _ARG_MS_CODE; }
                set { _ARG_MS_CODE = value; }
            }

            private string _ARG_OS_CODE;
            [DataType(Value = "VARCHAR2")]
            public string ARG_OS_CODE
            {
                get { return _ARG_OS_CODE; }
                set { _ARG_OS_CODE = value; }
            }

            private string _ARG_UPPER_MATERIAL;
            [DataType(Value = "VARCHAR2")]
            public string ARG_UPPER_MATERIAL
            {
                get { return _ARG_UPPER_MATERIAL; }
                set { _ARG_UPPER_MATERIAL = value; }
            }

            private string _ARG_MS_MATERIAL;
            [DataType(Value = "VARCHAR2")]
            public string ARG_MS_MATERIAL
            {
                get { return _ARG_MS_MATERIAL; }
                set { _ARG_MS_MATERIAL = value; }
            }

            private string _ARG_OUTSOLE_MATERIAL;
            [DataType(Value = "VARCHAR2")]
            public string ARG_OUTSOLE_MATERIAL
            {
                get { return _ARG_OUTSOLE_MATERIAL; }
                set { _ARG_OUTSOLE_MATERIAL = value; }
            }

            private string _ARG_NEEDLE_SIZE;
            [DataType(Value = "VARCHAR2")]
            public string ARG_NEEDLE_SIZE
            {
                get { return _ARG_NEEDLE_SIZE; }
                set { _ARG_NEEDLE_SIZE = value; }
            }

            private string _ARG_SPI;
            [DataType(Value = "VARCHAR2")]
            public string ARG_SPI
            {
                get { return _ARG_SPI; }
                set { _ARG_SPI = value; }
            }

            private string _ARG_STITCHING_MARGIN;
            [DataType(Value = "VARCHAR2")]
            public string ARG_STITCHING_MARGIN
            {
                get { return _ARG_STITCHING_MARGIN; }
                set { _ARG_STITCHING_MARGIN = value; }
            }

            private string _ARG_TWOROW_STITCHING_MARGIN;
            [DataType(Value = "VARCHAR2")]
            public string ARG_TWOROW_STITCHING_MARGIN
            {
                get { return _ARG_TWOROW_STITCHING_MARGIN; }
                set { _ARG_TWOROW_STITCHING_MARGIN = value; }
            }

            private string _ARG_THREAD_TYPE;
            [DataType(Value = "VARCHAR2")]
            public string ARG_THREAD_TYPE
            {
                get { return _ARG_THREAD_TYPE; }
                set { _ARG_THREAD_TYPE = value; }
            }

            private string _ARG_DCS_YN;
            [DataType(Value = "VARCHAR2")]
            public string ARG_DCS_YN
            {
                get { return _ARG_DCS_YN; }
                set { _ARG_DCS_YN = value; }
            }

            private string _ARG_WS_COMMENT1;
            [DataType(Value = "VARCHAR2")]
            public string ARG_WS_COMMENT1
            {
                get { return _ARG_WS_COMMENT1; }
                set { _ARG_WS_COMMENT1 = value; }
            }

            private string _ARG_WS_COMMENT2;
            [DataType(Value = "VARCHAR2")]
            public string ARG_WS_COMMENT2
            {
                get { return _ARG_WS_COMMENT2; }
                set { _ARG_WS_COMMENT2 = value; }
            }

            private string _ARG_WS_COMMENT3;
            [DataType(Value = "VARCHAR2")]
            public string ARG_WS_COMMENT3
            {
                get { return _ARG_WS_COMMENT3; }
                set { _ARG_WS_COMMENT3 = value; }
            }

            private string _ARG_WS_COMMENT4;
            [DataType(Value = "VARCHAR2")]
            public string ARG_WS_COMMENT4
            {
                get { return _ARG_WS_COMMENT4; }
                set { _ARG_WS_COMMENT4 = value; }
            }

            private string _ARG_WS_COMMENT5;
            [DataType(Value = "VARCHAR2")]
            public string ARG_WS_COMMENT5
            {
                get { return _ARG_WS_COMMENT5; }
                set { _ARG_WS_COMMENT5 = value; }
            }

            private string _ARG_WS_COMMENT6;
            [DataType(Value = "VARCHAR2")]
            public string ARG_WS_COMMENT6
            {
                get { return _ARG_WS_COMMENT6; }
                set { _ARG_WS_COMMENT6 = value; }
            }

            private string _ARG_WS_COMMENT7;
            [DataType(Value = "VARCHAR2")]
            public string ARG_WS_COMMENT7
            {
                get { return _ARG_WS_COMMENT7; }
                set { _ARG_WS_COMMENT7 = value; }
            }

            private string _ARG_UPD_USER;
            [DataType(Value = "VARCHAR2")]
            public string ARG_UPD_USER
            {
                get { return _ARG_UPD_USER; }
                set { _ARG_UPD_USER = value; }
            }

            private string _ARG_NIKE_DEV;
            [DataType(Value = "VARCHAR2")]
            public string ARG_NIKE_DEV
            {
                get { return _ARG_NIKE_DEV; }
                set { _ARG_NIKE_DEV = value; }
            }

            private string _ARG_WHQ_DEV;
            [DataType(Value = "VARCHAR2")]
            public string ARG_WHQ_DEV
            {
                get { return _ARG_WHQ_DEV; }
                set { _ARG_WHQ_DEV = value; }
            }
        }

        public class INSERT_WS_TMPLT_OP
        {
            private string _ARG_FACTORY;
            [DataType(Value = "VARCHAR2")]
            public string ARG_FACTORY
            {
                get { return _ARG_FACTORY; }
                set { _ARG_FACTORY = value; }
            }

            private string _ARG_KEY_VALUE;
            [DataType(Value = "VARCHAR2")]
            public string ARG_KEY_VALUE
            {
                get { return _ARG_KEY_VALUE; }
                set { _ARG_KEY_VALUE = value; }
            }

            private string _ARG_KEY_TYPE;
            [DataType(Value = "VARCHAR2")]
            public string ARG_KEY_TYPE
            {
                get { return _ARG_KEY_TYPE; }
                set { _ARG_KEY_TYPE = value; }
            }

            private string _ARG_OP_CD;
            [DataType(Value = "VARCHAR2")]
            public string ARG_OP_CD
            {
                get { return _ARG_OP_CD; }
                set { _ARG_OP_CD = value; }
            }

            private string _ARG_OP_CHK;
            [DataType(Value = "VARCHAR2")]
            public string ARG_OP_CHK
            {
                get { return _ARG_OP_CHK; }
                set { _ARG_OP_CHK = value; }
            }

            private string _ARG_UPD_USER;
            [DataType(Value = "VARCHAR2")]
            public string ARG_UPD_USER
            {
                get { return _ARG_UPD_USER; }
                set { _ARG_UPD_USER = value; }
            }
        }

        public class INSERT_REP_BOM
        {
            private string _ARG_WORK_TYPE;
            [DataType(Value = "VARCHAR2")]
            public string ARG_WORK_TYPE
            {
                get { return _ARG_WORK_TYPE; }
                set { _ARG_WORK_TYPE = value; }
            }

            private string _ARG_KEY_VALUE;
            [DataType(Value = "VARCHAR2")]
            public string ARG_KEY_VALUE
            {
                get { return _ARG_KEY_VALUE; }
                set { _ARG_KEY_VALUE = value; }
            }

            private string _ARG_FACTORY;
            [DataType(Value = "VARCHAR2")]
            public string ARG_FACTORY
            {
                get { return _ARG_FACTORY; }
                set { _ARG_FACTORY = value; }
            }

            private string _ARG_WS_NO;
            [DataType(Value = "VARCHAR2")]
            public string ARG_WS_NO
            {
                get { return _ARG_WS_NO; }
                set { _ARG_WS_NO = value; }
            }
        }

        public class INSERT_RESET_BY_ORG_DATA
        {
            private string _ARG_WORK_TYPE;
            [DataType(Value = "VARCHAR2")]
            public string ARG_WORK_TYPE
            {
                get { return _ARG_WORK_TYPE; }
                set { _ARG_WORK_TYPE = value; }
            }

            private string _ARG_FACTORY;
            [DataType(Value = "VARCHAR2")]
            public string ARG_FACTORY
            {
                get { return _ARG_FACTORY; }
                set { _ARG_FACTORY = value; }
            }

            private string _ARG_WS_NO;
            [DataType(Value = "VARCHAR2")]
            public string ARG_WS_NO
            {
                get { return _ARG_WS_NO; }
                set { _ARG_WS_NO = value; }
            }

            private string _ARG_UPD_USER;
            [DataType(Value = "VARCHAR2")]
            public string ARG_UPD_USER
            {
                get { return _ARG_UPD_USER; }
                set { _ARG_UPD_USER = value; }
            }
        }

        public class INSERT_REPLACE_JSON_ORG_HEADER
        {
            private string _ARG_FACTORY;
            [DataType(Value = "VARCHAR2")]
            public string ARG_FACTORY
            {
                get { return _ARG_FACTORY; }
                set { _ARG_FACTORY = value; }
            }

            private string _ARG_WS_NO;
            [DataType(Value = "VARCHAR2")]
            public string ARG_WS_NO
            {
                get { return _ARG_WS_NO; }
                set { _ARG_WS_NO = value; }
            }

            private string _ARG_OBJ_ID;
            [DataType(Value = "VARCHAR2")]
            public string ARG_OBJ_ID
            {
                get { return _ARG_OBJ_ID; }
                set { _ARG_OBJ_ID = value; }
            }

            private string _ARG_BOM_PART_UUID;
            [DataType(Value = "VARCHAR2")]
            public string ARG_BOM_PART_UUID
            {
                get { return _ARG_BOM_PART_UUID; }
                set { _ARG_BOM_PART_UUID = value; }
            }

            private string _ARG_OBJ_TYPE;
            [DataType(Value = "VARCHAR2")]
            public string ARG_OBJ_TYPE
            {
                get { return _ARG_OBJ_TYPE; }
                set { _ARG_OBJ_TYPE = value; }
            }

            private string _ARG_BOM_CONTRACT_VER;
            [DataType(Value = "VARCHAR2")]
            public string ARG_BOM_CONTRACT_VER
            {
                get { return _ARG_BOM_CONTRACT_VER; }
                set { _ARG_BOM_CONTRACT_VER = value; }
            }

            private string _ARG_DEV_STYLE_ID;
            [DataType(Value = "VARCHAR2")]
            public string ARG_DEV_STYLE_ID
            {
                get { return _ARG_DEV_STYLE_ID; }
                set { _ARG_DEV_STYLE_ID = value; }
            }

            private string _ARG_DEV_COLORWAY_ID;
            [DataType(Value = "VARCHAR2")]
            public string ARG_DEV_COLORWAY_ID
            {
                get { return _ARG_DEV_COLORWAY_ID; }
                set { _ARG_DEV_COLORWAY_ID = value; }
            }

            private string _ARG_COLORWAY_NAME;
            [DataType(Value = "VARCHAR2")]
            public string ARG_COLORWAY_NAME
            {
                get { return _ARG_COLORWAY_NAME; }
                set { _ARG_COLORWAY_NAME = value; }
            }

            private string _ARG_SRC_CONFIG_ID;
            [DataType(Value = "VARCHAR2")]
            public string ARG_SRC_CONFIG_ID
            {
                get { return _ARG_SRC_CONFIG_ID; }
                set { _ARG_SRC_CONFIG_ID = value; }
            }

            private string _ARG_SRC_CONFIG_NAME;
            [DataType(Value = "VARCHAR2")]
            public string ARG_SRC_CONFIG_NAME
            {
                get { return _ARG_SRC_CONFIG_NAME; }
                set { _ARG_SRC_CONFIG_NAME = value; }
            }

            private string _ARG_SEASON_ID;
            [DataType(Value = "VARCHAR2")]
            public string ARG_SEASON_ID
            {
                get { return _ARG_SEASON_ID; }
                set { _ARG_SEASON_ID = value; }
            }

            private string _ARG_BOM_ID;
            [DataType(Value = "VARCHAR2")]
            public string ARG_BOM_ID
            {
                get { return _ARG_BOM_ID; }
                set { _ARG_BOM_ID = value; }
            }

            private string _ARG_BOM_NAME;
            [DataType(Value = "VARCHAR2")]
            public string ARG_BOM_NAME
            {
                get { return _ARG_BOM_NAME; }
                set { _ARG_BOM_NAME = value; }
            }

            private string _ARG_BOM_VERSION_NUM;
            [DataType(Value = "VARCHAR2")]
            public string ARG_BOM_VERSION_NUM
            {
                get { return _ARG_BOM_VERSION_NUM; }
                set { _ARG_BOM_VERSION_NUM = value; }
            }

            private string _ARG_BOM_DESC;
            [DataType(Value = "VARCHAR2")]
            public string ARG_BOM_DESC
            {
                get { return _ARG_BOM_DESC; }
                set { _ARG_BOM_DESC = value; }
            }

            private string _ARG_BOM_COMMENTS;
            [DataType(Value = "VARCHAR2")]
            public string ARG_BOM_COMMENTS
            {
                get { return _ARG_BOM_COMMENTS; }
                set { _ARG_BOM_COMMENTS = value; }
            }

            private string _ARG_BOM_STATUS_IND;
            [DataType(Value = "VARCHAR2")]
            public string ARG_BOM_STATUS_IND
            {
                get { return _ARG_BOM_STATUS_IND; }
                set { _ARG_BOM_STATUS_IND = value; }
            }

            private string _ARG_CREATE_TIME_STAMP;
            [DataType(Value = "VARCHAR2")]
            public string ARG_CREATE_TIME_STAMP
            {
                get { return _ARG_CREATE_TIME_STAMP; }
                set { _ARG_CREATE_TIME_STAMP = value; }
            }

            private string _ARG_CHANGE_TIME_STAMP;
            [DataType(Value = "VARCHAR2")]
            public string ARG_CHANGE_TIME_STAMP
            {
                get { return _ARG_CHANGE_TIME_STAMP; }
                set { _ARG_CHANGE_TIME_STAMP = value; }
            }

            private string _ARG_CREATED_BY;
            [DataType(Value = "VARCHAR2")]
            public string ARG_CREATED_BY
            {
                get { return _ARG_CREATED_BY; }
                set { _ARG_CREATED_BY = value; }
            }

            private string _ARG_MODIFIED_BY;
            [DataType(Value = "VARCHAR2")]
            public string ARG_MODIFIED_BY
            {
                get { return _ARG_MODIFIED_BY; }
                set { _ARG_MODIFIED_BY = value; }
            }

            private string _ARG_STYLE_NUMBER;
            [DataType(Value = "VARCHAR2")]
            public string ARG_STYLE_NUMBER
            {
                get { return _ARG_STYLE_NUMBER; }
                set { _ARG_STYLE_NUMBER = value; }
            }

            private string _ARG_STYLE_NAME;
            [DataType(Value = "VARCHAR2")]
            public string ARG_STYLE_NAME
            {
                get { return _ARG_STYLE_NAME; }
                set { _ARG_STYLE_NAME = value; }
            }

            private string _ARG_MODEL_ID;
            [DataType(Value = "VARCHAR2")]
            public string ARG_MODEL_ID
            {
                get { return _ARG_MODEL_ID; }
                set { _ARG_MODEL_ID = value; }
            }

            private string _ARG_GENDER;
            [DataType(Value = "VARCHAR2")]
            public string ARG_GENDER
            {
                get { return _ARG_GENDER; }
                set { _ARG_GENDER = value; }
            }

            private string _ARG_AGE;
            [DataType(Value = "VARCHAR2")]
            public string ARG_AGE
            {
                get { return _ARG_AGE; }
                set { _ARG_AGE = value; }
            }

            private string _ARG_PRODUCT_ID;
            [DataType(Value = "VARCHAR2")]
            public string ARG_PRODUCT_ID
            {
                get { return _ARG_PRODUCT_ID; }
                set { _ARG_PRODUCT_ID = value; }
            }

            private string _ARG_PRODUCT_CODE;
            [DataType(Value = "VARCHAR2")]
            public string ARG_PRODUCT_CODE
            {
                get { return _ARG_PRODUCT_CODE; }
                set { _ARG_PRODUCT_CODE = value; }
            }

            private string _ARG_COLORWAY_CODE;
            [DataType(Value = "VARCHAR2")]
            public string ARG_COLORWAY_CODE
            {
                get { return _ARG_COLORWAY_CODE; }
                set { _ARG_COLORWAY_CODE = value; }
            }

            private string _ARG_DEV_STYLE_TYPE_ID;
            [DataType(Value = "VARCHAR2")]
            public string ARG_DEV_STYLE_TYPE_ID
            {
                get { return _ARG_DEV_STYLE_TYPE_ID; }
                set { _ARG_DEV_STYLE_TYPE_ID = value; }
            }

            private string _ARG_LOGIC_BOM_STATE_ID;
            [DataType(Value = "VARCHAR2")]
            public string ARG_LOGIC_BOM_STATE_ID
            {
                get { return _ARG_LOGIC_BOM_STATE_ID; }
                set { _ARG_LOGIC_BOM_STATE_ID = value; }
            }

            private string _ARG_LOGIC_BOM_GATE_ID;
            [DataType(Value = "VARCHAR2")]
            public string ARG_LOGIC_BOM_GATE_ID
            {
                get { return _ARG_LOGIC_BOM_GATE_ID; }
                set { _ARG_LOGIC_BOM_GATE_ID = value; }
            }

            private string _ARG_CYCLE_YEAR;
            [DataType(Value = "VARCHAR2")]
            public string ARG_CYCLE_YEAR
            {
                get { return _ARG_CYCLE_YEAR; }
                set { _ARG_CYCLE_YEAR = value; }
            }
        }

        public class TRANSFER_DATA_TO_PCXBOM
        {
            private string _ARG_PCX_KEY;
            [DataType(Value = "VARCHAR2")]
            public string ARG_PCX_KEY
            {
                get { return _ARG_PCX_KEY; }
                set { _ARG_PCX_KEY = value; }
            }

            private string _ARG_FACTORY;
            [DataType(Value = "VARCHAR2")]
            public string ARG_FACTORY
            {
                get { return _ARG_FACTORY; }
                set { _ARG_FACTORY = value; }
            }

            private string _ARG_WS_NO;
            [DataType(Value = "VARCHAR2")]
            public string ARG_WS_NO
            {
                get { return _ARG_WS_NO; }
                set { _ARG_WS_NO = value; }
            }

            private string _ARG_UPD_USER;
            [DataType(Value = "VARCHAR2")]
            public string ARG_UPD_USER
            {
                get { return _ARG_UPD_USER; }
                set { _ARG_UPD_USER = value; }
            }
        }

        public class TRANSFER_DATA_TO_CDMKR
        {
            private string _ARG_PART_NAME;
            [DataType(Value = "VARCHAR2")]
            public string ARG_PART_NAME
            {
                get { return _ARG_PART_NAME; }
                set { _ARG_PART_NAME = value; }
            }

            private string _ARG_MXSXL_NUMBER;
            [DataType(Value = "VARCHAR2")]
            public string ARG_MXSXL_NUMBER
            {
                get { return _ARG_MXSXL_NUMBER; }
                set { _ARG_MXSXL_NUMBER = value; }
            }

            private string _ARG_MAT_NAME;
            [DataType(Value = "VARCHAR2")]
            public string ARG_MAT_NAME
            {
                get { return _ARG_MAT_NAME; }
                set { _ARG_MAT_NAME = value; }
            }

            private string _ARG_MAT_COMMENT;
            [DataType(Value = "VARCHAR2")]
            public string ARG_MAT_COMMENT
            {
                get { return _ARG_MAT_COMMENT; }
                set { _ARG_MAT_COMMENT = value; }
            }

            private string _ARG_MCS_NUMBER;
            [DataType(Value = "VARCHAR2")]
            public string ARG_MCS_NUMBER
            {
                get { return _ARG_MCS_NUMBER; }
                set { _ARG_MCS_NUMBER = value; }
            }

            private string _ARG_COLOR_CD;
            [DataType(Value = "VARCHAR2")]
            public string ARG_COLOR_CD
            {
                get { return _ARG_COLOR_CD; }
                set { _ARG_COLOR_CD = value; }
            }

            private string _ARG_COLOR_NAME;
            [DataType(Value = "VARCHAR2")]
            public string ARG_COLOR_NAME
            {
                get { return _ARG_COLOR_NAME; }
                set { _ARG_COLOR_NAME = value; }
            }

            private string _ARG_COLOR_COMMENT;
            [DataType(Value = "VARCHAR2")]
            public string ARG_COLOR_COMMENT
            {
                get { return _ARG_COLOR_COMMENT; }
                set { _ARG_COLOR_COMMENT = value; }
            }

            private string _ARG_PCX_MAT_ID;
            [DataType(Value = "VARCHAR2")]
            public string ARG_PCX_MAT_ID
            {
                get { return _ARG_PCX_MAT_ID; }
                set { _ARG_PCX_MAT_ID = value; }
            }

            private string _ARG_PCX_SUPP_MAT_ID;
            [DataType(Value = "VARCHAR2")]
            public string ARG_PCX_SUPP_MAT_ID
            {
                get { return _ARG_PCX_SUPP_MAT_ID; }
                set { _ARG_PCX_SUPP_MAT_ID = value; }
            }

            private string _ARG_REQ_USER;
            [DataType(Value = "VARCHAR2")]
            public string ARG_REQ_USER
            {
                get { return _ARG_REQ_USER; }
                set { _ARG_REQ_USER = value; }
            }

            private string _ARG_FACTORY;
            [DataType(Value = "VARCHAR2")]
            public string ARG_FACTORY
            {
                get { return _ARG_FACTORY; }
                set { _ARG_FACTORY = value; }
            }

            private string _ARG_WS_NO;
            [DataType(Value = "VARCHAR2")]
            public string ARG_WS_NO
            {
                get { return _ARG_WS_NO; }
                set { _ARG_WS_NO = value; }
            }

            private string _ARG_PART_SEQ;
            [DataType(Value = "VARCHAR2")]
            public string ARG_PART_SEQ
            {
                get { return _ARG_PART_SEQ; }
                set { _ARG_PART_SEQ = value; }
            }
        }

        public class UPDATE_BOM_HEAD
        {
            private string _ARG_FACTORY;
            [DataType(Value = "VARCHAR2")]
            public string ARG_FACTORY
            {
                get { return _ARG_FACTORY; }
                set { _ARG_FACTORY = value; }
            }

            private string _ARG_WS_NO;
            [DataType(Value = "VARCHAR2")]
            public string ARG_WS_NO
            {
                get { return _ARG_WS_NO; }
                set { _ARG_WS_NO = value; }
            }

            private string _ARG_DPA;
            [DataType(Value = "VARCHAR2")]
            public string ARG_DPA
            {
                get { return _ARG_DPA; }
                set { _ARG_DPA = value; }
            }

            private string _ARG_PCM_BOM_ID;
            [DataType(Value = "VARCHAR2")]
            public string ARG_PCM_BOM_ID
            {
                get { return _ARG_PCM_BOM_ID; }
                set { _ARG_PCM_BOM_ID = value; }
            }

            private string _ARG_SAMPLE_TYPE;
            [DataType(Value = "VARCHAR2")]
            public string ARG_SAMPLE_TYPE
            {
                get { return _ARG_SAMPLE_TYPE; }
                set { _ARG_SAMPLE_TYPE = value; }
            }

            private string _ARG_SEASON_CD;
            [DataType(Value = "VARCHAR2")]
            public string ARG_SEASON_CD
            {
                get { return _ARG_SEASON_CD; }
                set { _ARG_SEASON_CD = value; }
            }

            //private string _ARG_PCC_ORG;
            //[DataType(Value = "VARCHAR2")]
            //public string ARG_PCC_ORG
            //{
            //    get { return _ARG_PCC_ORG; }
            //    set { _ARG_PCC_ORG = value; }
            //}

            private string _ARG_CATEGORY;
            [DataType(Value = "VARCHAR2")]
            public string ARG_CATEGORY
            {
                get { return _ARG_CATEGORY; }
                set { _ARG_CATEGORY = value; }
            }

            private string _ARG_DEV_NAME;
            [DataType(Value = "VARCHAR2")]
            public string ARG_DEV_NAME
            {
                get { return _ARG_DEV_NAME; }
                set { _ARG_DEV_NAME = value; }
            }

            private string _ARG_PRODUCT_CODE;
            [DataType(Value = "VARCHAR2")]
            public string ARG_PRODUCT_CODE
            {
                get { return _ARG_PRODUCT_CODE; }
                set { _ARG_PRODUCT_CODE = value; }
            }

            private string _ARG_SAMPLE_ETS;
            [DataType(Value = "VARCHAR2")]
            public string ARG_SAMPLE_ETS
            {
                get { return _ARG_SAMPLE_ETS; }
                set { _ARG_SAMPLE_ETS = value; }
            }

            private string _ARG_SAMPLE_QTY;
            [DataType(Value = "VARCHAR2")]
            public string ARG_SAMPLE_QTY
            {
                get { return _ARG_SAMPLE_QTY; }
                set { _ARG_SAMPLE_QTY = value; }
            }

            private string _ARG_SAMPLE_SIZE;
            [DataType(Value = "VARCHAR2")]
            public string ARG_SAMPLE_SIZE
            {
                get { return _ARG_SAMPLE_SIZE; }
                set { _ARG_SAMPLE_SIZE = value; }
            }

            private string _ARG_SUB_TYPE_REMARK;
            [DataType(Value = "VARCHAR2")]
            public string ARG_SUB_TYPE_REMARK
            {
                get { return _ARG_SUB_TYPE_REMARK; }
                set { _ARG_SUB_TYPE_REMARK = value; }
            }

            private string _ARG_GENDER;
            [DataType(Value = "VARCHAR2")]
            public string ARG_GENDER
            {
                get { return _ARG_GENDER; }
                set { _ARG_GENDER = value; }
            }

            private string _ARG_TD;
            [DataType(Value = "VARCHAR2")]
            public string ARG_TD
            {
                get { return _ARG_TD; }
                set { _ARG_TD = value; }
            }

            private string _ARG_COLOR_VER;
            [DataType(Value = "VARCHAR2")]
            public string ARG_COLOR_VER
            {
                get { return _ARG_COLOR_VER; }
                set { _ARG_COLOR_VER = value; }
            }

            private string _ARG_MODEL_ID;
            [DataType(Value = "VARCHAR2")]
            public string ARG_MODEL_ID
            {
                get { return _ARG_MODEL_ID; }
                set { _ARG_MODEL_ID = value; }
            }

            private string _ARG_PCC_PM;
            [DataType(Value = "VARCHAR2")]
            public string ARG_PCC_PM
            {
                get { return _ARG_PCC_PM; }
                set { _ARG_PCC_PM = value; }
            }

            private string _ARG_DEV_COLORWAY_ID;
            [DataType(Value = "VARCHAR2")]
            public string ARG_DEV_COLORWAY_ID
            {
                get { return _ARG_DEV_COLORWAY_ID; }
                set { _ARG_DEV_COLORWAY_ID = value; }
            }

            private string _ARG_PRODUCT_ID;
            [DataType(Value = "VARCHAR2")]
            public string ARG_PRODUCT_ID
            {
                get { return _ARG_PRODUCT_ID; }
                set { _ARG_PRODUCT_ID = value; }
            }

            private string _ARG_DEV_SAMPLE_REQ_ID;
            [DataType(Value = "VARCHAR2")]
            public string ARG_DEV_SAMPLE_REQ_ID
            {
                get { return _ARG_DEV_SAMPLE_REQ_ID; }
                set { _ARG_DEV_SAMPLE_REQ_ID = value; }
            }

            private string _ARG_STYLE_NUMBER;
            [DataType(Value = "VARCHAR2")]
            public string ARG_STYLE_NUMBER
            {
                get { return _ARG_STYLE_NUMBER; }
                set { _ARG_STYLE_NUMBER = value; }
            }

            private string _ARG_GEL_YN;
            [DataType(Value = "VARCHAR2")]
            public string ARG_GEL_YN
            {
                get { return _ARG_GEL_YN; }
                set { _ARG_GEL_YN = value; }
            }
        }

        public class UPDATE_BOM_TAIL
        {
            private string _ARG_FACTORY;
            [DataType(Value = "VARCHAR2")]
            public string ARG_FACTORY
            {
                get { return _ARG_FACTORY; }
                set { _ARG_FACTORY = value; }
            }

            private string _ARG_WS_NO;
            [DataType(Value = "VARCHAR2")]
            public string ARG_WS_NO
            {
                get { return _ARG_WS_NO; }
                set { _ARG_WS_NO = value; }
            }

            private string _ARG_PART_SEQ;
            [DataType(Value = "VARCHAR2")]
            public string ARG_PART_SEQ
            {
                get { return _ARG_PART_SEQ; }
                set { _ARG_PART_SEQ = value; }
            }

            private string _ARG_PART_NIKE_NO;
            [DataType(Value = "VARCHAR2")]
            public string ARG_PART_NIKE_NO
            {
                get { return _ARG_PART_NIKE_NO; }
                set { _ARG_PART_NIKE_NO = value; }
            }

            private string _ARG_PART_NIKE_COMMENT;
            [DataType(Value = "VARCHAR2")]
            public string ARG_PART_NIKE_COMMENT
            {
                get { return _ARG_PART_NIKE_COMMENT; }
                set { _ARG_PART_NIKE_COMMENT = value; }
            }

            private string _ARG_PART_CD;
            [DataType(Value = "VARCHAR2")]
            public string ARG_PART_CD
            {
                get { return _ARG_PART_CD; }
                set { _ARG_PART_CD = value; }
            }

            private string _ARG_PART_NAME;
            [DataType(Value = "VARCHAR2")]
            public string ARG_PART_NAME
            {
                get { return _ARG_PART_NAME; }
                set { _ARG_PART_NAME = value; }
            }

            private string _ARG_PART_TYPE;
            [DataType(Value = "VARCHAR2")]
            public string ARG_PART_TYPE
            {
                get { return _ARG_PART_TYPE; }
                set { _ARG_PART_TYPE = value; }
            }

            private string _ARG_BTTM;
            [DataType(Value = "VARCHAR2")]
            public string ARG_BTTM
            {
                get { return _ARG_BTTM; }
                set { _ARG_BTTM = value; }
            }

            private string _ARG_MXSXL_NUMBER;
            [DataType(Value = "VARCHAR2")]
            public string ARG_MXSXL_NUMBER
            {
                get { return _ARG_MXSXL_NUMBER; }
                set { _ARG_MXSXL_NUMBER = value; }
            }

            private string _ARG_CS_CD;
            [DataType(Value = "VARCHAR2")]
            public string ARG_CS_CD
            {
                get { return _ARG_CS_CD; }
                set { _ARG_CS_CD = value; }
            }

            private string _ARG_MCS_NUMBER;
            [DataType(Value = "VARCHAR2")]
            public string ARG_MCS_NUMBER
            {
                get { return _ARG_MCS_NUMBER; }
                set { _ARG_MCS_NUMBER = value; }
            }

            private string _ARG_MAT_CD;
            [DataType(Value = "VARCHAR2")]
            public string ARG_MAT_CD
            {
                get { return _ARG_MAT_CD; }
                set { _ARG_MAT_CD = value; }
            }

            private string _ARG_MAT_NAME;
            [DataType(Value = "VARCHAR2")]
            public string ARG_MAT_NAME
            {
                get { return _ARG_MAT_NAME; }
                set { _ARG_MAT_NAME = value; }
            }

            private string _ARG_MAT_COMMENT;
            [DataType(Value = "VARCHAR2")]
            public string ARG_MAT_COMMENT
            {
                get { return _ARG_MAT_COMMENT; }
                set { _ARG_MAT_COMMENT = value; }
            }

            private string _ARG_COLOR_CD;
            [DataType(Value = "VARCHAR2")]
            public string ARG_COLOR_CD
            {
                get { return _ARG_COLOR_CD; }
                set { _ARG_COLOR_CD = value; }
            }

            private string _ARG_COLOR_NAME;
            [DataType(Value = "VARCHAR2")]
            public string ARG_COLOR_NAME
            {
                get { return _ARG_COLOR_NAME; }
                set { _ARG_COLOR_NAME = value; }
            }

            private string _ARG_COLOR_COMMENT;
            [DataType(Value = "VARCHAR2")]
            public string ARG_COLOR_COMMENT
            {
                get { return _ARG_COLOR_COMMENT; }
                set { _ARG_COLOR_COMMENT = value; }
            }

            private string _ARG_SORT_NO;
            [DataType(Value = "VARCHAR2")]
            public string ARG_SORT_NO
            {
                get { return _ARG_SORT_NO; }
                set { _ARG_SORT_NO = value; }
            }

            private string _ARG_REMARKS;
            [DataType(Value = "VARCHAR2")]
            public string ARG_REMARKS
            {
                get { return _ARG_REMARKS; }
                set { _ARG_REMARKS = value; }
            }

            private string _ARG_UPD_USER;
            [DataType(Value = "VARCHAR2")]
            public string ARG_UPD_USER
            {
                get { return _ARG_UPD_USER; }
                set { _ARG_UPD_USER = value; }
            }

            private string _ARG_PTRN_PART_NAME;
            [DataType(Value = "VARCHAR2")]
            public string ARG_PTRN_PART_NAME
            {
                get { return _ARG_PTRN_PART_NAME; }
                set { _ARG_PTRN_PART_NAME = value; }
            }

            private string _ARG_PCX_MAT_ID;
            [DataType(Value = "VARCHAR2")]
            public string ARG_PCX_MAT_ID
            {
                get { return _ARG_PCX_MAT_ID; }
                set { _ARG_PCX_MAT_ID = value; }
            }

            private string _ARG_PCX_SUPP_MAT_ID;
            [DataType(Value = "VARCHAR2")]
            public string ARG_PCX_SUPP_MAT_ID
            {
                get { return _ARG_PCX_SUPP_MAT_ID; }
                set { _ARG_PCX_SUPP_MAT_ID = value; }
            }

            private string _ARG_PCX_COLOR_ID;
            [DataType(Value = "VARCHAR2")]
            public string ARG_PCX_COLOR_ID
            {
                get { return _ARG_PCX_COLOR_ID; }
                set { _ARG_PCX_COLOR_ID = value; }
            }

            private string _ARG_ROW_STATUS;
            [DataType(Value = "VARCHAR2")]
            public string ARG_ROW_STATUS
            {
                get { return _ARG_ROW_STATUS; }
                set { _ARG_ROW_STATUS = value; }
            }

            private string _ARG_PROCESS;
            [DataType(Value = "VARCHAR2")]
            public string ARG_PROCESS
            {
                get { return _ARG_PROCESS; }
                set { _ARG_PROCESS = value; }
            }

            private string _ARG_VENDOR_NAME;
            [DataType(Value = "VARCHAR2")]
            public string ARG_VENDOR_NAME
            {
                get { return _ARG_VENDOR_NAME; }
                set { _ARG_VENDOR_NAME = value; }
            }

            private string _ARG_COMBINE_YN;
            [DataType(Value = "VARCHAR2")]
            public string ARG_COMBINE_YN
            {
                get { return _ARG_COMBINE_YN; }
                set { _ARG_COMBINE_YN = value; }
            }

            private string _ARG_STICKER_YN;
            [DataType(Value = "VARCHAR2")]
            public string ARG_STICKER_YN
            {
                get { return _ARG_STICKER_YN; }
                set { _ARG_STICKER_YN = value; }
            }

            private string _ARG_PTRN_PART_CD;
            [DataType(Value = "VARCHAR2")]
            public string ARG_PTRN_PART_CD
            {
                get { return _ARG_PTRN_PART_CD; }
                set { _ARG_PTRN_PART_CD = value; }
            }

            private string _ARG_MDSL_CHK;
            [DataType(Value = "VARCHAR2")]
            public string ARG_MDSL_CHK
            {
                get { return _ARG_MDSL_CHK; }
                set { _ARG_MDSL_CHK = value; }
            }

            private string _ARG_OTSL_CHK;
            [DataType(Value = "VARCHAR2")]
            public string ARG_OTSL_CHK
            {
                get { return _ARG_OTSL_CHK; }
                set { _ARG_OTSL_CHK = value; }
            }

            private string _ARG_CELL_COLOR;
            [DataType(Value = "VARCHAR2")]
            public string ARG_CELL_COLOR
            {
                get { return _ARG_CELL_COLOR; }
                set { _ARG_CELL_COLOR = value; }
            }

            private string _ARG_CS_PTRN_CD;
            [DataType(Value = "VARCHAR2")]
            public string ARG_CS_PTRN_CD
            {
                get { return _ARG_CS_PTRN_CD; }
                set { _ARG_CS_PTRN_CD = value; }
            }

            private string _ARG_CS_PTRN_NAME;
            [DataType(Value = "VARCHAR2")]
            public string ARG_CS_PTRN_NAME
            {
                get { return _ARG_CS_PTRN_NAME; }
                set { _ARG_CS_PTRN_NAME = value; }
            }

            private string _ARG_LOGIC_GROUP;
            [DataType(Value = "VARCHAR2")]
            public string ARG_LOGIC_GROUP
            {
                get { return _ARG_LOGIC_GROUP; }
                set { _ARG_LOGIC_GROUP = value; }
            }

            private double _ARG_MAT_FORECAST_PRCNT;
            [DataType(Value = "NUMBER")]
            public double ARG_MAT_FORECAST_PRCNT
            {
                get { return _ARG_MAT_FORECAST_PRCNT; }
                set { _ARG_MAT_FORECAST_PRCNT = value; }
            }

            private double _ARG_COLOR_FORECAST_PRCNT;
            [DataType(Value = "NUMBER")]
            public double ARG_COLOR_FORECAST_PRCNT
            {
                get { return _ARG_COLOR_FORECAST_PRCNT; }
                set { _ARG_COLOR_FORECAST_PRCNT = value; }
            }

            private string _ARG_ENCODED_CMT;
            [DataType(Value = "VARCHAR2")]
            public string ARG_ENCODED_CMT
            {
                get { return _ARG_ENCODED_CMT; }
                set { _ARG_ENCODED_CMT = value; }
            }
        }

        public class UPDATE_PUR_CHK_Y
        {
            private string _ARG_FACTORY;
            [DataType(Value = "VARCHAR2")]
            public string ARG_FACTORY
            {
                get { return _ARG_FACTORY; }
                set { _ARG_FACTORY = value; }
            }

            private string _ARG_WS_NO;
            [DataType(Value = "VARCHAR2")]
            public string ARG_WS_NO
            {
                get { return _ARG_WS_NO; }
                set { _ARG_WS_NO = value; }
            }

            private string _ARG_PART_SEQ;
            [DataType(Value = "VARCHAR2")]
            public string ARG_PART_SEQ
            {
                get { return _ARG_PART_SEQ; }
                set { _ARG_PART_SEQ = value; }
            }

            private string _ARG_TYPE;
            [DataType(Value = "VARCHAR2")]
            public string ARG_TYPE
            {
                get { return _ARG_TYPE; }
                set { _ARG_TYPE = value; }
            }
        }

        public class UPDATE_PUR_CHK_N
        {
            private string _ARG_FACTORY;
            [DataType(Value = "VARCHAR2")]
            public string ARG_FACTORY
            {
                get { return _ARG_FACTORY; }
                set { _ARG_FACTORY = value; }
            }

            private string _ARG_CONCAT_WS_NO;
            [DataType(Value = "VARCHAR2")]
            public string ARG_CONCAT_WS_NO
            {
                get { return _ARG_CONCAT_WS_NO; }
                set { _ARG_CONCAT_WS_NO = value; }
            }
        }

        public class UPDATE_MAT_INFO_BY_PUR
        {
            private string _ARG_WORK_TYPE;
            [DataType(Value = "VARCHAR2")]
            public string ARG_WORK_TYPE
            {
                get { return _ARG_WORK_TYPE; }
                set { _ARG_WORK_TYPE = value; }
            }

            private string _ARG_FACTORY;
            [DataType(Value = "VARCHAR2")]
            public string ARG_FACTORY
            {
                get { return _ARG_FACTORY; }
                set { _ARG_FACTORY = value; }
            }

            private string _ARG_WS_NO;
            [DataType(Value = "VARCHAR2")]
            public string ARG_WS_NO
            {
                get { return _ARG_WS_NO; }
                set { _ARG_WS_NO = value; }
            }

            private string _ARG_PART_SEQ;
            [DataType(Value = "VARCHAR2")]
            public string ARG_PART_SEQ
            {
                get { return _ARG_PART_SEQ; }
                set { _ARG_PART_SEQ = value; }
            }

            private string _ARG_PART_NAME;
            [DataType(Value = "VARCHAR2")]
            public string ARG_PART_NAME
            {
                get { return _ARG_PART_NAME; }
                set { _ARG_PART_NAME = value; }
            }

            private string _ARG_PART_TYPE;
            [DataType(Value = "VARCHAR2")]
            public string ARG_PART_TYPE
            {
                get { return _ARG_PART_TYPE; }
                set { _ARG_PART_TYPE = value; }
            }

            private string _ARG_MXSXL_NUMBER;
            [DataType(Value = "VARCHAR2")]
            public string ARG_MXSXL_NUMBER
            {
                get { return _ARG_MXSXL_NUMBER; }
                set { _ARG_MXSXL_NUMBER = value; }
            }

            private string _ARG_PCX_SUPP_MAT_ID;
            [DataType(Value = "VARCHAR2")]
            public string ARG_PCX_SUPP_MAT_ID
            {
                get { return _ARG_PCX_SUPP_MAT_ID; }
                set { _ARG_PCX_SUPP_MAT_ID = value; }
            }

            private string _ARG_MAT_CD;
            [DataType(Value = "VARCHAR2")]
            public string ARG_MAT_CD
            {
                get { return _ARG_MAT_CD; }
                set { _ARG_MAT_CD = value; }
            }

            private string _ARG_MAT_NAME;
            [DataType(Value = "VARCHAR2")]
            public string ARG_MAT_NAME
            {
                get { return _ARG_MAT_NAME; }
                set { _ARG_MAT_NAME = value; }
            }

            private string _ARG_MAT_COMMENT;
            [DataType(Value = "VARCHAR2")]
            public string ARG_MAT_COMMENT
            {
                get { return _ARG_MAT_COMMENT; }
                set { _ARG_MAT_COMMENT = value; }
            }

            private string _ARG_PCX_COLOR_ID;
            [DataType(Value = "VARCHAR2")]
            public string ARG_PCX_COLOR_ID
            {
                get { return _ARG_PCX_COLOR_ID; }
                set { _ARG_PCX_COLOR_ID = value; }
            }

            private string _ARG_COLOR_CD;
            [DataType(Value = "VARCHAR2")]
            public string ARG_COLOR_CD
            {
                get { return _ARG_COLOR_CD; }
                set { _ARG_COLOR_CD = value; }
            }

            private string _ARG_COLOR_NAME;
            [DataType(Value = "VARCHAR2")]
            public string ARG_COLOR_NAME
            {
                get { return _ARG_COLOR_NAME; }
                set { _ARG_COLOR_NAME = value; }
            }

            private string _ARG_COLOR_COMMENT;
            [DataType(Value = "VARCHAR2")]
            public string ARG_COLOR_COMMENT
            {
                get { return _ARG_COLOR_COMMENT; }
                set { _ARG_COLOR_COMMENT = value; }
            }

            private string _ARG_PCX_MAT_ID;
            [DataType(Value = "VARCHAR2")]
            public string ARG_PCX_MAT_ID
            {
                get { return _ARG_PCX_MAT_ID; }
                set { _ARG_PCX_MAT_ID = value; }
            }

            private string _ARG_CS_CD;
            [DataType(Value = "VARCHAR2")]
            public string ARG_CS_CD
            {
                get { return _ARG_CS_CD; }
                set { _ARG_CS_CD = value; }
            }

            private string _ARG_VENDOR_NAME;
            [DataType(Value = "VARCHAR2")]
            public string ARG_VENDOR_NAME
            {
                get { return _ARG_VENDOR_NAME; }
                set { _ARG_VENDOR_NAME = value; }
            }

            private string _ARG_UPD_USER;
            [DataType(Value = "VARCHAR2")]
            public string ARG_UPD_USER
            {
                get { return _ARG_UPD_USER; }
                set { _ARG_UPD_USER = value; }
            }
        }

        public class UPDATE_BOM_HEAD_BY_PUR
        {
            private string _ARG_FACTORY;
            [DataType(Value = "VARCHAR2")]
            public string ARG_FACTORY
            {
                get { return _ARG_FACTORY; }
                set { _ARG_FACTORY = value; }
            }

            private string _ARG_CHAINED_WS_NO;
            [DataType(Value = "VARCHAR2")]
            public string ARG_CHAINED_WS_NO
            {
                get { return _ARG_CHAINED_WS_NO; }
                set { _ARG_CHAINED_WS_NO = value; }
            }

            private string _ARG_UPD_USER;
            [DataType(Value = "VARCHAR2")]
            public string ARG_UPD_USER
            {
                get { return _ARG_UPD_USER; }
                set { _ARG_UPD_USER = value; }
            }
        }

        public class UPDATE_WS_PURPOSE
        {
            private string _ARG_FACTORY;
            [DataType(Value = "VARCHAR2")]
            public string ARG_FACTORY
            {
                get { return _ARG_FACTORY; }
                set { _ARG_FACTORY = value; }
            }

            private string _ARG_WS_NO;
            [DataType(Value = "VARCHAR2")]
            public string ARG_WS_NO
            {
                get { return _ARG_WS_NO; }
                set { _ARG_WS_NO = value; }
            }

            private string _ARG_PURPOSE;
            [DataType(Value = "VARCHAR2")]
            public string ARG_PURPOSE
            {
                get { return _ARG_PURPOSE; }
                set { _ARG_PURPOSE = value; }
            }
        }

        public class UPDATE_WS_TAG_OTHER
        {
            private string _ARG_FACTORY;
            [DataType(Value = "VARCHAR2")]
            public string ARG_FACTORY
            {
                get { return _ARG_FACTORY; }
                set { _ARG_FACTORY = value; }
            }

            private string _ARG_WS_NO;
            [DataType(Value = "VARCHAR2")]
            public string ARG_WS_NO
            {
                get { return _ARG_WS_NO; }
                set { _ARG_WS_NO = value; }
            }

            private string _ARG_PROD_FACTORY;
            [DataType(Value = "VARCHAR2")]
            public string ARG_PROD_FACTORY
            {
                get { return _ARG_PROD_FACTORY; }
                set { _ARG_PROD_FACTORY = value; }
            }

            private string _ARG_SAMPLE_ETS;
            [DataType(Value = "VARCHAR2")]
            public string ARG_SAMPLE_ETS
            {
                get { return _ARG_SAMPLE_ETS; }
                set { _ARG_SAMPLE_ETS = value; }
            }

            private string _ARG_UPPER_MATERIAL;
            [DataType(Value = "VARCHAR2")]
            public string ARG_UPPER_MATERIAL
            {
                get { return _ARG_UPPER_MATERIAL; }
                set { _ARG_UPPER_MATERIAL = value; }
            }

            private string _ARG_MS_MATERIAL;
            [DataType(Value = "VARCHAR2")]
            public string ARG_MS_MATERIAL
            {
                get { return _ARG_MS_MATERIAL; }
                set { _ARG_MS_MATERIAL = value; }
            }

            private string _ARG_OUTSOLE_MATERIAL;
            [DataType(Value = "VARCHAR2")]
            public string ARG_OUTSOLE_MATERIAL
            {
                get { return _ARG_OUTSOLE_MATERIAL; }
                set { _ARG_OUTSOLE_MATERIAL = value; }
            }

            private string _ARG_LAST_CD;
            [DataType(Value = "VARCHAR2")]
            public string ARG_LAST_CD
            {
                get { return _ARG_LAST_CD; }
                set { _ARG_LAST_CD = value; }
            }

            private string _ARG_PATTERN_ID;
            [DataType(Value = "VARCHAR2")]
            public string ARG_PATTERN_ID
            {
                get { return _ARG_PATTERN_ID; }
                set { _ARG_PATTERN_ID = value; }
            }

            private string _ARG_STL_FILE;
            [DataType(Value = "VARCHAR2")]
            public string ARG_STL_FILE
            {
                get { return _ARG_STL_FILE; }
                set { _ARG_STL_FILE = value; }
            }

            private string _ARG_SAMPLE_WEIGHT;
            [DataType(Value = "VARCHAR2")]
            public string ARG_SAMPLE_WEIGHT
            {
                get { return _ARG_SAMPLE_WEIGHT; }
                set { _ARG_SAMPLE_WEIGHT = value; }
            }

            private string _ARG_HEEL_HEIGHT;
            [DataType(Value = "VARCHAR2")]
            public string ARG_HEEL_HEIGHT
            {
                get { return _ARG_HEEL_HEIGHT; }
                set { _ARG_HEEL_HEIGHT = value; }
            }

            private string _ARG_MEDIAL_HEIGHT;
            [DataType(Value = "VARCHAR2")]
            public string ARG_MEDIAL_HEIGHT
            {
                get { return _ARG_MEDIAL_HEIGHT; }
                set { _ARG_MEDIAL_HEIGHT = value; }
            }

            private string _ARG_LATERAL_HEIGHT;
            [DataType(Value = "VARCHAR2")]
            public string ARG_LATERAL_HEIGHT
            {
                get { return _ARG_LATERAL_HEIGHT; }
                set { _ARG_LATERAL_HEIGHT = value; }
            }

            private string _ARG_LACE_LENGTH;
            [DataType(Value = "VARCHAR2")]
            public string ARG_LACE_LENGTH
            {
                get { return _ARG_LACE_LENGTH; }
                set { _ARG_LACE_LENGTH = value; }
            }

            private string _ARG_MS_HARDNESS;
            [DataType(Value = "VARCHAR2")]
            public string ARG_MS_HARDNESS
            {
                get { return _ARG_MS_HARDNESS; }
                set { _ARG_MS_HARDNESS = value; }
            }

            private string _ARG_IDS_LENGTH;
            [DataType(Value = "VARCHAR2")]
            public string ARG_IDS_LENGTH
            {
                get { return _ARG_IDS_LENGTH; }
                set { _ARG_IDS_LENGTH = value; }
            }

            private string _ARG_LENGTH_TOE_SPRING;
            [DataType(Value = "VARCHAR2")]
            public string ARG_LENGTH_TOE_SPRING
            {
                get { return _ARG_LENGTH_TOE_SPRING; }
                set { _ARG_LENGTH_TOE_SPRING = value; }
            }

            private string _ARG_MS_CODE;
            [DataType(Value = "VARCHAR2")]
            public string ARG_MS_CODE
            {
                get { return _ARG_MS_CODE; }
                set { _ARG_MS_CODE = value; }
            }

            private string _ARG_OS_CODE;
            [DataType(Value = "VARCHAR2")]
            public string ARG_OS_CODE
            {
                get { return _ARG_OS_CODE; }
                set { _ARG_OS_CODE = value; }
            }

            private string _ARG_WS_QTY;
            [DataType(Value = "VARCHAR2")]
            public string ARG_WS_QTY
            {
                get { return _ARG_WS_QTY; }
                set { _ARG_WS_QTY = value; }
            }

            private string _ARG_NIKE_SEND_QTY;
            [DataType(Value = "VARCHAR2")]
            public string ARG_NIKE_SEND_QTY
            {
                get { return _ARG_NIKE_SEND_QTY; }
                set { _ARG_NIKE_SEND_QTY = value; }
            }

            private string _ARG_DPA;
            [DataType(Value = "VARCHAR2")]
            public string ARG_DPA
            {
                get { return _ARG_DPA; }
                set { _ARG_DPA = value; }
            }

            private string _ARG_NIKE_DEV;
            [DataType(Value = "VARCHAR2")]
            public string ARG_NIKE_DEV
            {
                get { return _ARG_NIKE_DEV; }
                set { _ARG_NIKE_DEV = value; }
            }

            private string _ARG_WHQ_DEV;
            [DataType(Value = "VARCHAR2")]
            public string ARG_WHQ_DEV
            {
                get { return _ARG_WHQ_DEV; }
                set { _ARG_WHQ_DEV = value; }
            }

            private string _ARG_DEV_SAMPLE_REQ_ID;
            [DataType(Value = "VARCHAR2")]
            public string ARG_DEV_SAMPLE_REQ_ID
            {
                get { return _ARG_DEV_SAMPLE_REQ_ID; }
                set { _ARG_DEV_SAMPLE_REQ_ID = value; }
            }

            private string _ARG_IPW;
            [DataType(Value = "VARCHAR2")]
            public string ARG_IPW
            {
                get { return _ARG_IPW; }
                set { _ARG_IPW = value; }
            }

            private string _ARG_BOM_ID;
            [DataType(Value = "VARCHAR2")]
            public string ARG_BOM_ID
            {
                get { return _ARG_BOM_ID; }
                set { _ARG_BOM_ID = value; }
            }

            private string _ARG_NEEDLE_SIZE;
            [DataType(Value = "VARCHAR2")]
            public string ARG_NEEDLE_SIZE
            {
                get { return _ARG_NEEDLE_SIZE; }
                set { _ARG_NEEDLE_SIZE = value; }
            }

            private string _ARG_SPI;
            [DataType(Value = "VARCHAR2")]
            public string ARG_SPI
            {
                get { return _ARG_SPI; }
                set { _ARG_SPI = value; }
            }

            private string _ARG_STITCHING_MARGIN;
            [DataType(Value = "VARCHAR2")]
            public string ARG_STITCHING_MARGIN
            {
                get { return _ARG_STITCHING_MARGIN; }
                set { _ARG_STITCHING_MARGIN = value; }
            }

            private string _ARG_TWOROW_STITCHING_MARGIN;
            [DataType(Value = "VARCHAR2")]
            public string ARG_TWOROW_STITCHING_MARGIN
            {
                get { return _ARG_TWOROW_STITCHING_MARGIN; }
                set { _ARG_TWOROW_STITCHING_MARGIN = value; }
            }

            private string _ARG_THREAD_TYPE;
            [DataType(Value = "VARCHAR2")]
            public string ARG_THREAD_TYPE
            {
                get { return _ARG_THREAD_TYPE; }
                set { _ARG_THREAD_TYPE = value; }
            }

            private string _ARG_PROTO_YN;
            [DataType(Value = "VARCHAR2")]
            public string ARG_PROTO_YN
            {
                get { return _ARG_PROTO_YN; }
                set { _ARG_PROTO_YN = value; }
            }

            private string _ARG_WATER_JET_YN;
            [DataType(Value = "VARCHAR2")]
            public string ARG_WATER_JET_YN
            {
                get { return _ARG_WATER_JET_YN; }
                set { _ARG_WATER_JET_YN = value; }
            }

            private string _ARG_DCS;
            [DataType(Value = "VARCHAR2")]
            public string ARG_DCS
            {
                get { return _ARG_DCS; }
                set { _ARG_DCS = value; }
            }

            private string _ARG_REWORK_YN;
            [DataType(Value = "VARCHAR2")]
            public string ARG_REWORK_YN
            {
                get { return _ARG_REWORK_YN; }
                set { _ARG_REWORK_YN = value; }
            }

            private string _ARG_INNOVATION_YN;
            [DataType(Value = "VARCHAR2")]
            public string ARG_INNOVATION_YN
            {
                get { return _ARG_INNOVATION_YN; }
                set { _ARG_INNOVATION_YN = value; }
            }

            private string _ARG_CBD_YN;
            [DataType(Value = "VARCHAR2")]
            public string ARG_CBD_YN
            {
                get { return _ARG_CBD_YN; }
                set { _ARG_CBD_YN = value; }
            }

            private string _ARG_UPD_USER;
            [DataType(Value = "VARCHAR2")]
            public string ARG_UPD_USER
            {
                get { return _ARG_UPD_USER; }
                set { _ARG_UPD_USER = value; }
            }

            private string _ARG_SUB_TYPE_REMARK;
            [DataType(Value = "VARCHAR2")]
            public string ARG_SUB_TYPE_REMARK
            {
                get { return _ARG_SUB_TYPE_REMARK; }
                set { _ARG_SUB_TYPE_REMARK = value; }
            }

            private string _ARG_REWORK_COMMENT;
            [DataType(Value = "VARCHAR2")]
            public string ARG_REWORK_COMMENT
            {
                get { return _ARG_REWORK_COMMENT; }
                set { _ARG_REWORK_COMMENT = value; }
            }
        }

        public class UPDATE_WS_COMMENTS
        {
            private string _ARG_FACTORY;
            [DataType(Value = "VARCHAR2")]
            public string ARG_FACTORY
            {
                get { return _ARG_FACTORY; }
                set { _ARG_FACTORY = value; }
            }

            private string _ARG_WS_NO;
            [DataType(Value = "VARCHAR2")]
            public string ARG_WS_NO
            {
                get { return _ARG_WS_NO; }
                set { _ARG_WS_NO = value; }
            }

            private string _ARG_SEQ;
            [DataType(Value = "VARCHAR2")]
            public string ARG_SEQ
            {
                get { return _ARG_SEQ; }
                set { _ARG_SEQ = value; }
            }

            private string _ARG_COMMENT;
            [DataType(Value = "VARCHAR2")]
            public string ARG_COMMENT
            {
                get { return _ARG_COMMENT; }
                set { _ARG_COMMENT = value; }
            }

            private string _ARG_UPD_USER;
            [DataType(Value = "VARCHAR2")]
            public string ARG_UPD_USER
            {
                get { return _ARG_UPD_USER; }
                set { _ARG_UPD_USER = value; }
            }
        }

        public class UPDATE_WS_OPCD
        {
            private string _ARG_FACTORY;
            [DataType(Value = "VARCHAR2")]
            public string ARG_FACTORY
            {
                get { return _ARG_FACTORY; }
                set { _ARG_FACTORY = value; }
            }

            private string _ARG_WS_NO;
            [DataType(Value = "VARCHAR2")]
            public string ARG_WS_NO
            {
                get { return _ARG_WS_NO; }
                set { _ARG_WS_NO = value; }
            }

            private string _ARG_OP_CD;
            [DataType(Value = "VARCHAR2")]
            public string ARG_OP_CD
            {
                get { return _ARG_OP_CD; }
                set { _ARG_OP_CD = value; }
            }

            private string _ARG_OP_CHK;
            [DataType(Value = "VARCHAR2")]
            public string ARG_OP_CHK
            {
                get { return _ARG_OP_CHK; }
                set { _ARG_OP_CHK = value; }
            }

            private string _ARG_OP_QTY;
            [DataType(Value = "VARCHAR2")]
            public string ARG_OP_QTY
            {
                get { return _ARG_OP_QTY; }
                set { _ARG_OP_QTY = value; }
            }

            private string _ARG_OP_YMD;
            [DataType(Value = "VARCHAR2")]
            public string ARG_OP_YMD
            {
                get { return _ARG_OP_YMD; }
                set { _ARG_OP_YMD = value; }
            }

            private string _ARG_UPD_USER;
            [DataType(Value = "VARCHAR2")]
            public string ARG_UPD_USER
            {
                get { return _ARG_UPD_USER; }
                set { _ARG_UPD_USER = value; }
            }
        }

        public class UPDATE_BOM_CFM_INFO
        {
            private string _ARG_FACTORY;
            [DataType(Value = "VARCHAR2")]
            public string ARG_FACTORY
            {
                get { return _ARG_FACTORY; }
                set { _ARG_FACTORY = value; }
            }

            private string _ARG_WS_NO;
            [DataType(Value = "VARCHAR2")]
            public string ARG_WS_NO
            {
                get { return _ARG_WS_NO; }
                set { _ARG_WS_NO = value; }
            }

            private string _ARG_CS_BOM_CFM;
            [DataType(Value = "VARCHAR2")]
            public string ARG_CS_BOM_CFM
            {
                get { return _ARG_CS_BOM_CFM; }
                set { _ARG_CS_BOM_CFM = value; }
            }

            private string _ARG_CBD_YN;
            [DataType(Value = "VARCHAR2")]
            public string ARG_CBD_YN
            {
                get { return _ARG_CBD_YN; }
                set { _ARG_CBD_YN = value; }
            }

            private string _ARG_UPD_USER;
            [DataType(Value = "VARCHAR2")]
            public string ARG_UPD_USER
            {
                get { return _ARG_UPD_USER; }
                set { _ARG_UPD_USER = value; }
            }
        }

        public class UPDATE_INIT_SEQ_WS_NO
        {
            private string _ARG_WORK_TYPE;
            [DataType(Value = "VARCHAR2")]
            public string ARG_WORK_TYPE
            {
                get { return _ARG_WORK_TYPE; }
                set { _ARG_WORK_TYPE = value; }
            }

            private string _ARG_FACTORY;
            [DataType(Value = "VARCHAR2")]
            public string ARG_FACTORY
            {
                get { return _ARG_FACTORY; }
                set { _ARG_FACTORY = value; }
            }

            private string _ARG_UPD_USER;
            [DataType(Value = "VARCHAR2")]
            public string ARG_UPD_USER
            {
                get { return _ARG_UPD_USER; }
                set { _ARG_UPD_USER = value; }
            }
        }

        public class UPDATE_WS_RELEASE_BOM
        {
            private string _ARG_FACTORY;
            [DataType(Value = "VARCHAR2")]
            public string ARG_FACTORY
            {
                get { return _ARG_FACTORY; }
                set { _ARG_FACTORY = value; }
            }

            private string _ARG_WS_NO;
            [DataType(Value = "VARCHAR2")]
            public string ARG_WS_NO
            {
                get { return _ARG_WS_NO; }
                set { _ARG_WS_NO = value; }
            }

            private string _ARG_UPD_USER;
            [DataType(Value = "VARCHAR2")]
            public string ARG_UPD_USER
            {
                get { return _ARG_UPD_USER; }
                set { _ARG_UPD_USER = value; }
            }
        }

        public class DELETE_BOM_DATA
        {
            private string _ARG_PCC;
            [DataType(Value = "VARCHAR2")]
            public string ARG_PCC
            {
                get { return _ARG_PCC; }
                set { _ARG_PCC = value; }
            }

            private string _ARG_CONCAT_WS_NO;
            [DataType(Value = "VARCHAR2")]
            public string ARG_CONCAT_WS_NO
            {
                get { return _ARG_CONCAT_WS_NO; }
                set { _ARG_CONCAT_WS_NO = value; }
            }

            private string _ARG_DELETE_USER;
            [DataType(Value = "VARCHAR2")]
            public string ARG_DELETE_USER
            {
                get { return _ARG_DELETE_USER; }
                set { _ARG_DELETE_USER = value; }
            }
        }

        public class DELETE_MATERIALS_TO_EMPTY
        {
            private string _ARG_FACTORY;
            [DataType(Value = "VARCHAR2")]
            public string ARG_FACTORY
            {
                get { return _ARG_FACTORY; }
                set { _ARG_FACTORY = value; }
            }

            private string _ARG_CONCAT_WS_NO;
            [DataType(Value = "VARCHAR2")]
            public string ARG_CONCAT_WS_NO
            {
                get { return _ARG_CONCAT_WS_NO; }
                set { _ARG_CONCAT_WS_NO = value; }
            }

            private string _ARG_LOCATION;
            [DataType(Value = "VARCHAR2")]
            public string ARG_LOCATION
            {
                get { return _ARG_LOCATION; }
                set { _ARG_LOCATION = value; }
            }
        }

        public class DELETE_MATERIALS_TO_PUR
        {
            private string _ARG_FACTORY;
            [DataType(Value = "VARCHAR2")]
            public string ARG_FACTORY
            {
                get { return _ARG_FACTORY; }
                set { _ARG_FACTORY = value; }
            }

            private string _ARG_WS_NO;
            [DataType(Value = "VARCHAR2")]
            public string ARG_WS_NO
            {
                get { return _ARG_WS_NO; }
                set { _ARG_WS_NO = value; }
            }

            private string _ARG_PART_SEQ;
            [DataType(Value = "VARCHAR2")]
            public string ARG_PART_SEQ
            {
                get { return _ARG_PART_SEQ; }
                set { _ARG_PART_SEQ = value; }
            }
        }

        public class DELETE_BOM_IMAGE
        {
            private string _ARG_FACTORY;
            [DataType(Value = "VARCHAR2")]
            public string ARG_FACTORY
            {
                get { return _ARG_FACTORY; }
                set { _ARG_FACTORY = value; }
            }

            private string _ARG_WS_NO;
            [DataType(Value = "VARCHAR2")]
            public string ARG_WS_NO
            {
                get { return _ARG_WS_NO; }
                set { _ARG_WS_NO = value; }
            }
        }

        public class DELETE_SMS_USER
        {
            private string _ARG_FACTORY;
            [DataType(Value = "VARCHAR2")]
            public string ARG_FACTORY
            {
                get { return _ARG_FACTORY; }
                set { _ARG_FACTORY = value; }
            }

            private string _ARG_WS_NO;
            [DataType(Value = "VARCHAR2")]
            public string ARG_WS_NO
            {
                get { return _ARG_WS_NO; }
                set { _ARG_WS_NO = value; }
            }

            private string _ARG_USER_ID;
            [DataType(Value = "VARCHAR2")]
            public string ARG_USER_ID
            {
                get { return _ARG_USER_ID; }
                set { _ARG_USER_ID = value; }
            }
        }

        public class CANCEL_RELEASE_BOM
        {
            private string _ARG_WORK_TYPE;
            [DataType(Value = "VARCHAR2")]
            public string ARG_WORK_TYPE
            {
                get { return _ARG_WORK_TYPE; }
                set { _ARG_WORK_TYPE = value; }
            }

            private string _ARG_FACTORY;
            [DataType(Value = "VARCHAR2")]
            public string ARG_FACTORY
            {
                get { return _ARG_FACTORY; }
                set { _ARG_FACTORY = value; }
            }

            private string _ARG_WS_NO;
            [DataType(Value = "VARCHAR2")]
            public string ARG_WS_NO
            {
                get { return _ARG_WS_NO; }
                set { _ARG_WS_NO = value; }
            }

            private string _ARG_UPD_USER;
            [DataType(Value = "VARCHAR2")]
            public string ARG_UPD_USER
            {
                get { return _ARG_UPD_USER; }
                set { _ARG_UPD_USER = value; }
            }

            private string _OUT_CURSOR;
            [DataType(Value = "REF CURSOR")]
            public string OUT_CURSOR
            {
                get { return _OUT_CURSOR; }
                set { _OUT_CURSOR = value; }
            }
        }

        public class PART_LIST_FOR_PE
        {
            private string _ARG_WORK_TYPE;
            [DataType(Value = "VARCHAR2")]
            public string ARG_WORK_TYPE
            {
                get { return _ARG_WORK_TYPE; }
                set { _ARG_WORK_TYPE = value; }
            }

            private string _ARG_FACTORY;
            [DataType(Value = "VARCHAR2")]
            public string ARG_FACTORY
            {
                get { return _ARG_FACTORY; }
                set { _ARG_FACTORY = value; }
            }

            private string _ARG_WS_NO;
            [DataType(Value = "VARCHAR2")]
            public string ARG_WS_NO
            {
                get { return _ARG_WS_NO; }
                set { _ARG_WS_NO = value; }
            }

            private string _OUT_CURSOR;
            [DataType(Value = "REF CURSOR")]
            public string OUT_CURSOR
            {
                get { return _OUT_CURSOR; }
                set { _OUT_CURSOR = value; }
            }
        }

        public class BOM_IMPORT
        {
            private string _ARG_WORK_TYPE;
            [DataType(Value = "VARCHAR2")]
            public string ARG_WORK_TYPE
            {
                get { return _ARG_WORK_TYPE; }
                set { _ARG_WORK_TYPE = value; }
            }

            private string _ARG_FACTORY;
            [DataType(Value = "VARCHAR2")]
            public string ARG_FACTORY
            {
                get { return _ARG_FACTORY; }
                set { _ARG_FACTORY = value; }
            }

            private string _ARG_WS_NO;
            [DataType(Value = "VARCHAR2")]
            public string ARG_WS_NO
            {
                get { return _ARG_WS_NO; }
                set { _ARG_WS_NO = value; }
            }

            private string _ARG_UPD_USER;
            [DataType(Value = "VARCHAR2")]
            public string ARG_UPD_USER
            {
                get { return _ARG_UPD_USER; }
                set { _ARG_UPD_USER = value; }
            }

            private string _OUT_CURSOR;
            [DataType(Value = "REF CURSOR")]
            public string OUT_CURSOR
            {
                get { return _OUT_CURSOR; }
                set { _OUT_CURSOR = value; }
            }
        }

        public class BOM_LOCK
        {
            private string _ARG_WORK_TYPE;
            [DataType(Value = "VARCHAR2")]
            public string ARG_WORK_TYPE
            {
                get { return _ARG_WORK_TYPE; }
                set { _ARG_WORK_TYPE = value; }
            }

            private string _ARG_FACTORY;
            [DataType(Value = "VARCHAR2")]
            public string ARG_FACTORY
            {
                get { return _ARG_FACTORY; }
                set { _ARG_FACTORY = value; }
            }

            private string _ARG_WS_NO;
            [DataType(Value = "VARCHAR2")]
            public string ARG_WS_NO
            {
                get { return _ARG_WS_NO; }
                set { _ARG_WS_NO = value; }
            }

            private string _OUT_CURSOR;
            [DataType(Value = "REF CURSOR")]
            public string OUT_CURSOR
            {
                get { return _OUT_CURSOR; }
                set { _OUT_CURSOR = value; }
            }
        }

        public class GET_PIC_FROM_PMX
        {
            private string _ARG_WORK_TYPE;
            [DataType(Value = "VARCHAR2")]
            public string ARG_WORK_TYPE
            {
                get { return _ARG_WORK_TYPE; }
                set { _ARG_WORK_TYPE = value; }
            }

            private string _ARG_DPA;
            [DataType(Value = "VARCHAR2")]
            public string ARG_DPA
            {
                get { return _ARG_DPA; }
                set { _ARG_DPA = value; }
            }

            private string _ARG_JOB;
            [DataType(Value = "VARCHAR2")]
            public string ARG_JOB
            {
                get { return _ARG_JOB; }
                set { _ARG_JOB = value; }
            }

            private string _OUT_CURSOR;
            [DataType(Value = "REF CURSOR")]
            public string OUT_CURSOR
            {
                get { return _OUT_CURSOR; }
                set { _OUT_CURSOR = value; }
            }
        }
    }

    public class PKG_INTG_COMPARE_BOM
    {
        public class SELECT_LONGEST_BOM
        {
            private string _ARG_FACTORY;
            [DataType(Value = "VARCHAR2")]
            public string ARG_FACTORY
            {
                get { return _ARG_FACTORY; }
                set { _ARG_FACTORY = value; }
            }

            private string _ARG_CHAINED_WS_NO;
            [DataType(Value = "VARCHAR2")]
            public string ARG_CHAINED_WS_NO
            {
                get { return _ARG_CHAINED_WS_NO; }
                set { _ARG_CHAINED_WS_NO = value; }
            }

            private string _OUT_CURSOR;
            [DataType(Value = "REF CURSOR")]
            public string OUT_CURSOR
            {
                get { return _OUT_CURSOR; }
                set { _OUT_CURSOR = value; }
            }
        }

        public class SELECT_COMPARE_VLDTN
        {
            private string _ARG_WORK_TYPE;
            [DataType(Value = "VARCHAR2")]
            public string ARG_WORK_TYPE
            {
                get { return _ARG_WORK_TYPE; }
                set { _ARG_WORK_TYPE = value; }
            }

            private string _ARG_FACTORY;
            [DataType(Value = "VARCHAR2")]
            public string ARG_FACTORY
            {
                get { return _ARG_FACTORY; }
                set { _ARG_FACTORY = value; }
            }

            private string _ARG_CHAINED_WS_NO;
            [DataType(Value = "VARCHAR2")]
            public string ARG_CHAINED_WS_NO
            {
                get { return _ARG_CHAINED_WS_NO; }
                set { _ARG_CHAINED_WS_NO = value; }
            }

            private string _OUT_CURSOR;
            [DataType(Value = "REF CURSOR")]
            public string OUT_CURSOR
            {
                get { return _OUT_CURSOR; }
                set { _OUT_CURSOR = value; }
            }
        }

        public class SELECT_SOURCE_DATA
        {
            private string _ARG_FACTORY;
            [DataType(Value = "VARCHAR2")]
            public string ARG_FACTORY
            {
                get { return _ARG_FACTORY; }
                set { _ARG_FACTORY = value; }
            }

            private string _ARG_BASE_WS_NO;
            [DataType(Value = "VARCHAR2")]
            public string ARG_BASE_WS_NO
            {
                get { return _ARG_BASE_WS_NO; }
                set { _ARG_BASE_WS_NO = value; }
            }

            private string _ARG_SUB_WS_NO;
            [DataType(Value = "VARCHAR2")]
            public string ARG_SUB_WS_NO
            {
                get { return _ARG_SUB_WS_NO; }
                set { _ARG_SUB_WS_NO = value; }
            }

            private string _OUT_CURSOR;
            [DataType(Value = "REF CURSOR")]
            public string OUT_CURSOR
            {
                get { return _OUT_CURSOR; }
                set { _OUT_CURSOR = value; }
            }
        }

        public class UPDATE_BOM_TAIL_DATA
        {
            private string _ARG_FACTORY;
            [DataType(Value = "VARCHAR2")]
            public string ARG_FACTORY
            {
                get { return _ARG_FACTORY; }
                set { _ARG_FACTORY = value; }
            }

            private string _ARG_WS_NO;
            [DataType(Value = "VARCHAR2")]
            public string ARG_WS_NO
            {
                get { return _ARG_WS_NO; }
                set { _ARG_WS_NO = value; }
            }

            private string _ARG_PART_SEQ;
            [DataType(Value = "VARCHAR2")]
            public string ARG_PART_SEQ
            {
                get { return _ARG_PART_SEQ; }
                set { _ARG_PART_SEQ = value; }
            }

            private string _ARG_NIKE_COMMENT;
            [DataType(Value = "VARCHAR2")]
            public string ARG_NIKE_COMMENT
            {
                get { return _ARG_NIKE_COMMENT; }
                set { _ARG_NIKE_COMMENT = value; }
            }

            private string _ARG_PART_CD;
            [DataType(Value = "VARCHAR2")]
            public string ARG_PART_CD
            {
                get { return _ARG_PART_CD; }
                set { _ARG_PART_CD = value; }
            }

            private string _ARG_PART_NAME;
            [DataType(Value = "VARCHAR2")]
            public string ARG_PART_NAME
            {
                get { return _ARG_PART_NAME; }
                set { _ARG_PART_NAME = value; }
            }

            private string _ARG_PART_TYPE;
            [DataType(Value = "VARCHAR2")]
            public string ARG_PART_TYPE
            {
                get { return _ARG_PART_TYPE; }
                set { _ARG_PART_TYPE = value; }
            }

            private string _ARG_BTTM;
            [DataType(Value = "VARCHAR2")]
            public string ARG_BTTM
            {
                get { return _ARG_BTTM; }
                set { _ARG_BTTM = value; }
            }

            private string _ARG_MXSXL_NUMBER;
            [DataType(Value = "VARCHAR2")]
            public string ARG_MXSXL_NUMBER
            {
                get { return _ARG_MXSXL_NUMBER; }
                set { _ARG_MXSXL_NUMBER = value; }
            }

            private string _ARG_CS_CD;
            [DataType(Value = "VARCHAR2")]
            public string ARG_CS_CD
            {
                get { return _ARG_CS_CD; }
                set { _ARG_CS_CD = value; }
            }

            private string _ARG_MAT_CD;
            [DataType(Value = "VARCHAR2")]
            public string ARG_MAT_CD
            {
                get { return _ARG_MAT_CD; }
                set { _ARG_MAT_CD = value; }
            }

            private string _ARG_MAT_NAME;
            [DataType(Value = "VARCHAR2")]
            public string ARG_MAT_NAME
            {
                get { return _ARG_MAT_NAME; }
                set { _ARG_MAT_NAME = value; }
            }

            private string _ARG_MAT_COMMENT;
            [DataType(Value = "VARCHAR2")]
            public string ARG_MAT_COMMENT
            {
                get { return _ARG_MAT_COMMENT; }
                set { _ARG_MAT_COMMENT = value; }
            }

            private string _ARG_MCS_NUMBER;
            [DataType(Value = "VARCHAR2")]
            public string ARG_MCS_NUMBER
            {
                get { return _ARG_MCS_NUMBER; }
                set { _ARG_MCS_NUMBER = value; }
            }

            private string _ARG_COLOR_CD;
            [DataType(Value = "VARCHAR2")]
            public string ARG_COLOR_CD
            {
                get { return _ARG_COLOR_CD; }
                set { _ARG_COLOR_CD = value; }
            }

            private string _ARG_COLOR_NAME;
            [DataType(Value = "VARCHAR2")]
            public string ARG_COLOR_NAME
            {
                get { return _ARG_COLOR_NAME; }
                set { _ARG_COLOR_NAME = value; }
            }

            private string _ARG_COLOR_COMMENT;
            [DataType(Value = "VARCHAR2")]
            public string ARG_COLOR_COMMENT
            {
                get { return _ARG_COLOR_COMMENT; }
                set { _ARG_COLOR_COMMENT = value; }
            }

            private string _ARG_REMARKS;
            [DataType(Value = "VARCHAR2")]
            public string ARG_REMARKS
            {
                get { return _ARG_REMARKS; }
                set { _ARG_REMARKS = value; }
            }

            private string _ARG_UPD_USER;
            [DataType(Value = "VARCHAR2")]
            public string ARG_UPD_USER
            {
                get { return _ARG_UPD_USER; }
                set { _ARG_UPD_USER = value; }
            }

            private string _ARG_PTRN_PART_NAME;
            [DataType(Value = "VARCHAR2")]
            public string ARG_PTRN_PART_NAME
            {
                get { return _ARG_PTRN_PART_NAME; }
                set { _ARG_PTRN_PART_NAME = value; }
            }

            private string _ARG_PCX_MAT_ID;
            [DataType(Value = "VARCHAR2")]
            public string ARG_PCX_MAT_ID
            {
                get { return _ARG_PCX_MAT_ID; }
                set { _ARG_PCX_MAT_ID = value; }
            }

            private string _ARG_PCX_SUPP_MAT_ID;
            [DataType(Value = "VARCHAR2")]
            public string ARG_PCX_SUPP_MAT_ID
            {
                get { return _ARG_PCX_SUPP_MAT_ID; }
                set { _ARG_PCX_SUPP_MAT_ID = value; }
            }

            private string _ARG_PCX_COLOR_ID;
            [DataType(Value = "VARCHAR2")]
            public string ARG_PCX_COLOR_ID
            {
                get { return _ARG_PCX_COLOR_ID; }
                set { _ARG_PCX_COLOR_ID = value; }
            }

            private string _ARG_ROW_STATUS;
            [DataType(Value = "VARCHAR2")]
            public string ARG_ROW_STATUS
            {
                get { return _ARG_ROW_STATUS; }
                set { _ARG_ROW_STATUS = value; }
            }

            private string _ARG_PROCESS;
            [DataType(Value = "VARCHAR2")]
            public string ARG_PROCESS
            {
                get { return _ARG_PROCESS; }
                set { _ARG_PROCESS = value; }
            }

            private string _ARG_VENDOR_NAME;
            [DataType(Value = "VARCHAR2")]
            public string ARG_VENDOR_NAME
            {
                get { return _ARG_VENDOR_NAME; }
                set { _ARG_VENDOR_NAME = value; }
            }

            private string _ARG_COMBINE_YN;
            [DataType(Value = "VARCHAR2")]
            public string ARG_COMBINE_YN
            {
                get { return _ARG_COMBINE_YN; }
                set { _ARG_COMBINE_YN = value; }
            }

            private string _ARG_STICKER_YN;
            [DataType(Value = "VARCHAR2")]
            public string ARG_STICKER_YN
            {
                get { return _ARG_STICKER_YN; }
                set { _ARG_STICKER_YN = value; }
            }

            private string _ARG_PTRN_PART_CD;
            [DataType(Value = "VARCHAR2")]
            public string ARG_PTRN_PART_CD
            {
                get { return _ARG_PTRN_PART_CD; }
                set { _ARG_PTRN_PART_CD = value; }
            }

            private string _ARG_MDSL_CHK;
            [DataType(Value = "VARCHAR2")]
            public string ARG_MDSL_CHK
            {
                get { return _ARG_MDSL_CHK; }
                set { _ARG_MDSL_CHK = value; }
            }

            private string _ARG_OTSL_CHK;
            [DataType(Value = "VARCHAR2")]
            public string ARG_OTSL_CHK
            {
                get { return _ARG_OTSL_CHK; }
                set { _ARG_OTSL_CHK = value; }
            }

            private string _ARG_CS_PTRN_CD;
            [DataType(Value = "VARCHAR2")]
            public string ARG_CS_PTRN_CD
            {
                get { return _ARG_CS_PTRN_CD; }
                set { _ARG_CS_PTRN_CD = value; }
            }

            private string _ARG_CS_PTRN_NAME;
            [DataType(Value = "VARCHAR2")]
            public string ARG_CS_PTRN_NAME
            {
                get { return _ARG_CS_PTRN_NAME; }
                set { _ARG_CS_PTRN_NAME = value; }
            }

            private string _ARG_ENCODED_CMT;
            [DataType(Value = "VARCHAR2")]
            public string ARG_ENCODED_CMT
            {
                get { return _ARG_ENCODED_CMT; }
                set { _ARG_ENCODED_CMT = value; }
            }
        }

        public class UPDATE_BOM_HEAD_DATA
        {
            private string _ARG_FACTORY;
            [DataType(Value = "VARCHAR2")]
            public string ARG_FACTORY
            {
                get { return _ARG_FACTORY; }
                set { _ARG_FACTORY = value; }
            }

            private string _ARG_CHAINED_WS_NO;
            [DataType(Value = "VARCHAR2")]
            public string ARG_CHAINED_WS_NO
            {
                get { return _ARG_CHAINED_WS_NO; }
                set { _ARG_CHAINED_WS_NO = value; }
            }

            private string _ARG_UPD_USER;
            [DataType(Value = "VARCHAR2")]
            public string ARG_UPD_USER
            {
                get { return _ARG_UPD_USER; }
                set { _ARG_UPD_USER = value; }
            }
        }
    }

    public class PKG_INTG_BOM_PURCHASE
    {
        public class LOAD_MATERIALS_TO_PURCHASE
        {
            private string _ARG_FACTORY;
            [DataType(Value = "VARCHAR2")]
            public string ARG_FACTORY
            {
                get { return _ARG_FACTORY; }
                set { _ARG_FACTORY = value; }
            }

            private string _ARG_CONCAT_WS_NO;
            [DataType(Value = "VARCHAR2")]
            public string ARG_CONCAT_WS_NO
            {
                get { return _ARG_CONCAT_WS_NO; }
                set { _ARG_CONCAT_WS_NO = value; }
            }

            private string _OUT_CURSOR;
            [DataType(Value = "REF CURSOR")]
            public string OUT_CURSOR
            {
                get { return _OUT_CURSOR; }
                set { _OUT_CURSOR = value; }
            }
        }

        public class LOAD_SHOP_BASKET_BY_LOC
        {
            private string _ARG_FACTORY;
            [DataType(Value = "VARCHAR2")]
            public string ARG_FACTORY
            {
                get { return _ARG_FACTORY; }
                set { _ARG_FACTORY = value; }
            }

            private string _ARG_CONCAT_WS_NO;
            [DataType(Value = "VARCHAR2")]
            public string ARG_CONCAT_WS_NO
            {
                get { return _ARG_CONCAT_WS_NO; }
                set { _ARG_CONCAT_WS_NO = value; }
            }

            private string _ARG_LOCATION;
            [DataType(Value = "VARCHAR2")]
            public string ARG_LOCATION
            {
                get { return _ARG_LOCATION; }
                set { _ARG_LOCATION = value; }
            }

            private string _OUT_CURSOR;
            [DataType(Value = "REF CURSOR")]
            public string OUT_CURSOR
            {
                get { return _OUT_CURSOR; }
                set { _OUT_CURSOR = value; }
            }
        }

        public class UPDATE_PUR_CHK_TO_Y
        {
            private string _ARG_FACTORY;
            [DataType(Value = "VARCHAR2")]
            public string ARG_FACTORY
            {
                get { return _ARG_FACTORY; }
                set { _ARG_FACTORY = value; }
            }

            private string _ARG_WS_NO;
            [DataType(Value = "VARCHAR2")]
            public string ARG_WS_NO
            {
                get { return _ARG_WS_NO; }
                set { _ARG_WS_NO = value; }
            }

            private string _ARG_PART_SEQ;
            [DataType(Value = "VARCHAR2")]
            public string ARG_PART_SEQ
            {
                get { return _ARG_PART_SEQ; }
                set { _ARG_PART_SEQ = value; }
            }

            private string _ARG_TYPE;
            [DataType(Value = "VARCHAR2")]
            public string ARG_TYPE
            {
                get { return _ARG_TYPE; }
                set { _ARG_TYPE = value; }
            }
        }

        public class UPDATE_PUR_CHK_TO_N
        {
            private string _ARG_FACTORY;
            [DataType(Value = "VARCHAR2")]
            public string ARG_FACTORY
            {
                get { return _ARG_FACTORY; }
                set { _ARG_FACTORY = value; }
            }

            private string _ARG_CONCAT_WS_NO;
            [DataType(Value = "VARCHAR2")]
            public string ARG_CONCAT_WS_NO
            {
                get { return _ARG_CONCAT_WS_NO; }
                set { _ARG_CONCAT_WS_NO = value; }
            }
        }

        public class ADD_TO_SHOP_BASKET
        {
            private string _ARG_FACTORY;
            [DataType(Value = "VARCHAR2")]
            public string ARG_FACTORY
            {
                get { return _ARG_FACTORY; }
                set { _ARG_FACTORY = value; }
            }

            private string _ARG_CONCAT_WS_NO;
            [DataType(Value = "VARCHAR2")]
            public string ARG_CONCAT_WS_NO
            {
                get { return _ARG_CONCAT_WS_NO; }
                set { _ARG_CONCAT_WS_NO = value; }
            }

            private string _ARG_DEVELOPER;
            [DataType(Value = "VARCHAR2")]
            public string ARG_DEVELOPER
            {
                get { return _ARG_DEVELOPER; }
                set { _ARG_DEVELOPER = value; }
            }
        }

        public class CALCULATE_NEXT_PART_SEQ
        {
            private string _ARG_FACTORY;
            [DataType(Value = "VARCHAR2")]
            public string ARG_FACTORY
            {
                get { return _ARG_FACTORY; }
                set { _ARG_FACTORY = value; }
            }

            private string _ARG_WS_NO;
            [DataType(Value = "VARCHAR2")]
            public string ARG_WS_NO
            {
                get { return _ARG_WS_NO; }
                set { _ARG_WS_NO = value; }
            }

            private string _OUT_CURSOR;
            [DataType(Value = "REF CURSOR")]
            public string OUT_CURSOR
            {
                get { return _OUT_CURSOR; }
                set { _OUT_CURSOR = value; }
            }
        }

        public class ENROLL_MANUAL_MATERIALS
        {
            private string _ARG_FACTORY;
            [DataType(Value = "VARCHAR2")]
            public string ARG_FACTORY
            {
                get { return _ARG_FACTORY; }
                set { _ARG_FACTORY = value; }
            }

            private string _ARG_WS_NO;
            [DataType(Value = "VARCHAR2")]
            public string ARG_WS_NO
            {
                get { return _ARG_WS_NO; }
                set { _ARG_WS_NO = value; }
            }

            private string _ARG_PART_SEQ;
            [DataType(Value = "VARCHAR2")]
            public string ARG_PART_SEQ
            {
                get { return _ARG_PART_SEQ; }
                set { _ARG_PART_SEQ = value; }
            }

            private string _ARG_PART_NAME;
            [DataType(Value = "VARCHAR2")]
            public string ARG_PART_NAME
            {
                get { return _ARG_PART_NAME; }
                set { _ARG_PART_NAME = value; }
            }

            private string _ARG_PART_TYPE;
            [DataType(Value = "VARCHAR2")]
            public string ARG_PART_TYPE
            {
                get { return _ARG_PART_TYPE; }
                set { _ARG_PART_TYPE = value; }
            }

            private string _ARG_MXSXL_NUMBER;
            [DataType(Value = "VARCHAR2")]
            public string ARG_MXSXL_NUMBER
            {
                get { return _ARG_MXSXL_NUMBER; }
                set { _ARG_MXSXL_NUMBER = value; }
            }

            private string _ARG_PCX_SUPP_MAT_ID;
            [DataType(Value = "VARCHAR2")]
            public string ARG_PCX_SUPP_MAT_ID
            {
                get { return _ARG_PCX_SUPP_MAT_ID; }
                set { _ARG_PCX_SUPP_MAT_ID = value; }
            }

            private string _ARG_MAT_CD;
            [DataType(Value = "VARCHAR2")]
            public string ARG_MAT_CD
            {
                get { return _ARG_MAT_CD; }
                set { _ARG_MAT_CD = value; }
            }

            private string _ARG_MAT_NAME;
            [DataType(Value = "VARCHAR2")]
            public string ARG_MAT_NAME
            {
                get { return _ARG_MAT_NAME; }
                set { _ARG_MAT_NAME = value; }
            }

            private string _ARG_MAT_COMMENT;
            [DataType(Value = "VARCHAR2")]
            public string ARG_MAT_COMMENT
            {
                get { return _ARG_MAT_COMMENT; }
                set { _ARG_MAT_COMMENT = value; }
            }

            private string _ARG_PCX_COLOR_ID;
            [DataType(Value = "VARCHAR2")]
            public string ARG_PCX_COLOR_ID
            {
                get { return _ARG_PCX_COLOR_ID; }
                set { _ARG_PCX_COLOR_ID = value; }
            }

            private string _ARG_COLOR_CD;
            [DataType(Value = "VARCHAR2")]
            public string ARG_COLOR_CD
            {
                get { return _ARG_COLOR_CD; }
                set { _ARG_COLOR_CD = value; }
            }

            private string _ARG_COLOR_NAME;
            [DataType(Value = "VARCHAR2")]
            public string ARG_COLOR_NAME
            {
                get { return _ARG_COLOR_NAME; }
                set { _ARG_COLOR_NAME = value; }
            }

            private string _ARG_OWNER;
            [DataType(Value = "VARCHAR2")]
            public string ARG_OWNER
            {
                get { return _ARG_OWNER; }
                set { _ARG_OWNER = value; }
            }

            private string _ARG_LOCATION;
            [DataType(Value = "VARCHAR2")]
            public string ARG_LOCATION
            {
                get { return _ARG_LOCATION; }
                set { _ARG_LOCATION = value; }
            }

            private string _ARG_COLOR_COMMENT;
            [DataType(Value = "VARCHAR2")]
            public string ARG_COLOR_COMMENT
            {
                get { return _ARG_COLOR_COMMENT; }
                set { _ARG_COLOR_COMMENT = value; }
            }

            private string _ARG_PCX_MAT_ID;
            [DataType(Value = "VARCHAR2")]
            public string ARG_PCX_MAT_ID
            {
                get { return _ARG_PCX_MAT_ID; }
                set { _ARG_PCX_MAT_ID = value; }
            }

            private string _ARG_VENDOR_NAME;
            [DataType(Value = "VARCHAR2")]
            public string ARG_VENDOR_NAME
            {
                get { return _ARG_VENDOR_NAME; }
                set { _ARG_VENDOR_NAME = value; }
            }

            private string _ARG_CS_CD;
            [DataType(Value = "VARCHAR2")]
            public string ARG_CS_CD
            {
                get { return _ARG_CS_CD; }
                set { _ARG_CS_CD = value; }
            }

            private string _ARG_ENCODED_CMT;
            [DataType(Value = "VARCHAR2")]
            public string ARG_ENCODED_CMT
            {
                get { return _ARG_ENCODED_CMT; }
                set { _ARG_ENCODED_CMT = value; }
            }
        }

        public class RELEASE_MANUAL_MATERIALS
        {
            private string _ARG_FACTORY;
            [DataType(Value = "VARCHAR2")]
            public string ARG_FACTORY
            {
                get { return _ARG_FACTORY; }
                set { _ARG_FACTORY = value; }
            }

            private string _ARG_WS_NO;
            [DataType(Value = "VARCHAR2")]
            public string ARG_WS_NO
            {
                get { return _ARG_WS_NO; }
                set { _ARG_WS_NO = value; }
            }

            private string _ARG_PART_SEQ;
            [DataType(Value = "VARCHAR2")]
            public string ARG_PART_SEQ
            {
                get { return _ARG_PART_SEQ; }
                set { _ARG_PART_SEQ = value; }
            }
        }

        public class EMPTY_SHOP_BASKET
        {
            private string _ARG_FACTORY;
            [DataType(Value = "VARCHAR2")]
            public string ARG_FACTORY
            {
                get { return _ARG_FACTORY; }
                set { _ARG_FACTORY = value; }
            }

            private string _ARG_CONCAT_WS_NO;
            [DataType(Value = "VARCHAR2")]
            public string ARG_CONCAT_WS_NO
            {
                get { return _ARG_CONCAT_WS_NO; }
                set { _ARG_CONCAT_WS_NO = value; }
            }

            private string _ARG_LOCATION;
            [DataType(Value = "VARCHAR2")]
            public string ARG_LOCATION
            {
                get { return _ARG_LOCATION; }
                set { _ARG_LOCATION = value; }
            }
        }

        public class UPDATE_BOM
        {
            private string _ARG_WORK_TYPE;
            [DataType(Value = "VARCHAR2")]
            public string ARG_WORK_TYPE
            {
                get { return _ARG_WORK_TYPE; }
                set { _ARG_WORK_TYPE = value; }
            }

            private string _ARG_FACTORY;
            [DataType(Value = "VARCHAR2")]
            public string ARG_FACTORY
            {
                get { return _ARG_FACTORY; }
                set { _ARG_FACTORY = value; }
            }

            private string _ARG_WS_NO;
            [DataType(Value = "VARCHAR2")]
            public string ARG_WS_NO
            {
                get { return _ARG_WS_NO; }
                set { _ARG_WS_NO = value; }
            }

            private string _ARG_PART_SEQ;
            [DataType(Value = "VARCHAR2")]
            public string ARG_PART_SEQ
            {
                get { return _ARG_PART_SEQ; }
                set { _ARG_PART_SEQ = value; }
            }

            private string _ARG_PART_NAME;
            [DataType(Value = "VARCHAR2")]
            public string ARG_PART_NAME
            {
                get { return _ARG_PART_NAME; }
                set { _ARG_PART_NAME = value; }
            }

            private string _ARG_PART_TYPE;
            [DataType(Value = "VARCHAR2")]
            public string ARG_PART_TYPE
            {
                get { return _ARG_PART_TYPE; }
                set { _ARG_PART_TYPE = value; }
            }

            private string _ARG_MXSXL_NUMBER;
            [DataType(Value = "VARCHAR2")]
            public string ARG_MXSXL_NUMBER
            {
                get { return _ARG_MXSXL_NUMBER; }
                set { _ARG_MXSXL_NUMBER = value; }
            }

            private string _ARG_PCX_SUPP_MAT_ID;
            [DataType(Value = "VARCHAR2")]
            public string ARG_PCX_SUPP_MAT_ID
            {
                get { return _ARG_PCX_SUPP_MAT_ID; }
                set { _ARG_PCX_SUPP_MAT_ID = value; }
            }

            private string _ARG_MAT_CD;
            [DataType(Value = "VARCHAR2")]
            public string ARG_MAT_CD
            {
                get { return _ARG_MAT_CD; }
                set { _ARG_MAT_CD = value; }
            }

            private string _ARG_MAT_NAME;
            [DataType(Value = "VARCHAR2")]
            public string ARG_MAT_NAME
            {
                get { return _ARG_MAT_NAME; }
                set { _ARG_MAT_NAME = value; }
            }

            private string _ARG_MAT_COMMENT;
            [DataType(Value = "VARCHAR2")]
            public string ARG_MAT_COMMENT
            {
                get { return _ARG_MAT_COMMENT; }
                set { _ARG_MAT_COMMENT = value; }
            }

            private string _ARG_PCX_COLOR_ID;
            [DataType(Value = "VARCHAR2")]
            public string ARG_PCX_COLOR_ID
            {
                get { return _ARG_PCX_COLOR_ID; }
                set { _ARG_PCX_COLOR_ID = value; }
            }

            private string _ARG_COLOR_CD;
            [DataType(Value = "VARCHAR2")]
            public string ARG_COLOR_CD
            {
                get { return _ARG_COLOR_CD; }
                set { _ARG_COLOR_CD = value; }
            }

            private string _ARG_COLOR_NAME;
            [DataType(Value = "VARCHAR2")]
            public string ARG_COLOR_NAME
            {
                get { return _ARG_COLOR_NAME; }
                set { _ARG_COLOR_NAME = value; }
            }

            private string _ARG_COLOR_COMMENT;
            [DataType(Value = "VARCHAR2")]
            public string ARG_COLOR_COMMENT
            {
                get { return _ARG_COLOR_COMMENT; }
                set { _ARG_COLOR_COMMENT = value; }
            }

            private string _ARG_PCX_MAT_ID;
            [DataType(Value = "VARCHAR2")]
            public string ARG_PCX_MAT_ID
            {
                get { return _ARG_PCX_MAT_ID; }
                set { _ARG_PCX_MAT_ID = value; }
            }

            private string _ARG_CS_CD;
            [DataType(Value = "VARCHAR2")]
            public string ARG_CS_CD
            {
                get { return _ARG_CS_CD; }
                set { _ARG_CS_CD = value; }
            }

            private string _ARG_VENDOR_NAME;
            [DataType(Value = "VARCHAR2")]
            public string ARG_VENDOR_NAME
            {
                get { return _ARG_VENDOR_NAME; }
                set { _ARG_VENDOR_NAME = value; }
            }

            private string _ARG_UPD_USER;
            [DataType(Value = "VARCHAR2")]
            public string ARG_UPD_USER
            {
                get { return _ARG_UPD_USER; }
                set { _ARG_UPD_USER = value; }
            }

            private string _ARG_ENCODED_CMT;
            [DataType(Value = "VARCHAR2")]
            public string ARG_ENCODED_CMT
            {
                get { return _ARG_ENCODED_CMT; }
                set { _ARG_ENCODED_CMT = value; }
            }
        }

        public class REQUEST_PURCHASE_ORDER
        {
            private string _ARG_FACTORY;
            [DataType(Value = "VARCHAR2")]
            public string ARG_FACTORY
            {
                get { return _ARG_FACTORY; }
                set { _ARG_FACTORY = value; }
            }

            private string _ARG_CONCAT_WS_NO;
            [DataType(Value = "VARCHAR2")]
            public string ARG_CONCAT_WS_NO
            {
                get { return _ARG_CONCAT_WS_NO; }
                set { _ARG_CONCAT_WS_NO = value; }
            }

            private string _ARG_PUR_USER;
            [DataType(Value = "VARCHAR2")]
            public string ARG_PUR_USER
            {
                get { return _ARG_PUR_USER; }
                set { _ARG_PUR_USER = value; }
            }
        }

        public class SET_BOM_CFM_DATE
        {
            private string _ARG_FACTORY;
            [DataType(Value = "VARCHAR2")]
            public string ARG_FACTORY
            {
                get { return _ARG_FACTORY; }
                set { _ARG_FACTORY = value; }
            }

            private string _ARG_CHAINED_WS_NO;
            [DataType(Value = "VARCHAR2")]
            public string ARG_CHAINED_WS_NO
            {
                get { return _ARG_CHAINED_WS_NO; }
                set { _ARG_CHAINED_WS_NO = value; }
            }

            private string _ARG_UPD_USER;
            [DataType(Value = "VARCHAR2")]
            public string ARG_UPD_USER
            {
                get { return _ARG_UPD_USER; }
                set { _ARG_UPD_USER = value; }
            }
        }
    }

    public class PKG_INTG_BOM_WORKSHEET
    {
        public class LOAD_WORKSHEET_CONTENTS
        {
            private string _ARG_WORK_TYPE;
            [DataType(Value = "VARCHAR2")]
            public string ARG_WORK_TYPE
            {
                get { return _ARG_WORK_TYPE; }
                set { _ARG_WORK_TYPE = value; }
            }

            private string _ARG_FACTORY;
            [DataType(Value = "VARCHAR2")]
            public string ARG_FACTORY
            {
                get { return _ARG_FACTORY; }
                set { _ARG_FACTORY = value; }
            }

            private string _ARG_WS_NO;
            [DataType(Value = "VARCHAR2")]
            public string ARG_WS_NO
            {
                get { return _ARG_WS_NO; }
                set { _ARG_WS_NO = value; }
            }

            private string _OUT_CURSOR;
            [DataType(Value = "REF CURSOR")]
            public string OUT_CURSOR
            {
                get { return _OUT_CURSOR; }
                set { _OUT_CURSOR = value; }
            }
        }

        public class LOAD_BOTTOM_MATERIALS
        {
            private string _ARG_WORK_TYPE;
            [DataType(Value = "VARCHAR2")]
            public string ARG_WORK_TYPE
            {
                get { return _ARG_WORK_TYPE; }
                set { _ARG_WORK_TYPE = value; }
            }

            private string _ARG_FACTORY;
            [DataType(Value = "VARCHAR2")]
            public string ARG_FACTORY
            {
                get { return _ARG_FACTORY; }
                set { _ARG_FACTORY = value; }
            }

            private string _ARG_WS_NO;
            [DataType(Value = "VARCHAR2")]
            public string ARG_WS_NO
            {
                get { return _ARG_WS_NO; }
                set { _ARG_WS_NO = value; }
            }

            private string _ARG_PART_TYPE;
            [DataType(Value = "VARCHAR2")]
            public string ARG_PART_TYPE
            {
                get { return _ARG_PART_TYPE; }
                set { _ARG_PART_TYPE = value; }
            }

            private string _OUT_CURSOR;
            [DataType(Value = "REF CURSOR")]
            public string OUT_CURSOR
            {
                get { return _OUT_CURSOR; }
                set { _OUT_CURSOR = value; }
            }
        }

        public class CREATE_TEMPLATE_BY_STYLE
        {
            private string _ARG_WORK_TYPE;
            [DataType(Value = "VARCHAR2")]
            public string ARG_WORK_TYPE
            {
                get { return _ARG_WORK_TYPE; }
                set { _ARG_WORK_TYPE = value; }
            }

            private string _ARG_FACTORY;
            [DataType(Value = "VARCHAR2")]
            public string ARG_FACTORY
            {
                get { return _ARG_FACTORY; }
                set { _ARG_FACTORY = value; }
            }

            private string _ARG_KEY_VALUE;
            [DataType(Value = "VARCHAR2")]
            public string ARG_KEY_VALUE
            {
                get { return _ARG_KEY_VALUE; }
                set { _ARG_KEY_VALUE = value; }
            }

            private string _ARG_KEY_TYPE;
            [DataType(Value = "VARCHAR2")]
            public string ARG_KEY_TYPE
            {
                get { return _ARG_KEY_TYPE; }
                set { _ARG_KEY_TYPE = value; }
            }

            private string _OUT_CURSOR;
            [DataType(Value = "REF CURSOR")]
            public string OUT_CURSOR
            {
                get { return _OUT_CURSOR; }
                set { _OUT_CURSOR = value; }
            }
        }

        public class UPDATE_TEMPLATE_SPEC
        {
            private string _ARG_FACTORY;
            [DataType(Value = "VARCHAR2")]
            public string ARG_FACTORY
            {
                get { return _ARG_FACTORY; }
                set { _ARG_FACTORY = value; }
            }

            private string _ARG_KEY_VALUE;
            [DataType(Value = "VARCHAR2")]
            public string ARG_KEY_VALUE
            {
                get { return _ARG_KEY_VALUE; }
                set { _ARG_KEY_VALUE = value; }
            }

            private string _ARG_KEY_TYPE;
            [DataType(Value = "VARCHAR2")]
            public string ARG_KEY_TYPE
            {
                get { return _ARG_KEY_TYPE; }
                set { _ARG_KEY_TYPE = value; }
            }

            private string _ARG_LAST_CD;
            [DataType(Value = "VARCHAR2")]
            public string ARG_LAST_CD
            {
                get { return _ARG_LAST_CD; }
                set { _ARG_LAST_CD = value; }
            }

            private string _ARG_HEEL_HEIGHT;
            [DataType(Value = "VARCHAR2")]
            public string ARG_HEEL_HEIGHT
            {
                get { return _ARG_HEEL_HEIGHT; }
                set { _ARG_HEEL_HEIGHT = value; }
            }

            private string _ARG_MEDIAL_HEIGHT;
            [DataType(Value = "VARCHAR2")]
            public string ARG_MEDIAL_HEIGHT
            {
                get { return _ARG_MEDIAL_HEIGHT; }
                set { _ARG_MEDIAL_HEIGHT = value; }
            }

            private string _ARG_LATERAL_HEIGHT;
            [DataType(Value = "VARCHAR2")]
            public string ARG_LATERAL_HEIGHT
            {
                get { return _ARG_LATERAL_HEIGHT; }
                set { _ARG_LATERAL_HEIGHT = value; }
            }

            private string _ARG_LACE_LENGTH;
            [DataType(Value = "VARCHAR2")]
            public string ARG_LACE_LENGTH
            {
                get { return _ARG_LACE_LENGTH; }
                set { _ARG_LACE_LENGTH = value; }
            }

            private string _ARG_MS_HARDNESS;
            [DataType(Value = "VARCHAR2")]
            public string ARG_MS_HARDNESS
            {
                get { return _ARG_MS_HARDNESS; }
                set { _ARG_MS_HARDNESS = value; }
            }

            private string _ARG_IDS_LENGTH;
            [DataType(Value = "VARCHAR2")]
            public string ARG_IDS_LENGTH
            {
                get { return _ARG_IDS_LENGTH; }
                set { _ARG_IDS_LENGTH = value; }
            }

            private string _ARG_MS_CODE;
            [DataType(Value = "VARCHAR2")]
            public string ARG_MS_CODE
            {
                get { return _ARG_MS_CODE; }
                set { _ARG_MS_CODE = value; }
            }

            private string _ARG_OS_CODE;
            [DataType(Value = "VARCHAR2")]
            public string ARG_OS_CODE
            {
                get { return _ARG_OS_CODE; }
                set { _ARG_OS_CODE = value; }
            }

            private string _ARG_UPPER_MATERIAL;
            [DataType(Value = "VARCHAR2")]
            public string ARG_UPPER_MATERIAL
            {
                get { return _ARG_UPPER_MATERIAL; }
                set { _ARG_UPPER_MATERIAL = value; }
            }

            private string _ARG_MS_MATERIAL;
            [DataType(Value = "VARCHAR2")]
            public string ARG_MS_MATERIAL
            {
                get { return _ARG_MS_MATERIAL; }
                set { _ARG_MS_MATERIAL = value; }
            }

            private string _ARG_OUTSOLE_MATERIAL;
            [DataType(Value = "VARCHAR2")]
            public string ARG_OUTSOLE_MATERIAL
            {
                get { return _ARG_OUTSOLE_MATERIAL; }
                set { _ARG_OUTSOLE_MATERIAL = value; }
            }

            private string _ARG_NEEDLE_SIZE;
            [DataType(Value = "VARCHAR2")]
            public string ARG_NEEDLE_SIZE
            {
                get { return _ARG_NEEDLE_SIZE; }
                set { _ARG_NEEDLE_SIZE = value; }
            }

            private string _ARG_SPI;
            [DataType(Value = "VARCHAR2")]
            public string ARG_SPI
            {
                get { return _ARG_SPI; }
                set { _ARG_SPI = value; }
            }

            private string _ARG_STITCHING_MARGIN;
            [DataType(Value = "VARCHAR2")]
            public string ARG_STITCHING_MARGIN
            {
                get { return _ARG_STITCHING_MARGIN; }
                set { _ARG_STITCHING_MARGIN = value; }
            }

            private string _ARG_TWOROW_STITCHING_MARGIN;
            [DataType(Value = "VARCHAR2")]
            public string ARG_TWOROW_STITCHING_MARGIN
            {
                get { return _ARG_TWOROW_STITCHING_MARGIN; }
                set { _ARG_TWOROW_STITCHING_MARGIN = value; }
            }

            private string _ARG_THREAD_TYPE;
            [DataType(Value = "VARCHAR2")]
            public string ARG_THREAD_TYPE
            {
                get { return _ARG_THREAD_TYPE; }
                set { _ARG_THREAD_TYPE = value; }
            }

            private string _ARG_DCS_YN;
            [DataType(Value = "VARCHAR2")]
            public string ARG_DCS_YN
            {
                get { return _ARG_DCS_YN; }
                set { _ARG_DCS_YN = value; }
            }

            private string _ARG_WS_COMMENT1;
            [DataType(Value = "VARCHAR2")]
            public string ARG_WS_COMMENT1
            {
                get { return _ARG_WS_COMMENT1; }
                set { _ARG_WS_COMMENT1 = value; }
            }

            private string _ARG_WS_COMMENT2;
            [DataType(Value = "VARCHAR2")]
            public string ARG_WS_COMMENT2
            {
                get { return _ARG_WS_COMMENT2; }
                set { _ARG_WS_COMMENT2 = value; }
            }

            private string _ARG_WS_COMMENT3;
            [DataType(Value = "VARCHAR2")]
            public string ARG_WS_COMMENT3
            {
                get { return _ARG_WS_COMMENT3; }
                set { _ARG_WS_COMMENT3 = value; }
            }

            private string _ARG_WS_COMMENT4;
            [DataType(Value = "VARCHAR2")]
            public string ARG_WS_COMMENT4
            {
                get { return _ARG_WS_COMMENT4; }
                set { _ARG_WS_COMMENT4 = value; }
            }

            private string _ARG_WS_COMMENT5;
            [DataType(Value = "VARCHAR2")]
            public string ARG_WS_COMMENT5
            {
                get { return _ARG_WS_COMMENT5; }
                set { _ARG_WS_COMMENT5 = value; }
            }

            private string _ARG_WS_COMMENT6;
            [DataType(Value = "VARCHAR2")]
            public string ARG_WS_COMMENT6
            {
                get { return _ARG_WS_COMMENT6; }
                set { _ARG_WS_COMMENT6 = value; }
            }

            private string _ARG_WS_COMMENT7;
            [DataType(Value = "VARCHAR2")]
            public string ARG_WS_COMMENT7
            {
                get { return _ARG_WS_COMMENT7; }
                set { _ARG_WS_COMMENT7 = value; }
            }

            private string _ARG_UPD_USER;
            [DataType(Value = "VARCHAR2")]
            public string ARG_UPD_USER
            {
                get { return _ARG_UPD_USER; }
                set { _ARG_UPD_USER = value; }
            }

            private string _ARG_NIKE_DEV;
            [DataType(Value = "VARCHAR2")]
            public string ARG_NIKE_DEV
            {
                get { return _ARG_NIKE_DEV; }
                set { _ARG_NIKE_DEV = value; }
            }

            private string _ARG_WHQ_DEV;
            [DataType(Value = "VARCHAR2")]
            public string ARG_WHQ_DEV
            {
                get { return _ARG_WHQ_DEV; }
                set { _ARG_WHQ_DEV = value; }
            }
        }

        public class UPDATE_TEMPLATE_OPERATION
        {
            private string _ARG_FACTORY;
            [DataType(Value = "VARCHAR2")]
            public string ARG_FACTORY
            {
                get { return _ARG_FACTORY; }
                set { _ARG_FACTORY = value; }
            }

            private string _ARG_KEY_VALUE;
            [DataType(Value = "VARCHAR2")]
            public string ARG_KEY_VALUE
            {
                get { return _ARG_KEY_VALUE; }
                set { _ARG_KEY_VALUE = value; }
            }

            private string _ARG_KEY_TYPE;
            [DataType(Value = "VARCHAR2")]
            public string ARG_KEY_TYPE
            {
                get { return _ARG_KEY_TYPE; }
                set { _ARG_KEY_TYPE = value; }
            }

            private string _ARG_OP_CD;
            [DataType(Value = "VARCHAR2")]
            public string ARG_OP_CD
            {
                get { return _ARG_OP_CD; }
                set { _ARG_OP_CD = value; }
            }

            private string _ARG_OP_CHK;
            [DataType(Value = "VARCHAR2")]
            public string ARG_OP_CHK
            {
                get { return _ARG_OP_CHK; }
                set { _ARG_OP_CHK = value; }
            }

            private string _ARG_UPD_USER;
            [DataType(Value = "VARCHAR2")]
            public string ARG_UPD_USER
            {
                get { return _ARG_UPD_USER; }
                set { _ARG_UPD_USER = value; }
            }
        }

        public class LOAD_TEMPALTE_BY_TYPE
        {
            private string _ARG_WORK_TYPE;
            [DataType(Value = "VARCHAR2")]
            public string ARG_WORK_TYPE
            {
                get { return _ARG_WORK_TYPE; }
                set { _ARG_WORK_TYPE = value; }
            }

            private string _ARG_KEY_VALUE;
            [DataType(Value = "VARCHAR2")]
            public string ARG_KEY_VALUE
            {
                get { return _ARG_KEY_VALUE; }
                set { _ARG_KEY_VALUE = value; }
            }

            private string _ARG_KEY_TYPE;
            [DataType(Value = "VARCHAR2")]
            public string ARG_KEY_TYPE
            {
                get { return _ARG_KEY_TYPE; }
                set { _ARG_KEY_TYPE = value; }
            }

            private string _ARG_FACTORY;
            [DataType(Value = "VARCHAR2")]
            public string ARG_FACTORY
            {
                get { return _ARG_FACTORY; }
                set { _ARG_FACTORY = value; }
            }

            private string _OUT_CURSOR;
            [DataType(Value = "REF CURSOR")]
            public string OUT_CURSOR
            {
                get { return _OUT_CURSOR; }
                set { _OUT_CURSOR = value; }
            }
        }

        public class COPY_WORKSHEET_CONTENTS
        {
            private string _ARG_S_FACTORY;
            [DataType(Value = "VARCHAR2")]
            public string ARG_S_FACTORY
            {
                get { return _ARG_S_FACTORY; }
                set { _ARG_S_FACTORY = value; }
            }

            private string _ARG_S_WS_NO;
            [DataType(Value = "VARCHAR2")]
            public string ARG_S_WS_NO
            {
                get { return _ARG_S_WS_NO; }
                set { _ARG_S_WS_NO = value; }
            }

            private string _ARG_T_FACTORY;
            [DataType(Value = "VARCHAR2")]
            public string ARG_T_FACTORY
            {
                get { return _ARG_T_FACTORY; }
                set { _ARG_T_FACTORY = value; }
            }

            private string _ARG_T_WS_NO;
            [DataType(Value = "VARCHAR2")]
            public string ARG_T_WS_NO
            {
                get { return _ARG_T_WS_NO; }
                set { _ARG_T_WS_NO = value; }
            }

            private string _ARG_UPD_USER;
            [DataType(Value = "VARCHAR2")]
            public string ARG_UPD_USER
            {
                get { return _ARG_UPD_USER; }
                set { _ARG_UPD_USER = value; }
            }
        }

        public class UPDATE_TAG_NEEDLE
        {
            private string _ARG_FACTORY;
            [DataType(Value = "VARCHAR2")]
            public string ARG_FACTORY
            {
                get { return _ARG_FACTORY; }
                set { _ARG_FACTORY = value; }
            }

            private string _ARG_WS_NO;
            [DataType(Value = "VARCHAR2")]
            public string ARG_WS_NO
            {
                get { return _ARG_WS_NO; }
                set { _ARG_WS_NO = value; }
            }

            private string _ARG_PROD_FACTORY;
            [DataType(Value = "VARCHAR2")]
            public string ARG_PROD_FACTORY
            {
                get { return _ARG_PROD_FACTORY; }
                set { _ARG_PROD_FACTORY = value; }
            }

            private string _ARG_SAMPLE_ETS;
            [DataType(Value = "VARCHAR2")]
            public string ARG_SAMPLE_ETS
            {
                get { return _ARG_SAMPLE_ETS; }
                set { _ARG_SAMPLE_ETS = value; }
            }

            private string _ARG_UPPER_MATERIAL;
            [DataType(Value = "VARCHAR2")]
            public string ARG_UPPER_MATERIAL
            {
                get { return _ARG_UPPER_MATERIAL; }
                set { _ARG_UPPER_MATERIAL = value; }
            }

            private string _ARG_MS_MATERIAL;
            [DataType(Value = "VARCHAR2")]
            public string ARG_MS_MATERIAL
            {
                get { return _ARG_MS_MATERIAL; }
                set { _ARG_MS_MATERIAL = value; }
            }

            private string _ARG_OUTSOLE_MATERIAL;
            [DataType(Value = "VARCHAR2")]
            public string ARG_OUTSOLE_MATERIAL
            {
                get { return _ARG_OUTSOLE_MATERIAL; }
                set { _ARG_OUTSOLE_MATERIAL = value; }
            }

            private string _ARG_LAST_CD;
            [DataType(Value = "VARCHAR2")]
            public string ARG_LAST_CD
            {
                get { return _ARG_LAST_CD; }
                set { _ARG_LAST_CD = value; }
            }

            private string _ARG_PATTERN_ID;
            [DataType(Value = "VARCHAR2")]
            public string ARG_PATTERN_ID
            {
                get { return _ARG_PATTERN_ID; }
                set { _ARG_PATTERN_ID = value; }
            }

            private string _ARG_STL_FILE;
            [DataType(Value = "VARCHAR2")]
            public string ARG_STL_FILE
            {
                get { return _ARG_STL_FILE; }
                set { _ARG_STL_FILE = value; }
            }

            private string _ARG_SAMPLE_WEIGHT;
            [DataType(Value = "VARCHAR2")]
            public string ARG_SAMPLE_WEIGHT
            {
                get { return _ARG_SAMPLE_WEIGHT; }
                set { _ARG_SAMPLE_WEIGHT = value; }
            }

            private string _ARG_HEEL_HEIGHT;
            [DataType(Value = "VARCHAR2")]
            public string ARG_HEEL_HEIGHT
            {
                get { return _ARG_HEEL_HEIGHT; }
                set { _ARG_HEEL_HEIGHT = value; }
            }

            private string _ARG_MEDIAL_HEIGHT;
            [DataType(Value = "VARCHAR2")]
            public string ARG_MEDIAL_HEIGHT
            {
                get { return _ARG_MEDIAL_HEIGHT; }
                set { _ARG_MEDIAL_HEIGHT = value; }
            }

            private string _ARG_LATERAL_HEIGHT;
            [DataType(Value = "VARCHAR2")]
            public string ARG_LATERAL_HEIGHT
            {
                get { return _ARG_LATERAL_HEIGHT; }
                set { _ARG_LATERAL_HEIGHT = value; }
            }

            private string _ARG_LACE_LENGTH;
            [DataType(Value = "VARCHAR2")]
            public string ARG_LACE_LENGTH
            {
                get { return _ARG_LACE_LENGTH; }
                set { _ARG_LACE_LENGTH = value; }
            }

            private string _ARG_MS_HARDNESS;
            [DataType(Value = "VARCHAR2")]
            public string ARG_MS_HARDNESS
            {
                get { return _ARG_MS_HARDNESS; }
                set { _ARG_MS_HARDNESS = value; }
            }

            private string _ARG_IDS_LENGTH;
            [DataType(Value = "VARCHAR2")]
            public string ARG_IDS_LENGTH
            {
                get { return _ARG_IDS_LENGTH; }
                set { _ARG_IDS_LENGTH = value; }
            }

            private string _ARG_LENGTH_TOE_SPRING;
            [DataType(Value = "VARCHAR2")]
            public string ARG_LENGTH_TOE_SPRING
            {
                get { return _ARG_LENGTH_TOE_SPRING; }
                set { _ARG_LENGTH_TOE_SPRING = value; }
            }

            private string _ARG_MS_CODE;
            [DataType(Value = "VARCHAR2")]
            public string ARG_MS_CODE
            {
                get { return _ARG_MS_CODE; }
                set { _ARG_MS_CODE = value; }
            }

            private string _ARG_OS_CODE;
            [DataType(Value = "VARCHAR2")]
            public string ARG_OS_CODE
            {
                get { return _ARG_OS_CODE; }
                set { _ARG_OS_CODE = value; }
            }

            private string _ARG_WS_QTY;
            [DataType(Value = "VARCHAR2")]
            public string ARG_WS_QTY
            {
                get { return _ARG_WS_QTY; }
                set { _ARG_WS_QTY = value; }
            }

            private string _ARG_NIKE_SEND_QTY;
            [DataType(Value = "VARCHAR2")]
            public string ARG_NIKE_SEND_QTY
            {
                get { return _ARG_NIKE_SEND_QTY; }
                set { _ARG_NIKE_SEND_QTY = value; }
            }

            private string _ARG_DPA;
            [DataType(Value = "VARCHAR2")]
            public string ARG_DPA
            {
                get { return _ARG_DPA; }
                set { _ARG_DPA = value; }
            }

            private string _ARG_NIKE_DEV;
            [DataType(Value = "VARCHAR2")]
            public string ARG_NIKE_DEV
            {
                get { return _ARG_NIKE_DEV; }
                set { _ARG_NIKE_DEV = value; }
            }

            private string _ARG_WHQ_DEV;
            [DataType(Value = "VARCHAR2")]
            public string ARG_WHQ_DEV
            {
                get { return _ARG_WHQ_DEV; }
                set { _ARG_WHQ_DEV = value; }
            }

            private string _ARG_DEV_SAMPLE_REQ_ID;
            [DataType(Value = "VARCHAR2")]
            public string ARG_DEV_SAMPLE_REQ_ID
            {
                get { return _ARG_DEV_SAMPLE_REQ_ID; }
                set { _ARG_DEV_SAMPLE_REQ_ID = value; }
            }

            private string _ARG_IPW;
            [DataType(Value = "VARCHAR2")]
            public string ARG_IPW
            {
                get { return _ARG_IPW; }
                set { _ARG_IPW = value; }
            }

            private string _ARG_BOM_ID;
            [DataType(Value = "VARCHAR2")]
            public string ARG_BOM_ID
            {
                get { return _ARG_BOM_ID; }
                set { _ARG_BOM_ID = value; }
            }

            private string _ARG_NEEDLE_SIZE;
            [DataType(Value = "VARCHAR2")]
            public string ARG_NEEDLE_SIZE
            {
                get { return _ARG_NEEDLE_SIZE; }
                set { _ARG_NEEDLE_SIZE = value; }
            }

            private string _ARG_SPI;
            [DataType(Value = "VARCHAR2")]
            public string ARG_SPI
            {
                get { return _ARG_SPI; }
                set { _ARG_SPI = value; }
            }

            private string _ARG_STITCHING_MARGIN;
            [DataType(Value = "VARCHAR2")]
            public string ARG_STITCHING_MARGIN
            {
                get { return _ARG_STITCHING_MARGIN; }
                set { _ARG_STITCHING_MARGIN = value; }
            }

            private string _ARG_TWOROW_STITCHING_MARGIN;
            [DataType(Value = "VARCHAR2")]
            public string ARG_TWOROW_STITCHING_MARGIN
            {
                get { return _ARG_TWOROW_STITCHING_MARGIN; }
                set { _ARG_TWOROW_STITCHING_MARGIN = value; }
            }

            private string _ARG_THREAD_TYPE;
            [DataType(Value = "VARCHAR2")]
            public string ARG_THREAD_TYPE
            {
                get { return _ARG_THREAD_TYPE; }
                set { _ARG_THREAD_TYPE = value; }
            }

            private string _ARG_PROTO_YN;
            [DataType(Value = "VARCHAR2")]
            public string ARG_PROTO_YN
            {
                get { return _ARG_PROTO_YN; }
                set { _ARG_PROTO_YN = value; }
            }

            private string _ARG_WATER_JET_YN;
            [DataType(Value = "VARCHAR2")]
            public string ARG_WATER_JET_YN
            {
                get { return _ARG_WATER_JET_YN; }
                set { _ARG_WATER_JET_YN = value; }
            }

            private string _ARG_DCS;
            [DataType(Value = "VARCHAR2")]
            public string ARG_DCS
            {
                get { return _ARG_DCS; }
                set { _ARG_DCS = value; }
            }

            private string _ARG_REWORK_YN;
            [DataType(Value = "VARCHAR2")]
            public string ARG_REWORK_YN
            {
                get { return _ARG_REWORK_YN; }
                set { _ARG_REWORK_YN = value; }
            }

            private string _ARG_INNOVATION_YN;
            [DataType(Value = "VARCHAR2")]
            public string ARG_INNOVATION_YN
            {
                get { return _ARG_INNOVATION_YN; }
                set { _ARG_INNOVATION_YN = value; }
            }

            private string _ARG_CBD_YN;
            [DataType(Value = "VARCHAR2")]
            public string ARG_CBD_YN
            {
                get { return _ARG_CBD_YN; }
                set { _ARG_CBD_YN = value; }
            }

            private string _ARG_UPD_USER;
            [DataType(Value = "VARCHAR2")]
            public string ARG_UPD_USER
            {
                get { return _ARG_UPD_USER; }
                set { _ARG_UPD_USER = value; }
            }

            private string _ARG_SUB_TYPE_REMARK;
            [DataType(Value = "VARCHAR2")]
            public string ARG_SUB_TYPE_REMARK
            {
                get { return _ARG_SUB_TYPE_REMARK; }
                set { _ARG_SUB_TYPE_REMARK = value; }
            }

            private string _ARG_REWORK_COMMENT;
            [DataType(Value = "VARCHAR2")]
            public string ARG_REWORK_COMMENT
            {
                get { return _ARG_REWORK_COMMENT; }
                set { _ARG_REWORK_COMMENT = value; }
            }
        }

        public class UPDATE_COMMENTS
        {
            private string _ARG_FACTORY;
            [DataType(Value = "VARCHAR2")]
            public string ARG_FACTORY
            {
                get { return _ARG_FACTORY; }
                set { _ARG_FACTORY = value; }
            }

            private string _ARG_WS_NO;
            [DataType(Value = "VARCHAR2")]
            public string ARG_WS_NO
            {
                get { return _ARG_WS_NO; }
                set { _ARG_WS_NO = value; }
            }

            private string _ARG_SEQ;
            [DataType(Value = "VARCHAR2")]
            public string ARG_SEQ
            {
                get { return _ARG_SEQ; }
                set { _ARG_SEQ = value; }
            }

            private string _ARG_COMMENT;
            [DataType(Value = "VARCHAR2")]
            public string ARG_COMMENT
            {
                get { return _ARG_COMMENT; }
                set { _ARG_COMMENT = value; }
            }

            private string _ARG_UPD_USER;
            [DataType(Value = "VARCHAR2")]
            public string ARG_UPD_USER
            {
                get { return _ARG_UPD_USER; }
                set { _ARG_UPD_USER = value; }
            }
        }
    }
}