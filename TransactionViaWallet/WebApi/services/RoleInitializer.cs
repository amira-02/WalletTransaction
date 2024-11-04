//using Microsoft.AspNetCore.Identity;
//using System.Threading.Tasks;

//namespace WebApi.services  // Assurez-vous que le namespace est correct
//{
//    public class RoleInitializer : IRoleInitializer
//    {
//        private readonly RoleManager<IdentityRole> _roleManager;

//        public RoleInitializer(RoleManager<IdentityRole> roleManager)
//        {
//            _roleManager = roleManager;
//        }

//        public async Task InitializeRoles()
//        {
//            string[] roleNames = { "User", "Admin" };
//            foreach (var roleName in roleNames)
//            {
//                var roleExist = await _roleManager.RoleExistsAsync(roleName);
//                if (!roleExist)
//                {
//                    await _roleManager.CreateAsync(new IdentityRole(roleName));
//                }
//            }
//        }
//    }
//}
