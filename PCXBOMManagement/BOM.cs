using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

using CSI.Client.ProjectBaseForm;   //ProjectBaseForm Class
using CSI.PCC.PCX.COM;              //Common Class
using CSI.PCC.PCX.PACKAGE;          //Package Class
using DevExpress.XtraGrid.Views.Grid;   //GridView Event

namespace CSI.PCC.PCX
{
    public partial class BOM : DevExpress.XtraEditors.XtraForm
    {
        #region User Defined Variables
        public ProjectBaseForm projectForm = Common.PROJECTBASEFORM.projectForm;
        public PCXBOMManagement parentForm = null;

        public string _bomKey = string.Empty;
        public string _formName = string.Empty;
        #endregion

        public BOM()
        {
            InitializeComponent();
        }

        private void BOM_Load(object sender, EventArgs e)
        {
            this.Text = _formName;

            parentForm._SetGridDragRowSelect(gvwBomData);   //그리드 로우 멀티 셀렉트

            #region BOM Data 그리드 바인딩
            PKG_PCX_BOM_MNG.SELECT_BOM_DATA pkgSelectBomData = new PKG_PCX_BOM_MNG.SELECT_BOM_DATA();
            pkgSelectBomData.ARG_BOM_KEY = _bomKey;
            pkgSelectBomData.OUT_CURSOR = string.Empty;

            DataTable dataSource = projectForm.Exe_Select_PKG(pkgSelectBomData).Tables[0];

            //foreach (DataColumn column in dataSource.Columns)
            //{
            //    column.AllowDBNull = true;
            //}

            grdBomData.DataSource = dataSource;
            #endregion

            gvwBomData.BestFitColumns();
        }

        #region Control Event handler
        private void gvwBomData_RowCellStyle(object sender, DevExpress.XtraGrid.Views.Grid.RowCellStyleEventArgs e)
        {
            GridView gvwBomData = sender as GridView;

            //선택한 로우는 셀 스타일 적용하지 않음
            if (gvwBomData.GetSelectedRows().Contains(e.RowHandle)) return;

            string fieldName = e.Column.FieldName;  //현재 셀의 컬럼의 필드명

            if (fieldName.Equals("CS_PART_NAME"))   //매칭되지 않은 파트는 빨간불
            {
                if (gvwBomData.GetRowCellValue(e.RowHandle, "CS_PART_NAME").ToString().Equals(""))
                    e.Appearance.BackColor = Color.Red;
            }
        }
        #endregion

        private void cntxBomDataMenu_Click(object sender, EventArgs e)
        {
            ToolStripMenuItem menu = sender as ToolStripMenuItem;

            string clickedItem = menu.Tag.ToString();

            switch (clickedItem)
            {
                case "1":
                    {
                        AddLine();
                    }
                    break;
                case "2":
                    {
                        FindCode();
                    }
                    break;
            }
        }

        private void AddLine()
        {
            DataTable dataSource = grdBomData.DataSource as DataTable;

            int maxPartSeq = 0;
            //현재 라인 아이템 중 가장 큰 Part Sequence를 구함
            foreach (DataRow dr in dataSource.Rows)
            {
                int curPartSeq = Convert.ToInt32(dr["PART_SEQ"].ToString());
                maxPartSeq = Math.Max(maxPartSeq, curPartSeq);
            }

            DataRow newRow = dataSource.NewRow();   //신규 라인 생성
            newRow["BOM_KEY"] = _bomKey;            //키 값 업데이트
            newRow["PART_SEQ"] = (maxPartSeq + 1).ToString();   //신규 파트 시퀀스 업데이트

            dataSource.Rows.InsertAt(newRow, gvwBomData.FocusedRowHandle + 1);  //신규 라인 삽입

            grdBomData.DataSource = dataSource; //데이터 소스에 반영
        }

        private void FindCode()
        {
            int initialIndex = 0;
            string initialContent = gvwBomData.GetFocusedRowCellValue(gvwBomData.FocusedColumn).ToString();

            if (gvwBomData.FocusedColumn.FieldName.Equals("PCX_PART_TYPE"))
                initialIndex = 0;
            else if (gvwBomData.FocusedColumn.FieldName.Equals("PCX_PART_NAME"))
                initialIndex = 1;
            else if (gvwBomData.FocusedColumn.FieldName.Equals("CS_PART_NAME"))
                initialIndex = 2;
            else if (gvwBomData.FocusedColumn.FieldName.Equals("PDM_SUPP_MAT_NUM") || gvwBomData.FocusedColumn.FieldName.Equals("CS_CD") || gvwBomData.FocusedColumn.FieldName.Equals("PDM_MAT_NAME"))
                initialIndex = 3;

            FindCode findForm = new FindCode();
            findForm._initialIndex = initialIndex;
            findForm._initialContent = initialContent;
            findForm.Show();
        }
    }
}