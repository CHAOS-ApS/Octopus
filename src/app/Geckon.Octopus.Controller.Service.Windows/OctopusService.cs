using System.ServiceProcess;
using Geckon.Octopus.Controller.Core;
using Geckon.Octopus.Controller.Interface;

namespace Geckon.Octopus.Controller.Service.Windows
{
    public partial class OctopusService : ServiceBase
    {
        #region Fields

        private IControllerEngine _Controller;

        #endregion
        #region Properties

        public IControllerEngine Controller
        {
            get
            {
                return _Controller;
            }
            set
            {
                _Controller = value;
            }
        }

        #endregion

        public OctopusService()
        {
            InitializeComponent();
        }

        protected override void OnStart(string[] args)
        {
            if( _Controller != null )
                OnStop();

            Controller = new ControllerEngine( true, 5000 );
        }

        protected override void OnStop()
        {
            if( _Controller == null )
                return;

            _Controller.Dispose();
        }
    }
}