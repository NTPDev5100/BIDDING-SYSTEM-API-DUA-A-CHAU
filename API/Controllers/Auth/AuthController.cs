using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;
using Wangkanai.Detection.Services;

namespace API.Controllers.Auth
{
    /// <summary>
    /// Quản lý tài khoản
    /// </summary>
    [Route("api/authenticate")]
    [ApiController]
    [Description("Authenticate")]
    public class AuthController : BaseAPI.Controllers.Auth.AuthController
    {
        public AuthController(IServiceProvider serviceProvider, IConfiguration configuration, IMapper mapper, ILogger<AuthController> logger, IDetectionService detectionService) : base(serviceProvider, configuration, mapper, logger, detectionService)
        {
        }
    }
}