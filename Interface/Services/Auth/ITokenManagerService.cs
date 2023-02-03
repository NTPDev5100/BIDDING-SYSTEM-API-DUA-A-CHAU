using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Interface.Services.Auth
{
    public interface ITokenManagerService
    {
        Task<bool> IsCurrentActiveToken();
        Task DeactivateCurrentAsync();
        Task<bool> IsActiveAsync(string token);
        Task DeactivateAsync(string token);
    }
}
