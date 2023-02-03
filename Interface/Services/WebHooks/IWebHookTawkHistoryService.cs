using Entities.DomainEntities;
using Entities.WebHooks;
using Interface.Services.DomainServices;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Interface.Services.WebHooks
{
    public interface IWebHookTawkHistoryService : IDomainService<WebHookTawkHistories, BaseSearch>
    {
    }
}
