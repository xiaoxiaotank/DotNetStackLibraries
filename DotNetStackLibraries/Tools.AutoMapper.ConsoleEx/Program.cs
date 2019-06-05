using AutoMapper;
using System;
using System.Collections.Generic;
using Tools.AutoMapper.ConsoleEx.Converters;
using Tools.AutoMapper.ConsoleEx.ValueResolvers;
using Tools.AutoMapper.ConsoleEx.MappingActions;
using System.Linq;

namespace Tools.AutoMapper.ConsoleEx
{
    class Program
    {
        #region static readonly fileds
        private static readonly User _user = new User()
        {
            Id = 1,
            Name = "jjj",
            Age = 22,
            CreationTime = DateTime.Now,
            Birthday = DateTime.Now,
            DeletionTime = "2019-1-1 12:11:11",
            Cash = 11,
            Deposit = 89,
            Value = 10,
            身高 = 180,
            Pre_FirstName = "first name"
        };

        private static readonly Role _role = new Role()
        {
            Id = 11,
            Name = "admin",
            Priority = -10
        };

        private static readonly DotNet _dotNet = new DotNet()
        {
            Id = 1,
            Company = new Company()
            {
                Name = "Microsoft",
                ShortName = "ms"
            },
            Time = new Time()
            {
                CreationTime = DateTime.Now
            }
        };

        private static readonly Menu _menu = new Menu()
        {
            Id = 100,
            Name = "用户管理"
        };

        private static readonly Generic<int> _generic = new Generic<int>()
        {
            Value = 10
        }; 
        #endregion

        static void Main(string[] args)
        {
            //StaticAPIExample();
            //InstanceAPIExample();
            //Flattern();
            //Projection();
            //InlineMapping();
            //ListAndArray();
            //TypeConverter();
            //ValueResolverAndConverter();
            //ValueTransformer();
            //NullSubstitution();
            //MappingAction();
            //ReplacingSourceMemberName();
            //RecognizePrefixes();
            //AttributeMap();
            //ConstructorMap();
            //ConditionalMap();
            OpenGenerics();

            Console.ReadKey();
        }

        /// <summary>
        /// 使用静态API配置Mapper
        /// </summary>
        static void StaticAPIExample()
        {
            ConfigMapperByStaticAPI();

            //指明源类型
            //var dto = Mapper.Map<User, UserDto>(_user);
            //源类型推断
            var dto = Mapper.Map<UserDto>(_user);
            dto.Show();
        }

        /// <summary>
        /// 使用实例API配置Mapper
        /// </summary>
        static void InstanceAPIExample()
        {
            var mapper = ConfigMapperByInstanceAPI();
            //var dto = mapper.Map<User, UserDto>(_user);
            var dto = mapper.Map<UserDto>(_user);
            dto.Show();
        }

        /// <summary>
        /// 扁平化
        /// </summary>
        static void Flattern()
        {
            Mapper.Initialize(config =>
            {
                //将属性Time中的CreationTime映射到Dto中的CreationTime
                config.CreateMap<DotNet, DotNetDto>().IncludeMembers(s => s.Time);
                //参数默认值为MemberList.Destination，表示目标模型所有成员都需要被映射
                config.CreateMap<Time, DotNetDto>(MemberList.None);
            });

            var dto = Mapper.Map<DotNetDto>(_dotNet);
            dto.Show();
        }
        
        /// <summary>
        /// 投影
        /// </summary>
        static void Projection()
        {
            Mapper.Initialize(config => config.CreateMap<User, UserDto>()
                .ForMember(d => d.CreationDate, options => options.MapFrom(s => s.CreationTime.Date))
                .ForMember(d => d.CreationHour, options => options.MapFrom(s => s.CreationTime.Hour))
                .ForMember(d => d.CreationMinute, options => options.MapFrom(s => s.CreationTime.Minute)));

            var dto = Mapper.Map<UserDto>(_user);
            dto.Show();
        }

        /// <summary>
        /// 内联映射
        /// </summary>
        static void InlineMapping()
        {
            //var mapper = ConfigMapperByInstanceAPI();
            //var dto = mapper.Map<UserDto>(_user, options => ));
        }

        /// <summary>
        /// List和Array的映射处理
        /// </summary>
        static void ListAndArray()
        {
            var mapper = new MapperConfiguration(config =>
            {
#warning 该属性不起作用，默认为false
                //config.AllowNullCollections = null;
                config.CreateMap<User, UserDto>();
            }).CreateMapper();

            List<User> userList = null;
            //var userList = new List<User>() { _user, _user };
            //默认的返回的是空的集合
            var dtos = mapper.Map<UserDto[]>(userList);
            Array.ForEach(dtos, dto => dto.Show());
        }

