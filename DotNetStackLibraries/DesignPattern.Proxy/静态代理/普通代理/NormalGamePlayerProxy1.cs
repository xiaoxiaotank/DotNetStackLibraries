using System;
using System.Collections.Generic;
using System.Text;

namespace DesignPattern.Proxy
{
    class NormalGamePlayerProxy1 : IGamePlayer
    {
        private readonly IGamePlayer _gamePlayer;

        public NormalGamePlayerProxy1(IGamePlayer gamePlayer)
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
    }
}
