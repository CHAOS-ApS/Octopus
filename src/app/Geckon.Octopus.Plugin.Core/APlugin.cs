using System;
using Geckon.Events;
using Geckon.Octopus.Plugin.Interface;
using Geckon.Serialization.Xml;

namespace Geckon.Octopus.Plugin.Core
{
	[Document("Plugin", true)]
    public abstract class APlugin : IPlugin
    {
        #region Fields

        private double _OperationProgress;

    	private string _Version;

    	private bool _IsOperationRightAvailable = true;
		private readonly object _OperatorObjectLock;

    	#endregion
        #region Properties

		[Serialization.Xml.Attribute("Status")]
		public PluginStatus Status { get; set; }

		[Serialization.Xml.Attribute("OperationProgress", true)]
        public double OperationProgress
        {
            get { return _OperationProgress; }
            protected set
            {
                double oldOperationProgres = _OperationProgress;
				double oldTotalProgres = TotalProgress;

            	_OperationProgress = Math.Min(1d, Math.Max(0d, value));

				if (_OperationProgress == oldOperationProgres)
					return;

				OperationProgressChanged(this, new ObjectProgressEventArgs<IPlugin>(this, oldOperationProgres, _OperationProgress));
				TotalProgressChanged(this, new ObjectProgressEventArgs<IPlugin>(this, oldTotalProgres, TotalProgress));
            }
        }

        [Serialization.Xml.Attribute("TotalProgress", true)]
		public virtual double TotalProgress
		{
			get
			{
				switch (Status)
				{
					case PluginStatus.Initializing:
					case PluginStatus.Initialized:
						return 0d;
					case PluginStatus.Executing:
						return OperationProgress * 0.9d;
					case PluginStatus.Executed:
					case PluginStatus.ExecuteFailed:
						return 0.9d;
					case PluginStatus.Committing:
						return 0.9d + OperationProgress * 0.1d;
					case PluginStatus.Committed:
					case PluginStatus.CommitFailed:
						return 1d;
					case PluginStatus.Rollingback:
						return 0.9d + OperationProgress * 0.1d;
					case PluginStatus.Rolledback:
					case PluginStatus.RollbackFailed:
						return 1d;
				}

				throw new NotImplementedException("TotalProgress not implemented for current Status: " + Status);
			}
		}

    	public string UniqueIdentifier { get; protected set; }

        [Serialization.Xml.Attribute("Version", true)]
        public string Version
        {
            get
            {
                if( string.IsNullOrEmpty( _Version ) )
                    _Version = GetType().Assembly.GetName().Version.ToString(); // Reflection is slow, so cache version

                return _Version;
            }
        }

        [Serialization.Xml.Attribute("RollbackLevel")]
    	public uint RollbackLevel { get; set; }

    	public string PluginIdentifier
        {
            get { return Version + ", " + GetType().FullName; }
        }

		public bool IsRunning
		{
			get 
			{
				return Status == PluginStatus.Executing || Status == PluginStatus.Committing || Status == PluginStatus.Rollingback;
			}
		}

		public bool IsReadyForOperation
		{
			get
			{
				return Status == PluginStatus.Initialized || Status == PluginStatus.Executed || Status == PluginStatus.ExecuteFailed;
			}
		}

    	public bool IsOperationRightAvailable
    	{
			get { return _IsOperationRightAvailable; }
			private set { _IsOperationRightAvailable = value; }
    	}

    	#endregion
        #region Events

        public event EventHandlers.ObjectEventHandler<IPlugin>         ExecuteCompleted			= delegate { };
        public event EventHandlers.ObjectErrorEventHandler<IPlugin>    ExecuteFailed			= delegate { };
        public event EventHandlers.ObjectEventHandler<IPlugin>         CommitCompleted			= delegate { };
        public event EventHandlers.ObjectErrorEventHandler<IPlugin>    CommitFailed				= delegate { };
        public event EventHandlers.ObjectEventHandler<IPlugin>         RollbackCompleted		= delegate { };
        public event EventHandlers.ObjectErrorEventHandler<IPlugin>    RollbackFailed			= delegate { };

		public event EventHandlers.ObjectProgressEventHandler<IPlugin> OperationProgressChanged = delegate { };
		public event EventHandlers.ObjectProgressEventHandler<IPlugin> TotalProgressChanged		= delegate { };

        #endregion
        #region Construction

    	protected APlugin()
        {
            UniqueIdentifier		= Guid.NewGuid().ToString();
            Status					= PluginStatus.Initialized;
			_OperatorObjectLock		= new object();
        }

        #endregion
        #region Business Logic

        public void BeginExecute()
        {
            try
            {
				Status = PluginStatus.Executing;

            	OperationProgress = 0.0;

            	RollbackLevel = RollbackLevels.INITIAL_LEVEL;
                
                Execute();

				RollbackLevel = RollbackLevels.FINAL_LEVEL;

                OperationProgress = 1.0;

				Status = PluginStatus.Executed;

				ExecuteCompleted(this, new ObjectEventArgs<IPlugin>(this));
            }
            catch( Exception e )
            {
                Octopus.Core.Logging.Logging.Instance.Log(string.Format("APlugin@BeginExecute() - Message: {0}\nStackTrace: {1}", e.Message, e.StackTrace), true);
                Status = PluginStatus.ExecuteFailed;

				ExecuteFailed(this, new ObjectErrorEventArgs<IPlugin>(this, e));
            }
        }

        public void BeginCommit()
        {
            try
            {
				Status = PluginStatus.Committing;

				OperationProgress = 0.0;
                
                Commit();

				OperationProgress = 1.0;

				Status = PluginStatus.Committed;

				CommitCompleted(this, new ObjectEventArgs<IPlugin>(this));
            }
            catch( Exception e )
            {
                Octopus.Core.Logging.Logging.Instance.Log(string.Format("APlugin@BeginCommit() - Message: {0}\nStackTrace: {1}", e.Message, e.StackTrace), true);
                Status = PluginStatus.CommitFailed;

				CommitFailed(this, new ObjectErrorEventArgs<IPlugin>(this, e));
            }
        }

        public void BeginRollback()
        {
            try
            {
                Status = PluginStatus.Rollingback;

				OperationProgress = 0.0;

                Rollback();

				OperationProgress = 1.0;

				Status = PluginStatus.Rolledback;

				RollbackCompleted(this, new ObjectEventArgs<IPlugin>(this));
            }
            catch( Exception e )
            {
                Octopus.Core.Logging.Logging.Instance.Log(string.Format("APlugin@BeginRollback() - Message: {0}\nStackTrace: {1}", e.Message, e.StackTrace), true);
                Status = PluginStatus.RollbackFailed;

				RollbackFailed(this, new ObjectErrorEventArgs<IPlugin>(this, e));
            }
        }

		public bool GetOperationRight()
		{
			lock (_OperatorObjectLock)
			{
				if (!IsOperationRightAvailable)
					return false;

				IsOperationRightAvailable = false;

				return true;
			}
		}

		public void ReleaseOperationRight()
		{
			lock (_OperatorObjectLock)
			{
				if (IsOperationRightAvailable)
					throw new InvalidOperationException("Operation right is already available");

				IsOperationRightAvailable = true;
			}
		}

        #endregion
        #region Virtual methods

        protected virtual void Execute() { }
        protected virtual void Commit() { }
        protected virtual void Rollback() { }

        #endregion
    }
}