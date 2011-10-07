using System;
using System.Collections;
using System.Collections.Generic;
using Geckon.Events;
using Geckon.Octopus.Agent.Interface;
using Geckon.Octopus.Core.Exceptions;
using Geckon.Octopus.Plugin.Interface;

namespace Geckon.Octopus.Agent.Core
{
    public class ExecutionManager : IExecutionManager
    {
        #region Fields

        private readonly IPluginManager _PluginManager;
        private readonly IDictionary<IAllocationDefinition, IExecutionSlot> _ExecutionSlots;

        #endregion
        #region Properties

        public IPluginManager PluginManager
        {
            get { return _PluginManager; }
        }

        public int Count
        {
            get { return ExecutionSlots.Count; }
        }

    	public int CountPlugins
    	{
    		get
    		{
    			var count = 0;

    			foreach (var slot in ExecutionSlots)
    				count += slot.Value.UsedSlots;

    			return count;
    		}
    	}

		public IEnumerable<IPlugin> Plugins
		{
			get
			{
				foreach (KeyValuePair<IAllocationDefinition, IExecutionSlot> slot in ExecutionSlots)
				{
					foreach (var plugin in slot.Value)
						yield return plugin;
				}
			}
		}

        private IDictionary<IAllocationDefinition, IExecutionSlot> ExecutionSlots
        {
            get { return _ExecutionSlots; }
        }

		public IExecutionSlot this[IAllocationDefinition definition]
		{
			get
			{
				return ExecutionSlots[definition];
			}
		}

        #endregion
        #region Events

		public event EventHandlers.ObjectEventHandler<IPlugin>		ExecuteCompleted	= delegate { };
    	public event EventHandlers.ObjectErrorEventHandler<IPlugin>	ExecuteFailed		= delegate { };
    	public event EventHandlers.ObjectEventHandler<IPlugin>		CommitCompleted		= delegate { };
    	public event EventHandlers.ObjectErrorEventHandler<IPlugin>	CommitFailed		= delegate { };
    	public event EventHandlers.ObjectEventHandler<IPlugin>		RollbackCompleted	= delegate { };
    	public event EventHandlers.ObjectErrorEventHandler<IPlugin>	RollbackFailed		= delegate { };

		#region Event Raisers

		private void RaiseExecuteCompleted(object sender, ObjectEventArgs<IPlugin> eventArgs)
		{
			ExecuteCompleted(this, eventArgs);
		}

		private void RaiseExecuteFailed(object sender, ObjectErrorEventArgs<IPlugin> eventArgs)
		{
			ExecuteFailed(this, eventArgs);
		}

		private void RaiseCommitCompleted(object sender, ObjectEventArgs<IPlugin> eventArgs)
		{
			CommitCompleted(this, eventArgs);
		}

		private void RaiseCommitFailed(object sender, ObjectErrorEventArgs<IPlugin> eventArgs)
		{
			CommitFailed(this, eventArgs);
		}

		private void RaiseRollbackCompleted(object sender, ObjectEventArgs<IPlugin> eventArgs)
		{
			RollbackCompleted(this, eventArgs);
		}

		private void RaiseRollbackFailed(object sender, ObjectErrorEventArgs<IPlugin> eventArgs)
		{
			RollbackFailed(sender, eventArgs);
		}

		#endregion
        #endregion
        #region Construction

        public ExecutionManager( IPluginManager manager )
        {
            _PluginManager  = manager;
            _ExecutionSlots = new Dictionary<IAllocationDefinition, IExecutionSlot>();
        }

        #endregion
        #region IEnumerable

        public IEnumerator<IExecutionSlot> GetEnumerator()
        {
            foreach( KeyValuePair<IAllocationDefinition, IExecutionSlot> slot in ExecutionSlots )
            {
                yield return slot.Value;
            }
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        #endregion
        #region Business Logic

        public void Add( IExecutionSlot executionSlot )
        {
			lock (executionSlot)
			{
				executionSlot.ExecuteCompleted		+= RaiseExecuteCompleted;
				executionSlot.ExecuteFailed			+= RaiseExecuteFailed;
				executionSlot.CommitCompleted		+= RaiseCommitCompleted;
				executionSlot.CommitFailed			+= RaiseCommitFailed;
				executionSlot.RollbackCompleted		+= RaiseRollbackCompleted;
				executionSlot.RollbackFailed		+= RaiseRollbackFailed;
			}
			
			ExecutionSlots.Add( executionSlot.Definition, executionSlot );
        }

        public void Execute( IPlugin plugin )
        {
            foreach( KeyValuePair<IAllocationDefinition, IExecutionSlot> slot in ExecutionSlots )
            {
                if( slot.Key.Contains( plugin.PluginIdentifier ) && ExecutionSlots[ slot.Key ].UsedSlots < slot.Key.MaxSlots )
                {
                    slot.Value.Execute( plugin );

                    return;
                }
            }

            throw new NoOpenExecutionSlotsException();
        }

        public void Commit( IPlugin plugin )
        {
            foreach( KeyValuePair<IAllocationDefinition, IExecutionSlot> slot in ExecutionSlots )
            {
                if( slot.Key.Contains( plugin.PluginIdentifier ) && ExecutionSlots[ slot.Key ].UsedSlots < slot.Key.MaxSlots )
                {
                    slot.Value.Commit( plugin );

                    return;
                }
            }

            throw new NoOpenExecutionSlotsException();
        }

        public void Rollback( IPlugin plugin )
        {
            foreach( KeyValuePair<IAllocationDefinition, IExecutionSlot> slot in ExecutionSlots )
            {
                if( slot.Key.Contains( plugin.PluginIdentifier ) && ExecutionSlots[ slot.Key ].UsedSlots < slot.Key.MaxSlots )
                {
                    slot.Value.Rollback( plugin );

                    return;
                }
            }

            throw new NoOpenExecutionSlotsException();
        }

		public bool ContainsPlugin(string uniqueIdentifier)
		{
			foreach (var slot in ExecutionSlots)
			{
				if(slot.Value.Contains(uniqueIdentifier))
					return true;
			}
			return false;
		}

    	#endregion

    }
}