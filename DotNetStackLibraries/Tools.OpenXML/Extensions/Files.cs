using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace Tools.OpenXML.Extensions
{
    static class Files
    {
        /// <summary>
        /// 文件路径
        /// </summary>
        public static readonly string FilePath = Path.Combine(Applications.BasePath, "Files");

        /// <summary>
        /// 检查文件是否被占用
        /// </summary>
        /// <param name="fileFullName"></param>
        /// <returns></returns>
        public static bool IsBusy(string fileFullName)
        {
            bool isBusy = false;

            FileStream fs = null;
            try
            {
                fs = new FileStream(fileFullName, FileMode.Open, FileAccess.Read, FileShare.None);
            }
            catch
            {
                isBusy = true;
            }
            finally
            {
                if (fs != null)
                {
                    fs.Close();
                }
            }
            return isBusy;
        }
    }
}
