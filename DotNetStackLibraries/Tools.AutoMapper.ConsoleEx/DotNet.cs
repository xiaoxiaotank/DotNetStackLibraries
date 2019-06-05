using System;
using System.Collections.Generic;
using System.Text;

namespace Tools.AutoMapper.ConsoleEx
{
    class DotNet
    {
        public int Id { get; set; }

        public Company Company { get; set; }

        public Time Time { get; set; }

        /// <summary>
        /// 以Get开头或直接用Version命名
        /// </summary>
        /// <returns></returns>
        public string GetVersion()
        {
            return "Core";
        }
    }

    class Company
    {
        public string Name { get; set; }

        public string ShortName { get; set; }
    }

    public class Time
    {
        public DateTime CreationTime { get; set; }
    }

    class DotNetDto
    {
        public int Id { get; set; }
        
        /// <summary>
        /// DotNet的变量名Company + Company的变量名Name
        /// </summary>
        public string CompanyName { get; set; }

        public string CompanyShortName { get; set; }

        /// <summary>
        /// 没有使用变量名拼接，需要使用IncludeMembers
        /// </summary>
        public DateTime CreationTime { get; set; }

        public string Version { get; set; }
    }
}
