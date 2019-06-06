using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace DesignPattern.Mediator
{
    class Customer : Colleague<HouseAgency>
    {
        public Customer(HouseAgency houseAgency) : base(houseAgency) 
        {

        }


        public string Name { get; set; }

        public Identity Identity { get; set; }

        public virtual void Say(string message)
        {
            _mediator.Notify(this, message);
        }
    }

    enum Identity
    {
        [Description("房东")]
        Landloard,
        [Description("租户")]
        Tenant
    }
}
