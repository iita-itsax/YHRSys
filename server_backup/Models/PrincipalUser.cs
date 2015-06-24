using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.Owin.Security;

namespace YHRSys.Models
{
    public class PrincipalUser
    {
        private ApplicationDbContext db = new ApplicationDbContext();
        public PrincipalUser(){
        }
        public ApplicationUser GetUser()
        {
            var manager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(new ApplicationDbContext()));
            string currentUserId = HttpContext.Current.User.Identity.GetUserId();

            //var currentUser = manager.FindById(currentUserId);
             //= db.Users.FirstOrDefault(x => x.Id == currentUserId);
            return manager.FindById(currentUserId);
        }
    }
}