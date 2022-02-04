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
using DevExpress.XtraGrid.Views.Grid;           // GridView

namespace CSI.PCC.PCX
{
    public partial class BOMSearch : DevExpress.XtraEditors.XtraForm
    {
        public string Factory { get; set; }
        public string WSNumber { get; set; }
        public string StyleName { get; set; }
        public string Mode { get; set; }
        private bool hasDone = false;

        public BOMSearch()
        {
            InitializeComponent();
        }

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);

            // Bind dataSource to LookUpEidt.
            Common.BindLookUpEdit("Factory", lePCC, true, "");
            Common.BindLookUpEdit("Season", leSeason, true, "");
            Common.BindLookUpEdit("SampleType", leSampleType, true, "A");
            Common.BindLookUpEdit("SubType", leSubType, true, "");
            Common.BindLookUpEdit("Developer", leDeveloper, true, "");

            if (Mode.Equals("Worksheet"))
                txtStyleName.Text = StyleName;
        }

        #region Button Events.

        private void btnSearch_Click(object sender, EventArgs e)
        {
            grdBOMHead.DataSource = BindDataSourceGridView();
        }

        private DataTable BindDataSourceGridView()
        {
            DataSet ds;

            PKG_INTG_BOM.SELECT_BOM_LIST_COMMON pkgSelect = new PKG_INTG_BOM.SELECT_BOM_LIST_COMMON();
            pkgSelect.ARG_FACTORY = lePCC.EditValue.ToString();
            pkgSelect.ARG_WS_NO = WSNumber;
            pkgSelect.ARG_SEASON_CD = leSeason.EditValue.ToString();
            pkgSelect.ARG_ST_CD = leSampleType.EditValue.ToString();
            pkgSelect.ARG_SUB_ST_CD = leSubType.EditValue.ToString();
            pkgSelect.ARG_PCC_PM = leDeveloper.EditValue.ToString();
            pkgSelect.ARG_DEV_COLORWAY_ID = txtDevColorwayId.Text.Trim();
            pkgSelect.ARG_STYLE_NAME = txtStyleName.Text.Trim();
            pkgSelect.OUT_CURSOR = string.Empty;

            ds = Common.projectBaseForm.Exe_Select_PKG(pkgSelect);

            if (ds.Tables[0].Rows.Count > 0)
                return ds.Tables[0];
            else
                return null;
        }

        #endregion

        #region Grid Events.

        private void gvwBOMHead_DoubleClick(object sender, EventArgs e)
        {
            GridView view = sender as GridView;

            if (Mode.Equals("Worksheet"))
            {
                if (CopyWorksheetInformation(view))
                    MessageBox.Show("Complete.");
            }
            else if (Mode.Equals("BOM"))
            {
                if (CanCopyBOM())
                    MessageBox.Show("Complete.");
            }

            hasDone = true;
            this.Close();
        }

        #endregion

        #region Control Events.

        private void CustomKeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                grdBOMHead.DataSource = BindDataSourceGridView();
            }
        }

        private void BOMSearch_FormClosing(object sender, FormClosingEventArgs e)
        {
            this.DialogResult = hasDone ? System.Windows.Forms.DialogResult.OK : System.Windows.Forms.DialogResult.No;
        }

        #endregion

        #region User defiend fuctions.

        /// <summary>
        /// 타겟 BOM의 Worksheet 정보를 복사하여 소스 BOM에 저장
        /// </summary>
        /// <param name="view"></param>
        /// <returns></returns>
        private bool CopyWorksheetInformation(GridView view)
        {
            try
            {
                ComLib.Wait_Form_Show(this);

                string sourceFactory = Factory;
                string sourceWsNo = WSNumber;
                string targetFactory = view.GetFocusedRowCellValue("FACTORY").ToString();
                string targetWsNo = view.GetFocusedRowCellValue("WS_NO").ToString();

                PKG_INTG_BOM_WORKSHEET.COPY_WORKSHEET_CONTENTS pkgInsert = new PKG_INTG_BOM_WORKSHEET.COPY_WORKSHEET_CONTENTS();
                pkgInsert.ARG_S_FACTORY = sourceFactory;
                pkgInsert.ARG_S_WS_NO = sourceWsNo;
                pkgInsert.ARG_T_FACTORY = targetFactory;
                pkgInsert.ARG_T_WS_NO = targetWsNo;
                pkgInsert.ARG_UPD_USER = Common.sessionID;

                ArrayList arrayList = new ArrayList();
                arrayList.Add(pkgInsert);

                if (Common.projectBaseForm.Exe_Modify_PKG(arrayList) == null)
                {
                    MessageBox.Show("Failed to copy worksheet contents.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return false;
                }

                return true;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
                return false;
            }
            finally
            {
                ComLib.Wait_Form_Close(this);
            }

        }

        /// <summary>
        /// BOM 라인 아이템을 선택한 BOM의 라인 아이템으로 일괄 변경
        /// </summary>
        /// <param name="view"></param>
        /// <returns></returns>
        private bool CanCopyBOM()
        {
            DataTable dataSource;
            ArrayList arrayList = new ArrayList();
            string targetFactory = gvwBOMHead.GetFocusedRowCellValue("FACTORY").ToString();
            string targetWsNo = gvwBOMHead.GetFocusedRowCellValue("WS_NO").ToString();

            // Show options of replace.
            ReplaceOption replaceOptionForm = new ReplaceOption();

            if (replaceOptionForm.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                switch (replaceOptionForm.REPLACE_OPTION)
                {
                    case "S":

                        #region Same As Target.

                        // Validate that there is a material in the BOM that has already been purchased.
                        dataSource = ReturnBOMInfo("PurchaseCheck");

                        if (dataSource.Rows.Count > 0)
                        {
                            if (Convert.ToInt32(dataSource.Rows[0]["CNT"]) > 0)
                            {
                                // If the material exists, this BOM can not be replaced. - discussed at BOM TFT.
                                Common.ShowMessageBox("There is a material in the BOM that has already been purchased.", "W");
                                return false;
                            }
                        }
                        else
                        {
                            Common.ShowMessageBox("Failed to load purchase order history.", "E");
                            return false;
                        }

                        PKG_INTG_BOM.REPLACE_SAME_AS_TARGET pkgUpdate = new PKG_INTG_BOM.REPLACE_SAME_AS_TARGET();
                        pkgUpdate.ARG_S_FACTORY = Factory;
                        pkgUpdate.ARG_S_WS_NO = WSNumber;
                        pkgUpdate.ARG_T_FACTORY = targetFactory;
                        pkgUpdate.ARG_T_WS_NO = targetWsNo;
                        pkgUpdate.ARG_UPD_USER = Common.sessionID;

                        arrayList.Add(pkgUpdate);

                        if (Common.projectBaseForm.Exe_Modify_PKG(arrayList) == null)
                        {
                            Common.ShowMessageBox("Failed to replace.", "E");
                            return false;
                        }

                        break;

                        #endregion

                    case "O":

                        #region by Option.

                        // Validate that there is a material in the BOM that has already been purchased.
                        dataSource = ReturnBOMInfo("PurchaseCheck");

                        if (dataSource.Rows.Count > 0)
                        {
                            if (Convert.ToInt32(dataSource.Rows[0]["CNT"]) > 0)
                                Common.ShowMessageBox("Order Data before replace can be tracked on Purchase List", "W");
                        }
                        else
                        {
                            Common.ShowMessageBox("Failed to load purchase order history.", "E");
                            return false;
                        }

                        MatchOption matchOptionForm = new MatchOption();

                        if (matchOptionForm.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                        {
                            if (ArePartsDuplicate("source", Factory, WSNumber))
                                return false;

                            if (ArePartsDuplicate("target", targetFactory, targetWsNo))
                                return false;

                            PKG_INTG_BOM.REPLACE_BY_OPTION pkgInsert = new PKG_INTG_BOM.REPLACE_BY_OPTION();
                            pkgInsert.ARG_S_FACTORY = Factory;
                            pkgInsert.ARG_S_WS_NO = WSNumber;
                            pkgInsert.ARG_T_FACTORY = targetFactory;
                            pkgInsert.ARG_T_WS_NO = targetWsNo;
                            pkgInsert.ARG_UPD_USER = Common.sessionID;
                            pkgInsert.ARG_MAT_OPT = matchOptionForm.MATERIAL_OPTION;
                            pkgInsert.ARG_COLOR_OPT = matchOptionForm.COLOR_OPTION;

                            arrayList.Add(pkgInsert);

                            if (Common.projectBaseForm.Exe_Modify_PKG(arrayList) == null)
                            {
                                Common.ShowMessageBox("Failed to replace.", "E");
                                return false;
                            }
                        }
                        else
                            return false;

                        break;

                        #endregion
                }
            }
            else
                return false;

            return true;
        }

        /// <summary>
        /// See if there are parts duplicate in the BOM.
        /// </summary>
        /// <param name="workType"></param>
        /// <param name="factory"></param>
        /// <param name="wsNo"></param>
        /// <returns></returns>
        private bool ArePartsDuplicate(string workType, string factory, string wsNo)
        {
            PKG_INTG_BOM.SELECT_PART_DUPLICATE pkgSelect = new PKG_INTG_BOM.SELECT_PART_DUPLICATE();
            pkgSelect.ARG_WORK_TYPE = workType;
            pkgSelect.ARG_FACTORY = factory;
            pkgSelect.ARG_WS_NO = wsNo;
            pkgSelect.OUT_CURSOR = string.Empty;

            DataTable dataSource = Common.projectBaseForm.Exe_Select_PKG(pkgSelect).Tables[0];

            if (dataSource.Rows.Count > 0)
            {
                MessageBox.Show(dataSource.Rows[0]["PART_NAME"].ToString() + " is duplicate part. Please check " + workType + " BOM.", "",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning, MessageBoxDefaultButton.Button1);
                
                return true;
            }

            return false;
        }

        /// <summary>
        /// Load BOM header information.
        /// </summary>
        /// <param name="workType"></param>
        /// <returns></returns>
        private DataTable ReturnBOMInfo(string workType)
        {
            PKG_INTG_BOM.SELECT_BOM_INFO_BY_CONFIRM pkgSelect = new PKG_INTG_BOM.SELECT_BOM_INFO_BY_CONFIRM();
            pkgSelect.ARG_WORK_TYPE = workType;
            pkgSelect.ARG_FACTORY = Factory;
            pkgSelect.ARG_WS_NO = WSNumber;
            pkgSelect.ARG_PART_SEQ = "";
            pkgSelect.OUT_CURSOR = string.Empty;

            DataTable dataSource = Common.projectBaseForm.Exe_Select_PKG(pkgSelect).Tables[0];

            return dataSource;
        }

        #endregion
    }
}