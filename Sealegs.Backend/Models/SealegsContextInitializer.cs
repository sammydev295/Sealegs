using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Migrations;

using Sealegs.DataObjects;

namespace Sealegs.Backend.Models
{
    //DropCreateDatabaseIfModelChanges
    public class SealegsContextInitializer : DropCreateDatabaseIfModelChanges<SealegsContext>
    {
        public SealegsContextInitializer() : base()
        {
            //new SealegsUserRole();
        }
        protected override void Seed(SealegsContext context)
        {
            //List<SealegsUserRole> roles = new List<SealegsUserRole>
            //{
            //    new SealegsUserRole { Id = SealegsUserRole.AdminRole, RoleName = "Admin", Description = "System Administrator" },
            //    new SealegsUserRole { Id = SealegsUserRole.BartenderRole, RoleName = "Bartender", Description = "Bartender" },
            //    new SealegsUserRole { Id = SealegsUserRole.CustomerRole, RoleName = "Customer", Description = "Customer" },
            //    new SealegsUserRole { Id = SealegsUserRole.ManagerRole, RoleName = "Manager", Description = "Manager" },
            //    new SealegsUserRole { Id = SealegsUserRole.ServerRole, RoleName = "Server", Description = "Server" }
            //};
            //roles.ForEach(r => context.Set<SealegsUserRole>().Add(r));


            //List<SealegsUser> users = new List<SealegsUser>
            //{
            //    new SealegsUser { Id = "410B039A-467B-460C-9A42-6531405B607A", Email = "sammy.dev@drdev.com", FirstName = "Sammy", LastName = "Dev", Role = roles.Find(r => r.Id == "C5AEB9A8-5E2F-475B-910E-A0DD068D38C7"), Password = "ns+lsiAelIHT6eMFP+CS48X0YuG740=", PasswordFormat = 1, PasswordSalt = "hqbg7V8o2w0NDFpMSePM0g==", LastLoginDate = DateTime.Parse("2016-01-04 08:38:52.610"), IsAnnonymous = false, IsApproved = true },
            //    new SealegsUser { Id = "E4B13349-5231-426F-9F46-30DBB325972B", Email = "omar@sealegswine.com", FirstName = "Omar", LastName = "Khashen", Role = roles.Find(r => r.Id == "36A301B4-6F66-4C44-8E19-65B39CE2544C"), Password = "gOqor5kbk4ajf3AuEg5El+uK6y8=", PasswordFormat = 1, PasswordSalt = "PA0zjgpWsy7IKoqEvm/76A==", LastLoginDate = DateTime.Parse("2014-10-08 03:39:29.063"), IsAnnonymous = false, IsApproved = true },
            //    new SealegsUser { Id = "15286BB6-714B-42B1-892A-606F9C1CABD0", Email = "gus.calderon@drdev.com", FirstName = "Gus", LastName = "Calderon", Role = roles.Find(r => r.Id == "934B07D1-71FC-4528-B880-11EEFFF4B4DF"), Password = "QxScC6lH0YvyK7wYcjpoDKViKiY=", PasswordFormat = 1, PasswordSalt = "so4xVv6j8BizWvSQqleQ1A==", LastLoginDate = DateTime.Parse("2014-09-30 18:35:39.000"), IsAnnonymous = false, IsApproved = true },
            //    new SealegsUser { Id = "EA80F481-BAA0-422B-A1C4-AE37DFF7C2FC", Email = "alicia.whitney@sealegswine.com", FirstName = "Alicia", LastName = "Whitney", Role = roles.Find(r => r.Id == "C5AEB9A8-5E2F-475B-910E-A0DD068D38C7"), Password = "ns+jCSnXTMVAUXx0JQungu/yCtk=", PasswordFormat = 1, PasswordSalt = "PGVoSKdYD1AXia/jt5V5Kw==", LastLoginDate = DateTime.Parse("2014-09-22 05:12:34.197"), IsAnnonymous = false, IsApproved = true }
            //};
            //users.ForEach(u => context.Set<SealegsUser>().Add(u));

            base.Seed(context);
        }
    }
}