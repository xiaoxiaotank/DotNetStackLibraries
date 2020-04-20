using System;
using System.Collections.Generic;
using System.Text;

namespace CSharp.Observer
{
    /// <summary>
    /// 观察者
    /// </summary>
    class LocationReporter : IObserver<Location>
    {
        private IDisposable _unsubscriber;

        public LocationReporter(string name)
        {
            Name = name;
        }

        public string Name { get; }

        public void OnCompleted()
        {
            Console.WriteLine($"Completed:{Name}");
            Unsubscribe();
        }

        public void OnError(Exception ex)
        {
            Console.WriteLine($"Error:{ex.Message}");
        }

        public void OnNext(Location value)
        {
            Console.WriteLine($"Current location:{Name},Latitude:{value.Latitude},Longtitude:{value.Longitude}");
        }

        public virtual void Subscribe(IObservable<Location> provider)
        {
            if(provider != null)
            {
                _unsubscriber = provider.Subscribe(this);
            }
        }

        public virtual void Unsubscribe()
        {
            _unsubscriber?.Dispose();
        }
    }
}
