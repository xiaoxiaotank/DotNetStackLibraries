using System;
using System.Collections.Generic;
using System.Text;

namespace DesignPattern.Mediator
{
    /// <summary>
    /// 租户
    /// </summary>
    class Tenant : Customer
    {
        public Tenant(HouseAgency houseAgency) : base(houseAgency)
        {

        }
    }
}
