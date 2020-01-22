using CSharp.CultureInfo.MultiLanguage.Properties;
using System;
using System.Globalization;

namespace CSharp.CultureInfo.MultiLanguage
{
    /// <summary>
    /// 1.在属性中的“资源”选项卡中新建默认资源文件
    /// 2.在资源文件中配置资源
    /// 3.复制一份该默认资源文件，并将其该名为Resource.en-US.resx，并在“访问修饰符”中选择无代码生成
    ///   如果该资源文件下的cs文件仍存在，则手动删除
    /// 
    /// 发布的文件中，会生成对应的语言资源文件夹，内部的dll称为“卫星程序集”
    /// </summary>
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine($"当前文化：{Resources.Culture}");
            Console.WriteLine(Resources.Name);

            Resources.Culture = new System.Globalization.CultureInfo("en-US");
            Console.WriteLine($"当前文化：{Resources.Culture}");
            Console.WriteLine(Resources.Name);

            Console.ReadKey();
        }
    }
}
