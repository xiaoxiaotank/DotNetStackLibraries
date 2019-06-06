using System;
using System.Collections.Generic;
using System.Text;

namespace DesignPattern.Mediator
{
    abstract class Mediator
    {
        protected readonly List<Colleague> _colleagueList = new List<Colleague>();

        public virtual void AddColleague(Colleague colleague)
        {
            _colleagueList.Add(colleague);
        }
    }
}
