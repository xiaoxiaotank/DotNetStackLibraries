using AutoMapper;
using AutoMapper.Configuration.Annotations;

namespace Tools.AutoMapper.ConsoleEx
{
    class Role
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public int Priority { get; set; }
    }

    /// <summary>
    /// 使用特性映射 相当于CreateMap<Role RoleDto>
    /// </summary>
    [AutoMap(typeof(Role), ReverseMap = true)]
    class RoleDto
    {
        [Ignore]
        public int Id { get; set; }

        [SourceMember(nameof(Role.Id))]
        public int Id2 { get; set; }

        public string Name { get; set; }

        public uint Priority { get; set; }
    }
}
