using System;
using System.Collections.Generic;
using System.Xml;
using Geckon.Events;
using Geckon.Octopus.Controller.Interface;
using Geckon.Octopus.Plugin.Interface;
using Geckon.Serialization.Xml;
using System.Xml.Linq;

namespace Geckon.Octopus.Controller.Core
{
    [Document("Job")]
    public class Job : Flow, IJob
    {
        #region Fields

    	#endregion
        #region Events

		public event EventHandlers.ObjectEventHandler<IJob>			JobCommitted		= delegate { };
		public event EventHandlers.ObjectErrorEventHandler<IPlugin> JobCommitFailed		= delegate { };
		public event EventHandlers.ObjectEventHandler<IJob>			JobRolledback		= delegate { };
		public event EventHandlers.ObjectErrorEventHandler<IPlugin> JobRollbackFailed	= delegate { };
		public event EventHandlers.ObjectEventHandler<IJob>			JobExecuted			= delegate { };
		public event EventHandlers.ObjectErrorEventHandler<IPlugin> JobExecuteFailed	= delegate { };

    	#endregion
        #region Properties

    	[Serialization.Xml.Attribute("ID")]
    	public int ID { get; set; }

        [Serialization.Xml.Attribute("Command")]
    	public JobCommand CurrentCommand { get; set; }

    	public int StatusID { get; set; }

    	public XElement JobXML
        {
            get
            {
            	return XElement.Parse(
                    XmlSerialize.ToXML(this).OuterXml
                    );
            }
            set
            {
                
            }
        }

    	public DateTime CreatedDate { get; set; }

    	public DateTime LastUpdated { get; set; }

    	#endregion
        #region Construction

        public Job( )
        {
            CurrentCommand = JobCommand.Execute;
            StatusID       = StatusID == (int) Status.New ? (int)Status.JobLoaded : StatusID ; // If it is a new Job set it to loaded
        }

    	public Job( IJobData job ) : this()
        {
    	    ID          = job.ID;
    	    StatusID    = job.StatusID;
    	    CreatedDate = job.CreatedDate;
    	    LastUpdated = job.LastUpdated;

			var xDoc = new XmlDocument();
			xDoc.LoadXml( job.JobXML.ToString());

    		AddPluginTrackables(xDoc.DocumentElement);
        }

        #endregion
		#region Event Logic

		protected override void PluginContainer_PluginAdded(object sender, ObjectEventArgs<IPlugin> eventArgs)
		{
			//TODO: Check if job state needs to be updated.

			base.PluginContainer_PluginAdded(sender, eventArgs);
		}

		protected override void PluginContainer_PluginRemoved(object sender, ObjectEventArgs<IPlugin> eventArgs)
		{
			//TODO: Check if job state needs to be updated.

			base.PluginContainer_PluginRemoved(sender, eventArgs);
		}

		protected override void PluginTrackable_ExecuteComplete(object sender, ObjectEventArgs<IPlugin> eventArgs)
		{
			base.PluginTrackable_ExecuteComplete(sender, eventArgs);

			if (!IsAllPluginsStatus(PluginStatus.Executed))
				return;

			CurrentCommand = JobCommand.Commit;
			StatusID = (int)Status.ExecuteComplete;

			JobExecuted(this, new ObjectEventArgs<IJob>(this));
		}

		protected override void PluginTrackable_ExecuteFailed(object sender, ObjectErrorEventArgs<IPlugin> eventArgs)
		{
			base.PluginTrackable_ExecuteFailed(sender, eventArgs);

			CurrentCommand = JobCommand.Rollback;
			StatusID = (int)Status.ExecuteFailed;

			JobExecuteFailed(this, eventArgs);
		}

		protected override void PluginTrackable_CommitCompleted(object sender, ObjectEventArgs<IPlugin> eventArgs)
		{
			base.PluginTrackable_CommitCompleted(sender, eventArgs);

			if (!IsAllPluginsStatus(PluginStatus.Committed))
				return;

			CurrentCommand = JobCommand.Finalize;
			StatusID = (int)Status.CommitComplete;

			JobCommitted(this, new ObjectEventArgs<IJob>(this));
		}

		protected override void PluginTrackable_CommitFailed(object sender, ObjectErrorEventArgs<IPlugin> eventArgs)
		{
			base.PluginTrackable_CommitFailed(sender, eventArgs);

			CurrentCommand = JobCommand.Finalize;
			StatusID = (int)Status.CommitFailed;

			JobCommitFailed(this, eventArgs);
		}

		protected override void PluginTrackable_RollbackCompleted(object sender, ObjectEventArgs<IPlugin> eventArgs)
		{
			base.PluginTrackable_RollbackCompleted(sender, eventArgs);

			if (!IsAllPluginsStatus(PluginStatus.Rolledback, PluginStatus.Initialized))
				return;

			CurrentCommand = JobCommand.Finalize;
			StatusID = (int)Status.RollbackComplete;

			JobRolledback(this, new ObjectEventArgs<IJob>(this));
		}

		protected override void PluginTrackable_RollbackFailed(object sender, ObjectErrorEventArgs<IPlugin> eventArgs)
		{
			base.PluginTrackable_RollbackFailed(sender, eventArgs);

			CurrentCommand = JobCommand.Finalize;
			StatusID = (int)Status.RollbackFailed;

			JobRollbackFailed(this, eventArgs);
		}
		
		#endregion
		#region Business Logic

		public IEnumerable<IPlugin> GetRunablePlugins( )
        {
			return base.GetRunablePlugins(CurrentCommand);
        }

		public override IEnumerable<IPlugin> GetRunablePlugins(JobCommand command)
		{
			throw new MethodAccessException("This function is not available on Job class.");
		}

		public void FinalizeJob()
		{
			CurrentCommand = JobCommand.None;
		}

		private bool IsAllPluginsStatus(params PluginStatus[] statusFilter)
		{
			return IsPluginsStatus(GetAllPlugins(), statusFilter);
		}

		private static bool IsPluginsStatus(IEnumerable<IPlugin> plugins, params PluginStatus[] statusFilter)
		{
			foreach (var plugin in plugins)
			{
				if (!Contains(plugin.Status, statusFilter))
					return false;
			}

			return true;
		}

		private static bool Contains(PluginStatus inStatus, params PluginStatus[] statusFilter)
		{
			foreach (PluginStatus status in statusFilter)
			{
				if (inStatus == status)
					return true;
			}

			return false;
		}

        #endregion
    }
}