        /// <summary>
        /// 类型转换器
        /// </summary>
        static void TypeConverter()
        {
            var mapperConfig = new MapperConfiguration(config => 
            {
                //将DateTime转为string
                config.CreateMap<DateTime, string>().ConvertUsing(s => s.ToString("yyyy年MM月dd日"));
                config.CreateMap<string, DateTime>().ConvertUsing<DateTimeTypeConverter>();
                config.CreateMap<User, UserDto>();
            });
            var mapper = mapperConfig.CreateMapper();

            mapper.Map<UserDto>(_user).Show();
        }

        /// <summary>
        /// 值解析器
        /// </summary>
        static void ValueResolverAndConverter()
        {
            var mapperConfig = new MapperConfiguration(config =>
            {
                config.CreateMap<User, UserDto>()
                    //通过计算得到TotalMoney
                    .ForMember(d => d.TotalMoney, options => options.MapFrom<UserTotalMoneyValueResolver>())
                    //ForPath与ForMember类似，相当于Task.Run与Task.Factory.StartNew的关系
                    //.ForPath(d => d.TotalMoney, options => options.MapFrom(s => s.Cash + s.Deposit));
                    //对属性Value的映射值格式进行转换
                    .ForMember(d => d.ValueStr, options => options.ConvertUsing<CurrencyFormatterValueConverters, decimal>(s => s.Value));
            });
            var mapper = mapperConfig.CreateMapper();
            mapper.Map<UserDto>(_user).Show();
        }

        /// <summary>
        /// 值变形器
        /// </summary>
        static void ValueTransformer()
        {
            var mapperConfig = new MapperConfiguration(config =>
            {   
                //为所有字符串后方加上“!!!”
                config.ValueTransformers.Add<string>(s => s + "!!!");
                config.CreateMap<User, UserDto>();
            });
            var mapper = mapperConfig.CreateMapper();
            mapper.Map<UserDto>(_user).Show();
        }

        /// <summary>
        /// null替换
        /// </summary>
        static void NullSubstitution()
        {
            var mapperConfig = new MapperConfiguration(config =>
            {
                config.CreateMap<User, UserDto>()
                    //性别为null时替换为未知
                    .ForMember(d => d.Gender, options => options.NullSubstitute(Gender.Unknown));
            });
            var mapper = mapperConfig.CreateMapper();
            mapper.Map<UserDto>(_user).Show();
        }

        /// <summary>
        /// 映射Action
        /// </summary>
        static void MappingAction()
        {
            //不建议在全局中配置MappingAction，因为常常会导致开发人员找不到问题所在
            //var mapperConfig = new MapperConfiguration(config =>
            //{
            //    config.CreateMap<User, UserDto>()
            //        //在映射前对源对象的Name值进行更改，也可以更改目标对象
            //        .BeforeMap<DecorateUserNameMappingAction>()
            //        //在映射后对目标对象的DeletionTime值进行更改，也可以更改源对象
            //        .AfterMap((s, d) => d.DeletionTime = DateTime.MaxValue);
            //});
            //var mapper = mapperConfig.CreateMapper();
            //mapper.Map<UserDto>(_user).Show();


            var mapperConfig = new MapperConfiguration(config =>
            {
                config.CreateMap<User, UserDto>();
            });
            var mapper = mapperConfig.CreateMapper();
            mapper.Map<User, UserDto>(_user, options =>
            {
                //建议在此处配置MappingAction
                options.BeforeMap((s, d) => s.Name = $"[{s.Name}]");
                options.AfterMap((s, d) => d.DeletionTime = DateTime.MaxValue);
            }).Show();
        }

        /// <summary>
        /// 替换源属性名
        /// </summary>
        static void ReplacingSourceMemberName()
        {
            var mapperConfig = new MapperConfiguration(config =>
            {
                //将“身高”映射到“Height”
                config.ReplaceMemberName("身高", "Height");
                config.CreateMap<User, UserDto>();
            });
            var mapper = mapperConfig.CreateMapper();
            mapper.Map<UserDto>(_user).Show();
        }

