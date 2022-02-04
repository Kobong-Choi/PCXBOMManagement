namespace CSI.PCC.PCX
{
    partial class TransferRecord
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            DevExpress.XtraGrid.StyleFormatCondition styleFormatCondition1 = new DevExpress.XtraGrid.StyleFormatCondition();
            this.panelControl1 = new DevExpress.XtraEditors.PanelControl();
            this.panelControl2 = new DevExpress.XtraEditors.PanelControl();
            this.panelControl3 = new DevExpress.XtraEditors.PanelControl();
            this.groupControl1 = new DevExpress.XtraEditors.GroupControl();
            this.grdData = new DevExpress.XtraGrid.GridControl();
            this.gvwData = new DevExpress.XtraGrid.Views.Grid.GridView();
            this.gridColumn1 = new DevExpress.XtraGrid.Columns.GridColumn();
            this.colFACTORY = new DevExpress.XtraGrid.Columns.GridColumn();
            this.colWS_NO = new DevExpress.XtraGrid.Columns.GridColumn();
            this.colPART_SEQ = new DevExpress.XtraGrid.Columns.GridColumn();
            this.gridColumn2 = new DevExpress.XtraGrid.Columns.GridColumn();
            this.colUPD_USER = new DevExpress.XtraGrid.Columns.GridColumn();
            this.colUPD_YMD = new DevExpress.XtraGrid.Columns.GridColumn();
            this.colPART_NAME = new DevExpress.XtraGrid.Columns.GridColumn();
            this.colMXSXL_NUMBER = new DevExpress.XtraGrid.Columns.GridColumn();
            this.colMCS_NUMBER = new DevExpress.XtraGrid.Columns.GridColumn();
            this.colPCX_SUPP_MAT_ID = new DevExpress.XtraGrid.Columns.GridColumn();
            this.colPCX_MAT_ID = new DevExpress.XtraGrid.Columns.GridColumn();
            this.colMAT_NAME = new DevExpress.XtraGrid.Columns.GridColumn();
            this.colMAT_COMMENT = new DevExpress.XtraGrid.Columns.GridColumn();
            this.colCOLOR_CD = new DevExpress.XtraGrid.Columns.GridColumn();
            this.colCOLOR_NAME = new DevExpress.XtraGrid.Columns.GridColumn();
            this.gridColumn30 = new DevExpress.XtraGrid.Columns.GridColumn();
            ((System.ComponentModel.ISupportInitialize)(this.panelControl1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.panelControl2)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.panelControl3)).BeginInit();
            this.panelControl3.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.groupControl1)).BeginInit();
            this.groupControl1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.grdData)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.gvwData)).BeginInit();
            this.SuspendLayout();
            // 
            // panelControl1
            // 
            this.panelControl1.Dock = System.Windows.Forms.DockStyle.Top;
            this.panelControl1.Location = new System.Drawing.Point(0, 0);
            this.panelControl1.Name = "panelControl1";
            this.panelControl1.Size = new System.Drawing.Size(1184, 51);
            this.panelControl1.TabIndex = 0;
            this.panelControl1.Visible = false;
            // 
            // panelControl2
            // 
            this.panelControl2.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panelControl2.Location = new System.Drawing.Point(0, 582);
            this.panelControl2.Name = "panelControl2";
            this.panelControl2.Size = new System.Drawing.Size(1184, 100);
            this.panelControl2.TabIndex = 1;
            this.panelControl2.Visible = false;
            // 
            // panelControl3
            // 
            this.panelControl3.Controls.Add(this.groupControl1);
            this.panelControl3.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelControl3.Location = new System.Drawing.Point(0, 51);
            this.panelControl3.Name = "panelControl3";
            this.panelControl3.Size = new System.Drawing.Size(1184, 531);
            this.panelControl3.TabIndex = 2;
            // 
            // groupControl1
            // 
            this.groupControl1.Controls.Add(this.grdData);
            this.groupControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.groupControl1.Location = new System.Drawing.Point(2, 2);
            this.groupControl1.Name = "groupControl1";
            this.groupControl1.Size = new System.Drawing.Size(1180, 527);
            this.groupControl1.TabIndex = 0;
            this.groupControl1.Text = "Records";
            // 
            // grdData
            // 
            this.grdData.Dock = System.Windows.Forms.DockStyle.Fill;
            this.grdData.Location = new System.Drawing.Point(2, 22);
            this.grdData.MainView = this.gvwData;
            this.grdData.Name = "grdData";
            this.grdData.Size = new System.Drawing.Size(1176, 503);
            this.grdData.TabIndex = 0;
            this.grdData.ViewCollection.AddRange(new DevExpress.XtraGrid.Views.Base.BaseView[] {
            this.gvwData});
            // 
            // gvwData
            // 
            this.gvwData.Columns.AddRange(new DevExpress.XtraGrid.Columns.GridColumn[] {
            this.gridColumn1,
            this.colFACTORY,
            this.colWS_NO,
            this.colPART_SEQ,
            this.gridColumn2,
            this.colUPD_USER,
            this.colUPD_YMD,
            this.colPART_NAME,
            this.colMXSXL_NUMBER,
            this.colMCS_NUMBER,
            this.colPCX_SUPP_MAT_ID,
            this.colPCX_MAT_ID,
            this.colMAT_NAME,
            this.colMAT_COMMENT,
            this.colCOLOR_CD,
            this.colCOLOR_NAME,
            this.gridColumn30});
            styleFormatCondition1.Appearance.BackColor = System.Drawing.Color.Black;
            styleFormatCondition1.Appearance.ForeColor = System.Drawing.Color.White;
            styleFormatCondition1.Appearance.Options.UseBackColor = true;
            styleFormatCondition1.Appearance.Options.UseForeColor = true;
            styleFormatCondition1.Condition = DevExpress.XtraGrid.FormatConditionEnum.Expression;
            styleFormatCondition1.Expression = "[TempPartCD] = \'X\'";
            this.gvwData.FormatConditions.AddRange(new DevExpress.XtraGrid.StyleFormatCondition[] {
            styleFormatCondition1});
            this.gvwData.GridControl = this.grdData;
            this.gvwData.IndicatorWidth = 30;
            this.gvwData.Name = "gvwData";
            this.gvwData.OptionsBehavior.CopyToClipboardWithColumnHeaders = false;
            this.gvwData.OptionsSelection.MultiSelect = true;
            this.gvwData.OptionsSelection.MultiSelectMode = DevExpress.XtraGrid.Views.Grid.GridMultiSelectMode.CellSelect;
            this.gvwData.OptionsView.ColumnAutoWidth = false;
            this.gvwData.OptionsView.ShowGroupPanel = false;
            this.gvwData.RowCellStyle += new DevExpress.XtraGrid.Views.Grid.RowCellStyleEventHandler(this.gvwData_RowCellStyle);
            // 
            // gridColumn1
            // 
            this.gridColumn1.Caption = "REQ_KEY";
            this.gridColumn1.FieldName = "REQ_KEY";
            this.gridColumn1.Fixed = DevExpress.XtraGrid.Columns.FixedStyle.Left;
            this.gridColumn1.Name = "gridColumn1";
            this.gridColumn1.OptionsColumn.AllowEdit = false;
            // 
            // colFACTORY
            // 
            this.colFACTORY.AppearanceCell.Options.UseTextOptions = true;
            this.colFACTORY.AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.colFACTORY.AppearanceHeader.Options.UseTextOptions = true;
            this.colFACTORY.AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.colFACTORY.Caption = "FactoryCode";
            this.colFACTORY.FieldName = "FACTORY";
            this.colFACTORY.Name = "colFACTORY";
            this.colFACTORY.OptionsColumn.AllowEdit = false;
            this.colFACTORY.Width = 100;
            // 
            // colWS_NO
            // 
            this.colWS_NO.AppearanceCell.Options.UseTextOptions = true;
            this.colWS_NO.AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.colWS_NO.AppearanceHeader.Options.UseTextOptions = true;
            this.colWS_NO.AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.colWS_NO.Caption = "WorkshopNumber";
            this.colWS_NO.FieldName = "WS_NO";
            this.colWS_NO.Name = "colWS_NO";
            this.colWS_NO.OptionsColumn.AllowEdit = false;
            this.colWS_NO.Width = 100;
            // 
            // colPART_SEQ
            // 
            this.colPART_SEQ.AppearanceCell.Options.UseTextOptions = true;
            this.colPART_SEQ.AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.colPART_SEQ.AppearanceHeader.Options.UseTextOptions = true;
            this.colPART_SEQ.AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.colPART_SEQ.Caption = "PartSequence";
            this.colPART_SEQ.FieldName = "PART_SEQ";
            this.colPART_SEQ.Name = "colPART_SEQ";
            this.colPART_SEQ.OptionsColumn.AllowEdit = false;
            this.colPART_SEQ.Width = 100;
            // 
            // gridColumn2
            // 
            this.gridColumn2.AppearanceCell.Options.UseTextOptions = true;
            this.gridColumn2.AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.gridColumn2.AppearanceHeader.Options.UseTextOptions = true;
            this.gridColumn2.AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.gridColumn2.Caption = "Status";
            this.gridColumn2.FieldName = "PROC_STATUS";
            this.gridColumn2.Fixed = DevExpress.XtraGrid.Columns.FixedStyle.Left;
            this.gridColumn2.Name = "gridColumn2";
            this.gridColumn2.OptionsColumn.AllowEdit = false;
            this.gridColumn2.Visible = true;
            this.gridColumn2.VisibleIndex = 0;
            // 
            // colUPD_USER
            // 
            this.colUPD_USER.AppearanceCell.Options.UseTextOptions = true;
            this.colUPD_USER.AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.colUPD_USER.AppearanceHeader.Options.UseTextOptions = true;
            this.colUPD_USER.AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.colUPD_USER.Caption = "Req. User";
            this.colUPD_USER.FieldName = "REQ_USER";
            this.colUPD_USER.Fixed = DevExpress.XtraGrid.Columns.FixedStyle.Left;
            this.colUPD_USER.Name = "colUPD_USER";
            this.colUPD_USER.OptionsColumn.AllowEdit = false;
            this.colUPD_USER.Visible = true;
            this.colUPD_USER.VisibleIndex = 1;
            this.colUPD_USER.Width = 85;
            // 
            // colUPD_YMD
            // 
            this.colUPD_YMD.AppearanceHeader.Options.UseTextOptions = true;
            this.colUPD_YMD.AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.colUPD_YMD.Caption = "Req. Date";
            this.colUPD_YMD.DisplayFormat.FormatString = "G";
            this.colUPD_YMD.DisplayFormat.FormatType = DevExpress.Utils.FormatType.DateTime;
            this.colUPD_YMD.FieldName = "REQ_DATE";
            this.colUPD_YMD.Fixed = DevExpress.XtraGrid.Columns.FixedStyle.Left;
            this.colUPD_YMD.Name = "colUPD_YMD";
            this.colUPD_YMD.OptionsColumn.AllowEdit = false;
            this.colUPD_YMD.Visible = true;
            this.colUPD_YMD.VisibleIndex = 2;
            this.colUPD_YMD.Width = 150;
            // 
            // colPART_NAME
            // 
            this.colPART_NAME.AppearanceHeader.Options.UseTextOptions = true;
            this.colPART_NAME.AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.colPART_NAME.Caption = "Part Name";
            this.colPART_NAME.FieldName = "PART_NAME";
            this.colPART_NAME.Name = "colPART_NAME";
            this.colPART_NAME.OptionsColumn.AllowEdit = false;
            this.colPART_NAME.OptionsFilter.FilterPopupMode = DevExpress.XtraGrid.Columns.FilterPopupMode.CheckedList;
            this.colPART_NAME.Visible = true;
            this.colPART_NAME.VisibleIndex = 3;
            this.colPART_NAME.Width = 150;
            // 
            // colMXSXL_NUMBER
            // 
            this.colMXSXL_NUMBER.AppearanceCell.Options.UseTextOptions = true;
            this.colMXSXL_NUMBER.AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Near;
            this.colMXSXL_NUMBER.AppearanceHeader.Options.UseTextOptions = true;
            this.colMXSXL_NUMBER.AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.colMXSXL_NUMBER.Caption = "PDM Supp.Mat.Code";
            this.colMXSXL_NUMBER.FieldName = "MXSXL_NUMBER";
            this.colMXSXL_NUMBER.Name = "colMXSXL_NUMBER";
            this.colMXSXL_NUMBER.OptionsColumn.AllowEdit = false;
            this.colMXSXL_NUMBER.Visible = true;
            this.colMXSXL_NUMBER.VisibleIndex = 4;
            this.colMXSXL_NUMBER.Width = 80;
            // 
            // colMCS_NUMBER
            // 
            this.colMCS_NUMBER.AppearanceCell.Options.UseTextOptions = true;
            this.colMCS_NUMBER.AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.colMCS_NUMBER.AppearanceHeader.Options.UseTextOptions = true;
            this.colMCS_NUMBER.AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.colMCS_NUMBER.Caption = "MCS";
            this.colMCS_NUMBER.FieldName = "MCS_NUMBER";
            this.colMCS_NUMBER.Name = "colMCS_NUMBER";
            this.colMCS_NUMBER.OptionsColumn.AllowEdit = false;
            this.colMCS_NUMBER.Visible = true;
            this.colMCS_NUMBER.VisibleIndex = 5;
            this.colMCS_NUMBER.Width = 80;
            // 
            // colPCX_SUPP_MAT_ID
            // 
            this.colPCX_SUPP_MAT_ID.AppearanceCell.Options.UseTextOptions = true;
            this.colPCX_SUPP_MAT_ID.AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.colPCX_SUPP_MAT_ID.AppearanceHeader.Options.UseTextOptions = true;
            this.colPCX_SUPP_MAT_ID.AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Near;
            this.colPCX_SUPP_MAT_ID.Caption = "PCX Supp. Mat ID";
            this.colPCX_SUPP_MAT_ID.FieldName = "PCX_SUPP_MAT_ID";
            this.colPCX_SUPP_MAT_ID.Name = "colPCX_SUPP_MAT_ID";
            this.colPCX_SUPP_MAT_ID.OptionsColumn.AllowEdit = false;
            this.colPCX_SUPP_MAT_ID.Visible = true;
            this.colPCX_SUPP_MAT_ID.VisibleIndex = 6;
            this.colPCX_SUPP_MAT_ID.Width = 80;
            // 
            // colPCX_MAT_ID
            // 
            this.colPCX_MAT_ID.AppearanceCell.Options.UseTextOptions = true;
            this.colPCX_MAT_ID.AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.colPCX_MAT_ID.AppearanceHeader.Options.UseTextOptions = true;
            this.colPCX_MAT_ID.AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Near;
            this.colPCX_MAT_ID.Caption = "PCX Mat. ID";
            this.colPCX_MAT_ID.FieldName = "PCX_MAT_ID";
            this.colPCX_MAT_ID.Name = "colPCX_MAT_ID";
            this.colPCX_MAT_ID.OptionsColumn.AllowEdit = false;
            this.colPCX_MAT_ID.Visible = true;
            this.colPCX_MAT_ID.VisibleIndex = 7;
            // 
            // colMAT_NAME
            // 
            this.colMAT_NAME.AppearanceHeader.Options.UseTextOptions = true;
            this.colMAT_NAME.AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.colMAT_NAME.Caption = "PDM Material Name";
            this.colMAT_NAME.FieldName = "MAT_NAME";
            this.colMAT_NAME.Name = "colMAT_NAME";
            this.colMAT_NAME.OptionsColumn.AllowEdit = false;
            this.colMAT_NAME.OptionsFilter.FilterPopupMode = DevExpress.XtraGrid.Columns.FilterPopupMode.CheckedList;
            this.colMAT_NAME.Visible = true;
            this.colMAT_NAME.VisibleIndex = 8;
            this.colMAT_NAME.Width = 250;
            // 
            // colMAT_COMMENT
            // 
            this.colMAT_COMMENT.AppearanceHeader.Options.UseTextOptions = true;
            this.colMAT_COMMENT.AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.colMAT_COMMENT.Caption = "Material Comment";
            this.colMAT_COMMENT.FieldName = "MAT_COMMENT";
            this.colMAT_COMMENT.Name = "colMAT_COMMENT";
            this.colMAT_COMMENT.OptionsColumn.AllowEdit = false;
            this.colMAT_COMMENT.Visible = true;
            this.colMAT_COMMENT.VisibleIndex = 9;
            this.colMAT_COMMENT.Width = 220;
            // 
            // colCOLOR_CD
            // 
            this.colCOLOR_CD.AppearanceCell.Options.UseTextOptions = true;
            this.colCOLOR_CD.AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.colCOLOR_CD.AppearanceHeader.Options.UseTextOptions = true;
            this.colCOLOR_CD.AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.colCOLOR_CD.Caption = "PDM Color Code";
            this.colCOLOR_CD.FieldName = "COLOR_CD";
            this.colCOLOR_CD.Name = "colCOLOR_CD";
            this.colCOLOR_CD.OptionsColumn.AllowEdit = false;
            this.colCOLOR_CD.Visible = true;
            this.colCOLOR_CD.VisibleIndex = 10;
            // 
            // colCOLOR_NAME
            // 
            this.colCOLOR_NAME.AppearanceCell.Options.UseTextOptions = true;
            this.colCOLOR_NAME.AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.colCOLOR_NAME.AppearanceHeader.Options.UseTextOptions = true;
            this.colCOLOR_NAME.AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.colCOLOR_NAME.Caption = "PDM Color Name";
            this.colCOLOR_NAME.FieldName = "COLOR_NAME";
            this.colCOLOR_NAME.Name = "colCOLOR_NAME";
            this.colCOLOR_NAME.OptionsColumn.AllowEdit = false;
            this.colCOLOR_NAME.OptionsFilter.FilterPopupMode = DevExpress.XtraGrid.Columns.FilterPopupMode.CheckedList;
            this.colCOLOR_NAME.Visible = true;
            this.colCOLOR_NAME.VisibleIndex = 11;
            this.colCOLOR_NAME.Width = 99;
            // 
            // gridColumn30
            // 
            this.gridColumn30.AppearanceHeader.Options.UseTextOptions = true;
            this.gridColumn30.AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.gridColumn30.Caption = "Color Comment";
            this.gridColumn30.FieldName = "COLOR_COMMENT";
            this.gridColumn30.Name = "gridColumn30";
            this.gridColumn30.OptionsColumn.AllowEdit = false;
            this.gridColumn30.Visible = true;
            this.gridColumn30.VisibleIndex = 12;
            this.gridColumn30.Width = 220;
            // 
            // TransferRecord
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 14F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1184, 682);
            this.Controls.Add(this.panelControl3);
            this.Controls.Add(this.panelControl2);
            this.Controls.Add(this.panelControl1);
            this.Name = "TransferRecord";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "TransferRecord";
            this.TopMost = true;
            ((System.ComponentModel.ISupportInitialize)(this.panelControl1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.panelControl2)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.panelControl3)).EndInit();
            this.panelControl3.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.groupControl1)).EndInit();
            this.groupControl1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.grdData)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.gvwData)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private DevExpress.XtraEditors.PanelControl panelControl1;
        private DevExpress.XtraEditors.PanelControl panelControl2;
        private DevExpress.XtraEditors.PanelControl panelControl3;
        private DevExpress.XtraEditors.GroupControl groupControl1;
        private DevExpress.XtraGrid.GridControl grdData;
        private DevExpress.XtraGrid.Views.Grid.GridView gvwData;
        private DevExpress.XtraGrid.Columns.GridColumn gridColumn1;
        private DevExpress.XtraGrid.Columns.GridColumn colFACTORY;
        private DevExpress.XtraGrid.Columns.GridColumn colWS_NO;
        private DevExpress.XtraGrid.Columns.GridColumn colPART_SEQ;
        private DevExpress.XtraGrid.Columns.GridColumn colPART_NAME;
        private DevExpress.XtraGrid.Columns.GridColumn colMXSXL_NUMBER;
        private DevExpress.XtraGrid.Columns.GridColumn colMCS_NUMBER;
        private DevExpress.XtraGrid.Columns.GridColumn colPCX_SUPP_MAT_ID;
        private DevExpress.XtraGrid.Columns.GridColumn colPCX_MAT_ID;
        private DevExpress.XtraGrid.Columns.GridColumn colMAT_NAME;
        private DevExpress.XtraGrid.Columns.GridColumn colMAT_COMMENT;
        private DevExpress.XtraGrid.Columns.GridColumn colCOLOR_CD;
        private DevExpress.XtraGrid.Columns.GridColumn colCOLOR_NAME;
        private DevExpress.XtraGrid.Columns.GridColumn gridColumn30;
        private DevExpress.XtraGrid.Columns.GridColumn colUPD_USER;
        private DevExpress.XtraGrid.Columns.GridColumn colUPD_YMD;
        private DevExpress.XtraGrid.Columns.GridColumn gridColumn2;
    }
}