using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;

namespace API.Controllers
{
    /// <summary>
    /// Upload file lên hệ thống
    /// </summary>
    [Route("api/file")]
    [ApiController]
    [Description("Upload file lên hệ thống")]
    [Authorize]
    public class FileController : BaseAPI.Controllers.BaseFileController
    {
        public FileController(IServiceProvider serviceProvider, ILogger<FileController> logger, IWebHostEnvironment env, IHttpContextAccessor httpContextAccessor) : base(serviceProvider, logger, env, httpContextAccessor)
        {
        }
    }
}
