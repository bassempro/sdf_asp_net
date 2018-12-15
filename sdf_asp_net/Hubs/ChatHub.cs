using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;   
using Microsoft.AspNet.SignalR;
using sdf_asp_net.Models;

namespace sdf_asp_net.Hubs
{
    public class ChatHub : Hub
    {
        ApplicationDbContext _context;
        public ChatHub()
        {
            _context = new ApplicationDbContext();
        }
        public override System.Threading.Tasks.Task OnConnected()
        {
            Clients.All.user();
            return base.OnConnected();
        }

        public void Send(string name, string message)
        {
            //wenn ich eine Message verschicke 
            Clients.Caller.message("You: " + message);
            //wenn andere eine Message verschickt
            Clients.Others.message(message);
            ChatModel chatModel = new ChatModel()
            {
                //init object
                user = name,
                message = message,
                dateStamp = DateTime.Now

            };
            //add to DB and save changes
            _context.ChatModels.Add(chatModel);
            _context.SaveChanges();



        }
    }
}