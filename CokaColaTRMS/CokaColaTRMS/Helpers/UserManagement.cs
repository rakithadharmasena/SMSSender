using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CokaColaTRMS.Helpers
{
   public class UserManagement
    {
       public static bool IsUserValid(string username, string password)
       {
           try
           {
               using (CokeTRMSEntities context = new CokeTRMSEntities())
               {
                   string hashedPw = CryptoHelper.GetMD5Hash(password);

                   if (context.Users.Where(u => u.Username.Equals(username, StringComparison.OrdinalIgnoreCase) &&
                       u.Password.Equals(hashedPw, StringComparison.OrdinalIgnoreCase)).SingleOrDefault() != null)
                   {
                       return true;
                   }
                   else
                   {
                       return false;
                   }
               }
           }
           catch (Exception ex)
           {
               return false;
           }
       }
    }
}
