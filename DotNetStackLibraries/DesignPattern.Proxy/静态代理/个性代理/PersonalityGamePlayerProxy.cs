using System;
using System.Collections.Generic;
using System.Text;

namespace DesignPattern.Proxy
{
    class PersonalityGamePlayerProxy : IGamePlayer, IProxy
    {
        private readonly IGamePlayer _gamePlayer;

        public PersonalityGamePlayerProxy(IGamePlayer gamePlayer)
        {
            _gamePlayer = gamePlayer;
        }

        public void KillBoss()
        {
            _gamePlayer.KillBoss();
        }

        public void Upgrade()
        {
            _gamePlayer.Upgrade();
        }

        public void Count()
        {
            Console.WriteLine("收费标准： 100元/小时");
        }
    }
}
