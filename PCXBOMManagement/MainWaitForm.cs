using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

using DevExpress.XtraWaitForm;

namespace CSI.PCC.PCX
{
    public partial class MainWaitForm : WaitForm
    {
        public MainWaitForm()
        {
            InitializeComponent();
        }

        public override void ProcessCommand(Enum cmd, object arg)
        {
            base.ProcessCommand(cmd, arg);

            SplashScreenCommand command = (SplashScreenCommand)cmd;

            if (command == SplashScreenCommand.SendEmail)
                progressPanel1.Description = "Sending Email...";
            else if (command == SplashScreenCommand.TransferToNCF)
                progressPanel1.Description = "Transferring Data...";
            else if (command == SplashScreenCommand.ConvertToOCF)
                progressPanel1.Description = "Converting to OCF...";
            else if (command == SplashScreenCommand.UpdateStatus)
                progressPanel1.Description = "Update BOM status...";
        }

        public enum SplashScreenCommand
        {
            SendEmail,
            TransferToNCF,
            ConvertToOCF,
            UpdateStatus
        }
    }
}
