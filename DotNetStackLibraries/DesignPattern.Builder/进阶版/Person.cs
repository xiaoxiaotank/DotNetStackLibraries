using System;
using System.Collections.Generic;
using System.Text;

namespace DesignPattern.Builder
{
    class Person : IPerson
    {
        public string Name { get; set; }

        public int Gender { get; set; }

        public int Age { get; set; }
    }
}
