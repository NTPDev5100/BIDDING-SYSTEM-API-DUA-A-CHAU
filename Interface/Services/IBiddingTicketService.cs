using Entities;
using Entities.Search;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Interface.Services
{
    public interface IBiddingTicketService : DomainServices.IDomainService<tbl_BiddingTickets, BiddingTicketsSearch>
    {
        string CheckTicketExist(Guid userId, Guid biddingSessionId);
    }
}
