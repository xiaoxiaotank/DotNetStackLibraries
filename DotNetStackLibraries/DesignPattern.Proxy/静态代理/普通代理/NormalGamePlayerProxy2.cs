using System;
using System.Collections.Generic;
using System.Text;

namespace DesignPattern.Proxy
{
    class NormalGamePlayerProxy2 : IGamePlayer
    {
        private readonly IGamePlayer _gamePlayer;

        public NormalGamePlayerProxy2(string userName)
        {
            _gamePlayer = NormalGamePlayer2.Login(this, userName);
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
