using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Alumni.Models
{
    public partial class Skill
    {
        public Skill()
        {
            UserCVLink = new HashSet<UserCVLink>();
        }

        public int SkillID { get; set; }

        [Display(Name = "Data de Inceput")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime? StartDate { get; set; }

        [Display(Name = "Data de Sfarsit")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime? EndDate { get; set; }

        [RegularExpression(@"^[A-Z]+[a-zA-Z''-'\s]*$", ErrorMessage = "Datele introduse nu sunt in formatul corect.")]
        [Display(Name = "Zona")]
        [StringLength(100)]
        public string SkillArea { get; set; }

        [RegularExpression(@"^[A-Z]+[a-zA-Z''-'\s]*$", ErrorMessage = "Datele introduse nu sunt in formatul corect.")]
        [Display(Name = "Nume")]
        [StringLength(100)]
        public string SkillName { get; set; }

        [RegularExpression(@"^[A-Z]+[a-zA-Z''-'\s]*$", ErrorMessage = "Datele introduse nu sunt in formatul corect.")]
        [Display(Name = "Descriere")]
        [StringLength(100)]
        public string SkillDescription { get; set; }

        public virtual ICollection<UserCVLink> UserCVLink { get; set; }
    }
}
