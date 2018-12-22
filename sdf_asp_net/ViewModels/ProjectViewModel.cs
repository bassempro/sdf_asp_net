using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace sdf_asp_net.ViewModels
{
    public class ProjectViewModel
    {

        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public List<string> Member { get; set; }
        public string ManagerId { get; set; }
        public string ManagerName { get; set; }

        public string Username { get; set; }

        public List<string> Chatlist { get; set; }

        public List<string> Chatlistoff { get; set; }


        public List<MessageViewModel> Messages { get; set; }
      



        public ProjectViewModel()
        {
            
            Member = new List<string>();
            Messages = new List<MessageViewModel>();
            Chatlist = new List<string>();
            Chatlistoff = new List<string>();

        }


    }
}