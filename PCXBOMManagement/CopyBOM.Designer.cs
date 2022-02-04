namespace CSI.PCC.PCX
{
    partial class CopyBOM
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(CopyBOM));
            DevExpress.Utils.SuperToolTip superToolTip2 = new DevExpress.Utils.SuperToolTip();
            DevExpress.Utils.ToolTipTitleItem toolTipTitleItem2 = new DevExpress.Utils.ToolTipTitleItem();
            DevExpress.Utils.ToolTipItem toolTipItem2 = new DevExpress.Utils.ToolTipItem();
            this.panelControl1 = new DevExpress.XtraEditors.PanelControl();
            this.rgTypeSelection = new DevExpress.XtraEditors.RadioGroup();
            this.btnSave = new DevExpress.XtraEditors.SimpleButton();
            this.panelControl2 = new DevExpress.XtraEditors.PanelControl();
            this.labelControl5 = new DevExpress.XtraEditors.LabelControl();
            this.leSeason = new DevExpress.XtraEditors.LookUpEdit();
            this.labelControl3 = new DevExpress.XtraEditors.LabelControl();
            this.leSampleType = new DevExpress.XtraEditors.LookUpEdit();
            this.txtSubTypeRemark = new DevExpress.XtraEditors.TextEdit();
            this.labelControl2 = new DevExpress.XtraEditors.LabelControl();
            this.labelControl1 = new DevExpress.XtraEditors.LabelControl();
            this.leSubType = new DevExpress.XtraEditors.LookUpEdit();
            this.labelControl4 = new DevExpress.XtraEditors.LabelControl();
            this.leFactory = new DevExpress.XtraEditors.LookUpEdit();
            ((System.ComponentModel.ISupportInitialize)(this.panelControl1)).BeginInit();
            this.panelControl1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.rgTypeSelection.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.panelControl2)).BeginInit();
            this.panelControl2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.leSeason.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.leSampleType.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtSubTypeRemark.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.leSubType.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.leFactory.Properties)).BeginInit();
            this.SuspendLayout();
            // 
            // panelControl1
            // 
            this.panelControl1.Controls.Add(this.rgTypeSelection);
            this.panelControl1.Controls.Add(this.btnSave);
            this.panelControl1.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panelControl1.Location = new System.Drawing.Point(0, 161);
            this.panelControl1.Name = "panelControl1";
            this.panelControl1.Size = new System.Drawing.Size(316, 47);
            this.panelControl1.TabIndex = 0;
            // 
            // rgTypeSelection
            // 
            this.rgTypeSelection.EditValue = "N";
            this.rgTypeSelection.Location = new System.Drawing.Point(11, 5);
            this.rgTypeSelection.Name = "rgTypeSelection";
            this.rgTypeSelection.Properties.BorderStyle = DevExpress.XtraEditors.Controls.BorderStyles.Simple;
            this.rgTypeSelection.Properties.Items.AddRange(new DevExpress.XtraEditors.Controls.RadioGroupItem[] {
            new DevExpress.XtraEditors.Controls.RadioGroupItem("N", "CS BOM"),
            new DevExpress.XtraEditors.Controls.RadioGroupItem("F", "Fake BOM")});
            this.rgTypeSelection.Size = new System.Drawing.Size(160, 36);
            this.rgTypeSelection.TabIndex = 1;
            // 
            // btnSave
            // 
            this.btnSave.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnSave.Image = ((System.Drawing.Image)(resources.GetObject("btnSave.Image")));
            this.btnSave.Location = new System.Drawing.Point(229, 6);
            this.btnSave.Name = "btnSave";
            this.btnSave.Size = new System.Drawing.Size(75, 35);
            toolTipTitleItem2.Text = "Criteria of Unique";
            toolTipItem2.LeftIndent = 6;
            toolTipItem2.Text = "Season + Sample Type + Sub Type + Sub. Remark";
            superToolTip2.Items.Add(toolTipTitleItem2);
            superToolTip2.Items.Add(toolTipItem2);
            this.btnSave.SuperTip = superToolTip2;
            this.btnSave.TabIndex = 11;
            this.btnSave.Text = "Save";
            this.btnSave.Click += new System.EventHandler(this.btnSave_Click);
            // 
            // panelControl2
            // 
            this.panelControl2.Controls.Add(this.leFactory);
            this.panelControl2.Controls.Add(this.labelControl4);
            this.panelControl2.Controls.Add(this.labelControl5);
            this.panelControl2.Controls.Add(this.leSeason);
            this.panelControl2.Controls.Add(this.labelControl3);
            this.panelControl2.Controls.Add(this.leSampleType);
            this.panelControl2.Controls.Add(this.txtSubTypeRemark);
            this.panelControl2.Controls.Add(this.labelControl2);
            this.panelControl2.Controls.Add(this.labelControl1);
            this.panelControl2.Controls.Add(this.leSubType);
            this.panelControl2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelControl2.Location = new System.Drawing.Point(0, 0);
            this.panelControl2.Name = "panelControl2";
            this.panelControl2.Size = new System.Drawing.Size(316, 161);
            this.panelControl2.TabIndex = 1;
            // 
            // labelControl5
            // 
            this.labelControl5.Appearance.BackColor = System.Drawing.Color.Coral;
            this.labelControl5.Appearance.Font = new System.Drawing.Font("Tahoma", 10F, System.Drawing.FontStyle.Bold);
            this.labelControl5.Appearance.ForeColor = System.Drawing.Color.White;
            this.labelControl5.AutoSizeMode = DevExpress.XtraEditors.LabelAutoSizeMode.None;
            this.labelControl5.Location = new System.Drawing.Point(11, 12);
            this.labelControl5.Name = "labelControl5";
            this.labelControl5.Padding = new System.Windows.Forms.Padding(5, 0, 0, 0);
            this.labelControl5.Size = new System.Drawing.Size(98, 23);
            this.labelControl5.TabIndex = 1;
            this.labelControl5.Text = "Season";
            // 
            // leSeason
            // 
            this.leSeason.Location = new System.Drawing.Point(115, 12);
            this.leSeason.Name = "leSeason";
            this.leSeason.Properties.AutoHeight = false;
            this.leSeason.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.leSeason.Properties.CloseUpKey = new DevExpress.Utils.KeyShortcut((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.W));
            this.leSeason.Properties.Columns.AddRange(new DevExpress.XtraEditors.Controls.LookUpColumnInfo[] {
            new DevExpress.XtraEditors.Controls.LookUpColumnInfo("CODE", "Code", 20, DevExpress.Utils.FormatType.None, "", false, DevExpress.Utils.HorzAlignment.Default),
            new DevExpress.XtraEditors.Controls.LookUpColumnInfo("NAME", "Name")});
            this.leSeason.Properties.DisplayMember = "NAME";
            this.leSeason.Properties.NullText = "";
            this.leSeason.Properties.ShowFooter = false;
            this.leSeason.Properties.ShowHeader = false;
            this.leSeason.Properties.ValueMember = "CODE";
            this.leSeason.Size = new System.Drawing.Size(189, 23);
            this.leSeason.TabIndex = 6;
            // 
            // labelControl3
            // 
            this.labelControl3.Appearance.BackColor = System.Drawing.Color.Coral;
            this.labelControl3.Appearance.Font = new System.Drawing.Font("Tahoma", 10F, System.Drawing.FontStyle.Bold);
            this.labelControl3.Appearance.ForeColor = System.Drawing.Color.White;
            this.labelControl3.AutoSizeMode = DevExpress.XtraEditors.LabelAutoSizeMode.None;
            this.labelControl3.Location = new System.Drawing.Point(11, 41);
            this.labelControl3.Name = "labelControl3";
            this.labelControl3.Padding = new System.Windows.Forms.Padding(5, 0, 0, 0);
            this.labelControl3.Size = new System.Drawing.Size(98, 23);
            this.labelControl3.TabIndex = 2;
            this.labelControl3.Text = "Sample Type";
            // 
            // leSampleType
            // 
            this.leSampleType.Location = new System.Drawing.Point(115, 41);
            this.leSampleType.Name = "leSampleType";
            this.leSampleType.Properties.AutoHeight = false;
            this.leSampleType.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.leSampleType.Properties.CloseUpKey = new DevExpress.Utils.KeyShortcut((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.W));
            this.leSampleType.Properties.Columns.AddRange(new DevExpress.XtraEditors.Controls.LookUpColumnInfo[] {
            new DevExpress.XtraEditors.Controls.LookUpColumnInfo("CODE", "Code", 20, DevExpress.Utils.FormatType.None, "", false, DevExpress.Utils.HorzAlignment.Default),
            new DevExpress.XtraEditors.Controls.LookUpColumnInfo("NAME", "Name")});
            this.leSampleType.Properties.DisplayMember = "NAME";
            this.leSampleType.Properties.NullText = "";
            this.leSampleType.Properties.ShowFooter = false;
            this.leSampleType.Properties.ShowHeader = false;
            this.leSampleType.Properties.ValueMember = "CODE";
            this.leSampleType.Size = new System.Drawing.Size(189, 23);
            this.leSampleType.TabIndex = 7;
            // 
            // txtSubTypeRemark
            // 
            this.txtSubTypeRemark.Location = new System.Drawing.Point(115, 99);
            this.txtSubTypeRemark.Name = "txtSubTypeRemark";
            this.txtSubTypeRemark.Properties.AutoHeight = false;
            this.txtSubTypeRemark.Properties.CharacterCasing = System.Windows.Forms.CharacterCasing.Upper;
            this.txtSubTypeRemark.Size = new System.Drawing.Size(189, 23);
            this.txtSubTypeRemark.TabIndex = 9;
            // 
            // labelControl2
            // 
            this.labelControl2.Appearance.BackColor = System.Drawing.Color.Coral;
            this.labelControl2.Appearance.Font = new System.Drawing.Font("Tahoma", 10F, System.Drawing.FontStyle.Bold);
            this.labelControl2.Appearance.ForeColor = System.Drawing.Color.White;
            this.labelControl2.AutoSizeMode = DevExpress.XtraEditors.LabelAutoSizeMode.None;
            this.labelControl2.Location = new System.Drawing.Point(11, 99);
            this.labelControl2.Name = "labelControl2";
            this.labelControl2.Padding = new System.Windows.Forms.Padding(5, 0, 0, 0);
            this.labelControl2.Size = new System.Drawing.Size(98, 23);
            this.labelControl2.TabIndex = 4;
            this.labelControl2.Text = "Sub. Remark";
            // 
            // labelControl1
            // 
            this.labelControl1.Appearance.BackColor = System.Drawing.Color.Coral;
            this.labelControl1.Appearance.Font = new System.Drawing.Font("Tahoma", 10F, System.Drawing.FontStyle.Bold);
            this.labelControl1.Appearance.ForeColor = System.Drawing.Color.White;
            this.labelControl1.AutoSizeMode = DevExpress.XtraEditors.LabelAutoSizeMode.None;
            this.labelControl1.Location = new System.Drawing.Point(11, 70);
            this.labelControl1.Name = "labelControl1";
            this.labelControl1.Padding = new System.Windows.Forms.Padding(5, 0, 0, 0);
            this.labelControl1.Size = new System.Drawing.Size(98, 23);
            this.labelControl1.TabIndex = 3;
            this.labelControl1.Text = "Sub. Type";
            // 
            // leSubType
            // 
            this.leSubType.Location = new System.Drawing.Point(115, 70);
            this.leSubType.Name = "leSubType";
            this.leSubType.Properties.AutoHeight = false;
            this.leSubType.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.leSubType.Properties.CloseUpKey = new DevExpress.Utils.KeyShortcut((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.W));
            this.leSubType.Properties.Columns.AddRange(new DevExpress.XtraEditors.Controls.LookUpColumnInfo[] {
            new DevExpress.XtraEditors.Controls.LookUpColumnInfo("CODE", "Code", 20, DevExpress.Utils.FormatType.None, "", false, DevExpress.Utils.HorzAlignment.Default),
            new DevExpress.XtraEditors.Controls.LookUpColumnInfo("NAME", "Name")});
            this.leSubType.Properties.DisplayMember = "NAME";
            this.leSubType.Properties.NullText = "";
            this.leSubType.Properties.ShowFooter = false;
            this.leSubType.Properties.ShowHeader = false;
            this.leSubType.Properties.ValueMember = "CODE";
            this.leSubType.Size = new System.Drawing.Size(189, 23);
            this.leSubType.TabIndex = 8;
            // 
            // labelControl4
            // 
            this.labelControl4.Appearance.BackColor = System.Drawing.Color.Coral;
            this.labelControl4.Appearance.Font = new System.Drawing.Font("Tahoma", 10F, System.Drawing.FontStyle.Bold);
            this.labelControl4.Appearance.ForeColor = System.Drawing.Color.White;
            this.labelControl4.AutoSizeMode = DevExpress.XtraEditors.LabelAutoSizeMode.None;
            this.labelControl4.Location = new System.Drawing.Point(11, 128);
            this.labelControl4.Name = "labelControl4";
            this.labelControl4.Padding = new System.Windows.Forms.Padding(5, 0, 0, 0);
            this.labelControl4.Size = new System.Drawing.Size(98, 23);
            this.labelControl4.TabIndex = 10;
            this.labelControl4.Text = "Factory";
            // 
            // leFactory
            // 
            this.leFactory.Location = new System.Drawing.Point(115, 128);
            this.leFactory.Name = "leFactory";
            this.leFactory.Properties.AutoHeight = false;
            this.leFactory.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.leFactory.Properties.CloseUpKey = new DevExpress.Utils.KeyShortcut((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.W));
            this.leFactory.Properties.Columns.AddRange(new DevExpress.XtraEditors.Controls.LookUpColumnInfo[] {
            new DevExpress.XtraEditors.Controls.LookUpColumnInfo("CODE", "Code", 20, DevExpress.Utils.FormatType.None, "", false, DevExpress.Utils.HorzAlignment.Default),
            new DevExpress.XtraEditors.Controls.LookUpColumnInfo("NAME", "Name")});
            this.leFactory.Properties.DisplayMember = "NAME";
            this.leFactory.Properties.NullText = "";
            this.leFactory.Properties.ShowFooter = false;
            this.leFactory.Properties.ShowHeader = false;
            this.leFactory.Properties.ValueMember = "CODE";
            this.leFactory.Size = new System.Drawing.Size(189, 23);
            this.leFactory.TabIndex = 11;
            // 
            // CopyBOM
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 14F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(316, 208);
            this.Controls.Add(this.panelControl2);
            this.Controls.Add(this.panelControl1);
            this.Name = "CopyBOM";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "CopyBOM";
            ((System.ComponentModel.ISupportInitialize)(this.panelControl1)).EndInit();
            this.panelControl1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.rgTypeSelection.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.panelControl2)).EndInit();
            this.panelControl2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.leSeason.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.leSampleType.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtSubTypeRemark.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.leSubType.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.leFactory.Properties)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private DevExpress.XtraEditors.PanelControl panelControl1;
        private DevExpress.XtraEditors.SimpleButton btnSave;
        private DevExpress.XtraEditors.PanelControl panelControl2;
        private DevExpress.XtraEditors.TextEdit txtSubTypeRemark;
        private DevExpress.XtraEditors.LabelControl labelControl2;
        private DevExpress.XtraEditors.LabelControl labelControl1;
        private DevExpress.XtraEditors.LookUpEdit leSubType;
        private DevExpress.XtraEditors.LabelControl labelControl3;
        private DevExpress.XtraEditors.LookUpEdit leSampleType;
        private DevExpress.XtraEditors.LabelControl labelControl5;
        private DevExpress.XtraEditors.LookUpEdit leSeason;
        private DevExpress.XtraEditors.RadioGroup rgTypeSelection;
        private DevExpress.XtraEditors.LookUpEdit leFactory;
        private DevExpress.XtraEditors.LabelControl labelControl4;
    }
}