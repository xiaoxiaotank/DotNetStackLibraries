using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Serialization;

namespace CSharp.XML.Serialize
{
    [XmlRoot("form")]
    public class FormModel
    {
        [XmlAttribute("version")]
        public string Version { get; set; }

        [XmlElement("export")]
        public List<ExportModel> Export { get; set; }

    }

    public class ExportModel
    {
        [XmlElement("base")]
        public BaseModel Base { get; set; }

        [XmlElement("header")]
        public HeaderModel Header { get; set; }

        [XmlElement("body")]
        public BodyModel Body { get; set; }
    }

    public class BaseModel
    {
        [XmlAttribute("id")]
        public string Id { get; set; }

        [XmlAttribute("name")]
        public string Name { get; set; }
    }

    public class HeaderModel
    {
        [XmlElement("column")]
        public List<HeaderColumnModel> Columns { get; set; }
    }

    public class HeaderColumnModel
    {
        [XmlAttribute("id")]
        public string Id { get; set; }

        [XmlAttribute("type")]
        public HeaderColumnType Type { get; set; }

        [XmlAttribute("name")]
        public string Name { get; set; }

        [XmlAttribute("nullable")]
        public bool Nullable { get; set; }

        [XmlAttribute("length")]
        public string Length { get; set; }

    }

    public class BodyModel
    {
        [XmlElement("column")]
        public List<BodyColumnModel> Columns { get; set; }
    }

    public class BodyColumnModel
    {
        [XmlAttribute("name")]
        public string Name { get; set; }

        [XmlIgnore]
        public string Value { get; set; }

        [XmlElement("value")]
        public XmlCDataSection ValueCData
        {
            get => new XmlDocument().CreateCDataSection(Value);
            set => Value = value.Value;
        }

    }

    public enum HeaderColumnType
    {
        [XmlEnum("0")]
        Type1 = 0,
        [XmlEnum("Type2")]
        Type2 = 1
    }
}
