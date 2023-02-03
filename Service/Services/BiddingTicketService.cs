using AutoMapper;
using Entities;
using Entities.Search;
using Interface.DbContext;
using Interface.Services;
using Interface.UnitOfWork;
using Microsoft.EntityFrameworkCore;
using Service.Services.DomainServices;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Service.Services
{
    public class BiddingTicketService : DomainService<tbl_BiddingTickets, BiddingTicketsSearch>, IBiddingTicketService
    {
        protected IAppDbContext coreDbContext;
        public BiddingTicketService(IAppUnitOfWork unitOfWork, IMapper mapper, IAppDbContext coreDbContext) : base(unitOfWork, mapper)
        {
            this.coreDbContext = coreDbContext;
        }
        protected override string GetStoreProcName()
        {
            return "BiddingTicket_GetPagingData";
        }


        public string CheckTicketExist(Guid userId,Guid biddingSessionId)
        {
            string mess = string.Empty;
            bool existTicket =  this.Queryable.Any(x => x.CreatedBy == userId && x.BiddingSessionId == biddingSessionId);
            if (existTicket)
                mess = "Bạn đã đấu thầu của phiên này!";
            return mess;
        }
    }
}
