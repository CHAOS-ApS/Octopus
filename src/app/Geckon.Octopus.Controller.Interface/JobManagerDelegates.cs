using System.Collections.Generic;

namespace Geckon.Octopus.Controller.Interface
{
    public class JobManagerDelegates
    {
        public delegate void JobManagerSynchronized( object sender, CollectionEventArgs<IList<IJob>> eventArgs );
    }
}