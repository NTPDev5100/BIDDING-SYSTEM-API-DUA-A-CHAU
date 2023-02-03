using Entities;
using Entities.Search;
using Interface.Services.DomainServices;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Interface.Services
{
    public interface IProviderService : IDomainService<tbl_Providers, ProviderSearch>
    {
        Task<tbl_Providers> Login(string userName, string password);
        Task<string> CheckCurrentUserPassword(Guid providerId, string password, string newPasssword);
        Task<bool> UpdateUserPassword(Guid providerId, string newPassword);
        Task<string> GetExistItemMessage(tbl_Providers item);
        Task AccountActivationNotice(Guid providerId, bool isActive, string personInCharge, string email);
        Task<string> GetPersonCharge(string personChargeStr);   
    }
}
