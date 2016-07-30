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
using Microsoft.AspNetCore.Http;

namespace Alumni.Controllers
{
    [Authorize]
    public class ManageCvController : Controller
    {
        private readonly AlumniDataContext _context;
        private readonly UserManager<ApplicationUser> _userManager;

        public ManageCvController(AlumniDataContext context, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        // GET: ManageCv
        public async Task<IActionResult> Index()
        {
            var userID = _userManager.GetUserId(HttpContext.User);            
            var currentUser = await _context.DBUser.SingleAsync(u => u.AspNetUser == userID);            
            var alumniDataContext = _context.UserCVLink.Include(u => u.Education).Include(u => u.Location).Include(u => u.Organization).Include(u => u.Publication).Include(u => u.Referenece).Include(u => u.Skill).Include(u => u.User).Where(u => u.UserId == currentUser.UserID);
            return View(await alumniDataContext.ToListAsync());
        }

        // GET: ManageCv/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var userCVLink = await _context.UserCVLink.SingleOrDefaultAsync(m => m.UserCVLinkID == id);
            if (userCVLink == null)
            {
                return NotFound();
            }

            return RedirectToAction("Curriculum", "DBUsers", new { id = userCVLink.UserId });
            //return View(userCVLink);
        }

        // GET: ManageCv/Create
        public async Task<IActionResult> Create()
        {            
            UserCVLink link = new UserCVLink();
            var userID = _userManager.GetUserId(HttpContext.User);
            link.User = await _context.DBUser.Where(u => u.AspNetUser == userID).FirstOrDefaultAsync();
            link.UserId = link.User.UserID;
            
            return View(link);
        }

        // POST: ManageCv/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Education,Location,Organization,Publication,Skill,Referenece")] UserCVLink userCVLink)
        {
            var errors = ModelState.Values.SelectMany(v => v.Errors);
            if (ModelState.IsValid)
            {
                await AddCvElementsAsync(userCVLink);

                _context.Add(userCVLink);
                await _context.SaveChangesAsync();
                return RedirectToAction("Index", "Home");
            }
            return View(userCVLink);
        }

        // GET: ManageCv/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var userCVLink = await _context.UserCVLink.SingleOrDefaultAsync(m => m.UserCVLinkID == id);
            if (userCVLink == null)
            {
                return NotFound();
            }
            return RedirectToAction("Curriculum", "DBUsers", new {id = userCVLink.UserId });
        }

