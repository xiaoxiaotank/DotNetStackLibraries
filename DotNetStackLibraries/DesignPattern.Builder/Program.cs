using System;

namespace DesignPattern.Builder
{
    class Program
    {
        static void Main(string[] args)
        {
            #region 基础版
            var humanDirector = new Director(new HumanBuilder());
            var aliensDirector = new Director(new AliensBuilder());

            humanDirector.Build();
            aliensDirector.Build();
            #endregion

            #region 进阶版
            var personBuilder = PersonBuilderHelper.CreatePersonBuilder();
            var person = personBuilder.SetAge(1).SetName("jjj").Build();
            Console.WriteLine($"{person.Name},{person.Gender},{person.Age}");
            #endregion

            Console.ReadKey();
        }
    }
}
