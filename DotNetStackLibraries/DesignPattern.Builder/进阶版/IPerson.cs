using System;
using System.Collections.Generic;
using System.Text;

namespace DesignPattern.Builder
{
    public interface IPerson
    {
        string Name { get; }

        int Gender { get; }

        int Age { get; }

    }
}