        // POST: ManageCv/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("UserCVLinkID,EducationId,LocationId,OrganizationId,PublicationId,RefereneceId,SkillId,UserId")] UserCVLink userCVLink)
        {
            if (id != userCVLink.UserCVLinkID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(userCVLink);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!UserCVLinkExists(userCVLink.UserCVLinkID))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction("Index");
            }
            ViewData["EducationId"] = new SelectList(_context.Education, "EducationID", "Education", userCVLink.EducationId);
            ViewData["LocationId"] = new SelectList(_context.Location, "LocationID", "Location", userCVLink.LocationId);
            ViewData["OrganizationId"] = new SelectList(_context.Organization, "OrganizationID", "Organization", userCVLink.OrganizationId);
            ViewData["PublicationId"] = new SelectList(_context.Publication, "PublicationID", "Publication", userCVLink.PublicationId);
            ViewData["RefereneceId"] = new SelectList(_context.Reference, "ReferenceID", "Referenece", userCVLink.RefereneceId);
            ViewData["SkillId"] = new SelectList(_context.Skill, "SkillID", "Skill", userCVLink.SkillId);
            ViewData["UserId"] = new SelectList(_context.DBUser, "UserID", "User", userCVLink.UserId);
            return View(userCVLink);
        }

        // GET: ManageCv/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var userCVLink = await _context.UserCVLink.SingleOrDefaultAsync(m => m.UserCVLinkID == id);
            if (userCVLink == null)
            {
                return NotFound();
            }

            return View(userCVLink);
        }

        // POST: ManageCv/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var userCVLink = await _context.UserCVLink.SingleOrDefaultAsync(m => m.UserCVLinkID == id);

            if(userCVLink.EducationId != null)
            {
                var education = await _context.Education.SingleOrDefaultAsync(e => e.EducationID == userCVLink.EducationId);
                if (education != null) _context.Education.Remove(education);
            }
            if(userCVLink.LocationId != null)
            {
                var location = await _context.Location.SingleOrDefaultAsync(l => l.LocationID == userCVLink.LocationId);
                if (location != null) _context.Location.Remove(location);
            }
            if(userCVLink.OrganizationId != null)
            {
                var organization = await _context.Organization.SingleOrDefaultAsync(o => o.OrganizationID == userCVLink.OrganizationId);
                if (organization != null) _context.Organization.Remove(organization);
            }
            if(userCVLink.PublicationId != null)
            {
                var publication = await _context.Publication.SingleOrDefaultAsync(p => p.PublicationID == userCVLink.PublicationId);
                if (publication != null) _context.Publication.Remove(publication);
            }
            if(userCVLink.RefereneceId != null)
            {
                var reference = await _context.Reference.SingleOrDefaultAsync(r => r.ReferenceID == userCVLink.RefereneceId);
                if (reference != null) _context.Reference.Remove(reference);
            }
            if(userCVLink.SkillId != null)
            {
                var skill = await _context.Skill.SingleOrDefaultAsync(s => s.SkillID == userCVLink.SkillId);
                if (skill != null) _context.Skill.Remove(skill);
            }            
            await _context.SaveChangesAsync();

            _context.UserCVLink.Remove(userCVLink);
            await _context.SaveChangesAsync();
            return RedirectToAction("Index");
        }

        private bool UserCVLinkExists(int id)
        {
            return _context.UserCVLink.Any(e => e.UserCVLinkID == id);
        }

        private async Task AddCvElementsAsync(UserCVLink userCVLink)
        {
            _context.Education.AddRange(
                    new Education
                    {
                        DateEarned = userCVLink.Education.DateEarned,
                        EducationDescription = userCVLink.Education.EducationDescription,
                        EducationTitle = userCVLink.Education.EducationTitle,
                        EducationType = userCVLink.Education.EducationType,
                        GrantingInstitution = userCVLink.Education.GrantingInstitution
                    });
            _context.Location.AddRange(
                new Location
                {
                    City = userCVLink.Location.City,
                    Country = userCVLink.Location.Country,
                    StateOrProvince = userCVLink.Location.StateOrProvince,
                    StartDate = userCVLink.Location.StartDate,
                    EndDate = userCVLink.Location.EndDate
                });
            _context.Organization.AddRange(
                new Organization
                {
                    StartDate = userCVLink.Organization.StartDate,
                    EndDate = userCVLink.Organization.EndDate,
                    OrganizationName = userCVLink.Organization.OrganizationName
                });
            _context.Publication.AddRange(
                new Publication
                {
                    PublicationDescription = userCVLink.Publication.PublicationDescription,
                    PublicationLocation = userCVLink.Publication.PublicationLocation,
                    PublicationTopic = userCVLink.Publication.PublicationTopic
                });
            _context.Reference.AddRange(
                new Reference
                {
                    ReferenceDetail = userCVLink.Referenece.ReferenceDetail,
                    ReferenceType = userCVLink.Referenece.ReferenceType
                });
            _context.Skill.AddRange(
                new Skill
                {
                    StartDate = userCVLink.Skill.StartDate,
                    EndDate = userCVLink.Skill.EndDate,
                    SkillArea = userCVLink.Skill.SkillArea,
                    SkillDescription = userCVLink.Skill.SkillDescription,
                    SkillName = userCVLink.Skill.SkillName
                });
            await _context.SaveChangesAsync();
        }
    }
}
