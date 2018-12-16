using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
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
        public void SendGroups(string name,string groupName, string message)
        {
            // Call the addNewMessageToPage method to update clients.
            Clients.Group(groupName).addNewMessageToPage(name + ": " + message);

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
        public void JoinGroup(string name,string groupName)
        {
            Groups.Add(Context.ConnectionId, groupName);
            Clients.Group(groupName).addNewMessageToPage($"{name} join [{groupName}]");

        }


        public override Task OnConnected()
        {
            Clients.Caller.addNewMessageToPage("Connected");
            return base.OnConnected();
        }

        public override Task OnDisconnected(bool stopCalled)
        {
            Clients.All.addNewMessageToPage("DisConnected");
            return base.OnDisconnected(stopCalled);
        }
        public override Task OnReconnected()
        {
            Clients.All.addNewMessageToPage($"{Context.ConnectionId}: ReConnected");

            return base.OnReconnected();
        }


       
    }
}