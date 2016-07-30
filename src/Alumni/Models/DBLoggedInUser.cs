using System;
using System.Collections.Generic;

namespace Alumni.Models
{
    public partial class DBLoggedInUser
    {
        public int LoggedInUserID { get; set; }
        public int? UserId { get; set; }
        public int? RoomId { get; set; }

        public virtual DBChatRoom Room { get; set; }
        public virtual DBUser User { get; set; }
    }
}
