using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Alumni.Models
{
    public partial class Publication
    {
        public Publication()
        {
            UserCVLink = new HashSet<UserCVLink>();
        }

        public int PublicationID { get; set; }

        [RegularExpression(@"^[A-Z]+[a-zA-Z''-'\s]*$", ErrorMessage = "Datele introduse nu sunt in formatul corect.")]
        [Display(Name = "Locatia")]
        [StringLength(100)]
        public string PublicationLocation { get; set; }

        [RegularExpression(@"^[A-Z]+[a-zA-Z''-'\s]*$", ErrorMessage = "Datele introduse nu sunt in formatul corect.")]
        [Display(Name = "Subiectul")]
        [StringLength(100)]
        public string PublicationTopic { get; set; }

        [RegularExpression(@"^[A-Z]+[a-zA-Z''-'\s]*$", ErrorMessage = "Datele introduse nu sunt in formatul corect.")]
        [Display(Name = "Descriere")]
        [StringLength(300)]
        public string PublicationDescription { get; set; }

        public virtual ICollection<UserCVLink> UserCVLink { get; set; }
    }
}
