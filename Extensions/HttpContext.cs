using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Extensions
{
    public static class HttpContext
    {
        private static IHttpContextAccessor _contextAccessor;

        public static Microsoft.AspNetCore.Http.HttpContext Current => _contextAccessor.HttpContext;

        public static void Configure(IHttpContextAccessor contextAccessor)
        {
            _contextAccessor = contextAccessor;
        }
    }
}
