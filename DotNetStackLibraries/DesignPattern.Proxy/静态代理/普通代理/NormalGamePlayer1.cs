using System;
using System.Collections.Generic;
using System.Text;

namespace DesignPattern.Proxy
{
    class NormalGamePlayer1 : IGamePlayer
    {
        private readonly string _userName;

        private NormalGamePlayer1(string userName)
        {
            _userName = userName;
        }

        public static IGamePlayer Login(string userName)
        {
            var gamePlayer = new NormalGamePlayer1(userName);
            Console.WriteLine($"{userName} 登录游戏成功！");

            return gamePlayer;
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
