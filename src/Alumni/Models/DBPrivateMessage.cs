using System;
using System.Collections.Generic;

namespace Alumni.Models
{
    public partial class DBPrivateMessage
    {
        public int PrivateMessageID { get; set; }
        public int? UserId { get; set; }
        public int? ToUserId { get; set; }
        public string Text { get; set; }
        public DateTime TimeStamp { get; set; }
        public string Color { get; set; }

        public virtual DBUser ToUser { get; set; }
        public virtual DBUser User { get; set; }
    }
}
