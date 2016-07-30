using System;
using System.Collections.Generic;

namespace Alumni.Models
{
    public partial class UserCVLink
    {
        public UserCVLink()
        {
            Education = new Education();
            Organization = new Organization();
            Publication = new Publication();
            Skill = new Skill();
            Location = new Location();
            Referenece = new Reference();
        }

        public int UserCVLinkID { get; set; }
        public int? UserId { get; set; }
        public int? OrganizationId { get; set; }
        public int? SkillId { get; set; }
        public int? PublicationId { get; set; }
        public int? EducationId { get; set; }
        public int? RefereneceId { get; set; }
        public int? LocationId { get; set; }

        public virtual Education Education { get; set; }
        public virtual Location Location { get; set; }
        public virtual Organization Organization { get; set; }
        public virtual Publication Publication { get; set; }
        public virtual Reference Referenece { get; set; }
        public virtual Skill Skill { get; set; }
        public virtual DBUser User { get; set; }
    }
}
