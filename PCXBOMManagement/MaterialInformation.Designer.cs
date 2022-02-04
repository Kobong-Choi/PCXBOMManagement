namespace CSI.PCC.PCX
{
    partial class MaterialInformation
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
            this.grdBase = new DevExpress.XtraGrid.GridControl();
            this.gvwBase = new DevExpress.XtraGrid.Views.Grid.GridView();
            this.colMAT_NAME = new DevExpress.XtraGrid.Columns.GridColumn();
            this.colMAT_COMMENT = new DevExpress.XtraGrid.Columns.GridColumn();
            this.colMXSXL_NUMBER = new DevExpress.XtraGrid.Columns.GridColumn();
            this.colMCS_NUMBER = new DevExpress.XtraGrid.Columns.GridColumn();
            this.colVENDOR_NAME = new DevExpress.XtraGrid.Columns.GridColumn();
            this.colVEN_COUNTRY = new DevExpress.XtraGrid.Columns.GridColumn();
            this.colMCS_STATUS = new DevExpress.XtraGrid.Columns.GridColumn();
            this.colUOM = new DevExpress.XtraGrid.Columns.GridColumn();
            this.colWIDTH = new DevExpress.XtraGrid.Columns.GridColumn();
            this.colPRICE = new DevExpress.XtraGrid.Columns.GridColumn();
            this.colCURRENCY = new DevExpress.XtraGrid.Columns.GridColumn();
            this.gridColumnEx1 = new DevExpress.XtraGrid.Columns.GridColumn();
            this.gridColumnEx2 = new DevExpress.XtraGrid.Columns.GridColumn();
            this.gridColumnEx3 = new DevExpress.XtraGrid.Columns.GridColumn();
            this.colSAMPLE_LT = new DevExpress.XtraGrid.Columns.GridColumn();
            this.colPROD_LT = new DevExpress.XtraGrid.Columns.GridColumn();
            this.colMSI_POINT = new DevExpress.XtraGrid.Columns.GridColumn();
            this.colUSAGE_STATUS = new DevExpress.XtraGrid.Columns.GridColumn();
            this.colRISK_CLASS = new DevExpress.XtraGrid.Columns.GridColumn();
            this.gridColumnEx4 = new DevExpress.XtraGrid.Columns.GridColumn();
            this.colPROBLEM_SHARING = new DevExpress.XtraGrid.Columns.GridColumn();
            this.colGUIDE_LINE = new DevExpress.XtraGrid.Columns.GridColumn();
            ((System.ComponentModel.ISupportInitialize)(this.grdBase)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.gvwBase)).BeginInit();
            this.SuspendLayout();
            // 
            // grdBase
            // 
            this.grdBase.Dock = System.Windows.Forms.DockStyle.Fill;
            this.grdBase.Location = new System.Drawing.Point(0, 0);
            this.grdBase.MainView = this.gvwBase;
            this.grdBase.Name = "grdBase";
            this.grdBase.Size = new System.Drawing.Size(817, 202);
            this.grdBase.TabIndex = 0;
            this.grdBase.ViewCollection.AddRange(new DevExpress.XtraGrid.Views.Base.BaseView[] {
            this.gvwBase});
            // 
            // gvwBase
            // 
            this.gvwBase.Columns.AddRange(new DevExpress.XtraGrid.Columns.GridColumn[] {
            this.colMAT_NAME,
            this.colMAT_COMMENT,
            this.colMXSXL_NUMBER,
            this.colMCS_NUMBER,
            this.colVENDOR_NAME,
            this.colVEN_COUNTRY,
            this.colMCS_STATUS,
            this.colUOM,
            this.colWIDTH,
            this.colPRICE,
            this.colCURRENCY,
            this.gridColumnEx1,
            this.gridColumnEx2,
            this.gridColumnEx3,
            this.colSAMPLE_LT,
            this.colPROD_LT,
            this.colMSI_POINT,
            this.colUSAGE_STATUS,
            this.colRISK_CLASS,
            this.gridColumnEx4,
            this.colPROBLEM_SHARING,
            this.colGUIDE_LINE});
            this.gvwBase.GridControl = this.grdBase;
            this.gvwBase.Name = "gvwBase";
            this.gvwBase.OptionsView.ColumnAutoWidth = false;
            this.gvwBase.OptionsView.ShowGroupPanel = false;
            this.gvwBase.DoubleClick += new System.EventHandler(this.gvwBase_DoubleClick);
            // 
            // colMAT_NAME
            // 
            this.colMAT_NAME.AppearanceCell.Options.UseTextOptions = true;
            this.colMAT_NAME.AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Near;
            this.colMAT_NAME.AppearanceHeader.Options.UseTextOptions = true;
            this.colMAT_NAME.AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.colMAT_NAME.Caption = "Material Name";
            this.colMAT_NAME.FieldName = "MAT_NAME";
            this.colMAT_NAME.Name = "colMAT_NAME";
            this.colMAT_NAME.OptionsColumn.AllowEdit = false;
            this.colMAT_NAME.Visible = true;
            this.colMAT_NAME.VisibleIndex = 0;
            this.colMAT_NAME.Width = 200;
            // 
            // colMAT_COMMENT
            // 
            this.colMAT_COMMENT.AppearanceCell.Options.UseTextOptions = true;
            this.colMAT_COMMENT.AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Near;
            this.colMAT_COMMENT.AppearanceHeader.Options.UseTextOptions = true;
            this.colMAT_COMMENT.AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.colMAT_COMMENT.Caption = "Comment";
            this.colMAT_COMMENT.FieldName = "MAT_COMMENT";
            this.colMAT_COMMENT.Name = "colMAT_COMMENT";
            this.colMAT_COMMENT.OptionsColumn.AllowEdit = false;
            this.colMAT_COMMENT.Width = 100;
            // 
            // colMXSXL_NUMBER
            // 
            this.colMXSXL_NUMBER.AppearanceCell.Options.UseTextOptions = true;
            this.colMXSXL_NUMBER.AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.colMXSXL_NUMBER.AppearanceHeader.Options.UseTextOptions = true;
            this.colMXSXL_NUMBER.AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.colMXSXL_NUMBER.Caption = "MxSxL #";
            this.colMXSXL_NUMBER.FieldName = "MXSXL_NUMBER";
            this.colMXSXL_NUMBER.Name = "colMXSXL_NUMBER";
            this.colMXSXL_NUMBER.OptionsColumn.AllowEdit = false;
            this.colMXSXL_NUMBER.Visible = true;
            this.colMXSXL_NUMBER.VisibleIndex = 1;
            this.colMXSXL_NUMBER.Width = 80;
            // 
            // colMCS_NUMBER
            // 
            this.colMCS_NUMBER.AppearanceCell.Options.UseTextOptions = true;
            this.colMCS_NUMBER.AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Near;
            this.colMCS_NUMBER.AppearanceHeader.Options.UseTextOptions = true;
            this.colMCS_NUMBER.AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.colMCS_NUMBER.Caption = "MCS#";
            this.colMCS_NUMBER.FieldName = "MCS_NUMBER";
            this.colMCS_NUMBER.Name = "colMCS_NUMBER";
            this.colMCS_NUMBER.OptionsColumn.AllowEdit = false;
            this.colMCS_NUMBER.Width = 100;
            // 
            // colVENDOR_NAME
            // 
            this.colVENDOR_NAME.AppearanceCell.Options.UseTextOptions = true;
            this.colVENDOR_NAME.AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Near;
            this.colVENDOR_NAME.AppearanceHeader.Options.UseTextOptions = true;
            this.colVENDOR_NAME.AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.colVENDOR_NAME.Caption = "Vendor";
            this.colVENDOR_NAME.FieldName = "VENDOR_NAME";
            this.colVENDOR_NAME.Name = "colVENDOR_NAME";
            this.colVENDOR_NAME.OptionsColumn.AllowEdit = false;
            this.colVENDOR_NAME.Visible = true;
            this.colVENDOR_NAME.VisibleIndex = 2;
            this.colVENDOR_NAME.Width = 200;
            // 
            // colVEN_COUNTRY
            // 
            this.colVEN_COUNTRY.AppearanceCell.Options.UseTextOptions = true;
            this.colVEN_COUNTRY.AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Near;
            this.colVEN_COUNTRY.AppearanceHeader.Options.UseTextOptions = true;
            this.colVEN_COUNTRY.AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.colVEN_COUNTRY.Caption = "Country";
            this.colVEN_COUNTRY.FieldName = "VEN_COUNTRY";
            this.colVEN_COUNTRY.Name = "colVEN_COUNTRY";
            this.colVEN_COUNTRY.OptionsColumn.AllowEdit = false;
            this.colVEN_COUNTRY.Visible = true;
            this.colVEN_COUNTRY.VisibleIndex = 3;
            this.colVEN_COUNTRY.Width = 100;
            // 
            // colMCS_STATUS
            // 
            this.colMCS_STATUS.AppearanceCell.Options.UseTextOptions = true;
            this.colMCS_STATUS.AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Near;
            this.colMCS_STATUS.AppearanceHeader.Options.UseTextOptions = true;
            this.colMCS_STATUS.AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.colMCS_STATUS.Caption = "Status";
            this.colMCS_STATUS.FieldName = "MCS_STATUS";
            this.colMCS_STATUS.Name = "colMCS_STATUS";
            this.colMCS_STATUS.OptionsColumn.AllowEdit = false;
            this.colMCS_STATUS.Width = 100;
            // 
            // colUOM
            // 
            this.colUOM.AppearanceCell.Options.UseTextOptions = true;
            this.colUOM.AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Near;
            this.colUOM.AppearanceHeader.Options.UseTextOptions = true;
            this.colUOM.AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.colUOM.Caption = "Uom";
            this.colUOM.FieldName = "UOM";
            this.colUOM.Name = "colUOM";
            this.colUOM.OptionsColumn.AllowEdit = false;
            this.colUOM.Width = 100;
            // 
            // colWIDTH
            // 
            this.colWIDTH.AppearanceCell.Options.UseTextOptions = true;
            this.colWIDTH.AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Near;
            this.colWIDTH.AppearanceHeader.Options.UseTextOptions = true;
            this.colWIDTH.AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.colWIDTH.Caption = "Width";
            this.colWIDTH.FieldName = "WIDTH";
            this.colWIDTH.Name = "colWIDTH";
            this.colWIDTH.OptionsColumn.AllowEdit = false;
            this.colWIDTH.Width = 100;
            // 
            // colPRICE
            // 
            this.colPRICE.AppearanceCell.Options.UseTextOptions = true;
            this.colPRICE.AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Near;
            this.colPRICE.AppearanceHeader.Options.UseTextOptions = true;
            this.colPRICE.AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.colPRICE.Caption = "Price";
            this.colPRICE.FieldName = "PRICE";
            this.colPRICE.Name = "colPRICE";
            this.colPRICE.OptionsColumn.AllowEdit = false;
            this.colPRICE.Width = 100;
            // 
            // colCURRENCY
            // 
            this.colCURRENCY.AppearanceCell.Options.UseTextOptions = true;
            this.colCURRENCY.AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Near;
            this.colCURRENCY.AppearanceHeader.Options.UseTextOptions = true;
            this.colCURRENCY.AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.colCURRENCY.Caption = "Currency";
            this.colCURRENCY.FieldName = "CURRENCY";
            this.colCURRENCY.Name = "colCURRENCY";
            this.colCURRENCY.OptionsColumn.AllowEdit = false;
            this.colCURRENCY.Width = 100;
            // 
            // gridColumnEx1
            // 
            this.gridColumnEx1.AppearanceCell.Options.UseTextOptions = true;
            this.gridColumnEx1.AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Near;
            this.gridColumnEx1.AppearanceHeader.Options.UseTextOptions = true;
            this.gridColumnEx1.AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.gridColumnEx1.Caption = "\'\'";
            this.gridColumnEx1.FieldName = "\'\'";
            this.gridColumnEx1.Name = "gridColumnEx1";
            this.gridColumnEx1.OptionsColumn.AllowEdit = false;
            this.gridColumnEx1.Width = 100;
            // 
            // gridColumnEx2
            // 
            this.gridColumnEx2.AppearanceCell.Options.UseTextOptions = true;
            this.gridColumnEx2.AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Near;
            this.gridColumnEx2.AppearanceHeader.Options.UseTextOptions = true;
            this.gridColumnEx2.AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.gridColumnEx2.Caption = "\'\'1";
            this.gridColumnEx2.FieldName = "\'\'1";
            this.gridColumnEx2.Name = "gridColumnEx2";
            this.gridColumnEx2.OptionsColumn.AllowEdit = false;
            this.gridColumnEx2.Width = 100;
            // 
            // gridColumnEx3
            // 
            this.gridColumnEx3.AppearanceCell.Options.UseTextOptions = true;
            this.gridColumnEx3.AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Near;
            this.gridColumnEx3.AppearanceHeader.Options.UseTextOptions = true;
            this.gridColumnEx3.AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.gridColumnEx3.Caption = "\'\'2";
            this.gridColumnEx3.FieldName = "\'\'2";
            this.gridColumnEx3.Name = "gridColumnEx3";
            this.gridColumnEx3.OptionsColumn.AllowEdit = false;
            this.gridColumnEx3.Width = 100;
            // 
            // colSAMPLE_LT
            // 
            this.colSAMPLE_LT.AppearanceCell.Options.UseTextOptions = true;
            this.colSAMPLE_LT.AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Near;
            this.colSAMPLE_LT.AppearanceHeader.Options.UseTextOptions = true;
            this.colSAMPLE_LT.AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.colSAMPLE_LT.Caption = "SampleLt";
            this.colSAMPLE_LT.FieldName = "SAMPLE_LT";
            this.colSAMPLE_LT.Name = "colSAMPLE_LT";
            this.colSAMPLE_LT.OptionsColumn.AllowEdit = false;
            this.colSAMPLE_LT.Width = 100;
            // 
            // colPROD_LT
            // 
            this.colPROD_LT.AppearanceCell.Options.UseTextOptions = true;
            this.colPROD_LT.AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Near;
            this.colPROD_LT.AppearanceHeader.Options.UseTextOptions = true;
            this.colPROD_LT.AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.colPROD_LT.Caption = "ProductionLt";
            this.colPROD_LT.FieldName = "PROD_LT";
            this.colPROD_LT.Name = "colPROD_LT";
            this.colPROD_LT.OptionsColumn.AllowEdit = false;
            this.colPROD_LT.Width = 100;
            // 
            // colMSI_POINT
            // 
            this.colMSI_POINT.AppearanceCell.Options.UseTextOptions = true;
            this.colMSI_POINT.AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Near;
            this.colMSI_POINT.AppearanceHeader.Options.UseTextOptions = true;
            this.colMSI_POINT.AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.colMSI_POINT.Caption = "MsiPoint";
            this.colMSI_POINT.FieldName = "MSI_POINT";
            this.colMSI_POINT.Name = "colMSI_POINT";
            this.colMSI_POINT.OptionsColumn.AllowEdit = false;
            this.colMSI_POINT.Width = 100;
            // 
            // colUSAGE_STATUS
            // 
            this.colUSAGE_STATUS.AppearanceCell.Options.UseTextOptions = true;
            this.colUSAGE_STATUS.AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Near;
            this.colUSAGE_STATUS.AppearanceHeader.Options.UseTextOptions = true;
            this.colUSAGE_STATUS.AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.colUSAGE_STATUS.Caption = "UsageStatus";
            this.colUSAGE_STATUS.FieldName = "USAGE_STATUS";
            this.colUSAGE_STATUS.Name = "colUSAGE_STATUS";
            this.colUSAGE_STATUS.OptionsColumn.AllowEdit = false;
            this.colUSAGE_STATUS.Width = 100;
            // 
            // colRISK_CLASS
            // 
            this.colRISK_CLASS.AppearanceCell.Options.UseTextOptions = true;
            this.colRISK_CLASS.AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.colRISK_CLASS.AppearanceHeader.Options.UseTextOptions = true;
            this.colRISK_CLASS.AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.colRISK_CLASS.Caption = "Risk";
            this.colRISK_CLASS.FieldName = "RISK_CLASS";
            this.colRISK_CLASS.Name = "colRISK_CLASS";
            this.colRISK_CLASS.OptionsColumn.AllowEdit = false;
            this.colRISK_CLASS.Visible = true;
            this.colRISK_CLASS.VisibleIndex = 4;
            this.colRISK_CLASS.Width = 60;
            // 
            // gridColumnEx4
            // 
            this.gridColumnEx4.AppearanceCell.Options.UseTextOptions = true;
            this.gridColumnEx4.AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Near;
            this.gridColumnEx4.AppearanceHeader.Options.UseTextOptions = true;
            this.gridColumnEx4.AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.gridColumnEx4.Caption = "Special Comment";
            this.gridColumnEx4.FieldName = "\'\'3";
            this.gridColumnEx4.Name = "gridColumnEx4";
            this.gridColumnEx4.OptionsColumn.AllowEdit = false;
            this.gridColumnEx4.Visible = true;
            this.gridColumnEx4.VisibleIndex = 5;
            this.gridColumnEx4.Width = 103;
            // 
            // colPROBLEM_SHARING
            // 
            this.colPROBLEM_SHARING.AppearanceCell.Options.UseTextOptions = true;
            this.colPROBLEM_SHARING.AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.colPROBLEM_SHARING.AppearanceHeader.Options.UseTextOptions = true;
            this.colPROBLEM_SHARING.AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.colPROBLEM_SHARING.Caption = "Problem";
            this.colPROBLEM_SHARING.FieldName = "PROBLEM_SHARING";
            this.colPROBLEM_SHARING.Name = "colPROBLEM_SHARING";
            this.colPROBLEM_SHARING.OptionsColumn.AllowEdit = false;
            this.colPROBLEM_SHARING.Width = 60;
            // 
            // colGUIDE_LINE
            // 
            this.colGUIDE_LINE.AppearanceCell.Options.UseTextOptions = true;
            this.colGUIDE_LINE.AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.colGUIDE_LINE.AppearanceHeader.Options.UseTextOptions = true;
            this.colGUIDE_LINE.AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.colGUIDE_LINE.Caption = "Guideline";
            this.colGUIDE_LINE.FieldName = "GUIDE_LINE";
            this.colGUIDE_LINE.Name = "colGUIDE_LINE";
            this.colGUIDE_LINE.OptionsColumn.AllowEdit = false;
            this.colGUIDE_LINE.Width = 60;
            // 
            // MaterialInformation
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 14F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(817, 202);
            this.Controls.Add(this.grdBase);
            this.Name = "MaterialInformation";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "MaterialInformation";
            this.TopMost = true;
            ((System.ComponentModel.ISupportInitialize)(this.grdBase)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.gvwBase)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private DevExpress.XtraGrid.GridControl grdBase;
        private DevExpress.XtraGrid.Views.Grid.GridView gvwBase;
        private DevExpress.XtraGrid.Columns.GridColumn colMAT_NAME;
        private DevExpress.XtraGrid.Columns.GridColumn colMAT_COMMENT;
        private DevExpress.XtraGrid.Columns.GridColumn colMXSXL_NUMBER;
        private DevExpress.XtraGrid.Columns.GridColumn colMCS_NUMBER;
        private DevExpress.XtraGrid.Columns.GridColumn colVENDOR_NAME;
        private DevExpress.XtraGrid.Columns.GridColumn colVEN_COUNTRY;
        private DevExpress.XtraGrid.Columns.GridColumn colMCS_STATUS;
        private DevExpress.XtraGrid.Columns.GridColumn colUOM;
        private DevExpress.XtraGrid.Columns.GridColumn colWIDTH;
        private DevExpress.XtraGrid.Columns.GridColumn colPRICE;
        private DevExpress.XtraGrid.Columns.GridColumn colCURRENCY;
        private DevExpress.XtraGrid.Columns.GridColumn gridColumnEx1;
        private DevExpress.XtraGrid.Columns.GridColumn gridColumnEx2;
        private DevExpress.XtraGrid.Columns.GridColumn gridColumnEx3;
        private DevExpress.XtraGrid.Columns.GridColumn colSAMPLE_LT;
        private DevExpress.XtraGrid.Columns.GridColumn colPROD_LT;
        private DevExpress.XtraGrid.Columns.GridColumn colMSI_POINT;
        private DevExpress.XtraGrid.Columns.GridColumn colUSAGE_STATUS;
        private DevExpress.XtraGrid.Columns.GridColumn colRISK_CLASS;
        private DevExpress.XtraGrid.Columns.GridColumn gridColumnEx4;
        private DevExpress.XtraGrid.Columns.GridColumn colPROBLEM_SHARING;
        private DevExpress.XtraGrid.Columns.GridColumn colGUIDE_LINE;
    }
}