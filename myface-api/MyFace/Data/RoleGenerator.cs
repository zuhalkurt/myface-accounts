using System;
using MyFace.Models.Database;

namespace MyFace.Data
{
    public static class RoleGenerator
    {

        enum Roles { Admin,
                    Member}
        
        public static string GetRandomRole()
        {
           string[] roles = Enum.GetNames(typeof (Roles));

            Random random = new Random();

            int randomEnum = random.Next(roles.Length);

            var ret = Enum.Parse(typeof (Roles), roles[randomEnum]);
            return ret.ToString();
    
    
        }     
    
    }
}