        /// <summary>
        /// 识别前缀/后缀
        /// 通过将前缀识别出来，进行不包含前缀的映射
        /// </summary>
        static void RecognizePrefixes()
        {
            var mapperConfig = new MapperConfiguration(config =>
            {
                config.RecognizePrefixes("Pre_");
                config.CreateMap<User, UserDto>();
            });
            var mapper = mapperConfig.CreateMapper();
            mapper.Map<UserDto>(_user).Show();
        }

        /// <summary>
        /// 特性映射
        /// </summary>
        static void AttributeMap()
        {
            var mapperConfig = new MapperConfiguration(config => config.AddMaps(typeof(Program)));
            var mapper = mapperConfig.CreateMapper();
            mapper.Map<RoleDto>(_role).Show();
        }

        /// <summary>
        /// 构造函数映射
        /// </summary>
        static void ConstructorMap()
        {
            #region 构造函数参数命名与源类型中属性命名相同，则可以直接进行映射
            //var mapperConfig = new MapperConfiguration(config => config.CreateMap<Menu, MenuDto>());
            //var mapper = mapperConfig.CreateMapper();
            //mapper.Map<MenuDto>(_menu).Show(); 
            #endregion

            var mapperConfig = new MapperConfiguration(config =>
            {
                //禁用构造函数映射
                //config.DisableConstructorMapping();
                //不映射私有构造函数
                config.ShouldUseConstructor = ctor => !ctor.IsPrivate;
                config.CreateMap<Menu, MenuDto>()
                  .ForCtorParam("title", options => options.MapFrom(s => s.Name));
            });
            var mapper = mapperConfig.CreateMapper();
            mapper.Map<MenuDto>(_menu).Show();
        }

        /// <summary>
        /// 条件映射
        /// </summary>
        static void ConditionalMap()
        {
            var mapperConfig = new MapperConfiguration(config =>
                //Priority >= 0时才映射
                config.CreateMap<Role, RoleDto>().ForMember(d => d.Priority, options => options.PreCondition(s => s.Priority >= 0)));
            var mapper = mapperConfig.CreateMapper();
            mapper.Map<RoleDto>(_role).Show();
        }

        /// <summary>
        /// 开放泛型
        /// </summary>
        static void OpenGenerics()
        {
            var mapperConfig = new MapperConfiguration(config =>
                config.CreateMap(typeof(Generic<>), typeof(GenericDto<>)));
            var mapper = mapperConfig.CreateMapper();
            mapper.Map<GenericDto<int>>(_generic).Show();
            mapper.Map<GenericDto<string>>(_generic).Show();
        }

        /// <summary>
        /// 
        /// </summary>
        static void UseAsDataSource_ForAndProjectTo()
        {
            //dataContext.OrderLines.UseAsDataSource().For<OrderLineDTO>().Where(dto => dto.Name.StartsWith("A"))
            //相当于 dataContext.OrderLines.Where(ol => ol.Item.Name.StartsWith("A")).ProjectTo<OrderLineDTO>()




            //场景：需要对映射查询出来的集合进行编辑，但是还想要返回IQueryable类型

            //一般情况下会ToList查到数据后进行编辑
            //var dtos = context.OrderLines.Where(ol => ol.OrderId == orderId)
            // .ProjectTo<OrderLineDTO>().ToList();
            //foreach (var dto in dtos)
            //{
            //    // edit some property, or load additional data from the database and augment the dtos
            //}
            //return dtos;

            //可以使用OnEnumerated，它会修改内部表达式，并不会去查询数据。直到真正使用数据时才回去查询数据库
            //return context.OrderLines.Where(ol => ol.OrderId == orderId)
            // .UseAsDataSource()
            // .For<OrderLineDTO>()
            // .OnEnumerated((dtos) =>
            // {
            //     foreach (var dto in dtosCast<OrderLineDTO>())
            //     {
            //         // edit some property, or load additional data from the database and augment the dtos
            //     }
            // }
        }


        /// <summary>
        /// 以User为基准的静态API
        /// </summary>
        static void ConfigMapperByStaticAPI()
        {
            Mapper.Initialize(config => config.CreateMap<User, UserDto>());
            //Mapper.AssertConfigurationIsValid();
        }


        /// <summary>
        /// 以User为基准的实例API
        /// </summary>
        /// <returns></returns>
        static IMapper ConfigMapperByInstanceAPI()
        {
            var mapperConfig = new MapperConfiguration(config => config.CreateMap<User, UserDto>());
            //mapperConfig.AssertConfigurationIsValid();
            return mapperConfig.CreateMapper(); //new Mapper(mapperConfig);
        }
    }
}
