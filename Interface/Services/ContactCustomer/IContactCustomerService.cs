using Entities;
using Interface.Services.DomainServices;
using QuanLy.Entities.Search;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Utilities;

namespace Interface.Services.ContactCustomer
{
    public interface IContactCustomerService : IDomainService<ContactCustomers, SearchContactCustomer>
    {
        /// <summary>
        /// Lấy thông tin liên hệ theo email/phone
        /// </summary>
        /// <param name="phone"></param>
        /// <param name="email"></param>
        /// <returns></returns>
        Task<ContactCustomers> GetExistContactCustomer(string phone, string email);
        /// <summary>
        /// Phân công contact cho nhân viên chăm sóc, tư vấn
        /// </summary>
        /// <param name="contactCustomerId"></param>
        /// <param name="saleId"></param>
        /// <returns></returns>
        Task<Utilities.AppDomainResult> AssignContact(Guid contactCustomerId, Guid saleId);
        /// <summary>
        /// Cập nhật trạng thái contact
        /// </summary>
        /// <param name="contactCustomerId"></param>
        /// <param name="statusId"></param>
        /// <returns></returns>
        Task<Utilities.AppDomainResult> UpdateStatusContact(Guid contactCustomerId, CatalogueEnums.ContactCustomerStatus statusId);
    }
}
