using System;
using System.Collections.Generic;
using System.IO;
using System.Threading;
using Geckon.Events;
using Geckon.Octopus.Agent.Core;
using Geckon.Octopus.Controller.Interface;
using Geckon.Octopus.Core.Exceptions;
using Geckon.Octopus.Data;
using Geckon.Octopus.Plugin.Core;
using Geckon.Octopus.Plugin.Interface;
using ThreadState=System.Threading.ThreadState;

namespace Geckon.Octopus.Controller.Core
{
	public class ControllerEngine : IControllerEngine
	{
		#region Fields

		private readonly IJobManager		_JobManager;
		private readonly IBroker			_Broker;

		private bool						_ShouldRunPlugins;
	    private readonly Thread             _PluginThread;
		private readonly AutoResetEvent		_PluginThreadBlocker;

		#endregion
		#region Properties

		public IJobManager JobManager
		{
			get { return _JobManager; }
		}

		public IBroker Broker
		{
			get { return _Broker; }
		}

		public bool IsDisposed			{ get; private set; }

		private Thread PluginThread		{ get { return _PluginThread; } }

	    public bool ShouldRunPlugins
		{
			get{ return _ShouldRunPlugins; }
			set
			{
				lock(this)
				{
					ValidateIsNotDisposed();

					if (_ShouldRunPlugins == value)
						return;

					_ShouldRunPlugins = value;

					if( _ShouldRunPlugins )
                        BeginCheckAllPluginsAndRunRunables();
				}
			}
		}

		#endregion
		#region Construction

		public ControllerEngine(bool startSynchronization) : this(startSynchronization, Core.JobManager.DEFAULT_SYNCHRONIZATION_FREQUENCY_INTERVAL)
		{
		}

		public ControllerEngine(bool startSynchronization, int syncFrequency)
		{
			_JobManager					= new JobManager();
			_Broker						= new Broker();
			_PluginThreadBlocker		= new AutoResetEvent(false);
			
			IsDisposed					= false;
			_ShouldRunPlugins			= startSynchronization;

			_PluginThread				= new Thread(CheckAllPluginsAndRunRunablesLoop);
			_PluginThread.Name			= "Controller Thread";

			JobManager.JobAddedToQueue	+= JobManager_JobAddedToQueue;
			Broker.ExecuteCompleted		+= Broker_PluginOperationEnded;
			Broker.ExecuteFailed		+= Broker_PluginOperationEnded;
			Broker.CommitCompleted		+= Broker_PluginOperationEnded;
			Broker.CommitFailed			+= Broker_PluginOperationEnded;
			Broker.RollbackCompleted	+= Broker_PluginOperationEnded;
			Broker.RollbackFailed		+= Broker_PluginOperationEnded;
			
		    InitializeAgents();

			if (startSynchronization)
                _JobManager.BeginSynchronize(syncFrequency);
		}

	    private void InitializeAgents()
	    { 
            using( var db = new DatabaseDataContext() )
            {
                foreach( var agentData in db.Agent_GetBy(null,null) )
                    Broker.Add( new AgentEngine( agentData.ID ) );

                PluginLoader.Clear();

                foreach( AssemblyInfo info in db.AssemblyInfo_Get( null, null, null, null, null, null, null, true ) )
                {
                    if( !PluginLoader.IsAssemblyLoaded( info.Version + ", " + info.Name ) )
                        PluginLoader.Add( info.Version + ", " + info.Name, info.ReadURL ); // Add AssemblyIdentifier !!!!!!!!!!!!!!
                }
            }
	    }

		#endregion
		#region Business Logic

		private void JobManager_JobAddedToQueue(object sender, ObjectEventArgs<IJob> eventargs)
		{
			if( IsDisposed || !ShouldRunPlugins )
				return;
            
			BeginCheckAllPluginsAndRunRunables();
		}

		private void Broker_PluginOperationEnded(object sender, ObjectEventArgs<IPlugin> eventargs)
		{
           if( IsDisposed || !ShouldRunPlugins )
				return;
			
			BeginCheckAllPluginsAndRunRunables();
		}

