using System.Collections.Generic;
using Geckon.Events;
using Geckon.Octopus.Plugin.Interface;

namespace Geckon.Octopus.Agent.Interface
{
    public interface IExecutionSlot : IEnumerable<IPlugin>
    {
        /// <summary>
        /// The IAllocationDefinition this slot is limited to.
        /// </summary>
        IAllocationDefinition Definition { get; }

        /// <summary>
        /// Get the number of usedslots
        /// </summary>
        int UsedSlots { get; }

		/// <summary>
		/// Returns true if a plugin with a matching UniqueIdentifier is running on the IExecutionSlot.
		/// </summary>
		/// <param name="uniqueIdentifier">The UniqueIdentifier of a IPlugin</param>
		bool Contains(string uniqueIdentifier);

        /// <summary>
        /// Executes a concrete IPlugin (Task).
        /// </summary>
        /// <param name="plugin">The plugin to execute</param>
        void Execute(IPlugin plugin);

        /// <summary>
        /// Rollback a concrete IPlugin.
        /// </summary>
        /// <param name="plugin">The plugin to rollback</param>
        void Rollback(IPlugin plugin);

        /// <summary>
        /// Commits a concrete IPlugin.
        /// </summary>
        /// <param name="plugin">The plugin to commit</param>
        void Commit( IPlugin plugin );

        event EventHandlers.ObjectEventHandler<IPlugin> ExecuteCompleted;
		event EventHandlers.ObjectErrorEventHandler<IPlugin> ExecuteFailed;
		event EventHandlers.ObjectEventHandler<IPlugin>  RollbackCompleted;
		event EventHandlers.ObjectErrorEventHandler<IPlugin> RollbackFailed;
		event EventHandlers.ObjectEventHandler<IPlugin> CommitCompleted;
		event EventHandlers.ObjectErrorEventHandler<IPlugin> CommitFailed;
    }
}
