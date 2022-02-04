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
    public partial class FindProcess : DevExpress.XtraEditors.XtraForm
    {
        public ProjectBaseForm projectBaseForm = Common.projectBaseForm;
        // 유저가 선택한 프로세스 리스트
        public List<string> LIST_OF_SELECTED_PROCESS = new List<string>();
        // 이미 선택된 프로세스를 팝업에 체크 표시하기 위함
        public string SELECTED_PROCESS_FROM_PARENT;

        public FindProcess()
        {
            InitializeComponent();
        }

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);

            #region 그리드에 데이터 바인딩
            // BE
            PKG_INTG_BOM.SELECT_MST_PROCESS pkgSelectBE = new PKG_INTG_BOM.SELECT_MST_PROCESS();
            pkgSelectBE.ARG_DIV = "BE";
            pkgSelectBE.OUT_CURSOR = string.Empty;

            DataTable dataSourceBE = projectBaseForm.Exe_Select_PKG(pkgSelectBE).Tables[0];

            grdBE.DataSource = dataSourceBE;

            // MO
            PKG_INTG_BOM.SELECT_MST_PROCESS pkgSelectMO = new PKG_INTG_BOM.SELECT_MST_PROCESS();
            pkgSelectMO.ARG_DIV = "MO";
            pkgSelectMO.OUT_CURSOR = string.Empty;

            DataTable dataSourceMO = projectBaseForm.Exe_Select_PKG(pkgSelectMO).Tables[0];

            grdMO.DataSource = dataSourceMO;

            // SL
            PKG_INTG_BOM.SELECT_MST_PROCESS pkgSelectSL = new PKG_INTG_BOM.SELECT_MST_PROCESS();
            pkgSelectSL.ARG_DIV = "SL";
            pkgSelectSL.OUT_CURSOR = string.Empty;

            DataTable dataSourceSL = projectBaseForm.Exe_Select_PKG(pkgSelectSL).Tables[0];

            grdSL.DataSource = dataSourceSL;

            // UP
            PKG_INTG_BOM.SELECT_MST_PROCESS pkgSelectUP = new PKG_INTG_BOM.SELECT_MST_PROCESS();
            pkgSelectUP.ARG_DIV = "UP";
            pkgSelectUP.OUT_CURSOR = string.Empty;

            DataTable dataSourceUP = projectBaseForm.Exe_Select_PKG(pkgSelectUP).Tables[0];

            grdUP.DataSource = dataSourceUP;
            #endregion

            #region 선택된 프로세스가 이미 있는 경우 체크 표시

            string[] process = SELECTED_PROCESS_FROM_PARENT.Split(',');

            if (process.Length > 0)
            {
                for (int i = 0; i < gvwBE.RowCount; i++)
                {
                    string nameOfProcess = gvwBE.GetRowCellValue(i, "OP_NAME_EN").ToString();

                    if (process.Contains(nameOfProcess))
                        gvwBE.SetRowCellValue(i, "CHK", "Y");
                }

                for (int i = 0; i < gvwMO.RowCount; i++)
                {
                    string nameOfProcess = gvwMO.GetRowCellValue(i, "OP_NAME_EN").ToString();

                    if (process.Contains(nameOfProcess))
                        gvwMO.SetRowCellValue(i, "CHK", "Y");
                }

                for (int i = 0; i < gvwSL.RowCount; i++)
                {
                    string nameOfProcess = gvwSL.GetRowCellValue(i, "OP_NAME_EN").ToString();

                    if (process.Contains(nameOfProcess))
                        gvwSL.SetRowCellValue(i, "CHK", "Y");
                }

                for (int i = 0; i < gvwUP.RowCount; i++)
                {
                    string nameOfProcess = gvwUP.GetRowCellValue(i, "OP_NAME_EN").ToString();

                    if (process.Contains(nameOfProcess))
                        gvwUP.SetRowCellValue(i, "CHK", "Y");
                }
            }
            ShowSelectedProcess();
            #endregion
        }

        /// <summary>
        /// 선택한 프로세스 표기
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void CustomClick(object sender, EventArgs e)
        {
            try
            {
                // 이벤트를 발생 시킨 뷰
                GridView view = sender as GridView;
                // 현재 체크 상태값
                string isChk = view.GetRowCellValue(view.FocusedRowHandle, "CHK").ToString();
                // 상태값에 따라 체크 표기
                if (isChk == "Y")
                    view.SetRowCellValue(view.FocusedRowHandle, "CHK", "N");
                else
                    view.SetRowCellValue(view.FocusedRowHandle, "CHK", "Y");

                ShowSelectedProcess();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
                return;
            }
        }

        /// <summary>
        /// 체크된 프로세스를 상단 라벨에 표시
        /// </summary>
        private void ShowSelectedProcess()
        {
            // 호출 시점에 항상 초기화하여 중복 제거
            lblConcatProcess.Text = "";
            // 각 그리드 별로 반복 수행
            for (int i = 0; i < gvwBE.RowCount; i++)
            {
                string isChk = gvwBE.GetRowCellValue(i, "CHK").ToString();
                if (isChk == "Y")
                {
                    string nameOfProcess = gvwBE.GetRowCellValue(i, "OP_NAME_EN").ToString();
                    lblConcatProcess.Text += "*" + nameOfProcess + "\n";
                }
            }

            for (int i = 0; i < gvwMO.RowCount; i++)
            {
                string isChk = gvwMO.GetRowCellValue(i, "CHK").ToString();
                if (isChk == "Y")
                {
                    string nameOfProcess = gvwMO.GetRowCellValue(i, "OP_NAME_EN").ToString();
                    lblConcatProcess.Text += "*" + nameOfProcess + "\n";
                }

            }

            for (int i = 0; i < gvwSL.RowCount; i++)
            {
                string isChk = gvwSL.GetRowCellValue(i, "CHK").ToString();
                if (isChk == "Y")
                {
                    string nameOfProcess = gvwSL.GetRowCellValue(i, "OP_NAME_EN").ToString();
                    lblConcatProcess.Text += "*" + nameOfProcess + "\n";
                }
            }

            for (int i = 0; i < gvwUP.RowCount; i++)
            {
                string isChk = gvwUP.GetRowCellValue(i, "CHK").ToString();
                if (isChk == "Y")
                {
                    string nameOfProcess = gvwUP.GetRowCellValue(i, "OP_NAME_EN").ToString();
                    lblConcatProcess.Text += "*" + nameOfProcess + "\n";
                }
            }
        }
        
        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnSave_Click(object sender, EventArgs e)
        {
            AddSelectedProcessToList(gvwBE);
            AddSelectedProcessToList(gvwMO);
            AddSelectedProcessToList(gvwSL);
            AddSelectedProcessToList(gvwUP);

            this.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.Close();
        }

        /// <summary>
        /// 각 탭에 선택된 프로세스를 리스트에 저장하여 부모폼으로 내림
        /// </summary>
        /// <param name="view"></param>
        private void AddSelectedProcessToList(GridView view)
        {
            // 각 그리드의 프로세스 개수만큼 반복
            for (int i = 0; i < view.RowCount; i++)
            {
                // 체크 여부
                string isChk = view.GetRowCellValue(i, "CHK").ToString();
                // 체크된 경우 해당 프로세스를 리스트에 저장
                if (isChk == "Y")
                {
                    // 프로세스 명칭
                    string processName = view.GetRowCellValue(i, "OP_NAME_EN").ToString();
                    // 리스트에 저장
                    LIST_OF_SELECTED_PROCESS.Add(processName);
                }
            }
        }
    }
}