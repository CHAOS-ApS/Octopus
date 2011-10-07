using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using Geckon.Events;
using Geckon.Octopus.Agent.Interface;
using Geckon.Octopus.Core.Exceptions;
using Geckon.Octopus.Plugin.Interface;
using Geckon.Parallel;

namespace Geckon.Octopus.Agent.Core
{
    public class ExecutionSlot : IExecutionSlot
    {
        #region Fields

        private readonly IAllocationDefinition _Definition;
        private readonly IList<IPlugin> _Plugins;
        private readonly ParallelQueue _ParallelPluginRunner;

        #endregion
        #region Properties

        public IAllocationDefinition Definition
        {
            get { return _Definition; }
        }

        public int UsedSlots
        {
            get { return Plugins.Count; }
        }

    	private IList<IPlugin> Plugins
        {
            get { return _Plugins; }
        }

    	private ParallelQueue ParallelPluginRunner
        {
            get { return _ParallelPluginRunner; }
        }

        #endregion
        #region Events

    	public event EventHandlers.ObjectEventHandler<IPlugin>		ExecuteCompleted	= delegate { };
		public event EventHandlers.ObjectErrorEventHandler<IPlugin>	ExecuteFailed		= delegate { };
		public event EventHandlers.ObjectEventHandler<IPlugin>		CommitCompleted		= delegate { };
		public event EventHandlers.ObjectErrorEventHandler<IPlugin>	CommitFailed		= delegate { };
		public event EventHandlers.ObjectEventHandler<IPlugin>		RollbackCompleted	= delegate { };
		public event EventHandlers.ObjectErrorEventHandler<IPlugin>	RollbackFailed		= delegate { };

        #endregion
        #region Construction

        public ExecutionSlot( IAllocationDefinition definition )
        {
            if( definition == null )
                throw new NullReferenceException( "Definition cannot be null" );

            _Plugins              = new List<IPlugin>();
            _Definition           = definition;
            _ParallelPluginRunner = new ParallelQueue( definition.MaxSlots, true );
        }

        #endregion
        #region Business Logic

