using System;
using System.Collections.Generic;

namespace Alumni.Models
{
    public partial class DBDegree
    {
        public DBDegree()
        {
            DBUser = new HashSet<DBUser>();
        }

        public int DegreeID { get; set; }
        public string DegreeName { get; set; }
        public string DegreeType { get; set; }

        public virtual ICollection<DBUser> DBUser { get; set; }
    }
}
