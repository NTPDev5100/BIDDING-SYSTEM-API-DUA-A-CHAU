using Entities.ContactCustomer;
using Interface.Services.DomainServices;
using QuanLy.Entities.Search;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Interface.Services.ContactCustomer
{
    public interface IContactCustomerSaleRequestService : IDomainService<ContactCustomerSaleRequests, SearchContactCustomer>
    {
    }
}
