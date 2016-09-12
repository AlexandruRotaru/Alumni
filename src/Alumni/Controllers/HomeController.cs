using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Alumni.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using System.Collections;
using Alumni.Models.UserViewModels;

namespace Alumni.Controllers
{
    [Authorize]
    public class HomeController : Controller
    {
        private readonly AlumniDataContext _context;
        private readonly UserManager<ApplicationUser> _userManager;

        public HomeController(AlumniDataContext context, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        public IActionResult Index()
        {
            return View();
        }

        public async Task<ActionResult> Search(string searchString, SearchViewModel model)
        {
            List<DBUser> userss = new List<DBUser>();
            //HttpContext.Session.SetString("SuccessfulSearch", "No"); 
            var users = from u in _context.DBUser
                        select u;
            if (!string.IsNullOrWhiteSpace(searchString))
            {
                if (model.isAdvancedSearch && (model.isEducation || model.isLocation || model.isOrganization || model.isPublication || model.isReference || model.isSkill))
                {
                    Dictionary<bool, List<DBUser>> userDictionary = new Dictionary<bool,List<DBUser>>();
                    List<UserCVLink> linkList = new List<UserCVLink>();
                    List<DBUser> userList = new List<DBUser>();

                    //Location
                    var countryQuery = await _context.Location.Where(s => s.City.Contains(searchString) || s.Country.Contains(searchString)).Select(s => s.LocationID).ToListAsync();
                    if (countryQuery.Any())
                    {
                        foreach (var skill in countryQuery)
                        {
                            var link = await _context.UserCVLink.Where(l => l.LocationId == skill).FirstOrDefaultAsync();
                            if (link != null) linkList.Add(link);

                        }
                    }


                    if (linkList.Any())
                    {
                        foreach (var l in linkList)
                        {
                           var iusers  =  await _context.DBUser.Where(u => u.UserID == l.UserId).FirstAsync();
                           userList.Add(iusers);
                        }
                        
                        if (userList != null) userDictionary.Add(model.isLocation, userList.Distinct().ToList());
                    }                    

                    //Skill
                    linkList.Clear();
                    userList.Clear();
                    var skillQuery = await _context.Skill.Where(s => s.SkillArea.Contains(searchString) || s.SkillDescription.Contains(searchString) ||
                                                                     s.SkillName.Contains(searchString)).Select(s => s.SkillID).ToListAsync();
                    if (skillQuery.Any())
                    {
                        foreach (var skill in skillQuery)
                        {
                            var link = await _context.UserCVLink.Where(l => l.SkillId == skill).FirstOrDefaultAsync();
                            if (link != null) linkList.Add(link);

                        }
                    }


                    if (linkList.Any())
                    {
                        foreach (var l in linkList)
                        {
                            var use = await _context.DBUser.Where(u => u.UserID == l.UserId).FirstAsync();
                            userList.Add(use);
                        }
                        if (userList != null) userDictionary.Add(model.isSkill, userList.Distinct().ToList());
                    }

                    //Education
                    linkList.Clear();
                    userList.Clear();
                    var educationlQuery = await _context.Education.Where(e => e.EducationDescription.Contains(searchString) || e.EducationTitle.Contains(searchString) ||
                                                                    e.EducationType.Contains(searchString) || e.GrantingInstitution.Contains(searchString)).Select(e => e.EducationID).ToListAsync();
                    if (educationlQuery.Any())
                    {
                        foreach (var education in educationlQuery)
                        {
                            var link = await _context.UserCVLink.Where(l => l.EducationId == education).FirstOrDefaultAsync();
                            if (link != null) linkList.Add(link);

                        }
                    }


                    if (linkList.Any())
                    {
                        foreach (var l in linkList)
                        {
                            var use = await _context.DBUser.Where(u => u.UserID == l.UserId).FirstAsync();
                            userList.Add(use);
                        }
                        if (userList != null) userDictionary.Add(model.isEducation, userList.Distinct().ToList());
                    }

                    //Reference
                    linkList.Clear();
                    userList.Clear();
                    var referenceQuery = await _context.Reference.Where(r => r.ReferenceDetail.Contains(searchString) || r.ReferenceType.Contains(searchString)).Select(r => r.ReferenceID).ToListAsync();
                    if (referenceQuery.Any())
                    {
                        foreach (var reference in referenceQuery)
                        {
                            var link = await _context.UserCVLink.Where(l => l.RefereneceId == reference).FirstOrDefaultAsync();
                            if (link != null) linkList.Add(link);

                        }
                    }


                    if (linkList.Any())
                    {
                        foreach (var l in linkList)
                        {
                            var use = await _context.DBUser.Where(u => u.UserID == l.UserId).FirstAsync();
                            userList.Add(use);
                        }
                        if (userList != null) userDictionary.Add(model.isReference, userList.Distinct().ToList());
                    }


                    //Organzation
                    linkList.Clear();
                    userList.Clear();
                    var organizationQuery = await _context.Organization.Where(o => o.OrganizationName.Contains(searchString)).Select(o => o.OrganizationID).ToListAsync();
                    if (organizationQuery.Any())
                    {
                        foreach (var organization in organizationQuery)
                        {
                            var link = await _context.UserCVLink.Where(l => l.OrganizationId == organization).FirstOrDefaultAsync();
                            if (link != null) linkList.Add(link);

                        }
                    }


                    if (linkList.Any())
                    {
                        foreach (var l in linkList)
                        {
                            var use = await _context.DBUser.Where(u => u.UserID == l.UserId).FirstAsync();
                            userList.Add(use);
                        }
                        if (userList != null) userDictionary.Add(model.isOrganization, userList.Distinct().ToList());
                    }

                    //Publication
                    linkList.Clear();
                    userList.Clear();
                    var publicationQuery = await _context.Publication.Where(p => p.PublicationDescription.Contains(searchString) || p.PublicationTopic.Contains(searchString)).Select(p => p.PublicationID).ToListAsync();
                    if (publicationQuery.Any())
                    {
                        foreach (var publication in publicationQuery)
                        {
                            var link = await _context.UserCVLink.Where(l => l.OrganizationId == publication).FirstOrDefaultAsync();
                            if (link != null) linkList.Add(link);

                        }
                    }


                    if (linkList.Any())
                    {
                        foreach (var l in linkList)
                        {
                            var use = await _context.DBUser.Where(u => u.UserID == l.UserId).FirstAsync();
                            userList.Add(use);
                        }
                        if (userList != null) userDictionary.Add(model.isPublication, userList.Distinct().ToList());
                    }

                    foreach (KeyValuePair<bool, List<DBUser>> us in userDictionary)
                    {
                        if (us.Key != false)
                        {
                            userss = us.Value;
                        }
                    }

                }
                else
                {
                    
                    users = users.Where(us => us.fName == searchString || us.fName.Contains(searchString) ||
                                                              us.lName == searchString || us.lName.Contains(searchString)
                                                       );
                    userss = await users.Distinct().ToListAsync();
                    
                }

                foreach (var user in users)
                {
                    var us = await _userManager.FindByIdAsync(user.AspNetUser);
                    if (await _userManager.IsInRoleAsync(us, "Admin") || await _userManager.IsInRoleAsync(us, "Profesor"))
                    {
                        userss.Remove(user);
                    }
                }

                if (userss.Any()) HttpContext.Session.SetString("SuccessfulSearch", "Yes");
                else HttpContext.Session.SetString("SuccessfulSearch", "No");
            }
            else HttpContext.Session.SetString("SuccessfulSearch", "Nothing");
            model.usersList = userss;

            return View(model);
        }

        public IActionResult Chat()
        {
            return View();
        }

        public IActionResult About()
        {
            ViewData["Message"] = "Descrierea Aplicatiei.";

            return View();
        }

        public IActionResult Contact()
        {
            ViewData["Message"] = "Pagina de Contact.";

            return View();
        }

        public IActionResult Error()
        {
            return View();
        }
    }
}
