using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Alumni.Models.UserViewModels
{
    public class UserCvViewModel
    {
        public UserCvViewModel()
        {
            User = new DBUser();
            UserCvLink = new UserCVLink();
            UserCvLink.Education = new Education();
            UserCvLink.Location = new Location();
            UserCvLink.Organization = new Organization();
            UserCvLink.Publication = new Publication();
            UserCvLink.Referenece = new Reference();
            UserCvLink.Skill = new Skill();
        }
        public DBUser User { get; set; }
        public UserCVLink UserCvLink { get; set; }
        public List<UserCVLink> UserCvLinkList { get; set; }

    }
}
