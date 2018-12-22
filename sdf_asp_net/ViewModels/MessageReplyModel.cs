using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace sdf_asp_net.ViewModels
{
    public class MessageReplyModel
    {
        public int Id { get; set; }
        public string Message { get; set; }
        public string Author { get; set; }
        public string Date { get; set; }
    }
}