using System;
using System.IO;
using System.Linq;
using System.Text;
using System.Xml;
using System.Xml.Serialization;

namespace CSharp.XML.Serialize
{
    class Program
    {
        static void Main(string[] args)
        {
            FormModel form = null;
            using (var fs = File.Open("Test.xml", FileMode.Open))
            using (var sr = new StreamReader(fs, Encoding.UTF8))
            {
                var xmlSerializer = new XmlSerializer(typeof(FormModel));
                form = (FormModel)xmlSerializer.Deserialize(sr);
            }


            form.Export.First().Body.Columns.First().Value = "将1111改为2222";

            string xml = null;
            var xmlWriterSettings = new XmlWriterSettings
            {
                Indent = true,
                OmitXmlDeclaration = false,
                Encoding = Encoding.UTF8
            };
            using (var ms = new MemoryStream())
            {
                using (var xmlWriter = XmlWriter.Create(ms, xmlWriterSettings))
                {
                    var xmlSerializer = new XmlSerializer(typeof(FormModel));
                    // 同时去除form节点上的默认命名空间
                    xmlSerializer.Serialize(xmlWriter, form, new XmlSerializerNamespaces(new[] { new XmlQualifiedName() }));
                }

                ms.Position = 0;
                using (var sr = new StreamReader(ms))
                {
                    xml = sr.ReadToEnd();
                }
            }

            Console.WriteLine(xml);
            Console.ReadKey();

        }
    }
}
