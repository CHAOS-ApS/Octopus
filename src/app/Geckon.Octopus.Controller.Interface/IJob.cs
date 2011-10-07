using System.Collections.Generic;
using Geckon.Events;
using Geckon.Octopus.Plugin.Interface;

namespace Geckon.Octopus.Controller.Interface
{
    public interface IJob : IJobData, IFlow
    {
        event EventHandlers.ObjectEventHandler<IJob> JobCommitted;
		event EventHandlers.ObjectErrorEventHandler<IPlugin> JobCommitFailed;
		event EventHandlers.ObjectEventHandler<IJob> JobRolledback;
		event EventHandlers.ObjectErrorEventHandler<IPlugin> JobRollbackFailed;
		event EventHandlers.ObjectEventHandler<IJob> JobExecuted;
		event EventHandlers.ObjectErrorEventHandler<IPlugin> JobExecuteFailed;

        IEnumerable<IPlugin> GetRunablePlugins();

        JobCommand CurrentCommand { get; }

        void FinalizeJob();
    }
}