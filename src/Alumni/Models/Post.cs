using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Alumni.Models
{
    public class Post
    {
        public Post()
        {
            Comment = new HashSet<Comment>();
        }
        public int PostID { get; set; }
        public int? UserId { get; set; }
        public string Message { get; set; }
        public DateTime? Timestamp { get; set; }

        public virtual ICollection<Comment> Comment { get; set; }
        public virtual DBUser User { get; set; }
    }
}
