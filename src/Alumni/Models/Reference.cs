using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Alumni.Models
{
    public partial class Reference
    {
        public Reference()
        {
            UserCVLink = new HashSet<UserCVLink>();
        }

        public int ReferenceID { get; set; }

        [RegularExpression(@"^[A-Z]+[a-zA-Z''-'\s]*$", ErrorMessage = "Datele introduse nu sunt in formatul corect.")]
        [Display(Name = "Tipul")]
        [StringLength(100)]
        public string ReferenceType { get; set; }

        [RegularExpression(@"^[A-Z]+[a-zA-Z''-'\s]*$", ErrorMessage = "Datele introduse nu sunt in formatul corect.")]
        [Display(Name = "Detalii")]
        [StringLength(100)]
        public string ReferenceDetail { get; set; }

        public virtual ICollection<UserCVLink> UserCVLink { get; set; }
    }
}
