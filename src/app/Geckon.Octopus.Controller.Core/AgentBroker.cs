using System;
using System.Collections.Generic;
using Geckon.Events;
using Geckon.Octopus.Agent.Interface;
using Geckon.Octopus.Controller.Interface;
using Geckon.Octopus.Core.Exceptions;
using Geckon.Octopus.Plugin.Interface;

namespace Geckon.Octopus.Controller.Core
{
	public class Broker : IBroker
	{
		#region Fields

		private readonly IList<IAgent> _Agents;

		#endregion
		#region Properties

		private IList<IAgent> AgentsList
		{
			get { return _Agents; }
		}

		#endregion
		#region Events

		public event EventHandlers.ObjectEventHandler<IPlugin>			ExecuteCompleted	= delegate { };
		public event EventHandlers.ObjectErrorEventHandler<IPlugin>		ExecuteFailed		= delegate { };
		public event EventHandlers.ObjectEventHandler<IPlugin>			CommitCompleted		= delegate { };
		public event EventHandlers.ObjectErrorEventHandler<IPlugin>		CommitFailed		= delegate { };
		public event EventHandlers.ObjectEventHandler<IPlugin>			RollbackCompleted	= delegate { };
		public event EventHandlers.ObjectErrorEventHandler<IPlugin>		RollbackFailed		= delegate { };

		#endregion
		#region Construction

		public Broker()
		{
			_Agents = new List<IAgent>();
		}

		#endregion
		#region Business Logic

		public void Add(IAgent agent)
		{
			AgentsList.Add(agent);

			agent.ExecuteCompleted		+= Agent_ExecuteComplete;
			agent.ExecuteFailed			+= Agent_ExecuteFailed;
			agent.CommitCompleted		+= Agent_CommitComplete;
			agent.CommitFailed			+= Agent_CommitFailed;
			agent.RollbackCompleted		+= Agent_RollbackComplete;
			agent.RollbackFailed		+= Agent_RollbackFailed;
		}

		public IEnumerable<IAgent> Agents
		{
			get { return AgentsList; }
		}

		public IEnumerable<IPlugin> GetPluginsOnAgents()
		{
			foreach (IAgent agent in AgentsList)
			{
				foreach (IPlugin plugin in agent.Plugins)
				{
					yield return plugin;
				}
			}
		}

		public void Execute( IPlugin plugin )
		{
			RunPluginOperation(plugin, (plg, agent) => agent.Execute(plg));
		}

		public void Commit(IPlugin plugin)
		{
			RunPluginOperation(plugin, (plg, agent) => agent.Commit(plg));
		}

		public void Rollback(IPlugin plugin)
		{
			RunPluginOperation(plugin, (plg, agent) => agent.Rollback(plg));		
		}

		private void RunPluginOperation(IPlugin plugin, Action<IPlugin, IAgent> runAction)
		{
			bool isPluginInstalled = false;
			bool hasAvailableSlots = false;

			foreach (IAgent agent in AgentsList)
			{
				if (!isPluginInstalled && agent.IsPluginInstalled(plugin.PluginIdentifier))//TODO: Should check if plugin is actually used on any slots, not if it is installed on the agent.
					isPluginInstalled = true;

				if (agent.HasAvailableSlots(plugin.PluginIdentifier))
				{
					try
					{
						if (plugin.GetOperationRight())
							runAction(plugin, agent);
					}
					catch (Exception)
					{
						plugin.ReleaseOperationRight();

						throw;
					}

					hasAvailableSlots = true;

					break;
				}
			}

			if (!isPluginInstalled)
				throw new PluginNotInstalledException("Plugin: " + plugin.PluginIdentifier + ", not installed.");

			if (!hasAvailableSlots)
				throw new NoOpenExecutionSlotsException("No slots able to run: " + plugin.PluginIdentifier + ", are available.");
		}

        public bool HasAvailableSlots()
        {
            foreach( IAgent agent in Agents )
            {
                if( agent.HasAvailableSlots( ) )
                    return true;
            }

            return false;
        }

		public bool HasAvailableSlots(IPlugin plugin)
		{
			return HasAvailableSlots(plugin, false);
		}

		public bool HasAvailableSlots(IPlugin plugin, bool ignoreQueue)
		{
			foreach( IAgent agent in AgentsList )
			{
				if (agent.IsPluginInstalled(plugin.PluginIdentifier) &&
				    (ignoreQueue || agent.HasAvailableSlots(plugin.PluginIdentifier)))
					return true;
			}

			return false;
		}

		#region Internal Event Handlers

		private void Agent_ExecuteComplete(object sender, ObjectEventArgs<IPlugin> eventArgs)
		{
			eventArgs.EventObject.ReleaseOperationRight();

			ExecuteCompleted(this, eventArgs);
		}

		private void Agent_ExecuteFailed(object sender, ObjectErrorEventArgs<IPlugin> eventArgs)
		{
			eventArgs.EventObject.ReleaseOperationRight();

			ExecuteFailed(this, eventArgs);
		}

		private void Agent_CommitComplete(object sender, ObjectEventArgs<IPlugin> eventArgs)
		{
			eventArgs.EventObject.ReleaseOperationRight();

			CommitCompleted(this, eventArgs);
		}

		private void Agent_CommitFailed(object sender, ObjectErrorEventArgs<IPlugin> eventArgs)
		{
			eventArgs.EventObject.ReleaseOperationRight();

			CommitFailed(this, eventArgs);
		}

		private void Agent_RollbackComplete(object sender, ObjectEventArgs<IPlugin> eventArgs)
		{
			eventArgs.EventObject.ReleaseOperationRight();

			RollbackCompleted(this, eventArgs);
		}

		private void Agent_RollbackFailed(object sender, ObjectErrorEventArgs<IPlugin> eventArgs)
		{
			eventArgs.EventObject.ReleaseOperationRight();

			RollbackFailed(this, eventArgs);
		}

		#endregion

		#endregion
	}
}