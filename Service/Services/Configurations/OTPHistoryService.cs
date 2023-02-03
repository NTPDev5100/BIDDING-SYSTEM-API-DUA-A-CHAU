using Entities.Configuration;
using Entities.DomainEntities;
using Interface;
using Interface.UnitOfWork;
using AutoMapper;
using System;
using System.Collections.Generic;
using System.Text;
using Service.Services.DomainServices;
using Interface.Services.Configuration;

namespace Service.Services.Configurations
{
    public class OTPHistoryService : DomainService<OTPHistories, BaseSearch>, IOTPHistoryService
    {
        public OTPHistoryService(IAppUnitOfWork unitOfWork, IMapper mapper) : base(unitOfWork, mapper)
        {
        }
    }
}
