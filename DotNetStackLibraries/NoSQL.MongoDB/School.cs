using System;
using System.Collections.Generic;
using System.Text;

namespace NoSQL.MongoDB
{
    class School
    {
        public string Name { get; set; }

        public School(string name)
        {
            Name = name;
        }
    }
}
