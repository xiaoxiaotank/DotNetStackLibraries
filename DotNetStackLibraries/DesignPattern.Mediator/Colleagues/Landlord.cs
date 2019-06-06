using System;
using System.Collections.Generic;
using System.Text;

namespace DesignPattern.Mediator
{
    /// <summary>
    /// 房东
    /// </summary>
    class Landlord : Customer
    {
        public Landlord(HouseAgency houseAgency): base(houseAgency)
        {

        }
    }
}
