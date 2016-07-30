using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Alumni.Models.UserViewModels
{
    public class CreateUserViewModel
    {
        [Required]
        public DBDegree Degree { get; set; }

        [Required]
        public DBUser User { get; set; }

        [Required]
        public AspNetUsers LogInUser { get; set; }

        [Required]
        public AspNetRoles UserRoles { get; set; }

        [Required]
        [Display (Name = "Parola")]
        public string Password { get; set; }
    }
}
