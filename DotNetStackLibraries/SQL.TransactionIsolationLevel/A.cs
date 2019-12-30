using System;
using System.Collections.Generic;
using System.Text;

namespace SQL.TransactionIsolationLevel
{
    public class A : Entity
    {
        public string Name { get; set; }

        public bool HasUpdated { get; set; }
    }
}