        public IEnumerator<IPlugin> GetEnumerator()
        {
            return Plugins.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

    	public bool Contains(string uniqueIdentifier)
    	{
    		foreach (var plugin in Plugins)
    		{
    			if(plugin.UniqueIdentifier == uniqueIdentifier)
    				return true;
    		}

    		return false;
    	}

    	public void Execute( IPlugin plugin )
        {
			if (plugin == null)
				throw new ArgumentNullException("plugin");
			
			try
        	{
				plugin.ExecuteCompleted += ExecutionSlot_ExecuteCompleted;
				plugin.ExecuteFailed += ExecutionSlot_ExecuteFailed;

				VerifyPluginAndRunMethod(plugin, plugin.BeginExecute);
        	}
        	catch (Exception)
        	{
				plugin.ExecuteCompleted -= ExecutionSlot_ExecuteCompleted;
				plugin.ExecuteFailed -= ExecutionSlot_ExecuteFailed;

        		throw;
        	}
			
        }

		public void Commit(IPlugin plugin)
		{
			if (plugin == null)
				throw new ArgumentNullException("plugin");
			
			try
			{
				plugin.CommitCompleted += ExecutionSlot_CommitCompleted;
				plugin.CommitFailed += ExecutionSlot_CommitFailed;

				VerifyPluginAndRunMethod(plugin, plugin.BeginCommit);
			}
			catch (Exception)
			{
				plugin.CommitCompleted -= ExecutionSlot_CommitCompleted;
				plugin.CommitFailed -= ExecutionSlot_CommitFailed;

				throw;
			}
		}

        public void Rollback( IPlugin plugin )
        {
			if (plugin == null)
				throw new ArgumentNullException("plugin");
			
			try
        	{
				plugin.RollbackCompleted += ExecutionSlot_RollbackCompleted;
				plugin.RollbackFailed += ExecutionSlot_RollbackFailed;

				VerifyPluginAndRunMethod(plugin, plugin.BeginRollback);
        	}
        	catch (Exception)
        	{
				plugin.RollbackCompleted -= ExecutionSlot_RollbackCompleted;
				plugin.RollbackFailed -= ExecutionSlot_RollbackFailed;

        		throw;
        	}
        }

        private void VerifyPluginAndRunMethod( IPlugin plugin, ThreadStart method )
        {
            lock( Plugins )
            {
                if (Plugins.Contains(plugin))
                    throw new ArgumentException("Plugin " + plugin.UniqueIdentifier + " of type " + plugin.PluginIdentifier + " is already added to this executionSlot");

                if( Definition.MaxSlots <= UsedSlots )
                    throw new NoOpenExecutionSlotsException();

                if( !Definition.Contains( plugin.PluginIdentifier ) )
                    throw new PluginNotInstalledException(string.Format("This ExecutionSlot doesn't support the plugin '{0}'", plugin.PluginIdentifier));

                Plugins.Add( plugin );

                ParallelPluginRunner.Put( method );
            }
        }

		private void ExecutionSlot_ExecuteCompleted(object sender, ObjectEventArgs<IPlugin> eventArgs)
        {
			lock( Plugins )
            {
				Plugins.Remove(eventArgs.EventObject);

				eventArgs.EventObject.ExecuteCompleted -= ExecutionSlot_ExecuteCompleted;
				eventArgs.EventObject.ExecuteFailed -= ExecutionSlot_ExecuteFailed;
            }

			ExecuteCompleted(this, eventArgs);
        }

		private void ExecutionSlot_ExecuteFailed(object sender, ObjectErrorEventArgs<IPlugin> eventArgs)
		{
			lock (Plugins)
			{
				Plugins.Remove(eventArgs.EventObject);

				eventArgs.EventObject.ExecuteCompleted -= ExecutionSlot_ExecuteCompleted;
				eventArgs.EventObject.ExecuteFailed -= ExecutionSlot_ExecuteFailed;
			}
			Octopus.Core.Logging.Logging.Instance.Log(string.Format("ExecutionSlot@ExecutionSlot_ExecuteFailed() - Message: {0}\nStackTrace: {1}", eventArgs.Exception.Message, eventArgs.Exception.StackTrace), true);

			ExecuteFailed(this, eventArgs);
		}

		void ExecutionSlot_CommitCompleted(object sender, ObjectEventArgs<IPlugin> eventArgs)
        {
			lock (Plugins)
			{
				Plugins.Remove(eventArgs.EventObject);

				eventArgs.EventObject.CommitCompleted -= ExecutionSlot_CommitCompleted;
				eventArgs.EventObject.CommitFailed -= ExecutionSlot_CommitFailed;
			}

			CommitCompleted(this, eventArgs);
        }

		void ExecutionSlot_CommitFailed(object sender, ObjectErrorEventArgs<IPlugin> eventArgs)
		{
			lock (Plugins)
			{
				Plugins.Remove(eventArgs.EventObject);

				eventArgs.EventObject.CommitCompleted -= ExecutionSlot_CommitCompleted;
				eventArgs.EventObject.CommitFailed -= ExecutionSlot_CommitFailed;
			}
			Octopus.Core.Logging.Logging.Instance.Log(string.Format("ExecutionSlot@ExecutionSlot_ExecuteFailed() - Message: {0}\nStackTrace: {1}", eventArgs.Exception.Message, eventArgs.Exception.StackTrace), true);

			CommitFailed(this, eventArgs);
		}

		void ExecutionSlot_RollbackCompleted(object sender, ObjectEventArgs<IPlugin> eventArgs)
        {
			lock (Plugins)
			{
				Plugins.Remove(eventArgs.EventObject);

				eventArgs.EventObject.RollbackCompleted -= ExecutionSlot_RollbackCompleted;
				eventArgs.EventObject.RollbackFailed -= ExecutionSlot_RollbackFailed;
			}

			RollbackCompleted(this, eventArgs);
        }

		void ExecutionSlot_RollbackFailed(object sender, ObjectErrorEventArgs<IPlugin> eventArgs)
		{
			lock (Plugins)
			{
				Plugins.Remove(eventArgs.EventObject);

				eventArgs.EventObject.RollbackCompleted -= ExecutionSlot_RollbackCompleted;
				eventArgs.EventObject.RollbackFailed -= ExecutionSlot_RollbackFailed;
			}
			Octopus.Core.Logging.Logging.Instance.Log(string.Format("ExecutionSlot@ExecutionSlot_ExecuteFailed() - Message: {0}\nStackTrace: {1}", eventArgs.Exception.Message, eventArgs.Exception.StackTrace), true);

			RollbackFailed(this, eventArgs);
		}

        #endregion
    }
}