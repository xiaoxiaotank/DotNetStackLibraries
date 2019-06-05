using System;
using System.Collections.Generic;
using System.Text;

namespace Tools.AutoMapper.ConsoleEx
{
    static class DisplayExtensions
    {
        public static void Show(this UserDto dto)
        {
            Console.WriteLine($"Id:{dto.Id}\nName:{dto.Name}\nAge:{dto.Age}\n" +
                $"Creation Date:{dto.CreationDate}\nCreation Hour:{dto.CreationHour}\nCreation Minute:{dto.CreationMinute}\n" +
                $"Birthday:{dto.Birthday}\nDeletion Time:{dto.DeletionTime}\nTotal Money:{dto.TotalMoney}\n" +
                $"Value:{dto.ValueStr}\nGender:{dto.Gender}\nHeight:{dto.Height}\nFirst Name:{dto.FirstName}");
        }

        public static void Show(this RoleDto dto)
        {
            Console.WriteLine($"Id:{dto.Id}\nId2:{dto.Id2}\nName:{dto.Name}\nPriority: {dto.Priority}");
        }


        public static void Show(this DotNetDto dto)
        {
            Console.WriteLine($"Id:{dto.Id}\nCompany Name:{dto.CompanyName}\nCompany Short Name:{dto.CompanyShortName}\n" +
                $"Creation Time:{dto.CreationTime}\nVersion:{dto.Version}");
        }

        public static void Show(this MenuDto dto)
        {
            Console.WriteLine($"Id:{dto.Id}\nName:{dto.Title}");
        }

        public static void Show<T>(this GenericDto<T> dto)
        {
            Console.WriteLine($"Value:{dto.Value}");
        }

    }
}
