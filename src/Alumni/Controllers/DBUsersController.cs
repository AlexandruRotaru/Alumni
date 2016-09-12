using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Alumni.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Authorization;
using Alumni.Models.UserViewModels;

namespace Alumni.Controllers
{
    [Authorize]
    public class DBUsersController : Controller
    {
        private readonly AlumniDataContext _context;
        private readonly UserManager<ApplicationUser> _userManager;

        public DBUsersController(AlumniDataContext context, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _userManager = userManager;   
        }

        // GET: DBUsers
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Index()
        {
            var alumniDataContext = _context.DBUser.Include(d => d.AspNetUserNavigation).Include(d => d.Degree);
            return View(await alumniDataContext.ToListAsync());
        }

        // GET: DBUsers/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var dBUser = await _context.DBUser.SingleOrDefaultAsync(m => m.UserID == id);
            if (dBUser == null)
            {
                return NotFound();
            }

            return View(dBUser);
        }

        // GET: DBUsers/Create
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Create()
        {
            CreateUserViewModel model = new CreateUserViewModel();            
            var degreeName = await _context.DBDegree.Select(u => u.DegreeName).Distinct().ToListAsync();
            var degereType = await _context.DBDegree.Select(u => u.DegreeType).Distinct().ToListAsync();
            var roles = await _context.AspNetRoles.Select(u => u.Name).ToListAsync();            
            ViewData["Rol"] = new SelectList(roles, "Name");
            ViewData["Nume Specializare"] = new SelectList(degreeName, "DegreeName");
            ViewData["Nivel Specializare"] = new SelectList(degereType, "DegreeType");
            return View(model);
        }

        // POST: DBUsers/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [Authorize(Roles = "Admin")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(CreateUserViewModel dBUser)
        {
            if (ModelState.IsValid)
            {
                var user = new ApplicationUser { UserName = dBUser.LogInUser.Email, Email = dBUser.LogInUser.Email };
                var result = await _userManager.CreateAsync(user, dBUser.Password);
                if(result.Succeeded)
                {
                    await _userManager.AddToRoleAsync(user, dBUser.UserRoles.Name);
                    var uu = new DBUser();
                    uu.Adress = dBUser.User.Adress;
                    var idQuery = await (from u in _context.AspNetUsers
                                         where u.Email == dBUser.LogInUser.Email
                                         select u.Id).FirstAsync();
                    uu.AspNetUser = idQuery;
                    uu.City = dBUser.User.City;
                    uu.CNP = dBUser.User.CNP;
                    uu.Country = dBUser.User.Country;
                    var degreeQuery = await (from d in _context.DBDegree
                                             where d.DegreeName == dBUser.Degree.DegreeName && d.DegreeType == dBUser.Degree.DegreeType
                                             select d.DegreeID).FirstAsync();
                    uu.DegreeId = degreeQuery;
                    uu.fName = dBUser.User.fName;
                    uu.lName = dBUser.User.lName;
                    uu.Sex = dBUser.User.Sex;
                    uu.Email = dBUser.LogInUser.Email;
                    uu.Telephone_Number = dBUser.User.Telephone_Number;
                    _context.DBUser.Add(uu);
                }                
                await _context.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            var degreeName = _context.DBDegree.Select(u => u.DegreeName).Distinct().ToList();
            var degereType = _context.DBDegree.Select(u => u.DegreeType).Distinct().ToList();
            var roles = _context.AspNetRoles.Select(u => u.Name).ToList();
            ViewData["Rol"] = new SelectList(roles, "Name");
            ViewData["Nume Specializare"] = new SelectList(degreeName, "DegreeName");
            ViewData["Nivel Specializare"] = new SelectList(degereType, "DegreeType");
            return View(dBUser);
        }

        // GET: DBUsers/Edit/5
        [Authorize(Roles = "Admin,Student,Profesor")]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var dBUser = await _context.DBUser.SingleOrDefaultAsync(m => m.UserID == id);
            if (dBUser == null)
            {
                return NotFound();
            }
            //var degreeName = _context.DBDegree.Select(u => u.DegreeName).Distinct().ToList();
            //var degereType = _context.DBDegree.Select(u => u.DegreeType).Distinct().ToList();
            //ViewData["Nume Specializare"] = new SelectList(degreeName, "DegreeName");
            //ViewData["Nivel Specializare"] = new SelectList(degereType, "DegreeType");
            return View(dBUser);
        }

