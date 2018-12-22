using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace sdf_asp_net.ViewModels
{
    public class ChatViewModell
    {
        public int Id { get; set; }
        public string Username { get; set; }
        public List<string> Online { get; set; }

        public ChatViewModell()
        {

            Online = new List<string>();

        }
    }
}