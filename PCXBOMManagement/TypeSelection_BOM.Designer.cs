namespace CSI.PCC.PCX
{
    partial class TypeSelection_BOM
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(TypeSelection_BOM));
            this.groupControl1 = new DevExpress.XtraEditors.GroupControl();
            this.btnConfirm = new DevExpress.XtraEditors.SimpleButton();
            this.rdTypes = new DevExpress.XtraEditors.RadioGroup();
            ((System.ComponentModel.ISupportInitialize)(this.groupControl1)).BeginInit();
            this.groupControl1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.rdTypes.Properties)).BeginInit();
            this.SuspendLayout();
            // 
            // groupControl1
            // 
            this.groupControl1.Controls.Add(this.btnConfirm);
            this.groupControl1.Controls.Add(this.rdTypes);
            this.groupControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.groupControl1.Location = new System.Drawing.Point(0, 0);
            this.groupControl1.Name = "groupControl1";
            this.groupControl1.Size = new System.Drawing.Size(264, 110);
            this.groupControl1.TabIndex = 0;
            this.groupControl1.Text = "BOM Types";
            // 
            // btnConfirm
            // 
            this.btnConfirm.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.btnConfirm.Appearance.Font = new System.Drawing.Font("Calibri", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnConfirm.Appearance.Options.UseFont = true;
            this.btnConfirm.Image = ((System.Drawing.Image)(resources.GetObject("btnConfirm.Image")));
            this.btnConfirm.Location = new System.Drawing.Point(84, 67);
            this.btnConfirm.Name = "btnConfirm";
            this.btnConfirm.Size = new System.Drawing.Size(89, 33);
            this.btnConfirm.TabIndex = 1;
            this.btnConfirm.Text = "Confirm";
            this.btnConfirm.Click += new System.EventHandler(this.btnConfirm_Click);
            // 
            // rdTypes
            // 
            this.rdTypes.Dock = System.Windows.Forms.DockStyle.Top;
            this.rdTypes.Location = new System.Drawing.Point(2, 22);
            this.rdTypes.Name = "rdTypes";
            this.rdTypes.Properties.Appearance.Font = new System.Drawing.Font("Calibri", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.rdTypes.Properties.Appearance.Options.UseFont = true;
            this.rdTypes.Properties.Appearance.Options.UseTextOptions = true;
            this.rdTypes.Properties.Appearance.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Near;
            this.rdTypes.Properties.BorderStyle = DevExpress.XtraEditors.Controls.BorderStyles.NoBorder;
            this.rdTypes.Properties.Items.AddRange(new DevExpress.XtraEditors.Controls.RadioGroupItem[] {
            new DevExpress.XtraEditors.Controls.RadioGroupItem("XML", "XML"),
            new DevExpress.XtraEditors.Controls.RadioGroupItem("EXCEL", "EXCEL"),
            new DevExpress.XtraEditors.Controls.RadioGroupItem("JSON", "JSON"),
            new DevExpress.XtraEditors.Controls.RadioGroupItem("LOGIC", "LOGIC")});
            this.rdTypes.Size = new System.Drawing.Size(260, 39);
            this.rdTypes.TabIndex = 0;
            // 
            // TypeSelection_BOM
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 14F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(264, 110);
            this.Controls.Add(this.groupControl1);
            this.Name = "TypeSelection_BOM";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "TypeSelection";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.TypeSelection_FormClosing);
            ((System.ComponentModel.ISupportInitialize)(this.groupControl1)).EndInit();
            this.groupControl1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.rdTypes.Properties)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private DevExpress.XtraEditors.GroupControl groupControl1;
        private DevExpress.XtraEditors.RadioGroup rdTypes;
        private DevExpress.XtraEditors.SimpleButton btnConfirm;

    }
}