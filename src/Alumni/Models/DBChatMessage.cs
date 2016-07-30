using System;
using System.Collections.Generic;

namespace Alumni.Models
{
    public partial class DBChatMessage
    {
        public int MessageID { get; set; }
        public int? RoomId { get; set; }
        public int? UserId { get; set; }
        public int? ToUserId { get; set; }
        public string Text { get; set; }
        public DateTime? Timestamp { get; set; }
        public string Color { get; set; }

        public virtual DBChatRoom Room { get; set; }
        public virtual DBUser ToUser { get; set; }
        public virtual DBUser User { get; set; }
    }
}
