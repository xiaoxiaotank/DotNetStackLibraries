using System;

namespace DesignPattern.Builder
{
    class Program
    {
        static void Main(string[] args)
        {
            #region 经典版
            var dogBuilder = new DogBuilder();
            var dogDirector = new Director(dogBuilder);
            dogDirector.Construct();
            dogBuilder.GetAnimal().Display();

            var catBuilder = new CatBuilder();
            var catDirector = new Director(catBuilder);
            catDirector.Construct();
            catBuilder.GetAnimal().Display();
            #endregion

            #region 进阶版
            var person = PersonBuilderHelper.CreatePersonBuilder()
                .SetAge(20)
                .SetName("jjj")
                .Build();
            Console.WriteLine($"{person.Name},{person.Gender},{person.Age}");
            #endregion

            Console.ReadKey();
        }
    }
}
