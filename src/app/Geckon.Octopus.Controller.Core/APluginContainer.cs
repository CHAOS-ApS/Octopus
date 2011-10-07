using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Geckon.Collections;
using Geckon.Events;
using Geckon.Octopus.Controller.Interface;
using Geckon.Octopus.Plugin.Interface;

namespace Geckon.Octopus.Controller.Core
{
	public abstract class APluginContainer : IPluginContainer
	{
		#region Fields

		private readonly EventEnabledList<IPluginTrackable> _PluginTrackables;

		#endregion
		#region Events

		public event EventHandlers.ObjectEventHandler<IPlugin>			PluginAdded					= delegate { };
		public event EventHandlers.ObjectEventHandler<IPlugin>			PluginRemoved				= delegate { };

		public event EventHandlers.ObjectEventHandler<IPlugin>			ExecuteCompleted			= delegate { };
		public event EventHandlers.ObjectErrorEventHandler<IPlugin>		ExecuteFailed				= delegate { };
		public event EventHandlers.ObjectEventHandler<IPlugin>			CommitCompleted				= delegate { };
		public event EventHandlers.ObjectErrorEventHandler<IPlugin>		CommitFailed				= delegate { };
		public event EventHandlers.ObjectEventHandler<IPlugin>			RollbackCompleted			= delegate { };
		public event EventHandlers.ObjectErrorEventHandler<IPlugin>		RollbackFailed				= delegate { };
    	
		public event EventHandlers.ObjectProgressEventHandler<IPlugin>	OperationProgressChanged	= delegate { };
		public event EventHandlers.ObjectProgressEventHandler<IPlugin>	TotalProgressChanged		= delegate { };

    	#endregion
		#region Properties

		protected EventEnabledList<IPluginTrackable> PluginTrackables
		{
			get { return _PluginTrackables; }
		}

        [Serialization.Xml.Attribute("OperationProgress", true)]
        public double OperationProgress
        {
            get
            {
                double sum = 0.0;

                foreach( IPluginTrackable pluginTrackable in PluginTrackables )
					sum += pluginTrackable.OperationProgress;

                return sum / PluginTrackables.Count;
            }
        }

        [Serialization.Xml.Attribute("TotalProgress", true)]
		public double TotalProgress
		{
			get
			{
				double sum = 0.0;

				foreach (IPluginTrackable pluginTrackable in PluginTrackables)
					sum += pluginTrackable.TotalProgress;

				return sum / PluginTrackables.Count;
			}
		}

		public bool HasRunningPlugins
		{
			get
			{
				foreach (IPluginTrackable pluginTrackable in PluginTrackables)
				{
					if (pluginTrackable is IPlugin)
					{
						if(((IPlugin)pluginTrackable).IsRunning)
							return true;
					}
					else if (pluginTrackable is IPluginContainer)
					{
						if (((IPluginContainer)pluginTrackable).HasRunningPlugins)
							return true;
					} 
					else
						throw new NotImplementedException("Not implemented for " + pluginTrackable.GetType().FullName);
				}

				return false;
			}
		}

		public bool IsSynchronized
		{
			get { return PluginTrackables.IsSynchronized; }
		}

		public object SyncRoot
		{
			get { return PluginTrackables.SyncRoot; }
		}

		public int Count
		{
			get { return PluginTrackables.Count; }
		}

		public bool IsReadOnly
		{
			get { return (PluginTrackables as IList).IsReadOnly; }
		}

		#endregion
		#region Construction

		protected APluginContainer()
		{
			_PluginTrackables = new EventEnabledList<IPluginTrackable>();

			_PluginTrackables.ItemAdded += PluginTrackables_ItemAdded;
			_PluginTrackables.ItemRemoved += PluginTrackables_ItemRemoved;
		}

		#endregion
		#region Event Logic

		private void PluginTrackables_ItemAdded(object sender, ObjectEventArgs<IPluginTrackable> eventArgs)
		{
			eventArgs.EventObject.ExecuteCompleted				+= PluginTrackable_ExecuteComplete;
			eventArgs.EventObject.ExecuteFailed					+= PluginTrackable_ExecuteFailed;
			eventArgs.EventObject.CommitCompleted				+= PluginTrackable_CommitCompleted;
			eventArgs.EventObject.CommitFailed					+= PluginTrackable_CommitFailed;
			eventArgs.EventObject.RollbackCompleted				+= PluginTrackable_RollbackCompleted;
			eventArgs.EventObject.RollbackFailed				+= PluginTrackable_RollbackFailed;

			eventArgs.EventObject.OperationProgressChanged		+= PluginTrackable_OperationProgressChanged;
			eventArgs.EventObject.TotalProgressChanged			+= PluginTrackable_TotalProgressChanged;
			
			if (eventArgs.EventObject is IPlugin)
				PluginAdded(this, new ObjectEventArgs<IPlugin>((IPlugin)eventArgs.EventObject));
			else if(eventArgs.EventObject is IPluginContainer)
				SubscribeToPluginContainer((IPluginContainer) eventArgs.EventObject);
			else
				throw new NotImplementedException("Not implemented for " + eventArgs.EventObject.GetType().FullName);
		}

