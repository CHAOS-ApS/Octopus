using System;
using System.ServiceProcess;
using System.Threading;
using Geckon.Logging;
using Geckon.Octopus.Agent.Core;
using Geckon.Octopus.Agent.Core.Test;
using Geckon.Octopus.Agent.Interface;
using Geckon.Octopus.Controller.Interface;
using Geckon.Octopus.Data;
using Geckon.Octopus.Data.Interface;

namespace Geckon.Octopus.Controller.Core.WindowsService
{
    public partial class OctopusControllerService : ServiceBase
    {
        #region Fields

        private IControllerEngine _Controller;

        #endregion

        public OctopusControllerService()
        {
            Logging.Instance.Write("Service Initializing");
            InitializeComponent();
            Logging.Instance.Write("Service Initialized");
        }

        protected override void OnStart( string[] args )
        {
            try
            {
                Logging.Instance.Write("Service OnStart");

                if (_Controller != null)
                    OnStop();

                Logging.Instance.Write("Service OnStart - Initializing Controller");

                _Controller = new ControllerEngine(true);

                Logging.Instance.Write("Service OnStart - Controller Initialized");

                IAgent agent = new AgentEngine();

                Logging.Instance.Write("Service OnStart - Agent");

                IAllocationDefinition definition = new AllocationDefinition(2);

                Logging.Instance.Write("Service OnStart - Definition");

                using( OctopusDataContext db = new OctopusDataContext() )
                {
                    Logging.Instance.Write("Service OnStart - Data context");

                    foreach( IPluginInfo pluginInfo in db.PluginInfo_GetAll() )
                    {
                        Logging.Instance.Write("Service OnStart - Adding plugininfo");

                        definition.Add( pluginInfo );

                        Logging.Instance.Write("Service OnStart - PluginInfo added");
                    }
                }

                agent.AddDefinition(definition);

                Logging.Instance.Write("Service OnStart - Definition added");

                _Controller.Broker.Add(agent);

                Logging.Instance.Write("Service OnStart - Agent Added");
            }
            catch (Exception e)
            {
                Logging.Instance.Write("Octopus: {0}, \nStacktrace: {1}", e.Message, e.StackTrace);

                throw;
            }

            Logging.Instance.Write("Service OnStart - Ended");
        }

        protected override void OnStop()
        {
            if( _Controller == null)
                return;

            _Controller.Dispose();
        }
    }
}
