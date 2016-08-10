using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.SignalR;
using Alumni.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.SignalR.Hubs;

namespace Alumni.Hubs
{
    public class ChatHub : Hub
    {
        private readonly AlumniDataContext _context;

        public ChatHub(AlumniDataContext context)
        {
            _context = context;
        }

        public void Send(string name, string message)
        {
            if (Clients != null)
            {
                // Call the addMessage method on all clients
                Clients.All.addNewMessage(name, message);
            }
        }
    }
}

