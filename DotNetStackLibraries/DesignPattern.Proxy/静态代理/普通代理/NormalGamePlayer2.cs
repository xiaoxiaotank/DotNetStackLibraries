using System;
using System.Collections.Generic;
using System.Text;

namespace DesignPattern.Proxy
{
    class NormalGamePlayer2 : IGamePlayer
    {
        private readonly string _userName;

        private NormalGamePlayer2(string userName)
        {
            _userName = userName;
        }

        public static IGamePlayer Login(IGamePlayer gamePlayerProxy, string userName)
        {
            if(gamePlayerProxy == null)
            {
                throw new ArgumentNullException(nameof(gamePlayerProxy), "代理不能为null");
            }

            var gamePlayer = new NormalGamePlayer2(userName);
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
