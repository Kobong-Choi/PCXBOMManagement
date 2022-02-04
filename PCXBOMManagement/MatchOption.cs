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
using DevExpress.XtraGrid.Columns;              // GridColumn
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
    public partial class MatchOption : DevExpress.XtraEditors.XtraForm
    {
        public string MATERIAL_OPTION = string.Empty;
        public string COLOR_OPTION = string.Empty;
        public bool IS_CLICKED = false;

        public MatchOption()
        {
            InitializeComponent();
        }

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);

            rdMatOpt.SelectedIndex = 1;
            rdColorOpt.SelectedIndex = 0;
        }
        
        private void btnConfirm_Click(object sender, EventArgs e)
        {
            // 매칭 자재 옵션 저장
            MATERIAL_OPTION = rdMatOpt.EditValue.ToString();
            // 매칭 컬러 옵션 저장 
            COLOR_OPTION = rdColorOpt.EditValue.ToString();
            // 정상 처리
            IS_CLICKED = true;
            // 폼 종료
            DialogResult = System.Windows.Forms.DialogResult.OK;
        }

        private void MatchOption_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (IS_CLICKED == true)
            {
                // 정상 종료
            }
            else
            {
                // 매칭을 안할 것인지 물어봄
                if (MessageBox.Show("Don't you want to match?", "Question", MessageBoxButtons.OKCancel, MessageBoxIcon.Question,
                    MessageBoxDefaultButton.Button1) == System.Windows.Forms.DialogResult.OK)
                {
                    DialogResult = System.Windows.Forms.DialogResult.Cancel;
                }
                else
                {
                    MessageBox.Show("Please choose the options.");
                    e.Cancel = true; ;
                }
            }
        }
    }
}