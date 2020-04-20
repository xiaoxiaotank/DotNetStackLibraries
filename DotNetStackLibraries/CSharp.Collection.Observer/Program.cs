using System;

namespace CSharp.Observer
{
    class Program
    {
        static void Main(string[] args)
        {
            var provider = new LocationTracker();

            var reporter1 = new LocationReporter("FixedGPS");
            reporter1.Subscribe(provider);

            var reporter2 = new LocationReporter("MobileGPS");
            reporter2.Subscribe(provider);

            provider.TrackLocation(new Location(47, -122));
            reporter1.Unsubscribe();
            provider.TrackLocation(new Location(50, -100));
            provider.TrackLocation(null);
            provider.EndTransmission();

            Console.ReadKey();
        }
    }
}
