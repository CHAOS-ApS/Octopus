using System.Collections.Generic;
using Geckon.Events;
using Geckon.Octopus.Plugin.Interface;

namespace Geckon.Octopus.Agent.Interface
{
    public interface IAgent
    {
    	event EventHandlers.ObjectEventHandler<IPlugin>			ExecuteCompleted;
    	event EventHandlers.ObjectErrorEventHandler<IPlugin>	ExecuteFailed;
    	event EventHandlers.ObjectEventHandler<IPlugin>			CommitCompleted;
    	event EventHandlers.ObjectErrorEventHandler<IPlugin>	CommitFailed;
    	event EventHandlers.ObjectEventHandler<IPlugin>			RollbackCompleted;
    	event EventHandlers.ObjectErrorEventHandler<IPlugin>	RollbackFailed;
		
		/// <summary>
        /// Adds a new definition to the
        /// </summary>
        /// <param name="definition">The definition to add</param>
        void AddDefinition(IAllocationDefinition definition);

        /// <summary>
        /// Adds the IPlugin with the ID, to the appropriate ExecutionSlot if available
        /// </summary>
		/// <param name="plugin">The IPlugin</param>
		void Execute(IPlugin plugin);

        /// <summary>
        /// Adds the IPlugin with the ID, to the appropriate ExecutionSlot if available
        /// </summary>
		/// <param name="plugin">The IPlugin</param>
		void Rollback(IPlugin plugin);

        /// <summary>
        /// Adds the IPlugin with the ID, to the appropriate ExecutionSlot if available
        /// </summary>
		/// <param name="plugin">The IPlugin</param>
		void Commit(IPlugin plugin);

    	/// <summary>
    	/// Returns a IEnumerable containing the current plugins.
    	/// </summary>
    	/// <returns></returns>
    	IEnumerable<IPlugin> Plugins { get; }

        /// <summary>
        /// Checks if the agent has any available slots
        /// </summary>
        /// <returns></returns>
        bool HasAvailableSlots();

    	/// <summary>
        /// Checks if the agent has any available slots, to handle the plugin
        /// </summary>
        /// <param name="pluginIdentifier"></param>
        /// <returns></returns>
        bool HasAvailableSlots( string pluginIdentifier );

        /// <summary>
        /// Determines if the plugin is added to the agent queue
        /// </summary>
        /// <param name="uniqueIdentifier"></param>
        /// <returns></returns>
        bool ContainsPlugin( string uniqueIdentifier );

        /// <summary>
        /// Determines if the plugin is installed on the agent
        /// </summary>
        /// <param name="pluginIdentifier"></param>
        /// <returns></returns>
        bool IsPluginInstalled(string pluginIdentifier);
    }
}