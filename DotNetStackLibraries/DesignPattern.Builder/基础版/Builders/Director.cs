using System;
using System.Collections.Generic;
using System.Text;

namespace DesignPattern.Builder
{
    class Director
    {
        private readonly IBuilder _builder;

        public Director(IBuilder builder)
        {
            _builder = builder;
        }

        public void Build()
        {
            _builder.SetHair();
            _builder.SetHead();
            _builder.SetBody();
            _builder.SetFoots();
        }
    }
}
