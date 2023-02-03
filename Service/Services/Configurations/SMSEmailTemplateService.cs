using Entities;
using Entities.Configuration;
using Entities.DomainEntities;
using Interface.Services.Configuration;
using Interface.UnitOfWork;
using Service.Services.DomainServices;
using Utilities;
using AutoMapper;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Service.Services.Configurations
{
    public class SMSEmailTemplateService : DomainService<SMSEmailTemplates, BaseSearch>, ISMSEmailTemplateService
    {
        public SMSEmailTemplateService(IAppUnitOfWork unitOfWork, IMapper mapper) : base(unitOfWork, mapper)
        {
        }

    }
}
