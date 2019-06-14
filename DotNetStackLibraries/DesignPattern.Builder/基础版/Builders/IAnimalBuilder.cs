using System;
using System.Collections.Generic;
using System.Text;

namespace DesignPattern.Builder
{
    interface IAnimalBuilder<TAnimal> : IAnimalBuilder where TAnimal : Animal 
    {
        TAnimal GetAnimal();
    }

    interface IAnimalBuilder
    {
        void SetHead();

        void SetBody();

        void SetFoots();
    }

    abstract class Animal
    {
        public string Head { get; set; }

        public string Body { get; set; }

        public string Foots { get; set; }

        public abstract void Display();
    }

    class Dog : Animal
    {
        public override void Display()
        {
            Console.WriteLine("-------------- 狗的基本信息 -------------------");
            Console.WriteLine($"头：{Head}");
            Console.WriteLine($"身体：{Body}");
            Console.WriteLine($"脚：{Body}");
        }
    }

    class Cat : Animal
    {
        public override void Display()
        {
            Console.WriteLine("-------------- 猫的基本信息 -------------------");
            Console.WriteLine($"头：{Head}");
            Console.WriteLine($"身体：{Body}");
            Console.WriteLine($"脚：{Body}");
        }
    }
}
