namespace WebApplication23.Migrations
{
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Migrations;
    using System.Linq;
    using WebApplication23.Models;
    using WebApplication23.Models.Classes;
    using System.Collections.Generic;
    using Microsoft.AspNet.Identity;
    using Microsoft.AspNet.Identity.EntityFramework;

    internal sealed class Configuration : DbMigrationsConfiguration<WebApplication23.Models.ApplicationDbContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = false;
        }

        protected override void Seed(WebApplication23.Models.ApplicationDbContext context)
        {
            //  This method will be called after migrating to the latest version.

            //  You can use the DbSet<T>.AddOrUpdate() helper extension method 
            //  to avoid creating duplicate seed data. E.g.
            //
            //    context.People.AddOrUpdate(
            //      p => p.FullName,
            //      new Person { FullName = "Andrew Peters" },
            //      new Person { FullName = "Brice Lambson" },
            //      new Person { FullName = "Rowan Miller" }
            //    );
            //

            RoleStore<IdentityRole> roleStore = new RoleStore<IdentityRole>(context);
            RoleManager<IdentityRole> roleManager = new RoleManager<IdentityRole>(roleStore);

            string[] roleNames = new[] { "Teacher", "Student" };
            foreach (string roleName in roleNames)
            {
                if (!context.Roles.Any(r => r.Name == roleName))
                {
                    IdentityRole role = new IdentityRole { Name = roleName };
                    IdentityResult result = roleManager.Create(role);
                    if (!result.Succeeded)
                    {
                        throw new Exception(string.Join("\n", result.Errors));
                    }
                }
            }
            UserStore<ApplicationUser> userStore = new UserStore<ApplicationUser>(context);
            UserManager<ApplicationUser> userManager = new UserManager<ApplicationUser>(userStore);
            string[] emails = new[] { "john@lexicon.se", "teacher@lexicon.se", "bob@lexicon.se", "hans@lexicon.se", "olle@lexicon.se" };
            string[] firstName = new[] { "John", "Teacher", "Bob", "Hans", "Olle" };
            string[] lastName = new[] { "Hellman", "Lexicon", "Bobsson", "Andersen", "Oren" };

            int i = 0;
            foreach (string email in emails)
            {
                if (!context.Users.Any(u => u.UserName == email))
                {
                    ApplicationUser user = new ApplicationUser { UserName = email, Email = email, FirstName = firstName[i], LastName = lastName[i], TimeOfRegistration = DateTime.Now };
                    var result = userManager.Create(user, "password");
                    if (!result.Succeeded)
                    {
                        throw new Exception(string.Join("\n", result.Errors));
                    }
                }
                i++;
            }

            ApplicationUser Teacher = userManager.FindByName("john@lexicon.se");
            userManager.AddToRole(Teacher.Id, "Teacher");

            Teacher = userManager.FindByName("teacher@lexicon.se");
            userManager.AddToRole(Teacher.Id, "Teacher");

            foreach (ApplicationUser user in userManager.Users.ToList().Where(u => (u.Email != "admin@admin.ad" && u.Email != "admin@Gymbokning.se")))
            {
                userManager.AddToRole(user.Id, "Student");
            }

            context.SaveChanges();

            CourseClass[] course = new CourseClass[] {
                new CourseClass
                {
                    Name = ".NET Höst 2015",
                    Description = "En superrolig kurs som passa alla mellan 10 - 100 år. I kursen ingår moduler som 'Databasdesign', 'AngularJS'.",
                    Duration = new TimeSpan(2, 60, 0),
                    StartTime = new DateTime(2015, 09, 11),
                    AttendingStudents = new List<ApplicationUser>()
                },

                 new CourseClass
                {
                    Name = "Ny programmeringskurs 2019",
                    Description = "Vi lär dig allt om programmering.",
                    Duration = new TimeSpan(12, 30, 23),
                    StartTime = new DateTime(2019, 02, 06),
                    AttendingStudents = new List<ApplicationUser>()
                },

                 new CourseClass
                {
                    Name = "Shoppingkurs distans 2017",
                    Description = "Handla fina saker online.",
                    Duration = new TimeSpan(21, 30, 23),
                    StartTime = new DateTime(2017, 02, 06),
                    AttendingStudents = new List<ApplicationUser>()
                }
            };
            foreach (CourseClass g in course)
            {
                context.CourseClasses.Add(g);
            }
        }
    }
}
