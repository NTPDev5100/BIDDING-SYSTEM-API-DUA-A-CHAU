using Entities;
using Entities.Search;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Interface.Services
{
    public interface IBiddingSessionService : DomainServices.IDomainService<tbl_BiddingSessions, BiddingSessionsSearch>
    {
        //Task BiddingSessionAuto();
        Task ChangeStatusBiddingSessionStart(tbl_BiddingSessions item);
        Task ChangeStatusBiddingSessionEnd(tbl_BiddingSessions item);
    }
}
