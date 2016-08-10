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

        public async Task<IActionResult> Index(string searchString)
        {
            List<DBUser> userss = new List<DBUser>();
            //HttpContext.Session.SetString("SuccessfulSearch", "No");
            var users = from u in _context.DBUser
                        select u;
            if(!string.IsNullOrWhiteSpace(searchString))
            {
                 users = users.Where(us => us.fName == searchString || us.fName.Contains(searchString) ||
                                           us.lName == searchString || us.lName.Contains(searchString)
                                    );
                userss = await users.ToListAsync();
                foreach (var user in users)
                {
                    var us = await _userManager.FindByIdAsync(user.AspNetUser);
                    if (await _userManager.IsInRoleAsync(us, "Admin") || await _userManager.IsInRoleAsync(us, "Profesor"))
                    {
                        userss.Remove(user);
                    }

                }
                if (users.Any()) HttpContext.Session.SetString("SuccessfulSearch", "Yes");
                else HttpContext.Session.SetString("SuccessfulSearch", "No");
            }
            else HttpContext.Session.SetString("SuccessfulSearch", "Nothing");
            return View(userss);
        }
        public ActionResult Chat()
        {
            return View();
        }

        public IActionResult About()
        {
            ViewData["Message"] = "Your application description page.";

            return View();
        }

        public IActionResult Contact()
        {
            ViewData["Message"] = "Your contact page.";

            return View();
        }

        public IActionResult Error()
        {
            return View();
        }
    }
}
