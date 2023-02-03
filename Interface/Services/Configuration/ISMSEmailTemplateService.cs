using Entities;
using Entities.Configuration;
using Entities.DomainEntities;
using Interface.Services.DomainServices;
using System;
using System.Collections.Generic;
using System.Text;

namespace Interface.Services.Configuration
{
    public interface ISMSEmailTemplateService : IDomainService<SMSEmailTemplates, BaseSearch>
    {
    }
}
