namespace Geckon.Octopus.Agent.Service
{
    partial class ProjectInstaller
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

        #region Component Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.AgetServiceProcessInstaller = new System.ServiceProcess.ServiceProcessInstaller();
            this.AgentServiceInstaller = new System.ServiceProcess.ServiceInstaller();
            // 
            // AgetServiceProcessInstaller
            // 
            this.AgetServiceProcessInstaller.Password = null;
            this.AgetServiceProcessInstaller.Username = null;
            // 
            // AgentServiceInstaller
            // 
            this.AgentServiceInstaller.DisplayName = "Octopus Agent";
            this.AgentServiceInstaller.ServiceName = "AgentWindowsService";
            // 
            // ProjectInstaller
            // 
            this.Installers.AddRange(new System.Configuration.Install.Installer[] {
            this.AgetServiceProcessInstaller,
            this.AgentServiceInstaller});

        }

        #endregion

        private System.ServiceProcess.ServiceProcessInstaller AgetServiceProcessInstaller;
        private System.ServiceProcess.ServiceInstaller AgentServiceInstaller;
    }
}