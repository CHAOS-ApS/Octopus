using System;
using System.Collections.Generic;
using Geckon.Events;
using Geckon.Octopus.Agent.Interface;
using Geckon.Octopus.Data;
using Geckon.Octopus.Plugin.Interface;

namespace Geckon.Octopus.Agent.Core
{
    public class AgentEngine : IAgentEngine
    {
        #region Fields

        private readonly IExecutionManager _ExecutionManager;

        #endregion
		#region Events

		public event EventHandlers.ObjectEventHandler<IPlugin>			ExecuteCompleted	= delegate { };
		public event EventHandlers.ObjectErrorEventHandler<IPlugin>		ExecuteFailed		= delegate { };
		public event EventHandlers.ObjectEventHandler<IPlugin>			CommitCompleted		= delegate { };
		public event EventHandlers.ObjectErrorEventHandler<IPlugin>		CommitFailed		= delegate { };
		public event EventHandlers.ObjectEventHandler<IPlugin>			RollbackCompleted	= delegate { };
		public event EventHandlers.ObjectErrorEventHandler<IPlugin>		RollbackFailed		= delegate { };
        private int p;

		#endregion
        #region Properties

        public IPluginManager PluginManager
        {
            get { return _ExecutionManager.PluginManager; }
        }

        public IExecutionManager ExecutionManager
        {
            get { return _ExecutionManager; }
        }

        public int CountPlugins
        {
            get
            {
            	return _ExecutionManager.CountPlugins;
            }
        }

        #endregion
        #region Construction

        public AgentEngine() : this( new ExecutionManager( new PluginManager() ) )
        {
        }

		public AgentEngine(IExecutionManager executionManager)
		{
			_ExecutionManager = executionManager;

			_ExecutionManager.ExecuteCompleted += ExecutionManager_ExecuteCompleted;
			_ExecutionManager.ExecuteFailed += ExecutionManager_ExecuteFailed;
			_ExecutionManager.CommitCompleted += ExecutionManager_CommitCompleted;
			_ExecutionManager.CommitFailed += ExecutionManager_CommitFailed;
			_ExecutionManager.RollbackCompleted += ExecutionManager_RollbackCompleted;
			_ExecutionManager.RollbackFailed += ExecutionManager_RollbackFailed;
		}

        public AgentEngine( int settingsID ):this(  )
        {
            using( DatabaseDataContext db = new DatabaseDataContext() )
            {
                foreach (var executionSlotData in db.ExecutionSlot_GetBy( null, settingsID, null, null ) )
                {
                    IAllocationDefinition definition = new AllocationDefinition(Convert.ToUInt32(executionSlotData.MaxSlots));

                    foreach (var pluginInfo in db.PluginInfo_GetBy(null, executionSlotData.ID, null, null, null, null, null, null, null, null))
                        definition.Add(pluginInfo);

                    AddDefinition( definition );
                }
            }
        }

    	#endregion
        #region Business Logic

        public void AddDefinition( IAllocationDefinition definition )
        {
            IExecutionSlot executionSlot = new ExecutionSlot( definition );

            PluginManager.Install( definition );
            ExecutionManager.Add( executionSlot );
        }

		private void ExecutionManager_ExecuteCompleted(object sender, ObjectEventArgs<IPlugin> eventArgs)
		{
			ExecuteCompleted(this, eventArgs);
		}

		private void ExecutionManager_ExecuteFailed(object sender, ObjectErrorEventArgs<IPlugin> eventArgs)
		{
			ExecuteFailed(this, eventArgs);
		}

		private void ExecutionManager_CommitCompleted(object sender, ObjectEventArgs<IPlugin> eventArgs)
		{
			CommitCompleted(this, eventArgs);
		}

		private void ExecutionManager_CommitFailed(object sender, ObjectErrorEventArgs<IPlugin> eventArgs)
		{
			CommitFailed(this, eventArgs);
		}

		private void ExecutionManager_RollbackCompleted(object sender, ObjectEventArgs<IPlugin> eventArgs)
		{
			RollbackCompleted(this, eventArgs);
		}

		private void ExecutionManager_RollbackFailed(object sender, ObjectErrorEventArgs<IPlugin> eventArgs)
		{
			RollbackFailed(this, eventArgs);
		}

    	public IEnumerable<IPlugin> Plugins
    	{
    		get
    		{
    			return ExecutionManager.Plugins;
    		}
    	}

        public bool HasAvailableSlots( )
        {
            foreach( ExecutionSlot slot in ExecutionManager )
            { 
                if( slot.UsedSlots < slot.Definition.MaxSlots )
                    return true;
            }

            return false;
        }

    	public bool HasAvailableSlots( string pluginIdentifier )
        {
            foreach( ExecutionSlot slot in ExecutionManager )
            { 
                if( slot.Definition.Contains( pluginIdentifier ) && slot.UsedSlots < slot.Definition.MaxSlots )
                    return true;
            }

            return false;
        }

    	public bool ContainsPlugin(string uniqueIdentifier)
    	{
    		return ExecutionManager.ContainsPlugin(uniqueIdentifier);
    	}

    	public bool IsPluginInstalled(string pluginIdentifier)
        {
            return ExecutionManager.PluginManager.IsPluginLoaded( pluginIdentifier );
        }

        public void Execute( IPlugin plugin )
        {
			ExecutionManager.Execute(plugin);
        }

		public void Rollback(IPlugin plugin)
        {
			ExecutionManager.Rollback(plugin);
        }

        public void Commit( IPlugin plugin ) 
        {
            ExecutionManager.Commit( plugin );
        }

        #endregion

    }
}