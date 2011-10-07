using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading;
using Geckon.Events;
using Geckon.Octopus.Controller.Interface;
using Geckon.Octopus.Data;
using ThreadState=System.Threading.ThreadState;

namespace Geckon.Octopus.Controller.Core
{
    public class JobManager : IJobManager
    {
        #region Fields

		public const int				DEFAULT_SYNCHRONIZATION_FREQUENCY_INTERVAL = 5000;

        private readonly Thread			_SynchronizationThread;
        private readonly AutoResetEvent	_SynchronizationThreadBlocker;
		private readonly IList<IJob>	_JobQueue;
		private readonly Timer			_SynchronizationTimer;

        #endregion
        #region Properties

        public IDictionary<FileSystemWatcher, WatchFolder> WatchFolders { get; set; }

        private IList<IJob> JobQueue
    	{
    		get { return _JobQueue; }
    	}
    	
    	private Timer SynchronizationTimer
    	{
    		get { return _SynchronizationTimer; }
    	}

    	private Thread SynchronizationThread
        { 
            get { return _SynchronizationThread; }
        }

        public int Count
        {
            get { return JobQueue.Count; }
        }

		public bool IsDisposed { get; private set; }

    	#endregion
        #region Events

        public event JobManagerDelegates.JobManagerSynchronized SyncCompleted	= delegate { };
        public event EventHandlers.ObjectEventHandler<IJob>     JobAddedToQueue	= delegate { };

        #endregion
        #region Construction

        public JobManager( )
        {
            _JobQueue						= new List<IJob>();
			_SynchronizationTimer			= new Timer(SynchronizationTimerCallback, null, Timeout.Infinite, Timeout.Infinite);
			_SynchronizationThreadBlocker	= new AutoResetEvent(false);
			IsDisposed						= false;

			_SynchronizationThread			= new Thread(SynchronizeLoop);
			_SynchronizationThread.Name		= "Synchronization Thread";
        }

        #endregion
        #region Business Logic

        #region BeginSynchronize

        private void SynchronizeLoop()
        {
            while( !IsDisposed )
            {
                try
                {
                    UpdateWatchFolders();
					Synchronize();

                	_SynchronizationThreadBlocker.WaitOne();
                }
                catch( Exception e )
                { 
				    //TODO: Implement Error handling
                    Octopus.Core.Logging.Logging.Instance.Log(string.Format("JobManager@Synchronize() - Thread: {0}\nMessage: {1}\nStackTrace: {2}", Thread.CurrentThread.ManagedThreadId, e.Message, e.StackTrace), true);
                }
            }
        }

		private void Synchronize()
        {
            IList<IJob> addedJobs   = SyncInMemoryCollectionWithDB();
            IList<IJob> removedJobs = SyncDBWithInMemoryCollection();

            lock( JobQueue )
            {
                foreach( IJob job in addedJobs )
                {
                    job.StatusID = ( int ) Status.JobQueued;

                    JobQueue.Add( job );
                }
                        
                foreach( IJob job in removedJobs )
                {
                    JobQueue.Remove( job );
                }
            }

			foreach( var job in addedJobs )
				JobAddedToQueue(this, new ObjectEventArgs<IJob>(job));

			SyncCompleted( this, new CollectionEventArgs< IList<IJob> >( JobQueue ) );
        }

        private void SynchronizationTimerCallback( object state )
        {
			_SynchronizationThreadBlocker.Set();
        }

		public void SynchronizeOnce()
		{
			lock (this)
			{
				ValidateIsNotDisposed();

				if (SynchronizationThread.ThreadState == ThreadState.Unstarted)
					SynchronizationThread.Start();
				else
					_SynchronizationThreadBlocker.Set();
			}
		}

		public void BeginSynchronize( int synchronizationFrequency )
        {
			lock( this )
			{
				ValidateIsNotDisposed();

				SynchronizeOnce();

				SynchronizationTimer.Change(synchronizationFrequency, synchronizationFrequency);
			}
        }

        private IList<IJob> SyncDBWithInMemoryCollection( )
        {
            
            IList<IJob> deleteJobs   = new List<IJob>();
            IList<IJob> currentQueue = new List<IJob>();

            lock( JobQueue )
            {
                foreach( IJob job in JobQueue )
                {
                    currentQueue.Add( job );

                    if( job.CurrentCommand == JobCommand.None )
                        deleteJobs.Add(job);
                }
            }

            using( DatabaseDataContext db = new DatabaseDataContext() )
            {
            	var jobsStatus = new List<uint>{0, 0, 0, 0, 0};
				var jobsTotalProgress = 0d;

                foreach( IJob job in currentQueue )
                {
                	jobsStatus[(int)job.CurrentCommand]++;
					jobsTotalProgress += job.TotalProgress;

                    db.Job_Update(job); // Slows down the Lock, if inside!
                }

            	Trace.Write(
            		string.Format(
						"Thread: {0}\tTotal: {1}({2})\tExecute: {3}\tCommit: {4}\tRollback: {5}\tFinalize: {6}\tNone: {7}\n",
            			Thread.CurrentThread.ManagedThreadId, currentQueue.Count,
											(currentQueue.Count == 0 ? 0 : jobsTotalProgress / currentQueue.Count).ToString("F"),
            								jobsStatus[(int) JobCommand.Execute],
											jobsStatus[(int)JobCommand.Commit],
											jobsStatus[(int)JobCommand.Rollback],
											jobsStatus[(int)JobCommand.Finalize],
											jobsStatus[(int)JobCommand.None]));
            }

            return deleteJobs;
        }

