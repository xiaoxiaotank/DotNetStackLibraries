using System;

namespace Tools.AutoMapper.ConsoleEx
{
    class User
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public int Age { get; set; }

        public DateTime CreationTime { get; set; }

        public DateTime Birthday { get; set; }

        public string DeletionTime { get; set; }

        public int Cash { get; set; }

        public int Deposit { get; set; }

        public decimal Value { get; set; }

        public Gender? Gender { get; set; }

        public int 身高 { get; set; }

        public string Pre_FirstName { get; set; }
    }

    class UserDto
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public int Age { get; set; }

        public DateTime CreationDate { get; set; }

        public int CreationHour { get; set; }

        public int CreationMinute { get; set; }

        public string Birthday { get; set; }

        public DateTime DeletionTime { get; set; }

        public int TotalMoney { get; set; }

        public string ValueStr { get; set; }

        public Gender? Gender { get; set; }

        public int Height { get; set; }

        public string FirstName { get; set; }
    }

    enum Gender
    {
        Female,
        Male,
        Unknown
    }
}
