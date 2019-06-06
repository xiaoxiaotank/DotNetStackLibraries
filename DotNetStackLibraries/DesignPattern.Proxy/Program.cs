using System;

namespace DesignPattern.Proxy
{
    /// <summary>
    /// 代理模式与中介者模式区别：
    ///     1.代理模式：
    ///         一对多，只能代理单一对象，即一个代理只能代表一个对象与其他多个对象进行通信
    ///         使用上来说，是将对象传入代理
    ///         操作上来说，是通过代理来执行动作
    ///     2.中介者模式：
    ///         多对多，即多个对象可以通过中介与多个对象进行通信
    ///         使用上来说，是将中介传入对象
    ///         操作上来说，还是对象来执行动作
    /// </summary>
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
