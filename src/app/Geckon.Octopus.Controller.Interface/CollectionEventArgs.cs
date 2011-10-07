using System;

namespace Geckon.Octopus.Controller.Interface
{
    public class CollectionEventArgs<T> : EventArgs where T : System.Collections.IEnumerable
    {
        #region Fields

        private T _Collection;

        #endregion
        #region Properties

        public T Collection
        {
            get { return _Collection; }
            set { _Collection = value; }
        }

        #endregion
        #region Construction

        public CollectionEventArgs()
        {
        }

        public CollectionEventArgs( T collection )
        {
            _Collection = collection;
        }

        #endregion
    }
}