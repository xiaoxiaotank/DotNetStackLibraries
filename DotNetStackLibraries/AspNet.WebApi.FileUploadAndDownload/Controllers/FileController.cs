using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Formatting;
using System.Net.Http.Headers;
using System.Web;
using System.Web.Http;
using Xceed.Words.NET;

namespace AspNet.WebApi.FileUploadAndDownload.Controllers
{
    [RoutePrefix("api/file")]
    public class FileController : ApiController
    {
        /// <summary>
        /// 上传文件
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [Route("upload")]
        public IHttpActionResult Upload()
        {
            var files = HttpContext.Current.Request.Files;
            if (files != null && files.Count > 0)
            {
                foreach (string key in files)
                {
                    var file = files[key];
                    var path = Path.Combine(AppDomain.CurrentDomain.BaseDirectory,"Files", DateTime.Now.ToString("yyyy-MM-dd"), file.FileName);
                    var directory = Path.GetDirectoryName(path);
                    if (!Directory.Exists(directory))
                    {
                        Directory.CreateDirectory(directory);
                    }
                    try
                    {
                        file.SaveAs(path);
                    }
                    catch(Exception)
                    {

                    }
                }
            }


            return Ok();
        }

        /// <summary>
        /// 下载文件
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Route("download")]
        public HttpResponseMessage DownLoad()
        {
            var path = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "testfile.png");
            var fs = new FileStream(path, FileMode.Open);

            var response = new HttpResponseMessage(HttpStatusCode.OK)
            {
                Content = new StreamContent(fs)
            };
            response.Content.Headers.ContentType = new MediaTypeHeaderValue("application/octet-stream");
            response.Content.Headers.ContentDisposition = new ContentDispositionHeaderValue("attachment")
            {
                FileName = "testfile.png"
            };

            return response;
        }

        /// <summary>
        /// 需要提交参数的下载文件
        /// </summary>
        /// <param name="form"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("download")]
        public HttpResponseMessage Download([FromBody]FormDataCollection form)
        {
            var image64 = form["image64"];
            var stream = new MemoryStream();
            var path = HttpContext.Current.Server.MapPath("/测试报告.docx");

            using (var doc = DocX.Create(path))
            {
                using (var ms = new MemoryStream(Convert.FromBase64String(image64.Split(',').Last())))
                {
                    var picture = doc.AddImage(ms).CreatePicture(300, 600);
                    doc.InsertParagraph()
                        .AppendPicture(picture)
                        .SpacingAfter(50)
                        .Alignment = Alignment.center;
                    ms.Close();
                }

                //只能Save一次
                doc.SaveAs(stream);
                //doc.Save();
            }

            var response = new HttpResponseMessage()
            {
                Content = new FileContent(stream, "测试报告.docx")
            };

            //var response = new HttpResponseMessage()
            //{
            //    Content = new FileContent(path, "测试报告.docx")
            //};

            //var response = new HttpResponseMessage(HttpStatusCode.OK)
            //{
            //    Content = new ByteArrayContent(stream.ToArray())
            //};
            //response.Content.Headers.ContentType = new MediaTypeHeaderValue("application/octet-stream");
            //response.Content.Headers.ContentDisposition = new ContentDispositionHeaderValue("attachment")
            //{
            //    FileName = "测试报告.docx"
            //};

            return response;
        }
    }
}
