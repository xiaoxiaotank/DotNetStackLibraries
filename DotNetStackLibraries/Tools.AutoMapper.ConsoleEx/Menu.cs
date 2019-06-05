using System;
using System.Collections.Generic;
using System.Text;

namespace Tools.AutoMapper.ConsoleEx
{
    class Menu
    {
        public int Id { get; set; }

        public string Name { get; set; }
    }

    class MenuDto
    {
        //public MenuDto(string name)
        //{
        //    Name = name;
        //}

        public MenuDto(string title)
        {
            Title = title;
        }

        public int Id { get; set; }

        public string Title { get; }
    }
}