		private void BeginCheckAllPluginsAndRunRunables()
		{
	        try
	        {
				if (PluginThread.ThreadState == ThreadState.Unstarted)
					PluginThread.Start();
				else
					_PluginThreadBlocker.Set();
	        }
	        catch( Exception e )
	        {
                // TODO: Proper error handling
                Octopus.Core.Logging.Logging.Instance.Log(string.Format("ControllerEngine@BeginCheckAllPluginsAndRunRunables() - Thread: {0}\nMessage: {1}\nStackTrace: {2}", Thread.CurrentThread.ManagedThreadId, e.Message, e.StackTrace), true);
	        }
		}

        private void CheckAllPluginsAndRunRunablesLoop()
		{
		    try
		    {
		        while( !IsDisposed )
		        {
                    CheckAllPluginsAndRunRunables( );

		        	_PluginThreadBlocker.WaitOne();
		        }
		    }
		    catch( Exception e )
		    {
                Octopus.Core.Logging.Logging.Instance.Log(string.Format("ControllerEngine@CheckPluginsAndRunRunables() - Thread: {0}\nMessage: {1}\nStackTrace: {2}", Thread.CurrentThread.ManagedThreadId, e.Message, e.StackTrace), true);
		    }
		}

	    private void CheckAllPluginsAndRunRunables( )
	    {
            foreach( IJob job in JobManager )
	        {
	            if( !Broker.HasAvailableSlots() )
	                return;

                // REVIEW: Better implementation of prioritizing commit/rollback/finalize
                if( job.CurrentCommand == JobCommand.Execute )
                    continue;

	            CheckPluginsAndRunRunables(job);
	        }

	        foreach( IJob job in JobManager )
	        {
	            if( !Broker.HasAvailableSlots() )
                    return;

	            CheckPluginsAndRunRunables(job);
	        }
	    }

	    private void CheckPluginsAndRunRunables(IJob job)
		{
			try
			{
				switch (job.CurrentCommand)
				{
					case JobCommand.Execute:
						ExecutePlugins(job, plugin => Broker.Execute(plugin));
						break;
					case JobCommand.Commit:
						ExecutePlugins(job, plugin => Broker.Commit(plugin));
						break;
					case JobCommand.Rollback:
						ExecutePlugins(job, plugin => Broker.Rollback(plugin));
						break;
					case JobCommand.Finalize:
						job.FinalizeJob();
						break;
					case JobCommand.None:
                            
						break;
				}
			}
			catch( NoOpenExecutionSlotsException )
			{
				// TODO: Handle exception
			}
			catch( PluginNotInstalledException e )
			{
                Octopus.Core.Logging.Logging.Instance.Log( string.Format( "ControllerEngine@CheckPluginsAndRunRunables(IJob) - Message: {0}\nStackTrace: {1}", e.Message, e.StackTrace ), false );
				// TODO: Handle exception
			}
		}

		private void ExecutePlugins(IJob job, Action<IPlugin> pluginAction)
		{
			foreach( IPlugin plugin in job.GetRunablePlugins() )
	        {
				lock (plugin)
				{
					if(plugin.IsRunning || !plugin.IsReadyForOperation)
						continue;

					if (Broker.HasAvailableSlots(plugin /*, job.CurrentCommand == JobCommands.Commit*/))
						pluginAction(plugin);
				}
	        }
		}

		#endregion
		#region IDisposable

		public void Dispose()
		{
			lock (this)
			{
				if (IsDisposed)
					return;

				IsDisposed = true;

				GC.SuppressFinalize(this);

                _PluginThreadBlocker.Set();

                if( _PluginThread.ThreadState != ThreadState.Unstarted )
                    _PluginThread.Join();

				_PluginThreadBlocker.Close();

				if (JobManager != null)
					JobManager.Dispose();
			}
		}

		private void ValidateIsNotDisposed()
		{
			if (IsDisposed)
				throw new ObjectDisposedException(GetType().ToString());
		}

		#endregion
	}
}