using System;
using System.Collections.Generic;
using System.Text;

namespace DesignPattern.Singleton
{
    class Singleton
    {
        private readonly static Lazy<Singleton> _instanceLazy = new Lazy<Singleton>(() => new Singleton(), true);

        private Singleton()
        {

        }

        public static Singleton Instance => _instanceLazy.Value;
    }
}
