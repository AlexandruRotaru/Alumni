using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Alumni.Models
{
    public partial class DBUser
    {
        public DBUser()
        {
            DBChatMessageToUser = new HashSet<DBChatMessage>();
            DBChatMessageUser = new HashSet<DBChatMessage>();
            DBLoggedInUser = new HashSet<DBLoggedInUser>();
            DBPrivateMessageToUser = new HashSet<DBPrivateMessage>();
            DBPrivateMessageUser = new HashSet<DBPrivateMessage>();
            UserCVLink = new HashSet<UserCVLink>();
        }

        public int UserID { get; set; }

        [StringLength(13)]
        [RegularExpression(@"^[0-9]{13}$") ]
        public string CNP { get; set; }

        [Display(Name = "Nume")]
        [StringLength(30)]
        [RegularExpression(@"^[A-Z]+[a-zA-Z''-'\s]*$")]
        public string fName { get; set; }

        [Display(Name = "Prenume")]
        [StringLength(90)]
        [RegularExpression(@"^[A-Z]+[a-zA-Z''-'\s]*$")]
        public string lName { get; set; }

        [Display(Name = "Numar de telefon")]
        [StringLength(10)]
        [RegularExpression(@"^[0-9]{10}$")]
        public string Telephone_Number { get; set; }

        [DataType(DataType.EmailAddress)]
        public string Email { get; set; }

        [Display(Name = "Gen")]
        public string Sex { get; set; }

        [Display(Name = "Tara")]
        [StringLength(90)]
        [RegularExpression(@"^[A-Z]+[a-zA-Z''-'\s]*$")]
        public string Country { get; set; }

        [Display(Name = "Oras")]
        [StringLength(90)]
        [RegularExpression(@"^[A-Z]+[a-zA-Z''-'\s]*$")]
        public string City { get; set; }

        [Display(Name = "Adresa")]
        [StringLength(90)]
        public string Adress { get; set; }

        public int? DegreeId { get; set; }

        public string AspNetUser { get; set; }

        public virtual ICollection<DBChatMessage> DBChatMessageToUser { get; set; }
        public virtual ICollection<DBChatMessage> DBChatMessageUser { get; set; }
        public virtual ICollection<DBLoggedInUser> DBLoggedInUser { get; set; }
        public virtual ICollection<DBPrivateMessage> DBPrivateMessageToUser { get; set; }
        public virtual ICollection<DBPrivateMessage> DBPrivateMessageUser { get; set; }
        public virtual ICollection<UserCVLink> UserCVLink { get; set; }
        public virtual AspNetUsers AspNetUserNavigation { get; set; }
        public virtual DBDegree Degree { get; set; }
    }
}
