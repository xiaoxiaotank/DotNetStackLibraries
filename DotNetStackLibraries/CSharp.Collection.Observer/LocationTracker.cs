using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CSharp.Observer
{
    /// <summary>
    /// 被观察者
    /// </summary>
    class LocationTracker : IObservable<Location>
    {
        private List<IObserver<Location>> _observerList;

        public LocationTracker()
        {
            _observerList = new List<IObserver<Location>>();
        }

        public IDisposable Subscribe(IObserver<Location> observer)
        {
            if (!_observerList.Contains(observer))
            {
                _observerList.Add(observer);
            }

            return new Unsubscriber(_observerList, observer);
        }

        public void TrackLocation(Location? location)
        {
            foreach (var observer in _observerList)
            {
                if (location.HasValue)
                {
                    observer.OnNext(location.Value);
                }
                else
                {
                    observer.OnError(new LocationUnknownException());
                }
            }
        }

        public void EndTransmission()
        {
            for (int i = 0; i < _observerList.Count; i++)
            {
                _observerList[i].OnCompleted();
            }

            _observerList.Clear();
        }

        private class Unsubscriber : IDisposable
        {
            private readonly List<IObserver<Location>> _observerList;
            private readonly IObserver<Location> _observer;

            public Unsubscriber(List<IObserver<Location>> observerList, IObserver<Location> observer)
            {
                _observerList = observerList;
                _observer = observer;
            }

            public void Dispose()
            {
                if(_observerList.Contains(_observer))
                {
                    _observerList.Remove(_observer);
                }
            }
        }
    }

    public class LocationUnknownException : Exception
    {
        internal LocationUnknownException()
        { }
    }
}
