using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace sdf_asp_net.Models
{
    public class ChatModel
    {
        public int id { get; set; }
        public string user { get; set; }
        public string message { get; set; }
        public DateTime dateStamp { get; set; }
        public int MyProperty { get; set; }

        public List<string> OnlineMember { get; set; }


        public ChatModel()
        {

            OnlineMember = new List<string>();

        }
    }
}