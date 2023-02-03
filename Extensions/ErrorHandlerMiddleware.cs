using Utilities;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text.Json;
using System.Threading.Tasks;

namespace Extensions
{
    public class ErrorHandlerMiddleware
    {
        private readonly RequestDelegate _next;
        private ILogger<ControllerBase> _logger;


        public ErrorHandlerMiddleware(RequestDelegate next, ILogger<ControllerBase> logger)
        {
            _next = next;
            _logger = logger;
        }

        public async Task Invoke(Microsoft.AspNetCore.Http.HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (Exception error)
            {
                var response = context.Response;
                response.ContentType = "application/json";

                switch (error)
                {
                    case AggregateException e:
                        response.StatusCode = (int)HttpStatusCode.Locked;
                        break;
                    case AppException e:
                        response.StatusCode = (int)HttpStatusCode.BadRequest;
                        break;
                    case UnauthorizedAccessException e:
                        response.StatusCode = (int)HttpStatusCode.Unauthorized;
                        break;
                    case InvalidCastException e:
                        response.StatusCode = (int)HttpStatusCode.Forbidden;
                        break;
                    case EntryPointNotFoundException e:
                        response.StatusCode = (int)HttpStatusCode.NotFound;
                        break;
                    case KeyNotFoundException e:
                        response.StatusCode = (int)HttpStatusCode.NotFound;
                        break;
                    case TimeoutException e:
                        response.StatusCode = (int)HttpStatusCode.RequestTimeout;
                        break;
                    default:
                        {
                            var RouteData = context.Request.Path.Value.Split("/");
                            string apiName = string.Empty;
                            string actionName = string.Empty;

                            if (RouteData.Count() >= 2)
                                apiName = RouteData[1];
                            if (RouteData.Count() >= 3)
                                actionName = RouteData[2];

                            _logger.LogError(string.Format("{0} {1}: {2}", apiName
                                , actionName, error?.Message));
                            response.StatusCode = (int)HttpStatusCode.InternalServerError;
                        }

                        break;
                }
                var result = JsonSerializer.Serialize(new AppDomainResult()
                {
                    ResultCode = response.StatusCode,
                    ResultMessage = error?.Message,
                    Success = false
                });
                await response.WriteAsync(result);
            }
        }
    }
}
