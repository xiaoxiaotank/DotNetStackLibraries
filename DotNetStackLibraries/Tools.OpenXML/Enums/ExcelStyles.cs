using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Tools.OpenXML.Enums
{
    enum EditingLanguage
    {
        [EnumMember(Value = "zh-CN")]
        SimpleChinese,

        [EnumMember(Value = "en-US")]
        English,
    }
}
