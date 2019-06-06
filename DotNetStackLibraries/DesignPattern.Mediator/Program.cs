using System;

namespace DesignPattern.Mediator
{
    /// <summary>
    /// 中介者模式与代理模式区别：
    ///     1.中介者模式：
    ///         多对多，即多个对象可以通过中介与多个对象进行通信
    ///         使用上来说，是将中介传入对象
    ///         操作上来说，还是对象来执行动作
    ///     2.代理模式：
    ///         一对多，只能代理单一对象，即一个代理只能代表一个对象与其他多个对象进行通信
    ///         使用上来说，是将对象传入代理
    ///         操作上来说，是通过代理来执行动作
    /// </summary>
    class Program
    {
        static void Main(string[] args)
        {
            var houseAgency = new HouseAgency();
            var landlord = new Landlord(houseAgency)
            {
                Name = "小张",
                Identity = Identity.Landloard
            };
            var tenant1 = new Tenant(houseAgency)
            {
                Name = "小贾",
                Identity = Identity.Tenant
            };
            var tenant2 = new Tenant(houseAgency)
            {
                Name = "小赵",
                Identity = Identity.Tenant
            };

            tenant1.Say("谁有房子出租？");
            landlord.Say("我这有！");
            tenant1.Say("去看下房吧。");
            tenant2.Say("等等，我也要租房！");
            tenant1.Say("我先来的！");
            landlord.Say("一起去吧，请！");
            tenant1.Say("走吧");
            tenant2.Say("走吧");

            Console.ReadKey();
        }
    }
}
