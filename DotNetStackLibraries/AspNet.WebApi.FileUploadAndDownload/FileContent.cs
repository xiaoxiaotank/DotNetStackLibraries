using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using System.Web;

namespace AspNet.WebApi.FileUploadAndDownload
{
    public class FileContent : HttpContent
    {
        private Stream _stream;

        private FileContent(string fileName, string mediaType)
        {
            Headers.ContentDisposition = new ContentDispositionHeaderValue("attachment")
            {
                FileName = fileName
            };
            Headers.ContentType = new MediaTypeHeaderValue(mediaType);
        }

        public FileContent(Stream stream, string fileName, string mediaType = "application/octet-stream") : this(fileName,  mediaType)
        {
            _stream = stream;
        }

        public FileContent(string path, string fileName, string mediaType = "application/octet-stream") : this(fileName, mediaType)
        {
            _stream = new FileStream(path, FileMode.Open, FileAccess.Read);
        }


        protected override Task SerializeToStreamAsync(Stream stream, TransportContext context)
        {
            if(_stream is MemoryStream ms)
            {
                var byteStream = ms.ToArray();
                return stream.WriteAsync(byteStream, 0, byteStream.Length);
            }

            return _stream.CopyToAsync(stream);
        }

        /// <summary>
        /// true:长度固定
        /// false:长度可变
        /// </summary>
        /// <param name="length"></param>
        /// <returns></returns>
        protected override bool TryComputeLength(out long length)
        {
            if (_stream.CanSeek)
            {
                length = _stream.Length;
                return true;
            }

            length = 0;
            return false;
        }
    }
}