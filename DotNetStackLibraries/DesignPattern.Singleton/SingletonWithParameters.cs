using System;
using System.Collections.Generic;
using System.Text;

namespace DesignPattern.Singleton
{
    class SingletonWithParameters
    {
        private static readonly object _syncLock = new object();
        private static SingletonWithParameters _instance;

        private SingletonWithParameters() { }

        public int X { get; private set; }

        public int Y { get; private set; }

        public static SingletonWithParameters GetInstance(int x, int y)
        {
            if (_instance == null)
            {
                lock (_syncLock)
                {
                    if(_instance == null)
                    {
                        _instance = new SingletonWithParameters()
                        {
                            X = x,
                            Y = y
                        };
                    }
                    else
                    {
                        Initialize();
                    }
                }
            }
            else
            {
                Initialize();
            }

            return _instance;

            void Initialize()
            {
                _instance.X = x;
                _instance.Y = y;
            }
        }
    }
}
