using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Alumni.Models
{
    public partial class Location
    {
        public Location()
        {
            UserCVLink = new HashSet<UserCVLink>();
        }
        
        public int LocationID { get; set; }

        [Display(Name = "Data Inceperii")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime? StartDate { get; set; }

        [Display(Name = "Data Incheierii")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime? EndDate { get; set; }

        [RegularExpression(@"^[A-Z]+[a-zA-Z''-'\s]*$", ErrorMessage = "Datele introduse nu sunt in formatul corect.")]
        [Display(Name = "Oras")]
        [StringLength(30)]
        public string City { get; set; }

        [RegularExpression(@"^[A-Z]+[a-zA-Z''-'\s]*$", ErrorMessage = "Datele introduse nu sunt in formatul corect.")]
        [Display(Name = "Stat/Provincie/Judet")]
        [StringLength(30)]
        public string StateOrProvince { get; set; }

        [RegularExpression(@"^[A-Z]+[a-zA-Z''-'\s]*$", ErrorMessage = "Datele introduse nu sunt in formatul corect.")]
        [Display(Name = "Tara")]
        [StringLength(30)]
        public string Country { get; set; }

        public virtual ICollection<UserCVLink> UserCVLink { get; set; }
    }
}
