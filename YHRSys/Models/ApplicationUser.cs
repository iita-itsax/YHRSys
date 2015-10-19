using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.AspNet.Identity;
using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations.Schema;

using System.Data.Entity;
using System.Security.Claims;
using System.Threading.Tasks;
using System;

namespace YHRSys.Models
{
    public class ApplicationUser : IdentityUser
    {
        public ApplicationUser()
            : base()
        {
            this.Groups = new HashSet<ApplicationUserGroup>();
        }

        public async Task<ClaimsIdentity> GenerateUserIdentityAsync(UserManager<ApplicationUser> manager)
        {
            // Note the authenticationType must match the one defined in CookieAuthenticationOptions.AuthenticationType
            var userIdentity = await manager.CreateIdentityAsync(this, DefaultAuthenticationTypes.ApplicationCookie);
            // Add custom user claims here
            return userIdentity;
        }

        private const bool DEFAULT_VALUE = false;

        [Required]
        public string FirstName { get; set; }

        [Required]
        public string LastName { get; set; }

        [Required]
        public string Email { get; set; }

        public virtual ICollection<ApplicationUserGroup> Groups { get; set; }

        public string Token { get; set; }

        [DefaultValue(DEFAULT_VALUE)]
        [Required]
        public bool TokenExpired { get; set; }

        [DisplayName("Partner")]
        public Nullable<int> partnerId { get; set; }

        [ForeignKey("partnerId")]
        public virtual Partner partner { get; set; }

        [NotMapped]
        public string FullName
        {
            get
            {
                return FirstName + " " + LastName;
            }
        }
    }
}