using System;
using System.Collections.Generic;
using System.Text;

namespace DesignPattern.Builder
{
    class Director
    {
        private readonly IAnimalBuilder _builder;

        public Director(IAnimalBuilder builder)
        {
            _builder = builder;
        }

        public void Construct()
        {
            _builder.SetHead();
            _builder.SetBody();
            _builder.SetFoots();
        }
    }
}
