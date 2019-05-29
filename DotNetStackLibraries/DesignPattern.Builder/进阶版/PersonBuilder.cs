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
        private string _name;
        private int _gender;
        private int _age;

        public IPerson Build()
        {
            return new Person()
            {
                Name = _name,
                Gender = _gender,
                Age = _age
            };
        }

        public IPersonBuilder SetName(string name)
        {
            _name = name;
            return this;
        }

        public IPersonBuilder SetGender(int gender)
        {
            _gender = gender;
            return this;
        }

        public IPersonBuilder SetAge(int age)
        {
            _age = age;
            return this;
        }      
    }
}
