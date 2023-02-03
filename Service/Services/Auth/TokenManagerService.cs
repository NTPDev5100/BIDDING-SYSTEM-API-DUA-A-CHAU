using Interface.Services.Auth;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Primitives;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service.Services.Auth
{
    public class TokenManagerService : ITokenManagerService
    {
        private readonly IDistributedCache _cache;
        private readonly IHttpContextAccessor _httpContextAccessor;
        //private readonly IOptions<JwtOptions> _jwtOptions;

        public TokenManagerService(IDistributedCache cache,
                IHttpContextAccessor httpContextAccessor
            )
        {
            _cache = cache;
            _httpContextAccessor = httpContextAccessor;
        }

        public async Task<bool> IsCurrentActiveToken()
            => await IsActiveAsync(GetCurrentAsync());

        public async Task DeactivateCurrentAsync()
            => await DeactivateAsync(GetCurrentAsync());

        public async Task<bool> IsActiveAsync(string token)
            => await _cache.GetStringAsync(GetKey(token)) == null;

        public async Task DeactivateAsync(string token)
            => await _cache.SetStringAsync(GetKey(token),
                " ", new DistributedCacheEntryOptions
                {
                    AbsoluteExpirationRelativeToNow =
                        TimeSpan.FromHours(24)
                });

        private string GetCurrentAsync()
        {
            StringValues result = string.Empty;
            var authorizationHeader = _httpContextAccessor
                .HttpContext.Request.Headers["authorization"];
            result = authorizationHeader == StringValues.Empty
                ? string.Empty
                : authorizationHeader.Single().Split(" ").Last();
            return result;
        }

        private static string GetKey(string token)
            => $"tokens:{token}:deactivated";
    }
}
