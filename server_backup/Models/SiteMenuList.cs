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
    
    public class SiteMenuList
    {
        private ApplicationDbContext db = new ApplicationDbContext();
        public List<string> menuList() { 
            //Display sitecontents list
            var menu = (from r in db.SiteContents select r.caption).ToList();

            return menu;
        }
    }
}