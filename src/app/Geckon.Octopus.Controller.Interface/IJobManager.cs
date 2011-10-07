using System;
using System.Collections.Generic;
using Geckon.Events;

namespace Geckon.Octopus.Controller.Interface
{
    public interface IJobManager : IEnumerable<IJob>, IDisposable
    {
        void SynchronizeOnce();
		void BeginSynchronize(int synchronizationFrequency);

        int Count { get; }
        bool ContainsKey(int id);

        event JobManagerDelegates.JobManagerSynchronized SyncCompleted;
        event EventHandlers.ObjectEventHandler<IJob> JobAddedToQueue;

        void AddJob( int? statusID, string jobXml );
    }
}