using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace Alumni.Models.UserViewModels
{
    public class SearchViewModel
    {
        public List<DBUser> usersList;
        public bool isAdvancedSearch { get; set; }       
        public bool isEducation { get; set; }
        public bool isReference { get; set; }
        public bool isPublication { get; set; }
        public bool isOrganization { get; set; }
        public bool isSkill { get; set; }
        public bool isLocation { get; set; }
    }
}
