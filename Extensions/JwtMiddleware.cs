using Interface.Services;
using Interface.Services.Auth;
using Utilities;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Net;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace Extensions
{
    public class JwtMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly AppSettings _appSettings;
        private readonly ITokenManagerService tokenManagerService;

        public JwtMiddleware(RequestDelegate next, IOptions<AppSettings> appSettings, ITokenManagerService tokenManagerService)
        {
            _next = next;
            _appSettings = appSettings.Value;
            this.tokenManagerService = tokenManagerService;
        }

        public async Task Invoke(Microsoft.AspNetCore.Http.HttpContext context, IUserService userService)
        {
            var token = context.Request.Headers["Authorization"].FirstOrDefault()?.Split(" ").Last();

            if (await tokenManagerService.IsCurrentActiveToken())
            {
                if (token != null)
                    attachUserToContext(context, userService, token);
                await _next(context);
            }
            else
            {
                context.Response.StatusCode = (int)HttpStatusCode.Unauthorized;
                var result = System.Text.Json.JsonSerializer.Serialize(new AppDomainResult()
                {
                    ResultCode = context.Response.StatusCode,
                    Success = false
                });
                await context.Response.WriteAsync(result);
            }
        }

        private void attachUserToContext(Microsoft.AspNetCore.Http.HttpContext context, IUserService userService, string token)
        {
            try
            {
                var tokenHandler = new JwtSecurityTokenHandler();
                var key = Encoding.ASCII.GetBytes(_appSettings.secret);
                tokenHandler.ValidateToken(token, new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(key),
                    ValidateIssuer = false,
                    ValidateAudience = false,
                    ClockSkew = TimeSpan.Zero
                }, out SecurityToken validatedToken);

                var jwtToken = (JwtSecurityToken)validatedToken;
                var userModel = new UserLoginModel();
                var claim = jwtToken.Claims.First(x => x.Type == ClaimTypes.UserData);
                if (claim != null)
                {
                    userModel = JsonConvert.DeserializeObject<UserLoginModel>(claim.Value);
                }

                context.Items["User"] = userModel;
            }
            catch(Exception ex)
            {
                //Hết hạn token
                //throw new TimeoutException(ex.Message);
                throw new UnauthorizedAccessException("Phiên hiệu lực đã hết hạn, xin vui lòng đăng nhập lại!");
            }
        }
    }
}
