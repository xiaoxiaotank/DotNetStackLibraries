using System;
using System.Collections.Generic;
using System.Text;

namespace DesignPattern.Mediator
{
    abstract class Colleague<TMediator> : Colleague where TMediator : Mediator
    {
        protected readonly TMediator _mediator;

        public Colleague(TMediator mediator) : base(mediator)
        {
            _mediator = mediator;
        }
    }

    abstract class Colleague
    {        
        public Colleague(Mediator mediator)
        {
            mediator.AddColleague(this);
        }
    }
}
