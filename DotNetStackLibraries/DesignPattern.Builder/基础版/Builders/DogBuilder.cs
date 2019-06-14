using System;
using System.Collections.Generic;
using System.Text;

namespace DesignPattern.Builder
{
    class DogBuilder : IAnimalBuilder<Dog>
    {
        private readonly Dog _dog = new Dog();

        public void SetHead()
        {
            _dog.Head = "狗的头";
        }

        public void SetBody()
        {
            _dog.Body = "狗的身体";
        }

        public void SetFoots()
        {
            _dog.Foots = "狗的脚";
        }

        public Dog GetAnimal()
        {
            return _dog;
        }
    }
}
