using System;
using System.Collections.Generic;
using System.Text;

namespace DesignPattern.Builder
{
    public class PersonBuilderHelper
    {
        public static IPersonBuilder CreatePersonBuilder()
        {
            return new PersonBuilder();
        }
    }

    class PersonBuilder : IPersonBuilder
    {
        private readonly Person _person = new Person();

        public IPerson Build()
        {
            return _person;
        }

        public IPersonBuilder SetName(string name)
        {
            _person.Name = name;
            return this;
        }

        public IPersonBuilder SetGender(int gender)
        {
            _person.Gender = gender;
            return this;
        }

        public IPersonBuilder SetAge(int age)
        {
            _person.Age = age;
            return this;
        }      
    }
}
