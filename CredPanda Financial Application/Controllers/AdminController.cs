using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace CredPanda_Financial_Application.Controllers
{
    [Authorize(Roles = "Administrator")]
    public class AdminController : Controller
    {
        private readonly RoleManager<IdentityRole> roleManager;
        
        public AdminController(RoleManager<IdentityRole> roleManager)
        {
            this.roleManager = roleManager;
            
        }

        public IActionResult Index()
        {
            return View();
        }
        public IActionResult Create()
        {
            return View();
        }
        [HttpPost]

      
        public async Task<IActionResult> Create(CredPanda_Financial_Application.Models.DatabaseRole role)
        {
            var roleExist = await roleManager.RoleExistsAsync(role.roleName);
            if(!roleExist)
            {
                var result = await roleManager.CreateAsync(new IdentityRole(role.roleName));
            }
            return View();
        }
        
    }
}
