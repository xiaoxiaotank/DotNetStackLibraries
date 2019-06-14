using System;
using System.Collections.Generic;
using System.Text;

namespace DesignPattern.Builder
{
    class CatBuilder : IAnimalBuilder<Cat>
    {
        private readonly Cat _cat = new Cat();

        public void SetHead()
        {
            _cat.Head = "猫的头";
        }

        public void SetBody()
        {
            _cat.Body = "猫的身体";
        }

        public void SetFoots()
        {
            _cat.Foots = "猫的脚";
        }

        public Cat GetAnimal()
        {
            return _cat;
        }
    }
}
