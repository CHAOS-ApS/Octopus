using System.Configuration;
using System.ServiceModel;
using System.ServiceProcess;
using Geckon.Octopus.Agent.Core;
using Geckon.Octopus.Agent.Interface;

namespace Geckon.Octopus.Agent.Service
{
    public partial class AgentWindowsService : ServiceBase
    {
        #region Fields

        private IAgent      _AgentEngine;
        private ServiceHost _ServiceHost;

        #endregion
        #region Properties

        protected IAgent AgentEngine
        {
            get
            {
                return _AgentEngine;
            }
            set { _AgentEngine = value; }
        }

        protected ServiceHost ServiceHost
        {
            get
            {
                return _ServiceHost;
            }
            set { _ServiceHost = value; }
        }

        #endregion
        #region Constructor

        public AgentWindowsService()
        {
            InitializeComponent();
        }

        #endregion
        #region Service Logic

        protected override void OnStart( string[] args )
        {
             if( _ServiceHost != null )
                    _ServiceHost.Close();

            ServiceHost = new ServiceHost( typeof( AgentService ) );
            ServiceHost.Open();

            _AgentEngine = new AgentEngine( int.Parse( ConfigurationManager.AppSettings["SETTINGS_ID"] ) );
        }

        protected override void OnStop()
        {
            if( _ServiceHost == null )
                return;
                
            // Stop accepting new jobs
            ServiceHost.Close();
            ServiceHost = null;

            // Stop running plugins
            // TODO: Handle shutdown of running plugins
            _AgentEngine = null;
        }

        #endregion
    }
}
