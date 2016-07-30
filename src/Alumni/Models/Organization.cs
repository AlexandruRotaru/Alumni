using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Alumni.Models
{
    public partial class Organization
    {
        public Organization()
        {
            UserCVLink = new HashSet<UserCVLink>();
        }

        public int OrganizationID { get; set; }

        [Display(Name = "Data Inceperii")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime? StartDate { get; set; }

        [Display(Name = "Data Incheierii")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime? EndDate { get; set; }

        [RegularExpression(@"^[A-Z]+[a-zA-Z''-'\s]*$", ErrorMessage = "Datele introduse nu sunt in formatul corect.")]
        [Display(Name = "Numele Organizatiei")]
        [StringLength(100)]
        public string OrganizationName { get; set; }

        public virtual ICollection<UserCVLink> UserCVLink { get; set; }
    }
}