		private IList<IJob> SyncInMemoryCollectionWithDB()
        {
            IList<IJob> addedJobs = new List<IJob>();

			using( DatabaseDataContext db = new DatabaseDataContext() )
            {
                foreach( Data.Job job in db.Job_GetUnfinishedJobs() )
                {
                    if( ContainsKey( job.ID ) ) 
                        continue;

					addedJobs.Add( new Job( job ) );
                }
            }

            return addedJobs;
        }

        #endregion
        #region Collection

        public IEnumerator<IJob> GetEnumerator()
        {
            ValidateIsNotDisposed();

            lock( JobQueue )
            {
                foreach( IJob job in JobQueue )
                {
                    yield return job;
                }
            }
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
//        	ValidateIsNotDisposed();
			
			return GetEnumerator();
        }

        public bool ContainsKey( int id )
        {
			ValidateIsNotDisposed();

            lock( JobQueue )
            {
                // REVIEW: If performance become an issue, replace with a combination of Dictionary and IList, to get fast lookup and a Sequenced queue
                return ( from jobCol in JobQueue where jobCol.ID == id select jobCol ).Count() == 1;
            }
        }

        #endregion
        #region Watchfolder

        private void UpdateWatchFolders()
        {
            if( WatchFolders == null )
                WatchFolders = new Dictionary<FileSystemWatcher, WatchFolder>();

            using( DatabaseDataContext db = new DatabaseDataContext() )
            {
                foreach( WatchFolder watchFolder in db.WatchFolder_Get( null, null ) )
                {
                    if( IsWatchfolderAlreadyAddedAndUnchanged( watchFolder ) )
                        continue;

                    FileSystemWatcher watcher = GetWatchfolder( watchFolder.ID );

                    if( WatchFolders.ContainsKey( watcher ) )
                        WatchFolders[ watcher ] = watchFolder;
                    else
                        WatchFolders.Add( watcher, watchFolder );

                    watcher.InternalBufferSize  = 32 * 1024;
                    watcher.Filter              = watchFolder.Filter;
                    watcher.Path                = watchFolder.Destination.WriteURL;
                    watcher.EnableRaisingEvents = watchFolder.IsEnabled;                    
                }
            }
        }


        private FileSystemWatcher GetWatchfolder( int id )
        {
            foreach( KeyValuePair<FileSystemWatcher, WatchFolder> keyValuePair in WatchFolders )
            {
                if( keyValuePair.Value.ID == id )
                    return keyValuePair.Key;
            }

            FileSystemWatcher watcher = new FileSystemWatcher();

            watcher.Created += WatchFolder_Created;
            watcher.Error   += watcher_Error;

            return watcher;
        }

        void watcher_Error( object sender, ErrorEventArgs e )
        {
            Octopus.Core.Logging.Logging.Instance.Log( string.Format("JobManager@watcher_Error() - Exception: {0}", e.GetException().Message), true );
        }

        private bool IsWatchfolderAlreadyAddedAndUnchanged( WatchFolder currentFolder )
        {
            return WatchFolders.Values.Any( watchFolder => currentFolder.ID                     == watchFolder.ID && 
                                                           currentFolder.WorkflowXML.ToString() == watchFolder.WorkflowXML.ToString() &&
                                                           currentFolder.DestinationID          == watchFolder.DestinationID &&
                                                           currentFolder.Filter                 == watchFolder.Filter &&
                                                           currentFolder.IsEnabled              == watchFolder.IsEnabled );
        }

        private Dictionary<string, DateTime> _WatchedFiles = new Dictionary<string, DateTime>();

        void WatchFolder_Created( object sender, FileSystemEventArgs e )
        {
            lock( _WatchedFiles )
            {
                if( IsDoubleEvent( e.FullPath ) )
                    return;

                _WatchedFiles.Add( e.FullPath, DateTime.Now );
            }
            
            while( true )
            {
                try
                {
                    using( new FileStream( e.FullPath, FileMode.Open ) )
                    {
                        WatchFolder watchFolder = WatchFolders[(FileSystemWatcher)sender];

                        // TODO: Proper implementation of place holders, possibly XSLT
                        string jobXml = watchFolder.WorkflowXML.ToString();

                        jobXml = jobXml.Replace("{{FILE_PATH}}", e.FullPath);
                        jobXml = jobXml.Replace("{{FILE_NAME}}", Path.GetFileNameWithoutExtension(e.FullPath));
                        jobXml = jobXml.Replace("{{FILE_NAME_WITH_EXTENSION}}", Path.GetFileName(e.FullPath));

                        AddJob( null, jobXml );
                    }

                    break;
                }
                catch (Exception)
                {
                    // TODO: Implement escape

                    Thread.Sleep(1000);
                    continue;
                }
            }
        }

        private bool IsDoubleEvent( string fullPath )
        {
            IList<string> toDelete = ( from watchedFile in _WatchedFiles
                                       where DateTime.Now.Subtract( watchedFile.Value ).TotalMinutes > 30.0
                                       select watchedFile.Key ).ToList();

            foreach( var key in toDelete )
                _WatchedFiles.Remove( key );

            return _WatchedFiles.ContainsKey( fullPath );
        }

        #endregion

        public void AddJob( int? statusID, string jobXML )
        {
            using( DatabaseDataContext db = new DatabaseDataContext() )
            {
                db.Job_Insert( statusID, jobXML );
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

				SynchronizationTimer.Dispose();

                _SynchronizationThreadBlocker.Set();
                
                if( _SynchronizationThread.ThreadState != ThreadState.Unstarted )
                    _SynchronizationThread.Join();

				_SynchronizationThreadBlocker.Close();
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