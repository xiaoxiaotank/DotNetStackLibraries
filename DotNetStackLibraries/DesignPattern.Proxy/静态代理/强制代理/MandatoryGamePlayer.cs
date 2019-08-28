using System;
using System.Collections.Generic;
using System.Text;

namespace DesignPattern.Proxy
{
    class MandatoryGamePlayer : IGamePlayer
    {
        private readonly string _userName;

        private MandatoryGamePlayer(string userName)
        {
            _userName = userName;
        }

        public static IGamePlayer Login(string userName)
        {
            var gamePlayer = new MandatoryGamePlayer(userName);
            Console.WriteLine($"{userName} 登录游戏成功！");

            return new MandatoryGamePlayerProxy(gamePlayer);
        }

        public void KillBoss()
        {
            Console.WriteLine($"{_userName} 杀了一只Boss！"); ;
        }


        public void Upgrade()
        {
            Console.WriteLine($"{_userName} 升级了！"); ;
        }
    }
}
