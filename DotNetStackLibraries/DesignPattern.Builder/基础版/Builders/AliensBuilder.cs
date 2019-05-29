using System;
using System.Collections.Generic;
using System.Text;

namespace DesignPattern.Builder
{
    class AliensBuilder : IBuilder
    {
        public void SetHair()
        {
            Console.WriteLine("外星人的头发");
        }

        public void SetHead()
        {
            Console.WriteLine("外星人的头");
        }

        public void SetBody()
        {
            Console.WriteLine("外星人的身体");
        }

        public void SetFoots()
        {
            Console.WriteLine("外星人的脚");
        }
    }
}
