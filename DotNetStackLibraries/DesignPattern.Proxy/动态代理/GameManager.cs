using System;
using System.Collections.Generic;
using System.Text;

namespace DesignPattern.Proxy
{
    class GameManager : IGameManager
    {
        public void Nofity()
        {
            Console.WriteLine("游戏管理员发送通知：我是渣渣辉，是兄弟就来砍我！");
        }
    }
}
