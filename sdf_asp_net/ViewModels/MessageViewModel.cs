using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace sdf_asp_net.ViewModels
{
    public class MessageViewModel
    {
        public int Id { get; set; }
        public string Message { get; set; }
        public string Author { get; set; }
        public string Date { get; set; }
        public string FileName { get; set; }
        public string FileContentType { get; set; }
        public byte[] FileData { get; set; }
        public List<MessageReplyModel> MessageReplies { get; set; }

        public MessageViewModel()
        {
            MessageReplies = new List<MessageReplyModel>();
        }

    }
}