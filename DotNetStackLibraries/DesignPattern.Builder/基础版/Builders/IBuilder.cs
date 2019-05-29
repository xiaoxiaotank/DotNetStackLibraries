using System;
using System.Collections.Generic;
using System.Text;

namespace DesignPattern.Builder
{
    interface IBuilder
    {
        void SetHair();

        void SetHead();

        void SetBody();

        void SetFoots();
    }
}
