using System;

namespace DesignPattern.Proxy
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("-------普通代理-------");
            #region 普通代理

            //通过传入不同的GamePlayer，来代理多种GamePlayer
            var normalGamePlayer1 = NormalGamePlayer1.Login("jjj");
            var normalGamePlayerProxy1 = new NormalGamePlayerProxy1(normalGamePlayer1);
            normalGamePlayerProxy1.KillBoss();
            normalGamePlayerProxy1.Upgrade();

            //无需关心具体的GamePlayer，直接获取代理
            var normalGamePlayerProxy2 = new NormalGamePlayerProxy2("jjj");
            normalGamePlayerProxy2.KillBoss();
            normalGamePlayerProxy2.Upgrade();

            #endregion
            Console.WriteLine("-------强制代理-------");
            #region 强制代理

            //无需关心具体的代理，在具体GamePlayer已知的前提下，通过获取代理进行操作
            var mandatoryGamePlayerProxy = MandatoryGamePlayer.Login("jjj");
            mandatoryGamePlayerProxy.KillBoss();
            mandatoryGamePlayerProxy.Upgrade();

            #endregion
            Console.WriteLine("-------个性代理-------");
            #region 个性代理

            //通过IProxy接口未代理提供额外的功能
            var personalityGamePlayerProxy = new PersonalityGamePlayerProxy(normalGamePlayer1);
            personalityGamePlayerProxy.KillBoss();
            personalityGamePlayerProxy.Upgrade();
            personalityGamePlayerProxy.Count();

            #endregion
            Console.WriteLine("-------动态代理-------");
            #region 动态代理
            //无需手动创建代理类，动态代理可以根据代理对象动态创建相应的代理
            var gamePlayerProxy = LoggingProxy<IGamePlayer>.Create(
                NormalGamePlayer1.Login("jjj"),
                methodName => Console.WriteLine($"AOP:调用IGamePlayer的方法{methodName}"));
            gamePlayerProxy.KillBoss();
            gamePlayerProxy.Upgrade();

            var gameManagerProxy = LoggingProxy<IGameManager>.Create(
                new GameManager(),
                methodName => Console.WriteLine($"AOP:调用IGameManager的方法{methodName}"));
            gameManagerProxy.Nofity();

            #endregion

            Console.ReadKey();
        }
    }
}
