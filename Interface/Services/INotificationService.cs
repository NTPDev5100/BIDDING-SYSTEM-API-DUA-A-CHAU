using Entities;
using Entities.Search;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Utilities.CatalogueEnums;

namespace Interface.Services
{
    public interface INotificationService : DomainServices.IDomainService<tbl_Notification, NotificationsSearch>
    {
        Task Send(Guid? userId, Guid? createdById, string title, string content, int? isType);
        Task OneSignalPushNotifications(string headings, string content, string OneSignal_PlayerId);
    }
}
