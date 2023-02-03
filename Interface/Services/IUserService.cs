using Entities;
using Entities.Search;
using Interface.Services.DomainServices;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Interface.Services
{
    public interface IUserService: IDomainService<tbl_Users, UserSearch>
    {
        //Task<bool> Verify(string userName, string password);
        Task<tbl_Users> Login(string userName, string password);
        Task<string> CheckCurrentUserPassword(Guid userId, string password, string newPasssword);
        //Task<bool> UpdateUserToken(Guid userId, string token, bool isLogin = false);
        Task<bool> UpdateUserPassword(Guid userId, string newPassword);
        Task<bool> HasPermission(string roles, string controller, string permission);
        Task<List<int>> GetRoleNumberLevel(Guid userId);

    }
}
