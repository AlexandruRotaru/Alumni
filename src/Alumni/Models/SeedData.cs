using Alumni.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Alumni.Models
{
    public class SeedData
    {
        public static void Initialize(IServiceProvider serviceProvider)
        {
            using (var context = new ApplicationDbContext(
                serviceProvider.GetRequiredService<DbContextOptions<ApplicationDbContext>>()))
            {
                if(context.Roles.Any())
                {
                    return;
                }

                context.Roles.AddRange(
                    new Microsoft.AspNetCore.Identity.EntityFrameworkCore.IdentityRole
                    {
                        Name = "Admin",
                        NormalizedName = "ADMIN",
                    },

                    new Microsoft.AspNetCore.Identity.EntityFrameworkCore.IdentityRole
                    {
                        Name = "Profesor",
                        NormalizedName = "PROFESOR",
                    },

                    new Microsoft.AspNetCore.Identity.EntityFrameworkCore.IdentityRole
                    {
                        Name = "Student",
                        NormalizedName = "STUDENT",
                    });

                context.SaveChanges();

                var firstUser = (from user in context.Users
                                 select user.Id).First();

                var roleQuery = (from adm in context.Roles
                                  where adm.Name == "Admin"
                                  select adm.Id).First();

                context.UserRoles.AddRange(
                    new Microsoft.AspNetCore.Identity.EntityFrameworkCore.IdentityUserRole<string>
                    {
                        UserId = firstUser,
                        RoleId = roleQuery,
                    });

                context.SaveChanges();
            }
        }

        public static void InitializeData(IServiceProvider serviceProvider)
        {
            using (var context = new AlumniDataContext(
                serviceProvider.GetRequiredService<DbContextOptions<AlumniDataContext>>()))
            {
                if(context.DBDegree.Any())
                {
                    return;
                }

                context.DBDegree.AddRange(
                    new DBDegree
                    {
                        DegreeName = "Automatica",
                        DegreeType = "Licenta",
                    },

                    new DBDegree
                    {
                        DegreeName = "Automatica",
                        DegreeType = "Master",
                    },

                    new DBDegree
                    {
                        DegreeName = "Automatica",
                        DegreeType = "Doctorat",
                    },

                    new DBDegree
                    {
                        DegreeName = "Calculatoare",
                        DegreeType = "Licenta",
                    },

                    new DBDegree
                    {
                        DegreeName = "Calculatoare",
                        DegreeType = "Master",
                    },

                    new DBDegree
                    {
                        DegreeName = "Calculatoare",
                        DegreeType = "Doctorat",
                    });

                context.SaveChanges();
            }
        }
    }
}
