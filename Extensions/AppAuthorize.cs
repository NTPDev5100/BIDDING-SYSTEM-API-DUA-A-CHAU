using Interface.Services;
using Utilities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Controllers;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using static Utilities.CatalogueEnums;
using Newtonsoft.Json;

namespace Extensions
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, AllowMultiple = true, Inherited = true)]
    public class AppAuthorize : AuthorizeAttribute, IAuthorizationFilter
    {
        public AppAuthorize()
        {
        }

        public void OnAuthorization(AuthorizationFilterContext context)
        {
            var user = (UserLoginModel)context.HttpContext.Items["User"];//.User;
            string controllerName = string.Empty;
            string actionName = string.Empty;
            if (context.ActionDescriptor is ControllerActionDescriptor descriptor)
            {
                controllerName = descriptor.ControllerName;
                actionName = descriptor.ActionName;
            }

            if (user == null)
            {
                context.Result = new JsonResult(new AppDomainResult()
                {
                    ResultCode = (int)HttpStatusCode.Unauthorized,
                    ResultMessage = "Unauthorized"
                });
                //return;
                throw new UnauthorizedAccessException();
            }
            //if (user.expiredDate < Timestamp.UtcNow())
            //{
            //    context.Result = new JsonResult(new AppDomainResult()
            //    {
            //        ResultCode = (int)HttpStatusCode.Unauthorized,
            //        ResultMessage = "Unauthorized"
            //    });
            //    throw new UnauthorizedAccessException();
            //}

            IUserService userService = (IUserService)context.HttpContext.RequestServices.GetService(typeof(IUserService));
            IRoleService roleService = (IRoleService)context.HttpContext.RequestServices.GetService(typeof(IRoleService));
            bool hasPermit = false;
            if (user.isAdmin)
            {
                hasPermit = true;
            }
            if (!user.isAdmin && !string.IsNullOrEmpty(LoginContext.Instance.CurrentUser.roles))
            {
                var permissionArray = new List<string>();
                var roles = JsonConvert.DeserializeObject<List<ObjectJsonRole>>(LoginContext.Instance.CurrentUser.roles);
                foreach (var item in roles)
                {
                    var role =  roleService.GetById(item.Id);
                    if (role != null)
                    {
                        permissionArray.Add(role.Permissions);
                    }
                }
                var permission = string.Join('|', permissionArray);
                var userCheckResult = userService.HasPermission(permission, controllerName, actionName);
                hasPermit = userCheckResult.Result;
            }
            if (!hasPermit)
            {
                context.Result = new JsonResult(new AppDomainResult()
                {
                    ResultCode = (int)HttpStatusCode.Unauthorized,
                    ResultMessage = "Unauthorized"
                });
                throw new UnauthorizedAccessException();
            }

        }
    }
}
