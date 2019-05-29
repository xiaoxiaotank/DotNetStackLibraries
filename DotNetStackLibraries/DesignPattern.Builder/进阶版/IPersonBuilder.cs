using System;
using System.Collections.Generic;
using System.Text;

namespace DesignPattern.Builder
{
    public interface IPersonBuilder
    {
        IPerson Build();

        IPersonBuilder SetName(string name);

        IPersonBuilder SetGender(int gender);

        IPersonBuilder SetAge(int age);
    }
}