        // POST: DBUsers/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [Authorize(Roles = "Admin,Student,Profesor")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("UserID,Adress,CNP,City,Country,Email,Sex,Telephone_Number,fName,lName,AspNetUser,DegreeId")] DBUser dBUser)
        {
            if (id != dBUser.UserID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {           
                    _context.Update(dBUser);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!DBUserExists(dBUser.UserID))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                var checkUser = await _userManager.GetUserAsync(HttpContext.User);
                if(await _userManager.IsInRoleAsync(checkUser, "Student"))
                {
                    return RedirectToAction("Index", "Home");
                }
                else
                {
                    return RedirectToAction("Index");
                }
               
            }
            //var degreeName = _context.DBDegree.Select(u => u.DegreeName).Distinct().ToList();
            //var degereType = _context.DBDegree.Select(u => u.DegreeType).Distinct().ToList();
            //ViewData["Nume Specializare"] = new SelectList(degreeName, "DegreeName");
            //ViewData["Nivel Specializare"] = new SelectList(degereType, "DegreeType");
            return View(dBUser);
        }

        // GET: DBUsers/Delete/5
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            
            var dBUser = await _context.DBUser.SingleOrDefaultAsync(m => m.UserID == id);
            if (dBUser == null)
            {
                return NotFound();
            }

