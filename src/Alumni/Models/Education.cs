using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Alumni.Models
{
    public partial class Education
    {
        public Education()
        {
            UserCVLink = new HashSet<UserCVLink>();
        }

        public int EducationID { get; set; }

        [Display(Name = "Data Obtinerii")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime? DateEarned { get; set; }

        [RegularExpression(@"^[A-Z]+[a-zA-Z''-'\s]*$", ErrorMessage = "Datele introduse nu sunt in formatul corect.")]
        [Display(Name = "Denumire")]
        [StringLength(100)]
        public string EducationTitle { get; set; }

        [Display(Name = "Tip")]
        [StringLength(100)]
        [RegularExpression(@"^[A-Z]+[a-zA-Z''-'\s]*$", ErrorMessage = "Datele introduse nu sunt in formatul corect.")]        
        public string EducationType { get; set; }

        public string GrantingInstitution { get; set; }

        [Display(Name = "Descriere")]
        [StringLength(1000)]
        [RegularExpression(@"^[A-Z]+[a-zA-Z''-'\s]*$", ErrorMessage = "Datele introduse nu sunt in formatul corect.")]
        public string EducationDescription { get; set; }

        public virtual ICollection<UserCVLink> UserCVLink { get; set; }
    }
}
