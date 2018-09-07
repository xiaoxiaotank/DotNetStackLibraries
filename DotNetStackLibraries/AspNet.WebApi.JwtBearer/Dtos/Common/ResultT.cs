using AspNet.WebApi.JwtBearer.Consts;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AspNet.WebApi.JwtBearer.Dtos.Common
{
    public class ResultT<T> where T : class
    {
        [JsonProperty("data")]
        public T Data { get; set; }

        [JsonProperty("code")]
        public string Code { get; set; }

        [JsonProperty("success")]
        public bool IsSuccessful { get; set; }

        [JsonProperty("msg")]
        public string Message { get; set; }

        /// <summary>
        /// 成功时
        /// </summary>
        /// <param name="result"></param>
        public ResultT(T result)
        {
            Data = result;
            IsSuccessful = true;
            Code = Result.SuccessCode;
        }

        /// <summary>
        /// 失败时
        /// 推荐传递Result的类型，便于接收者知晓数据类型
        /// </summary>
        /// <param name="message"></param>
        public ResultT(string message, string code = Result.FailCode)
        {
            Message = message;
            IsSuccessful = false;
            Code = code;
        }
    }
}