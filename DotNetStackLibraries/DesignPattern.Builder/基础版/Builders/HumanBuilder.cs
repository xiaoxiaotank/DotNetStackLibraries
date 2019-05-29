using System;
using System.Collections.Generic;
using System.Text;

namespace DesignPattern.Builder
{
    class HumanBuilder : IBuilder
    {
        public void SetHair()
        {
            Console.WriteLine("人类的头发");
        }

        public void SetHead()
        {
            Console.WriteLine("人类的头");
        }

        public void SetBody()
        {
            Console.WriteLine("人类的身体");
        }

        public void SetFoots()
        {
            Console.WriteLine("人类的脚");
        }
        
    }
}