		private void PluginTrackables_ItemRemoved(object sender, ObjectEventArgs<IPluginTrackable> eventArgs)
		{
			eventArgs.EventObject.ExecuteCompleted				-= PluginTrackable_ExecuteComplete;
			eventArgs.EventObject.ExecuteFailed					-= PluginTrackable_ExecuteFailed;
			eventArgs.EventObject.CommitCompleted				-= PluginTrackable_CommitCompleted;
			eventArgs.EventObject.CommitFailed					-= PluginTrackable_CommitFailed;
			eventArgs.EventObject.RollbackCompleted				-= PluginTrackable_RollbackCompleted;
			eventArgs.EventObject.RollbackFailed				-= PluginTrackable_RollbackFailed;

			eventArgs.EventObject.OperationProgressChanged		-= PluginTrackable_OperationProgressChanged;
			eventArgs.EventObject.TotalProgressChanged			-= PluginTrackable_TotalProgressChanged;

			if (eventArgs.EventObject is IPlugin)
				PluginRemoved(this, new ObjectEventArgs<IPlugin>((IPlugin)eventArgs.EventObject));
			else if(eventArgs.EventObject is IPluginContainer)
				UnsubscribeToPluginContainer((IPluginContainer)eventArgs.EventObject);
			else
				throw new NotImplementedException("Not implemented for " + eventArgs.EventObject.GetType().FullName);
		}

		private void SubscribeToPluginContainer(IPluginContainer pluginContainer)
		{
			List<IPlugin> plugins;
			
			lock (pluginContainer)
			{
				pluginContainer.PluginAdded += PluginContainer_PluginAdded;
				pluginContainer.PluginRemoved += PluginContainer_PluginRemoved;

				plugins = pluginContainer.GetAllPlugins().ToList();
			}

			foreach (var plugin in plugins)
				PluginAdded(this, new ObjectEventArgs<IPlugin>(plugin));
		}

		private void UnsubscribeToPluginContainer(IPluginContainer pluginContainer)
		{
			List<IPlugin> plugins;
			
			lock (pluginContainer)
			{
				pluginContainer.PluginAdded -= PluginContainer_PluginAdded;
				pluginContainer.PluginRemoved -= PluginContainer_PluginRemoved;

				plugins = pluginContainer.GetAllPlugins().ToList();
			}

			foreach (var plugin in plugins)
				PluginRemoved(this, new ObjectEventArgs<IPlugin>(plugin));
		}

		protected virtual void PluginContainer_PluginAdded(object sender, ObjectEventArgs<IPlugin> eventArgs)
		{
			PluginAdded(this, eventArgs);
		}

		protected virtual void PluginContainer_PluginRemoved(object sender, ObjectEventArgs<IPlugin> eventArgs)
		{
			PluginRemoved(this, eventArgs);
		}

		protected virtual void PluginTrackable_ExecuteComplete(object sender, ObjectEventArgs<IPlugin> eventArgs)
		{
			ExecuteCompleted(this, eventArgs);
		}

		protected virtual void PluginTrackable_ExecuteFailed(object sender, ObjectErrorEventArgs<IPlugin> eventArgs)
		{
			ExecuteFailed(this, eventArgs);
		}

		protected virtual void PluginTrackable_CommitCompleted(object sender, ObjectEventArgs<IPlugin> eventArgs)
		{
			CommitCompleted(this, eventArgs);
		}

		protected virtual void PluginTrackable_CommitFailed(object sender, ObjectErrorEventArgs<IPlugin> eventArgs)
		{
			CommitFailed(this, eventArgs);
		}

		protected virtual void PluginTrackable_RollbackCompleted(object sender, ObjectEventArgs<IPlugin> eventArgs)
		{
			RollbackCompleted(this, eventArgs);
		}

		protected virtual void PluginTrackable_RollbackFailed(object sender, ObjectErrorEventArgs<IPlugin> eventArgs)
		{
			RollbackFailed(this, eventArgs);
		}

		protected virtual void PluginTrackable_OperationProgressChanged(object sender, ObjectProgressEventArgs<IPlugin> eventArgs)
		{
			OperationProgressChanged(this, eventArgs);
		}

		protected virtual void PluginTrackable_TotalProgressChanged(object sender, ObjectProgressEventArgs<IPlugin> eventArgs)
		{
			TotalProgressChanged(this, eventArgs);
		}

		#endregion
		#region Business Logic

		public IEnumerable<IPlugin> GetAllPlugins()
		{
			foreach (IPluginTrackable pluginTrackable in PluginTrackables)
			{
				if (pluginTrackable is IPlugin)
				{
					yield return (IPlugin)pluginTrackable;
				}
				else if (pluginTrackable is IPluginContainer)
				{
					foreach (IPlugin plugin in ((IPluginContainer)pluginTrackable).GetAllPlugins())
					{
						yield return plugin;
					}
				}
				else
					throw new NotImplementedException("Not implemented for " + pluginTrackable.GetType().FullName);
			}
		}

		public abstract IEnumerable<IPlugin> GetRunablePlugins(JobCommand command);

		#endregion
	}
}
