using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;

namespace YHRSys.Models
{
    public class GenerateCustomPasswordResetToken
    {
        public static string GenerateRandomToken(string UserName, int length = 10)
        {
            Random random = new Random();
            string token;
            StringBuilder sbuilder = new StringBuilder();
            for (int x = 0; x < length; ++x)
            {
                sbuilder.Append((char)random.Next(33, 126));
            }
            token = sbuilder.ToString();
            ApplicationDbContext dbContext = new ApplicationDbContext();

            var user = dbContext.Users.First(u => u.UserName == UserName);
            user.Token = token;
            user.TokenExpired = false;
            dbContext.Entry(user).State = System.Data.Entity.EntityState.Modified;
            dbContext.SaveChanges();
            return token;
        }

        public static ApplicationUser ResetOldPassword(string ReturnToken)
        {
            ApplicationDbContext dbContext = new ApplicationDbContext();

            try {
                var user = dbContext.Users.First(u => u.Token == ReturnToken);
                return user;
            }
            catch(Exception ex){
                return null;
            }
        }
    }
}