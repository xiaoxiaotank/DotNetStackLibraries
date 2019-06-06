using System;

namespace DesignPattern.Mediator
{
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
