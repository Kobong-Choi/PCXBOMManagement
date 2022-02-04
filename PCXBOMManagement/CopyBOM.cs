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
using DevExpress.XtraSplashScreen;              // XtraSplashScreen
using CSI.Client.ProjectBaseForm;               // ProjectBaseForm Class
using CSI.PCC.PCX.COM;                          // Common Class
using CSI.PCC.PCX.PACKAGE;                      // Package Class

namespace CSI.PCC.PCX
{
    public partial class CopyBOM : DevExpress.XtraEditors.XtraForm
    {
        public ProjectBaseForm projectBaseForm = Common.projectBaseForm;

        // Properties.
        public string Factory { get; set; }
        public string WSNo { get; set; }
        public string SeasonCode { get; set; }
        public string SampleTypeCode { get; set; }
        public string SubTypeCode { get; set; }
        public string ColorwayID { get; set; }

        public CopyBOM()
        {
            InitializeComponent();
        }

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);

            Common.BindLookUpEdit("Season", leSeason, false, "");
            Common.BindLookUpEdit("SampleType", leSampleType, false, "Y");
            Common.BindLookUpEdit("SubType", leSubType, false, "");
            Common.BindLookUpEdit("Factory", leFactory, false, "");

            // Set default value.
            leSeason.EditValue = SeasonCode;
            leSampleType.EditValue = SampleTypeCode;
            leSubType.EditValue = SubTypeCode;

            // Only show factory to JJ.
            if (Factory.Equals("JJ") == false)
            {
                labelControl4.Visible = false;
                leFactory.Visible = false;
                this.Height = 217;
            }
        }

        /// <summary>
        /// Copy BOM.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnSave_Click(object sender, EventArgs e)
        {
            try
            {
                // WaitForm Show.
                SplashScreenManager.ShowForm(this, typeof(MainWaitForm), false, false, false);

                bool isRJCopy = false;

                // JJ can only change the factory to RJ.
                if (Factory.Equals("JJ") && leFactory.EditValue.ToString().Equals("RJ"))
                {
                    isRJCopy = true;
                    Factory = "RJ";
                }

                // Major rounds that can not be copied.
                string[] majorRounds = new string[] { "PRDR", "PRAL", "PRO", "GTMC", "PRDC" };
                string subTypeRemark = txtSubTypeRemark.Text.Trim();

                /************************ Copy Rule*************************
                 * 1. Remark is required.
                 * 2. Major rounds can not be copied.
                 * ---------------------------------------------------------
                 *           Sample Type           / Sub Type
                 * ---------------------------------------------------------
                 *   (PRDR, PRAL, PRO, GTMC, PRDC) /    NA    -> Copy X
                 ***********************************************************/

                if ((majorRounds.Contains(leSampleType.EditValue.ToString()) == true) &&
                    leSubType.EditValue.ToString() == "NA")
                {
                    MessageBox.Show("Major rounds can not be copied.", "",
                        MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }
                else if (subTypeRemark == "")
                {
                    MessageBox.Show("Please input remark.", "",
                        MessageBoxButtons.OK, MessageBoxIcon.Information);
                    txtSubTypeRemark.Focus();

                    return;
                }
                
                // Check BOM uniqueness.
                PKG_INTG_BOM.SELECT_IS_EXISTING_BOM pkgSelect = new PKG_INTG_BOM.SELECT_IS_EXISTING_BOM();
                pkgSelect.ARG_FACTORY = Factory;
                pkgSelect.ARG_WS_NO = "All";    // Allow search BOM including itself.
                pkgSelect.ARG_SEASON_CD = leSeason.EditValue.ToString();
                pkgSelect.ARG_SAMPLE_TYPE = leSampleType.EditValue.ToString();
                pkgSelect.ARG_SUB_TYPE = leSubType.EditValue.ToString();
                pkgSelect.ARG_SUB_TYPE_REMARK = subTypeRemark;
                pkgSelect.ARG_DEV_COLORWAY_ID = ColorwayID;
                pkgSelect.OUT_CURSOR = string.Empty;

                DataTable result = projectBaseForm.Exe_Select_PKG(pkgSelect).Tables[0];

                bool isExisting = Convert.ToInt32(result.Rows[0]["ROW_COUNT"]) == 0 ? false : true;
                if (isExisting == true)
                {
                    MessageBox.Show("A BOM with the same information already exists.\nPlease check the criteria of unique.", "",
                        MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    txtSubTypeRemark.Focus();

                    return;
                }
                else
                {
                    /* Copy BOM. */

                    PKG_INTG_BOM.INSERT_COPY_BOM pkgInsert = new PKG_INTG_BOM.INSERT_COPY_BOM();
                    pkgInsert.ARG_FACTORY = Factory;
                    pkgInsert.ARG_WS_NO = WSNo;
                    pkgInsert.ARG_SEASON_CD = leSeason.EditValue.ToString();
                    pkgInsert.ARG_ST_CD = leSampleType.EditValue.ToString();
                    pkgInsert.ARG_SUB_ST_CD = leSubType.EditValue.ToString();
                    pkgInsert.ARG_SUB_TYPE_REMARK = subTypeRemark;
                    pkgInsert.ARG_UPD_USER = Common.sessionID;
                    pkgInsert.ARG_CS_BOM_CFM = rgTypeSelection.EditValue.ToString();
                    pkgInsert.ARG_IS_RJ_COPY = (isRJCopy == true) ? "Y" : "N";

                    ArrayList arrayList = new ArrayList();
                    arrayList.Add(pkgInsert);

                    if (projectBaseForm.Exe_Modify_PKG(arrayList) == null)
                    {
                        MessageBox.Show("Failed to copy.");
                        return;
                    }
                }

                // WaitForm Close
                SplashScreenManager.CloseForm(false);

                MessageBox.Show("Complete");

                this.DialogResult = System.Windows.Forms.DialogResult.OK;
                this.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
                return;
            }
            finally
            {
                // WaitForm Close
                SplashScreenManager.CloseForm(false);
            }
        }
    }
}