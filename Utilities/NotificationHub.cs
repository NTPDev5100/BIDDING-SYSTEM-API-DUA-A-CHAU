using Microsoft.AspNetCore.SignalR;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Utilities
{
    public class NotificationHub : Hub
    {
        [HubMethodName("get-list-notification")]
        public async Task GetListNotification()
        {
            await Clients.All.SendAsync("GetNotifications");
        }

        /// <summary>
        /// Hub lấy tổng số notification hiện tại
        /// </summary>
        /// <returns></returns>
        [HubMethodName("get-total-notification")]
        public async Task GetTotalNotification()
        {
            await Clients.All.SendAsync("get-total-notification");
        }

    }
}
