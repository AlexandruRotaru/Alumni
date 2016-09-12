using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Alumni.Models
{
    public class Comment
    {
        public int CommentID { get; set; }
        public int? UserId { get; set; }
        public int? PostId { get; set; }
        public string Message { get; set; }
        public DateTime? Timestamp { get; set; }

        public virtual DBUser User { get; set; }
        public virtual Post Post { get; set; }
    }
}
