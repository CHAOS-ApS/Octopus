using System.ComponentModel;
using System.Configuration.Install;


namespace Geckon.Octopus.Controller.Core.WindowsService
{
    [RunInstaller(true)]
    public partial class ProjectInstaller : Installer
    {
        public ProjectInstaller()
        {
            InitializeComponent();


        }
    }
}
