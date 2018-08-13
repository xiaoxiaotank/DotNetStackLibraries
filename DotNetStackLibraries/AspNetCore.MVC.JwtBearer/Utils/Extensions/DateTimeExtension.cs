using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AspNetCore.MVC.JwtBearer.Utils.Extensions
{
    public static class DateTimeExtension
    {
        public static long ToUnixTimeSeconds(this DateTime dateTime) =>
            new DateTimeOffset(dateTime).ToUnixTimeSeconds();
    }
}
