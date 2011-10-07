using System.Collections.Generic;
using Geckon.Events;
using Geckon.Octopus.Agent.Interface;
using Geckon.Octopus.Plugin.Interface;

namespace Geckon.Octopus.Controller.Interface
{
    public interface IBroker
    {
		event EventHandlers.ObjectEventHandler<IPlugin> ExecuteCompleted;
		event EventHandlers.ObjectErrorEventHandler<IPlugin> ExecuteFailed;
		event EventHandlers.ObjectEventHandler<IPlugin> CommitCompleted;
		event EventHandlers.ObjectErrorEventHandler<IPlugin> CommitFailed;
		event EventHandlers.ObjectEventHandler<IPlugin> RollbackCompleted;
		event EventHandlers.ObjectErrorEventHandler<IPlugin> RollbackFailed;

        void Add(IAgent agent);

    	IEnumerable<IAgent> Agents { get; }

    	IEnumerable<IPlugin> GetPluginsOnAgents();

    	void Execute(IPlugin plugin);
        void Rollback(IPlugin plugin);
        void Commit(IPlugin plugin);

        bool HasAvailableSlots();
        bool HasAvailableSlots(IPlugin plugin);
        bool HasAvailableSlots(IPlugin plugin, bool ignoreQueue );
    }
}