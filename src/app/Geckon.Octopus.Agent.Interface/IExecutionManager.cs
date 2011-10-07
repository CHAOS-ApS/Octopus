using System.Collections.Generic;
using Geckon.Events;
using Geckon.Octopus.Plugin.Interface;

namespace Geckon.Octopus.Agent.Interface
{
    public interface IExecutionManager : IEnumerable<IExecutionSlot>
    {
        event EventHandlers.ObjectEventHandler<IPlugin> ExecuteCompleted;
		event EventHandlers.ObjectErrorEventHandler<IPlugin> ExecuteFailed;
		event EventHandlers.ObjectEventHandler<IPlugin> CommitCompleted;
		event EventHandlers.ObjectErrorEventHandler<IPlugin> CommitFailed;
		event EventHandlers.ObjectEventHandler<IPlugin> RollbackCompleted;
		event EventHandlers.ObjectErrorEventHandler<IPlugin> RollbackFailed;

		/// <summary>
		/// The number of plugins on all executionslots contained by the IExecutionManager.
		/// </summary>
		int CountPlugins { get; }

		/// <summary>
		/// All plugins on all executionslots contained by the IExecutionManager.
		/// </summary>
		IEnumerable<IPlugin> Plugins { get; }

        /// <summary>
        /// The IPluginManager used by this IExecutionManager
        /// </summary>
        IPluginManager PluginManager { get; }

        /// <summary>
        /// Returns the IExecutionSlot at a given index
        /// </summary>
        /// <param name="index"></param>
        /// <returns></returns>
        IExecutionSlot this[IAllocationDefinition definition] { get; }

        /// <summary>
        /// The number of ExecutionSlots added
        /// </summary>
        int Count { get; }

        /// <summary>
        /// A IDictionary containing the installed Execution slots
        /// </summary>
        void Add( IExecutionSlot executionSlot );

        /// <summary>
        /// Executes the plugin, when there is a free execution slot
        /// </summary>
        /// <param name="plugin">The IPlugin to execute</param>
        void Execute( IPlugin plugin );

        /// <summary>
        /// Commits the plugin
        /// </summary>
        /// <param name="plugin">The IPlugin to commit</param>
        void Commit( IPlugin plugin );

        /// <summary>
        /// Rollsback the plugin
        /// </summary>
        /// <param name="plugin">The IPlugin to rollback</param>
        void Rollback( IPlugin plugin );

		/// <summary>
		/// Returns true if a plugin with a matching UniqueIdentifier is running on the ExecutionManager.
		/// </summary>
		/// <param name="uniqueIdentifier">The UniqueIdentifier of a IPlugin</param>
    	bool ContainsPlugin(string uniqueIdentifier);
    }
}
