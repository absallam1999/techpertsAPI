using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.SignalR;

namespace Service.Utilities
{

    public class NotificationHub : Hub
    {
        public async Task JoinDeliveryPersonGroup(string deliveryPersonId)
        {
            await Groups.AddToGroupAsync(Context.ConnectionId, deliveryPersonId);
        }
    }

}
