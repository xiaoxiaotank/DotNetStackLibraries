using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace MyApps.UploadFiles.Controllers
{
    /// <summary>
    /// 简单地文件上传：
    ///     - 需要整个文件全部上传至服务器后才能开始处理
    ///     - 内存最大占用65kb，当超出时，旧的数据会被持久化到磁盘（虚拟内存），新的数据会存储到内存
    ///  缺点：无法边上传边处理，数据持久化到磁盘会降低I/O速度，不适用于大文件上传
    /// </summary>
    public class SimpleUploadFileController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

        public async Task<IActionResult> UploadFile(IFormFile file)
        {
            await Task.CompletedTask;
            return View();
        }

        public async Task<IActionResult> UploadFiles(IEnumerable<IFormFile> files)
        {
            await Task.CompletedTask;
            return View();
        }

        public async Task<IActionResult> UploadFiles(IFormFile file1, IFormFile file2)
        {
            await Task.CompletedTask;
            return View();
        }
    }
}