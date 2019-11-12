using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace MyApps.UploadFiles.Controllers
{
    /// <summary>
    /// 基于文件流的上传：
    ///     - 利用滑动窗口的原理，边上传边处理
    ///  优点：可用于大文件上传且速度快，内存占用度低
    /// </summary>
    public class StreamedUploadFileController : Controller
    {
        public IActionResult Index()
        {
            return View();
        } 
    }
}