            return View(dBUser);
        }

        // POST: DBUsers/Delete/5
        [HttpPost, ActionName("Delete")]
        [Authorize(Roles = "Admin")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var dBUser = await _context.DBUser.SingleOrDefaultAsync(m => m.UserID == id);
            var aspUser = await _context.AspNetUsers.SingleOrDefaultAsync(m => m.Id == dBUser.AspNetUser);
            var aspUserRole = await _context.AspNetUserRoles.SingleOrDefaultAsync(r => r.UserId == aspUser.Id);            
            _context.AspNetUserRoles.Remove(aspUserRole);
            _context.AspNetUsers.Remove(aspUser);
            await _context.SaveChangesAsync();

            var links = await _context.UserCVLink.Where(l => l.UserId == id).ToListAsync();
            await RemoveCvComponentsAsync(links);
            links.ForEach(l => _context.UserCVLink.Remove(l));
            await _context.SaveChangesAsync();

            //ToDo: Remove Chat related to USER        
            _context.DBUser.Remove(dBUser);
            await _context.SaveChangesAsync();
            return RedirectToAction("Index");
        }

        // GET: DBUsers/Curriculum
        public async Task<IActionResult> Curriculum(int? id)
        {
            UserCvViewModel model = new UserCvViewModel();
            UserCVLink userCv = new UserCVLink();
            DBUser user = new DBUser();

            if (id != null)
            {
                //user = await _context.DBUser.Where(u => u.UserID == id).FirstAsync();
                user = (from u in _context.DBUser
                        where u.UserID == id
                        select u).First();

                if (user == null) return NotFound();
                model.User = user;
                userCv = await _context.UserCVLink.Where(us => us.UserId == id).FirstOrDefaultAsync();                
            }
            else
            {
                var loggedUser = await _userManager.GetUserAsync(HttpContext.User);
                //user = await _context.DBUser.Where(u => u.AspNetUser == loggedUser.Id).FirstAsync();
                user = (from u in _context.DBUser
                        where u.AspNetUser == loggedUser.Id
                        select u).First();

                if (user == null) return NotFound();
                model.User = user;

                userCv = await _context.UserCVLink.Where(us => us.UserId == user.UserID).FirstOrDefaultAsync();
            }


            if (userCv != null)
            {
                //var cvQuery = await _context.UserCVLink.Where(cv => cv.UserId == user.UserID).ToListAsync();
                var cvQuery = await (from cv in _context.UserCVLink
                                     where cv.UserId == user.UserID
                                     select cv).ToListAsync();
                foreach (var item in cvQuery)
                {
                    item.Education = await _context.Education.Where(e => e.EducationID == item.EducationId).FirstOrDefaultAsync();
                    item.Location = await _context.Location.Where(l => l.LocationID == item.LocationId).FirstOrDefaultAsync();
                    item.Organization = await _context.Organization.Where(o => o.OrganizationID == item.OrganizationId).FirstOrDefaultAsync();
                    item.Publication = await _context.Publication.Where(p => p.PublicationID == item.PublicationId).FirstOrDefaultAsync();
                    item.Referenece = await _context.Reference.Where(r => r.ReferenceID == item.RefereneceId).FirstOrDefaultAsync();
                    item.Skill = await _context.Skill.Where(s => s.SkillID == item.SkillId).FirstOrDefaultAsync();
                }
                model.UserCvLinkList = cvQuery;
            }
            else
            {
                model.UserCvLink = new UserCVLink();
                model.UserCvLink.UserId = user.UserID;
                _context.UserCVLink.AddRange(model.UserCvLink);
                await _context.SaveChangesAsync();
                var cvQuery = await _context.UserCVLink.Where(l => l.UserId == user.UserID).ToListAsync();
                model.UserCvLinkList = cvQuery;
            }               
            return View(model);
        }

        // POST: DBUsers/Curriculum
        [HttpPost, ActionName("Curriculum")]
        [Authorize(Roles = "Student")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CurriculumChanged(string update, UserCVLink model)
        {
            var errors = ModelState.Values.SelectMany(v => v.Errors);           

            if (ModelState.IsValid)
            {
                try
                {
                    UserCVLink link = await _context.UserCVLink.Where(x => x.UserCVLinkID == model.UserCVLinkID).SingleOrDefaultAsync();

                    await UserCvLinkUpdateAsync(link, model);

                    return RedirectToAction("Curriculum");
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (! _context.UserCVLink.Any(l => l.UserCVLinkID == model.UserCVLinkID))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }                                             
            }
            UserCvViewModel viewElements = new UserCvViewModel();
            await InvalidCvModelAsync(viewElements, model);
            return View(viewElements);
        }

        private bool DBUserExists(int id)
        {
            return _context.DBUser.Any(e => e.UserID == id);
        }

        #region CVHelpers
        private void EducationUpdate(Education e, UserCVLink model)
        {
            e.DateEarned = model.Education.DateEarned;
            e.EducationDescription = model.Education.EducationDescription;
            e.EducationTitle = model.Education.EducationTitle;
            e.EducationType = model.Education.EducationType;
        }

        private void LocationUpdate(Location l, UserCVLink model)
        {
            l.EndDate = model.Location.EndDate;
            l.StartDate = model.Location.StartDate;
            l.City = model.Location.City;
            l.StateOrProvince = model.Location.StateOrProvince;
            l.Country = model.Location.Country;
        }

        private void OrganizationUpdate(Organization o, UserCVLink model)
        {
            o.EndDate = model.Organization.EndDate;
            o.StartDate = model.Organization.StartDate;
            o.OrganizationName = model.Organization.OrganizationName;
        }

        private void PublicationUpdate(Publication p, UserCVLink model)
        {
            p.PublicationDescription = model.Publication.PublicationDescription;
            p.PublicationLocation = model.Publication.PublicationLocation;
            p.PublicationTopic = model.Publication.PublicationTopic;
        }

        private void ReferenceUpdate(Reference r, UserCVLink model)
        {
            r.ReferenceDetail = model.Referenece.ReferenceDetail;
            r.ReferenceType = model.Referenece.ReferenceType;
        }

        private void SkillUpdate(Skill s, UserCVLink model)
        {
            s.SkillArea = model.Skill.SkillArea;
            s.EndDate = model.Skill.EndDate;
            s.SkillDescription = model.Skill.SkillDescription;
            s.SkillName = model.Skill.SkillName;
            s.StartDate = model.Skill.StartDate;
        }

        private async Task InvalidCvModelAsync(UserCvViewModel viewElements, UserCVLink model)
        {
            viewElements.UserCvLink = model;
            viewElements.UserCvLinkList = await _context.UserCVLink.Where(x => x.UserId == model.UserId).ToListAsync();
            foreach (var item in viewElements.UserCvLinkList)
            {
                item.Education = await _context.Education.Where(e => e.EducationID == item.EducationId).FirstOrDefaultAsync();
                item.Location = await _context.Location.Where(l => l.LocationID == item.LocationId).FirstOrDefaultAsync();
                item.Organization = await _context.Organization.Where(o => o.OrganizationID == item.OrganizationId).FirstOrDefaultAsync();
                item.Publication = await _context.Publication.Where(p => p.PublicationID == item.PublicationId).FirstOrDefaultAsync();
                item.Referenece = await _context.Reference.Where(r => r.ReferenceID == item.RefereneceId).FirstOrDefaultAsync();
                item.Skill = await _context.Skill.Where(s => s.SkillID == item.SkillId).FirstOrDefaultAsync();
            }
            viewElements.User = await _context.DBUser.Where(u => u.UserID == model.UserId).FirstOrDefaultAsync();
        }

        private async Task UserCvLinkUpdateAsync(UserCVLink link, UserCVLink model)
        {
            Education edu = await _context.Education.Where(e => e.EducationID == link.EducationId).SingleOrDefaultAsync();
            EducationUpdate(edu, model);
            _context.Education.Update(edu);

            Location loc = await _context.Location.Where(l => l.LocationID == link.LocationId).SingleOrDefaultAsync();
            LocationUpdate(loc, model);
            _context.Location.Update(loc);

            Organization org = await _context.Organization.Where(o => o.OrganizationID == link.OrganizationId).SingleOrDefaultAsync();
            OrganizationUpdate(org, model);
            _context.Organization.Update(org);

            Publication pub = await _context.Publication.Where(p => p.PublicationID == link.PublicationId).SingleOrDefaultAsync();
            PublicationUpdate(pub, model);
            _context.Publication.Update(pub);

            Reference reff = await _context.Reference.Where(r => r.ReferenceID == link.RefereneceId).SingleOrDefaultAsync();
            ReferenceUpdate(reff, model);
            _context.Reference.Update(reff);

            Skill sk = await _context.Skill.Where(s => s.SkillID == link.SkillId).SingleOrDefaultAsync();
            SkillUpdate(sk, model);
            _context.Skill.Update(sk);

            await _context.SaveChangesAsync();              
        }

        private async Task RemoveCvComponentsAsync(List<UserCVLink> links)
        {
            foreach(var link in links)
            {
                if (link.EducationId != null)
                {
                    var education = await _context.Education.SingleOrDefaultAsync(e => e.EducationID == link.EducationId);
                    if (education != null) _context.Education.Remove(education);
                }
                if (link.LocationId != null)
                {
                    var location = await _context.Location.SingleOrDefaultAsync(l => l.LocationID == link.LocationId);
                    if (location != null) _context.Location.Remove(location);
                }
                if (link.OrganizationId != null)
                {
                    var organization = await _context.Organization.SingleOrDefaultAsync(o => o.OrganizationID == link.OrganizationId);
                    if (organization != null) _context.Organization.Remove(organization);
                }
                if (link.PublicationId != null)
                {
                    var publication = await _context.Publication.SingleOrDefaultAsync(p => p.PublicationID == link.PublicationId);
                    if (publication != null) _context.Publication.Remove(publication);
                }
                if (link.RefereneceId!= null)
                {
                    var reference = await _context.Reference.SingleOrDefaultAsync(r => r.ReferenceID == link.RefereneceId);
                    if (reference != null) _context.Reference.Remove(reference);
                }
                if (link.SkillId != null)
                {
                    var skill = await _context.Skill.SingleOrDefaultAsync(s => s.SkillID == link.SkillId);
                    if (skill != null) _context.Skill.Remove(skill);
                }
               await  _context.SaveChangesAsync();
            }
            
        }
        #endregion
    }
}
