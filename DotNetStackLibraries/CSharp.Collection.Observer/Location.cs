using System;
using System.Collections.Generic;
using System.Text;

namespace CSharp.Observer
{
    struct Location
    {
        public Location(double latitude, double longitude)
        {
            Latitude = latitude;
            Longitude = longitude;
        }

        public double Latitude { get; private set; }

        public double Longitude { get; private set; }
    }
}
