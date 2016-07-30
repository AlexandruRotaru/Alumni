using System;
using System.Collections.Generic;

namespace Alumni.Models
{
    public partial class DBChatRoom
    {
        public DBChatRoom()
        {
            DBChatMessage = new HashSet<DBChatMessage>();
            DBLoggedInUser = new HashSet<DBLoggedInUser>();
        }

        public int RoomID { get; set; }
        public string Name { get; set; }

        public virtual ICollection<DBChatMessage> DBChatMessage { get; set; }
        public virtual ICollection<DBLoggedInUser> DBLoggedInUser { get; set; }
    }
}
