using System;
using Geckon.Octopus.Plugin.Interface;

namespace Geckon.Octopus.Agent.Core
{
    public abstract class APlugin : IPlugin
    {
        #region Fields

        private double _Progress;

        #endregion
        #region Properties

        public double Progress
        {
            get { return _Progress; }
            protected set
            {
                double oldProgres = _Progress;

                if( value > 1.0 )
                    _Progress = 1.0;
                else if( value < 0.0 )
                    _Progress = 0.0;
                else
                    _Progress = value;

                if( ProgressChanged != null )
					ProgressChanged(this, oldProgres);
            }
        }

        public TaskStatus Status { get; private set; }

        public string Version
        {
            get { return GetType().Assembly.GetName().Version.ToString(); }
        }

        #endregion
        #region Events

    	public delegate void ProgressChangedHandler( IPlugin plugin, double oldProgress );
        public delegate void ExecuteCompletedHandler( IPlugin plugin);
        public delegate void ExecuteErrorHandler( IPlugin plugin, Exception exception );
        public delegate void CommitCompletedHandler(IPlugin plugin);
        public delegate void CommitErrorHandler( IPlugin plugin, Exception exception );
        public delegate void RollbackCompletedHandler( IPlugin plugin);
        public delegate void RollbackErrorHandler( IPlugin plugin, Exception exception );

        public event ProgressChangedHandler     ProgressChanged;
        public event ExecuteCompletedHandler	ExecuteCompleted;
        public event ExecuteErrorHandler		ExecuteFailed;
        public event CommitCompletedHandler		CommitCompleted;
        public event CommitErrorHandler			CommitFailed;
        public event RollbackCompletedHandler	RollbackCompleted;
        public event RollbackErrorHandler		RollbackFailed;

        #endregion
        #region Construction

        protected APlugin()
        {
            Status = TaskStatus.Initialized;
        }

        #endregion
        #region Business Logic

        public void BeginExecute()
        {
            try
            {
                Status = TaskStatus.Executing;
                
                Execute();

                Progress = 1.0;

                if( ExecuteCompleted != null )
                    ExecuteCompleted( this );

                Status = TaskStatus.Executed;
            }
            catch( Exception e )
            {
                Status = TaskStatus.Failed;

                if( ExecuteFailed != null )
                    ExecuteFailed( this, e );
            }
        }

        public void BeginCommit()
        {
            try
            {
                Status = TaskStatus.Committing;

                Commit();

                if( CommitCompleted != null )
                    CommitCompleted( this );

                Status = TaskStatus.Committed;
            }
            catch( Exception e )
            {
                Status = TaskStatus.Failed;

                if( CommitFailed != null )
                    CommitFailed( this, e );
            }
        }

        public void BeginRollback()
        {
            try
            {
                Status = TaskStatus.Rollingback;

                Rollback();

                if( RollbackCompleted != null )
                    RollbackCompleted(this);

                Status = TaskStatus.Rolledback;
            }
            catch( Exception e )
            {
                Status = TaskStatus.Failed;

                if( RollbackFailed != null )
                    RollbackFailed( this, e );
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
