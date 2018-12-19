using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WPF.Login
{
    public static class FileHelper
    {
        private readonly static Encoding _encoding = Encoding.UTF8;

        public static string ReadFileByStream(string path,FileMode fileMode,int bufferSizeByM)
        {
            var data = string.Empty;
            using(var fr = new FileStream(path, fileMode, FileAccess.Read))
            {
                var buffer = new byte[bufferSizeByM * 1024 * 1024];
                var readByte = fr.Read(buffer, 0, buffer.Length);
                data = _encoding.GetString(buffer, 0,readByte);
                fr.Flush();
            }
            return data;
        }

        public static void SaveFileByLine(string[] datas,string path,FileMode fileMode)
        {
            using(var fw = new FileStream(path, fileMode, FileAccess.Write))
            {
                var sbDatas = new StringBuilder();
                datas.ToList().ForEach(d => sbDatas.AppendLine(d));
                var byteDatas = _encoding.GetBytes(sbDatas.ToString());
                fw.Write(byteDatas, 0, byteDatas.Length);            
                fw.Flush();
            }
        }
    }
}
